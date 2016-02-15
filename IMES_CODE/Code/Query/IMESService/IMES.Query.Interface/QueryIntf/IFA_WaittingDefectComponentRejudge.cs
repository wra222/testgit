using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
   public interface IFA_WaittingDefectComponentRejudge
    {
       DataTable GetDefectComponentRejudgeQueryResult(string Connection, DateTime FromDate, DateTime ToDate, String parttype,String status);
    }
}
