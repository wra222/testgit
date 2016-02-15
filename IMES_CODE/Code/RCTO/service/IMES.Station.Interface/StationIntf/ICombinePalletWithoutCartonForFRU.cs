/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description: Combine Pallet Without Carton For FRU
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
    public interface ICombinePalletWithoutCartonForFRU
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ArrayList GetDnList(string dn, string shipDate, string model, string line, string editor, string station, string customer);
		
		/// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ArrayList GetPalletList(string dn, string line, string editor, string station, string customer);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ArrayList GetCntCartonOfPallet(string palletNo, string line, string editor, string station, string customer);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ArrayList Save(string dnsn, string palletNo, IList<PrintItem> printItems, string line, string editor, string station, string customer);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void cancel(string deliverNo);
		
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ArrayList Reprint(string dnsn, string palletNo, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems);
        
    }
}
