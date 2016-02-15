using System;
using System.Data;
using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUpdateConsolidate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consolidate"></param>
        /// <returns></returns>
        DataTable GetAbnormalConsolidate(string consolidate);

        ArrayList Update(string pdline, string station, string editor, string customer, string consolidate, string actqty);
    }
}
