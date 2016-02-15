/*
 * INVENTEC corporation ?012 all rights reserved. 
 * Description:IMES service implement for UnitWeight (for Docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight for Docking
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight for Docking            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-29  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    ///  UC 具体业务：  本站站号：85
    ///                1. Unit 称重；
    ///                2. 列印Unit Weight Label；
    ///                3. 上传数据至SAP
    /// </summary>
    public interface IPakUnitWeight
    {

        #region PakUnitWeight

        /// <summary>
        /// 此站输入的是SN，需要在BLL中先根据SN获取Product调用CommonImpl.GetProductByInput()
        /// 如果获取不到，报CHK079: 找不到与此序号匹配的Product! 
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
        /// <param name="configParams"></param>
        /// <returns>ArrayList对象</returns>
        ArrayList InputUUT(string custSN, decimal actualWeight, string line, string editor, string station, string customer);

        /// <summary>
        /// 保存机器重量，更新机器状态，结束工作流。
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        void Save(string productID);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        void Cancel(string productID);
        #endregion

        /// <summary>
        /// 获取ModelWeight
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns></returns>
        ModelWeightDef GetModelWeightByModelorCustSN(string inputData);

        /// <summary>
        /// 保存修改的ModelWeight
        /// </summary>
        /// <param name="item">item</param>
        /// <returns></returns>
        void SaveModelWeightItem(ModelWeightDef item);

    }
}
