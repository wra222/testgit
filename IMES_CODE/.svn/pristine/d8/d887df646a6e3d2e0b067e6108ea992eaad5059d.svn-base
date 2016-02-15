using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IFRUTravelCard
    {
        /// <summary>
        /// 打印FRU Travel Card
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="qty">Unit总数量</param>
        /// <param name="GiftOrCarton">true if gift is selected; false if carton is chosen</param>
        /// <param name="pcs">数量</param>
        /// <param name="gqty">unit总数量</param>
        /// <param name="cqty">每箱中unit数量</param>
        /// <param name="ys">不满箱的unit数量，即unit总数量/每箱中unit数量的余数</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">sation id</param>
        /// <param name="customerId">customer id</param>
        IList<PrintItem> Print(string model,
            int qty,
            Boolean GiftOrCarton,
            int pcs,
            int gqty, int cqty, int ys,
            string editor,string PdLine, string stationId, string customerId,IList<PrintItem> printItems);
    }
}
