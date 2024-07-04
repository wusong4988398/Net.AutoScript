using AutoScript.Share;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AutoScript.Server
{
    
    /// <summary>
    /// 脚本功能管理类
    /// </summary>
    public class GameHelper
    {
     
        private DeviceHandler DeviceHandler;
        //private ScriptExecutorBase Fun;
        private GameScriptExecutor gameScriptExecutor;


        private bool IsStop = false;
        private bool IsPause = false;
        //账号对象
        public Account Account { get; set; } =new Account();

        private GameHelperArgs args=new GameHelperArgs();
        /// <summary>
        /// 状态对象
        /// </summary>
        public GameHelperStatus Status { get; set; }

        public GameHelper(DeviceHandler deviceHandler)
        {
            this.DeviceHandler = deviceHandler;
            //this.Fun = new GameFunBase(deviceHandler);
            //this.Fun=(ScriptExecutorBase)Config.applicationContext.GetObject("ScriptExecutor", new object[] {deviceHandler});
            this.gameScriptExecutor = (GameScriptExecutor)Config.applicationContext.GetObject("ScriptExecutor", new object[] { deviceHandler });
            //this.gameScriptExecutor=new 问道(deviceHandler);
            gameScriptExecutor.StopCallBack = () => IsStop;
            gameScriptExecutor.PauseCallBack = () => IsPause;

        }


        public async Task Start(Account account = null)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            //得到一个游戏账号
            //循环script命令
            //执行fun.ExecScript()
            Init(); //初始化

            Account curAccount = null;
            if (account == null) {
                curAccount = Account.GetAccounts().FirstOrDefault(t => t.Hwnd == this.DeviceHandler.Device.Hwnd);
            }
            else
            {
                curAccount = account;
            }
            if (curAccount!=null)
            {
                this.Account = curAccount;
                //GameScriptExecutor gameScript = new 问道(DeviceHandler);
                // this.gameScriptExecutor=gameScript;
                gameScriptExecutor.StopCallBack= () => IsStop;
                gameScriptExecutor.PauseCallBack = () => IsPause;
                gameScriptExecutor.Account = this.Account;
                gameScriptExecutor._cancellationTokenSource = cts;
                gameScriptExecutor.Start();

                
            }
            

            
       
        }

        public void Stop()
        {
            IsStop = true;
            gameScriptExecutor.Stop();
        }


        public void Pause()
        {
            IsPause = true;

        }

        protected void SetStatus(string status)
        {
            this.Account.Status=status;
            args.Status = status;
            StatusChangedEvent(this,args);
        }
        public static event EventHandler StatusChangedEvent;
        protected void Init()
        {
            IsStop = false;
            IsPause = false;
            Status = GameHelperStatus.Ready;
            args.DeviceName = this.DeviceHandler.Device.Title;
            args.Hwnd=this.DeviceHandler.Device.Hwnd;
        }

    }
}
