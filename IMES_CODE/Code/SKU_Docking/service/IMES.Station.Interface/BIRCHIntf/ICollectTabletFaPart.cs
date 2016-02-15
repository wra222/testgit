/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:UI/WebMethod/Sevice Interface/Implementation for ConmbineTPM Page
* UI:CI-MES12-SPEC-FA-UI ConmbineTPM.docx –2011/10/11 
* UC:CI-MES12-SPEC-FA-UC ConmbineTPM.docx –2011/10/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   yang jie               (Reference Ebook SourceCode) Create
* Known issues:
=*/
using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
   /// </summary>
    public interface ICollectTabletFaPart
    {
        /// <summary>
        /// 输入ProdId和相关信息
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="curStation"></param>
        /// <returns>ProductModel</returns>
        ArrayList InputSN(string custSN, string line, string curStation,
                        string editor, string station, string customer,bool allnullbomitem);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);

        /// <summary>
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        ArrayList Save(string prodId, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void Cancel(string prodId);

        /// <summary>
        /// Reprint POD
        /// </summary>
        ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);
        ArrayList RePrintPOD(string sn, string reason, string editor, string station, string customer);
        
    }
}
