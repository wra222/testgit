/*
 *Issue
 *
 *ITC-1361-0044  itc210012  2011-01-16
 *ITC-1361-0043  itc210012  2011-01-16
 * 
 */
using System;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.FA.Product;

namespace IMES.Maintain.Implementation
{
    public class InternalCOAImpl : MarshalByRefObject, IInternalCOA       
    {
        public const String MACRANGE_STATUS_R = "R";
        public const String MACRANGE_STATUS_R_TEXT = "Created";
        public const String MACRANGE_STATUS_A = "A";
        public const String MACRANGE_STATUS_A_TEXT = "Active";
        public const String MACRANGE_STATUS_C = "C";
        public const String MACRANGE_STATUS_C_TEXT = "Closed";

        #region Implementation of InternalCOA

        /// <summary>
        /// 取得所有InternalCOA数据的list(按Type和Code排序)
        /// </summary>
        /// <returns>返回InternalCOADef列表</returns>
        public IList<InternalCOADef> GetAllInternalCOAInfoList()
        {
            List<InternalCOADef> retLst = new List<InternalCOADef>();

            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IList<InternalCOAInfo> getData = itemRepository.FindAllInternalCOA();

                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        InternalCOADef item = convert(getData[i]);
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



        private InternalCOADef convert(InternalCOAInfo data)
        {
            InternalCOADef item = new InternalCOADef();
            item.id = data.id.ToString();
            item.code = data.code;
            item.type = data.type;
            item.model = data.model;
            item.editor = data.editor;

            if (data.cdt == DateTime.MinValue)
            {
                item.cdt = "";
            }
            else
            {
                item.cdt = ((System.DateTime)data.cdt).ToString("yyyy-MM-dd HH:mm:ss");
            }
            return item;
        }

        /// <summary>
        /// 保存一条InternalCOA的记录数据(Add)
        /// </summary>
        /// <param name="obj">InternalCOADef结构</param>
        public void AddInternalCOA(InternalCOADef obj)
        {
            try
            {
                string code = obj.code;
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                
                if (code.Length != 10 && code.Length != 16)//若Code框中内容长度既不等于16也不等于10，则警示用户“不存在的Code”
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT065", erpara);//不存在的Code
                    throw ex;
                }
                else
                {
                    //若Code框中内容已经出现在Internal COA List表格的Code栏位中，则警示用户“此Code 已存在”
                    IList<InternalCOAInfo> exists  = itemRepository.GetExistInternalCOA(obj.code);
                    if (exists != null && exists.Count > 0)
                    {
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT066", erpara);//此Code已存在
                        throw ex;

                    }
                    String model = "";
                    String type = "";


                    if (code != null && code.Length == 16)//若Code框中内容长度=16，则检查该Code是否是一个已存在的DN。若否，则警示用户“不存在的Code”，放弃后续操作。反之，则取得该DN对应的Model，创建此新InternalCOA记录，记录创建人和日期，Type=“DN”。
                    {
                        IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
                        Delivery delivery = iDeliveryRepository.Find(code);
                        if (delivery != null){
                            model = delivery.ModelName;
                            type = "DN";
                        }else{
                            List<string> erpara = new List<string>();
                            FisException ex;
                            ex = new FisException("DMT067", erpara);//不存在的Code
                            throw ex;
                        }
                    }
                    else if (code != null && code.Length == 10)//若Code框中内容长度=10，则检查该Code是否是一个已存在的Product SN。若否，则警示用户“不存在的Code”，放弃后续操作。反之，则取得该SN对应的Model，创建此新InternalCOA记录，记录创建人和日期，Type=“SN”。
                    {
                        IProductRepository iProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                        //IProduct product = iProductRepository.Find(code);
                        //issuecode 
                        //ITC-1361-0044 itc210012 2012-2-16
                        IProduct product = iProductRepository.GetProductByCustomSn(code);
                        if (product != null){
                            model = product.Model;
                            type = "SN";
                        }else{
                            List<string> erpara = new List<string>();
                            FisException ex;
                            ex = new FisException("DMT067", erpara);//不存在的Code
                            throw ex;
                        }
                    }
                    InternalCOAInfo item = new InternalCOAInfo();
                    item.code = code;
                    item.type = type;
                    item.model = model;
                    item.editor = obj.editor;
                    item.cdt = DateTime.Now;
                    itemRepository.AddInternalCOA(item);
                }

              
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除一条InternalCOA的记录数据
        /// </summary>
        /// <param name="obj">InternalCOADef结构</param>
        public void DeleteInternalCOA(InternalCOADef obj)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                InternalCOAInfo item = itemRepository.FindInternalCOA(Int32.Parse(obj.id));
                if (item != null)
                {
                    itemRepository.RemoveInternalCOA(item);
                }
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
