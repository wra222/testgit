using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPCAOQCCosmetic
    {
        ArrayList CheckAndGetMBInfo(string mbsn, string line, string editor, string station, string customer);

        void WFCancel(string key);

        void Save(string key, string mbsn, string lotNo, string remark, string check, IList<string> list);

        ArrayList Query();

        ArrayList GetDefectInfo(string code);
    }
}
