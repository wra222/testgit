using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    /// <summary>
    /// UI要使用的PalletCollection信息
    /// </summary>
    [Serializable]
    public class PalletCollectionUI
    {
        
        public string CartonNo;
        public string DeliveryNo;
        public string PalletNo;
        public string Loc;
        public int TotalQty;
        public int PackedQty;
    }
}
