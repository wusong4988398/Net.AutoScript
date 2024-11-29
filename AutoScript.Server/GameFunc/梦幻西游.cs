using AutoScript.Server.GameFunc;
using AutoScript.Share;
using Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoScript.Server
{
    public class 梦幻西游 : GameScriptExecutor
    {
        public Mem MemLib = new Mem();
        public 梦幻西游(DeviceHandler deviceHandler) : base(deviceHandler)
        {
           
        }

        private void 操作记事本()
        {
            this._currentOperation = "操作记事本";
            while (true)
            {
                this.deviceHandler.dm.SendString(this.deviceHandler.Device.Hwnd, "我是来测试的");
                Thread.Sleep(1500);

            }

        }
        private async void 挖宝图()
        {


            this._currentOperation = "挖宝图";
            ActionParam param = new ActionParam() { delay = 1000,NotFindCount=60};  //动作参数对象

           // this.deviceHandler.找屏动作(["scrcpy_包裹", "scrcpy_宝图图标", "scrcpy_包裹_藏宝图_使用", "右下角_使用"], param, new ActionParam() { delay = 1500, NotFindCount = 80 });

            this.deviceHandler.找屏动作(["scrcpy_包裹"], param, ["scrcpy_宝图图标"],new ActionParam { delay=500});
            this.deviceHandler.找屏动作(["scrcpy_包裹_藏宝图_使用", "右下角_使用"], param, new ActionParam() { delay = 1500, NotFindCount = 80 });


            //this.deviceHandler.找屏动作(["scrcpy_包裹"], param, ["scrcpy_包裹弹窗"], null);
            //this.deviceHandler.找屏动作(["scrcpy_宝图图标"], param, ["scrcpy_包裹_藏宝图_使用"], new ActionParam { delay=500});
            //this.deviceHandler.找屏动作(["右下角_使用"], new ActionParam() { delay = 1500 }, new ActionParam() { NotFindCount = 100 });
            this._currentOperation = "挖宝图任务结束!!!";


        }
        private void 打宝图()
        {
            this._currentOperation = "打宝图";
            ActionParam param = new ActionParam() { delay = 500 };  //动作参数对象
                                                                    //ImageInfo info=this.deviceHandler.找屏动作(["活动图标"], param, ["师门任务"], new ActionParam());
                                                                    //scrcpy_日常-宝图任务
            this.deviceHandler.找屏动作(["scrcpy_活动图标"], param, ["scrcpy_宝图任务"], new ActionParam { delay = 500 });
         
           // this.deviceHandler.滑动(new ActionParam { ActionType = ActionType.Swipe, Swipe = (809, 117, 809, 224) });
            
            this.deviceHandler.找屏动作(["scrcpy_请选择要做的事_听听无妨"], param, ["scrcpy_日常-宝图任务"], new ActionParam { delay = 500, ActionType = ActionType.DoubleClick });

            this.deviceHandler.找屏动作(["scrcpy_日常-宝图任务"], new ActionParam() { delay = 6500 }, new ActionParam() { delay = 1500, NotFindCount = 80 });


            //this.deviceHandler.找屏动作(["活动图标"], param, ["活动弹窗"], new ActionParam());
            //ImageInfo info = this.deviceHandler.找屏动作(["宝图任务"], new ActionParam());
            //if (!info.Result.isFinded) return;
            //ImageInfo img = new ImageInfo();
            //while (!StopCallBack())
            //{
            //    Thread.Sleep(1000);
            //    ImageInfo info1 = this.deviceHandler.找屏动作(["战斗图标"], null);
            //    ImageInfo info2 = this.deviceHandler.找屏动作(["日常-宝图任务"], null);
            //    if (!info1.Result.isFinded&& !info2.Result.isFinded)
            //    {
            //        break;
            //    }


            //}
            this._currentOperation = "打宝图任务结束!!!";


        }
        /// <summary>
        /// 获取角色的当前坐标
        /// </summary>
        /// <returns></returns>
        private (int,int) GetRolePosition()
        {
            var info = this.deviceHandler.找屏动作(["scrcpy_主界面_坐标"], new ActionParam { ActionType = ActionType.None });
            Trace.WriteLine("当前坐标：" + info.OcrString);
            MatchCollection matches = Regex.Matches(info.OcrString, @"\d+");
            var result = (-1, -1);
            if (matches.Count > 0)
            {
                if (matches.Count == 2)
                {
                    int x = int.Parse(matches[0].Value);
                    int y = int.Parse(matches[1].Value);
                    result = (x, y);
                }
            }
            return result;
        }
        private void 秘境降妖()
        {
            this._currentOperation = "开始秘境降妖";
            ActionParam param = new ActionParam() { delay = 500 };
            //var cc2 = this.deviceHandler.找屏动作(["scrcpy_活动图标"], param);
            //Thread.Sleep(1000);
            //var cc=this.deviceHandler.找屏动作(["scrcpy_主界面_坐标"], new ActionParam { ActionType=ActionType.None});
            //Trace.WriteLine("当前坐标："+cc.OcrString);
            //var pos = GetRolePosition();
            //Trace.WriteLine($"位置: ({pos.Item1}, {pos.Item2})");

            while (true) { 
             Thread.Sleep(2000);
                if (!this.IsCombating&&this.IsStoped)
                {
                    return;
                }
            }

      

            this.deviceHandler.找屏动作(["scrcpy_活动图标"], param, ["scrcpy_秘境降妖"], new ActionParam { delay = 500 });

            Thread.Sleep(1000);

            this.deviceHandler.找屏动作(["scrcpy_请选择要做的事_秘境降妖", "scrcpy_秘境降妖_继续"], param, ["scrcpy_任务栏_秘境降妖"], new ActionParam { delay = 500, ActionType = ActionType.Click });

            //ImageInfo info11 = this.deviceHandler.找屏动作(["scrcpy_任务栏_秘境降妖"], param);
            //Console.WriteLine("111");

            while (!StopCallBack())
            {
                Thread.Sleep(1500);
                if (StopCallBack()) return;
        

                ImageInfo info = this.deviceHandler.找屏动作(["scrcpy_请选择要做的事_进入战斗"], new ActionParam() { delay = 500, ActionType = ActionType.None });
                if (info.Result.isFinded)
                {
                    this.deviceHandler.点击(new ActionParam { Point = (420, 188) });
                    Thread.Sleep(900);
                    ImageInfo img = this.deviceHandler.找屏动作(["scrcpy_任务栏_秘境降妖_第几个怪物"], null);
                    if (img.Result.isFinded)
                    {
                        decimal count = 0;
                        // 正则表达式匹配一个或多个数字
                        Match match = Regex.Match(img.OcrString, @"\d+");
                        if (match.Success) count = match.Value.ToDecimal();
                        if (count > 17) return;
                        this.deviceHandler.找屏动作(["scrcpy_任务栏_秘境降妖"], param);
                        Thread.Sleep(1500);
                        this.deviceHandler.找屏动作(["scrcpy_请选择要做的事_进入战斗"], new ActionParam() { delay = 500 });

                        //this.deviceHandler.找屏动作(["scrcpy_任务栏_秘境降妖"], param, ["scrcpy_请选择要做的事_进入战斗"], new ActionParam { delay = 1000, ActionType = ActionType.Click });

                    }

                }
                info = this.deviceHandler.找屏动作(["scrcpy_战斗失败"], new ActionParam() { delay = 500, ActionType = ActionType.None });
                if (info.Result.isFinded)
                {
                    this.deviceHandler.点击(new ActionParam { Point = (347, 329) });
                    Thread.Sleep(900);
                    this.deviceHandler.点击(new ActionParam { Point = (859, 204) });return;

                }
            }


            //this.deviceHandler.找屏动作(["scrcpy_日常-宝图任务"], new ActionParam() { delay = 6500 }, new ActionParam() { delay = 1500, NotFindCount = 80 });


            //this.deviceHandler.找屏动作(["活动图标"], param, ["活动弹窗"], new ActionParam());
            //ImageInfo info = this.deviceHandler.找屏动作(["宝图任务"], new ActionParam());
            //if (!info.Result.isFinded) return;
            //ImageInfo img = new ImageInfo();
            //while (!StopCallBack())
            //{
            //    Thread.Sleep(1000);
            //    ImageInfo info1 = this.deviceHandler.找屏动作(["战斗图标"], null);
            //    ImageInfo info2 = this.deviceHandler.找屏动作(["日常-宝图任务"], null);
            //    if (!info1.Result.isFinded&& !info2.Result.isFinded)
            //    {
            //        break;
            //    }


            //}
            this._currentOperation = "秘境降妖结束!!!";


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

        private void 清理背包()
        {
            this._currentOperation = "清理背包";




            this._currentOperation = "清理背包结束";


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
            //检查游戏是否卡主20秒内无反应
            Task.Factory.StartNew(() =>
            {
          
                MovementDetector detector = new MovementDetector(20);
                while (!token.IsCancellationRequested && _currentOperation != "Executed main script tasks") {
                    var pos = GetRolePosition();
                    detector.UpdatePosition(new Position(pos.Item1, pos.Item2));
                    this.IsStoped= detector.IsStoped;
                    Trace.WriteLine($"当前页面是否卡住: {this.IsStoped}");
                    Thread.Sleep(20000);
                }

            });

            while (!token.IsCancellationRequested && _currentOperation != "Executed main script tasks")
            {
                
                // 检测游戏窗口是否卡死、窗口是否崩溃等操作
                // Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + $"Monitoring... Current operation: {_currentOperation}");
                SubDelay(1000);
                //var pos = GetRolePosition();

                var info = this.deviceHandler.找屏动作(["scrcpy_战斗图标"], new ActionParam { ActionType = ActionType.None });
                //Trace.WriteLine($"当前战斗状态: {info.Result.isFinded}");

                this.IsCombating = info.Result.isFinded;
                Trace.WriteLine($"当前战斗状态: {this.IsCombating}");
                //Trace.WriteLine($"当前角色位置: ({pos.Item1}, {pos.Item2})");
                //await Task.Delay(1000); // 模拟监控操作


            }
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + $"监控任务执行完毕.................");
        }

    }
}