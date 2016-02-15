using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IPartCheckSetting
    {

//        取得PartCheckSetting列表
//如果model=="", [Model]='model'条件变成[Model]='model' AND [Model] IS NULL两个条件

//SELECT [Station],[PartType],[ID],[Customer],[Model]
//  FROM [IMES_GetData_Datamaintain].[dbo].[PartCheckSetting]
// where [Customer]='customer' AND [Model]='model'
//order by [Station],[PartType]
        //DataTable GetPartCheckSettingList(string customer, string model);

        IList<PartCheckSetting> GetPartCheckSettingList();

//如果model=''
//model->null
        string AddPartCheckSetting(PartCheckSettingDef item);


//    如果model=''
//model->null
        string SavePartCheckSetting(PartCheckSettingDef item);


        void DeletePartCheckSetting(PartCheckSettingDef item);


        IList<SelectInfoDef> GetCustomerModelList(string customer);


        IList<SelectInfoDef> GetStationList();


        IList<SelectInfoDef> GetCustomerPartTypeList(string customer);

        IList<SelectInfoDef> GetValueTypeListByCustomerAndPartType(string customer, string partType);

//        判断Station、Part Type的值对已经被Part Check List中其他Part Type使用
//如果id==null,忽略ID<>'id'条件
//如果model=="", [Model]='model'条件变为[Model]='model' AND [Model] IS NULL两个条件
//如果station=="", [Station]='station' 条件变为[Station]='station'  AND [Station] IS NULL两个条件
//如果partType=="", [PartType]='partType'条件变为[PartType]='partType' AND [PartType] IS NULL两个条件

//SELECT [ID] FROM [IMES_GetData].[dbo].[PartCheckSetting]
//WHERE [Customer]='customer' AND [Model]='model'
//AND [Station]='station' AND [PartType]='partType' AND ID<>'id'
 //DataTable GetExistPartCheckSetting(string customer,string model, string station,string partType, int id);

    }
}
