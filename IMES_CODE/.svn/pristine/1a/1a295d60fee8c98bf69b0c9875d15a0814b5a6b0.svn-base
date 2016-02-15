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
    public class KittingCodeMaintainImpl:MarshalByRefObject,IKittingCodeMaintain       
    {
        public const String MACRANGE_STATUS_R = "R";
        public const String MACRANGE_STATUS_R_TEXT = "Created";
        public const String MACRANGE_STATUS_A = "A";
        public const String MACRANGE_STATUS_A_TEXT = "Active";
        public const String MACRANGE_STATUS_C = "C";
        public const String MACRANGE_STATUS_C_TEXT = "Closed";

        #region Implementation of IKittingCodeMaintain

        /// <summary>
        /// 根据Type取得对应的Kitting Code数据的list(按Code栏位排序)
        /// </summary>
        /// <param name="type">过滤条件Type</param>
        /// <returns></returns>
        public IList<KittingCodeDef> GetKittingCodeList(string type){
            List<KittingCodeDef> retLst = new List<KittingCodeDef>();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                IList<LabelKittingCode> getData = itemRepository.GetLabelKittingCodeListByType(type);

                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        LabelKittingCode data = getData[i];
                        KittingCodeDef item = new KittingCodeDef();
                        item.Code = data.code;
                        item.Descr = data.descr;
                        item.Type = data.type;
                        item.Remark = data.remark;
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
        /// 保存一条kitting Code的记录数据(Add), 若Code与相同Type存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">KittingCodeDef结构</param>
        public void AddKittingCode(KittingCodeDef obj)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                //Code与相同Type存在记录的Code的名称相同，则提示业务异常
                DataTable exists = itemRepository.GetExistLabelKittingCode(obj.Code, obj.Type);
                if (exists != null && exists.Rows.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT081", erpara);
                    throw ex;

                }

                LabelKittingCode newItem = new LabelKittingCode();
                newItem.code = obj.Code;
                newItem.editor = obj.Editor;
                newItem.descr = obj.Descr;
                newItem.remark = obj.Remark;
                newItem.type = obj.Type;
                itemRepository.AddLabelKittingCode(newItem);  
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 保存一条kitting Code的记录数据(update), 若Code与相同Type存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">更新KittingCodeDef结构</param>
        /// <param name="oldCode">修改前Code</param>
        public void UpdateKittingCode(KittingCodeDef obj, string oldCode) {

            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                //Code与相同Type存在记录的Code的名称相同，则提示业务异常
                DataTable exists = itemRepository.GetExistLabelKittingCode(obj.Code, obj.Type);
                if (exists != null && exists.Rows.Count > 0 && oldCode != obj.Code)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT081", erpara);
                    throw ex;

                }

                LabelKittingCode itemOld = itemRepository.FindLabelKittingCode(oldCode, obj.Type);
                if (itemOld == null)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT082", erpara);
                    throw ex;
                }

                LabelKittingCode newItem = new LabelKittingCode();
                newItem.code = obj.Code;
                newItem.editor = obj.Editor;
                newItem.descr = obj.Descr;
                newItem.remark = obj.Remark;
                newItem.type = obj.Type;
                newItem.cdt = DateTime.Now; //Convert.ToDateTime(obj.Cdt);
                itemRepository.ChangeLabelKittingCode(newItem, oldCode, obj.Type);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 删除一条Kitting Code的记录数据
        /// </summary>
        /// <param name="obj">KittingCodeDef结构</param>
        public void DeleteKittingCode(KittingCodeDef obj) {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.RemoveLabelKittingCode(obj.Code, obj.Type);
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