using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IStation2
    {
        //SELECT [Station]
        //      ,[StationType]
        //      ,[OperationObject]
        //      ,[Descr]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Station]
        //order by [Cdt]
        DataTable GetStationInfoList();

        //DELETE FROM [Station]
        //      WHERE Station='station'
        void DeleteStation(string station); 

        //Boolean IsExistStation(string station)
        //IF EXISTS(
        //SELECT [Station]     
        //  FROM [Station]
        //where Station ='station)'
        //)
        //set @return='True'
        //ELSE
        //set @return='False' 

         string AddStation(StationDef item);

         string UpdateStation(StationDef item, string oldStationId);

         IList<StationDef> getStationByStationType(string stationType);

        #region for StationAttr table

        IList<StationAttrDef> GetStationAttr(string station);
        String GetStationAttrValue(string station, string attrName);
        void AddStationAttr(StationAttrDef attr);
        void UpdateStationAttr(StationAttrDef attr);
        void DeleteStationAttr(string station, string attrName);

        #endregion
    }
}
