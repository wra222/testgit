using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IPartCheck
    {

        /////
//        取得partCheck列表
//SELECT [Customer],[PartType],[ValueType],[NeedSave],[NeedCheck],[ID]
//  FROM [IMES_GetData].[dbo].[PartCheck] order by [Customer],[PartType],[ValueType]
DataTable GetPartCheckList();

//检查3列不能重复加入
//如果id==null，忽略[ID]<>'id'条件
        
//SELECT [ID]
//  FROM [IMES_GetData].[dbo].[PartCheck]
//WHERE [Customer]='customer' AND [PartType]='partType' AND [ValueType]='valueType'
//AND [ID]<>id
//GetExistPartCheck(String customer, string partType, string valueType, int id);



void DeletePartCheck(PartCheckDef item);

string AddPartCheck(PartCheckDef item);


string SavePartCheck(PartCheckDef item);

//取得MatchRule列表
//SELECT [RegExp],[PnExp],[PartPropertyExp],[ContainCheckBit],[ID],[PartCheckID]    
//  FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
//where [PartCheckID]=partCheckID
//order by [RegExp],[PnExp],[PartPropertyExp]
DataTable GetMatchRuleByPartCheckID(string partCheckID);

//删除一条PartCheckMatchRule
//DELETE FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
//      WHERE ID=id
void DeletePartCheckMatchRule(PartCheckMatchRuleDef item);


string AddPartCheckMatchRule(PartCheckMatchRuleDef item);


string SavePartCheckMatchRule(PartCheckMatchRuleDef item);


IList<SelectInfoDef> GetPartTypeList();

IList<SelectInfoDef> GetValueTypeList(string partType);

//        若RegExp、PartPropertyExp、PnExp的值序列已经被Match Rule List中某行使用
//如果id==null，忽略ID<>id 条件

//SELECT [ID]
//  FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
//Where [RegExp]='regExp' AND [PartPropertyExp]='partPropertyExp' AND [PnExp]='pnExp' AND ID<>id 
//GetExistPartCheckMatchRule(string regExp,string partPropertyExp,string pnExp,int id)




    }
}
