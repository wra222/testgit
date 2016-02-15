using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IFRUCartonLabelPrintCT
    {
        /// <summary>
        /// 当FRU出货的包装方式没有PIZZA时，采用此功能装箱
        /// </summary>
        /// <param name="fruCTorMB">fruCTorMB号</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="printItems">Print Items</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Print(string model, IList<string> pnoList, IList<IList<string>> scanList, int qty,
            string editor, string stationId, string customerId, out string carton, IList<PrintItem> printItems);

        /// <summary>
        /// 重印Carton标签
        /// </summary>
        /// <param name="carton">Carton号</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="printItems">Print Items</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Reprint(string carton,
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

        bool ValidateCartonNo(string cartonNo);
    }
}
