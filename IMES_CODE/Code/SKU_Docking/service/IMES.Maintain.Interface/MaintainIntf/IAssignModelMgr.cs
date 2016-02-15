using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IAssignModelMgr
    {

        DataTable GetByModelShipDate(string model, DateTime shipDate);

        void Import(IList<List<string>> lst, string editor);

        void Delete(IList<int> id);

    }
}
