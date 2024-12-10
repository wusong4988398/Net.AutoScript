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
            this.deviceHandler.找屏动作(["scrcpy_包裹_藏宝图_使用", "右下角_使用"], param, new ActionParam() { delay = 1500, NotFindCount = 100 });


            //this.deviceHandler.找屏动作(["scrcpy_包裹"], param, ["scrcpy_包裹弹窗"], null);
            //this.deviceHandler.找屏动作(["scrcpy_宝图图标"], param, ["scrcpy_包裹_藏宝图_使用"], new ActionParam { delay=500});
            //this.deviceHandler.找屏动作(["右下角_使用"], new ActionParam() { delay = 1500 }, new ActionParam() { NotFindCount = 100 });
            this._currentOperation = "挖宝图任务结束!!!";


        }
        private void 打宝图()
        {



            //while (true) { 

            //}
            Thread.Sleep(3000);

            this._currentOperation = "打宝图";
            if (!this.IsCombating && this.IsGameWindowBlocked) this.deviceHandler.dm.RightClick();
            var cur_pos = this.GetRolePosition();
            ActionParam param = new ActionParam() { delay = 500 };  //动作参数对象
                                                                    //ImageInfo info=this.deviceHandler.找屏动作(["活动图标"], param, ["师门任务"], new ActionParam());
                                                                    //scrcpy_日常-宝图任务
            //this.deviceHandler.找屏动作(["scrcpy_活动图标"], param, ["scrcpy_宝图任务"], new ActionParam { delay = 500 });
            
            
            //this.deviceHandler.找屏动作(["scrcpy_请选择要做的事_听听无妨"], param, ["scrcpy_日常-宝图任务"], new ActionParam { delay = 500, ActionType = ActionType.DoubleClick });

            //this.deviceHandler.找屏动作(["scrcpy_日常-宝图任务"], new ActionParam() { delay = 500 }, new ActionParam() { delay = 1500, NotFindCount = 15 });
            //var pre_pos = this.Position;
            //if (!this.IsCombating && (cur_pos.Item1==pre_pos.X&& cur_pos.Item2 == pre_pos.Y)) this.deviceHandler.滑动(new ActionParam { ActionType = ActionType.Swipe, Swipe = (809, 117, 809, 224) });

            this.deviceHandler.找屏动作(["scrcpy_日常-宝图任务"], new ActionParam() { delay = 6500 }, new ActionParam() { delay = 1500, NotFindCount = 2 });

            Trace.WriteLine("打宝图任务结束!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
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
            if (!this.IsCombating && this.IsGameWindowBlocked) this.deviceHandler.dm.RightClick();


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
        private void 快手看直播领金币()
        {
            while (true)
            {

                Thread.Sleep(1500);
                ImageInfo imageInfo = this.deviceHandler.找屏动作(["快手_浏览内容领金币"], new ActionParam { ActionType = ActionType.None });
                if (imageInfo.Result.isFinded)
                {
                    this.deviceHandler.滑动(new ActionParam { ActionType = ActionType.Swipe, Swipe = (210, 794, 210, 328) });
                    Thread.Sleep(3500);
                    this.deviceHandler.点击(new ActionParam { Point = (imageInfo.Result.x, imageInfo.Result.y + 200) });


                    Thread.Sleep(38000);
                    this.deviceHandler.滑动(new ActionParam { ActionType = ActionType.Swipe, Swipe = (0, 375, 300, 375) });
                    Thread.Sleep(2500);
                    this.deviceHandler.找屏动作(["快手_退出直播间"], new ActionParam { ActionType = ActionType.Click });

                }



                //快手_直播间_已领取
                //ImageInfo imageInfo2 = this.deviceHandler.找屏动作(["快手_直播间_已领取"], new ActionParam { ActionType = ActionType.None });
                //if (imageInfo2.Result.isFinded)
                //{
                //    this.deviceHandler.滑动(new ActionParam { ActionType = ActionType.Swipe, Swipe = (0, 375, 300, 375) });
                //    Thread.Sleep(2500);
                //    this.deviceHandler.找屏动作(["快手_退出直播间"], new ActionParam { ActionType = ActionType.Click });


                //}






                Thread.Sleep(2000);

            }
        }
        private void 清理背包()
        {
            this._currentOperation = "清理背包";
            ActionParam param = new ActionParam() { delay = 500 };

            //this.deviceHandler.滑动(new ActionParam { ActionType = ActionType.Swipe, Swipe = (210, 794, 210, 328),delay=500 });return;
            ////this.deviceHandler.dm.WheelDown();

            //return;
            while (true)
            {

                //this.deviceHandler.找屏动作(new List<string> { "scrcpy_小关闭", "scrcpy_大关闭", "scrcpy_中关闭", "scrcpy_大关闭2", "scrcpy_大关闭_排行榜", "scrcpy_大关闭_我的好友", "scrcpy_关闭_大地图" }, param, new List<string> { "scrcpy_活动图标" }, null);
                if (!this.IsCombating && this.IsGameWindowBlocked)
                {
                    this.deviceHandler.dm.RightClick();
                }
                Thread.Sleep(2000);
            }


            this.deviceHandler.找屏动作(["scrcpy_主界面_更多_展开按钮", "scrcpy_主界面_更多_系统按钮", "scrcpy_主界面_更多_系统_切换账号图片", "scrcpy_主界面_更多_系统_切换账号_确认", "scrcpy_登录页面_登录文字", "scrcpy_登录页面_切换角色", "scrcpy_登录页面_切换角色_已有角色"], param, new ActionParam() { delay = 500,NotFindCount=5 });

            List<Position> points = new List<Position>() { new Position(389,110), new Position(540, 113) , new Position(672, 109) , new Position(385, 285) , new Position(534, 284) , new Position(673, 286) };
            //List<Position> reversedPoints = points.ToList(); // 创建副本
            points.Reverse(); // 反转副本
            foreach (var point in points) {

                this.deviceHandler.点击(new ActionParam { Point = (point.X, point.Y) });
                Thread.Sleep(1500);
                var info= this.deviceHandler.找屏动作(["scrcpy_登录页面_切换角色_服务器选择"], new ActionParam { ActionType=ActionType.None});
                if (!info.Result.isFinded)
                {
                    break;
                }

            }


            this.deviceHandler.找屏动作(new List<string> { "scrcpy_小关闭", "scrcpy_大关闭", "scrcpy_中关闭", "scrcpy_大关闭2", "scrcpy_大关闭_排行榜", "scrcpy_大关闭_我的好友", "scrcpy_关闭_大地图" }, param, new List<string> { "scrcpy_活动图标" }, null);

       

            return;

            while (true)//找宝石、制造书、百炼精铁出售
            {
                ImageInfo imageInfo = this.deviceHandler.找屏动作(["scrcpy_翡翠石", "scrcpy_太阳石", "scrcpy_昆仑玉", "scrcpy_神秘石", "scrcpy_月亮石", "scrcpy_黑宝石", "scrcpy_舍利子", "scrcpy_红纹石", "scrcpy_百炼精铁", "scrcpy_制造书", "scrcpy_光芒石"], new ActionParam { NotFindCount = 20 }, ["scrcpy_宝石点击_更多按钮", "scrcpy_物品点击_商会出售"], param);
                this.deviceHandler.找屏动作(["scrcpy_宝石点击_更多_商会出售"], new ActionParam { NotFindCount = 10 }, ["scrcpy_宝石点击_更多_商会出售_出售"], param);
                this.deviceHandler.找屏动作(["scrcpy_宝石点击_更多_商会出售_出售"], param);
                if (!imageInfo.Result.isFinded)
                {
                    Trace.WriteLine("没有找到宝石、制造书、百炼精铁等物品 任务结束！！！！！！");
                    break;
                }


                Thread.Sleep(2000);
           
            }

            while (true)//藏宝图
            {
                this.deviceHandler.找屏动作(["scrcpy_宝图图标"], new ActionParam { NotFindCount = 20 }, ["scrcpy_包裹_藏宝图_使用"], new ActionParam { delay = 500 });
                ImageInfo imageInfo =  this.deviceHandler.找屏动作(["右下角_使用"], param, new ActionParam() { delay = 1500, NotFindCount = 100 });
                if (!imageInfo.Result.isFinded)
                {
                    Trace.WriteLine("没有找到宝图 任务结束！！！！！！");
                    break;
                }

                Thread.Sleep(2000);

            }



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

                    this.IsGameWindowBlocked = pos.Item1 == -1 ? true : false;
                    detector.UpdatePosition(new Position(pos.Item1, pos.Item2));
                    this.IsStoped= detector.IsStoped;
                    //Trace.WriteLine($"当前页面是否卡住: {this.IsStoped}");
                    Thread.Sleep(20000);
                }

            });


     

            while (!token.IsCancellationRequested && _currentOperation != "Executed main script tasks")
            {
                
                // 检测游戏窗口是否卡死、窗口是否崩溃等操作
                // Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + $"Monitoring... Current operation: {_currentOperation}");
       
                var pos = GetRolePosition();
                this.IsGameWindowBlocked = pos.Item1 == -1 ? true : false;
                var info = this.deviceHandler.找屏动作(["scrcpy_战斗图标"], new ActionParam { ActionType = ActionType.None });
                //Trace.WriteLine($"当前战斗状态: {info.Result.isFinded}");
                this.Position=new Position(pos.Item1, pos.Item2);
                this.IsCombating = info.Result.isFinded;
                Trace.WriteLine($"当前战斗状态: {this.IsCombating}");
                //Trace.WriteLine($"当前角色位置: ({pos.Item1}, {pos.Item2})");
                //await Task.Delay(1000); // 模拟监控操作
                SubDelay(1000);

            }
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + $"监控任务执行完毕.................");
        }

    }
}