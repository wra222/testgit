/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for COAStatusChange Page            
 * CI-MES12-SPEC-PAK COA Status Change.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc207003              Create
 * Known issues:
*/

using System;
using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;


namespace IMES.Station.Interface.StationIntf
{
    [Serializable]
    /// <summary>
    /// Row data define of COAStatusChange page
    /// </summary>
    public struct S_RowData_COAStatus
    {
        /// <summary>
        /// Status
        /// </summary>
        public string Status;
        /// <summary>
        /// Pno
        /// </summary>
        public string Pno;
        /// <summary>
        /// pdLine
        /// </summary>
        public string pdLine;
        /// <summary>
        /// CoaNO
        /// </summary>
        public string CoaNO;
    };

    /// <summary>
    /// COA Status Change
    /// </summary>
    public interface ICOAStatusChange
    {
        /// <summary>
        /// 获取COAStatus表相关信息
        /// </summary>
        /// <param name="begNO">begin NO</param>
        /// <param name="endNO">end NO</param>
        IList<S_RowData_COAStatus> GetCOAList(string begNO, string endNO);
        /// <summary>
        /// Update COAStatus
        /// </summary>
        /// <param name="begNO">begin NO</param>
        /// <param name="endNO">end NO</param>
        /// <param name="editor">editor</param>
        /// <param name="status">status</param>
        /// <param name="pdLine">pdLine</param>
        /// <param name="station">station</param> 
        void UpdateCOA(string begNO, string endNO, string editor, string status, string pdLine,  string station);
        /// <summary>
        /// Receive COA
        /// </summary>
        /// <param name="begNO">begin NO</param>
        /// <param name="endNO">end NO</param>
        /// <param name="editor">editor</param> 
        void ReceiveCOA(string begNO, string endNO, string editor);
        /// <summary>
        /// query
        /// </summary>
        /// <param name="begNO">begin NO</param>
        string QueryEarly(string begNO);
        /// <summary>
        /// GetCoaListOneByOne
        /// </summary>
        /// <param name="coaList">coaList</param>  
        IList<S_RowData_COAStatus> GetListOneByOne(List<string> coaList);
        /// <summary>
        /// Update COAStatus
        /// </summary>
        /// <param name="lotCoaList">lotCoaList</param>
        /// <param name="editor">editor</param>
        /// <param name="status">status</param>
        /// <param name="pdLine">pdLine</param>
        /// <param name="station">station</param> 
        void UpdateCOAList(List<string> lotCoaList,string editor, string status, string pdLine,  string station);
        
    }
}
