// INVENTEC corporation (c)2012 all rights reserved. 
// Description: DismantlePalletWeight Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-20   Yuan XiaoWei                 create
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
    public interface IDismantlePalletWeight
    {
        /// <summary>
        /// query pallet weight
        /// </summary>
        /// <param name="palletOrDn"></param>
        IList<DNPalletWeight> Query(string palletOrDn);

        /// <summary>
        /// Dismantle Pallet weight
        /// </summary>
        /// <param name="palletOrDn"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        IList<DNPalletWeight> Dismantle(string palletOrDn, string editor, string line, string station, string customer);

    }
}
