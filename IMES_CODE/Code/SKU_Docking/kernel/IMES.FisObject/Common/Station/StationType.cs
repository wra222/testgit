using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.Station
{
    /// <summary>
    /// 站点类型
    /// </summary>
    public enum StationType
    {
        Other,
        SATest, 
        SARepair, 
        SAOperation, 
        FATest, 
        FARepair,
        FAOperation,
        FAReturn,
        PAKKitting,
        FACombine, //2012.01.06 Asked by Song.J & Zhang.XM.
        Material,
        FACombineCheck,
        FAOfflineCombine
    }
}
