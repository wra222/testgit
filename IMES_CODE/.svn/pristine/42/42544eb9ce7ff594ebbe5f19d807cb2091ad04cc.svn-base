using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IFRUShiptoLabelPrint
    {
        /// <summary>
        /// 打印FRU shipto标签，TSB这边散货贴在外箱上，上栈板的机器贴塑料薄膜上，船务的相关资料都在里面。
        /// 按照DN打印，不是按照pallet打印
        /// </summary>
        /// <param name="dn">DN</param>
        /// <param name="qty">QTY</param>
        /// <param name="inqty">INQTY</param>
        /// <param name="piece">PIECE</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="printItems">Print Items</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Print(string dn, string editor, string stationId, string customerId, IList<PrintItem> printItems);

        /// <summary>
        /// Query DN by user input
        /// </summary>
        /// <param name="startShipdate">Start ship date</param>
        /// <param name="endShipdate">End ship date</param>
        /// <param name="pono">PO No</param>
        /// <param name="model">Model</param>
        /// <param name="dn">DN</param>
        /// <returns>DN shipto Info, please see UI spec for detail</returns>
        IList<DNForUI> getDNList(DNQueryCondition MyCondition);

        /// <summary>
        /// Query Pallet shipto info by DN
        /// </summary>
        /// <param name="dn">DN</param>
        /// <returns>Pallet shipto info, please check UP spec for detail</returns>
        IList<DNPalletQty> getPalletList(string dn);
    }
}
