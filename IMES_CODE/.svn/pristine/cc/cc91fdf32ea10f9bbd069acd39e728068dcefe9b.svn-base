using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;


namespace IMES.Maintain.Implementation
{
    public class PartCheckManager : MarshalByRefObject,IPartCheck
    {
        //const int BOM_MAX_DEEP_LEVEL=18;
        #region Implementation of IPartCheck

        /////
        //        取得partCheck列表
        //SELECT [Customer],[PartType],[ValueType],[NeedSave],[NeedCheck], Editor,Cdt,Udt, [ID] 
        //       FROM [IMES_GetData].[dbo].[PartCheck] order by [Customer],[PartType],[ValueType]
        public DataTable GetPartCheckList()
        {
            DataTable result = new DataTable();
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                result = itemRepository.GetPartCheckList();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartCheck(PartCheckDef item)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                PartCheck newItem = new PartCheck(Int32.Parse(item.ID), "", "", "", 0, 0, null);
 
                itemRepository.DeletePartCheck(newItem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //检查3列不能重复加入
        //如果id==null，忽略[ID]<>'id'条件

        //SELECT [ID]
        //  FROM [IMES_GetData].[dbo].[PartCheck]
        //WHERE [Customer]='customer' AND [PartType]='partType' AND [ValueType]='valueType'
        //AND [ID]<>id
        //GetExistPartCheck(String customer, string partType, string valueType, int id);
        public string AddPartCheck(PartCheckDef item)
        {
            string result = "";
            try
            {

                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                PartCheck newItem = new PartCheck(0,
                item.Customer,
                item.PartType,
                item.ValueType,
                Int32.Parse(item.NeedSave),
                Int32.Parse(item.NeedCheck), null);
                newItem.Editor = item.Editor;
                newItem.Cdt = DateTime.Now;
                newItem.Udt = DateTime.Now;

                DataTable existList = itemRepository.GetExistPartCheck(newItem.Customer, newItem.PartType, newItem.ValueType);
                if (existList != null && existList.Rows.Count > 0)
                {
                    //已经存在具有相同PartType、ValueType和Customer的记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT035", erpara);
                    throw ex;
                }

                itemRepository.AddPartCheck(newItem);
                result = newItem.ID.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public string SavePartCheck(PartCheckDef item)
        {
            string result = "";
            try
            {

                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                PartCheck newItem =new PartCheck(Int32.Parse(item.ID),
                item.Customer,
                item.PartType,
                item.ValueType,
                Int32.Parse(item.NeedSave),
                Int32.Parse(item.NeedCheck), null);
                newItem.Editor = item.Editor;
                newItem.Udt = DateTime.Now;

                DataTable existList = itemRepository.GetExistPartCheck(newItem.Customer, newItem.PartType, newItem.ValueType, Int32.Parse(item.ID));
                if (existList != null && existList.Rows.Count > 0)
                {
                    //已经存在具有相同PartType、ValueType和Customer的记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT035", erpara);
                    throw ex;
                }

                itemRepository.SavePartCheck(newItem);
                result = item.ID.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        //取得MatchRule列表
        //SELECT [RegExp],[PnExp],[PartPropertyExp],[ContainCheckBit], Editor,Cdt,Udt, [ID],[PartCheckID]    
        //  FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
        //where [PartCheckID]=partCheckID
        //order by [RegExp],[PnExp],[PartPropertyExp]
        public DataTable GetMatchRuleByPartCheckID(string partCheckID)
        {

            DataTable result = new DataTable();
            try
            {
                int id = Int32.Parse(partCheckID);
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                result = itemRepository.GetMatchRuleByPartCheckID(id);                
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        //删除一条PartCheckMatchRule
        //DELETE FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
        //      WHERE ID=id
        public void DeletePartCheckMatchRule(PartCheckMatchRuleDef item)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                PartCheckMatchRule newItem = new PartCheckMatchRule();
                newItem.ID = Int32.Parse(item.ID);
                itemRepository.DeletePartCheckMatchRule(newItem);
            }
            catch (Exception)
            {
                throw;
            }  
        }

        public string AddPartCheckMatchRule(PartCheckMatchRuleDef item)
        {
            string result = "";
            try
            {

                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                PartCheckMatchRule newItem = new PartCheckMatchRule();
                newItem.ContainCheckBit = Int32.Parse(item.ContainCheckBit);
                newItem.PartCheckID = Int32.Parse(item.PartCheckID);
                newItem.PartPropertyExp = item.PartPropertyExp;
                newItem.PnExp = item.PnExp;
                newItem.RegExp = item.RegExp;
                newItem.Editor = item.Editor;
                newItem.Cdt = DateTime.Now;
                newItem.Udt = DateTime.Now;

                DataTable existList = itemRepository.GetExistPartCheckMatchRule(newItem.RegExp, newItem.PartPropertyExp, newItem.PnExp, newItem.PartCheckID);
                if (existList != null && existList.Rows.Count > 0)
                {
                    //已经存在具有相同RegExp、PartPropertyExp、PnExp和PartCheckID的记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT036", erpara);
                    throw ex;
                }

                itemRepository.AddPartCheckMatchRule(newItem);
                result = newItem.ID.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public string SavePartCheckMatchRule(PartCheckMatchRuleDef item)
        {
            string result = "";
            try
            {

                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                PartCheckMatchRule newItem = new PartCheckMatchRule();
                newItem.ContainCheckBit = Int32.Parse(item.ContainCheckBit);
                newItem.PartCheckID = Int32.Parse(item.PartCheckID);
                newItem.PartPropertyExp = item.PartPropertyExp;
                newItem.PnExp = item.PnExp;
                newItem.RegExp = item.RegExp;
                newItem.ID = Int32.Parse(item.ID);
                newItem.Editor = item.Editor;
                newItem.Udt = DateTime.Now;

                DataTable existList = itemRepository.GetExistPartCheckMatchRule(newItem.RegExp, newItem.PartPropertyExp, newItem.PnExp,newItem.PartCheckID, newItem.ID);
                if (existList != null && existList.Rows.Count > 0)
                {
                    //已经存在具有相同RegExp、PartPropertyExp、PnExp和PartCheckID的记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT036", erpara);
                    throw ex;
                }

                itemRepository.SavePartCheckMatchRule(newItem);
                result = newItem.ID.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }


        //PartType列表，
        //SELECT [PartType]
        //       FROM [IMES_GetData].[dbo].[PartType] ORDER BY [PartType]
        //选项包括系统中所有的Part Type 
        //PartTypeAttribute
        public IList<SelectInfoDef> GetPartTypeList()
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                DataTable getData = itemRepository.GetPartTypes();
                for (int i = 0; i < getData.Rows.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    item.Value = Null2String(getData.Rows[i][0]);
                    item.Text = Null2String(getData.Rows[i][0]);
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        //Value Type列表

        //SELECT [Code]      
        //  FROM [IMES_GetData].[dbo].[PartTypeAttribute]
        //where PartType='partType' ORDER BY [Code]   

        //Change：1、ValueType下拉列表的选项全部换为与新选PartType相关的属性名称，其第一个选项被选取。
        //ValueType 选项包括PartNo、Descr、CustPartNo、FruNo、Vendor、IECVersion以及当前Part Type的所有Attribute
        public IList<SelectInfoDef> GetValueTypeList(string partType)
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            String param = Null2String(partType);
            if (param == "")
            {
                return result;
            }

            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                DataTable getData = itemRepository.GetValueTypeList(param);
                for (int i = 0; i < getData.Rows.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    item.Value = Null2String(getData.Rows[i][0]);
                    item.Text = Null2String(getData.Rows[i][0]);
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        //        若RegExp、PartPropertyExp、PnExp的值序列已经被Match Rule List中某行使用
        //如果id==null，忽略ID<>id 条件

        //SELECT [ID]
        //  FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
        //Where [RegExp]='regExp' AND [PartPropertyExp]='partPropertyExp' AND [PnExp]='pnExp' AND ID<>id 
        //GetExistPartCheckMatchRule(string regExp,string partPropertyExp,string pnExp,int id)


        #endregion

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        } 
    }
}