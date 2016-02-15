/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: PCA OQC Output
 * UI:CI-MES12-SPEC-SA-UI PCA OQC Output.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA OQC Output.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	记录SAOQC 结果，若有不良，则记录不良信息
 *               
 * UC Revision:  3382
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// PCA OQC Output
    /// 连板切割入口，实现连板的切割,并打印子板标签
    /// </summary> 
    /// 
    public interface IPCAOQCOutput
    {
        #region methods interact with the running workflow

        /// <summary>
        /// 刷mbsno，调用该方法启动工作流，根据输入mbsno获取model
        /// 返回model
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="curMBInfo">curMBInfo</param>
        /// <returns>model</returns>
        string inputMBSno(string mbsno, string editor, string station, string customer, out MBInfo curMBInfo);


        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="defectCodeList">defectCodeList</param>
        void save(string mbsno, IList<string> defectCodeList);

        #endregion


        #region methods do not interact with the running workflow


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn"></param>
        void Cancel(string mbsno);


        #endregion

    }
}
