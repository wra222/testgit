using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IRBPC_Tool
    {
        DataTable GetGroupList(string AccountId,string AppName);
        DataTable GetPermissionListByAccountId(string AccountId, string AppName);
    }
}
