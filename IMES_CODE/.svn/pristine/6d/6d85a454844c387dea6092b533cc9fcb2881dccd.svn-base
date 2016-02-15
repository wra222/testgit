/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Pizz
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-20   Yang.Weihua               Create   
* Known issues:
* TODO：
* 
*/

using System.Collections;
using IMES.DataModel;


namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 机器抽检，检查抽中的机器是否有不良。如果有不良需要记录不良信息，然后送到维修站
    /// </summary>
    public interface ICombinePizza
    {
        #region "methods interact with the running workflow"

        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，卡站，获取ProductModel
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

        /// <summary>
        /// 检查QCStatus
        /// </summary>
        /// <param name="prodId"></param>
        string CheckQCStatus(string prodId);

        /// <summary>
        /// CheckWarrantyCard
        /// </summary>
        /// <param name="prodId"></param>
        string CheckWarrantyCard(string prodId);
        
        #endregion

        #region "methods do not interact with the running workflow"
        #endregion
    }
}
