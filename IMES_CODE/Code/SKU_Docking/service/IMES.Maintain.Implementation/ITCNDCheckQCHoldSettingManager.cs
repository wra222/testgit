/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for ITCND Check QC Hold Setting Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/5/10 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/5/10              
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-4   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0020
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

namespace IMES.Maintain.Implementation
{
    public class ITCNDCheckQCHoldSettingManager : MarshalByRefObject, IITCNDCheckQCHoldSetting       
    {
        /*
        public const String MACRANGE_STATUS_R = "R";
        public const String MACRANGE_STATUS_R_TEXT = "Created";
        public const String MACRANGE_STATUS_A = "A";
        public const String MACRANGE_STATUS_A_TEXT = "Active";
        public const String MACRANGE_STATUS_C = "C";
        public const String MACRANGE_STATUS_C_TEXT = "Closed";
        */

        #region Implementation of IITCNDCheckQCHoldSetting

        /* 分UI列表结构ITCNDCheckQCHoldDef和数据库结构ITCNDCheckQCHold，现在看来两个可以合为一个结构，重构
        /// <summary>
        /// 取得所有ITCNDCheckQCHold记录(按Code栏位排序)
        /// </summary>
        /// <returns>ITCNDCheckQCHold记录列表</returns>
        public IList<ITCNDCheckQCHoldDef> GetITCNDCheckQCHoldList()
        {
            List<ITCNDCheckQCHoldDef> retLst = new List<ITCNDCheckQCHoldDef>();

            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                IList<ITCNDCheckQCHold> getData = itemRepository.GetITCNDCheckQCHoldList();

                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        ITCNDCheckQCHold data = getData[i];
                        ITCNDCheckQCHoldDef item = new ITCNDCheckQCHoldDef();
                        item.Code = data.code;
                        item.Descr = data.descr;
                        item.Editor = data.editor;
                        item.isHold = data.ishold;

                        if (data.cdt == DateTime.MinValue)
                        {
                            item.Cdt = "";
                        }
                        else
                        {
                            item.Cdt = ((System.DateTime)data.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        if (data.udt == DateTime.MinValue)
                        {
                            item.Udt = "";
                        }
                        else
                        {
                            item.Udt = ((System.DateTime)data.udt).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        retLst.Add(item);
                    }
                }

                return retLst;
            }
            catch (Exception)
            {
                throw;
            }           
            
        }

        /// <summary>
        /// 保存一条ITCNDCheckQCHold的记录数据(Add), 若Code与存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">ITCNDCheckQCHoldDef结构</param>
        public void AddITCNDCheckQCHold(ITCNDCheckQCHoldDef obj)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                //Code与存在记录的Code的名称相同，则提示业务异常
                DataTable exists = itemRepository.GetExistITCNDCheckQCHold(obj.Code);
                if (exists != null && exists.Rows.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT066", erpara);
                    throw ex;

                }

                ITCNDCheckQCHold newItem = new ITCNDCheckQCHold();
                newItem.code = obj.Code;
                newItem.editor = obj.Editor;
                newItem.descr = obj.Descr;
                newItem.ishold = obj.isHold;

                itemRepository.AddITCNDCheckQCHold(newItem);  
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 删除一条ITCNDCheckQCHold的记录数据
        /// </summary>
        /// <param name="obj">ITCNDCheckQCHoldDef结构</param>
        public void DeleteITCNDCheckQCHold(ITCNDCheckQCHoldDef obj)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.RemoveITCNDCheckQCHold(obj.Code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 更新一条ITCNDCheckQCHold的记录数据(update), 若记录中不存在与传入Code相同名称的Code，则提示业务异常
        /// </summary>
        /// <param name="obj">更新ITCNDCheckQCHoldDef结构</param>
        public void UpdateITCNDCheckQCHold(ITCNDCheckQCHoldDef obj)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                //若记录中不存在与传入Code相同名称的Code，则提示业务异常
                DataTable exists = itemRepository.GetExistITCNDCheckQCHold(obj.Code);
                if (exists == null || exists.Rows.Count <= 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT067", erpara);
                    throw ex;

                }

                ITCNDCheckQCHold newItem = new ITCNDCheckQCHold();
                newItem.code = obj.Code;
                newItem.editor = obj.Editor;
                newItem.descr = obj.Descr;
                newItem.ishold = obj.isHold;
                newItem.cdt = Convert.ToDateTime(obj.Cdt);
                newItem.udt = DateTime.Now; 

                itemRepository.ChangeITCNDCheckQCHold(newItem);
            }
            catch (Exception)
            {
                throw;
            }

        }
        */


