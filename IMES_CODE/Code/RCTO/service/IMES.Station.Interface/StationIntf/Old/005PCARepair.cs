// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 查询并显示MB 当前维修记录
    /// </summary>
    public interface IPCARepair
    {
        /// <summary>
        /// 1.1	UC-PCA-PCAR-01 Query
        /// 查询并显示MB 当前维修记录
        /// 查询成功后调用getMBInfo和getRepairLogList来取得查询结果
        /// </summary>
        /// <param name="MB_SNo"></param>
        /// <param name="pdLine"></param>
        /// <param name="editor">operator</param>
        void Query(
            string MB_SNo,
            string pdLine,
            string editor, string stationId, string customer);

        /// <summary>
        /// 1.2	UC-PCA-PCAR-02 Edit
        /// 修改指定的MB 维修记录
        /// </summary>
        /// <param name="log">被修改的RepairLogInfo</param>
        /// <param name="editor">operator</param>
        /// <returns>已维修的次数</returns>
        int Edit(
            string MB_SNo,
            RepairInfo log,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 1.3	UC-PCA-PCAR-03 Add
        /// 添加 MB 维修记录
        /// </summary>
        /// <param name="log">待添加的RepairLogInfo</param>
        /// <param name="editor">operator</param>
        /// <returns>已维修的次数</returns>
        int Add(
            string MB_SNo,
            RepairInfo log,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 1.4	UC-PCA-PCAR-04 Delete
        /// 删除指定的MB 维修记录
        /// </summary>
        /// <param name="repairLogId">待删除的RepairLogInfo</param>
        /// <param name="editor">operator</param>
        void Delete(
            string MB_SNo,
            string repairLogId,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 1.5	UC-PCA-PCAR-05 Save
        /// 已经完成对MB 的维修
        /// </summary>
        void Save(string MB_SNo);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string MB_SNo);
    }
}
