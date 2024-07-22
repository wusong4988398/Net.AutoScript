using Memory;
using Microsoft.AspNetCore.SignalR;

namespace AutoScript.API.Hubs
{
    public class RemoteHub : Hub, IRemoteHub
    {
        private Mem _memLib;
        public RemoteHub(Mem memLib)
        {
            this._memLib = memLib; 
        }

        public async Task<IEnumerable<long>> AoBScan(int pid,string search, bool writable = false, bool executable = true)
        {

            if (!_memLib.OpenProcess(pid)) return Enumerable.Empty<long>();
            return await _memLib.AoBScan(search, writable, executable);

        }

        
        public async Task<T?> ReadMemoryByFeatureCode<T>(int pid, string search)
        {
            IEnumerable<long> AoBScanResults = await AoBScan(pid,search, true, true);
            if (!AoBScanResults.Any()) return default;
            string realAddress = Convert.ToString(AoBScanResults.FirstOrDefault(), 16);
            T value =_memLib.ReadMemory<T>(realAddress);
            return value;

        }
    }
}
