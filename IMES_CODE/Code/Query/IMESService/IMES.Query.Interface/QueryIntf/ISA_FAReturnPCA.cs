using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_FAReturnPCA
    {

        string[] GetModel(string DBConnection);

        DataTable GetFAReturnPCAInfo(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> lstModel);    
    }    
}
