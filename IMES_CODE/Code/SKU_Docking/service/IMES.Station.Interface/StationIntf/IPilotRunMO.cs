using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{

    public interface IPilotRunMO
    {
        IList<PilotMoInfo> GetPilotMoList(PilotMoInfo condition, string beginCdt, string endCdt);

        /// <summary>
        /// GetMOTypeList
        /// </summary>
        /// <param name="Type">Type</param>
        /// <returns></returns>
        IList<ConstValueInfo> GetMOTypeList(string Type);

        ArrayList GenPilotRunMo(PilotMoInfo item, string startmotype, string endmotype, string customer);

        void UpdatePilotMO(PilotMoInfo item);

        void DeletePilotMO(string mo);

        void ReleasePilotMO(PilotMoInfo item);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        IList<string> GetAdd_Stage();

        IList<ConstValueInfo> GetAdd_MoType(string stage);

        IList<string[]> GetAdd_Family(string stage);

        IList<string> GetAdd_Model(string mbcode);

        ArrayList Print(string pilotrunmo, string qty, string line, string editor, string station, string customer, IList<PrintItem> printItems);

    }
}
