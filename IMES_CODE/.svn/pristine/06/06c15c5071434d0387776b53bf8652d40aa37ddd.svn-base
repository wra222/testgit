/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-01-10   Yang.Wei-Hua     Create 
 * 
 * Known issues:Any restrictions about this file 
 */
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 查询并显示MB 当前维修记录
    /// </summary>
    public interface IPCARepair
    {
        /// <summary>
        /// 成功后调用IMES.Station.Interface.CommonIntf.IMB.getMBInfo
        /// 和IMES.Station.Interface.CommonIntf.IRepair.GetMBRepairList来取得界面显示数据
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="line"></param>
        /// <param name="editor">operator</param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        string InputMBSn(
            string mbSno,
            string line,
            string editor, 
            string station, 
            string customer,
            out string secureOn);

        /// <summary>
        /// Edit
        /// 修改指定的MB 维修记录
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="repInfo">被修改的RepairLogInfo</param>
        /// <returns>已维修的次数</returns>
        int Edit(string mbSno, RepairInfo repInfo);

        /// <summary>
        /// 添加 MB 维修记录
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="repInfo">待添加的RepairLogInfo</param>
        /// <returns>已维修的次数</returns>
        int Add(string mbSno, RepairInfo repInfo);

        /// <summary>
        /// Save
        /// 已经完成对MB 的维修
        /// </summary>
        void Save(string mbSno);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string mbSno);
    }
}
