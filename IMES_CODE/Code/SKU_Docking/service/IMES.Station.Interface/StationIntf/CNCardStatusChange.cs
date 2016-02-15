/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for CNCardStatusChange Page            
 * CI-MES12-SPEC-PAK CN Card Status Change.docx
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
    /// Row data define of CNCardStatusChange page
    /// </summary>
    public struct S_RowData_CNCardStatus
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
    };

    /// <summary>
    /// CN Card Status Change
    /// </summary>
    public interface ICNCardStatusChange
    {
        /// <summary>
        /// 获取CSNMas表相关信息
        /// </summary>
        /// <param name="begNO">begin NO</param>
        /// <param name="endNO">end NO</param>
        IList<S_RowData_CNCardStatus> GetCSNList(string begNO, string endNO);
        /// <summary>
        /// Update CSNMas
        /// </summary>
        /// <param name="begNO">begin NO</param>
        /// <param name="endNO">end NO</param>
        /// <param name="editor">editor</param>
        /// <param name="udt">udt</param>
        /// <param name="status">status</param>
        /// <param name="pdLine">pdLine</param>
        void UpdateCSN(string begNO, string endNO, string editor, string udt, string status, string pdLine);
    }
}
