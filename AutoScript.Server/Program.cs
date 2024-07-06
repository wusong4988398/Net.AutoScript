using AutoScript.Share;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Policy;
using System.Xml.Linq;

namespace AutoScript.Server
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddScoped<IDevice, DevicePC>();
            builder.Build();

            // 免注册调用大漠插件
            //var registerDmSoftDllResult = RegisterDmSoft.RegisterDmSoftDll();
            //Console.WriteLine($"免注册调用大漠插件返回：{registerDmSoftDllResult}");
            //if (!registerDmSoftDllResult)
            //{
            //    throw new Exception("免注册调用大漠插件失败");
            //}
            //Dmsoft m_dm = new Dmsoft();

            //// 收费注册
            //int dm_ret = m_dm.Reg(DmConfig.DmRegCode, DmConfig.DmVerInfo);
            //Console.WriteLine($"收费注册返回：{dm_ret}");
            //if (dm_ret != 1)
            //{
            //    throw new MyException("收费注册失败,返回值:" + dm_ret.ToString());
            //}

            //GameScript gameScript = new GameScript();
            //gameScript.Start();

            //Console.WriteLine("Press any key to stop...");
            //Console.ReadKey();

            //gameScript.Stop();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());
            Application.Run(new FormMain());
        }
    }





public class GameScript
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Task _mainTask;
        private Task _monitorTask;
        private string _currentOperation;

        public GameScript()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _mainTask = Task.Run(() => MainTask(_cancellationTokenSource.Token));
            _monitorTask = Task.Run(() => MonitorTask(_cancellationTokenSource.Token));
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            Task.WaitAll(_mainTask, _monitorTask);
        }

        private async Task MainTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _currentOperation = "Executing main script tasks";
                // ... 执行核心脚本任务，如副本任务、主线任务等
                await Task.Delay(1000); // 模拟任务执行
            }
        }

        private async Task MonitorTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // 检测游戏窗口是否卡死、窗口是否崩溃等操作
                Console.WriteLine($"Monitoring... Current operation: {_currentOperation}");
                await Task.Delay(500); // 模拟监控操作
            }
        }
    }

 
}