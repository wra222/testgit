// INVENTEC corporation ©2011 all rights reserved. 
// Description:PAQC Input 
// UI:CI-MES12-SPEC-SA-UC BGA Input.docx –2012/1/04 
// UC:CI-MES12-SPEC-SA-UC BGA Input.docx –2012/1/04                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-1-04   liuqingbiao                  Create 
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
    public interface IBGAInput
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
        /// 显示过 list 到页面后，调用此过程返回到 workflow，存数据
        /// </summary>
        /// <param name="snoScaned"></param>
        void save(string snoScaned);//, IList<string> defectCodeList);

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
