/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:OfflinePizzaKitting
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-20   Yang.Weihua               Create   
* Known issues:
* TODO：
* 
*/
using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;


namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    ///
    /// </summary>
    public interface IOfflinePizzaKitting
    {
        #region "methods interact with the running workflow"

     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        ArrayList GetBOM(string model, string key, string lastKey, string line, string editor, string station, string customer);

        /// <summary>
        /// </summary>
        /// <param name="prodId"></param>
        ArrayList Save(IList<PrintItem> printItems, string key, string line);
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void Cancel(string key);
      
        /// <summary>
        /// RePrint
        /// </summary>
        ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);

        #endregion

        #region "methods do not interact with the running workflow"
        #endregion
    }
}
