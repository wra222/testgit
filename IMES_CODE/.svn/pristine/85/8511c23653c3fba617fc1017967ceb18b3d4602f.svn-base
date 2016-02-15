/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service interface for RepairInfo Page
 *             
 * UI:CI-MES12-SPEC-FA-UI RepairInfo Query.docx
 * UC:CI-MES12-SPEC-FA-UC RepairInfo Query.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-30  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System;
using System.Data;
using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Change Key Parts
    /// </summary>
    public interface IRepairInfo
    {
        /// <summary>
        /// 获取表格内容
        /// </summary>
        /// <param name="snList">type</param>
        /// <param name="rsList">dnLines</param>
        /// <param name="failSNList">pnLines</param>
        DataTable GetRepairInfoList(IList<string> snList, IList<string> rsList, out IList<string> failSNList);

        /// <summary>
        /// 修改指定的维修记录
        /// </summary>
        /// <param name="productID">ProductID</param>
        /// <param name="repInfo">被修改的RepairLogInfo</param>
        void EditRepairInfo(string productID, RepairInfo repInfo);

        /// <summary>
        /// 获取指定的维修记录
        /// </summary>
        /// <param name="id">ProductRepair_DefectInfo表中的ID</param>
        RepairInfo GetDefectInfo(int id);

        void UpdateProductRepair_DefectInfo_Mark(IList<int> mark_0, IList<int> mark_1, string editor);

        //void UpdateProductRepair_DefectInfo_MarkDefered(IList<int> mark_0, IList<int> mark_1, string editor);
    }
}
