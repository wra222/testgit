/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description: Combine Carton Pallet For 146
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* Known issues:
* TODO：
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// </summary>
    public interface ICombineCartonPalletFor146
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ArrayList Save(string cartonSn, IList<PrintItem> printItems, string line, string editor, string station, string customer);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void cancel(string deliverNo);
		
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ArrayList Reprint(string inputSN, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems);
        
    }
}
