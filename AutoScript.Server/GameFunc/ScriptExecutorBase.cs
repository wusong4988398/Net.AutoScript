using AutoScript.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Server
{
    /// <summary>
    /// 核心脚本执行基类
    /// </summary>
    public abstract class ScriptExecutorBase
    {
        private DeviceHandler deviceHandler;
        public Account Account;
        public ScriptExecutorBase(DeviceHandler deviceHandler)
        {
            this.deviceHandler = deviceHandler;
        }
        public CheckStopCallBack StopCallBack { get; set; }
        public void ExecScript(string cmd, object[] paramters)
        {
            
            if (this.deviceHandler.StopCallBack == null)
            {
                this.deviceHandler.StopCallBack = this.StopCallBack;
            }
            MethodInfo m= this.GetType().GetMethod(cmd, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
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

    }
}
