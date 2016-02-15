using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_ASTReport
    {

        DataTable GetASTReport(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> Family, IList<string> Model);
        DataTable GetSelectDetail(string DBConnection, DateTime StartTime, DateTime EndTime, string Model);
        DataTable GetFamily(string DBConnection );   
        DataTable GetModel(string DBConnection, IList<string> lFamily);
        DataTable GetASTModel(string DBConnection, IList<string> lFamily);
      
    }    
}
