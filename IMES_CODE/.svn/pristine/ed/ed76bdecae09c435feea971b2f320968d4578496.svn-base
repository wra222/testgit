/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 02
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 2.docx –2011/11/15 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 2.docx –2011/11/15            
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

namespace IMES.Station.Interface.StationIntf
{
    [Serializable]
    public struct S_TpPartNo
    {
        public string Tp;
        public string PartNo;
    }
    /// <summary>
    /// 亮灯提示本站粘贴的Label；
    /// 非BT 在此分配Delivery / Pallet / Box Id / UCC
    /// 列印Label；

    /// </summary>
    public interface IPDPALabel02
    {
        #region "methods interact with the running workflow"

        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，获取ProductModel
        /// </summary>
        /// <param name="uutSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ArrayList</returns>
        ArrayList InputSNforCQ(Boolean queryflag, string custSN, string line, string code, string floor, string editor, string station, string customer);


        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，获取ProductModel
        /// </summary>
        /// <param name="uutSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ArrayList</returns>
        ArrayList InputSN(Boolean queryflag, string custSN, string line, string code, string floor, string editor, string station, string customer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="WWANID"></param>
        /// <returns></returns>
        ArrayList InputWWANID(string prodId, string WWANID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="WWANSN"></param>
        /// <returns></returns>
        ArrayList checkCOA(string prodId, string WWANSN);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        ArrayList checkCOAQuery(string prodId);

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

        #endregion
    }
}
