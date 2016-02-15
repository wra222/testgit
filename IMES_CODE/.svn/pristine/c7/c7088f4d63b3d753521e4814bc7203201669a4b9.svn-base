// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-11   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// PAQC Repair
    /// </summary>
    public interface IPAQCRepair
    {
        #region "methods interact with the running workflow"


        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="prodIdOrSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        ProductModel Input(string prodIdOrSN, string line, string editor, string station, string customer);

        /// <summary>
        /// 修改不良信息
        /// </summary>
        /// <param name="ProductID">Product Id</param>
        /// <param name="updateRepairDefect">改变的Repair Defect</param>
        void Edit(string ProductID, RepairInfo updateRepairDefect);

        /// <summary>
        /// 修改不良信息
        /// </summary>
        /// <param name="ProductID">Product Id</param>
        /// <param name="newRepairDefect">新增的Repair Logs</param>
        void Add(string ProductID, RepairInfo newRepairDefect);


        /// <summary>
        /// 完成维修并保存
        /// </summary>
        /// <param name="ProductID">Product Id</param>
        void Save(string ProductID, string returnStation);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="ProductID"></param>
        void cancel(string ProductID);
        #endregion

        #region "methods do not interact with the running workflow"

        ///// <summary>
        ///// 获取全部合法Defect，用于缓存在客户端来判断输入的Defect是否正确。
        ///// </summary>
        ///// <returns></returns>
        //IList<DefectCodeDescr> GetAllDefect();

        #endregion
    }
}
