/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for SMT Defect Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI SMT Defect Input.docx –2012/05/21
 * UC:CI-MES12-SPEC-SA-UC SMT Defect Input.docx –2012/05/21            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-21  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// MB信息获取与保存
    /// </summary>
    public interface ISMTDefectInput
    {
        /// <summary>
        /// Check MBSno
        /// </summary>
        /// <param name="input">MBSno</param>
        /// <param name="line">Product Line</param>
        /// <param name="editor">Operator</param>
        /// <param name="station">Station</param>
        /// <param name="customer">Customer</param>
        void CheckMBSno(string input, string line, string editor, string station, string customer);

        /// <summary>
        /// Input Defect
        /// </summary>
        /// <param name="mb">MBSno</param>
        /// <param name="defect">Defect Code</param>
        IList<DefectCodeDescr> InputDefect(string mb, string defect);

        /// <summary>
        /// Clear Defect
        /// </summary>
        /// <param name="mb">MBSno</param>
        void ClearDefect(string mb);

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="mb">MBSno</param>
        void Save(string mb);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="mb">MBSno</param>
        void Cancel(string mb);  
    }
}
