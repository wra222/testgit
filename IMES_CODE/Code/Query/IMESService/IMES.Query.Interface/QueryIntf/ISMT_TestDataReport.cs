using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
 public   interface ISMT_TestDataReport
    {
        DataTable GetModel(string DBConnection, IList<String> PdLine,  DateTime From, DateTime To);
       // DataTable GetStation(string DBConnection, IList<String> Family);
       // DataTable GetPdLine(string DBConnection, IList<String> Family);
        DataTable GetTestCount(string DBConnection, IList<String> Model, IList<String> PdLine, IList<String> Station, DateTime From,DateTime To);
        DataTable Get_Defect_Rate(string DBConnection, IList<String> Model, IList<String> PdLine, IList<String> Station, DateTime From, DateTime To,string Type);
        DataTable Get_Defect_Top(string DBConnection, IList<String> Model, IList<String> PdLine, IList<String> Station, DateTime From, DateTime To);
        DataTable Get_Defect_Analysis(string DBConnection, IList<String> Model, IList<String> PdLine, IList<String> Station, DateTime From, DateTime To);
        DataTable Get_Detial(string DBConnection, string Model, string PdLine, string Station, DateTime From, DateTime To, string Defect, string Tp);
        
    }
}
