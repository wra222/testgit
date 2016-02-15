using System;
using System.Collections.Generic;
using System.Linq;
using IMES.DataModel;
using System.Collections;
namespace IMES.Station.Interface.StationIntf
{
   public  interface IMaterialRequest
    {
       IList<MBRepairControlInfo> GeMaterialRequest(MBRepairControlInfo condition);
       IList<MBRepairControlInfo> GeMaterialRequest(MBRepairControlInfo condition, MBRepairControlInfo betweencondition, string betweenColumnName, DateTime beginValue, DateTime endValue);

       void AddMBRepairControl(MBRepairControlInfo condition);
       void DelMBRepairControl(MBRepairControlInfo condition);
       void UpdateMBRepairControl(MBRepairControlInfo condition);
    }
}
