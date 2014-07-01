using System;

namespace HotelSupervisorClient.Managers
{
    /// <summary>
    /// Guid操作类。
    /// </summary>
    internal class GuidManager
    {
        /// <summary>
        /// 获得Guid。
        /// </summary>
        /// <returns>Guid。</returns>
        public static string GetNewGuid()
        {
            return Guid.NewGuid().ToString("B");
        }
    }
}
