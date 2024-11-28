using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Share
{
    /// <summary>
    /// 停止判断委托
    /// </summary>
    /// <returns></returns>
    public delegate bool CheckStopCallBack();
    public delegate bool CheckPauseCallBack();


    public delegate string Notify(NotifyArgs arg);
    public class ImageInfo
    {
        public string Name = "";
        public (int x1, int y1, int x2, int y2) Range = (0, 0, 800, 600);
        public (int x, int y) Offset = (0, 0);
        public string Color = "";
        public string Offset_color = "000000";
        public double Sim = 0.8;
        public int Dir = 0;
        public string PicName = "";
        public (Int32 x, Int32 y, bool isFinded) Result = (-1, -1, false);
        public DetectMode DetectMode { get; set; } = DetectMode.FindMultiColor;
        public Bitmap? Pic { get; set; }
        public static List<ImageInfo> AllImageInfos = new List<ImageInfo>();
        public static List<ImageInfo> Reload()
        {
            DataTable result = Utility.LoadFileData("Resource/ImageInfos.txt", Encoding.UTF8);
            if (AllImageInfos.Count > 0)
            {
                return AllImageInfos;
            }
            //类型转换
            AllImageInfos.Clear();
            foreach (DataRow row in result.Rows)
            {
                ImageInfo info = new ImageInfo()
                {
                    Name = row["name"].ToString(),
                    PicName = row["picname"]?.ToString(),
                    Range = (GetIntVal(row["x1"]), GetIntVal(row["y1"]), GetIntVal(row["x2"]), GetIntVal(row["y2"])),
                    Offset = (GetIntVal(row["offsetX"]), GetIntVal(row["offsetY"])),
                    Sim = double.Parse(row["sim"]?.ToString()),
                    Color = row["color"]?.ToString(),
                    ContainText= row["containText"]?.ToString(),
                    Offset_color = row["offsetColor"]?.ToString()
                };
                AllImageInfos.Add(info);
            }
            return AllImageInfos;
        }
        public static int GetIntVal(object val)
        {
            int ret = 0;
            try
            {
                ret = int.Parse(val?.ToString());
            }
            catch (Exception)
            {
            }
            return ret;
        }
        /// <summary>
        /// Ocr识别后的文字
        /// </summary>
        public string OcrString { get; set; } = "";
        /// <summary>
        /// 希望包含的文字
        /// </summary>
        public string ContainText { get; set; } = "";


    }
    /// <summary>
    /// 动作对象
    /// </summary>
    public class ActionParam
    {
        /// <summary>
        /// 点击目标的坐标
        /// </summary>
        public (int x, int y) Point = (0, 0);
        /// <summary>
        /// 要发送的字符串
        /// </summary>
        public string SendStr = "";
        /// <summary>
        /// 要发送的字符码
        /// </summary>
        public int KeyCode = 0;
        /// <summary>
        /// 滑动动作的起点终点
        /// </summary>
        public (int x1, int y1, int x2, int y2) Swipe;
        /// <summary>
        /// 动作延迟
        /// </summary>
        public int delay = 100;
        /// <summary>
        /// 动作类型
        /// </summary>
        public ActionType ActionType = ActionType.Click;
        /// <summary>
        /// 未找到目标次数,如果大于0 则如果执行的次数超过这个数，则结束任务
        /// </summary>
        public int NotFindCount = 0;
    }
    /// <summary>
    /// 动作类型
    /// </summary>
    public enum ActionType
    {
        None = 0,
        Click = 1,
        SendStr = 2,
        SendKey = 4,
        Swipe = 8
    }
    /// <summary>
    /// 脚本运行状态
    /// </summary>
    public enum GameHelperStatus
    {
        Ready=0,
        Running=1,
        Stoped = 2,
        Offline = 3,
        Finish =4
    }
    /// <summary>
    /// GameHelper产生变化时候的事件自定义参数
    /// </summary>
    public class GameHelperArgs : EventArgs
    {
        public string Status { get; set; }
        public string DeviceName { get; set; }
        public int Index { get; set; }
        public string Script { get; set; }
        public int Hwnd {  get; set; }  
    }
    /// <summary>
    /// 图色检测方式
    /// </summary>
    public enum DetectMode
    {
        Default = 0,
        FindImage = 1,         //找图
        FindMultiColor = 2,  //多点找色
        FindColor = 3,          //找色
        Template = 4,       //模板匹配（OpenCV）
        ImageClassification = 5,          //图片分类（ML）
        OCR = 6,     //飞桨OCR
    }
    /// <summary>
    /// 传递消息用的委托
    /// </summary>
    /// <param name="notifyArgs"></param>
    public delegate void GameHelperNotify(NotifyArgs notifyArgs);

    /// <summary>
    /// 消息委托的参数
    /// </summary>
    public class NotifyArgs:EventArgs
    {
        /// <summary>
        /// 要让对方执行的功能名
        /// </summary>
        public string Cmd { get; set; }
        /// <summary>
        /// 自己的状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 自己是否是队长
        /// </summary>
        public bool IsLeader { get; set; }
        /// <summary>
        /// 账号ID
        /// </summary>
        public int AccountIndex { get; set; }
        /// <summary>
        /// 发出者的角色ID
        /// </summary>
        public string RoleID  { get; set; }
    }
}
                       