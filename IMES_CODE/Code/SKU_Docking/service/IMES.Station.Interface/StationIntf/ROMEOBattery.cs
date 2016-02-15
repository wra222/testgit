/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 02
* UI:CI-MES12-SPEC-PAK-UI ROMEO Battery.docx
* UC:CI-MES12-SPEC-PAK-UC ROMEO Battery.docx           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-15   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/

using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 机器抽检，检查抽中的机器是否有不良。如果有不良需要记录不良信息，然后送到维修站
    /// </summary>
    public interface IROMEOBattery
    {
        #region "methods interact with the running workflow"


        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        ArrayList InputSN(string custSN, string line, string editor, string station, string customer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        IMES.DataModel.MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);

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

        #endregion

        #region "methods do not interact with the running workflow"

        /// <summary>
        /// 获取全部合法Defect，用于缓存在客户端来判断输入的Defect是否正确。
        /// </summary>
        /// <returns></returns>
        //IList<DefectCodeDescr> GetAllDefect();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<LightBomInfo> getBomByCode(string code);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<LightBomInfo> getBomByModel(string model, out string code);

        #endregion
    }
}
