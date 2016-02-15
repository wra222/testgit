using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_DockSnQuery
    {
        DataTable GetDockSnQueryResult(string Connection, String CUSTSN, String Model, Boolean NumType);
    }
}
