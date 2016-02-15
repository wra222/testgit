using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_IdleTime
    {
        DataTable GetIdleTimeResult(string Connection,string SelectDay,int SelectHours);
        DataTable GetIdleTimeDetailResult(string Connection, string SelectDay, int SelectHours, string Station, string Line);
    }
}
