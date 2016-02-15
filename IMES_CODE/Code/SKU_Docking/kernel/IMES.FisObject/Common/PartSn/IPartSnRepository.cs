using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.Common.PartSn
{
    public interface IPartSnRepository : IRepository<PartSn>
    {
        /// <summary>
        /// 根据Vendor SN取PartSn
        /// </summary>
        /// <param name="vendorSn">vendorSn</param>
        /// <returns>PartSn</returns>
        PartSn FindPartSnByVendorSn(string vendorSn);

        /// <summary>
        /// 获取不同时间段内KeyParts已结合的数量
        /// 夜班：前一天晚上20:30到早上7:30
        /// 白班：早上8:00到晚上20:30
        /// 线别最后一位D是白班(从早上8:00开始)，N是夜班(从晚上20:30开始)
        /// 注：
        /// A.若当天班次还没有到开班时间，则取前一天的数量
        /// B.从PartSN表里按照PartType获取时间段内已结合的数量
        /// </summary>
        /// <returns></returns>
        int GetCombineCountByPartType(string partType, DateTime startTime, DateTime endTime);
    }
}
