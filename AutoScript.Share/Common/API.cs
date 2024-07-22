using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
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
        //Const
        public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
        //---------------GDI API-----------------------------------------------//
        [DllImport("gdi32.dll")]
        public static extern int BitBlt(
          IntPtr hdcDest, // handle to destination DC目标设备的句柄
          int nXDest,   // x-coord of destination upper-left corner目标对象的左上角的X坐标
          int nYDest,   // y-coord of destination upper-left corner目标对象的左上角的Y坐标
          int nWidth,   // width of destination rectangle目标对象的矩形宽度
          int nHeight, // height of destination rectangle目标对象的矩形长度
          IntPtr hdcSrc,   // handle to source DC源设备的句柄
          int nXSrc,    // x-coordinate of source upper-left corner源对象的左上角的X坐标
          int nYSrc,    // y-coordinate of source upper-left corner源对象的左上角的Y坐标
          UInt32 dwRop   // raster operation code光栅的操作值
          );

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(
         IntPtr hdc // handle to DC
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
         IntPtr hdc,         // handle to DC
         int nWidth,      // width of bitmap, in pixels
         int nHeight      // height of bitmap, in pixels
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(
         IntPtr hdc,           // handle to DC
         IntPtr hgdiobj    // handle to object
         );

        [DllImport("gdi32.dll")]
        public static extern int DeleteDC(
         IntPtr hdc           // handle to DC
         );

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(
         IntPtr hwnd,                // Window to copy,Handle to the window that will be copied.
         IntPtr hdcBlt,              // HDC to print into,Handle to the device context.
         UInt32 nFlags               // Optional flags,Specifies the drawing options. It can be one of the following values.
         );

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(
         IntPtr hwnd
         );

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool TransparentBlt(
            IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int hHeightDest,
            IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int hHeightSrc,
            int crTransparent);


        [DllImport("gdi32.dll")]
        public static extern bool PatBlt(IntPtr hDC, int XLeft, int YLeft, int Width, int Height, uint Rop);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hDC, int XPos, int YPos);

        [DllImport("gdi32.dll")]
        public static extern int SetMapMode(IntPtr hDC, int fnMapMode);

        [DllImport("gdi32.dll")]
        public static extern int GetObjectType(IntPtr handle);
        //------------窗口查找 Start----------------------------------------------//
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);
        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, WNDENUMPROC lpEnumFunc, int lParam);
        [DllImport("user32")]
        private static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);
        private delegate bool CallBack(IntPtr hwnd, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        //-------------------------------窗口设备捕获----------------------------//
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, string lparam);
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        /// <summary>
        /// 获取窗体的句柄函数
        /// </summary>
        /// <param name="lpClassName">窗口类名</param>
        /// <param name="lpWindowName">窗口标题名</param>
        /// <returns>返回句柄</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 通过句柄，窗体显示函数
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="cmdShow">显示方式0：隐藏窗体，1：默认窗体，2：最小化窗体，3：最大化窗体</param>
        /// <returns>返工成功与否</returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindowAsync", SetLastError = true)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary>    
        /// 该函数检索一指定窗口的客户区域或整个屏幕的显示设备上下文环境的句柄，以后可以在GDI函数中使用该句柄来在设备上下文环境中绘图。hWnd：设备上下文环境被检索的窗口的句柄    
        /// </summary>    
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr hWnd);
        ///// <summary>    
        ///// 该函数设置指定窗口的显示状态。    
        ///// </summary>    
        [DllImport("user32.dll")]
        static public extern bool ShowWindow(IntPtr hWnd, short State);

        /// <summary>    
        /// 该函数返回指定窗口的边框矩形的尺寸。该尺寸以相对于屏幕坐标左上角的屏幕坐标给出。    
        /// </summary>
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, ref RECT rect);
        // 热键
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, Keys vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
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

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
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
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

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
        const int PROCESS_WM_READ = 0x0010;
        public static void GetAddressByFeatureCode2(int pid)
        {
            // 获取目标进程
            Process process = Process.GetProcessById(pid);
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);

            // 定义要搜索的特征码
            byte[] pattern = new byte[] { 0xC0, 0x72, 0xFF, 0x05, 0x4B, 0x02, 0x00, 0x00 };

            // 定义读取内存的缓冲区
            byte[] buffer = new byte[4096];
            int bytesRead;

            // 遍历进程的内存
            for (IntPtr address = IntPtr.Zero; address.ToInt64() < 0x7fffffff; address = IntPtr.Add(address, buffer.Length))
            {
                if (ReadProcessMemory(processHandle, address, buffer, buffer.Length, out bytesRead))
                {
                    // 在缓冲区中搜索特征码
                    for (int i = 0; i < bytesRead - pattern.Length; i++)
                    {
                        bool found = true;
                        for (int j = 0; j < pattern.Length; j++)
                        {
                            if (buffer[i + j] != pattern[j])
                            {
                                found = false;
                                break;
                            }
                        }

                        if (found)
                        {
                            Console.WriteLine($"Pattern found at address: 0x{address.ToInt64() + i:X}");
                        }
                    }
                }
            }

            CloseHandle(processHandle);
        }

        public static IntPtr GetAddressByFeatureCode(int pid, byte[] featureCode, long offset, IntPtr startAddr, IntPtr endAddr)
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

    public enum Keys
    {
        //
        // Summary:
        //     The bitmask to extract a key code from a key value.
        KeyCode = 0xFFFF,
        //
        // Summary:
        //     The bitmask to extract modifiers from a key value.
        Modifiers = -65536,
        //
        // Summary:
        //     No key pressed.
        None = 0,
        //
        // Summary:
        //     The left mouse button.
        LButton = 1,
        //
        // Summary:
        //     The right mouse button.
        RButton = 2,
        //
        // Summary:
        //     The CANCEL key.
        Cancel = 3,
        //
        // Summary:
        //     The middle mouse button (three-button mouse).
        MButton = 4,
        //
        // Summary:
        //     The first x mouse button (five-button mouse).
        XButton1 = 5,
        //
        // Summary:
        //     The second x mouse button (five-button mouse).
        XButton2 = 6,
        //
        // Summary:
        //     The BACKSPACE key.
        Back = 8,
        //
        // Summary:
        //     The TAB key.
        Tab = 9,
        //
        // Summary:
        //     The LINEFEED key.
        LineFeed = 0xA,
        //
        // Summary:
        //     The CLEAR key.
        Clear = 0xC,
        //
        // Summary:
        //     The RETURN key.
        Return = 0xD,
        //
        // Summary:
        //     The ENTER key.
        Enter = 0xD,
        //
        // Summary:
        //     The SHIFT key.
        ShiftKey = 0x10,
        //
        // Summary:
        //     The CTRL key.
        ControlKey = 0x11,
        //
        // Summary:
        //     The ALT key.
        Menu = 0x12,
        //
        // Summary:
        //     The PAUSE key.
        Pause = 0x13,
        //
        // Summary:
        //     The CAPS LOCK key.
        Capital = 0x14,
        //
        // Summary:
        //     The CAPS LOCK key.
        CapsLock = 0x14,
        //
        // Summary:
        //     The IME Kana mode key.
        KanaMode = 0x15,
        //
        // Summary:
        //     The IME Hanguel mode key. (maintained for compatibility; use HangulMode)
        HanguelMode = 0x15,
        //
        // Summary:
        //     The IME Hangul mode key.
        HangulMode = 0x15,
        //
        // Summary:
        //     The IME Junja mode key.
        JunjaMode = 0x17,
        //
        // Summary:
        //     The IME final mode key.
        FinalMode = 0x18,
        //
        // Summary:
        //     The IME Hanja mode key.
        HanjaMode = 0x19,
        //
        // Summary:
        //     The IME Kanji mode key.
        KanjiMode = 0x19,
        //
        // Summary:
        //     The ESC key.
        Escape = 0x1B,
        //
        // Summary:
        //     The IME convert key.
        IMEConvert = 0x1C,
        //
        // Summary:
        //     The IME nonconvert key.
        IMENonconvert = 0x1D,
        //
        // Summary:
        //     The IME accept key, replaces System.Windows.Forms.Keys.IMEAceept.
        IMEAccept = 0x1E,
        //
        // Summary:
        //     The IME accept key. Obsolete, use System.Windows.Forms.Keys.IMEAccept instead.
        IMEAceept = 0x1E,
        //
        // Summary:
        //     The IME mode change key.
        IMEModeChange = 0x1F,
        //
        // Summary:
        //     The SPACEBAR key.
        Space = 0x20,
        //
        // Summary:
        //     The PAGE UP key.
        Prior = 0x21,
        //
        // Summary:
        //     The PAGE UP key.
        PageUp = 0x21,
        //
        // Summary:
        //     The PAGE DOWN key.
        Next = 0x22,
        //
        // Summary:
        //     The PAGE DOWN key.
        PageDown = 0x22,
        //
        // Summary:
        //     The END key.
        End = 0x23,
        //
        // Summary:
        //     The HOME key.
        Home = 0x24,
        //
        // Summary:
        //     The LEFT ARROW key.
        Left = 0x25,
        //
        // Summary:
        //     The UP ARROW key.
        Up = 0x26,
        //
        // Summary:
        //     The RIGHT ARROW key.
        Right = 0x27,
        //
        // Summary:
        //     The DOWN ARROW key.
        Down = 0x28,
        //
        // Summary:
        //     The SELECT key.
        Select = 0x29,
        //
        // Summary:
        //     The PRINT key.
        Print = 0x2A,
        //
        // Summary:
        //     The EXECUTE key.
        Execute = 0x2B,
        //
        // Summary:
        //     The PRINT SCREEN key.
        Snapshot = 0x2C,
        //
        // Summary:
        //     The PRINT SCREEN key.
        PrintScreen = 0x2C,
        //
        // Summary:
        //     The INS key.
        Insert = 0x2D,
        //
        // Summary:
        //     The DEL key.
        Delete = 0x2E,
        //
        // Summary:
        //     The HELP key.
        Help = 0x2F,
        //
        // Summary:
        //     The 0 key.
        D0 = 0x30,
        //
        // Summary:
        //     The 1 key.
        D1 = 0x31,
        //
        // Summary:
        //     The 2 key.
        D2 = 0x32,
        //
        // Summary:
        //     The 3 key.
        D3 = 0x33,
        //
        // Summary:
        //     The 4 key.
        D4 = 0x34,
        //
        // Summary:
        //     The 5 key.
        D5 = 0x35,
        //
        // Summary:
        //     The 6 key.
        D6 = 0x36,
        //
        // Summary:
        //     The 7 key.
        D7 = 0x37,
        //
        // Summary:
        //     The 8 key.
        D8 = 0x38,
        //
        // Summary:
        //     The 9 key.
        D9 = 0x39,
        //
        // Summary:
        //     The A key.
        A = 0x41,
        //
        // Summary:
        //     The B key.
        B = 0x42,
        //
        // Summary:
        //     The C key.
        C = 0x43,
        //
        // Summary:
        //     The D key.
        D = 0x44,
        //
        // Summary:
        //     The E key.
        E = 0x45,
        //
        // Summary:
        //     The F key.
        F = 0x46,
        //
        // Summary:
        //     The G key.
        G = 0x47,
        //
        // Summary:
        //     The H key.
        H = 0x48,
        //
        // Summary:
        //     The I key.
        I = 0x49,
        //
        // Summary:
        //     The J key.
        J = 0x4A,
        //
        // Summary:
        //     The K key.
        K = 0x4B,
        //
        // Summary:
        //     The L key.
        L = 0x4C,
        //
        // Summary:
        //     The M key.
        M = 0x4D,
        //
        // Summary:
        //     The N key.
        N = 0x4E,
        //
        // Summary:
        //     The O key.
        O = 0x4F,
        //
        // Summary:
        //     The P key.
        P = 0x50,
        //
        // Summary:
        //     The Q key.
        Q = 0x51,
        //
        // Summary:
        //     The R key.
        R = 0x52,
        //
        // Summary:
        //     The S key.
        S = 0x53,
        //
        // Summary:
        //     The T key.
        T = 0x54,
        //
        // Summary:
        //     The U key.
        U = 0x55,
        //
        // Summary:
        //     The V key.
        V = 0x56,
        //
        // Summary:
        //     The W key.
        W = 0x57,
        //
        // Summary:
        //     The X key.
        X = 0x58,
        //
        // Summary:
        //     The Y key.
        Y = 0x59,
        //
        // Summary:
        //     The Z key.
        Z = 0x5A,
        //
        // Summary:
        //     The left Windows logo key (Microsoft Natural Keyboard).
        LWin = 0x5B,
        //
        // Summary:
        //     The right Windows logo key (Microsoft Natural Keyboard).
        RWin = 0x5C,
        //
        // Summary:
        //     The application key (Microsoft Natural Keyboard).
        Apps = 0x5D,
        //
        // Summary:
        //     The computer sleep key.
        Sleep = 0x5F,
        //
        // Summary:
        //     The 0 key on the numeric keypad.
        NumPad0 = 0x60,
        //
        // Summary:
        //     The 1 key on the numeric keypad.
        NumPad1 = 0x61,
        //
        // Summary:
        //     The 2 key on the numeric keypad.
        NumPad2 = 0x62,
        //
        // Summary:
        //     The 3 key on the numeric keypad.
        NumPad3 = 0x63,
        //
        // Summary:
        //     The 4 key on the numeric keypad.
        NumPad4 = 0x64,
        //
        // Summary:
        //     The 5 key on the numeric keypad.
        NumPad5 = 0x65,
        //
        // Summary:
        //     The 6 key on the numeric keypad.
        NumPad6 = 0x66,
        //
        // Summary:
        //     The 7 key on the numeric keypad.
        NumPad7 = 0x67,
        //
        // Summary:
        //     The 8 key on the numeric keypad.
        NumPad8 = 0x68,
        //
        // Summary:
        //     The 9 key on the numeric keypad.
        NumPad9 = 0x69,
        //
        // Summary:
        //     The multiply key.
        Multiply = 0x6A,
        //
        // Summary:
        //     The add key.
        Add = 0x6B,
        //
        // Summary:
        //     The separator key.
        Separator = 0x6C,
        //
        // Summary:
        //     The subtract key.
        Subtract = 0x6D,
        //
        // Summary:
        //     The decimal key.
        Decimal = 0x6E,
        //
        // Summary:
        //     The divide key.
        Divide = 0x6F,
        //
        // Summary:
        //     The F1 key.
        F1 = 0x70,
        //
        // Summary:
        //     The F2 key.
        F2 = 0x71,
        //
        // Summary:
        //     The F3 key.
        F3 = 0x72,
        //
        // Summary:
        //     The F4 key.
        F4 = 0x73,
        //
        // Summary:
        //     The F5 key.
        F5 = 0x74,
        //
        // Summary:
        //     The F6 key.
        F6 = 0x75,
        //
        // Summary:
        //     The F7 key.
        F7 = 0x76,
        //
        // Summary:
        //     The F8 key.
        F8 = 0x77,
        //
        // Summary:
        //     The F9 key.
        F9 = 0x78,
        //
        // Summary:
        //     The F10 key.
        F10 = 0x79,
        //
        // Summary:
        //     The F11 key.
        F11 = 0x7A,
        //
        // Summary:
        //     The F12 key.
        F12 = 0x7B,
        //
        // Summary:
        //     The F13 key.
        F13 = 0x7C,
        //
        // Summary:
        //     The F14 key.
        F14 = 0x7D,
        //
        // Summary:
        //     The F15 key.
        F15 = 0x7E,
        //
        // Summary:
        //     The F16 key.
        F16 = 0x7F,
        //
        // Summary:
        //     The F17 key.
        F17 = 0x80,
        //
        // Summary:
        //     The F18 key.
        F18 = 0x81,
        //
        // Summary:
        //     The F19 key.
        F19 = 0x82,
        //
        // Summary:
        //     The F20 key.
        F20 = 0x83,
        //
        // Summary:
        //     The F21 key.
        F21 = 0x84,
        //
        // Summary:
        //     The F22 key.
        F22 = 0x85,
        //
        // Summary:
        //     The F23 key.
        F23 = 0x86,
        //
        // Summary:
        //     The F24 key.
        F24 = 0x87,
        //
        // Summary:
        //     The NUM LOCK key.
        NumLock = 0x90,
        //
        // Summary:
        //     The SCROLL LOCK key.
        Scroll = 0x91,
        //
        // Summary:
        //     The left SHIFT key.
        LShiftKey = 0xA0,
        //
        // Summary:
        //     The right SHIFT key.
        RShiftKey = 0xA1,
        //
        // Summary:
        //     The left CTRL key.
        LControlKey = 0xA2,
        //
        // Summary:
        //     The right CTRL key.
        RControlKey = 0xA3,
        //
        // Summary:
        //     The left ALT key.
        LMenu = 0xA4,
        //
        // Summary:
        //     The right ALT key.
        RMenu = 0xA5,
        //
        // Summary:
        //     The browser back key (Windows 2000 or later).
        BrowserBack = 0xA6,
        //
        // Summary:
        //     The browser forward key (Windows 2000 or later).
        BrowserForward = 0xA7,
        //
        // Summary:
        //     The browser refresh key (Windows 2000 or later).
        BrowserRefresh = 0xA8,
        //
        // Summary:
        //     The browser stop key (Windows 2000 or later).
        BrowserStop = 0xA9,
        //
        // Summary:
        //     The browser search key (Windows 2000 or later).
        BrowserSearch = 0xAA,
        //
        // Summary:
        //     The browser favorites key (Windows 2000 or later).
        BrowserFavorites = 0xAB,
        //
        // Summary:
        //     The browser home key (Windows 2000 or later).
        BrowserHome = 0xAC,
        //
        // Summary:
        //     The volume mute key (Windows 2000 or later).
        VolumeMute = 0xAD,
        //
        // Summary:
        //     The volume down key (Windows 2000 or later).
        VolumeDown = 0xAE,
        //
        // Summary:
        //     The volume up key (Windows 2000 or later).
        VolumeUp = 0xAF,
        //
        // Summary:
        //     The media next track key (Windows 2000 or later).
        MediaNextTrack = 0xB0,
        //
        // Summary:
        //     The media previous track key (Windows 2000 or later).
        MediaPreviousTrack = 0xB1,
        //
        // Summary:
        //     The media Stop key (Windows 2000 or later).
        MediaStop = 0xB2,
        //
        // Summary:
        //     The media play pause key (Windows 2000 or later).
        MediaPlayPause = 0xB3,
        //
        // Summary:
        //     The launch mail key (Windows 2000 or later).
        LaunchMail = 0xB4,
        //
        // Summary:
        //     The select media key (Windows 2000 or later).
        SelectMedia = 0xB5,
        //
        // Summary:
        //     The start application one key (Windows 2000 or later).
        LaunchApplication1 = 0xB6,
        //
        // Summary:
        //     The start application two key (Windows 2000 or later).
        LaunchApplication2 = 0xB7,
        //
        // Summary:
        //     The OEM Semicolon key on a US standard keyboard (Windows 2000 or later).
        OemSemicolon = 0xBA,
        //
        // Summary:
        //     The OEM 1 key.
        Oem1 = 0xBA,
        //
        // Summary:
        //     The OEM plus key on any country/region keyboard (Windows 2000 or later).
        Oemplus = 0xBB,
        //
        // Summary:
        //     The OEM comma key on any country/region keyboard (Windows 2000 or later).
        Oemcomma = 0xBC,
        //
        // Summary:
        //     The OEM minus key on any country/region keyboard (Windows 2000 or later).
        OemMinus = 0xBD,
        //
        // Summary:
        //     The OEM period key on any country/region keyboard (Windows 2000 or later).
        OemPeriod = 0xBE,
        //
        // Summary:
        //     The OEM question mark key on a US standard keyboard (Windows 2000 or later).
        OemQuestion = 0xBF,
        //
        // Summary:
        //     The OEM 2 key.
        Oem2 = 0xBF,
        //
        // Summary:
        //     The OEM tilde key on a US standard keyboard (Windows 2000 or later).
        Oemtilde = 0xC0,
        //
        // Summary:
        //     The OEM 3 key.
        Oem3 = 0xC0,
        //
        // Summary:
        //     The OEM open bracket key on a US standard keyboard (Windows 2000 or later).
        OemOpenBrackets = 0xDB,
        //
        // Summary:
        //     The OEM 4 key.
        Oem4 = 0xDB,
        //
        // Summary:
        //     The OEM pipe key on a US standard keyboard (Windows 2000 or later).
        OemPipe = 0xDC,
        //
        // Summary:
        //     The OEM 5 key.
        Oem5 = 0xDC,
        //
        // Summary:
        //     The OEM close bracket key on a US standard keyboard (Windows 2000 or later).
        OemCloseBrackets = 0xDD,
        //
        // Summary:
        //     The OEM 6 key.
        Oem6 = 0xDD,
        //
        // Summary:
        //     The OEM singled/double quote key on a US standard keyboard (Windows 2000 or later).
        OemQuotes = 0xDE,
        //
        // Summary:
        //     The OEM 7 key.
        Oem7 = 0xDE,
        //
        // Summary:
        //     The OEM 8 key.
        Oem8 = 0xDF,
        //
        // Summary:
        //     The OEM angle bracket or backslash key on the RT 102 key keyboard (Windows 2000
        //     or later).
        OemBackslash = 0xE2,
        //
        // Summary:
        //     The OEM 102 key.
        Oem102 = 0xE2,
        //
        // Summary:
        //     The PROCESS KEY key.
        ProcessKey = 0xE5,
        //
        // Summary:
        //     Used to pass Unicode characters as if they were keystrokes. The Packet key value
        //     is the low word of a 32-bit virtual-key value used for non-keyboard input methods.
        Packet = 0xE7,
        //
        // Summary:
        //     The ATTN key.
        Attn = 0xF6,
        //
        // Summary:
        //     The CRSEL key.
        Crsel = 0xF7,
        //
        // Summary:
        //     The EXSEL key.
        Exsel = 0xF8,
        //
        // Summary:
        //     The ERASE EOF key.
        EraseEof = 0xF9,
        //
        // Summary:
        //     The PLAY key.
        Play = 0xFA,
        //
        // Summary:
        //     The ZOOM key.
        Zoom = 0xFB,
        //
        // Summary:
        //     A constant reserved for future use.
        NoName = 0xFC,
        //
        // Summary:
        //     The PA1 key.
        Pa1 = 0xFD,
        //
        // Summary:
        //     The CLEAR key.
        OemClear = 0xFE,
        //
        // Summary:
        //     The SHIFT modifier key.
        Shift = 0x10000,
        //
        // Summary:
        //     The CTRL modifier key.
        Control = 0x20000,
        //
        // Summary:
        //     The ALT modifier key.
        Alt = 0x40000
    }
}
