/*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface ICheckDoorSn
    {
        /// <summary>
        /// CheckDoorSn初次输入SN处理
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>ProductID</returns>
        ArrayList InputCustSN(string custsn, string pdLine, string editor, string stationId, string customerId);
     


        /// <summary>
        /// Save
        /// </summary>
        /// <param name="custsnOnPizza">Customer SN </param>

        void Save(string custsn);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

    }
}