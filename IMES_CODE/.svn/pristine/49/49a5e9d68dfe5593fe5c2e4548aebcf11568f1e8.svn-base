/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PCAShippingLabelPrint
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-12-01   zhu lei           Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [PCA Test Station] 实现如下功能：
    /// 记录Function测试结果：如有不良,还需记录不良信息
    /// </summary>
    public interface IPCAShippingLabelPrint 
    {
        /// <summary>
        /// 输入MBSNo, 然后卡站，最后返回MBInfo。
        /// </summary>
        /// <param name="pdLine">pdLine</param>
        /// <param name="MBSNo">MBSNo</param>
        /// <param name="checkPCMBRCTOMB">checkPCMBRCTOMB</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customerId</param>
        /// <returns>MBInfo</returns>
        IList InputMBSNo(
            string pdLine,
            string MBSNo,
            string checkPCMBRCTOMB,
            string editor, string station, string customerId);
       
        /// <summary>
        /// save
        /// </summary>
        /// <param name="MBno">MBno</param>
        /// <param name="model">model</param>
        /// <param name="dcode">dcode</param>
        /// <param name="region">region</param>
        /// <param name="printItems">printItems</param>
        /// <returns>printItems</returns>
        ArrayList save(
            string MBno,
            string model,
            string dcode,
            string region,
            IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        /// <summary>
        /// SetDataCodeValue
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        string SetDataCodeValue(string model, string customer);

        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        ArrayList ReprintLabel(string mbSno, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);
    }
}
