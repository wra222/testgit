using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// FA Repair
    /// </summary>
    public interface IFARepair
    {
        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>Repair Logs</returns>
        IList<RepairInfo> InputProdId(
            string pdLine,
            string prodId,
            string editor, string stationId, string customer);

        /// <summary>
        /// Edit Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">改变的Repair log</param>
        void Edit(
            string prodId,
            RepairInfo rll);

        /// <summary>
        /// Add Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">新增的Repair Log</param>
        void Add(
            string prodId,
            RepairInfo rll);

        /// <summary>
        /// Delete Repair logs
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="rll">删除的Repair log</param>
        void Delete(
            string prodId,
            RepairInfo rll);

        /// <summary>
        /// 完成维修并保存
        /// </summary>
        /// <param name="prodId">Product Id</param>
        void Save(
            string prodId,
            string testStation);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string prodId);
    }
}
