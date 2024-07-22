using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
namespace AutoScript.Share
{
    public static class Utility
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
        public static DataTable LoadFileData(string fileName, Encoding encoding)
        {
            //DataTable dataTable = new DataTable();
            //try
            //{
            //    using (StreamReader reader = new StreamReader(fileName, encoding))
            //    {
            //        // 读取第一行作为列名
            //        string[] headers = reader.ReadLine().Split(',');
            //        foreach (string header in headers)
            //        {
            //            dataTable.Columns.Add(header);
            //        }

            //        // 逐行读取数据
            //        while (!reader.EndOfStream)
            //        {
            //            //string[] rows = reader.ReadLine().Split(',');
            //            string[] rows = Regex.Split(reader.ReadLine(), @",(?=(?:[^\""]|\""[^\""]*\"")*$)");
            //            DataRow dataRow = dataTable.NewRow();
            //            for (int i = 0; i < headers.Length; i++)
            //            {
            //                dataRow[i] = rows[i].Replace("\"", "");
            //            }
            //            dataTable.Rows.Add(dataRow);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error reading file: " + ex.Message);
            //}
            //return dataTable;


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

        /// <summary>
        /// Bitmap转化为Base64
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static string ConvertToBase64(this Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // 将Bitmap保存到MemoryStream中
                bitmap.Save(memoryStream, bitmap.RawFormat);
                // 将MemoryStream转换为字节数组
                byte[] imageBytes = memoryStream.ToArray();
                // 将字节数组转换为Base64字符串
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static string ImgToBase64String(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public static string SendPost(string url, IDictionary<string, string> parameters, IDictionary<String, String> headers, String json)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }

            HttpWebRequest req = GetWebRequest(url, "POST");
            req.ContentType = "application/json;charset=utf-8";


            if (headers != null)
            {
                foreach (var item in headers)
                {
                    req.Headers.Add(item.Key, item.Value);
                }


            }


            byte[] postData = Encoding.UTF8.GetBytes(json);
            System.IO.Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            return GetResponseAsString(rsp, Encoding.UTF8);
        }
        private static HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "SEASHOP";
            req.Timeout = 300000;
            return req;
        }
        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            StringBuilder result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);

                // 按字符读取并写入字符串缓冲
                int ch = -1;
                while ((ch = reader.Read()) > -1)
                {
                    // 过滤结束符
                    char c = (char)ch;
                    if (c != '\0')
                    {
                        result.Append(c);
                    }
                }
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }

            return result.ToString();
        }
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    hasParam = true;
                }
            }

            return postData.ToString();
        }


        /// <summary>
        /// 转整形
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static int ToInt(this object thisValue)
        {
            int reval = 0;
            if (thisValue == null)
            {
                return 0;
            }

            if (thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }

            return reval;
        }

        /// <summary>
        /// 转整形
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static long ToInt64(this object thisValue)
        {
            long reval = 0;
            if (thisValue == null)
            {
                return 0;
            }

            if (thisValue != DBNull.Value && Int64.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }

            return reval;
        }



        /// <summary>
        /// 转数字
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object thisValue)
        {
            if (thisValue != null && thisValue != DBNull.Value &&
                decimal.TryParse(thisValue.ToString(), out decimal reval))
            {
                return reval;
            }


            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static double ToDouble(this object thisValue)
        {
            if (thisValue != null && thisValue != DBNull.Value &&
                double.TryParse(thisValue.ToString(), out double reval))
            {
                return reval;
            }


            return 0;
        }


        /// <summary>
        /// 转非空字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToNullString(this object obj)
        {
            return (((obj == null) && (obj != DBNull.Value)) ? string.Empty : obj.ToString().Trim());
        }
    }
}