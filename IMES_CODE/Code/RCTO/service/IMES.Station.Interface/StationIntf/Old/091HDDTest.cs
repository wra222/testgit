// INVENTEC corporation (c)2010 all rights reserved. 
// Description: HDDTEST Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-04   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 记录测试HDD作业的机台、Origin HDD和数据传输使用的Connector No。
    /// 目的：当后续有不良时，可以判断不良的原始HDD、机台和连接端口，减少重工风险
    /// </summary>
    public interface IHDDTest
    {
        /// <summary>
        /// 记录测试HDD作业的机台、Origin HDD和数据传输使用的Connector No。
        /// </summary>
        /// <param name="machineNo">测试机台编号</param>
        /// <param name="originalHDD">母HDD SN</param>
        /// <param name="connectNo">数据传输的连接端口编号</param>
        /// <param name="testHDDSn">Test HDD SN</param>
        /// <param name="editor">operator</param>
        /// <param name="line">line</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="pcode">pcode</param>
        void TestHDD(
            string machineNo,
            string originalHDD,
            string connectNo,
            string testHDDSn,
            string editor, string line, string station, string customer,string pcode);

        /// <summary>
        /// 检查输入的ConnectNo使用次数是否超过600次
        /// </summary>
        /// <param name="connectNo"></param>
        void CheckConnector(string connectNo);
    }
}
