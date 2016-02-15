/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:COA Return    
 * CI-MES12-SPEC-PAK-COA Return.docx 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-1-10   207003                Create  
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
    /// Row data define of COAReturn page
    /// </summary>
    public struct S_RowData_COAReturn
    {
        /// <summary>
        /// SN 
        /// </summary>
        public string SN;
        /// <summary>
        /// COA
        /// </summary>
        public string COAorError;
        /// <summary>
        /// OOA
        /// </summary>
        public string OOA; 
    };
    [Serializable]
    /// <summary>
    /// Row data define of COAReturn page
    /// </summary>
    public struct S_COAReturn
    {
        /// <summary>
        /// validProduct 
        /// </summary>
        public List<S_RowData_COAReturn> validProduct;
        /// <summary>
        /// validProduct 
        /// </summary>
        public List<S_RowData_COAReturn> inValidProduct;
        /// <summary>
        /// reValue
        /// </summary>
        public string reValue;
    };
    
    /// <summary>
    /// COA Return
    /// </summary>
    public interface ICOAReturn
    {

        /// <summary>
        /// Get Product Table
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="key">key</param>
        /// <param name="SN">SN</param> 
        /// <param name="complete">complete</param>  
        S_COAReturn GetProductTable(string line, string editor, string station, string customer, string key, List<string> SN, bool complete);
         /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="key"></param>
        void Cancel(string key);
        /// <summary>
        /// 接着做
        /// </summary>
        /// <param name="key"></param>
        void MakeSureSave(string key);
    }

}
