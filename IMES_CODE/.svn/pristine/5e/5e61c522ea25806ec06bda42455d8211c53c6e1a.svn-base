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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        ArrayList addSorting(string customer, string line, string station, string editor, string remark);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortingID"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        ArrayList updateStation(string sortingID, string editor, string station);

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
        /// 新增Product 记录至PAQCSortiing_Product
        /// </summary>
        /// <param name="sortingID"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        void insertSortingProduct(int sortingID, string line, string station,DateTime failTime);

        /// <summary>
        /// 取得Pass Qty
        /// </summary>
        /// <param name="sortingID"></param>
        /// <returns></returns>
        int getPassQty(int sortingID);

        #endregion
    }
}
