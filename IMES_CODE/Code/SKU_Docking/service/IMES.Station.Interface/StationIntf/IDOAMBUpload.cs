/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service interface for DOA MB Upload Page
 *             
 * UI:CI-MES12-SPEC-FA-UI DOA MB List Upload.docx
 * UC:CI-MES12-SPEC-FA-UC DOA MB List Upload.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-11-20  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Data;
using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    [Serializable]
    public struct S_RowData_FailMBSN
    {
        /// <summary>
        /// MBSN.
        /// </summary>
        public string m_mbsn;
        /// <summary>
        /// Cause type.
        /// </summary>
        public int m_cause;
    };

    /// <summary>
    /// DOA MB Upload
    /// </summary>
    public interface IDOAMBUpload
    {
        /// <summary>
        /// Save DOA MBSN List.
        /// </summary>
        /// <param name="mbList">MBSN List</param>
        /// <param name="passList">Pass MBSN List</param>
        /// <param name="failList">Fail MBSN List</param>
        void SaveDOAMBList(IList<string> mbList, string editor, out IList<string> passList, out IList<S_RowData_FailMBSN> failList);

        /// <summary>
        /// Fail cause list.
        /// </summary>
        IList<string> GetFailCauseList();
    }
}
