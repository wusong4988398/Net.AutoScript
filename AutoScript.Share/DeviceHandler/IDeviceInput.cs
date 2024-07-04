using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    public interface IDeviceInput
    {
        IDevice Device { get; set; }
        void 点击(ActionParam actionParam);
        void 发送字符串(ActionParam actionParam);
        void 发送按键(ActionParam actionParam);
        void 滑动(ActionParam actionParam);
        void 启动APP();
        void 退出APP();
    }
}