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

        
        public async Task<string> ReadMemoryByFeatureCode(int pid, string search)
        {
            IEnumerable<long> AoBScanResults = await AoBScan(pid,search, true, true);
            if (!AoBScanResults.Any()) return "";
            string realAddress = Convert.ToString(AoBScanResults.FirstOrDefault(), 16);
            string value =_memLib.ReadString(realAddress);
            return value;

        }
    }
}
