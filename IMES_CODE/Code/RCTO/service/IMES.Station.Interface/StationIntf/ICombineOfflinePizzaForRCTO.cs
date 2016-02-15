/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for CombineOfflinePizzaForRCTO
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 
* Known issues:
* TODO：
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface ICombineOfflinePizzaForRCTO
    {
        /// <summary>
        /// InputCartonId
        /// </summary>
        ArrayList InputCartonId(string cartonID, IList<PrintItem> printItems, string pdLine, string editor, string stationId, string customerId);
		
        /// <summary>
        /// InputPizzaId
        /// </summary>
        ArrayList InputPizzaId(string cartonID, string pizzaId, string pdLine, string editor, string stationId, string customerId);
		
		/// <summary>
        /// RePrint
        /// </summary>
        ArrayList RePrint(string cartonID, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);
		
        /// <summary>
        /// 取消工作流
        /// </summary>
        void Cancel(string pcbno);
    }
}