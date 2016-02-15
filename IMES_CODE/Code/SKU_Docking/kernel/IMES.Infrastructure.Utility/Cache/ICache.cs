using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Infrastructure.Utility
{
    /// <summary>
    /// Cache相关接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Cache功能是否打开
        /// </summary>
        /// <returns></returns>
        bool IsCached();

        /// <summary>
        /// 处理一个Cache更新请求
        /// </summary>
        /// <param name="item"></param>
        void ProcessItem(CacheUpdateInfo item);
    }
}
