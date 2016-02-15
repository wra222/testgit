using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IForwarder
    {
        DataTable GetForwarderList(string StartDate, string EndDate);

        void ImportForwarder(List<ForwarderInfo> items);

        void UpdateForwarder(ForwarderInfo item);

        void DeleteForwarder(ForwarderInfo item);

        DateTime GetCurDate();
    }
}
