using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AutoScript.Share
{
    public class Config
    {
        public static IApplicationContext applicationContext = ContextRegistry.GetContext();
        public static Config AppConfig = (Config)applicationContext.GetObject("Config");
        public Config_DM Config_DM;
        public Config_AJ Config_AJ;
        /// <summary>
        /// 插件名称：如大漠 AJ、乐玩、OP等插件
        /// </summary>
        public string PlugeName;
        public Config_GameWindow Config_GameWindow;
    }
    public class Config_DM
    {
        /// <summary>
        /// 显示设置
        /// </summary>
        public string Display { get; set; }
        /// <summary>
        /// 键盘
        /// </summary>
        public string Mouse { get; set; }
        /// <summary>
        /// 鼠标
        /// </summary>
        public string KeyBorad { get; set; }
        /// <summary>
        /// 绑定模式
        /// </summary>
        public int Mode { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 多点文件
        /// </summary>
        public string PointInfoPath { get; set; }
        /// <summary>
        /// 图片文件信息
        /// </summary>
        public string PicInfoPath { get; set; }
        /// <summary>
        /// 字库文件
        /// </summary>
        public string DictFile { get; set; }
    }


    public class Config_AJ
    {
        /// <summary>
        /// 显示设置
        /// </summary>
        public string Display { get; set; }
        /// <summary>
        /// 键盘
        /// </summary>
        public string Mouse { get; set; }
        /// <summary>
        /// 鼠标
        /// </summary>
        public string KeyBorad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Flag {  get; set; }

        /// <summary>
        /// 绑定模式
        /// </summary>
        public int Mode { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 多点文件
        /// </summary>
        public string PointInfoPath { get; set; }
        /// <summary>
        /// 图片文件信息
        /// </summary>
        public string PicInfoPath { get; set; }
        /// <summary>
        /// 字库文件
        /// </summary>
        public string DictFile { get; set; }
    }
    public class Config_GameWindow
    {
        public string MemProcessName { get; set; }
        public string WindowProcessName { get; set; }
        public string WindowTitle { get; set; }
        public string WindowClassName { get; set; }
        public string PackageName { get; set; }
        public string ActivityName { get; set; }
    }
}