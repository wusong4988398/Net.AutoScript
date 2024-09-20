using AoJiaLib;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Share
{
    /// <summary>
    /// 设备控制器类
    /// </summary>
    public class DeviceHandler :  IDeviceInput, IDeviceScreen, IDeviceMemory, IDevicePacket
    {
        public IDevice Device { get ; set ; }
        public Dm.dmsoft dm;
        public AoJiaD aj;
        private IDeviceInput deviceInput;
        private IDeviceScreen deviceScreen;
        private IDeviceMemory deviceMemory;
        private IDevicePacket devicePacket;
        public CheckStopCallBack StopCallBack { get; set; }

        public CheckPauseCallBack PauseCallBack { get; set; }

        public DeviceHandler(IDevice device, HubConnection connection)
        {
            device.Connection = connection;
            this.Device = device;

            //根据配置文件绑定大漠对象到设备


            //dm.SetDict(0, "关闭.txt");
            //dm.EnableRealKeypad(1);
            //dm.SetKeypadDelay("normal", 30);
            //根据配置文件创建实现各个接口的对象,也可以直接new
            //deviceInput = (IDeviceInput)Config.applicationContext.GetObject("DeviceInput", new object[] { device});
            if (Config.AppConfig.PlugeName == "AJ")//aj插件
            {
                aj = new AoJiaD();
                aj.KQHouTai(device.Hwnd,
                    Config.AppConfig.Config_AJ.Display,
                    Config.AppConfig.Config_AJ.Mouse,
                    Config.AppConfig.Config_AJ.KeyBorad,
                    Config.AppConfig.Config_AJ.Flag,
                    Config.AppConfig.Config_AJ.Mode);
                aj.SetPath(Config.AppConfig.Config_AJ.Path);
                aj.SetWindowSize(this.Device.Hwnd, 650, 500);
                deviceInput = new AJInput(device, aj);
                deviceScreen = new DeviceScreenAj(device, aj);

           
            }
            else if(Config.AppConfig.PlugeName == "DM")
            {
                dm = new Dm.dmsoft();
                dm.BindWindow(device.Hwnd,
                    Config.AppConfig.Config_DM.Display,
                    Config.AppConfig.Config_DM.Mouse,
                    Config.AppConfig.Config_DM.KeyBorad,
                    Config.AppConfig.Config_DM.Mode);
                dm.SetPath(Config.AppConfig.Config_DM.Path);
                dm.SetWindowSize(this.Device.Hwnd, 650, 500);
                deviceInput = new DmInput(device, dm);
                deviceScreen = new DeviceScreenDm(device, dm);
                //dm.SetMemoryHwndAsProcessId(1);
               //var result = dm.FindDataEx(42412, "0000000000000000-00007fffffffffff", "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 ?? ?? 00 00 ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? 00 00 ?? ?? 00 00 00 3F 00 00 00 3F 01 00 00 00 00 00 ?? 42 00 00 ?? 42 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 ?? ?? 43 00 00 DC 42 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 01 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 FE FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? 00 00 ?? ?? ?? ?? ?? ?? 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0F 00 00 00 00 00 00 00", 4, 1, 0);
                //Trace.WriteLine(result);

            }
            //
            //deviceScreen = new GDIScreenFinder(new IntPtr(device.Hwnd));
            deviceMemory = new Memory(device);
            devicePacket = new Packet (device);
        }

        public delegate string Test();
        public string test(Test t)
        {
            return t();
        }
        public ImageInfo 找屏动作(ImageInfo info, ActionParam param)
        {
            info = 找屏(info);
            //分析信息,逻辑处理
            if (info.Result.isFinded && param != null)
            {
                param.Point = (info.Result.x + info.Offset.x, info.Result.y + info.Offset.y);
                if (param!=null&&param.ActionType== ActionType.Click)
                {
                    Trace.WriteLine($"执行{info.Name},点击坐标:{param.Point.x},{param.Point.y}");

                }

                点击(param);
                发送字符串(param);
                发送按键(param);
                滑动(param);
            }
            return info;
        }
        public ImageInfo 找屏动作(List<string> names, ActionParam param)
        {
            if (names == null || names.Count == 0) return null;
            List<ImageInfo> infos =  ImageInfo.AllImageInfos.FindAll(p => { return names.Contains(p.Name); });
            ImageInfo ret = new ImageInfo();
            foreach (var info in infos)
            {
                info.Result = (-1, -1, false);
                
                ret = 找屏动作(info, param);
                if (ret.Result.isFinded)
                {
                    return ret;
                }
            }
            return ret;
        }
        public bool StopFlg = false;
        public ImageInfo 找屏动作(List<string> findLst, ActionParam findAction, List<string> stopLst, ActionParam stopAction)
        {
            ImageInfo ret = new ImageInfo();
            while (true)
            {
                //防止卡死,强制停止
                if (this.StopCallBack != null && this.StopCallBack())
                {
                    return ret;
                }
                找屏动作(findLst, findAction);
                ret = 找屏动作(stopLst, stopAction);
                if (ret.Result.isFinded)
                {
                    return ret;
                }
                Thread.Sleep(500);
            }
        }
        public void Intercept()
        {
            devicePacket.Intercept();
        }

        public async Task<T> ReadMemoryByFeatureCode<T>(string search) where T : struct
        {
            return await this.deviceMemory.ReadMemoryByFeatureCode<T>(search);
           
        }

        public void Recv()
        {
            throw new NotImplementedException();
        }

        public void Send()
        {
            throw new NotImplementedException();
        }

        public void WriteMemValue()
        {
            throw new NotImplementedException();
        }

        public void 发送字符串(ActionParam actionParam)
        {
            this.deviceInput.发送字符串(actionParam);
        }
        /// <summary>
        /// 发送按键
        /// </summary>
        /// <param name="actionParam">动作对象</param>
        public void 发送按键(ActionParam actionParam)
        {
            this.deviceInput.发送按键(actionParam);
        }

        public void 启动APP()
        {
            this.deviceInput.启动APP();
        }


        public void 滑动(ActionParam actionParam)
        {
            this.deviceInput.滑动(actionParam);
        }

        public void 点击(ActionParam actionParam)
        {
            this.deviceInput.点击(actionParam);
        }

        public void 退出APP()
        {
            this.deviceInput.退出APP();
        }

        public ImageInfo 找屏(ImageInfo info)
        {
            return this.deviceScreen.找屏(info);
        }
    }
}