using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Share
{
    public class DmServiceImpl : IDmService
    {
        private Idmsoft _dmInstance;
        public DmServiceImpl()
        {
            _dmInstance = DMP.CreateDM();
        }
        public void BindWindow(int hWnd, string display, string mouse, string keyboard, int mode)
        {
            // 调用大漠插件的BindWindow方法
            _dmInstance.BindWindow(hWnd, display, mouse, keyboard, mode);
        }

        public void SetPath(string path)
        {
            _dmInstance.SetPath(path);
        }

        public void SetWindowSize(int hWnd, int width, int height)
        {
            _dmInstance.SetWindowSize(hWnd, width, height);
        }
    }
}
