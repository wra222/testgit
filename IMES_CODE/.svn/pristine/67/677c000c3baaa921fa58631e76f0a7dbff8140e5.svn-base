/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Service for KP Repair Page
 *             
 * UI:CI-MES12-SPEC-FA-UI KeyParts Repair.docx
 * UC:CI-MES12-SPEC-FA-UC KeyParts Repair.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-26  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 查询并显示MB 当前维修记录
    /// </summary>
    public interface IKPRepair
    {
        /// <summary>
        /// 输入CTNO，创建workflow
        /// </summary>
        /// <param name="ctno">KP SN</param>
        /// <param name="line"></param>
        /// <param name="editor">operator</param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void InputCTNo(
            string ctno,
            string line,
            string editor, 
            string station, 
            string customer);

        /// <summary>
        /// 取得界面显示数据
        /// </summary>
        /// <param name="ctno">KP SN</param>
        IList<RepairInfo> GetKPRepairList(string ctno);

        /// <summary>
        /// 修改指定的KP 维修记录
        /// </summary>
        /// <param name="ctno">KP SN</param>
        /// <param name="repInfo">被修改的RepairLogInfo</param>
        /// <returns>已维修的次数</returns>
        int Edit(string ctno, RepairInfo repInfo);

        /// <summary>
        /// 添加 KP 维修记录
        /// </summary>
        /// <param name="ctno">KP SN</param>
        /// <param name="repInfo">待添加的RepairLogInfo</param>
        /// <returns>已维修的次数</returns>
        int Add(string ctno, RepairInfo repInfo);

        /// <summary>
        /// 已经完成对KP 的维修
        /// </summary>
        void Save(string ctno);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string ctno);
    }
}
