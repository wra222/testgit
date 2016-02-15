using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_KeyPartsRequirementQuery
    {

        DataTable GetData(string DBConnection, string models, out string outputmodels);

    }    
}
