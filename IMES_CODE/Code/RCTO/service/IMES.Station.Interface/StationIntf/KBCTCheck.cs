/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for KB CT Check Page
 * UI:CI-MES12-SPEC-FA-UI KB CT Check.docx –2012/6/12 
 * UC:CI-MES12-SPEC-FA-UC KB CT Check.docx –2012/6/12            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-6-12   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IKBCTCheck
    {
        /// <summary>
        /// ProdId相关判断处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        void CheckProdId(string prodid, string pdLine, string editor, string stationId, string customerId);
		void CheckProdId(string prodid, string pdLine, string editor, string stationId, string customerId, string CheckModel);
		

        /// <summary>
        /// Check及保存处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="kbct">KB CT</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        void CheckAndSave(string prodid, string kbct, string pdLine, string editor, string stationId, string customerId);

        
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}