using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;

namespace IMES.Maintain.Implementation
{
    public class BomDescrMaintainImpl : MarshalByRefObject, IBomDescrMaintain       
    {
        public const String MACRANGE_STATUS_R = "R";
        public const String MACRANGE_STATUS_R_TEXT = "Created";
        public const String MACRANGE_STATUS_A = "A";
        public const String MACRANGE_STATUS_A_TEXT = "Active";
        public const String MACRANGE_STATUS_C = "C";
        public const String MACRANGE_STATUS_C_TEXT = "Closed";

        #region Implementation of IBomDescrMaintain

        /// <summary>
        /// 根据Type取得对应的Bom Description数据的list(按Code栏位排序)
        /// </summary>
        /// <param name="type">过滤条件Type</param>
        /// <returns></returns>
        public IList<BomDescrDef> GetBomDescrList(String type)
        {
            List<BomDescrDef> retLst = new List<BomDescrDef>();
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IList<DescTypeInfo> getData = itemRepository.GetBOMDescrsByTp(type);

                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        DescTypeInfo data = getData[i];
                        BomDescrDef item = new BomDescrDef();
                        item.ID = data.id.ToString();
                        item.Code = data.code;
                        item.Descr = data.description;
                        item.Type = data.tp;
                        item.Editor = data.editor;
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
        /// 保存一条Bom Description的记录数据(Add), 若Code与存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">BomDescrDef结构</param>
        public void AddBomDescr(BomDescrDef obj)
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

                //Code与存在记录的Code的名称相同，则提示业务异常
                IList<DescTypeInfo> exists = itemRepository.GetBOMDescrsByCode(obj.Code);
                if (exists != null && exists.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT089", erpara);
                    throw ex;

                }

                DescTypeInfo newItem = new DescTypeInfo();
                newItem.code = obj.Code;
                newItem.editor = obj.Editor;
                newItem.tp = obj.Type;
                newItem.description = obj.Descr;
                itemRepository.InsertBOMDescr(newItem);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 保存一条Bom Description的记录数据(update), 若Code与存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">更新BomDescrDef结构</param>
        /// <param name="oldCode">修改前Code</param>
        public void UpdateBomDescr(BomDescrDef obj, String oldCode)
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

                //Code与存在记录的Code的名称相同，则提示业务异常
                IList<DescTypeInfo> exists = itemRepository.GetBOMDescrsByCode(obj.Code);
                if (exists != null && exists.Count > 0 && oldCode != obj.Code)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT089", erpara);
                    throw ex;

                }

                DescTypeInfo itemOld = itemRepository.FindBOMDescrById(Int32.Parse(obj.ID));
                if (itemOld == null)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT082", erpara);
                    throw ex;
                }

                DescTypeInfo newItem = new DescTypeInfo();
                newItem.code = obj.Code;
                newItem.id = Int32.Parse(obj.ID);
                newItem.editor = obj.Editor;
                newItem.tp = obj.Type;
                newItem.description = obj.Descr;
                newItem.cdt = DateTime.Now; //Convert.ToDateTime(obj.Cdt);
                itemRepository.UpdateBOMDescrById(newItem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除一条Bom Description的记录数据
        /// </summary>
        /// <param name="obj">BomDescrDef结构</param>
        public void DeleteBomDescr(BomDescrDef obj)
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                itemRepository.DeleteBOMDescrById(Int32.Parse(obj.ID));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得DescType数据表中的所有Tp记录，按字符序排列
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllDescrType()
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IList<string> reLst = itemRepository.GetTPsFromDescType();
                return reLst;
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
