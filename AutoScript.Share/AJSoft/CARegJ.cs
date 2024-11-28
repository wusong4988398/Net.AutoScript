

using System;
using System.Reflection;
using System.Runtime.InteropServices;

#pragma warning disable CA1416
#pragma warning disable CS8600
#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8605

namespace AutoScript.Share
{
    public class ARegJ
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpLibFileName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        delegate int dSetDllPathA(string DllPath, int Type); static dSetDllPathA fnSetDllPathA = null;

        public static int SetDllPathA(string ARegJPath, string AoJiaPath)
        {
            int ir = 0;
            if (fnSetDllPathA == null)
            {
                IntPtr hDll = LoadLibrary(ARegJPath);
                if (hDll != IntPtr.Zero)
                {
                    IntPtr pSetDllPathA = GetProcAddress(hDll, "SetDllPathA");
                    if (pSetDllPathA != IntPtr.Zero)
                    {
                        fnSetDllPathA = (dSetDllPathA)Marshal.GetDelegateForFunctionPointer(pSetDllPathA, typeof(dSetDllPathA));
                    }
                }
            }
            if (fnSetDllPathA != null)
            {
                ir = fnSetDllPathA(AoJiaPath, 0);
            }
            return ir;
        }
    }

    public class AoJia
    {
        public int ir; Type AJT; object AJ;

        public AoJia()
        {
            //函数GetTypeFromProgID和CreateInstance会去注册表查询插件的信息,如果查询不到就不能创建对象
            //如果不想使用免注册调用插件,可以先向系统注册表注册插件,然后调用这个无参构造函数创建对象
            ir = 0; AJT = Type.GetTypeFromProgID("AoJia.AoJiaD");
            if (AJT != null)
            {
                AJ = Activator.CreateInstance(AJT);
                if (AJ != null)
                {
                    ir = 1;
                }
            }
            else
            {
                AJ = null;
            }
        }

        public AoJia(string ARegJPath, string AoJiaPath)
        {
            ARegJ.SetDllPathA(ARegJPath, AoJiaPath);//有了这句代码就是免注册调用插件,即使注册表没有插件的信息也能创建对象
            ir = 0; AJT = Type.GetTypeFromProgID("AoJia.AoJiaD");
            if (AJT != null)
            {
                AJ = Activator.CreateInstance(AJT);
                if (AJ != null)
                {
                    ir = 1;
                }
            }
            else
            {
                AJ = null;
            }
        }

        ~AoJia()
        {
            Release();
        }

        public void Release()//可以调用这个函数主动释放创建的对象
        {
            if (AJ != null)
            {
                Marshal.ReleaseComObject(AJ); AJ = null;
            }
        }

        public string VerS()
        {
            object rt = AJT.InvokeMember("VerS", BindingFlags.InvokeMethod, null, AJ, null);
            return rt.ToString();
        }

        public int SetPath(string Path)
        {
            object[] args = new object[1]; args[0] = Path;
            object rt = AJT.InvokeMember("SetPath", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetErrorMsg(int Msg)
        {
            object[] args = new object[1]; args[0] = Msg;
            object rt = AJT.InvokeMember("SetErrorMsg", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetThread(int TN)
        {
            object[] args = new object[1]; args[0] = TN;
            object rt = AJT.InvokeMember("SetThread", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string GetModulePath(int PID, int Hwnd, string MN, int Type)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = MN; args[3] = Type;
            object rt = AJT.InvokeMember("GetModulePath", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string GetMachineCode()
        {
            object rt = AJT.InvokeMember("GetMachineCode", BindingFlags.InvokeMethod, null, AJ, null);
            return rt.ToString();
        }

        public int GetOs(out string SV, out string SVN, out int LVBN, out string SDir, int Type)
        {
            ParameterModifier p = new ParameterModifier(5); p[0] = true; p[1] = true; p[2] = true; p[3] = true; ParameterModifier[] mods = { p };
            object[] args = new object[5]; args[4] = Type;
            object rt = AJT.InvokeMember("GetOs", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            SV = args[0].ToString(); SVN = args[1].ToString(); LVBN = (int)args[2]; SDir = args[3].ToString();
            return (int)rt;
        }

        public int FindWindow(int Parent, string ProName, int ProID, string Class, string Title, int Type, int T)
        {
            object[] args = new object[7]; args[0] = Parent; args[1] = ProName; args[2] = ProID; args[3] = Class; args[4] = Title;
            args[5] = Type; args[6] = T;
            object rt = AJT.InvokeMember("FindWindow", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int CreateWindows(int x, int y, int Width, int Height, int EWidth, int EHeight, int Type)
        {
            object[] args = new object[7]; args[0] = x; args[1] = y; args[2] = Width; args[3] = Height; args[4] = EWidth;
            args[5] = EHeight; args[6] = Type;
            object rt = AJT.InvokeMember("CreateWindows", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int DeleteFolder(string FN)
        {
            object[] args = new object[1]; args[0] = FN;
            object rt = AJT.InvokeMember("DeleteFolder", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public long GetRemoteProcAddress(int PID, int Hwnd, string MN, string Func)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = MN; args[3] = Func;
            object rt = AJT.InvokeMember("GetRemoteProcAddress", BindingFlags.InvokeMethod, null, AJ, args);
            return (long)rt;
        }

        public int ClientOrScreen(int Hwnd, int xz, int yz, out int x, out int y, int Type)
        {
            ParameterModifier p = new ParameterModifier(6); p[3] = true; p[4] = true; ParameterModifier[] mods = { p };
            object[] args = new object[6]; args[0] = Hwnd; args[1] = xz; args[2] = yz; args[5] = Type;
            object rt = AJT.InvokeMember("ClientOrScreen", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            x = (int)args[3]; y = (int)args[4];
            return (int)rt;
        }

        public int CompressFile(string SF, string DF, int Type, int Level)
        {
            object[] args = new object[4]; args[0] = SF; args[1] = DF; args[2] = Type; args[3] = Level;
            object rt = AJT.InvokeMember("CompressFile", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int UnCompressFile(string SF, string DF, int Type)
        {
            object[] args = new object[3]; args[0] = SF; args[1] = DF; args[2] = Type;
            object rt = AJT.InvokeMember("UnCompressFile", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetFont(int Hwnd, string Name, int Size, int Weight, int Italic, int Underline, int StrikeOut)
        {
            object[] args = new object[7]; args[0] = Hwnd; args[1] = Name; args[2] = Size; args[3] = Weight;
            args[4] = Italic; args[5] = Underline; args[6] = StrikeOut;
            object rt = AJT.InvokeMember("SetFont", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetTextD(int Hwnd, int x1, int y1, int x2, int y2, int Row, int Dir)
        {
            object[] args = new object[7]; args[0] = Hwnd; args[1] = x1; args[2] = y1; args[3] = x2;
            args[4] = y2; args[5] = Row; args[6] = Dir;
            object rt = AJT.InvokeMember("SetTextD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int DrawTextD(int Hwnd, string Text, string Color, string BkColor)
        {
            object[] args = new object[4]; args[0] = Hwnd; args[1] = Text; args[2] = Color; args[3] = BkColor;
            object rt = AJT.InvokeMember("DrawTextD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int CreateFolder(string FN)
        {
            object[] args = new object[1]; args[0] = FN;
            object rt = AJT.InvokeMember("CreateFolder", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string FindFile(string FN)
        {
            object[] args = new object[1]; args[0] = FN;
            object rt = AJT.InvokeMember("FindFile", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int IsFileOrFolder(string FN)
        {
            object[] args = new object[1]; args[0] = FN;
            object rt = AJT.InvokeMember("IsFileOrFolder", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int IsFileOrFolderE(string FN)
        {
            object[] args = new object[1]; args[0] = FN;
            object rt = AJT.InvokeMember("IsFileOrFolderE", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int CopyFile(string SF, string DF, int Type)
        {
            object[] args = new object[3]; args[0] = SF; args[1] = DF; args[2] = Type;
            object rt = AJT.InvokeMember("CopyFile", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int CopyFolder(string SF, string DF, int Type)
        {
            object[] args = new object[3]; args[0] = SF; args[1] = DF; args[2] = Type;
            object rt = AJT.InvokeMember("CopyFolder", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int DeleteFile(string FN)
        {
            object[] args = new object[1]; args[0] = FN;
            object rt = AJT.InvokeMember("DeleteFile", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetPCData(int Type, string PicName)
        {
            object[] args = new object[2]; args[0] = Type; args[1] = PicName;
            object rt = AJT.InvokeMember("SetPCData", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetGlobalDict(int GD)
        {
            object[] args = new object[1]; args[0] = GD;
            object rt = AJT.InvokeMember("SetGlobalDict", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetLastError()
        {
            object rt = AJT.InvokeMember("GetLastError", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int SetPicCache(int PicD)
        {
            object[] args = new object[1]; args[0] = PicD;
            object rt = AJT.InvokeMember("SetPicCache", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string GetPath()
        {
            object rt = AJT.InvokeMember("GetPath", BindingFlags.InvokeMethod, null, AJ, null);
            return rt.ToString();
        }

        public int GetAoJiaID()
        {
            object rt = AJT.InvokeMember("GetAoJiaID", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int GetAoJiaNum()
        {
            object rt = AJT.InvokeMember("GetAoJiaNum", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int SetExcludeArea(int Type, string AreaD)
        {
            object[] args = new object[2]; args[0] = Type; args[1] = AreaD;
            object rt = AJT.InvokeMember("SetExcludeArea", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetPicPw(string Pw)
        {
            object[] args = new object[1]; args[0] = Pw;
            object rt = AJT.InvokeMember("SetPicPw", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetDictPw(string Pw)
        {
            object[] args = new object[1]; args[0] = Pw;
            object rt = AJT.InvokeMember("SetDictPw", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetDesktopArea(int Hwnd, int Dx1, int Dy1, int Dx2, int Dy2, int Flag, int Type)
        {
            object[] args = new object[7]; args[0] = Hwnd; args[1] = Dx1; args[2] = Dy1; args[3] = Dx2; args[4] = Dy2; args[5] = Flag; args[6] = Type;
            object rt = AJT.InvokeMember("SetDesktopArea", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string EnumProcess(string ProName)
        {
            object[] args = new object[1]; args[0] = ProName;
            object rt = AJT.InvokeMember("EnumProcess", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int TerminateProcess(int PID, int Hwnd, string ProName, int Type)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = ProName; args[3] = Type;
            object rt = AJT.InvokeMember("TerminateProcess", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string GetProcessInfo(int PID, int Hwnd)
        {
            object[] args = new object[2]; args[0] = PID; args[1] = Hwnd;
            object rt = AJT.InvokeMember("GetProcessInfo", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int DisableIME(int TID, int Type)
        {
            object[] args = new object[2]; args[0] = TID; args[1] = Type;
            object rt = AJT.InvokeMember("DisableIME", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string EnumThread(int PID, int Hwnd)
        {
            object[] args = new object[2]; args[0] = PID; args[1] = Hwnd;
            object rt = AJT.InvokeMember("EnumThread", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int GetCurrentThreadId()
        {
            object rt = AJT.InvokeMember("GetCurrentThreadId", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int TerminateThread(int TID)
        {
            object[] args = new object[1]; args[0] = TID;
            object rt = AJT.InvokeMember("TerminateThread", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetCurrentProcessId()
        {
            object rt = AJT.InvokeMember("GetCurrentProcessId", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public string GetProcessPath(int PID, int Type)
        {
            object[] args = new object[2]; args[0] = PID; args[1] = Type;
            object rt = AJT.InvokeMember("GetProcessPath", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int GetPTNum(out int PS, out int HC, out int TC)
        {
            ParameterModifier p = new ParameterModifier(3); p[0] = true; p[1] = true; p[2] = true; ParameterModifier[] mods = { p };
            object[] args = new object[3];
            object rt = AJT.InvokeMember("GetPTNum", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            PS = (int)args[0]; HC = (int)args[1]; TC = (int)args[2];
            return (int)rt;
        }

        public string GetCommandLine(int PID, int Hwnd)
        {
            object[] args = new object[2]; args[0] = PID; args[1] = Hwnd;
            object rt = AJT.InvokeMember("GetCommandLine", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string EnumModule(int PID, int Hwnd)
        {
            object[] args = new object[2]; args[0] = PID; args[1] = Hwnd;
            object rt = AJT.InvokeMember("EnumModule", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public long GetModuleBaseAddr(int PID, int Hwnd, string MN)
        {
            object[] args = new object[3]; args[0] = PID; args[1] = Hwnd; args[2] = MN;
            object rt = AJT.InvokeMember("GetModuleBaseAddr", BindingFlags.InvokeMethod, null, AJ, args);
            return (long)rt;
        }

        public uint GetModuleSize(int PID, int Hwnd, string MN)
        {
            object[] args = new object[3]; args[0] = PID; args[1] = Hwnd; args[2] = MN;
            object rt = AJT.InvokeMember("GetModuleSize", BindingFlags.InvokeMethod, null, AJ, args);
            return (uint)rt;
        }

        public int Is64Process(int PID, int Hwnd)
        {
            object[] args = new object[2]; args[0] = PID; args[1] = Hwnd;
            object rt = AJT.InvokeMember("Is64Process", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int FindWindowEx(string Scdt1, int Flag1, int Type1, string Scdt2, int Flag2, int Type2, string Scdt3, int Flag3, int Type3, int Visible, int T)
        {
            object[] args = new object[11]; args[0] = Scdt1; args[1] = Flag1; args[2] = Type1; args[3] = Scdt2; args[4] = Flag2; args[5] = Type2;
            args[6] = Scdt3; args[7] = Flag3; args[8] = Type3; args[9] = Visible; args[10] = T;
            object rt = AJT.InvokeMember("FindWindowEx", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string EnumWindow(int Parent, string ProName, int ProID, string Class, string Title, int Type, int Flag, int T)
        {
            object[] args = new object[8]; args[0] = Parent; args[1] = ProName; args[2] = ProID; args[3] = Class; args[4] = Title; args[5] = Type;
            args[6] = Flag; args[7] = T;
            object rt = AJT.InvokeMember("EnumWindow", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string EnumWindowEx(string Scdt1, int Flag1, int Type1, string Scdt2, int Flag2, int Type2, string Scdt3, int Flag3, int Type3, int Visible, int Sort, int T)
        {
            object[] args = new object[12]; args[0] = Scdt1; args[1] = Flag1; args[2] = Type1; args[3] = Scdt2; args[4] = Flag2; args[5] = Type2;
            args[6] = Scdt3; args[7] = Flag3; args[8] = Type3; args[9] = Visible; args[10] = Sort; args[11] = T;
            object rt = AJT.InvokeMember("EnumWindowEx", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string GetWindowClass(int Hwnd)
        {
            object[] args = new object[1]; args[0] = Hwnd;
            object rt = AJT.InvokeMember("GetWindowClass", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string GetWindowTitle(int Hwnd)
        {
            object[] args = new object[1]; args[0] = Hwnd;
            object rt = AJT.InvokeMember("GetWindowTitle", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int SetWindowTitle(int Hwnd, string Title)
        {
            object[] args = new object[2]; args[0] = Hwnd; args[1] = Title;
            object rt = AJT.InvokeMember("SetWindowTitle", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetClientRect(int Hwnd, out int x1, out int y1, out int x2, out int y2)
        {
            ParameterModifier p = new ParameterModifier(5); p[1] = true; p[2] = true; p[3] = true; p[4] = true; ParameterModifier[] mods = { p };
            object[] args = new object[5]; args[0] = Hwnd;
            object rt = AJT.InvokeMember("GetClientRect", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            x1 = (int)args[1]; y1 = (int)args[2]; x2 = (int)args[3]; y2 = (int)args[4];
            return (int)rt;
        }

        public int GetClientSize(int Hwnd, out int Width, out int Height)
        {
            ParameterModifier p = new ParameterModifier(3); p[1] = true; p[2] = true; ParameterModifier[] mods = { p };
            object[] args = new object[3]; args[0] = Hwnd;
            object rt = AJT.InvokeMember("GetClientSize", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Width = (int)args[1]; Height = (int)args[2];
            return (int)rt;
        }

        public int GetWindowRect(int Hwnd, out int x1, out int y1, out int x2, out int y2, int Type)
        {
            ParameterModifier p = new ParameterModifier(6); p[1] = true; p[2] = true; p[3] = true; p[4] = true; ParameterModifier[] mods = { p };
            object[] args = new object[6]; args[0] = Hwnd; args[5] = Type;
            object rt = AJT.InvokeMember("GetWindowRect", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            x1 = (int)args[1]; y1 = (int)args[2]; x2 = (int)args[3]; y2 = (int)args[4];
            return (int)rt;
        }

        public int GetWindowSize(int Hwnd, out int Width, out int Height)
        {
            ParameterModifier p = new ParameterModifier(3); p[1] = true; p[2] = true; ParameterModifier[] mods = { p };
            object[] args = new object[3]; args[0] = Hwnd;
            object rt = AJT.InvokeMember("GetWindowSize", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Width = (int)args[1]; Height = (int)args[2];
            return (int)rt;
        }

        public int SetClientSize(int Hwnd, int Width, int Height)
        {
            object[] args = new object[3]; args[0] = Hwnd; args[1] = Width; args[2] = Height;
            object rt = AJT.InvokeMember("SetClientSize", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetWindowSize(int Hwnd, int Width, int Height)
        {
            object[] args = new object[3]; args[0] = Hwnd; args[1] = Width; args[2] = Height;
            object rt = AJT.InvokeMember("SetWindowSize", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int MoveWindow(int Hwnd, int x, int y)
        {
            object[] args = new object[3]; args[0] = Hwnd; args[1] = x; args[2] = y;
            object rt = AJT.InvokeMember("MoveWindow", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetWindowState(int Hwnd, int Type)
        {
            object[] args = new object[2]; args[0] = Hwnd; args[1] = Type;
            object rt = AJT.InvokeMember("GetWindowState", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetWindowState(int Hwnd, int Type)
        {
            object[] args = new object[2]; args[0] = Hwnd; args[1] = Type;
            object rt = AJT.InvokeMember("SetWindowState", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetWindowThreadProcessId(int Hwnd, int Type)
        {
            object[] args = new object[2]; args[0] = Hwnd; args[1] = Type;
            object rt = AJT.InvokeMember("GetWindowThreadProcessId", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string GetWindowProcessPath(int Hwnd, int Type)
        {
            object[] args = new object[2]; args[0] = Hwnd; args[1] = Type;
            object rt = AJT.InvokeMember("GetWindowProcessPath", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int SetWindowTransparent(int Hwnd, string Color, int Tp, int Type)
        {
            object[] args = new object[4]; args[0] = Hwnd; args[1] = Color; args[2] = Tp; args[3] = Type;
            object rt = AJT.InvokeMember("SetWindowTransparent", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetForegroundWindow()
        {
            object rt = AJT.InvokeMember("GetForegroundWindow", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int GetFocus()
        {
            object rt = AJT.InvokeMember("GetFocus", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int GetWindowFromPoint(int x, int y)
        {
            object[] args = new object[2]; args[0] = x; args[1] = y;
            object rt = AJT.InvokeMember("GetWindowFromPoint", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetWindowFromMouse()
        {
            object rt = AJT.InvokeMember("GetWindowFromMouse", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int GetWindow(int Hwnd, int Type)
        {
            object[] args = new object[2]; args[0] = Hwnd; args[1] = Type;
            object rt = AJT.InvokeMember("GetWindow", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int CloseWindow(int Hwnd)
        {
            object[] args = new object[1]; args[0] = Hwnd;
            object rt = AJT.InvokeMember("CloseWindow", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int FillRect(int Hwnd, int x1, int y1, int x2, int y2, string Color)
        {
            object[] args = new object[6]; args[0] = Hwnd; args[1] = x1; args[2] = y1; args[3] = x2; args[4] = y2; args[5] = Color;
            object rt = AJT.InvokeMember("FillRect", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int DrawLine(int Hwnd, int x1, int y1, int x2, int y2, string Color, int Width, int Style)
        {
            object[] args = new object[8]; args[0] = Hwnd; args[1] = x1; args[2] = y1; args[3] = x2; args[4] = y2; args[5] = Color;
            args[6] = Width; args[7] = Style;
            object rt = AJT.InvokeMember("DrawLine", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int DrawPic(int Hwnd, int x, int y, string Color, string PicName)
        {
            object[] args = new object[5]; args[0] = Hwnd; args[1] = x; args[2] = y; args[3] = Color; args[4] = PicName;
            object rt = AJT.InvokeMember("DrawPic", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetCHwndSize(int Hwnd, int Width, int Height)
        {
            object[] args = new object[3]; args[0] = Hwnd; args[1] = Width; args[2] = Height;
            object rt = AJT.InvokeMember("SetCHwndSize", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int DrawGif(int Hwnd, int x, int y, string PicName, int TD, int Num)
        {
            object[] args = new object[6]; args[0] = Hwnd; args[1] = x; args[2] = y; args[3] = PicName; args[4] = TD; args[5] = Num;
            object rt = AJT.InvokeMember("DrawGif", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int StopGif(int Hwnd, int x, int y, string PicName)
        {
            object[] args = new object[4]; args[0] = Hwnd; args[1] = x; args[2] = y; args[3] = PicName;
            object rt = AJT.InvokeMember("StopGif", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int DrawText(int Hwnd, int x1, int y1, int x2, int y2, string Text, string Color, string BkColor, int Type)
        {
            object[] args = new object[9]; args[0] = Hwnd; args[1] = x1; args[2] = y1; args[3] = x2; args[4] = y2; args[5] = Text;
            args[6] = Color; args[7] = BkColor; args[8] = Type;
            object rt = AJT.InvokeMember("DrawText", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int ClearTextD(int Hwnd)
        {
            object[] args = new object[1]; args[0] = Hwnd;
            object rt = AJT.InvokeMember("ClearTextD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetCreateWindows(int Type)
        {
            object[] args = new object[1]; args[0] = Type;
            object rt = AJT.InvokeMember("SetCreateWindows", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int OpenFolder(string Fd)
        {
            object[] args = new object[1]; args[0] = Fd;
            object rt = AJT.InvokeMember("OpenFolder", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int MoveFile(string SF, string DF, int Type)
        {
            object[] args = new object[3]; args[0] = SF; args[1] = DF; args[2] = Type;
            object rt = AJT.InvokeMember("MoveFile", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetFileAttribute(string FN, out int RD, out int HD)
        {
            ParameterModifier p = new ParameterModifier(3); p[1] = true; p[2] = true; ParameterModifier[] mods = { p };
            object[] args = new object[3]; args[0] = FN;
            object rt = AJT.InvokeMember("GetFileAttribute", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            RD = (int)args[1]; HD = (int)args[2];
            return (int)rt;
        }

        public int SetFileAttribute(string FN, int RD, int HD)
        {
            object[] args = new object[3]; args[0] = FN; args[1] = RD; args[2] = HD;
            object rt = AJT.InvokeMember("SetFileAttribute", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetFileTime(string FN, out string CT, out string WT, out string AT)
        {
            ParameterModifier p = new ParameterModifier(4); p[1] = true; p[2] = true; p[3] = true; ParameterModifier[] mods = { p };
            object[] args = new object[4]; args[0] = FN;
            object rt = AJT.InvokeMember("GetFileTime", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            CT = args[1].ToString(); WT = args[2].ToString(); AT = args[3].ToString();
            return (int)rt;
        }

        public int SetFileTime(string FN, string CT, string WT, string AT)
        {
            object[] args = new object[4]; args[0] = FN; args[1] = CT; args[2] = WT; args[3] = AT;
            object rt = AJT.InvokeMember("SetFileTime", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public double GetFileSize(string FN, out int FSH, out int FSL)
        {
            ParameterModifier p = new ParameterModifier(3); p[1] = true; p[2] = true; ParameterModifier[] mods = { p };
            object[] args = new object[3]; args[0] = FN;
            object rt = AJT.InvokeMember("GetFileSize", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            FSH = (int)args[1]; FSL = (int)args[2];
            return (double)rt;
        }

        public int CompareFileTime(string FN, string FND, out int CT, out int WT, out int AT)
        {
            ParameterModifier p = new ParameterModifier(5); p[2] = true; p[3] = true; p[4] = true; ParameterModifier[] mods = { p };
            object[] args = new object[5]; args[0] = FN; args[1] = FND;
            object rt = AJT.InvokeMember("CompareFileTime", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            CT = (int)args[2]; WT = (int)args[3]; AT = (int)args[4];
            return (int)rt;
        }

        public int MoveFolder(string SF, string DF, int Type)
        {
            object[] args = new object[3]; args[0] = SF; args[1] = DF; args[2] = Type;
            object rt = AJT.InvokeMember("MoveFolder", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int ReNameFile(string SF, string DF)
        {
            object[] args = new object[2]; args[0] = SF; args[1] = DF;
            object rt = AJT.InvokeMember("ReNameFile", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string SelectFileOrFolder()
        {
            object rt = AJT.InvokeMember("SelectFileOrFolder", BindingFlags.InvokeMethod, null, AJ, null);
            return rt.ToString();
        }

        public int WriteIni(string FN, string Section, string Key, string Value, string Pw)
        {
            object[] args = new object[5]; args[0] = FN; args[1] = Section; args[2] = Key; args[3] = Value; args[4] = Pw;
            object rt = AJT.InvokeMember("WriteIni", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string ReadIni(string FN, string Section, string Key, string Pw)
        {
            object[] args = new object[4]; args[0] = FN; args[1] = Section; args[2] = Key; args[3] = Pw;
            object rt = AJT.InvokeMember("ReadIni", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int DeleteIni(string FN, string Section, string Key, string Pw)
        {
            object[] args = new object[4]; args[0] = FN; args[1] = Section; args[2] = Key; args[3] = Pw;
            object rt = AJT.InvokeMember("DeleteIni", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string EnumIni(string FN, string Section, string Pw)
        {
            object[] args = new object[3]; args[0] = FN; args[1] = Section; args[2] = Pw;
            object rt = AJT.InvokeMember("EnumIni", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int EncryptFile(string FN, string Pw)
        {
            object[] args = new object[2]; args[0] = FN; args[1] = Pw;
            object rt = AJT.InvokeMember("EncryptFile", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int DecryptFile(string FN, string Pw)
        {
            object[] args = new object[2]; args[0] = FN; args[1] = Pw;
            object rt = AJT.InvokeMember("DecryptFile", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string ReadFile(string FN, int Pos, int Flag, int Size, int Type, int TypeD)
        {
            object[] args = new object[6]; args[0] = FN; args[1] = Pos; args[2] = Flag; args[3] = Size; args[4] = Type; args[5] = TypeD;
            object rt = AJT.InvokeMember("ReadFile", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int WriteFile(string FN, string Str, int Pos, int Flag, int Size, int Type, int TypeD)
        {
            object[] args = new object[7]; args[0] = FN; args[1] = Str; args[2] = Pos; args[3] = Flag; args[4] = Size; args[5] = Type; args[6] = TypeD;
            object rt = AJT.InvokeMember("WriteFile", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int CompareFileData(string SN1, string SN2)
        {
            object[] args = new object[2]; args[0] = SN1; args[1] = SN2;
            object rt = AJT.InvokeMember("CompareFileData", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string DoubleToData(double db)
        {
            object[] args = new object[1]; args[0] = db;
            object rt = AJT.InvokeMember("DoubleToData", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string FloatToData(float fl)
        {
            object[] args = new object[1]; args[0] = fl;
            object rt = AJT.InvokeMember("FloatToData", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string IntToData(int i, int Type)
        {
            object[] args = new object[2]; args[0] = i; args[1] = Type;
            object rt = AJT.InvokeMember("IntToData", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string Int64ToData(long i64, int Type)
        {
            object[] args = new object[2]; args[0] = i64; args[1] = Type;
            object rt = AJT.InvokeMember("Int64ToData", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string StringToData(string Str, int Type, int Flag)
        {
            object[] args = new object[3]; args[0] = Str; args[1] = Type; args[2] = Flag;
            object rt = AJT.InvokeMember("StringToData", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int FreeProcessMemory(int PID, int Hwnd)
        {
            object[] args = new object[2]; args[0] = PID; args[1] = Hwnd;
            object rt = AJT.InvokeMember("FreeProcessMemory", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public long VirtualAllocEx(int PID, int Hwnd, long Addr, uint Size, int Type)
        {
            object[] args = new object[5]; args[0] = PID; args[1] = Hwnd; args[2] = Addr; args[3] = Size; args[4] = Type;
            object rt = AJT.InvokeMember("VirtualAllocEx", BindingFlags.InvokeMethod, null, AJ, args);
            return (long)rt;
        }

        public int VirtualFreeEx(int PID, int Hwnd, long Addr)
        {
            object[] args = new object[3]; args[0] = PID; args[1] = Hwnd; args[2] = Addr;
            object rt = AJT.InvokeMember("VirtualFreeEx", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string VirtualQueryEx(int PID, int Hwnd, long Addr, out int AProtect, out int Protect, out int State, out int Type)
        {
            ParameterModifier p = new ParameterModifier(7); p[3] = true; p[4] = true; p[5] = true; p[6] = true; ParameterModifier[] mods = { p };
            object[] args = new object[7]; args[0] = PID; args[1] = Hwnd; args[2] = Addr;
            object rt = AJT.InvokeMember("VirtualQueryEx", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            AProtect = (int)args[3]; Protect = (int)args[4]; State = (int)args[5]; Type = (int)args[6];
            return rt.ToString();
        }

        public uint VirtualProtectEx(int PID, int Hwnd, long Addr, uint Size, int Flag, uint Protect, out int Type)
        {
            ParameterModifier p = new ParameterModifier(7); p[6] = true; ParameterModifier[] mods = { p };
            object[] args = new object[7]; args[0] = PID; args[1] = Hwnd; args[2] = Addr; args[3] = Size; args[4] = Flag; args[5] = Protect;
            object rt = AJT.InvokeMember("VirtualProtectEx", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Type = (int)args[6];
            return (uint)rt;
        }

        public string ReadDataS(int PID, int Hwnd, string AddrS, int Len)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Len;
            object rt = AJT.InvokeMember("ReadDataS", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string ReadDataL(int PID, int Hwnd, long AddrL, int Len)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL; args[3] = Len;
            object rt = AJT.InvokeMember("ReadDataL", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int WriteDataS(int PID, int Hwnd, string AddrS, string Data)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Data;
            object rt = AJT.InvokeMember("WriteDataS", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int WriteDataL(int PID, int Hwnd, long AddrL, string Data)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL; args[3] = Data;
            object rt = AJT.InvokeMember("WriteDataL", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public long ReadDataSA(int PID, int Hwnd, string AddrS, int Len)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Len;
            object rt = AJT.InvokeMember("ReadDataSA", BindingFlags.InvokeMethod, null, AJ, args);
            return (long)rt;
        }

        public long ReadDataLA(int PID, int Hwnd, long AddrL, int Len)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL; args[3] = Len;
            object rt = AJT.InvokeMember("ReadDataLA", BindingFlags.InvokeMethod, null, AJ, args);
            return (long)rt;
        }

        public int WriteDataSA(int PID, int Hwnd, string AddrS, long Data, int Len)
        {
            object[] args = new object[5]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Data; args[4] = Len;
            object rt = AJT.InvokeMember("WriteDataSA", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int WriteDataLA(int PID, int Hwnd, long AddrL, long Data, int Len)
        {
            object[] args = new object[5]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL; args[3] = Data; args[4] = Len;
            object rt = AJT.InvokeMember("WriteDataLA", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public double ReadDoubleS(int PID, int Hwnd, string AddrS, out int Type)
        {
            ParameterModifier p = new ParameterModifier(4); p[3] = true; ParameterModifier[] mods = { p };
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS;
            object rt = AJT.InvokeMember("ReadDoubleS", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Type = (int)args[3];
            return (double)rt;
        }

        public double ReadDoubleL(int PID, int Hwnd, long AddrL, out int Type)
        {
            ParameterModifier p = new ParameterModifier(4); p[3] = true; ParameterModifier[] mods = { p };
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL;
            object rt = AJT.InvokeMember("ReadDoubleL", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Type = (int)args[3];
            return (double)rt;
        }

        public int WriteDoubleS(int PID, int Hwnd, string AddrS, double Db)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Db;
            object rt = AJT.InvokeMember("WriteDoubleS", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int WriteDoubleL(int PID, int Hwnd, long AddrL, double Db)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL; args[3] = Db;
            object rt = AJT.InvokeMember("WriteDoubleL", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public float ReadFloatS(int PID, int Hwnd, string AddrS, out int Type)
        {
            ParameterModifier p = new ParameterModifier(4); p[3] = true; ParameterModifier[] mods = { p };
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS;
            object rt = AJT.InvokeMember("ReadFloatS", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Type = (int)args[3];
            return (float)rt;
        }

        public float ReadFloatL(int PID, int Hwnd, long AddrL, out int Type)
        {
            ParameterModifier p = new ParameterModifier(4); p[3] = true; ParameterModifier[] mods = { p };
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL;
            object rt = AJT.InvokeMember("ReadFloatL", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Type = (int)args[3];
            return (float)rt;
        }

        public int WriteFloatS(int PID, int Hwnd, string AddrS, float Fl)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Fl;
            object rt = AJT.InvokeMember("WriteFloatS", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int WriteFloatL(int PID, int Hwnd, long AddrL, float Fl)
        {
            object[] args = new object[4]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL; args[3] = Fl;
            object rt = AJT.InvokeMember("WriteFloatL", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public long ReadIntS(int PID, int Hwnd, string AddrS, int Flag, out int Type)
        {
            ParameterModifier p = new ParameterModifier(5); p[4] = true; ParameterModifier[] mods = { p };
            object[] args = new object[5]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Flag;
            object rt = AJT.InvokeMember("ReadIntS", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Type = (int)args[4];
            return (long)rt;
        }

        public long ReadIntL(int PID, int Hwnd, long AddrL, int Flag, out int Type)
        {
            ParameterModifier p = new ParameterModifier(5); p[4] = true; ParameterModifier[] mods = { p };
            object[] args = new object[5]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL; args[3] = Flag;
            object rt = AJT.InvokeMember("ReadIntL", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Type = (int)args[4];
            return (long)rt;
        }

        public int WriteIntS(int PID, int Hwnd, string AddrS, long Ll, int Type)
        {
            object[] args = new object[5]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Ll; args[4] = Type;
            object rt = AJT.InvokeMember("WriteIntS", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int WriteIntL(int PID, int Hwnd, long AddrL, long Ll, int Type)
        {
            object[] args = new object[5]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL; args[3] = Ll; args[4] = Type;
            object rt = AJT.InvokeMember("WriteIntL", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string ReadStringS(int PID, int Hwnd, string AddrS, int Len, int Flag, out int Type)
        {
            ParameterModifier p = new ParameterModifier(6); p[5] = true; ParameterModifier[] mods = { p };
            object[] args = new object[6]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Len; args[4] = Flag;
            object rt = AJT.InvokeMember("ReadStringS", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Type = (int)args[5];
            return rt.ToString();
        }

        public string ReadStringL(int PID, int Hwnd, long AddrL, int Len, int Flag, out int Type)
        {
            ParameterModifier p = new ParameterModifier(6); p[5] = true; ParameterModifier[] mods = { p };
            object[] args = new object[6]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL; args[3] = Len; args[4] = Flag;
            object rt = AJT.InvokeMember("ReadStringL", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Type = (int)args[5];
            return rt.ToString();
        }

        public int WriteStringS(int PID, int Hwnd, string AddrS, string Str, int Type)
        {
            object[] args = new object[5]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Str; args[4] = Type;
            object rt = AJT.InvokeMember("WriteStringS", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int WriteStringL(int PID, int Hwnd, long AddrL, string Str, int Type)
        {
            object[] args = new object[5]; args[0] = PID; args[1] = Hwnd; args[2] = AddrL; args[3] = Str; args[4] = Type;
            object rt = AJT.InvokeMember("WriteStringL", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string FindData(int PID, int Hwnd, string AddrS, string Data, int Step, int Type, int PN, int TN, string FN)
        {
            object[] args = new object[9]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Data; args[4] = Step; args[5] = Type;
            args[6] = PN; args[7] = TN; args[8] = FN;
            object rt = AJT.InvokeMember("FindData", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string FindDouble(int PID, int Hwnd, string AddrS, double DbMin, double DbMax, int Step, int Type, int PN, int TN, string FN)
        {
            object[] args = new object[10]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = DbMin; args[4] = DbMax; args[5] = Step;
            args[6] = Type; args[7] = PN; args[8] = TN; args[9] = FN;
            object rt = AJT.InvokeMember("FindDouble", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string FindFloat(int PID, int Hwnd, string AddrS, float FlMin, float FlMax, int Step, int Type, int PN, int TN, string FN)
        {
            object[] args = new object[10]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = FlMin; args[4] = FlMax; args[5] = Step;
            args[6] = Type; args[7] = PN; args[8] = TN; args[9] = FN;
            object rt = AJT.InvokeMember("FindFloat", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string FindInt(int PID, int Hwnd, string AddrS, long LlMin, long LlMax, int Flag, int Step, int Type, int PN, int TN, string FN)
        {
            object[] args = new object[11]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = LlMin; args[4] = LlMax; args[5] = Flag;
            args[6] = Step; args[7] = Type; args[8] = PN; args[9] = TN; args[10] = FN;
            object rt = AJT.InvokeMember("FindInt", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string FindString(int PID, int Hwnd, string AddrS, string Str, int Flag, int Nul, int Step, int Type, int PN, int TN, string FN)
        {
            object[] args = new object[11]; args[0] = PID; args[1] = Hwnd; args[2] = AddrS; args[3] = Str; args[4] = Flag; args[5] = Nul;
            args[6] = Step; args[7] = Type; args[8] = PN; args[9] = TN; args[10] = FN;
            object rt = AJT.InvokeMember("FindString", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int SuiJi(int RMin, int RMax)
        {
            object[] args = new object[2]; args[0] = RMin; args[1] = RMax;
            object rt = AJT.InvokeMember("SuiJi", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GaiLu(int P)
        {
            object[] args = new object[1]; args[0] = P;
            object rt = AJT.InvokeMember("GaiLu", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int YanShi(int RMin, int RMax)
        {
            object[] args = new object[2]; args[0] = RMin; args[1] = RMax;
            object rt = AJT.InvokeMember("YanShi", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string GetClipboard()
        {
            object rt = AJT.InvokeMember("GetClipboard", BindingFlags.InvokeMethod, null, AJ, null);
            return rt.ToString();
        }

        public int SetClipboard(string Str)
        {
            object[] args = new object[1]; args[0] = Str;
            object rt = AJT.InvokeMember("SetClipboard", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetCPU(out string Type, out string CPUID)
        {
            ParameterModifier p = new ParameterModifier(2); p[0] = true; p[1] = true; ParameterModifier[] mods = { p };
            object[] args = new object[2];
            object rt = AJT.InvokeMember("GetCPU", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Type = args[0].ToString(); CPUID = args[1].ToString();
            return (int)rt;
        }

        public int GetCPURate()
        {
            object rt = AJT.InvokeMember("GetCPURate", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int SetAero(int Type)
        {
            object[] args = new object[1]; args[0] = Type;
            object rt = AJT.InvokeMember("SetAero", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int Beep(int Hz, int T)
        {
            object[] args = new object[2]; args[0] = Hz; args[1] = T;
            object rt = AJT.InvokeMember("Beep", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int Msg(int x, int y, string Color, string BkColor, string FmColor, string Text, int T)
        {
            object[] args = new object[7]; args[0] = x; args[1] = y; args[2] = Color; args[3] = BkColor; args[4] = FmColor; args[5] = Text; args[6] = T;
            object rt = AJT.InvokeMember("Msg", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetFontSmoothing()
        {
            object rt = AJT.InvokeMember("GetFontSmoothing", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int SetFontSmoothing(int Type)
        {
            object[] args = new object[1]; args[0] = Type;
            object rt = AJT.InvokeMember("SetFontSmoothing", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetUAC()
        {
            object rt = AJT.InvokeMember("GetUAC", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int SetUAC(int Type)
        {
            object[] args = new object[1]; args[0] = Type;
            object rt = AJT.InvokeMember("SetUAC", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetBeep()
        {
            object rt = AJT.InvokeMember("GetBeep", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int SetBeep(int Type)
        {
            object[] args = new object[1]; args[0] = Type;
            object rt = AJT.InvokeMember("SetBeep", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetPower(int VT, int ST)
        {
            object[] args = new object[2]; args[0] = VT; args[1] = ST;
            object rt = AJT.InvokeMember("SetPower", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetPowerState(int Type)
        {
            object[] args = new object[1]; args[0] = Type;
            object rt = AJT.InvokeMember("SetPowerState", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetPower(out int VT, out int ST)
        {
            ParameterModifier p = new ParameterModifier(2); p[0] = true; p[1] = true; ParameterModifier[] mods = { p };
            object[] args = new object[2];
            object rt = AJT.InvokeMember("GetPower", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            VT = (int)args[0]; ST = (int)args[1];
            return (int)rt;
        }

        public int SetScreenSave(int Type)
        {
            object[] args = new object[1]; args[0] = Type;
            object rt = AJT.InvokeMember("SetScreenSave", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int ExitOs(int Type)
        {
            object[] args = new object[1]; args[0] = Type;
            object rt = AJT.InvokeMember("ExitOs", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public uint GetTime()
        {
            object rt = AJT.InvokeMember("GetTime", BindingFlags.InvokeMethod, null, AJ, null);
            return (uint)rt;
        }

        public int GetScreen(out int Width, out int Height)
        {
            ParameterModifier p = new ParameterModifier(2); p[0] = true; p[1] = true; ParameterModifier[] mods = { p };
            object[] args = new object[2];
            object rt = AJT.InvokeMember("GetScreen", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Width = (int)args[0]; Height = (int)args[1];
            return (int)rt;
        }

        public int SetScreen(int Width, int Height)
        {
            object[] args = new object[2]; args[0] = Width; args[1] = Height;
            object rt = AJT.InvokeMember("SetScreen", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetMemory(out int TPhy, out double APhy)
        {
            ParameterModifier p = new ParameterModifier(2); p[0] = true; p[1] = true; ParameterModifier[] mods = { p };
            object[] args = new object[2];
            object rt = AJT.InvokeMember("GetMemory", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            TPhy = (int)args[0]; APhy = (double)args[1];
            return (int)rt;
        }

        public int GetDPI()
        {
            object rt = AJT.InvokeMember("GetDPI", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int Cmd(string CL, int Type)
        {
            object[] args = new object[2]; args[0] = CL; args[1] = Type;
            object rt = AJT.InvokeMember("Cmd", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string GetGc()
        {
            object rt = AJT.InvokeMember("GetGc", BindingFlags.InvokeMethod, null, AJ, null);
            return rt.ToString();
        }

        public string GetDisk(out string Model, out string Revision)
        {
            ParameterModifier p = new ParameterModifier(2); p[0] = true; p[1] = true; ParameterModifier[] mods = { p };
            object[] args = new object[2];
            object rt = AJT.InvokeMember("GetDisk", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Model = args[0].ToString(); Revision = args[1].ToString();
            return rt.ToString();
        }

        public int PlayMusic(string MF)
        {
            object[] args = new object[1]; args[0] = MF;
            object rt = AJT.InvokeMember("PlayMusic", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int StopMusic()
        {
            object rt = AJT.InvokeMember("StopMusic", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int RunApp(string Path, int Type)
        {
            object[] args = new object[2]; args[0] = Path; args[1] = Type;
            object rt = AJT.InvokeMember("RunApp", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string SuiJiMZ(int MN, int Type)
        {
            object[] args = new object[2]; args[0] = MN; args[1] = Type;
            object rt = AJT.InvokeMember("SuiJiMZ", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string GetSystemTime()
        {
            object rt = AJT.InvokeMember("GetSystemTime", BindingFlags.InvokeMethod, null, AJ, null);
            return rt.ToString();
        }

        public int SetSystemTime(string ST)
        {
            object[] args = new object[1]; args[0] = ST;
            object rt = AJT.InvokeMember("SetSystemTime", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string GetDiskSize()
        {
            object rt = AJT.InvokeMember("GetDiskSize", BindingFlags.InvokeMethod, null, AJ, null);
            return rt.ToString();
        }

        public string GetScreenS(out int xS, out int yS)
        {
            ParameterModifier p = new ParameterModifier(2); p[0] = true; p[1] = true; ParameterModifier[] mods = { p };
            object[] args = new object[2];
            object rt = AJT.InvokeMember("GetScreenS", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            xS = (int)args[0]; yS = (int)args[1];
            return rt.ToString();
        }

        public string GetMAC()
        {
            object rt = AJT.InvokeMember("GetMAC", BindingFlags.InvokeMethod, null, AJ, null);
            return rt.ToString();
        }

        public int KeyDown(ushort KeyD)
        {
            object[] args = new object[1]; args[0] = KeyD;
            object rt = AJT.InvokeMember("KeyDown", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int KeyUp(ushort KeyD)
        {
            object[] args = new object[1]; args[0] = KeyD;
            object rt = AJT.InvokeMember("KeyUp", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int KeyPress(ushort KeyD)
        {
            object[] args = new object[1]; args[0] = KeyD;
            object rt = AJT.InvokeMember("KeyPress", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int KeyDownS(string KeyDS)
        {
            object[] args = new object[1]; args[0] = KeyDS;
            object rt = AJT.InvokeMember("KeyDownS", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int KeyUpS(string KeyDS)
        {
            object[] args = new object[1]; args[0] = KeyDS;
            object rt = AJT.InvokeMember("KeyUpS", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int KeyPressS(string KeyDS)
        {
            object[] args = new object[1]; args[0] = KeyDS;
            object rt = AJT.InvokeMember("KeyPressS", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int KeyPressD(ushort KeyD, int RMin, int RMax, int RDMin, int RDMax)
        {
            object[] args = new object[5]; args[0] = KeyD; args[1] = RMin; args[2] = RMax; args[3] = RDMin; args[4] = RDMax;
            object rt = AJT.InvokeMember("KeyPressD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int KeyPressSD(string KeyDS, int RMin, int RMax, int RDMin, int RDMax)
        {
            object[] args = new object[5]; args[0] = KeyDS; args[1] = RMin; args[2] = RMax; args[3] = RDMin; args[4] = RDMax;
            object rt = AJT.InvokeMember("KeyPressSD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int KeyPressZ(string KeyZ, int RMin, int RMax)
        {
            object[] args = new object[3]; args[0] = KeyZ; args[1] = RMin; args[2] = RMax;
            object rt = AJT.InvokeMember("KeyPressZ", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetKeyDState(int KeyD)
        {
            object[] args = new object[1]; args[0] = KeyD;
            object rt = AJT.InvokeMember("GetKeyDState", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int WaitKeyD(int KeyD, int TKeyD)
        {
            object[] args = new object[2]; args[0] = KeyD; args[1] = TKeyD;
            object rt = AJT.InvokeMember("WaitKeyD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int LeftDown()
        {
            object rt = AJT.InvokeMember("LeftDown", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int LeftUp()
        {
            object rt = AJT.InvokeMember("LeftUp", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int RightDown()
        {
            object rt = AJT.InvokeMember("RightDown", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int RightUp()
        {
            object rt = AJT.InvokeMember("RightUp", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int MiddleDown()
        {
            object rt = AJT.InvokeMember("MiddleDown", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int MiddleUp()
        {
            object rt = AJT.InvokeMember("MiddleUp", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int WheelDown()
        {
            object rt = AJT.InvokeMember("WheelDown", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int WheelUp()
        {
            object rt = AJT.InvokeMember("WheelUp", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int LeftClick()
        {
            object rt = AJT.InvokeMember("LeftClick", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int RightClick()
        {
            object rt = AJT.InvokeMember("RightClick", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int MiddleClick()
        {
            object rt = AJT.InvokeMember("MiddleClick", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int LeftClickD(int RMin, int RMax, int RDMin, int RDMax)
        {
            object[] args = new object[4]; args[0] = RMin; args[1] = RMax; args[2] = RDMin; args[3] = RDMax;
            object rt = AJT.InvokeMember("LeftClickD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int RightClickD(int RMin, int RMax, int RDMin, int RDMax)
        {
            object[] args = new object[4]; args[0] = RMin; args[1] = RMax; args[2] = RDMin; args[3] = RDMax;
            object rt = AJT.InvokeMember("RightClickD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int MiddleClickD(int RMin, int RMax, int RDMin, int RDMax)
        {
            object[] args = new object[4]; args[0] = RMin; args[1] = RMax; args[2] = RDMin; args[3] = RDMax;
            object rt = AJT.InvokeMember("MiddleClickD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int WheelDownD(int Num, int RMin, int RMax, int RDMin, int RDMax)
        {
            object[] args = new object[5]; args[0] = Num; args[1] = RMin; args[2] = RMax; args[3] = RDMin; args[4] = RDMax;
            object rt = AJT.InvokeMember("WheelDownD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int WheelUpD(int Num, int RMin, int RMax, int RDMin, int RDMax)
        {
            object[] args = new object[5]; args[0] = Num; args[1] = RMin; args[2] = RMax; args[3] = RDMin; args[4] = RDMax;
            object rt = AJT.InvokeMember("WheelUpD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int LeftDoubleClick()
        {
            object rt = AJT.InvokeMember("LeftDoubleClick", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int LeftDoubleClickD(int RMin, int RMax, int RDMin, int RDMax, int RDRMin, int RDRMax)
        {
            object[] args = new object[6]; args[0] = RMin; args[1] = RMax; args[2] = RDMin; args[3] = RDMax; args[4] = RDRMin; args[5] = RDRMax;
            object rt = AJT.InvokeMember("LeftDoubleClickD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int RightDoubleClickD(int RMin, int RMax, int RDMin, int RDMax, int RDRMin, int RDRMax)
        {
            object[] args = new object[6]; args[0] = RMin; args[1] = RMax; args[2] = RDMin; args[3] = RDMax; args[4] = RDRMin; args[5] = RDRMax;
            object rt = AJT.InvokeMember("RightDoubleClickD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int MiddleDoubleClickD(int RMin, int RMax, int RDMin, int RDMax, int RDRMin, int RDRMax)
        {
            object[] args = new object[6]; args[0] = RMin; args[1] = RMax; args[2] = RDMin; args[3] = RDMax; args[4] = RDRMin; args[5] = RDRMax;
            object rt = AJT.InvokeMember("MiddleDoubleClickD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetDoubleClickTime()
        {
            object rt = AJT.InvokeMember("GetDoubleClickTime", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int SetDoubleClickTime(int T)
        {
            object[] args = new object[1]; args[0] = T;
            object rt = AJT.InvokeMember("SetDoubleClickTime", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetMouseAccuracy(int MouseA)
        {
            object[] args = new object[1]; args[0] = MouseA;
            object rt = AJT.InvokeMember("SetMouseAccuracy", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetMouseSpeed(int MouseS)
        {
            object[] args = new object[1]; args[0] = MouseS;
            object rt = AJT.InvokeMember("SetMouseSpeed", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetMouseSpeed()
        {
            object rt = AJT.InvokeMember("GetMouseSpeed", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int MoveTo(int x, int y)
        {
            object[] args = new object[2]; args[0] = x; args[1] = y;
            object rt = AJT.InvokeMember("MoveTo", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int MoveR(int Rx, int Ry)
        {
            object[] args = new object[2]; args[0] = Rx; args[1] = Ry;
            object rt = AJT.InvokeMember("MoveR", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int MoveToD(int x, int y, int xRMin, int xRMax, int yRMin, int yRMax, int RMin, int RMax, int Speed)
        {
            object[] args = new object[9]; args[0] = x; args[1] = y; args[2] = xRMin; args[3] = xRMax; args[4] = yRMin; args[5] = yRMax;
            args[6] = RMin; args[7] = RMax; args[8] = Speed;
            object rt = AJT.InvokeMember("MoveToD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int MoveRD(int Rx, int Ry, int xRMin, int xRMax, int yRMin, int yRMax, int RMin, int RMax, int Speed)
        {
            object[] args = new object[9]; args[0] = Rx; args[1] = Ry; args[2] = xRMin; args[3] = xRMax; args[4] = yRMin; args[5] = yRMax;
            args[6] = RMin; args[7] = RMax; args[8] = Speed;
            object rt = AJT.InvokeMember("MoveRD", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int MoveToQ(int x, int y, int xRMin, int xRMax, int yRMin, int yRMax, int RMin, int RMax, int Speed)
        {
            object[] args = new object[9]; args[0] = x; args[1] = y; args[2] = xRMin; args[3] = xRMax; args[4] = yRMin; args[5] = yRMax;
            args[6] = RMin; args[7] = RMax; args[8] = Speed;
            object rt = AJT.InvokeMember("MoveToQ", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int MoveRQ(int Rx, int Ry, int xRMin, int xRMax, int yRMin, int yRMax, int RMin, int RMax, int Speed)
        {
            object[] args = new object[9]; args[0] = Rx; args[1] = Ry; args[2] = xRMin; args[3] = xRMax; args[4] = yRMin; args[5] = yRMax;
            args[6] = RMin; args[7] = RMax; args[8] = Speed;
            object rt = AJT.InvokeMember("MoveRQ", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetMousePos(out int x, out int y, int Type)
        {
            ParameterModifier p = new ParameterModifier(3); p[0] = true; p[1] = true; ParameterModifier[] mods = { p };
            object[] args = new object[3]; args[2] = Type;
            object rt = AJT.InvokeMember("GetMousePos", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            x = (int)args[0]; y = (int)args[1];
            return (int)rt;
        }

        public int SetMousePos(int Sx1, int Sy1, int Sx2, int Sy2, int SDxy, int Dx1, int Dy1, int Dx2, int Dy2, int RMin, int RMax, int Speed, int Type)
        {
            object[] args = new object[13]; args[0] = Sx1; args[1] = Sy1; args[2] = Sx2; args[3] = Sy2; args[4] = SDxy; args[5] = Dx1;
            args[6] = Dy1; args[7] = Dx2; args[8] = Dy2; args[9] = RMin; args[10] = RMax; args[11] = Speed; args[12] = Type;
            object rt = AJT.InvokeMember("SetMousePos", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetMouseHotspot(out int x, out int y, int Type)
        {
            ParameterModifier p = new ParameterModifier(3); p[0] = true; p[1] = true; ParameterModifier[] mods = { p };
            object[] args = new object[3]; args[2] = Type;
            object rt = AJT.InvokeMember("GetMouseHotspot", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            x = (int)args[0]; y = (int)args[1];
            return (int)rt;
        }

        public string GetMouseShape(int Type, int Flag)
        {
            object[] args = new object[2]; args[0] = Type; args[1] = Flag;
            object rt = AJT.InvokeMember("GetMouseShape", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int SendString(int Hwnd, string Str, int RMin, int RMax, int Type, int Flag)
        {
            object[] args = new object[6]; args[0] = Hwnd; args[1] = Str; args[2] = RMin; args[3] = RMax; args[4] = Type; args[5] = Flag;
            object rt = AJT.InvokeMember("SendString", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int BlockInput(int Type)
        {
            object[] args = new object[1]; args[0] = Type;
            object rt = AJT.InvokeMember("BlockInput", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int OpenURL(string URL)
        {
            object[] args = new object[1]; args[0] = URL;
            object rt = AJT.InvokeMember("OpenURL", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int DownloadFile(string URL, string FN, int Type)
        {
            object[] args = new object[3]; args[0] = URL; args[1] = FN; args[2] = Type;
            object rt = AJT.InvokeMember("DownloadFile", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetDownloadState()
        {
            object rt = AJT.InvokeMember("GetDownloadState", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int KQHouTai(int Hwnd, string Screen, string Keyboard, string Mouse, string Flag, int Type)
        {
            object[] args = new object[6]; args[0] = Hwnd; args[1] = Screen; args[2] = Keyboard; args[3] = Mouse; args[4] = Flag; args[5] = Type;
            object rt = AJT.InvokeMember("KQHouTai", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GBHouTai()
        {
            object rt = AJT.InvokeMember("GBHouTai", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int SetHwndSKM(int HwndS, int HwndK, int HwndM)
        {
            object[] args = new object[3]; args[0] = HwndS; args[1] = HwndK; args[2] = HwndM;
            object rt = AJT.InvokeMember("SetHwndSKM", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetWindowSNA(int SNA)
        {
            object[] args = new object[1]; args[0] = SNA;
            object rt = AJT.InvokeMember("SetWindowSNA", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetKMSync(int Keyboard, uint uToK, int Mouse, uint uToM)
        {
            object[] args = new object[4]; args[0] = Keyboard; args[1] = uToK; args[2] = Mouse; args[3] = uToM;
            object rt = AJT.InvokeMember("SetKMSync", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetIme(int Type)
        {
            object[] args = new object[1]; args[0] = Type;
            object rt = AJT.InvokeMember("SetIme", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetKMLock(int LockK, int LockM)
        {
            object[] args = new object[2]; args[0] = LockK; args[1] = LockM;
            object rt = AJT.InvokeMember("SetKMLock", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetTimeS(uint TimeS)
        {
            object[] args = new object[1]; args[0] = TimeS;
            object rt = AJT.InvokeMember("SetTimeS", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int DownCpu(uint DCpuT, int DCpuD)
        {
            object[] args = new object[2]; args[0] = DCpuT; args[1] = DCpuD;
            object rt = AJT.InvokeMember("DownCpu", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int LockScreen(int LockS)
        {
            object[] args = new object[1]; args[0] = LockS;
            object rt = AJT.InvokeMember("LockScreen", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetFPS()
        {
            object rt = AJT.InvokeMember("GetFPS", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }

        public int FindShape(int x1, int y1, int x2, int y2, string ColorP, int Dir, double SimP, double SimD, out int x, out int y)
        {
            ParameterModifier p = new ParameterModifier(10); p[8] = true; p[9] = true; ParameterModifier[] mods = { p };
            object[] args = new object[10]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = ColorP;
            args[5] = Dir; args[6] = SimP; args[7] = SimD;
            object rt = AJT.InvokeMember("FindShape", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            x = (int)args[8]; y = (int)args[9];
            return (int)rt;
        }

        public string FindShapeEx(int x1, int y1, int x2, int y2, string ColorP, int Dir, double SimP, double SimD)
        {
            object[] args = new object[8]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = ColorP;
            args[5] = Dir; args[6] = SimP; args[7] = SimD;
            object rt = AJT.InvokeMember("FindShapeEx", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string FindColorSquEx(int x1, int y1, int x2, int y2, string Color, double Sim, double SimD, int Dir, int Width, int Height)
        {
            object[] args = new object[10]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Color;
            args[5] = Sim; args[6] = SimD; args[7] = Dir; args[8] = Width; args[9] = Height;
            object rt = AJT.InvokeMember("FindColorSquEx", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int FindColorSqu(int x1, int y1, int x2, int y2, string Color, double Sim, double SimD, int Dir, int Width, int Height, out int x, out int y)
        {
            ParameterModifier p = new ParameterModifier(12); p[10] = true; p[11] = true; ParameterModifier[] mods = { p };
            object[] args = new object[12]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Color;
            args[5] = Sim; args[6] = SimD; args[7] = Dir; args[8] = Width; args[9] = Height;
            object rt = AJT.InvokeMember("FindColorSqu", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            x = (int)args[10]; y = (int)args[11];
            return (int)rt;
        }

        public string FindPicDEx(int x1, int y1, int x2, int y2, string PicName, string ColorP, double Sim, double SimD, int Dir, int Type, int TypeT)
        {
            object[] args = new object[11]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = PicName;
            args[5] = ColorP; args[6] = Sim; args[7] = SimD; args[8] = Dir; args[9] = Type; args[10] = TypeT;
            object rt = AJT.InvokeMember("FindPicDEx", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int FindPicD(int x1, int y1, int x2, int y2, string PicName, string ColorP, double Sim, double SimD, int Dir, int Type, out string Pic, out int x, out int y)
        {
            ParameterModifier p = new ParameterModifier(13); p[10] = true; p[11] = true; p[12] = true; ParameterModifier[] mods = { p };
            object[] args = new object[13]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = PicName;
            args[5] = ColorP; args[6] = Sim; args[7] = SimD; args[8] = Dir; args[9] = Type;
            object rt = AJT.InvokeMember("FindPicD", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Pic = args[10].ToString(); x = (int)args[11]; y = (int)args[12];
            return (int)rt;
        }

        public string FindPicEx(int x1, int y1, int x2, int y2, string PicName, string ColorP, double Sim, int Dir, int Type, int TypeT)
        {
            object[] args = new object[10]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = PicName;
            args[5] = ColorP; args[6] = Sim; args[7] = Dir; args[8] = Type; args[9] = TypeT;
            object rt = AJT.InvokeMember("FindPicEx", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int FindPic(int x1, int y1, int x2, int y2, string PicName, string ColorP, double Sim, int Dir, int Type, out string Pic, out int x, out int y)
        {
            ParameterModifier p = new ParameterModifier(12); p[9] = true; p[10] = true; p[11] = true; ParameterModifier[] mods = { p };
            object[] args = new object[12]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = PicName;
            args[5] = ColorP; args[6] = Sim; args[7] = Dir; args[8] = Type;
            object rt = AJT.InvokeMember("FindPic", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            Pic = args[9].ToString(); x = (int)args[10]; y = (int)args[11];
            return (int)rt;
        }

        public string FindMultiColorEx(int x1, int y1, int x2, int y2, string Color, string ColorP, double Sim, int Dir, double SimP, double SimD)
        {
            object[] args = new object[10]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Color;
            args[5] = ColorP; args[6] = Sim; args[7] = Dir; args[8] = SimP; args[9] = SimD;
            object rt = AJT.InvokeMember("FindMultiColorEx", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int FindMultiColor(int x1, int y1, int x2, int y2, string Color, string ColorP, double Sim, int Dir, double SimP, double SimD, out int x, out int y)
        {
            ParameterModifier p = new ParameterModifier(12); p[10] = true; p[11] = true; ParameterModifier[] mods = { p };
            object[] args = new object[12]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Color;
            args[5] = ColorP; args[6] = Sim; args[7] = Dir; args[8] = SimP; args[9] = SimD;
            object rt = AJT.InvokeMember("FindMultiColor", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            x = (int)args[10]; y = (int)args[11];
            return (int)rt;
        }

        public string FindColorEx(int x1, int y1, int x2, int y2, string Color, double Sim, int Dir)
        {
            object[] args = new object[7]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Color; args[5] = Sim; args[6] = Dir;
            object rt = AJT.InvokeMember("FindColorEx", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int FindColor(int x1, int y1, int x2, int y2, string Color, double Sim, int Dir, out int x, out int y)
        {
            ParameterModifier p = new ParameterModifier(9); p[7] = true; p[8] = true; ParameterModifier[] mods = { p };
            object[] args = new object[9]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Color; args[5] = Sim; args[6] = Dir;
            object rt = AJT.InvokeMember("FindColor", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            x = (int)args[7]; y = (int)args[8];
            return (int)rt;
        }

        public int FindColorM(int x1, int y1, int x2, int y2, string Color, double Sim, int Count)
        {
            object[] args = new object[7]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Color; args[5] = Sim; args[6] = Count;
            object rt = AJT.InvokeMember("FindColorM", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetColorNum(int x1, int y1, int x2, int y2, string Color, double Sim)
        {
            object[] args = new object[6]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Color; args[5] = Sim;
            object rt = AJT.InvokeMember("GetColorNum", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public object GetColorAve(int x1, int y1, int x2, int y2, int Type)
        {
            object[] args = new object[5]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Type;
            object rt = AJT.InvokeMember("GetColorAve", BindingFlags.InvokeMethod, null, AJ, args);
            return rt;
        }

        public int CmpColor(int x, int y, string Color, double Sim, int Type)
        {
            object[] args = new object[5]; args[0] = x; args[1] = y; args[2] = Color; args[3] = Sim; args[4] = Type;
            object rt = AJT.InvokeMember("CmpColor", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public object GetColor(int x, int y, int Type, int TypeD)
        {
            object[] args = new object[4]; args[0] = x; args[1] = y; args[2] = Type; args[3] = TypeD;
            object rt = AJT.InvokeMember("GetColor", BindingFlags.InvokeMethod, null, AJ, args);
            return rt;
        }

        public string BGRorRGB(string Color)
        {
            object[] args = new object[1]; args[0] = Color;
            object rt = AJT.InvokeMember("BGRorRGB", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string BGRorRGBtoHSV(string Color, int Type)
        {
            object[] args = new object[2]; args[0] = Color; args[1] = Type;
            object rt = AJT.InvokeMember("BGRorRGBtoHSV", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public string HSVtoBGRorRGB(string Color, int Type)
        {
            object[] args = new object[2]; args[0] = Color; args[1] = Type;
            object rt = AJT.InvokeMember("HSVtoBGRorRGB", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int LoadPic(string PicName)
        {
            object[] args = new object[1]; args[0] = PicName;
            object rt = AJT.InvokeMember("LoadPic", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int FreePic(string PicName)
        {
            object[] args = new object[1]; args[0] = PicName;
            object rt = AJT.InvokeMember("FreePic", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int IsScreenStuck(int x1, int y1, int x2, int y2, int T)
        {
            object[] args = new object[5]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = T;
            object rt = AJT.InvokeMember("IsScreenStuck", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetPicSize(string PicName, out int PicW, out int PicH)
        {
            ParameterModifier p = new ParameterModifier(3); p[1] = true; p[2] = true; ParameterModifier[] mods = { p };
            object[] args = new object[3]; args[0] = PicName;
            object rt = AJT.InvokeMember("GetPicSize", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            PicW = (int)args[1]; PicH = (int)args[2];
            return (int)rt;
        }

        public int PicToBmp(string PicName, string BmpName)
        {
            object[] args = new object[2]; args[0] = PicName; args[1] = BmpName;
            object rt = AJT.InvokeMember("PicToBmp", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int ScreenShot(int x1, int y1, int x2, int y2, string PicName, int Type, int Quality, uint TD, uint T, int Flag, int Mouse)
        {
            object[] args = new object[11]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = PicName;
            args[5] = Type; args[6] = Quality; args[7] = TD; args[8] = T; args[9] = Flag; args[10] = Mouse;
            object rt = AJT.InvokeMember("ScreenShot", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int LoadDict(int DNum, string DName)
        {
            object[] args = new object[2]; args[0] = DNum; args[1] = DName;
            object rt = AJT.InvokeMember("LoadDict", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetDict(int DNum)
        {
            object[] args = new object[1]; args[0] = DNum;
            object rt = AJT.InvokeMember("SetDict", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public string Ocr(int x1, int y1, int x2, int y2, string Str, string Color, double Sim, int TypeC, int TypeD, int TypeR, int TypeT, string HLine, string PicName)
        {
            object[] args = new object[13]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Str; args[5] = Color;
            args[6] = Sim; args[7] = TypeC; args[8] = TypeD; args[9] = TypeR; args[10] = TypeT; args[11] = HLine; args[12] = PicName;
            object rt = AJT.InvokeMember("Ocr", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int FindStr(int x1, int y1, int x2, int y2, string Str, string Color, double Sim, int Dir, int TypeC, int TypeD, out string StrD, out int x, out int y)
        {
            ParameterModifier p = new ParameterModifier(13); p[10] = true; p[11] = true; p[12] = true; ParameterModifier[] mods = { p };
            object[] args = new object[13]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Str; args[5] = Color;
            args[6] = Sim; args[7] = Dir; args[8] = TypeC; args[9] = TypeD;
            object rt = AJT.InvokeMember("FindStr", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            StrD = args[10].ToString(); x = (int)args[11]; y = (int)args[12];
            return (int)rt;
        }

        public int FreeDict(int DNum)
        {
            object[] args = new object[1]; args[0] = DNum;
            object rt = AJT.InvokeMember("FreeDict", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int GetDict(int GD, int DNum, int Type)
        {
            object[] args = new object[3]; args[0] = GD; args[1] = DNum; args[2] = Type;
            object rt = AJT.InvokeMember("GetDict", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetDictWidth(int DW)
        {
            object[] args = new object[1]; args[0] = DW;
            object rt = AJT.InvokeMember("SetDictWidth", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int SetDictHeight(int DH)
        {
            object[] args = new object[1]; args[0] = DH;
            object rt = AJT.InvokeMember("SetDictHeight", BindingFlags.InvokeMethod, null, AJ, args);
            return (int)rt;
        }

        public int FindStrM(int x1, int y1, int x2, int y2, string Str, string Color, double Sim, int TypeC, int TypeD, out string StrD, out int x, out int y)
        {
            ParameterModifier p = new ParameterModifier(12); p[9] = true; p[10] = true; p[11] = true; ParameterModifier[] mods = { p };
            object[] args = new object[12]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Str; args[5] = Color;
            args[6] = Sim; args[7] = TypeC; args[8] = TypeD;
            object rt = AJT.InvokeMember("FindStrM", BindingFlags.InvokeMethod, null, AJ, args, mods, null, null);
            StrD = args[9].ToString(); x = (int)args[10]; y = (int)args[11];
            return (int)rt;
        }

        public string FindStrMEx(int x1, int y1, int x2, int y2, string Str, string Color, double Sim, int TypeC, int TypeD, string HLine)
        {
            object[] args = new object[10]; args[0] = x1; args[1] = y1; args[2] = x2; args[3] = y2; args[4] = Str; args[5] = Color;
            args[6] = Sim; args[7] = TypeC; args[8] = TypeD; args[9] = HLine;
            object rt = AJT.InvokeMember("FindStrMEx", BindingFlags.InvokeMethod, null, AJ, args);
            return rt.ToString();
        }

        public int SetParam64ToAddr()
        {
            object rt = AJT.InvokeMember("SetParam64ToAddr", BindingFlags.InvokeMethod, null, AJ, null);
            return (int)rt;
        }
    }
}
