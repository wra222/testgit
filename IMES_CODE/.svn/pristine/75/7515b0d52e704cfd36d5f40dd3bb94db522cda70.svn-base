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
    public interface IWHInspection
    {
        /// <summary>
        /// GetMaterialType
        /// </summary>
        /// <param name="type">string</param>
        /// <returns></returns>
        IList<ConstValueTypeInfo> GetMaterialType(string type);

        /// <summary>
        /// InputMaterialCT
        /// </summary>
        /// <param name="materialct">string</param>
        /// <param name="materialtype">string</param>
        /// <param name="pdLine">string</param>
        /// <param name="editor">string</param>
        /// <param name="stationId">string</param>
        /// <param name="customerId">string</param>
        /// <returns></returns>
        ArrayList InputMaterialCT(string materialct, string materialtype, string pdLine, string editor, string stationId, string customerId);
     
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="materialct">string</param>
        //void Save(string materialct);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

    }
}