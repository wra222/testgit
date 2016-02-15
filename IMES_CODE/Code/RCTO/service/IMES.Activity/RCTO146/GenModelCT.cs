// INVENTEC corporation (c)2012 all rights reserved. 
// Description: AcquireMBCT
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-16   Yuan XiaoWei                 create
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0424
// 2012-02-15   Yuan XiaoWei                 ITC-1360-0425
// 2012-02-15   Yuan XiaoWei                 ITC-1360-0426
// Known issues:

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.Extend;
using System.ComponentModel;
using IMES.FisObject.Common.Misc;
using IMES.Common;


namespace IMES.Activity
{
    /// <summary>
    /// Generate Model CT
    /// </summary>
    public partial class GenModelCT : BaseActivity
    {
        /// <summary>
        /// Generate Model CT
        /// </summary>
        public GenModelCT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IProduct prod = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            bool hasModelCTFlag = prod.ModelObj.Attributes.Any(x => x.Name == "ModelCT" && x.Value == "T");
            if (!hasModelCTFlag)
            {
                IList<ConstValueTypeInfo> constValueTypeList = null;
                bool hasFamilyFlag = utl.TryConstValueTypeWithRE("AllowGenModelCTByFamily", prod.Family, out constValueTypeList);
                if (!hasFamilyFlag)
                {
                    return base.DoExecute(executionContext);
                }
            }

            if (prod.ProductInfoes.Where(x => x.InfoType == this.StoreInfoType && !string.IsNullOrEmpty(x.InfoValue)).Any())
            {
                return base.DoExecute(executionContext);
            }

            string aseemblyCode = null;
            string revision = null;
            string supplier = null;
            IList<string> supplierList = new List<string>();
            if (hasModelCTFlag)
            {
                getModeAssemblyCode(prod, out aseemblyCode, out revision,out supplierList);
            }
            else
            {
                getModelInfoAssemblyCode(prod, out aseemblyCode, out revision, out  supplierList);
            }

            supplier = supplierList[0];

            string thisYear = "";
            string weekCode = "";
            IModelRepository CurrentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

            IList<HpweekcodeInfo> weekCodeList = CurrentModelRepository.GetHPWeekCodeInRangeOfDescr();
            if (weekCodeList != null && weekCodeList.Count > 0)
            {
                weekCode = weekCodeList[0].code;
                thisYear = weekCodeList[0].descr.Trim().Substring(0, 4);
            }
            else
            {
                throw new FisException("ICT009", new string[] { });
            }
            string noType = "ModelCT";

            string prefixCode = thisYear + aseemblyCode + revision + supplier + weekCode;
            string numCodeChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            IList<string> seq = utl.GenSN.GetNextCTSequenceWithCharString(this.Customer, noType, prefixCode, supplierList, "000", "ZZZ", numCodeChar);

            string ct = aseemblyCode + revision + seq[0] + weekCode + seq[1];

            session.AddValue("ModelCT", ct);

            prod.SetExtendedProperty(this.StoreInfoType, ct, this.Editor);

            if (string.IsNullOrEmpty(prod.CUSTSN) && this.IsStoreInCUSTSN)
            {
                prod.CUSTSN = ct;
            }

            IProductRepository ProdRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            ProdRep.Update(prod, session.UnitOfWork);

            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  必须要生成号保存到ProductInfoType
        /// </summary>
        public static DependencyProperty StoreInfoTypeProperty = DependencyProperty.Register("StoreInfoType", typeof(string), typeof(GenModelCT), new PropertyMetadata("SleeveCT"));

        /// <summary>
        ///  必须要生成号保存到ProductInfoType
        /// </summary>
        [DescriptionAttribute("StoreInfoType")]
        [CategoryAttribute(" StoreInfoType Arguments")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string StoreInfoType
        {
            get
            {
                return ((string)(base.GetValue(StoreInfoTypeProperty)));
            }
            set
            {
                base.SetValue(StoreInfoTypeProperty, value);
            }
        }


        /// <summary>
        ///  必须要生成号保存到Product.CUSTSN
        /// </summary>
        public static DependencyProperty IsStoreInCUSTSNProperty = DependencyProperty.Register("IsStoreInCUSTSN", typeof(bool), typeof(GenModelCT), new PropertyMetadata(false));

        /// <summary>
        ///  必须要生成号保存到Product.CUSTSN
        /// </summary>
        [DescriptionAttribute("IsStoreInCUSTSN")]
        [CategoryAttribute(" IsStoreInCUSTSN Arguments")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStoreInCUSTSN
        {
            get
            {
                return ((bool)(base.GetValue(IsStoreInCUSTSNProperty)));
            }
            set
            {
                base.SetValue(IsStoreInCUSTSNProperty, value);
            }
        }


        private void getModelInfoAssemblyCode(IProduct product, out string aseemblyCode, out string revision, out IList<String> supplierList)
        {
            //AssemblyCode/Revision/Supplier 抓取值 from Model Info
            aseemblyCode = product.ModelObj.Attributes.Where(x => x.Name == "ASSEMBLYCODE").Select(y => y.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(aseemblyCode))
            {
                throw new FisException("PAK085", new string[] { product.Model, "ASSEMBLYCODE" });
            }

            revision = product.ModelObj.Attributes.Where(x => x.Name == "REV").Select(y => y.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(revision))
            {
                throw new FisException("PAK085", new string[] { product.Model, "REV" });
            }

            string supplier = product.ModelObj.Attributes.Where(x => x.Name == "SUPPLIER").Select(y => y.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(supplier))
            {
                throw new FisException("PAK085", new string[] { product.Model, "SUPPLIER" });
            }

            supplierList = supplier.Split(new char[] { ',', '~' }).ToList();

        }

        private void getModeAssemblyCode(IProduct product, out string aseemblyCode, out string revision, out IList<String> supplierList)
        {
            IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            IList<ModelAssemblyCodeInfo> modelAssemblyInfoList = miscRep.GetData<IMES.Infrastructure.Repository._Metas.ModelAssemblyCode, ModelAssemblyCodeInfo>(new ModelAssemblyCodeInfo
            {
                Model = product.Model
            });

            if (modelAssemblyInfoList == null || modelAssemblyInfoList.Count == 0)
            {
                throw new FisException("CQCHK1110", new string[] { product.Model, "" });
            }

            //AssemblyCode/Revision/Supplier 抓取值 from Model Info
            aseemblyCode = modelAssemblyInfoList[0].AssemblyCode;
            if (string.IsNullOrEmpty(aseemblyCode))
            {
                throw new FisException("CQCHK1110", new string[] { product.Model, "AssemblyCode" });
            }

            revision = modelAssemblyInfoList[0].Revision;
            if (string.IsNullOrEmpty(revision))
            {
                throw new FisException("CQCHK1110", new string[] { product.Model, "Revision" });
            }

            string supplier = modelAssemblyInfoList[0].SupplierCode;
            if (string.IsNullOrEmpty(supplier))
            {
                throw new FisException("CQCHK1110", new string[] { product.Model, "SupplierCode" });
            }

            supplierList = supplier.Split(new char[] { ',', '~' }).ToList();

        }

    }
}
