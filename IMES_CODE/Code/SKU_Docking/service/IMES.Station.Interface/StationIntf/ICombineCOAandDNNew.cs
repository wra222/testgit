/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine COA and DN
* CI-MES12-SPEC-PAK-UI Combine COA and DN.docx
* CI-MES12-SPEC-PAK-UC Combine COA and DN.docx             
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/08/10   Du.Xuan               Create   
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
    public interface ICombineCOAandDNNew
    {
        #region "methods interact with the running workflow"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputSN(string custSN, string line, string editor, string station, string customer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="inputCOA"></param>
        /// <returns></returns>
        ArrayList checkCOA(string prodId, string inputCOA);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList save(string prodId, IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void cancel(string prodId);

        #endregion

        #region "methods do not interact with the running workflow"
        

        #endregion
    }
}
