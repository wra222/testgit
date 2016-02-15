using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    /// <summary>
    /// UI要使用的DNPallet信息
    /// </summary>
    [Serializable]
    public class DNPalletQty
    {
        public int ID;
        public string PalletNo;
        public string UCC;
        public int DeliveryQty;
    }
}
