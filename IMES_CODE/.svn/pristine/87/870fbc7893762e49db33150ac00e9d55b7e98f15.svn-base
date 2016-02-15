using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [Rework CPQSNO Upload] 实现如下功能：
    /// 1.	Upload
    /// 2.	Confirm
    /// 3.	Release
    /// 值得注意的是，客户提出 [Rework CPQSNO Upload] 在同一时刻只能处理一个
    /// Rework CPQSNO Upload 业务。
    /// </summary>
    public interface IReworkCPQSNOUpload
    {
        /// <summary>
        /// 上传CPQSNO
        /// </summary>
        /// <param name="CPQSNOList">CPQSNO列表</param>
        /// <param name="editor">operator</param>
        void UploadCPQSNO(
            IList<string> CPQSNOList,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 确认???
        /// </summary>
        void Confirm();

        /// <summary>
        /// Release Cpq Sno to DB
        /// </summary>
        /// <param name="partTypeList">选定的PartType列表</param>
        void Release(IList<string> partTypeList);
    }
}
