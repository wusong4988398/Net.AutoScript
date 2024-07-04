using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    public interface IDeviceMemory
    {
        void ReadMemValue();
        void WriteMemValue();
    }
}