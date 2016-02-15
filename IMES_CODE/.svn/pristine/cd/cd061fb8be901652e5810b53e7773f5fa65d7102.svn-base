/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Service for LCM Repair Page
 *             
 * UI:CI-MES12-SPEC-FA-UI RCTO LCM Repair.docx
 * UC:CI-MES12-SPEC-FA-UC RCTO LCM Repair.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-16  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 查询并显示MB 当前维修记录
    /// </summary>
    public interface ILCMRepair
    {
        /// <summary>
        /// 输入CTNO，创建workflow
        /// </summary>
        /// <param name="ctno">CT NO</param>
        /// <param name="editor">operator</param>
        /// <param name="station">Station</param>
        /// <param name="customer">Customer</param>
        /// <param name="id">ProductID(out)</param>
        /// <param name="model">ProductModel(out)</param>
        /// <param name="tStation">TestStation(out)</param>
        /// <param name="line">Line(out)</param>
        void InputCTNo(
            string ctno,
            string editor, 
            string station, 
            string customer,
            out string id,
            out string model,
            out string tStation,
            out string line);

        /// <summary>
        /// 取得界面显示数据
        /// </summary>
        /// <param name="id">ProductID</param>
        IList<RepairInfo> GetLCMRepairList(string id);

        /// <summary>
        /// 修改指定的LCM 维修记录
        /// </summary>
        /// <param name="id">ProductID</param>
        /// <param name="repInfo">被修改的RepairLogInfo</param>
        /// <returns>已维修的次数</returns>
        int Edit(string id, RepairInfo repInfo);

        /// <summary>
        /// 添加 LCM 维修记录
        /// </summary>
        /// <param name="id">ProductID</param>
        /// <param name="repInfo">待添加的RepairLogInfo</param>
        /// <returns>已维修的次数</returns>
        int Add(string id, RepairInfo repInfo);

        /// <summary>
        /// 已经完成对LCM 的维修
        /// </summary>
        void Save(string id);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string id);
    }
}
