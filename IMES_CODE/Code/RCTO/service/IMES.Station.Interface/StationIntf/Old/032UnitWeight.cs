// INVENTEC corporation (c)2009 all rights reserved. 
// Description:  UnitWeight Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// UnitWeight Interface,单台机器称重
    /// </summary>
    public interface IUnitWeight
    {

        #region UnitWeight

        /// <summary>
        /// 此站输入的是SN，需要在BLL中先根据SN获取Product调用CommonImpl.GetProductByInput()
        /// 如果获取不到，报CHK079！
        /// 用ProductID启动工作流
        /// 将获得的Product放到Session.Product中
        /// 将actualWeight放到Session.ActuralWeight中
        /// 获取Model和标准重量和ProductID
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="actualWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="productID"></param>
        /// <returns>StandardWeight对象</returns>
        StandardWeight InputUUT(string custSN, decimal actualWeight, string line, string editor, string station, string customer, out string productID);

        /// <summary>
        /// 保存机器重量，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 将printItems放到Session.PrintItems中
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        void  Save(string productID);
        //IList<PrintItem> Save(string productID,  IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        void Cancel(string productID);
        #endregion


        #region Reprint

        /// <summary>
        /// 重印Unit Weight Label
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<PrintItem> ReprintLabel(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItemList,out string productID,out string model);

        #endregion


        #region Modify StandardWeight

        /// <summary>
        /// 输入Model，获取标准重量ModelWeight的UnitWeight栏位
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        decimal GetStandardWeight(string Model);

        /// <summary>
        /// 启动工作流，修改Unit称重的标准重量，对应与ModelWeight表中的UnitWeight栏位
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="NewStandardWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void ModifyStandardWeight(string Model, decimal NewStandardWeight, string line, string editor, string station, string customer);

        #endregion


    }
}
