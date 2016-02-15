using System;
using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    [Serializable]
    public class LineExinfo
    {
        public string line;
        public string customerID;
        public string stage;
        public string descr;
        public string AliasLine;
        public int AvgManPower;
        public int AvgSpeed;
        public int AvgStationQty;
        public string IEOwner;
        public string Owner;
        public string editor;
        public DateTime cdt;
        public DateTime udt;
    }
    public interface ILine
    {
        //SELECT [Line]
        //      ,[Descr]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
        //      ,[CustomerID]
        //      ,[Stage]
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Line]
        //where [CustomerID]='CustomerID' AND [Stage]='stage'
        //order by [Line]
        DataTable GetLineInfoList(string customer,string stage);



         
        //SELECT [Stage]
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Stage]
        //order by [Stage]

        List<SelectInfoDef> GetStageList();


         
        //DELETE FROM [Line]
        //      WHERE Line='line'
        void DeleteLine(string line);


         
        //IF EXISTS(
        //SELECT [Line]
        //        FROM [IMES_GetData_Datamaintain].[dbo].[Line]
        //where [Line]='line'
        //)
        //set @return='True'
        //ELSE
        //set @return='False'
        //Boolean IsExistLine(string line);

        string AddLine(LineDef item);

        string UpdateLine(LineDef item, string oldLineId);

        void SaveLineEx(LineExinfo item);
    }
}
