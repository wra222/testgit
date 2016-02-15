/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PCA Repaire Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI PCA Repair Input.docx –2011/12/13 
 * UC:CI-MES12-SPEC-SA-UC PCA Repair Input.docx –2011/12/08            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-12  itc202017             (Reference Ebook SourceCode) Create
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
    public interface IPCARepairInput
    {
        /// <summary>
        /// 输入MBSno，卡站
        /// </summary>
        /// <param name="input">MBSno</param>
        /// <param name="line">Product Line</param>
        /// <param name="editor">Operator</param>
        /// <param name="station">Station</param>
        /// <param name="customer">Customer</param>
        void InputMBSno(string input, string line, string editor, string station, string customer);

        /// <summary>
        /// 检查MB是否在修护区
        /// </summary>
        /// <param name="mb">MBSno</param>
        bool IsMBInRepair(string mb);

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
