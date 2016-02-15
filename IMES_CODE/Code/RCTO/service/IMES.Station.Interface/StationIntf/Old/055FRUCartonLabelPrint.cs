using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IFRUCartonLabelPrint
    {
        /// <summary>
        /// Carton包装
        /// 1. 生成Carton号码
        /// 2. 将所有刷入的机器的CT号码和Carton号码绑定
        /// 3. 打印标签
        /// </summary>
        /// <param name="fruList">FRU No List</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="printItems">Print Items</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Packing(IList<string> ctList,
             string editor, string stationId, string customerId,out string carton, IList<PrintItem> printItems);

        /// <summary>
        /// Reprint Label
        /// </summary>
        /// <param name="cartonNo">卡通号</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="printItems">Print Items</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Reprint(string cartonNo,
             string editor, string stationId, string customerId, IList<PrintItem> printItems);

        /// <summary>
        /// Unpack Carton
        /// </summary>
        /// <param name="cartonNo">Carton号</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        void Unpack(string cartonNo,
             string editor, string stationId, string customerId);

        string  ValidateFruCT(string fruCT);
        bool ValidateCartonNo(string cartonNo);
    }
}
