/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Interface for Combine AST Page
* UI:CI-MES12-SPEC-FA-UI Combine AST .docx –2011/12/5 
* UC:CI-MES12-SPEC-FA-UC Combine AST .docx –2011/12/5            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-2   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface ICombineAST
    {

        /// <summary>
        /// 卡站处理
        /// </summary>
        /// <param name="prodid">prodid</param>
        /// <param name="length">AST length</param>
        /// <param name="pdline">product line</param>
        /// <param name="status">status</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customerId</param>
        /// <returns>AST值、errorFlag、imageUrl</returns>
        //string BlockCheck(string prodid, int length, string pdline, string status, string stationId, string editor, string customerId);
        List<string> BlockCheck(string prodid, int length, string pdline, string status, string stationId, string editor, string customerId);
        List<string> BlockCheck(string prodid, int length, string pdline, string status, string stationId, string editor, string customerId, bool isCombineAndGenerateAST);
        
        /*
        /// <summary>
        /// 删除已有标签及Check CDSI绑定等处理
        /// </summary>
        /// <param name="prodid">prodid</param>
        /// <param name="deleteflag">delete flag</param>
        /// <param name="stationId">station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customerId</param>
        /// <returns>CustomSN、ProductID、Model值</returns>
        List<string> DoPrint(string prodid, bool deleteflag, string stationId, string editor, string customerId);
        */
        /* 2012-5-2
        /// <summary>
        /// 删除已有标签及Check CDSI绑定等处理
        /// </summary>
        /// <param name="prodid">prodid</param>
        /// <param name="deleteflag">delete flag</param>
        /// <param name="stationId">station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customerId</param>
        /// <param name="printItems">Print Item鍒楄〃</param>
        /// <returns>CustomSN、ProductID、Model值、CDSI标志、Print Item鍒楄〃</returns>
        IList<PrintItem> DoPrint(string prodid, bool deleteflag, string stationId, string editor, string customerId, IList<PrintItem> printItems, out string prodId, out string custsn, out string model, out string CDSIFlag);
        */
        /// <summary>
        /// 删除已有标签及Check CDSI绑定等处理
        /// </summary>
        /// <param name="prodid">prodid</param>
        /// <param name="deleteflag">delete flag</param>
        /// <param name="stationId">station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customerId</param>
        /// <param name="printItems">Print Item鍒楄〃</param>
        /// <returns>CustomSN、ProductID、Model值、CDSI标志、Print Item鍒楄〃、AST</returns>
        IList<PrintItem> DoPrint(string prodid, bool deleteflag, string stationId, string editor, string customerId, IList<PrintItem> printItems, out string prodId, out string custsn, out string model, out string CDSIFlag, out string ast);

        /// <summary>
        /// 对AST进行保存处理
        /// </summary>
        /// <param name="prodid">prodid</param>
        /// <param name="ast">AST</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customerId</param>
        ArrayList DoSave(string prodid, string ast, string stationId, string editor, string customerId);
        void DoSaveAfterCheckATSN9(string prodid, string stationId, string editor, string customerId);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}