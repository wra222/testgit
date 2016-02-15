/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Interface for Online Generate AST Page
* UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2011/11/21 
* UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2011/11/21            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-21   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    public interface IOnlineGenerateAST
    {
        /// <summary>
        /// 打印Asset Tag Label
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        /// <param name="ast">AST</param>
        IList<PrintItem> Print(string custsn, string pdLine, string stationId, string editor, string customerId, IList<PrintItem> printItems, out string ast, out string astCodeList,out string errMsg);

        /// <summary>
        /// 重新打印Asset Tag Label
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="reason">reason</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        /// <param name="prodid">Product ID、ErrorFlag、imageURL</param>
        IList<PrintItem> RePrint(string custsn, string pdLine, string stationId, string editor, string customerId, string reason, IList<PrintItem> printItems, out string prodid, out string ErrorFlag, out string imageURL, out string astCodeList);//ast);


        /// <summary>
        /// 检查Customer SN合法性并获得对应Product ID
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <returns>CustomSN和ProductID值</returns>
        List<string> CheckCustomerSN(string custsn, string pdLine, string stationId, string editor, string customerId);
        List<string> CheckCustomerSN(string custsn, string pdLine, string stationId, string editor, string customerId, bool isCombineAndGenerateAST);
        ArrayList CheckCustomerSN_New(string custsn, string pdLine, string stationId, string editor, string customerId, bool isCombineAndGenerateAST);
        MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        /// <summary>
        /// CNRS Job Request Ast Number by AST HP PO
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        string GenAstNumberByAstHPPo(string productID,string hpPo,string station,string line, string editor, string customer);
       
    }
}