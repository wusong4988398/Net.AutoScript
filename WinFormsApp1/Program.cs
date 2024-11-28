using AutoScript.Share;

namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            if (Environment.Is64BitProcess)
            {
                Console.WriteLine("这是 64 位程序");
                Console.WriteLine("按任意键结束程序");
                Console.ReadKey();
                return;
            }
            // 免注册调用大漠插件
            var registerDmSoftDllResult = RegisterDmSoft.RegisterDmSoftDll();
            Console.WriteLine($"免注册调用大漠插件返回：{registerDmSoftDllResult}");
            if (!registerDmSoftDllResult)
            {
                throw new Exception("免注册调用大漠插件失败");
            }
            Idmsoft dm; 

            // 创建对象
            try
            {


                dm = DMP.CreateDM();
            }
            catch (Exception ex)
            {

                throw;
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}