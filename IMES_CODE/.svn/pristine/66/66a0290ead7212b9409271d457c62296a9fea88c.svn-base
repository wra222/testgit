using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 目标：AssignModel
    /// </summary>
    public interface IAssignModel
    {
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="model"></param>
        IList<string> GetActiveModel(string line, string model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custsn"></param>
        /// <param name="line"></param>
        /// <param name="model"></param>
        /// <param name="pdLine"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        ArrayList ToAssignModel(string custsn, string line, string family, string model, string pdLine, string editor, string stationId, string customerId, IList<PrintItem> printItems);
		
		/// <summary>
        /// </summary>
        ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);
    }

    
}
