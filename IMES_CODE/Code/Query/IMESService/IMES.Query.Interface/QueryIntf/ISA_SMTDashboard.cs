using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_SMTDashboard 
    {
        List<DataTable> GetQueryResult(string Connection, string line);
        List<SMT_DashBoard_Line> GetQueryLine();
        IList<SMT_DashBoard_MantainInfo> GetSMTDashBoardLineListByCondition(SMT_DashBoard_MantainInfo condition);
        IList<SMT_DashBoard_MantainInfo> GetSMTDashBoardLineListById(SMT_DashBoard_MantainInfo condition);
        IList<SMT_DashBoard_MantainInfo> GetSMTDashBoardRefreshTimeAndStationByLine(SMT_DashBoard_MantainInfo condition);
        void AddSMTDashboardInfo(SMT_DashBoard_MantainInfo condition);
        void UpdateSMTDashBoardInfo(SMT_DashBoard_MantainInfo condition);
        void AddOrUpDateSMTDashboardRefreshTimeAndStation(SMT_DashBoard_MantainInfo condition, string DBConnection);

    }


    [Serializable]
    public class SMT_DashBoard_Line
    {
        public string line;
    }

    [Serializable]
    public class SMT_DashBoard_MantainInfo
    {
        public int id;
        public string line;
        public int refreshTime;
        public string durTime;
        public int standardOutput;
        public string station;
        public string editor;
        public DateTime refreshTimeStationCdt;
        public DateTime refreshTimeStationUdt;
        public DateTime StandardOutputCdt;
        public DateTime StandardOutputUdt;
    }

}
