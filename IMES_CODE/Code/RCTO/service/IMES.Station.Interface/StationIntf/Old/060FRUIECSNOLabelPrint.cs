/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: IEC SNO label print
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-02-09   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 根据产线生产安排，对FRU出货的part打印出label
    /// FRU Part不需要与Vendor SN结合
    /// </summary>
    public interface IFRUIECSNOLabelPrint
    {
        /// <summary>
        /// Input Assembly Code
        /// </summary>
        /// <param name="assemblyCode">assemblyCode</param>
        /// <param name="fruNo">fruNo(Out)</param>
        /// <param name="vcode">vcode(Out)</param>
        /// <param name="editor">editor</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        void InputAssemblyCode(string assemblyCode,
            out string fruNo, 
            out string vcode, 
            out string partNo,
            out string key,
            string editor, 
            string stationId, 
            string customer);

        /// <summary>
        /// Print
        /// </summary>
        /// <param name="_151">_151</param>
        /// <param name="vcode">vcode</param>
        /// <param name="DateCode">DateCode</param>
        /// <param name="vendorDCCode">vendorDCCode</param>
        /// <param name="Qty">Qty</param>
        /// <param name="printItems">printItems</param>
        /// <param name="editor">editor</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        /// <param name="snList">snList</param>
        /// <returns>list of PrintItem</returns>
        IList<PrintItem> Print(
            string key,
            string assemblyCode,
            string _151,
            string vcode, 
            string DateCode,
            string vendorDCCode,
            int Qty,
            IList<PrintItem> printItems, 
            string editor, 
            string stationId, 
            string customer, 
            out IList<string> snList);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string key);        
    }
}
