using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IAdjustMO
    {
        /// <summary>
        /// 把选择的Unit调整到新的MO
        /// </summary>
        /// <param name="oldMo">old mo</param>
        /// <param name="newMo">new mo</param>
        /// <param name="adjustQty">Adjust Qty</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">station id</param>
        /// <param name="customerId">customer id</param>
        void Adjust(string oldMo,
            string  newMo,
            int adjustQty,
            IList<string> productList,
            string pCode,
            string editor, string pdLine,string stationId, string customerId);

        IList<IMES.DataModel.ProductStatus> GetProductFromProductStatus(string mo);

        IList<MOInfo> GetOldMOList(string model);
        IList<MOInfo> GetnewMOList(string model);
    }
}
