// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Benson          create
//2013-03-13    Vincent           release
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.BSam;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.DN;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.FisBOM;
using IMES.DataModel;

namespace IMES.Activity
{


    /// <summary>
    /// Check the DeliveryNo is null or empty in Product List
    /// </summary>

    public partial class CheckDnAndCOAByProductList : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CheckDnAndCOAByProductList()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckDnAndCOAByProductList), new PropertyMetadata(true));
        /// <summary>
        ///  禁用時要停止workflow
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF")]
        [BrowsableAttribute(true)]       
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(CheckDnAndCOAByProductList.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckDnAndCOAByProductList.IsStopWFProperty, value);
            }
        }

        /// <summary>
        /// Is Check DN
        /// </summary>
        public static DependencyProperty IsCheckDNProperty = DependencyProperty.Register("IsCheckDN", typeof(bool), typeof(CheckDnAndCOAByProductList), new PropertyMetadata(true));
        /// <summary>
        ///   Is Check DN
        /// </summary>
        [DescriptionAttribute("IsCheckDN")]
        [CategoryAttribute("IsCheckDN")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsCheckDN
        {
            get
            {
                return ((bool)(base.GetValue(CheckDnAndCOAByProductList.IsCheckDNProperty)));
            }
            set
            {
                base.SetValue(CheckDnAndCOAByProductList.IsCheckDNProperty, value);
            }
        }


        /// <summary>
        /// Is Check COA
        /// </summary>
        public static DependencyProperty IsCheckCOAProperty = DependencyProperty.Register("IsCheckCOA", typeof(bool), typeof(CheckDnAndCOAByProductList), new PropertyMetadata(true));
        /// <summary>
        ///   Is Check COA
        /// </summary>
        [DescriptionAttribute("IsCheckCOA")]
        [CategoryAttribute("IsCheckCOA")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsCheckCOA
        {
            get
            {
                return ((bool)(base.GetValue(CheckDnAndCOAByProductList.IsCheckCOAProperty)));
            }
            set
            {
                base.SetValue(CheckDnAndCOAByProductList.IsCheckCOAProperty, value);
            }
        }


        /// <summary>
        /// Is Check Office COA
        /// </summary>
        public static DependencyProperty IsCheckOfficeCOAProperty = DependencyProperty.Register("IsCheckOfficeCOA", typeof(bool), typeof(CheckDnAndCOAByProductList), new PropertyMetadata(true));
        /// <summary>
        ///   Is Check Office COA
        /// </summary>
        [DescriptionAttribute("IsCheckOfficeCOA")]
        [CategoryAttribute("IsCheckOfficeCOA")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsCheckOfficeCOA
        {
            get
            {
                return ((bool)(base.GetValue(CheckDnAndCOAByProductList.IsCheckOfficeCOAProperty)));
            }
            set
            {
                base.SetValue(CheckDnAndCOAByProductList.IsCheckOfficeCOAProperty, value);
            }
        }
     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            string officeCoaPartNo="";
            string coaPartNo="";
            bool officeCoaFlag = false;
            bool coaFlag = false;
            string site = "IPC";
            //Get COA and Office COA Flag
            if (IsCheckCOA || IsCheckOfficeCOA)
            {
                IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                string partno = string.Empty;
                IHierarchicalBOM sessionBOM = null;
                sessionBOM = ibomRepository.GetHierarchicalBOMByModel(currentProduct.Model);
                IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
                bomNodeLst = sessionBOM.FirstLevelNodes;
                if (bomNodeLst == null || bomNodeLst.Count <= 0)
                {
                    erpara.Add(currentProduct.Model);
                    ex = new FisException("PAK039", erpara);
                    throw ex;
                }
                IList<SysSettingInfo> siteList = ipartRepository.GetSysSettingInfoes(new SysSettingInfo() { name = "Site" }); 
                if (siteList != null && siteList.Count > 0)
                {
                    site = siteList[0].value;
                }
                foreach (IBOMNode ibomnode in bomNodeLst)
                {
                    IPart bomPart = ibomnode.Part;
                    if (site == "ICC")
                    {
                        if (bomPart.BOMNodeType == "P1" && bomPart.Descr.ToUpper().Contains("WIN7"))
                        {
                            coaPartNo = bomPart.PN;
                            coaFlag = true;  // 存在
                            break;
                        }
                    }
                    else
                    {
                        if (bomPart.BOMNodeType == "P1" && bomPart.Descr.Contains("COA"))
                        {
                            coaPartNo = bomPart.PN;
                            coaFlag = true;  // 存在
                            break;
                        }
                    }
                }
                foreach (IBOMNode ibomnode in bomNodeLst)
                {
                    IPart bomPart = ibomnode.Part;
                    if (bomPart.BOMNodeType == "P1" && bomPart.Descr.Contains("OFFICE"))
                    {
                        officeCoaPartNo = bomPart.PN;
                        officeCoaFlag = true;  // 存在
                        break;
                    }
                }
              
            }
            IList<IProduct> prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
            if (prodList != null && prodList.Count > 0)
            {
                foreach (IProduct prodObj in prodList)
                {
                
                    if(IsCheckDN)
                    {
                         if (string.IsNullOrEmpty(prodObj.DeliveryNo))
                        {
                            FisException e = new FisException("PAK177", new string[] { prodObj.ProId });
                            e.stopWF = IsStopWF;
                            throw e;
                        }
                    }
                    if(IsCheckOfficeCOA && officeCoaFlag)
                    {
                      if (!CheckOfficeCOA(prodObj,officeCoaPartNo) )
                        {
                            FisException e = new FisException("PAK037", new string[] { prodObj.ProId });
                            e.stopWF = IsStopWF;
                            throw e;
                        }
                    
                    }
                    if (IsCheckCOA && coaFlag)
                    {
                        if (!CheckCOA(prodObj, coaPartNo))
                        {
                            FisException e = new FisException("PAK176", new string[] { prodObj.ProId });
                            e.stopWF = IsStopWF;
                            throw e;
                        }
                    }
                   
                
                }
                     
            
            }
        
            return base.DoExecute(executionContext);
          
        }

        private bool CheckOfficeCOA(IProduct prodObj,string partNo)
        {
            bool flag = false;
            if (prodObj.PizzaObj != null)
            {
                IList<IProductPart> productParts = prodObj.PizzaObj.PizzaParts;


                if (productParts != null && productParts.Count > 0)
                {
                    foreach (ProductPart iProPart in productParts)
                    {
                        if (iProPart.PartID == partNo && iProPart.BomNodeType == "P1")
                        {

                            flag = true;   // 有绑定记录
                            break;
                        }
                    }

                }
               
            }
           else
           {
               IList<IProductPart> productParts = prodObj.ProductParts;
               if (productParts != null && productParts.Count > 0)
               {
                   foreach (ProductPart iProPart in productParts)
                   {
                       if (iProPart.PartID == partNo && iProPart.BomNodeType == "P1")
                       {

                           flag = true;   // 有绑定记录
                           break;
                       }
                   }

               }
           }
            return flag;
        }

        private bool CheckCOA(IProduct prodObj, string partNo)
        {
            IList<IProductPart> productParts = prodObj.ProductParts;
            bool flag = false;

            if (productParts != null && productParts.Count > 0)
            {
                foreach (ProductPart iProPart in productParts)
                {
                    if (iProPart.PartID == partNo && iProPart.BomNodeType == "P1")
                    {

                        flag = true;   // 有绑定记录
                        break;
                    }
                }

            }
            return flag;
        }
	}
}
