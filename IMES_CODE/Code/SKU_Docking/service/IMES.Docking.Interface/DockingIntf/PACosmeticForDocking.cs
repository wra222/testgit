//created by itc206070

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// [FA Cosmetic] 实现如下功能：
    /// 处理Product手动确认不良的业务
    /// </summary>
    public interface IPACosmeticDocking 
    {
        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="editor">Editor</param>
        /// <param name="stationId">Station</param>
        /// <param name="customer">Customer</param>
        /// <returns>prestation</returns>
        ArrayList InputCustSn(
            string pdLine,
            string prodID,
            string editor, string stationId, string customer);

        /// <summary>
        /// 不良品信息。
        /// </summary>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="defectList">Defect IList</param>
        void InputDefectCodeList(
            string custsn,
            IList<string> defectList);

        /// <summary>
        /// check pcid
        /// </summary>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="pcid">PCID</param>
        /// <returns></returns>
        bool checkpcid(
            string custsn, 
            string pcid);

        /// <summary>
        /// check wwan
        /// </summary>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="wwan">WWAN</param>
        /// <returns></returns>
        bool checkwwan(
            string custsn,
            string wwan);

        /// <summary>
        /// get Other Tips
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="swc">SWC</param>
        /// <returns>string</returns>
        string getOtherTips(
            string productId,
            string swc);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
