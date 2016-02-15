/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Interface for RCTO MB Change Page
* UI:CI-MES12-SPEC-SA-UI RCTO MB Change.docx –2012/6/15 
* UC:CI-MES12-SPEC-SA-UC RCTO MB Change.docx –2012/6/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-8-1    Jessica Liu           Create
* Known issues:
* TODO：
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IRCTOMBChange
    {
        /// <summary>
        /// Check MB SNo
        /// </summary>
        /// <param name="mbSno">mbSno</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <returns>mbSno和Model值</returns>
        List<string> CheckMBSNo(string mbSno, string pdLine, string stationId, string editor, string customerId);


        /// <summary>
        /// Save And Print
        /// </summary>
        /// <param name="mbSno">mbSno</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        /// <param name="newMBSno">new MBSno</param>
        IList<PrintItem> SaveAndPrint(string mbSno, string pdLine, string stationId, string editor, string customerId, IList<PrintItem> printItems, out string newMBSno);


        /// <summary>
        /// Reprint
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="reason"></param>
        /// <param name="pdLine"></param>
        /// <param name="customerId"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> RePrint(string mbSno, string reason, string pdLine, string customerId, string editor, string stationId, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}