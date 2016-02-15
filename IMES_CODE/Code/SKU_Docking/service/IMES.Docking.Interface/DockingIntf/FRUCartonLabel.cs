/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Service for FRU Carton Label Print (for Docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI FRU Carton Label for Docking
 * UC:CI-MES12-SPEC-PAK-UC FRU Carton Label for Docking      
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-25  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/

using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// Change Key Parts
    /// </summary>
    public interface IFRUCartonLabel
    {
        /// <summary>
        /// 检查Model是否存在
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="model">Model</param>
        bool CheckModelExist(string customer, string model);

        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="printItems">printItems</param>
        IList<PrintItem> GetPrintTemplate(string customer, IList<PrintItem> printItems);
    }
}
