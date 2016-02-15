// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Pizza称重站接口
    /// </summary>
    public interface IPizzaWeight
    {
        /// <summary>
        /// 启动工作流，根据输入productID获取Model和标准重量,成功后调用NeedCheckSN
        /// 如果需要检查的SN，调用CheckSN，否则调用Save
        /// 将actualWeight放到Session.ActuralWeight中，用于检查是否和标准重量相符
        /// 将actualWeight放到Session.CartonWeight中，用于保存CartonWeight重量用
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="actualWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        StandardWeight InputUUT(string productID, decimal actualWeight, string line, string editor, string station, string customer, out bool needCheckSN);

        ///// <summary>
        ///// 根据Model的Region判断是否是JPN
        ///// </summary>
        ///// <param name="productID"></param>
        ///// <returns></returns>
        //bool NeedCheckSN(string productID);

        /// <summary>
        /// 检查CustSn,成功后调用save
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="sn"></param>
        void CheckSN(string productID, string sn);

        /// <summary>
        /// 保存机器Pizza重量，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将printItems放到Session.PrintItems中
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        void Save(string productID);


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        void Cancel(string productID);

      


    }
}
