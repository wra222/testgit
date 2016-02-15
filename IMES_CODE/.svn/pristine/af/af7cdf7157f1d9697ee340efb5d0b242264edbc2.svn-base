using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
namespace IMES.Maintain.Implementation
{
    public class VendorCodeImpl:MarshalByRefObject,IVendorCode
    {
        public const String MACRANGE_STATUS_R = "R";
        public const String MACRANGE_STATUS_R_TEXT = "Created";
        public const String MACRANGE_STATUS_A = "A";
        public const String MACRANGE_STATUS_A_TEXT = "Active";
        public const String MACRANGE_STATUS_C = "C";
        public const String MACRANGE_STATUS_C_TEXT = "Closed";

        #region Implementation of IBattery
        /// <summary>
        /// 取得IqcPnoBom表中的Vendor值
        /// </summary>
        /// <returns></returns>
        public IList<string> GetVendorFromIqcPnoBom()
        {
            List<string> retLst = new List<string>();

            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                retLst.Add("AST");//AST 固定值
                retLst.Add("MB");//MB 固定值
                //IList<string> getData = itemRepository.GetVendorsByLikeDescr("MEM");
                IList<string> getData = partRep.GetConstValueTypeList("VendorList").Select(x => x.value).ToList();

                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                       retLst.Add(getData[i]);
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
        /// 取得VendorCode数据列表(按Vendor、Priority栏位排序)
        /// </summary>
        /// <returns></returns>
        public IList<VendorCodeDef> GetAllVendorCodeList()
        {
            List<VendorCodeDef> retLst = new List<VendorCodeDef>();

            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                IList<SupplierCodeInfo> getData = itemRepository.GetAllSupplierCodeList();

                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        VendorCodeDef item = convert(getData[i]);
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

        private VendorCodeDef convert(SupplierCodeInfo data)
        {
            VendorCodeDef item = new VendorCodeDef();
            item.ID = data.id.ToString();
            item.Vendor = data.vendor;
            item.Idex = data.idex;
            item.VendorCode = data.code;
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
            return item;
        }

        /// <summary>
        /// 保存一条Vendor Code的记录数据(Add)
        /// </summary>
        /// <param name="obj">VendorCodeDef结构</param>
        public void AddVendorCode(VendorCodeDef obj, string SelVender)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                //Modify 2012/08/01,UC Update
                //若Vendor<>’AST’,若Vendor Code框中内容已经存在于SupplierCode数据表的Code栏位中，则提示用户“此Vendor Code 已經存在！”，放弃后续操作。
                IList<SupplierCodeInfo> exists =new List<SupplierCodeInfo>();
                if (SelVender != "AST")
                {
                    //若Vendor Code框中内容已经存在于SupplierCode数据表的Code栏位中，则提示用户“此Vendor Code 已經存在！”
                    exists = itemRepository.GetSupplierCodeListByCode(obj.VendorCode);
                    if (exists != null && exists.Count > 0)
                    {
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT100", erpara);
                        throw ex;

                    }
                }
                else
                {
                    //Modify 2012/08/01,UC Update
                    //若Vendor=‘AST’，则检查PartNo是否在SupplierCode数据表的Code栏位，若存在，则提示用户“此PartNo已经存在”，放弃后续操作 
                    exists = itemRepository.GetSupplierCodeListByCode(obj.Vendor);
                    if (exists != null && exists.Count > 0)
                    {
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT097", erpara);
                        throw ex;
                    }
                }

                //若Vendor和Priority框中内容已经同时作为某个SupplierCode记录的Vendor和Indx，则提示用户“此Vendor Code的優先級重復！”
                exists = itemRepository.GetSupplierCodeListByCode(obj.Vendor, obj.Idex);
                if (exists != null && exists.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT101", erpara);
                    throw ex;

                }

                SupplierCodeInfo item = new SupplierCodeInfo();
                item.vendor = obj.Vendor;
                item.code = obj.VendorCode;
                item.idex = obj.Idex;
                item.editor = obj.Editor;
                item.cdt = DateTime.Now;
                item.udt = DateTime.Now;
                itemRepository.AddSupplierCodeInfo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除一条Vendor Code的记录数据
        /// </summary>
        /// <param name="obj">VendorCodeDef结构</param>
        public void DeleteVendorCode(VendorCodeDef obj)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                //SupplierCodeInfo item = itemRepository.FindSupplierCodeInfo(Int32.Parse(obj.ID));
                //if (item != null)
                //{
                    itemRepository.RemoveSupplierCodeInfo(Int32.Parse(obj.ID));
                //}
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
