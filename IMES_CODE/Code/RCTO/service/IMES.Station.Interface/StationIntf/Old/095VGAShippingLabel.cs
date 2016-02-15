/* INVENTEC corporation (c)2010 all rights reserved. 
 * Description: VGA Label Print 用于列印VGA Shipping Label。
 * 
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ===========================
 * 2010-02-01   Tong.Zhi-Yong                create
 * Known issues:
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
    /// VGA Label Print 用于列印VGA Shipping Label。
    /// </summary>
    public interface IVGAShippingLabel
    {
        /// <summary>
        /// 列印VGA Shipping Label。
        /// </summary>
        /// <param name="DCodeType">DCodeType</param>
        /// <param name="Version">Version</param>
        /// <param name="Qty">要打印的数量</param>
        /// <param name="VGASno">VGASno</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        /// <returns>IList<PrintItem></returns>
        IList<PrintItem> PrintVGAShippingLabel(
            string DCodeType,
            string Version,
            int Qty,
            string VGASno,
            IList<PrintItem> printItems,
            string editor, string stationId, string customer, out string FRUNo, out IList<string> snList, out string dcode);
    }
}
