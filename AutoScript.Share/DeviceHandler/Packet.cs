using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    /// <summary>
    /// 封包收发实现类
    /// </summary>
    public class Packet : IDevicePacket
    {
        private IDevice device;
        public Packet(IDevice device)
        {
            this.device = device;
        }
        public void Intercept()
        {
            throw new NotImplementedException();
        }

        public void Recv()
        {
            throw new NotImplementedException();
        }

        public void Send()
        {
            throw new NotImplementedException();
        }
    }
}