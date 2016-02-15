﻿/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Interface for PAQC Output For RCTO Page
* UI:CI-MES12-SPEC-PAK-UI PAQC Output_RCTO.docx –2012/6/11 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output_RCTO.docx –2012/7/10            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-07-25   Jessica Liu           Create
* Known issues:
* TODO：
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
    /// 机器抽检，检查抽中的机器是否有不良。如果有不良需要记录不良信息，然后送到维修站
    /// </summary>
    public interface IPAQCOutputForRCTO
    {
        #region "methods interact with the running workflow"


        /// <summary>
        /// 刷uutSn，启动工作流，检查输入的uutSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="uutSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputSN(string uutSn, string line, string editor, string station, string customer);


        /// <summary>
        /// check MBSno
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="prodId"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        //2012-7-20
        void checkMBSno(string mbSno, string prodId, string line, string editor, string station, string customer);


        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        void save(string prodId,IList<string> defectCodeList);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        void cancel(string prodId);
        #endregion

        #region "methods do not interact with the running workflow"

        /// <summary>
        /// 获取全部合法Defect，用于缓存在客户端来判断输入的Defect是否正确。
        /// </summary>
        /// <returns></returns>
        //IList<DefectCodeDescr> GetAllDefect();

        #endregion
    }
}
