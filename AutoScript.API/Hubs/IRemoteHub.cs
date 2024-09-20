namespace AutoScript.API.Hubs
{
    public interface IRemoteHub
    {
        /// <summary>
        /// 读取特征码
        /// </summary>
        /// <param name="search"></param>
        /// <param name="writable"></param>
        /// <param name="executable"></param>
        /// <returns></returns>
        Task<IEnumerable<long>> AoBScan(int pid, string search, bool writable = false, bool executable = true);
        /// <summary>
        /// 根据特征码读取内存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<string> ReadMemoryByFeatureCode(int pid, string search);
       
    }
}