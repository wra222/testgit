// INVENTEC corporation (c)2012 all rights reserved. 
// Description: MBBorrowControl Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-10   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IKBEsop
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        ArrayList InputKey(string key, string editor, string line, string station, string customer);

    }
}
