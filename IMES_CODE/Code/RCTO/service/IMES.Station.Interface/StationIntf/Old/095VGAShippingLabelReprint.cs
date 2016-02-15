// INVENTEC corporation (c)2010 all rights reserved. 
// Description: IVGAShippingLabelReprint Interface
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
using IMES.DataModel;
namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 检查SVB,获取FruNo，PartNo
    /// 检查通过后，获取PrintItem
    /// </summary>
    public interface IVGAShippingLabelReprint
    {
        /// <summary>
        /// 检查SVB,获取FruNo，PartNo
        /// </summary>
        /// <param name="dcode">dcode</param>
        /// <param name="version">version</param>
        /// <param name="svbsno">svbsno</param>
        /// <param name="editor">operator</param>
        /// <param name="line">line</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns></returns>
        SVBInfo CheckSVB(
            int dcode,
            string version,
            string svbsno,
            string editor, string line, string station, string customer);

        /// <summary>
        /// 检查通过后，获取PrintItem
        /// </summary>
        /// <param name="svbsno"></param>
        /// <param name="printItems"></param>
        /// <param name="reason"></param>
        /// <param name="dcode"></param>
        /// <returns></returns>
        IList<PrintItem> Print(string svbsno, IList<PrintItem> printItems, string reason, out string dcode);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="svbsno"></param>
        void Cancel(string svbsno);
    }
}
