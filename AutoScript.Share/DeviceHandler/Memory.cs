using Microsoft.AspNetCore.SignalR.Client;
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
        public async Task<T> ReadMemoryByFeatureCode<T>(string search) where T : struct
        {
            
            T value= await device.Connection.InvokeAsync<T>("ReadMemoryByFeatureCode", this.device.Pid,search);

            return value;
            
        }

        public void WriteMemValue()
        {
            throw new NotImplementedException();
        }
    }
}