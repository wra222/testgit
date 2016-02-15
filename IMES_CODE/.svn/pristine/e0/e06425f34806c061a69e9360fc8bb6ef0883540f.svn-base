using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Query.Interface.QueryIntf
{
    public  interface IConfigDB
    {
        List<string> GetHistoryDBList();

        string GetOnlineDB();

        List<string> GetOnlineDBList();

        string GetSelectedDB(string DBType, int DBIndex);
        DBInfo GetDBInfo();

        string GetOnlineDefaultDBName();
        bool CheckDockingDB(string DBName);
      

    }
    [Serializable]
    public struct DBInfo
    {

        public string[] OnLineDBList;
        public string[] HistoryDBList;
        public string OnLineConnectionString;
        public string HistoryConnectionString;
      
       
    }
}
