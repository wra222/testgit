using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    /// <summary>
    /// UI要使用的StationDescrQty信息
    /// </summary>
    [Serializable]
    public class StationDescrQty
    {
        public string Descr;
        public string Station;
        public int Qty;

        public StationDescrQty(string descr, string station, int qty)
        {
            this.Descr = descr;
            this.Station = station;
            this.Qty = qty;
        }
    }
}
