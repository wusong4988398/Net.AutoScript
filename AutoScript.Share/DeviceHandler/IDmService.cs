using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Share
{
    public interface IDmService
    {
        /// <summary>
        /// 绑定窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="display"></param>
        /// <param name="mouse"></param>
        /// <param name="keyboard"></param>
        /// <param name="mode"></param>
        void BindWindow(int hWnd, string display, string mouse, string keyboard, int mode);
        /// <summary>
        /// 设置路径
        /// </summary>
        /// <param name="path"></param>
        void SetPath(string path);
        /// <summary>
        /// 设置窗口大小
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void SetWindowSize(int hWnd, int width, int height);


    }
}