        /// <summary>
        /// 取得所有ITCNDCheckQCHold记录(按Code栏位排序)
        /// </summary>
        /// <returns>ITCNDCheckQCHold记录列表</returns>
        public IList<ITCNDCheckQCHoldDef> GetITCNDCheckQCHoldList()
        {
            List<ITCNDCheckQCHoldDef> retLst = new List<ITCNDCheckQCHoldDef>();

            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                IList<ITCNDCheckQCHoldDef> getData = itemRepository.GetITCNDCheckQCHoldList();

                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        ITCNDCheckQCHoldDef data = getData[i];
                        ITCNDCheckQCHoldDef item = new ITCNDCheckQCHoldDef();
                        item.Code = data.Code;
                        item.Descr = data.Descr;
                        item.Editor = data.Editor;
                        item.isHold = data.isHold;

                        /* 2012-5-14
                        if (data.Cdt == DateTime.MinValue)
                        {
                            item.Cdt = "";
                        }
                        else
                        {
                            item.Cdt = ((System.DateTime)data.Cdt).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        if (data.Udt == DateTime.MinValue)
                        {
                            item.Udt = "";
                        }
                        else
                        {
                            item.Udt = ((System.DateTime)data.Udt).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        */
                        item.Cdt = (System.DateTime)data.Cdt;
                        item.Udt = (System.DateTime)data.Udt;

                        retLst.Add(item);
                    }
                }

                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 取得15天前ITCNDCheckQCHold记录(按Code栏位排序)
        /// </summary>
        /// <returns>ITCNDCheckQCHold记录列表(15天)</returns>
        public IList<ITCNDCheckQCHoldDef> GetITCNDCheckQCHoldListByDay()
        {
            List<ITCNDCheckQCHoldDef> retLst = new List<ITCNDCheckQCHoldDef>();

            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                IList<ITCNDCheckQCHoldDef> getData = itemRepository.GetITCNDCheckQCHoldList();

                if (getData != null)
                {
                    IList<ITCNDCheckQCHoldDef> ListByDay = (from q in getData
                                                            where q.Udt > DateTime.Now.AddDays(-15)
                                                            select q).ToList<ITCNDCheckQCHoldDef>();



                    for (int i = 0; i < ListByDay.Count; i++)
                    {
                        ITCNDCheckQCHoldDef data = ListByDay[i];
                        ITCNDCheckQCHoldDef item = new ITCNDCheckQCHoldDef();
                        item.Code = data.Code;
                        item.Descr = data.Descr;
                        item.Editor = data.Editor;
                        item.isHold = data.isHold;

                        /* 2012-5-14
                        if (data.Cdt == DateTime.MinValue)
                        {
                            item.Cdt = "";
                        }
                        else
                        {
                            item.Cdt = ((System.DateTime)data.Cdt).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        if (data.Udt == DateTime.MinValue)
                        {
                            item.Udt = "";
                        }
                        else
                        {
                            item.Udt = ((System.DateTime)data.Udt).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        */
                        item.Cdt = (System.DateTime)data.Cdt;
                        item.Udt = (System.DateTime)data.Udt;

                        retLst.Add(item);
                    }
                }

                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 保存一条ITCNDCheckQCHold的记录数据(Add), 若Code与存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">ITCNDCheckQCHoldDef结构</param>
        public void AddITCNDCheckQCHold(ITCNDCheckQCHoldDef obj)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                //Code与存在记录的Code的名称相同，则提示业务异常
                ITCNDCheckQCHoldDef newObj = new ITCNDCheckQCHoldDef();
                newObj.Code = obj.Code;
                IList<ITCNDCheckQCHoldDef> exists = itemRepository.GetExistITCNDCheckQCHold(newObj);
                if (exists != null && exists.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT066", erpara);
                    throw ex;

                }

                ITCNDCheckQCHoldDef newItem = new ITCNDCheckQCHoldDef();
                newItem.Code = obj.Code;
                newItem.Editor = obj.Editor;
                newItem.Descr = obj.Descr;
                newItem.isHold = obj.isHold;

                itemRepository.AddITCNDCheckQCHold(newItem);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 删除一条ITCNDCheckQCHold的记录数据
        /// </summary>
        /// <param name="obj">ITCNDCheckQCHoldDef结构</param>
        public void DeleteITCNDCheckQCHold(ITCNDCheckQCHoldDef obj)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.RemoveITCNDCheckQCHold(obj);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 更新一条ITCNDCheckQCHold的记录数据(update), 若新Code与存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">更新ITCNDCheckQCHoldDef结构</param>
        /// <param name="oldCode">修改前Code</param>
        public void UpdateITCNDCheckQCHold(ITCNDCheckQCHoldDef obj, string oldCode)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                //新Code与存在记录的Code的名称相同，则提示业务异常
                ITCNDCheckQCHoldDef tempNewInfo = new ITCNDCheckQCHoldDef();
                tempNewInfo.Code = obj.Code;
                IList<ITCNDCheckQCHoldDef> exists = itemRepository.GetExistITCNDCheckQCHold(tempNewInfo);
                if (exists != null && exists.Count > 0 && oldCode != obj.Code)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT066", erpara);
                    throw ex;

                }

