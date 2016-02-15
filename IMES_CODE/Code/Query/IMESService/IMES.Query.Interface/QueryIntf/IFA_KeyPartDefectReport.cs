using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_KeyPartDefectReport
    {
        DataTable GetKeyPartDefectReport(string Connection, DateTime FromDate, DateTime ToDate);
    }
}
