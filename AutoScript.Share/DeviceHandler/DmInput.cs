using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace AutoScript.Share
{
    public class DmInput : IDeviceInput
    {
        //private Dm.dmsoft dm = new Dm.dmsoft();
        private Dm.dmsoft dm;
        public IDevice Device { get; set; }

        
        public DmInput(IDevice device, Dm.dmsoft dm)
        {
            this.Device = device;
            this.dm = dm;
        }

        public  void 点击(ActionParam param)
        {
            if ((param.ActionType | ActionType.Click) != param.ActionType)
            {
                return;
            }
            dm.MoveTo(param.Point.x, param.Point.y);
            Thread.Sleep(param.delay);
            dm.LeftClick();
        }

        //long SendString(hwnd, str)
        public  void 发送字符串(ActionParam param)
        {
            if ((param.ActionType | ActionType.SendStr) != param.ActionType)
            {
                return;
            }
            dm.MoveTo(param.Point.x, param.Point.y);
            dm.SendString(Device.Hwnd, param.SendStr);

        }
        //long KeyPress(vk_code)
        public  void 发送按键(ActionParam param)
        {
            if ((param.ActionType | ActionType.SendKey) != param.ActionType)
            {
                return;
            }
            dm.KeyPress(param.KeyCode);
        }
        public  void 滑动(ActionParam param)
        {
            if ((param.ActionType | ActionType.Swipe) != param.ActionType)
            {
                return;
            }
            dm.MoveTo(param.Swipe.x1, param.Swipe.y1);
            dm.LeftDown();
            Thread.Sleep(param.delay);
            dm.MoveTo(param.Swipe.x2, param.Swipe.y2);
            dm.LeftUp();
        }

        public void 启动APP()
        {
            throw new NotImplementedException();
        }

        public void 退出APP()
        {
            throw new NotImplementedException();
        }
    }
}