using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    public abstract class Device : IDevice
    {
        private static List<IDevice> AllDevices = new List<IDevice>();


        public static List<IDevice> GetAllDevices()
        {
            return AllDevices;
        }
        /// <summary>
        /// 重新生成所有的设备对象
        /// </summary>
        /// <returns></returns>
        public static List<IDevice> Reload()
        {
            //清除设备列表
            AllDevices.Clear();
            //取得所有设备进程
            Process[] ps = Process.GetProcessesByName(Config.AppConfig.Config_GameWindow.MemProcessName);
            //遍历进程，根据进程ID创建设备对象
            foreach (var p in ps)
            {
                //根据配置文件指定的子类，创建设备对象
                //AllDevices.Add((IDevice)Config.applicationContext.GetObject("Device", new object[] { p.Id }));
                AllDevices.Add(new DevicePC(p.Id));
            }
            return AllDevices;
        }
        public int Hwnd { get; set; }
        public string Title { get; set; }
        public int Pid { get; set; }
        public string Port { get; set; }
        public string IP { get; set; }
        public HubConnection Connection {get; set; }

        protected virtual void GetDeviceInfo(int pid)
        {
            return;
        }
    }
}