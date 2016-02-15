using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ICheckItem
    {

        //        取得列表
        //如果conditionValue=='', CheckItemSetting.CheckCondition = 'conditionValue' 条件变为 

        //CheckItemSetting.CheckCondition IS NULL AND CheckItemSetting.CheckCondition='' 

        //SELECT CheckItemSetting.Station, CheckItem.ItemName, CheckItem.MatchRule, CheckItemSetting.CheckType, 

        //CheckItemSetting.CheckValue, 
        //   CheckItemSetting.ID, CheckItemSetting.CheckItemID, CheckItemSetting.Customer, CheckItemSetting.CheckCondition, 
        //   CheckItemSetting.CheckConditionType, CheckItem.Mode, CheckItem.ItemType, CheckItem.ItemDisplayName
        //FROM  CheckItem INNER JOIN
        //      CheckItemSetting ON CheckItem.ID = CheckItemSetting.CheckItemID 
        //WHERE     CheckItemSetting.Customer = 'Customer'  AND CheckItemSetting.CheckConditionType = 'conditionType' AND 

        //CheckItemSetting.CheckCondition = 'conditionValue'
        //order by Station,ItemName

        //问题：是否需要AND CheckItem.Customer = CheckItemSetting.Customer条件  no
        //问题：点击一条数据的时候2、3物件不需要更新 ！
        DataTable GetCheckItemListByCustomAndCondition(string custom, string conditionType, string conditionValue);


       //  DELETE FROM [IMES_GetData].[dbo].[CheckItemSetting]
       //WHERE ID='id'
       void DeleteCheckItemSetiing(CheckItemSettingDef item);


        //添加修改时检查数据库中2项数据重复
        //    如果id==null，忽略[ID]<>'id'条件
        //如果checkCondition==''， CheckCondition='checkCondition'条件变为CheckCondition='checkCondition' AND CheckCondition 

        //IS NULL

        //SELECT [ID] FROM [IMES_GetData].[dbo].[CheckItemSetting]
        //WHERE [CheckItemID]='CheckItemID' AND [Station]='station' 
        //AND CheckConditionType='checkConditionType' AND CheckCondition='checkCondition'
        //AND [ID]<>'id'
        //DataTable GetExistCheckItemSetiing(string checkItemID,string station,string checkConditionType, string checkCondition, string id);


        //SELECT [ID] FROM [IMES_GetData].[dbo].[CheckItem] where [ItemName]='itemName' AND ID<>id
        //DataTable GetExistCheckItem(string itemName, int id);

        //增加一条CheckItem
        //id被写回在里面
        String AddCheckItem(CheckItemDef item);

        //增加一条CheckItemSetting
        //id被写回在里面
        String AddCheckItemSetting(CheckItemSettingDef item);

        //更新一条CheckItemSetting
        void SaveCheckItemSetting(CheckItemSettingDef item);

                //        SELECT     Station.Station 
        //FROM         CheckItem INNER JOIN
        //Station ON CheckItem.Mode = Station.OperationObject
        //WHERE CheckItem.ID=checkItemId order by Station
        //若新选项不是“Add a new Check Item”，则Station下拉列表中选项换为与新当前CheckItem对应Mode一致（Station.OperationObject）的Station。
        IList<SelectInfoDef> GetStationListByCheckedItemID(string checkItemId);

        //取得CheckItem下拉列表
        //SELECT [ID],[ItemName] FROM [IMES_GetData].[dbo].[CheckItem] WHERE Customer='customer' order by [ItemName]
        IList<SelectInfoDef> GetItemNameListByCustomer(string customer);

    }
}



