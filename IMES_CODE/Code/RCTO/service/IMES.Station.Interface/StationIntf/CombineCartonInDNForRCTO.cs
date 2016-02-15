/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI-Combine Carton in DN
* UC:CI-MES12-SPEC-PAK-UC-Combine Carton in DN                
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/09/07    ITC000052             Create  
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

    /// </summary>
    public interface ICombineCartonInDNForRCTO
    {
        #region "methods interact with the running workflow"


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSN"></param>
        /// <param name="model"></param>
        /// <param name="firstProID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputSN(string inputCartonNo, string line, string editor, string station, string customer);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        ArrayList getDeliveryInfo(string deliveryNo);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList Save(string firstProID, string deliveryNo, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void cancel(string deliverNo);
 
        #endregion
    }
}
