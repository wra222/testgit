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
    public interface IKeyPartsRequirementQuery
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <param name="outputmodels"></param>
        /// <returns></returns>
        DataTable KeyPartQuery(string models, out string outputmodels);
    }
}
