using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface ICOAStatus
    {
        DataTable GetCOAStatus(string Connection);
        DataTable GetCOADate(string Connection);
    }
}
