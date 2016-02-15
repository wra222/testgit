/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Service for KP Defect Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI KeyParts Defect Input.docx
 * UC:CI-MES12-SPEC-SA-UC KeyParts Defect Input.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-20  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// KP信息获取与保存
    /// </summary>
    public interface IKPDefectInput
    {
        /// <summary>
        /// Check CTNO
        /// </summary>
        void CheckCTNO(string input, string line, string editor, string station, string customer);

        /// <summary>
        /// Input Defect
        /// </summary>
        /// <param name="ctno">CTNO</param>
        /// <param name="defect">Defect Code</param>
        IList<DefectCodeDescr> InputDefect(string ctno, string defect);

        /// <summary>
        /// Clear Defect
        /// </summary>
        /// <param name="ctno">CTNO</param>
        void ClearDefect(string ctno);

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="ctno">CTNO</param>
        void Save(string ctno);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="ctno">CTNO</param>
        void Cancel(string ctno);  
    }
}
