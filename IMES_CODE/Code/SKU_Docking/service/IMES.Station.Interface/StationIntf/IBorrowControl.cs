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

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBorrowControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        string InputKey(string key, string editor, string line, string station, string customer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MBSno"></param>
        /// <param name="borrowerOrReturner"></param>
        /// <param name="action"></param>
        void Save(string key, string borrowerOrReturner, string action);


        /// <summary>
        /// 取消workflow
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
