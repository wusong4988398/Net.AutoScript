using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    /// <summary>
    /// 封包收发类
    /// </summary>
    public interface IDevicePacket
    {
        void Send();
        void Recv();
        void Intercept();
    }
}