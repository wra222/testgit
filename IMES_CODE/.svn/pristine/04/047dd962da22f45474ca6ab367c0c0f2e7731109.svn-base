/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for   
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
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
    public interface IFAIOutput
    {
        /// <summary>
        /// ProdId相关判断处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        ArrayList CheckProdId(string custsn, string pdLine, string editor, string stationId, string customerId);
		

        /// <summary>
        /// Check及保存处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="kbct">KB CT</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        ArrayList Save(string custsn, IList<string> defectList, string pdLine, string editor, string stationId, string customerId);
		
		
		/// <summary>
        /// Check及保存处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="kbct">KB CT</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        ArrayList CheckDefect(string custsn, string defect, string pdLine, string editor, string stationId, string customerId);


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}