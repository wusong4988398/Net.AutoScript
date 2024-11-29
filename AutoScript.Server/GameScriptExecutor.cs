using AutoScript.Share;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AutoScript.Server
{
    public abstract  class GameScriptExecutor
    {
        protected DeviceHandler deviceHandler;
        public Account Account;
        public CancellationTokenSource _cancellationTokenSource {  get; set; }
        private Task _mainTask;
        private Task _monitorTask;
        protected string _currentOperation;
        protected GameStatus _gameStatus= GameStatus.Running;

        protected bool IsStoped {  get; set; }

        protected bool IsRunning { get; set; }

        protected bool IsCombating { get; set; }


        public static event EventHandler StatusChangedEvent;

        private GameHelperArgs args = new GameHelperArgs();
        public CheckStopCallBack StopCallBack { get; set; }

        public CheckPauseCallBack PauseCallBack { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceHandler"></param>
        public GameScriptExecutor(DeviceHandler deviceHandler)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            this.deviceHandler = deviceHandler;
        }
        public void Start()
        {
            
            _mainTask = Task.Run(() => MainTask(_cancellationTokenSource.Token));
            _monitorTask = Task.Run(() => MonitorTask(_cancellationTokenSource.Token));
            Task.WaitAll(_mainTask, _monitorTask);
            Trace.WriteLine($"主任务和副任务执行完毕.................");

        }

        public void Stop()
        {
            if (_mainTask == null) return;
            _cancellationTokenSource.Cancel();
            
            Task.WaitAll(_mainTask, _monitorTask);
        }



       
        private async Task MainTask(CancellationToken token)
        {

            Init(); //初始化
                    //while (!token.IsCancellationRequested)
                    //{
                    //    _currentOperation = "Executing main script tasks";
                    //    // ... 执行核心脚本任务，如副本任务、主线任务等
                    //    await Task.Delay(1000); // 模拟任务执行
                    //}
            args.Index = this.Account.Index;
            _currentOperation = "Executing main script tasks";
            foreach (var cmd in Script.AllCommands)
            {
                args.Script = cmd;
                SetStatus("Running");
                if (!token.IsCancellationRequested)
                {
                    if (this.PauseCallBack())
                    {
                        while (true)
                        {
                            if (!this.PauseCallBack())
                            {
                                break;
                            }
                        }
                    }
                    this.ExecScript(cmd, null);
                    if (StopCallBack())
                    {
                        SetStatus("Stop");
                        //this.Stop();
                        return;
                    }
                }
                


            }
            SetStatus("Finish");//当一个账号完成所有脚本命令后，状态设置为Finish
            _currentOperation = "Executed main script tasks";
            Trace.WriteLine($"主任务执行完毕.................");

        }


        public void ExecScript(string cmd, object[] paramters)
        {
            if (this.deviceHandler.StopCallBack == null)
            {
                this.deviceHandler.StopCallBack = this.StopCallBack;
            }

            if (this.deviceHandler.PauseCallBack == null)
            {
                this.deviceHandler.PauseCallBack = this.PauseCallBack;
            }

            MethodInfo m = this.GetType().GetMethod(cmd, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (m == null) return;
            if (m.GetParameters().Length > 0)
            {
                m.Invoke(this, paramters);
            }
            else
            {
                m.Invoke(this, null);
            }

        }

        protected abstract Task  MonitorTask(CancellationToken token);

  

        protected void  MainDelay(int time)
        {
            


            Thread.Sleep(time);
        }

        protected void SubDelay(int time)
        {

            if (this.PauseCallBack())
            {
                while (true)
                {
                    if (!this.PauseCallBack())
                    {
                        break;
                    }
                }
            }

            Thread.Sleep(time);
        }
        protected void Init()
        {
            //IsStop = false;
            //Status = GameHelperStatus.Ready;
            args.DeviceName = this.deviceHandler.Device.Title;
            args.Hwnd = this.deviceHandler.Device.Hwnd;
        }
        protected void SetStatus(string status)
        {
            this.Account.Status = status;
            args.Status = status;
            StatusChangedEvent(this, args);
        }

    }
}
