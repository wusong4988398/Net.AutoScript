using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Share
{
    public struct EmumInfo
    {
        public int Pid;
        public IntPtr ProcessHandle;
        public IntPtr Hwnd;
        public string ADBPort;
        public string WindowText;
        public IntPtr StartAddress;
        public IntPtr EndAddress;
    }
    public struct WindowInfo
    {
        public IntPtr hWnd;
        public string szWindowName;
        public string szClassName;
    }

    public class API
    {
        //------------窗口查找 Start----------------------------------------------//
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);
        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, WNDENUMPROC lpEnumFunc, int lParam);
        [DllImport("user32")]
        private static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);
        private delegate bool CallBack(IntPtr hwnd, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        //------------窗口查找 End----------------------------------------------//
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, StringBuilder lpszOp, StringBuilder lpszFile, StringBuilder lpszParams, StringBuilder lpszDir, int FsShowCmd);
        //----------------INI文件读写 Start-------------------//
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //----------------INI文件读写 End-------------------//

        //----------------------内存读写 START-----------------------//
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll ")]
        public static extern bool CloseHandle(int hProcess);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }
        public const int MEM_COMMIT = 0x1000;       //已物理分配
        public const int MEM_PRIVATE = 0x20000;
        public const int PAGE_READWRITE = 0x04;     //可读写内存


        public const int PAGE_READONLY = 0x02;
        public const int PAGE_WRITECOPY = 0x08;
        public const int PAGE_EXECUTE = 0x10;
        public const int PAGE_EXECUTE_READ = 0x20;
        public const int PAGE_EXECUTE_READWRITE = 0x40;
        public const int PAGE_EXECUTE_WRITECOPY = 0x80;
        public const int SEC_COMMIT = 0x8000000;
        public const int SEC_IMAGE = 0x1000000;
        public const int SEC_NOCACHE = 0x10000000;
        public const int SEC_RESERVE = 0x4000000;
        public const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        public const int PROCESS_VM_READ = 0x0010;
        public const int PROCESS_VM_WRITE = 0x0020;


        const int INVALID_HANDLE_VALUE = -1;
        [DllImport("kernel32.dll")] //声明API函数
        public static extern int VirtualAllocEx(IntPtr hwnd, int lpaddress, int size, int type, int tect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int VirtualQueryEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            out MEMORY_BASIC_INFORMATION lpBuffer,
            uint dwLength);
        [DllImport("msvcrt.dll")]
        public static extern IntPtr Memcmp(byte[] b1, byte[] b2, IntPtr count);

        //public static IntPtr SearchBytes(byte[] src, )
        public struct FeatureCode
        {
            public string FeatureCode1;
            public string FeatureCode2;
            public int BetweenFeatureCode;
            public int Offset;
            public int SearchTimes;
            public IntPtr StartAddress;
            public IntPtr EndAddress;
        }

        public static int IndexOf(byte[] srcBytes, byte[] searchBytes)
        {
            if (srcBytes == null) { return -1; }
            if (searchBytes == null) { return -1; }
            if (srcBytes.Length == 0) { return -1; }
            if (searchBytes.Length == 0) { return -1; }
            if (srcBytes.Length < searchBytes.Length) { return -1; }
            for (int i = 0; i < srcBytes.Length - searchBytes.Length; i++)
            {
                if (srcBytes[i] == searchBytes[0])
                {
                    if (searchBytes.Length == 1) { return i; }
                    bool flag = true;
                    for (int j = 1; j < searchBytes.Length; j++)
                    {
                        if (srcBytes[i + j] != searchBytes[j])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag) { return i; }
                }
            }
            return -1;
        }
        /// <summary>
        /// 特征码查找
        /// </summary>
        /// <param name="pid">进程ID</param>
        /// <param name="fCode">特征码</param>
        /// <returns>内存地址</returns>
        public static IntPtr SearchByFeatureCode(int pid, FeatureCode fCode)
        {
            IntPtr procHandle;
            procHandle = OpenProcess(PROCESS_ALL_ACCESS, 0, (uint)pid);
            if (procHandle.ToInt32() <= 0)
            {
                return new IntPtr(0);
            }
            byte[] fcode1 = StrToToHexByte(fCode.FeatureCode1);
            byte[] fcode2 = StrToToHexByte(fCode.FeatureCode2);
            byte[] bytes = new byte[4096];
            IntPtr BaseAddress = fCode.StartAddress;
            int index = 0;
            int times = 0;
            while (BaseAddress.ToInt32() < fCode.EndAddress.ToInt32())
            {
                ReadProcessMemory(procHandle, BaseAddress, bytes, (uint)(4096 + fcode1.Length + fCode.BetweenFeatureCode + fcode2.Length), (IntPtr)0);
                index = IndexOf(bytes, fcode1);
                if (index >= 0)
                {
                    times++;
                    if (times >= fCode.SearchTimes)
                    {
                        break;
                    }
                }
                BaseAddress = new IntPtr(BaseAddress.ToInt32() + 4096);
            }
            CloseHandle(procHandle);
            BaseAddress = new IntPtr(BaseAddress.ToInt32() + index + fCode.Offset);
            return new IntPtr(0);
        }

        public static IntPtr GetAddressByFeatureCode(int pid, string featureCode, int offset, IntPtr startAddr, IntPtr endAddr)
        {
            if (pid <= 0)
            {
                return new IntPtr(0);
            }
            byte[] fcode = Encoding.UTF8.GetBytes(featureCode);
            byte[] bytes = new byte[4096];
            IntPtr pHandle = OpenProcess(PROCESS_ALL_ACCESS, 0, (uint)pid);
            IntPtr BaseAddress = startAddr;
            ulong baseAddr = (ulong)startAddr.ToInt64();
            int index = 0;
            while (baseAddr < (ulong)endAddr.ToInt64())
            {
                VirtualQueryEx(pHandle, (IntPtr)baseAddr, out MEMORY_BASIC_INFORMATION MI, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
                if (MI.State == MEM_COMMIT)
                {
                    ReadProcessMemory(pHandle, (IntPtr)baseAddr, bytes, (uint)(MI.RegionSize), (IntPtr)0);
                    index = IndexOf(bytes, fcode);
                    if (index >= 0)
                    {
                        baseAddr = (ulong)MI.BaseAddress + (ulong)index;
                        break;
                    }

                }
                baseAddr = (ulong)MI.BaseAddress + (ulong)MI.RegionSize;
            }
            return (IntPtr)baseAddr;
        }
        public static IntPtr GetAddressByFeatureCode(int pid, byte[] featureCode, int offset, IntPtr startAddr, IntPtr endAddr)
        {
            if (pid <= 0)
            {
                return new IntPtr(0);
            }
            //byte[] fcode = Encoding.UTF8.GetBytes(featureCode);
            byte[] bytes;
            IntPtr pHandle = OpenProcess(PROCESS_ALL_ACCESS, 0, (uint)pid);
            //IntPtr BaseAddress = startAddr;
            ulong baseAddr = (ulong)startAddr.ToInt32();
            int index = 0;
            while (baseAddr < (ulong)endAddr.ToInt32())
            {
                VirtualQueryEx(pHandle, (IntPtr)baseAddr, out MEMORY_BASIC_INFORMATION MI, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
                if (MI.State == MEM_COMMIT || MI.State == PAGE_READWRITE)
                {
                    bytes = new byte[(uint)(MI.RegionSize)];
                    ReadProcessMemory(pHandle, (IntPtr)baseAddr, bytes, (uint)(MI.RegionSize), (IntPtr)0);
                    index = IndexOf(bytes, featureCode);
                    if (index >= 0)
                    {
                        baseAddr = (ulong)baseAddr + (ulong)index + (ulong)offset;
                        break;
                    }

                }
                baseAddr = (ulong)MI.BaseAddress + (ulong)MI.RegionSize;
            }

            //while (BaseAddress.ToInt32() < endAddr.ToInt32())
            //{

            //    ReadProcessMemory(pHandle, BaseAddress, bytes, (uint)(4096 + featureCode.Length), (IntPtr)0);
            //    index = IndexOf(bytes, featureCode);
            //    if (index >= 0)
            //    {
            //        break;
            //    }
            //    BaseAddress = new IntPtr(BaseAddress.ToInt32() + 4096);
            //}
            CloseHandle(pHandle);
            //BaseAddress = new IntPtr(BaseAddress.ToInt32() + index + offset);
            return (IntPtr)baseAddr;
            //return BaseAddress;
        }
        public static string GetMemString(int pid, IntPtr addr, int size, Encoding encoding)
        {
            byte[] bytes = new byte[size];
            IntPtr pHandle = OpenProcess(PROCESS_ALL_ACCESS, 0, (uint)pid);
            ReadProcessMemory(pHandle, addr, bytes, (uint)size, (IntPtr)0);
            CloseHandle(pHandle);
            return encoding.GetString(bytes);
        }
        /// <summary>
        /// 十六进制字符串转byte数组
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string HexStringToString(string hs, Encoding encode)
        {
            //以%分割字符串，并去掉空字符
            string[] chars = hs.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        //----------------------内存读写 END-----------------------//
        //public delegate bool CallBack(IntPtr hwnd, int lParam);
        public const int WM_COPYDATA = 0x004A;
        public delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        /// <summary>
        /// 查找所有窗口, 返回句柄
        /// </summary>
        /// <param name="title"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Int32[] FindWindows(String title, String className)
        {
            List<Int32> wndList = new List<Int32>();
            List<WindowInfo> map = new List<WindowInfo>();
            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo w = new WindowInfo();
                //WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                GetWindowTextW(hWnd, sb, sb.Capacity);
                w.hWnd = hWnd;
                w.szWindowName = sb.ToString();
                if (!title.Equals(""))
                {
                    var val = sb.ToString();
                    if (!val.Contains(title))
                    {
                        return true;
                    }
                }
                GetClassNameW(hWnd, sb, sb.Capacity);
                w.szClassName = sb.ToString();
                if (!className.Equals(""))
                {
                    if (!sb.ToString().Contains(className))
                    {
                        return true;
                    }
                }
                wndList.Add(hWnd.ToInt32());
                map.Add(w);
                return true;
            }, 0);
            return wndList.ToArray();
        }
        public static Int32[] FindWindowsEx(String title, String classname)
        {
            List<Int32> wndList = new List<Int32>();

            IntPtr hwnd = IntPtr.Zero;
            do
            {
                hwnd = FindWindowEx(IntPtr.Zero, hwnd, classname, title);
                if (hwnd == IntPtr.Zero)
                {
                    break;
                }
                wndList.Add(hwnd.ToInt32());
            } while (hwnd != IntPtr.Zero);
            return wndList.ToArray();
        }

        /// <summary>
        /// 查找指定窗口标题和类名的窗口句柄的集合(可查找子窗口,尤其适合模拟器窗口)
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static List<WindowInfo> SearchAllWindows(IntPtr handle, string name, string classname)
        {
            List<WindowInfo> wndList = new List<WindowInfo>();
            Int32[] parentHwnds = FindWindows("", classname);
            for (int i = 0; i < parentHwnds.Length - 1; i++)
            {
                IntPtr hi = new IntPtr(parentHwnds[i]);
                EnumChildWindows(hi, delegate (IntPtr hWnd, IntPtr lParam)
                {
                    WindowInfo wnd = new WindowInfo();
                    StringBuilder sb = new StringBuilder(256);
                    //get hwnd 
                    wnd.hWnd = hWnd;
                    //get window name 
                    GetWindowTextW(hWnd, sb, sb.Capacity);
                    wnd.szWindowName = sb.ToString();
                    //get window class 
                    GetClassNameW(hWnd, sb, sb.Capacity);
                    wnd.szClassName = sb.ToString();
                    //add it into list 
                    wndList.Add(wnd);
                    //wndList.AddRange(SearchAllWindows(hWnd, name, classname));
                    return true;
                }, 0);
            }
            return wndList.Where(it =>
                (name == null
                    || name.Trim() == ""
                    || it.szWindowName.Contains(name))
                && (classname == null
                    || classname.Trim() == ""
                    || it.szClassName.Contains(classname)))
                .GroupBy(t => t.hWnd)
                .Select(t => t.First())
                .ToList();
        }

        public List<WindowInfo> GetAllDesktopWindows(string name, string classname)
        {
            List<WindowInfo> wndList = new List<WindowInfo>();

            //enum all desktop windows 
            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                //get hwnd 
                wnd.hWnd = hWnd;
                //get window name 
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                //get window class 
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                //add it into list 
                wndList.Add(wnd);
                return true;
            }, 0);

            return wndList.Where(it => it.szWindowName == name && it.szClassName == classname).ToList();
        }
        /// <summary>
        /// 查找指定窗口下的子窗口句柄
        /// </summary>
        /// <param name="hwndParent">父窗口句柄</param>
        /// <param name="className">窗口类名</param>
        /// <param name="caption">窗口标题</param>
        /// <returns></returns>
        //public static IntPtr FindChildWindow(IntPtr hwndParent, string className, string caption)
        //{
        //    IntPtr hwnd = IntPtr.Zero;
        //    int i = EnumChildWindows(hwndParent, (h, l) =>
        //    {
        //        hwnd = FindWindowEx(h, IntPtr.Zero, className, caption);
        //        return hwnd == IntPtr.Zero;
        //    }, 0);
        //    return hwnd;
        //}
        public static IntPtr FindChildWindow(IntPtr hwndParent, string classname, string caption)
        {
            List<WindowInfo> wndList = new List<WindowInfo>();
            EnumChildWindows(hwndParent, delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                //get hwnd 
                wnd.hWnd = hWnd;
                //get window name 
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                //get window class 
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                //add it into list 
                wndList.Add(wnd);
                return true;
            }, 0);
            IntPtr ret = IntPtr.Zero;
            if (wndList.Where(it => it.szWindowName == caption && it.szClassName == classname).ToList().Count > 0)
            {
                ret = wndList.Where(it => it.szWindowName == caption && it.szClassName == classname).ToList()[0].hWnd;
            }
            return ret;
        }
    }
}
