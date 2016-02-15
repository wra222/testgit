// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 用于Virtual，VirtualPallet站，传递给前台的参数
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-09   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class ProductModel
    {
        public string Model;
        public string CustSN;
        public string ProductID;
        public string DeliveryNo;
        public string PalletNo;
        public string CartonNo;
    }
}
