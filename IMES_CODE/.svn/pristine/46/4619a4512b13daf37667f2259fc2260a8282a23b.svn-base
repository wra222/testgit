// INVENTEC corporation (c)2012 all rights reserved. 
// Description: ICT Input Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-16   Yuan XiaoWei                 create
// 2012-01-17   Yang Weihua                  add methods for MBReinput, ECRReprint
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPalletCollection
    {
        /// <summary>
        /// scan carton
        /// </summary>
        /// <param name="carton"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputCarton(string carton, string floor, string editor, string line, string station, string customer, IList<PrintItem> printItems);


        #region Reprint

        /// <summary>
        /// reprint label
        /// </summary>
        /// <param name="carton"></param>
        /// <param name="reason"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        ArrayList Reprint(
            string carton,
            string reason,
            string editor,
            string line,
            string station,
            string customer,
            IList<PrintItem> printItems);
        #endregion


    }
}
