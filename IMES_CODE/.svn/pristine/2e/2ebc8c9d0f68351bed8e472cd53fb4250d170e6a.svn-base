using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// ICT 自動測試站
    /// </summary>
    public interface IDefectComponentRejudge
    {
        ArrayList GetDefectComponentInfo(string sn, string customer, string status, string user);

        void Save(string sn, string status, string comment);

        DataTable GetQuery(string partSN, string customer);

        void Cancel(string sn);
    }
}
