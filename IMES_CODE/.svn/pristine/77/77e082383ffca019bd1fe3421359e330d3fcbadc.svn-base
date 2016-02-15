/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for RemoveKPCT
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-6-12   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IRemoveKPCT
    {
        /// <summary>
        /// CheckProduct
        /// </summary>
        /// <param name="prodId">prodId</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customerId</param>
        void CheckProduct(string prodId, string pdLine, string editor, string stationId, string customerId);
        
        /// <summary>
        /// GetParts
        /// </summary>
        /// <param name="prodId">prodId</param>
		/// <param name="editor">editor</param>
		/// <param name="customerId">customerId</param>
        DataTable GetParts(string prodId, string mbct2, string pdLine, string editor, string stationId, string customerId);
		

        /// <summary>
        /// RemoveParts
        /// </summary>
        /// <param name="prodId">prodId</param>
		/// <param name="editor">editor</param>
		/// <param name="customerId">customerId</param>
        void RemoveParts(string prodId, string pdLine, string editor, string stationId, string customerId);

        
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="pcbno"></param>
        void Cancel(string pcbno);
    }
}