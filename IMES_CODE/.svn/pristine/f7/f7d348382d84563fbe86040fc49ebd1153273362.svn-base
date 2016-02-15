/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service interface for ConbimeOfflinePizza Page
 *             
 * UI:
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 
 * Known issues:
*/

using System.Collections;
using System.Collections.Generic;
using System.Data;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Conbime Offline Pizza
    /// </summary>
    public interface IConbimeOfflinePizza
    {
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="custSN">custSN</param>
        /// <param name="pizzaId">pizzaId</param>
		/// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param> 
        ArrayList Save(string custSN, string pizzaId, string line, string editor, string station, string customer, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sesKey">SessionKey(custSN)</param>
        void Cancel(string sesKey);

        /// <summary>
        /// 获取Product
        /// </summary>
        /// <param name="custSN">custSN</param>
		/// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        ArrayList getProduct(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems);
		
		/// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);
		
		ArrayList Reprint(string customerSN, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);

    }
}
