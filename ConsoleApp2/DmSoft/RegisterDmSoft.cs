﻿using System.Runtime.InteropServices;

namespace AutoScript.Share
{
    /// <summary>
    /// 免注册调用大漠插件
    /// </summary>
    public static class RegisterDmSoft
    {
        // 不注册调用大漠插件，实际上是使用 dmreg.dll 来配合实现，这个文件有 2 个导出接口 SetDllPathW 和 SetDllPathA。 SetDllPathW 对应 unicode，SetDllPathA 对应 ascii 接口。
        [DllImport(Config_DM.DmRegDllPath)]
        private static extern int SetDllPathA(string path, int mode);

        /// <summary>
        /// 免注册调用大漠插件
        /// </summary>ss
        /// <returns></returns>
        public static bool RegisterDmSoftDll()
        {
            var setDllPathResult = SetDllPathA(Config_DM.DmClassDllPath, 1);
            if (setDllPathResult == 0)
            {
                // 加载 dm.dll 失败
                return false;
            }

            return true;
        }
    }
}
