using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IStation
    {
        DataTable GetStation(string process, string DBConnection);
        DataTable GetStation(string DBConnection);
    }
}
