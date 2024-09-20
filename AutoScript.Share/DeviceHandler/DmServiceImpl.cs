using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Share
{
    public class DmServiceImpl : IDmService
    {
        private Dm.dmsoft _dmInstance;
        public DmServiceImpl()
        {
            _dmInstance=new Dm.dmsoft();
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
