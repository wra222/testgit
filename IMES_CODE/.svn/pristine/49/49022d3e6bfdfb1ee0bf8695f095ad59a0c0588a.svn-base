// INVENTEC corporation (c)2012 all rights reserved. 
// Description: DTPalletControl Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-21   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDTPalletControl
    {
        /// <summary>
        /// query dt pallet
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IList<WhPltLogInfo> Query(string palletNo, string from, string to);

        /// <summary>
        /// Dismantle Pallet weight
        /// </summary>
        /// <param name="palletOrDn"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        IList<WhPltLogInfo> DT(string palletNo, string editor, string line, string station, string customer);

    }
}
