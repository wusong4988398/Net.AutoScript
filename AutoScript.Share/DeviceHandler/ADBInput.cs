using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace AutoScript.Share
{
    public class ADBInput : IDeviceInput
    {
        public IDevice Device { get; set; }

        public ADBInput(IDevice device)
        {
            this.Device = device;
        }

        public List<string> ExecADBCommand(string cmd)
        {
            List<string> ret = new List<string>();
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = @"lib\adb.exe";
            p.StartInfo.Arguments = "-s " + Device.Port + " " + cmd;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            //string outtr = p.StandardOutput.ReadToEnd();
            StreamReader readerout = p.StandardOutput;
            while (!readerout.EndOfStream)
            {
                string line = readerout.ReadLine();
                ret.Add(line);
            }
            p.WaitForExit();
            p.Close();
            return ret;
        }

        public void 点击(ActionParam param)
        {
            if ((param.ActionType | ActionType.Click) != param.ActionType) return;
            string cmd = "";
            cmd += "shell input tap ";
            cmd += param.Point.x.ToString();
            cmd += " ";
            cmd += param.Point.y.ToString();
            ExecADBCommand(cmd);
        }

        ////long SendString(hwnd, str)
        public void 发送字符串(ActionParam param)
        {
            if ((param.ActionType | ActionType.SendStr) != param.ActionType) return;
            string cmd = "";
            cmd += "shell input tap ";
            cmd += param.Point.x.ToString();
            cmd += " ";
            cmd += param.Point.y.ToString();
            ExecADBCommand(cmd);
            Thread.Sleep(500);
            cmd = "shell input text '";
            cmd += param.SendStr + "'";
            ExecADBCommand(cmd);
        }
        ////long KeyPress(vk_code)
        public void 发送按键(ActionParam param)
        {
            if ((param.ActionType | ActionType.SendKey) != param.ActionType) return;
            string cmd = "";
            cmd += "shell input keyevent  ";
            cmd += param.KeyCode.ToString();
            ExecADBCommand(cmd);
        }
        public void 滑动(ActionParam param)
        {
            if ((param.ActionType | ActionType.Swipe) != param.ActionType) return;
            string cmd = "";
            cmd += "shell input swipe  ";
            cmd += param.Swipe.x1.ToString() + " ";
            cmd += param.Swipe.y1.ToString() + " ";
            cmd += param.Swipe.x2.ToString() + " ";
            cmd += param.Swipe.y2.ToString() + " ";
            cmd += param.delay.ToString();
            ExecADBCommand(cmd);
        }
        public void 启动APP()
        {
            string cmd = "shell am start " + Config.AppConfig.Config_GameWindow.PackageName;
            ExecADBCommand(cmd);
        }
        public void 退出APP()
        {
            string cmd = "shell am force-stop " + Config.AppConfig.Config_GameWindow.PackageName.Substring(0, Config.AppConfig.Config_GameWindow.PackageName.IndexOf("/"));
            ExecADBCommand(cmd);
        }
    }
}