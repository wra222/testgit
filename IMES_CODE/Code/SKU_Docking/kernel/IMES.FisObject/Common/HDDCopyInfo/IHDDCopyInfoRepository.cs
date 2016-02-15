using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.Common.HDDCopyInfo
{
    public interface IHDDCopyInfoRepository : IRepository<HDDCopyInfo>
    {
        /// <summary>
        /// 獲得Connector使用次數
        /// HDDCopyInfo.ConnectorID=connetct#
        /// </summary>
        /// <param name="connectorNo">connectorNo</param>
        /// <returns></returns>
        int GetCountByConnectorNo(string connectorNo);
    }
}
