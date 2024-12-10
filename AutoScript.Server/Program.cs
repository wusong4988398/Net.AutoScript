using AutoScript.Share;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Policy;
using System.Xml.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using Serilog;


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
            //Class1.dmtest();return;
            // HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
       
            IHostBuilder builder = Host.CreateDefaultBuilder();

            builder.ConfigureHostConfiguration(config =>
            {
                config.AddJsonFile("game.json");//��������ļ�
            });
            builder.ConfigureLogging(logging => { logging.ClearProviders().AddSerilog(); });

            //ConfigureServices(builder.Confi, builder.Services);
            //ע�����Ӷ�������
            builder.ConfigureServices((context, services) =>
            {
                var connection = new HubConnectionBuilder()
                                .WithUrl(context.Configuration["huburl"]!)
                                .WithAutomaticReconnect()
                                .Build();
                connection.ConfigureAwait(true);
                //�Զ������趨
                connection.Closed += async (error) =>
                {
                    await Task.Delay(3 * 1000);
                    await connection.StartAsync();
                };
                connection.StartAsync().Wait();
                //services.AddScoped<Func<int, Device>>(provider => pid =>
                //{
                //    return new DevicePC(pid);
                //});


                services.AddSingleton<HubConnection>(connection);
                services.AddScoped<SingleController>();
            });
            IHost host= builder.Start();


            //service.AddScoped<ScriptController>();


            //builder.Services.AddScoped<IDevice, DevicePC>();
            //builder.Build();



            // ��ע����ô�Į���
            //var registerDmSoftDllResult = RegisterDmSoft.RegisterDmSoftDll();
            //Console.WriteLine($"��ע����ô�Į������أ�{registerDmSoftDllResult}");
            //if (!registerDmSoftDllResult)
            //{
            //    throw new Exception("��ע����ô�Į���ʧ��");
            //}
            //Dmsoft m_dm = new Dmsoft();

            //// �շ�ע��
            //int dm_ret = m_dm.Reg(DmConfig.DmRegCode, DmConfig.DmVerInfo);
            //Console.WriteLine($"�շ�ע�᷵�أ�{dm_ret}");
            //if (dm_ret != 1)
            //{
            //    throw new MyException("�շ�ע��ʧ��,����ֵ:" + dm_ret.ToString());
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
            Application.Run(new FormMain(host));
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
                // ... ִ�к��Ľű������縱���������������
                await Task.Delay(1000); // ģ������ִ��
            }
        }

        private async Task MonitorTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // �����Ϸ�����Ƿ����������Ƿ�����Ȳ���
                Console.WriteLine($"Monitoring... Current operation: {_currentOperation}");
                await Task.Delay(500); // ģ���ز���
            }
        }
    }

 
}