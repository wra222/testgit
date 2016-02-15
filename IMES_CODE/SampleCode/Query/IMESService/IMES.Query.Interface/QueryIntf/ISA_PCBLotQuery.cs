using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_PCBLotQuery
    {
        DataTable GetPCBLotQueryResult(string Connection, String LotNo, String PCBNo, Boolean NumType);
    }
}
