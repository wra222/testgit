// INVENTEC corporation (c)2009 all rights reserved. 
// Description: IECLabelPrint
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-28   zhu lei                      create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 实现IECLabelPrint打印和重印功能
    /// </summary>
    public interface IIECLabelPrint
    {
        #region "methods interact with the running workflow"


        /// <summary>
        /// 打印标签
        /// </summary>
        /// <param name="dataCode"></param>
        /// <param name="supplierCode"></param>
        /// <param name="vendorCode"></param>
        /// <param name="partNoKP"></param>
        /// <param name="rev"></param>
        /// <param name="qty"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList Print(string dataCode, string config, string rev, string qty, string line, string editor, string station, string customer, List<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="vendorCT"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList ReprintLabel(string vendorCT, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);

        #endregion

        #region "methods do not interact with the running workflow"

        /// <summary>
        /// SetDataCodeValue
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customer"></param>
        /// <returns>string</returns>
        string SetDataCodeValue(string model, string customer);

        #endregion

    }
}
