using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Query.Interface.QueryIntf
{
   public interface IPAK_MrpQuery
    {
       string GetMrpLabelByDN( string Connection,string DeliveryNoOrCustsn);
    }
}
