using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IPdLine
    {
        DataTable GetPdLine(string customer,IList<string> process, string DBCconnection );
    }
}
