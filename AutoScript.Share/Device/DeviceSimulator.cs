using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    public class DeviceSimulator : Device
    {
     
        public DeviceSimulator(int pid)
        {
            GetDeviceInfo(pid);
        }

        protected override void GetDeviceInfo(int pid)
        {
            //将内存进程列表和窗口进程列表排序,就得到对应关系
            //内存进程列表
            Process[] memProcesss = (from p in Process.GetProcessesByName(Config.AppConfig.Config_GameWindow.MemProcessName) orderby p.StartTime select p).ToArray();
            //窗口进程列表
            Process[] windowProcesses = (from p in Process.GetProcessesByName(Config.AppConfig.Config_GameWindow.WindowProcessName) orderby p.StartTime select p).ToArray();
            Process memProc = null, winProc = null;
            //查找指定的进程对象
            for (int i = 0; i < memProcesss.Length; i++)
            {
                if (memProcesss[i].Id == pid)
                {
                    memProc = memProcesss[i];
                    winProc = windowProcesses[i];
                    break;
                }
            }
            Pid = pid;
            Hwnd = API.FindChildWindow(winProc.MainWindowHandle,
                    Config.AppConfig.Config_GameWindow.WindowClassName,
                    Config.AppConfig.Config_GameWindow.WindowTitle
                    ).ToInt32();
            IP = Utility.GetLocalIP();
            Port = Utility.GetADBPort(pid);
            Title = winProc.MainWindowTitle;
            return;
        }
    }
}