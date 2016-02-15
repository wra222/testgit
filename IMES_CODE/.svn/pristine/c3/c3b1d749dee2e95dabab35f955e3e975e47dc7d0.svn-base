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
    /// 将流程卡ProdId和TPM 结合。
    /// 目的：检查是物料是否和BOM相同供后续装配
    /// </summary>
    public interface ICombineTPM
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
                        string editor, string station, string customer);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        void Save(string prodId);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void Cancel(string prodId);


    }
}
