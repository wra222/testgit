using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_TouchGlassTimeQuery
    {
        void InsertShipNo(string DBConntion, DataTable dt);
        DataTable GetCTMessage(string DBConntion, DateTime from, DateTime to, string Tp);
        DataTable GetCTNo(string DBConntion,string CT);
    }
}
