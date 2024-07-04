using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace AutoScript.Share
{
    public class Utility
    {
        private static object CMDLocker = new object();
        public static List<string> ExecCMD(string cmd, string ExeFile = "cmd.exe")
        {
            List<string> ret = new List<string>();
            lock (CMDLocker)
            {
                Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = ExeFile;
                p.StartInfo.UseShellExecute = false; //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;//不显示程序窗口
                p.Start();//启动程序
                          //向cmd窗口发送输入信息
                p.StandardInput.WriteLine(cmd + "&exit");
                p.StandardInput.AutoFlush = true;
                StreamReader reader = p.StandardOutput;
                string line = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    ret.Add(reader.ReadLine().Trim());
                }
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
                return ret;
            }
        }
        /// <summary>
        /// 取指定进程的ADB端口号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetADBPort(int id)
        {
            List<string> ret = ExecCMD("netstat -aon|findstr ESTABLISHED.*" + id.ToString());
            foreach (var item in ret)
            {
                if (item.StartsWith("TCP"))
                {
                    return item.Substring(item.IndexOf("    ") + 4, item.IndexOf("    ", item.IndexOf("    ") + 4)).Trim();
                }
            }
            return "";
        }
        /// <summary>
        /// 取本地IP
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static DataTable LoadFileData(string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            string dataSource, tableName, connectionString, strCmd;
            dataSource = file.DirectoryName;
            tableName = file.Name;
            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};" +
                "Extended Properties=\"Text;HDR=Yes;FMT=Delimited\";Persist Security Info=False", dataSource);
            strCmd = string.Format("SELECT * FROM [{0}]", tableName);
            OleDbDataAdapter da = new OleDbDataAdapter(strCmd, connectionString);
            DataTable result = new DataTable();
            try
            {
                da.Fill(result);
            }
            catch (Exception err)
            {
                throw new ApplicationException("加载数据失败：" + err.Message, err);
            }
            da.Dispose();
            return result;
        }
    }
}