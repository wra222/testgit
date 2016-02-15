// INVENTEC corporation ©2011 all rights reserved. 
// Description:PAQC Output 
// UI:CI-MES12-SPEC-SA-UC BGA Output.docx –2012/1/04 
// UC:CI-MES12-SPEC-SA-UC BGA Output.docx –2012/1/04                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-1-04   Du.Xuan                      Create 
// Known issues:
// TODO：

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 更换主板器件后，记录更换信息，主板刷出窗口
    /// </summary>
    public interface BGAOutput
    {
        #region "methods interact with the running workflow"


       /// <summary>
        /// 刷MBSno，启动工作流，检查输入的Sno，卡站，获取RepairList
       /// </summary>
       /// <param name="sno"></param>
       /// <param name="line"></param>
       /// <param name="editor"></param>
       /// <param name="station"></param>
       /// <param name="customer"></param>
       /// <returns></returns>
        ArrayList InputSno(string sno, string line, string editor, string station, string customer);

        /// <summary>
        /// 检查rpt_PCARep是否存在同样的维修记录 ,Save Repair Item 
        /// </summary>
        /// <param name="sno"></param>
        /// <param name="reworkStation"></param>
        Rework addNew(string sno, string reworkStation);

        /// <summary>
        /// 扫描9999，结束工作流
        /// </summary>
        /// <param name="mbSno"></param>
        void save(string mbSno);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sno"></param>
        void cancel(string sno);
        #endregion

        #region "methods do not interact with the running workflow"

        #endregion
    }
}
