// INVENTEC corporation ©2011 all rights reserved. 
// Description:PAQC Output 
// UI:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20 
// UC:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20                          
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-20   Du.Xuan                      Create 
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
    /// 品管部门使用该页面完成指定线别指定站点的的良品监控
    /// </summary>
    public interface IPAQCSorting
    {
        #region "methods interact with the running workflow"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custSn"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputSN(string custSn, string line, string editor, string station, string customer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortingID"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        ArrayList save(int sortingID, string custSN, string editor, string station, string customer);

        ArrayList updateStation(int sortingID, string editor, string station, string customer);

        #endregion

        #region "methods do not interact with the running workflow"

        /// <summary>
        /// 获取PAQC Sorting 支持的每一个站点信息
        /// </summary>
        /// <returns></returns>
        IList<PaqcsortingInfo> GetStationList(string line, string editor, out string message);
        /// <summary>
        /// PAKSortingQty
        /// </summary>
        /// <returns></returns>
        int getSortingQty();
        /// <summary>
        /// Least Pass Qty
        /// </summary>
        /// <param name="sortingID"></param>
        /// <param name="failTime"></param>
        /// <returns></returns>
        int getLeastPassQty(int sortingID, DateTime failTime);
        /// <summary>
        /// 取得Pass Qty
        /// </summary>
        /// <param name="sortingID"></param>
        /// <returns></returns>
        int getPassQty(int sortingID);

        #endregion
    }
}
