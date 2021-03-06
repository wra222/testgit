﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Data;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Misc;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Maintain.Implementation
{
    public class ConstValueType : MarshalByRefObject, IConstValueType    
    {
       
        #region Implementation of IConstValueMaintain


        /// <summary>
        /// 取得所有ConstValue数据的list(按Type栏位排序)
        /// </summary>
        /// <returns></returns>
        public IList<ConstValueTypeInfo> GetConstValueTypeList()
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IList<ConstValueTypeInfo> retLst = itemRepository.GetConstValueTypeList("SYS");//itemRepository.GetTypeListFromConstValue();

                return retLst;
            }
            catch (Exception)
            {
                throw;
            } 
        }

        /// <summary>
        /// 根据Type取得对应的ConstValue数据的list(按Name栏位排序)
        /// </summary>
        /// <param name="type">过滤条件Type</param>
        /// <returns></returns>
        public IList<ConstValueTypeInfo> GetConstValueTypeList(String type)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                
                IList<ConstValueTypeInfo> retLst = itemRepository.GetConstValueTypeList(type);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }                       
        }


        /// <summary>
        /// 增加一条ConstValue的记录数据(update/insert)
        /// </summary>
        /// <param name="obj">增加ConstValueInfo结构</param>
        public void InsertConstValueType(ConstValueTypeInfo info)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                itemRepository.InsertConstValueType(info);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 保存一条ConstValue的记录数据(update/insert)
        /// </summary>
        /// <param name="obj">更新ConstValueInfo结构</param>
        public void UpdateConstValueType(ConstValueTypeInfo obj)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();

                ConstValueTypeInfo tempConstValue = new ConstValueTypeInfo();
                tempConstValue.id = obj.id;
                if (tempConstValue.id > 0)
                {
                    itemRepository.UpdateConstValueType(obj);
                }
                else
                {
                    itemRepository.InsertConstValueType(obj);
                }
                //tempConstValue.type = obj.type;
                //tempConstValue.value = obj.value;
                //tempConstValue.description = obj.description;
                //IList<ConstValueTypeInfo> exists = itemRepository.GetConstValueTypeList(tempConstValue.type);
                
                //if (exists != null && exists.Count > 0)
                //{
                //    foreach (ConstValueTypeInfo s in exists)
                //    {
                //        if (s.value == tempConstValue.value && s.description == tempConstValue.description)
                //        { itemRepository.UpdateConstValueType(obj); 
                            
                //        }
                //    }
                //    string a = "";
                //}
                //else
                //{
                //    itemRepository.InsertConstValueType(obj);
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除一条ConstValue的记录数据
        /// </summary>
        /// <param name="obj">ConstValueInfo结构</param>
        public void RemoveConstValueType(int id)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                itemRepository.RemoveConstValueType(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveConstValueType(IList<string> ids)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                IUnitOfWork uow = new UnitOfWork();
                foreach (string id in ids)
                {
                    ConstValueTypeInfo item = new ConstValueTypeInfo();
                    item.id = int.Parse(id);
                    itemRepository.DeleteDataDefered<IMES.Infrastructure.Repository._Metas.ConstValueType, ConstValueTypeInfo>(uow, item);
                }
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除多条ConstValue的记录数据
        /// </summary>
        /// <param name="obj">ConstValueInfo结构</param>
        public void RemoveMultiConstValueType(string type, string value)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                itemRepository.RemoveMultiConstValueType(type, value);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 新增多筆ConstValue的记录数据
        /// </summary>
        /// <param name="obj">增加多筆ConstValueInfo结构</param>

        public void InsertMultiConstValueType(string type, IList<string> values, string descr, string editor)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                itemRepository.InsertMultiConstValueType(type, values, descr, editor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Type&Value取得对应的ConstValue数据的list(按Name栏位排序)
        /// </summary>
        /// <param name="type">过滤条件Type</param>
        /// <param name="value">过滤条件Value</param>
        /// <returns></returns>
        public IList<ConstValueTypeInfo> GetConstValueTypeList(string type,string value)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();

                IList<ConstValueTypeInfo> retLst = itemRepository.GetConstValueTypeList(type,value);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion



        public static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }

            return _input.ToString().Trim();
        }
    }
}