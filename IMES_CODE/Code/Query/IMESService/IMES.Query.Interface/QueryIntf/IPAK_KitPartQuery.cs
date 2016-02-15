using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_KitPartQuery
    {

        DataTable GetData(string DBConnection, IList<string> Model, IList<string> Cnt);
        DataTable GetFamily(string DBConnection);
        DataTable GetModel(string DBConnection, IList<string> lFamily);
        DataTable GetData(string DBConnection, DataTable ModelLine, bool IsGroupByLine);
    }    
}
