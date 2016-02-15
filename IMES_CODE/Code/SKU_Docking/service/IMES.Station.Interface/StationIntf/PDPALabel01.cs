/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 01
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx –2011/10/28 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx –2011/10/28            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
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

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 亮灯提示本站粘贴的Label；
    /// 列印Label；(不需要Reprint 功能)
    /// </summary>
    public interface IPDPALabel01
    {
        #region "methods interact with the running workflow"

        /// <summary>
        /// 刷custSn，启动工作流，检查输入的custSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="queryflag"></param>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="code"></param>
        /// <param name="floor"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputSN(Boolean queryflag, string custSN, string line, string code, string floor, string editor, string station, string customer);
        
        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        void save(string prodId,IList<string> defectCodeList);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void cancel(string prodId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="line"></param>
        /// <param name="code"></param>
        /// <param name="floor"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> Print(string productID, string line, string code, string floor,
                               string editor, string station, string customer, IList<PrintItem> printItems);


        ArrayList ReprintLabel(string customerSN, string reason, IList<PrintItem> printItems, string line, string editor, string station, string customer);
        
        #endregion

    }
}
