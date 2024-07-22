using AutoScript.Share;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Server
{
    public abstract class ControllerBase : IController
    {

        protected static HubConnection connection;

        public ControllerBase(HubConnection connection)
        {
            ControllerBase.connection = connection;
        }


        protected static List<GameHelper> AllHelpers = new List<GameHelper>();
        //注意这只是一个临时写法，实际运行时，这里的Server端设备都是从客户端传递过来的，所以我们
        //需要定义一个类，维护客户端传过来的设备控制器，根据这个容器创建GameHelper对象
        public static List<GameHelper> Reload()
        {
            AllHelpers.Clear();
            Device.Reload();
            foreach (var device in Device.GetAllDevices())
            {
               // GameHelper g = (GameHelper)Config.applicationContext.GetObject("GameHelper", new object[] { new DeviceHandler(device) });
               
                GameHelper g=new GameHelper(new DeviceHandler(device, connection));
               
               AllHelpers.Add(g);
            }
            return AllHelpers;
        }


        public abstract void Start(Account account);

        public abstract void Start(List<Account> accounts);

        public abstract void Start();


        public abstract void Stop(Account account);

        public abstract void Stop();

        public abstract void Stop(List<Account> accounts);
    }
}
