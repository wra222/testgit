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
    public interface IMBCTCheck
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product">product</param>
        /// <param name="pdLine">pdLine</param>
        /// <param name="editor">editor</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns></returns>
        IList<string> InputProduct(string product, string pdLine, string editor, string stationId, string customerId);
		

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product">product</param>
        /// <param name="pdLine">pdLine</param>
        /// <param name="editor">editor</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns></returns>
        string Save(string product, string pdLine, string editor, string stationId, string customerId);

        
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="pcbno">pcbno</param>
        void Cancel(string pcbno);
    }
}