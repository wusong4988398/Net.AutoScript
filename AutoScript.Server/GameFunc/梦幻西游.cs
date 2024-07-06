using AutoScript.Share;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AutoScript.Server
{
    public class 梦幻西游 : GameScriptExecutor
    {
        public 梦幻西游(DeviceHandler deviceHandler) : base(deviceHandler)
        {
           
        }

  

        private void 登入()
        {
            this._currentOperation = "登录操作中";

            //this.deviceHandler.找屏动作(new List<string> { "自动匹配" }, new ActionParam());
            ActionParam param = new ActionParam() { delay=100};  //动作参数对象

            //this.deviceHandler.找屏动作(new List<string> { "便捷组队","自动匹配"}, param, new List<string> { "取消匹配" }, null);
            this.deviceHandler.找屏动作(new List<string> { "小关闭", "大关闭","中关闭","大关闭2", "大关闭_排行榜", "大关闭_我的好友", "关闭_大地图", "右下角_使用" }, param, new List<string> { "取消匹配" }, null);

            // this.deviceHandler.点击(new ActionParam { Point = (595, 28),delay=500 });

            this.MainDelay(3000);
            
            Trace.WriteLine("登入" + Thread.CurrentThread.ManagedThreadId);
       
            //Console.WriteLine("登入"+ Thread.CurrentThread.ManagedThreadId);
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