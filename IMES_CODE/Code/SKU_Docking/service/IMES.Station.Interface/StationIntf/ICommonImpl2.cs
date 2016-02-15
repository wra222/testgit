using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections;


namespace IMES.Station.Interface.StationIntf
{
    
    public interface ICommonImpl2
    {
        string CheckPodLabel(string custsn);
        string CheckConfigLabel(string custsn);
        IList<ConstValueTypeInfo> GetConstValueTypeByType(string type);
    }
}
