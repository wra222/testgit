using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_IMG_2PPQuery
    {
        DataTable GetQueryResult(string Connection, 
                            IList<string> lstPdLine, IList<string> Model, bool IsWithoutShift, string ModelCategory);
        DataTable GetQueryResultByModel(string Connection, 
                            IList<string> lstPdLine,IList<string> Model, bool IsWithoutShift, string ModelCategory);
    }
}
