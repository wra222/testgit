/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:MB Split
 * UI:CI-MES12-SPEC-SA-UI MB Split.docx 
 * UC:CI-MES12-SPEC-SA-UC MB Split.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13  Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	连板切割入口，实现连板的切割
 *                2.	打印子板标签
 * UC Revision: 3924
 */

using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// MB Split
    /// 连板切割入口，实现连板的切割,并打印子板标签
    /// </summary> 
    /// 
    public interface IMBSplit
    {
        #region methods interact with the running workflow

        /// <summary>
        /// 刷mbsno，调用该方法启动工作流，根据输入mbsno获取model,获取新生成的NewMBList
        /// </summary>
        /// <param name="pdline">pdline</param>
        /// <param name="mbsno">mbsno</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station(Station="86")</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param>
        /// <param name="model">model</param>
        /// <param name="MBObjectList">MBObjectList</param>
        /// <returns></returns>
        IList<PrintItem> inputMBSno(string pdline, string mbsno, string editor, string station, string customer,IList<PrintItem> printItems, out string model,out IList<string> MBObjectList);

       

        #endregion


        #region methods do not interact with the running workflow


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="mbsno"></param>
        void Cancel(string mbsno);

        #endregion

        #region rePrint


        /// <summary>
        /// 重印
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="reason">reason</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param>
        IList<PrintItem> rePrint(string mbsno, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);

        #endregion

    }
}
