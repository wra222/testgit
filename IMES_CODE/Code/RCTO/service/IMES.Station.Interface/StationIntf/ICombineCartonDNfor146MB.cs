/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description: Combine Carton DN for 146MB
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
    public interface ICombineCartonDNfor146MB
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ArrayList InputMBSN(string mbsn, bool isGetDn, string shipMode, string line, string editor, string station, string customer);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ArrayList GetDnQty(string dn, string model, string shipMode, string line, string editor, string station, string customer);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ArrayList Save(IList<string> mbsns, string dnsn, string usedModel, IList<PrintItem> printItems, string shipMode, string line, string editor, string station, string customer);

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

         void CheckFRUMBOA3(string mbsn, string usedmodel);
        
    }
}
