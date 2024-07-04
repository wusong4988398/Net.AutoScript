using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    public class Memory : IDeviceMemory
    {
        private IDevice device;
        public Memory(IDevice device)
        {
            this.device = device;
        }
        public void ReadMemValue()
        {
            throw new NotImplementedException();
        }

        public void WriteMemValue()
        {
            throw new NotImplementedException();
        }
    }
}