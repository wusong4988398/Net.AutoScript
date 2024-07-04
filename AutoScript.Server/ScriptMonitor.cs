using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Server
{
    /// <summary>
    /// 脚本运行监视类
    /// </summary>
    public class ScriptMonitor
    {
        private readonly CoreScriptExecutor _scriptExecutor;

        private string mainScriptStatus;
        public ScriptMonitor(CoreScriptExecutor scriptExecutor)
        {
            this._scriptExecutor = scriptExecutor;
          
            _scriptExecutor.OnActionChanged += (_, action) =>
            {
                Trace.WriteLine(Thread.CurrentThread.ManagedThreadId+$"监控器收到通知：主线程正在执行 - {action}");
                mainScriptStatus=action.ToString();
            };
        }
        public async Task MonitorAsync(CancellationToken _cancellationToken)
        {
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId+"游戏窗口监控开始...");

            while (!_cancellationToken.IsCancellationRequested&& mainScriptStatus!= "核心脚本任务执行完毕")
            {
                // 模拟监控操作
                //Trace.WriteLine($"监控中... 主线程当前操作：{_scriptExecutor.GetCurrentAction()}");
                Trace.WriteLine(Thread.CurrentThread.ManagedThreadId+$"监控中... 主线程当前操作：");
                // 检查游戏窗口状态，这里简化处理
                await Task.Delay(2000, _cancellationToken);
            }

            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId+"游戏窗口监控停止");
        }
    }
}
