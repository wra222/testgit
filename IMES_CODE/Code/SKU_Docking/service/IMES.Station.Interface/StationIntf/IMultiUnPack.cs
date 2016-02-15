using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.BSamIntf;

using System.Collections;

using System.Data;


namespace IMES.Station.Interface.StationIntf
{

    public interface IMultiUnPack
    {
        DataTable InputSnList(List<string> snList, string pdline, string editor, string station, string customer);
    }
}
