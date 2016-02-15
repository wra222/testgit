using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_MastereSOPQuery
    {

        //DataTable GetASTPart(string DBConnection);
        DataTable GetMastereSOP(string DBConnection, string ProId);
        //DataTable GetModel(string DBConnection, string PartNo);
        //DataTable GetASTeSOP(string DBConnection, string Model, string PartNo);
        
    }    
}
