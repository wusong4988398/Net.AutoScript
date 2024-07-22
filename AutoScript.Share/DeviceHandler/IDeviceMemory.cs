using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    public interface IDeviceMemory
    {
        /// <summary>
        /// 依据特征码读取内存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<T> ReadMemoryByFeatureCode<T>(string search) where T : struct;

        void WriteMemValue();
    }
}