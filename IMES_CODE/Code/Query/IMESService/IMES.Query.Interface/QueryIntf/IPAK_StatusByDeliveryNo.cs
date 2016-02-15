using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_StatusByDeliveryNo       
    {
        DataTable GetQueryResult(string Connection, List<string> deliveryNo, DateTime shipDate);
    }
    
}
