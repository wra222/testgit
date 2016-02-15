using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IModel_ASTQyery
    {
        DataTable GetData1(string DBConnection,DataTable  table);    
    }    
}
