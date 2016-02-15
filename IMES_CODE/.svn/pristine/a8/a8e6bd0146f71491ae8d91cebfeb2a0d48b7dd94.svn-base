using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// ICT 自動測試站
    /// </summary>
    public interface IDismantlePilotRun
    {
        #region "methods interact with the running workflow"
        /// <summary>
        /// 
        /// </summary>
        
        /// <returns></returns>
        IList<string> GetStage();
        void Cancel(string sn);
        void Dismantle(string sn);
        /// <summary>
        /// Test Completed
        /// </summary>
        /// <param name="mbSN"></param>
        ArrayList GetPilotMoInfo(string sn, string stage, string customer, string station, string user);
        #endregion


    }
}
