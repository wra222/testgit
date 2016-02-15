// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-29   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// Pallet Weight站接口定义
    /// </summary>
    public interface IPalletWeight
    {
        #region "methods interact with the running workflow"


        /// <summary>
        /// 刷pallletNo，启动工作流，检查输入的pallletNo，卡站
        /// 将pallletNo放到Session.PalletNo中
        /// </summary>
        /// <param name="inputID"></param>
        /// <param name="acturalWeight"></param>
        /// <param name="type"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputPallet(string inputID, decimal acturalWeight, string type,
                string line, string editor, string station, string customer);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="palletNo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        String InputCustSN(string custSN, string palletNo);

        /// <summary>
        /// 将ActuralWeight添加到Session.ActuralWeight中
        /// 将custSn放到Session.CustSN中
        /// 结束工作流
        /// </summary>
        /// <param name="pallletNo"></param>
        /// <param name="custSn"></param>
        /// <param name="acturalWeight"></param>
        /// <param name="currentStandardWeight"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> Save(string pallletNo, string custSn, decimal acturalWeight, IList<PrintItem> printItems);
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="pallletNo"></param>
        void Cancel(string pallletNo);
        #endregion

        #region "methods do not interact with the running workflow"
        /// <summary>
        /// 获取当天已经完成称重的Pallet数量
        /// </summary>
        /// <returns></returns>
        int getQtyOfPalletToday();

       /// <summary>
        /// 重印标签
       /// </summary>
       /// <param name="custSN"></param>
       /// <param name="reason"></param>
       /// <param name="line"></param>
       /// <param name="editor"></param>
       /// <param name="station"></param>
       /// <param name="customer"></param>
       /// <param name="printItems"></param>
       /// <returns></returns>
        ArrayList ReprintPalletWeightLabel(string custSN, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems);

        IList<COMSettingDef> GetWeightSettingInfo(string hostname);
        #endregion
    }
}
