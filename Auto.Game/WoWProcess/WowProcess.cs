﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.ShareLib;
using Microsoft.Extensions.Options;

namespace Auto.Game
{
    public sealed class WowProcess
    {
        private static readonly string[] defaultProcessNames = [
        "Wow",
        "WowClassic",
        "WowClassicT",
        "Wow-64",
        "WowClassicB",
            "QQ"
        ];

        private readonly Thread thread;
        private readonly CancellationToken token;

        public Version FileVersion { get; private set; }

        public string Path { get; private set; }

        private Process process;

        private int id = -1;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                process = Process.GetProcessById(id);
            }
        }

        public string ProcessName => process.ProcessName;

        public IntPtr MainWindowHandle => process.MainWindowHandle;

        public bool IsRunning { get; private set; }

        private WowProcess(CancellationTokenSource cts, int pid = -1)
        {
            token = cts.Token;

            Process? p = Get(pid)
                ?? throw new NullReferenceException(
                    $"Unable to find {(pid == -1 ? "any" : $"pid={pid}")} " +
                    $"running World of Warcraft process!");

            process = p;
            id = process.Id;
            IsRunning = true;
            (Path, FileVersion) = GetProcessInfo();

            thread = new(PollProcessExited);
            thread.Start();
        }

        public WowProcess(CancellationTokenSource cts, IOptions<StartupConfigPid> options) : this(cts, options.Value.Id) { }

        private void PollProcessExited()
        {
            while (!token.IsCancellationRequested)
            {
                process.Refresh();
                if (process.HasExited)
                {
                    IsRunning = false;

                    Process? p = Get();
                    if (p != null)
                    {
                        process = p;
                        id = process.Id;
                        IsRunning = true;
                        (Path, FileVersion) = GetProcessInfo();
                    }
                }

                token.WaitHandle.WaitOne(5000);
            }
        }

        public static Process? Get(int processId = -1)
        {
            if (processId != -1)
            {
                return Process.GetProcessById(processId);
            }

            Process[] processList = Process.GetProcesses();
            for (int i = 0; i < processList.Length; i++)
            {
                Process p = processList[i];
                for (int j = 0; j < defaultProcessNames.Length; j++)
                {
                    if (defaultProcessNames[j].Contains(p.ProcessName, StringComparison.OrdinalIgnoreCase))
                    {
                        return p;
                    }
                }
            }

            return null;
        }

        private (string path, Version version) GetProcessInfo()
        {
            string path = WinAPI.ExecutablePath.Get(process);
            if (string.IsNullOrEmpty(path))
            {
                throw new NullReferenceException("Unable identify World of Warcraft process path!");
            }

            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(System.IO.Path.Join(path, process.ProcessName + ".exe"));
            if (Version.TryParse(fileVersion.FileVersion, out Version? v))
            {
                return (path, v);
            }

            return (path, new());
        }
    }
}
