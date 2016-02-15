/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/21 
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/21           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-15   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// 亮灯提示本站粘贴的Label；
    /// 非BT 在此分配Delivery / Pallet / Box Id / UCC
    /// 列印Label；

    /// </summary>
    public interface ICombinePoInCarton
    {
        #region "methods interact with the running workflow"


        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，获取ProductModel
        /// </summary>
        /// <param name="inputSN"></param>
        /// <param name="deliveryNo"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputSN(string inputSN, string deliveryNo, string model, string firstProID, string line, string editor, string station, string customer);
        ArrayList InputSN(string inputSN, string deliveryNo, string model, string firstProID, string line, string editor, string station, string customer, string isBT);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList Save(string firstProID, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void cancel(string deliverNo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputID"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList ReprintCartonLabel(string inputSN, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems);
        #endregion

        #region "methods do not interact with the running workflow"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetSysSetting(string name);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameList"></param>
        /// <returns></returns>
        ArrayList GetSysSettingList(IList<string> nameList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defValue"></param>
        /// <param name="hostname"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        string GetSysSettingSafe(string name, string defValue, string hostname, string editor);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="hostname"></param>
        /// <param name="editor"></param>
        void SetSysSetting(string name, string value, string hostname, string editor);
        #endregion
    }
}
