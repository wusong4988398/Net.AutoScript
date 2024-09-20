using AutoScript.Share;
using Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AutoScript.Server
{
    public class 梦幻西游 : GameScriptExecutor
    {
        public Mem MemLib = new Mem();
        public 梦幻西游(DeviceHandler deviceHandler) : base(deviceHandler)
        {
           
        }
        private async void 挖宝图()
        {

            //double value= await this.deviceHandler.ReadMemoryByFeatureCode<double>("C5 8B 00 00 00 ?? ?? 40 03 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 04 00 00 00 ?? ?? ?? 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? 00 00 05 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 04 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 01 00 00 00 ?? ?? 00 00 01 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 04 00 00 00 ?? ?? ?? 00 00 00 00 00 00 00 00 00 C5 8B 00 00 00 00 ?? 40 03 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 04 00 00 00 ?? ?? ?? 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? 00 00 05 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 04 00 00 00 ?? ?? ?? 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? 00 00 04 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 04 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 C5 8B 00 00 00");
            //MemLib.OpenProcess(this.deviceHandler.Device.Pid);
            
            ////IEnumerable<long> AoBScanResults = MemLib.AoBScan(0x01000000, 0x04000000, "?? ?? ?? ?5 ?? ?? 5? 00 ?? 00 00 00 ?? 00 50 00", false, true).Result;
            //var speedAddr = MemLib.AoBScan("24 E9 39 74 34 15 F0 9C").Result;
            ////byte[] fd = API.StrToToHexByte("24 E9 39 74 34 15 F0 9C");
            //IntPtr addr = new IntPtr(0x10FFFFFF);
            //API.GetAddressByFeatureCode2(this.deviceHandler.Device.Pid);
            //addr = API.GetAddressByFeatureCode(this.deviceHandler.Device.Pid, new byte[] { 0x24, 0xE9, 0x39, 0x74, 0x34, 0x15, 0xF0, 0x9C }, 5, addr, new IntPtr(0x7FFFFFFF));
            //string strInfo = API.GetMemString(this.deviceHandler.Device.Pid, addr, 10, Encoding.UTF8);
            //int index_end = strInfo.IndexOf("\"");
            //strInfo = strInfo.Substring(0, index_end);


            this._currentOperation = "挖宝图";
            ActionParam param = new ActionParam() { delay = 500 };  //动作参数对象
            this.deviceHandler.找屏动作(["包裹"], param, ["包裹弹窗"], null);
            this.deviceHandler.找屏动作(["宝图图标"], param, ["包裹"], null);



        }
        private void 宝图()
        {
            this._currentOperation = "宝图任务";
            ActionParam param = new ActionParam() { delay = 500 };  //动作参数对象
            //ImageInfo info=this.deviceHandler.找屏动作(["活动图标"], param, ["师门任务"], new ActionParam());
            this.deviceHandler.找屏动作(["活动图标"], param, ["活动弹窗"], new ActionParam());
            ImageInfo info = this.deviceHandler.找屏动作(["宝图任务"], new ActionParam());
            if (!info.Result.isFinded) return;
            ImageInfo img = new ImageInfo();
            while (!StopCallBack())
            {
                Thread.Sleep(1000);
                ImageInfo info1 = this.deviceHandler.找屏动作(["战斗图标"], null);
                ImageInfo info2 = this.deviceHandler.找屏动作(["日常-宝图任务"], null);
                if (!info1.Result.isFinded&& !info2.Result.isFinded)
                {
                    break;
                }
           

            }


        }
        private void 师门()
        {
            this._currentOperation = "师门任务";
            ActionParam param = new ActionParam() { delay = 500 };  //动作参数对象
            //ImageInfo info=this.deviceHandler.找屏动作(["活动图标"], param, ["师门任务"], new ActionParam());
            this.deviceHandler.找屏动作(["活动图标"], param, ["活动弹窗"], new ActionParam());
            ImageInfo info = this.deviceHandler.找屏动作(["师门任务"], new ActionParam());
            if (!info.Result.isFinded) return;
            //this.deviceHandler.找屏动作(["师门-去完成", "师门-继续任务", "请选择要做的事"], param, ["请选择要做的事", "师门-继续任务"], null);
            //this.deviceHandler.找屏动作(new List<string> {"请选择要做的事" }, param, new List<string> { "" }, null);
            this.deviceHandler.找屏动作(["师门-去完成", "师门-继续任务", "请选择要做的事"], param);
            ImageInfo img = new ImageInfo();
            while (!StopCallBack())
            {
                Thread.Sleep(500);
                if (StopCallBack()) return;
                ImageInfo img2=this.deviceHandler.找屏动作(["上交", "使用", "请选择要做的事"], param);
                if (!img2.Result.isFinded)
                {
                    this.deviceHandler.找屏动作(["侧边栏-师门"], new ActionParam { delay = 500 });
                }
                ImageInfo img1 = this.deviceHandler.找屏动作(["购买"], null);
                if (img1.Result.isFinded)
                {
                    img = this.deviceHandler.找屏动作(["购买商品-第一个商品价格"], null);
                    if (img.Result.isFinded)
                    {
                        decimal price = img.OcrString.ToDecimal();
                        if (price > 500) return;
                        this.deviceHandler.点击(new ActionParam { Point = (img.Result.x, img.Result.y) });
                        Thread.Sleep(800);
                        this.deviceHandler.点击(new ActionParam { Point = (img1.Result.x, img1.Result.y) });
                    }
                }
            }
            this.MainDelay(1000);
            Trace.WriteLine("登入" + Thread.CurrentThread.ManagedThreadId);
        }

        private void 登入()
        {
            this._currentOperation = "登入";

            //this.deviceHandler.找屏动作(new List<string> { "自动匹配" }, new ActionParam());
            //this.deviceHandler.找屏动作(new List<string> { "购买物品" }, new ActionParam());
            
            ActionParam param = new ActionParam() { delay=500};  //动作参数对象

            //this.deviceHandler.找屏动作(new List<string> { "便捷组队","自动匹配"}, param, new List<string> { "取消匹配" }, null);
            //this.deviceHandler.找屏动作(new List<string> { "小关闭", "大关闭","中关闭","大关闭2", "大关闭_排行榜", "大关闭_我的好友", "关闭_大地图", "右下角_使用" }, param, new List<string> { "取消匹配" }, null);

            // this.deviceHandler.点击(new ActionParam { Point = (595, 28),delay=500 });
            //宝图任务
           // this.deviceHandler.找屏动作(new List<string> { "活动图标" }, param, new List<string> { "宝图任务" }, new ActionParam());

           // this.deviceHandler.找屏动作(new List<string> { "宝图任务"}, param, new List<string> { "战斗图标" }, null);
            //师门任务

            //this.deviceHandler.找屏动作(["活动图标"], param, ["师门任务"], new ActionParam());
           // this.deviceHandler.找屏动作(["师门-去完成", "师门-继续任务", "请选择要做的事"], param, ["请选择要做的事", "师门-继续任务"], null);
           //this.deviceHandler.找屏动作(new List<string> {"请选择要做的事" }, param, new List<string> { "" }, null);
            //ImageInfo img = new ImageInfo();
            //while (!StopCallBack())
            //{
            //    Thread.Sleep(100);
            //    if (StopCallBack()) return;
            //    this.deviceHandler.找屏动作(["请选择要做的事"], param);
            //    this.deviceHandler.找屏动作(["上交"], param);
            //    this.deviceHandler.找屏动作(["使用"], param);
            //    this.deviceHandler.找屏动作(["侧边栏-师门"], new ActionParam { delay = 5000 });
            //    ImageInfo img1 = this.deviceHandler.找屏动作(["购买"], null);
            //    if (img1.Result.isFinded)
            //    {
            //        img = this.deviceHandler.找屏动作(["购买商品-第一个商品价格"], null);
            //        if (img.Result.isFinded) {
            //          decimal price=img.OcrString.ToDecimal();
            //            if (price > 500) return;
            //            this.deviceHandler.点击(new ActionParam { Point = (img.Result.x, img.Result.y) });
            //            Thread.Sleep(800);
            //            this.deviceHandler.点击(new ActionParam { Point = (img1.Result.x, img1.Result.y) });
            //        }
            //    }
            //}


            //this.MainDelay(3000);
            
            //Trace.WriteLine("登入" + Thread.CurrentThread.ManagedThreadId);
       
            Console.WriteLine("登入"+ Thread.CurrentThread.ManagedThreadId);
        }
        private void 登出()
        {
            this._currentOperation = "登录操作中，请等待......";
            this.MainDelay(2000);

            Trace.WriteLine("登出" + Thread.CurrentThread.ManagedThreadId);
        }


        protected override async Task MonitorTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _currentOperation != "Executed main script tasks")
            {
                // 检测游戏窗口是否卡死、窗口是否崩溃等操作
                Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + $"Monitoring... Current operation: {_currentOperation}");

                SubDelay(500);
                //await Task.Delay(500); // 模拟监控操作
            }
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + $"监控任务执行完毕.................");
        }

    }
}