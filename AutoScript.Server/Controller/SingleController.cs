using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Server
{
    public class SingleController:ControllerBase
    {
        public SingleController(HubConnection connection) : base(connection)
        {

        }

        public override async void Start()
        {
            if (AllHelpers.Count==0)
            {
                Reload();
            }
            foreach (GameHelper helper in AllHelpers) { 
             
                //helper.Start();
                Func<Account,Task> action = helper.Start;
                Task task = Task.Run(() => action.Invoke(null));
                Trace.WriteLine("Start.......................................");
       
                //await helper.Start();
                //action.BeginInvoke(null,null);//异步调用 这里会报错 提示Operation is not supported on this platform.
                //Task.Run(() => action.Invoke(null));
                // Console.WriteLine("122");
            }
        }
        public override void Start(Account account)
        {
            if (AllHelpers.Count == 0)
            {
                Reload();
            }
            //找账号相同的脚本对象,如果没有,则找空闲的脚本对象
            foreach (var g in AllHelpers)
            {
                if (g.Account?.Index == account.Index)
                {
                    g.Status = Share.GameHelperStatus.Running;
                    Func<Account, Task> action = g.Start;
                    Task.Run(() => action.Invoke(g.Account));
                    return;
                }
            }
            //如果没有,则找空闲的脚本对象
            //TODO 脚本对象状态要维护
            foreach (var g in AllHelpers)
            {
                if (g.Status != Share.GameHelperStatus.Running)
                {
                    g.Status = Share.GameHelperStatus.Running;
                    Func<Account, Task> action = g.Start;
                    Task.Run(() => action.Invoke(account));
                    return;
                }
            }
        }

        public override void Start(List<Account> accounts)
        {
            foreach (var acc in accounts)
            {
                this.Start(acc);
            }
        }

        public override void Stop()
        {
            foreach (GameHelper helper in AllHelpers) {

                Action action= helper.Stop;
                //action.BeginInvoke(null, null);
                Task.Run(() => action.Invoke());
            }
        }
        public override void Stop(Account account)
        {
            //找账号相同的脚本对象,如果没有,则找空闲的脚本对象
            foreach (var g in AllHelpers)
            {
                if (g.Account?.Index == account.Index)
                {
                    Action action = g.Stop;
                    Task.Run(() => action.Invoke());
                    return;
                }
            }
        }

        public override void Stop(List<Account> accounts)
        {
            foreach (var item in accounts)
            {
                this.Stop(item);
            }
        }
    }
}
