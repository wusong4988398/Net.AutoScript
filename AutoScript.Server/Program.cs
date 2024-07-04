namespace AutoScript.Server
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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