                ITCNDCheckQCHoldDef tempInfo = new ITCNDCheckQCHoldDef();
                tempInfo.Code = oldCode;
                IList<ITCNDCheckQCHoldDef> oldexists = itemRepository.GetExistITCNDCheckQCHold(tempInfo);
                if (oldexists == null || oldexists.Count <= 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT082", erpara);
                    throw ex;

                }

                ITCNDCheckQCHoldDef newItem = new ITCNDCheckQCHoldDef();
                newItem.Code = obj.Code;
                newItem.Editor = obj.Editor;
                newItem.Descr = obj.Descr;
                newItem.isHold = obj.isHold;
                newItem.Cdt = Convert.ToDateTime(obj.Cdt);
                newItem.Udt = DateTime.Now;
                itemRepository.ChangeITCNDCheckQCHold(newItem, tempInfo);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateITCNDCheckQCHold(ITCNDCheckQCHoldDef obj)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                
                ITCNDCheckQCHoldDef tempInfo = new ITCNDCheckQCHoldDef();
                tempInfo.Code = obj.Code;
                ITCNDCheckQCHoldDef newItem = new ITCNDCheckQCHoldDef();
                newItem.Code = obj.Code;
                newItem.Editor = obj.Editor;
                newItem.Descr = obj.Descr;
                newItem.isHold = obj.isHold;
                newItem.Cdt = Convert.ToDateTime(obj.Cdt);
                newItem.Udt = DateTime.Now;
                itemRepository.ChangeITCNDCheckQCHold(newItem, tempInfo);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool CheckExistITCNDCheckQCHold(ITCNDCheckQCHoldDef obj)
        {
            try
            {

                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                ITCNDCheckQCHoldDef tempInfo = new ITCNDCheckQCHoldDef();
                tempInfo.Code = obj.Code;
                IList<ITCNDCheckQCHoldDef> exists = itemRepository.GetExistITCNDCheckQCHold(tempInfo);
                if (exists == null || exists.Count <= 0)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
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