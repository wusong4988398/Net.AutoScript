using AoJiaLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Share
{
    /// <summary>
    /// AJ插件输入
    /// </summary>
    public class AJInput : IDeviceInput
    {
        //private Dm.dmsoft dm = new Dm.dmsoft();
        private AoJiaD aj;
        public IDevice Device { get; set; }


        public AJInput(IDevice device, AoJiaD aj)
        {
            this.Device = device;
            this.aj = aj;
        }

        public void 点击(ActionParam param)
        {
            if ((param.ActionType | ActionType.Click) != param.ActionType)
            {
                return;
            }
            aj.MoveTo(param.Point.x, param.Point.y);
            Thread.Sleep(param.delay);
            aj.LeftClick();
        }

        //long SendString(hwnd, str)
        public void 发送字符串(ActionParam param)
        {
            if ((param.ActionType | ActionType.SendStr) != param.ActionType)
            {
                return;
            }
            aj.MoveTo(param.Point.x, param.Point.y);
            aj.SendString(Device.Hwnd, param.SendStr, 10, 20, 0, 1);

        }
        //long KeyPress(vk_code)
        public void 发送按键(ActionParam param)
        {
            if ((param.ActionType | ActionType.SendKey) != param.ActionType)
            {
                return;
            }
            aj.KeyPress((ushort)param.KeyCode);
        }
        public void 滑动(ActionParam param)
        {
            if ((param.ActionType | ActionType.Swipe) != param.ActionType)
            {
                return;
            }
            aj.MoveTo(param.Swipe.x1, param.Swipe.y1);
            aj.LeftDown();
            Thread.Sleep(param.delay);
            aj.MoveTo(param.Swipe.x2, param.Swipe.y2);
            aj.LeftUp();
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
