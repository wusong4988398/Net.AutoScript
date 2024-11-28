
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Share
{
    public class Class1
    {
        public static void dmtest()
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
            DmSoftClass dm;

            // 创建对象
            try
            {


                dm = new DmSoftClass();
            }
            catch (Exception ex)
            {

                throw;
            }
            // 收费注册
            var regResult = dm.Reg(Config_DM.DmRegCode, Config_DM.DmVerInfo);
            Console.WriteLine($"收费注册返回：{regResult}");
            if (regResult != 1)
            {
                throw new Exception("收费注册失败");
            }
            //591300
            //984470
            //int i= dm.BindWindow(7479138,
            //     "gdi",
            //     "windows",
            //     "windows",
            //     0);
            int dm_ret = dm.BindWindowEx(465066, "dx2", "windows", "windows", "dx.mouse.position.lock.api", 0);

            dm.MoveTo(68, 222);
            Thread.Sleep(500);
            dm.LeftClick();
        }


    }
}
