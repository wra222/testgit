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
using System.Collections;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Carton称重站接口
    /// </summary>
    public interface ICartonWeight
    {
        /// <summary>
        /// 因为Carton需要根据其中任意一个SN进行SFC，
        /// 所以在本方法中需要先根据Carton号码获取一个Product对像放到Session中
        /// CommonImpl.GetProductByInput(cartonNumber, CommonImpl.InputTypeEnum.Carton)
        /// 将cartonNumber放到Session.Carton
        /// 将CartonWeight放到Session.CartonWeight，用于保存CartonWeight重量用
        /// ProductRepository.GetProductIDListByCarton(string cartonSN)可以获取属于该Carton的所有的ProductID
        /// 根据CartonWeight/Carton中的机器数量获得平均每台机器的ActuralWeight，并放到Session.ActuralWeight，用于检查是否和标准重量相符
        /// 启动工作流，根据输入cartonNumber获取Model和标准重量
        /// </summary>
        /// <param name="cartonNumber"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>StandardWeight对象</returns>
        StandardWeight InputCarton(string cartonNumber, decimal cartonWeight, string line, string editor, string station, string customer, out string productID, out string cartonSN);

        /// <summary>
        /// 保存Carton重量，更新机器状态，返回打印重量标签的PrintItem，结束工作流。
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param>
        /// <param name="cartonSn"></param>
        /// <returns></returns>
        IList<PrintItem> save(string productID, IList<PrintItem> printItems, out string cartonSn);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        void cancel(string productID);

        /// <summary>
        /// 重印Carton Weight Label
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        IList<PrintItem> ReprintLabel(string custSNorCartonSn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems, ref string cartonSn);


        /// <summary>
        /// 启动工作流，修改Carton称重的标准重量，对应与ModelWeight表中的CartonWeight栏位
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="NewStandardWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void ModifyStandardWeight(string Model, decimal NewStandardWeight, string line, string editor, string station, string customer);

                /// <summary>
        /// 输入Model，获取标准重量ModelWeight的CartonWeight栏位
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        decimal GetStandardWeight(string Model);
    }
}
