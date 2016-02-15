/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: FRU Gift Label Print Interface
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-03-09   LuycLiu     Create 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IFRUGiftLabelPrint
    {
        /// <summary>
        /// 打印Gift标签
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="pnoList">Part NO 列表</param>
        /// <param name="scanList">scan列表</param>
        /// <param name="qty">数量</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="giftId">gift no</param>
        /// <param name="printItems">Print Items</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Print(string model, IList<string> pnoList, IList<IList<string>> scanList,int qty,
             string editor, string stationId, string customerId, out string giftID, IList<PrintItem> printItems);

        /// <summary>
        /// 重印Gift标签
        /// </summary>
        /// <param name="giftNo">gift NO</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="printItems">Print Items</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Reprint(string giftNo,
             string editor, string stationId, string customerId, IList<PrintItem> printItems);

        /// <summary>
        /// 根据Model取得PartNo和Descr列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>PartNo和Descr列表</returns>
        IList<PartNoDescrInfo> GetPartNoDescrListByModel(string model);

        /// <summary>
        /// 验证MB并返回其111
        /// 1、MB不合法返回异常
        /// 2、MB的WC不是8G返回异常
        /// 3、取得MB的111并返回
        /// </summary>
        /// <param name="mb">MB</param>
        /// <returns>111阶</returns>
        string Get8GMB111(string mb);

    }
}
