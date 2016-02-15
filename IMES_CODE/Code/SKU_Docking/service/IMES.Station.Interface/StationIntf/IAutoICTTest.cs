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
    public interface IAutoICTTest
    {
        #region "methods interact with the running workflow"
        /// <summary>
        /// CheckWC 
        /// </summary>
        /// <param name="toolId"></param>
        /// <param name="userId"></param>
        /// <param name="mbSN"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        ArrayList CheckWC(string toolId,string userId,string mbSN, string station, string line,string customer);

        /// <summary>
        /// Test Completed
        /// </summary>
        /// <param name="mbSN"></param>
        ArrayList ICTTestCompleted(string mbSN, string station, string toolId, string userId,string line,string customer,
                                                     string isPass, string failureCode, string testLogFilename, string actionName, IList<string> defectList, string testLogErrorCode);
        #endregion


    }
}
