﻿/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for Const Value Maintain Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1              
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-8-6     Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/


using System;
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
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Misc;

namespace IMES.Maintain.Implementation
{
    public class ConstValueMaintain:MarshalByRefObject,IConstValueMaintain       
    {
        /*
        public const String MACRANGE_STATUS_R = "R";
        public const String MACRANGE_STATUS_R_TEXT = "Created";
        public const String MACRANGE_STATUS_A = "A";
        public const String MACRANGE_STATUS_A_TEXT = "Active";
        public const String MACRANGE_STATUS_C = "C";
        public const String MACRANGE_STATUS_C_TEXT = "Closed";
        */

        #region Implementation of IConstValueMaintain


        /// <summary>
        /// 取得所有ConstValue数据的list(按Type栏位排序)
        /// </summary>
        /// <returns></returns>
        public IList<ConstValueInfo> GetConstValueTypeList()
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                ConstValueInfo temp = new ConstValueInfo();
                temp.type = "SYS";
                ConstValueInfo temp2 = new ConstValueInfo();
                temp2.name = "";
                IList<ConstValueInfo> retLst = itemRepository.GetConstValueListByType(temp, temp2);

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
        public IList<ConstValueInfo> GetConstValueListByType(String type)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                //2012-8-13
                //IList<ConstValueInfo> retLst = itemRepository.GetConstValueListByType(type);
                ConstValueInfo temp = new ConstValueInfo();
                temp.type = type;
                ConstValueInfo temp2 = new ConstValueInfo();
                temp2.name = "";
                IList<ConstValueInfo> retLst = itemRepository.GetConstValueListByType(temp, temp2);

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
        public void AddConstValue(ConstValueInfo obj)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();

                itemRepository.AddConstValue(obj);
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
        public void SaveConstValue(ConstValueInfo obj)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();

                ConstValueInfo tempConstValue = new ConstValueInfo();
                tempConstValue.name = obj.name;
                tempConstValue.type = obj.type;
                IList<ConstValueInfo> exists = itemRepository.GetConstValueInfoList(tempConstValue);
                if (exists != null && exists.Count > 0)
                {
                    //若Name+Type数据已出现在ConstValue表的记录中，则更新到ConstValue表中
                    itemRepository.UpdateConstValue(obj, tempConstValue);
                }
                else
                {
                    //若Name+Type数据在ConstValue表的记录不存在，则Insert
                    itemRepository.AddConstValue(obj);
                }
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
        public void DeleteConstValue(ConstValueInfo obj)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                itemRepository.RemoveConstValue(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteConstValue(IList<string> ids)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                IUnitOfWork uow = new UnitOfWork();
                foreach (string id in ids)
                {
                    ConstValueInfo item = new ConstValueInfo();
                    item.id = int.Parse(id);
                    itemRepository.DeleteDataDefered<IMES.Infrastructure.Repository._Metas.ConstValue, ConstValueInfo>(uow, item);
                }
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        public void DeleteConstValueByCondition(ConstValueInfo obj)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                itemRepository.RemoveConstValue(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 取得Type数据的Descrip(按Type栏位排序)
        /// </summary>
        /// <returns></returns>
        public string GetConstValueDescriptionByType(string Type)
        {
            try
            {
                string ret = "";
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                ConstValueInfo temp = new ConstValueInfo();
                temp.type = "SYS";
                temp.value = Type;
                ConstValueInfo temp2 = new ConstValueInfo();
                temp2.name = "";
                IList<ConstValueInfo> retLst = itemRepository.GetConstValueListByType(temp, temp2);
                if (retLst == null || retLst.Count == 0)
                {
                    ret = "暫無資料...";
                }
                else
                {
                    ret = (retLst[0].description.ToString().Trim() != "") ? retLst[0].description.ToString().Trim() : "暫無資料...";
                }

                return ret;
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