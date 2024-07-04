using AutoScript.Share;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Logging.Configuration.ArgUtils;

namespace AutoScript.Server
{
    public class CoreScriptExecutor: ICommunicationChannel
    {
        private string _currentAction;
        private readonly ConcurrentDictionary<string, string> _communicationDict = new ConcurrentDictionary<string, string>();
        public event EventHandler<string> OnActionChanged;
        private CancellationToken _cancellationToken;
        //private DeviceHandler deviceHandler;
        //public Account Account;
        //public CheckStopCallBack StopCallBack { get; set; }
      
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId+"核心脚本任务开始执行...");

            try
            {
                // 示例：模拟执行几个不同任务
                foreach (var action in new[] { "任务1", "任务2", "任务3" })
                {
                    _cancellationToken.ThrowIfCancellationRequested();
                    SetCurrentAction(action);
                    Trace.WriteLine(Thread.CurrentThread.ManagedThreadId+$"开始执行：{action}");
                    await Task.Delay(1000, _cancellationToken); // 模拟任务执行
                    Trace.WriteLine(Thread.CurrentThread.ManagedThreadId+$"完成执行：{action}");
                }
                Trace.WriteLine(Thread.CurrentThread.ManagedThreadId+"核心脚本任务执行完毕");
                SetCurrentAction("核心脚本任务执行完毕");
            }
            catch (OperationCanceledException)
            {
                Trace.WriteLine(Thread.CurrentThread.ManagedThreadId+"核心脚本任务被取消");
            }
        }

        public string GetCurrentAction()
        {
            return _currentAction;
        }

        private void SetCurrentAction(string action)
        {
            _currentAction = action;
            _communicationDict["CurrentAction"] = action;
            OnActionChanged?.Invoke(this, action);
        }
        public void Notify(string action)
        {
            // 副线程可以通过此方法通知主线程，但在这个场景中可能不适用，因为我们主要是主线程向副线程发送信息。
        }
    }
}
