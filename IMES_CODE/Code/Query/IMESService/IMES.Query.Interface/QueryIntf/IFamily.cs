using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IFamily
    {
        DataTable GetFamily(string DBConnection);
        DataTable GetFamily(string family, string DBConnection);
        DataTable GetPCBFamily(string DBConnection);
        DataTable GetPCBFamily(string DBConnection,string TableName);
   

    }   
}
