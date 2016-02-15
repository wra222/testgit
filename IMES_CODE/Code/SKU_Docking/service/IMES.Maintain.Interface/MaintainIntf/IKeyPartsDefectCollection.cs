using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IKeyPartsDefectCollection
    {
        //Select * from FailKPCollection 
        //where Date=[Date] and PdLine=[PdLine] order by Family, Parts

        IList<FailKPCollectionInfo> GetDefectPartList(DateTime date,string PdLine);


         
        //DELETE FROM [Line]
        //      WHERE Line='line'
        void Delete(int ID);


         
        //IF EXISTS(
        //SELECT [Line]
        //        FROM [IMES_GetData_Datamaintain].[dbo].[Line]
        //where [Line]='line'
        //)
        //set @return='True'
        //ELSE
        //set @return='False'
        //Boolean IsExistLine(string line);

        void AddLine(FailKPCollectionInfo item);

        void UpdateLine(FailKPCollectionInfo item);

        IList<ConstValueTypeInfo> GetConstValueTypeList(string Type);

        IList<int> ExistInFailKPCollection(FailKPCollectionInfo item);
    }
}
