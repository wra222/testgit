// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-22   Lucy Liu                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IKPPrint
    {

        /// <summary>
        /// 打印KP标签
        /// </summary>
        /// <param name="inputSn">sn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="outputSn">输出sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        IList<PrintItem> Print(string inputSn, string pdLine, string stationId, string editor, string customerId, out string outputSn, IList<PrintItem> printItems);
        
    }
}