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
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{


    /// <summary>
    /// Check the Model whether is BSam
    /// </summary>
  
    public partial class CheckBSamModel : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CheckBSamModel()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckBSamModel), new PropertyMetadata(true));
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
                return ((bool)(base.GetValue(CheckBSamModel.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckBSamModel.IsStopWFProperty, value);
            }
        }

        /// <summary>
        /// 找不到BSam Model 報錯
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", typeof(bool), typeof(CheckBSamModel), new PropertyMetadata(true));

        /// <summary>
        /// 找不到BSam Model 報錯
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of CheckBSamModel")]
        [BrowsableAttribute(true)]       
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NotExistException
        {
            get
            {
                return ((bool)(base.GetValue(CheckBSamModel.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(CheckBSamModel.NotExistExceptionProperty, value);
            }
        }

        /// <summary>
        /// 找到BSam Model 報錯
        /// </summary>
        public static DependencyProperty ExistExceptionProperty = DependencyProperty.Register("ExistException", typeof(bool), typeof(CheckBSamModel), new PropertyMetadata(false));

        /// <summary>
        /// 找到BSam Model 報錯
        /// </summary>
        [DescriptionAttribute("ExistException")]
        [CategoryAttribute("InArguments Of CheckBSamModel")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool ExistException
        {
            get
            {
                return ((bool)(base.GetValue(CheckBSamModel.ExistExceptionProperty)));
            }
            set
            {
                base.SetValue(CheckBSamModel.ExistExceptionProperty, value);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
             IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
             IFamilyRepository famliyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
             FamilyInfoDef fcond = new FamilyInfoDef();
             if (currentProduct != null)
             {
                 fcond.family = currentProduct.Family;
             }
             else
             {
                 Delivery delivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
                 string model = delivery.ModelName;
                 IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                 Model modelObj= modelRep.Find(model);
                 fcond.family = modelObj.FamilyName.Trim();
             }
           
             fcond.name = "Category";
             IList<FamilyInfoDef> famValList = famliyRep.GetExistFamilyInfo(fcond);
             CurrentSession.AddValue("IsTablet", false);
             if (famValList.Count > 0 && famValList[0].value=="Tablet")
             {
                 CurrentSession.AddValue("IsTablet", true);
                 CurrentSession.AddValue(ExtendSession.SessionKeys.IsBSamModel, "N");
                 return base.DoExecute(executionContext);
             }
         

             IBSamRepository bsamRepository = RepositoryFactory.GetInstance().GetRepository<IBSamRepository,BSamLocation>();
             string modelName = ""; 
            if (CurrentSession.GetValue(Session.SessionKeys.ModelName) != null)
             {
                 modelName = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName); 
             }
            else if (CurrentSession.GetValue(Session.SessionKeys.DeliveryNo) != null)
            {
                string dnNo=(string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo) ;
                Delivery dn= dnRep.Find(dnNo);
                if (dn == null)
                {
                    FisException e = new FisException("CHK190", new string[] { dnNo });
                    throw e;
                }
                modelName = dn.ModelName;
                CurrentSession.AddValue(Session.SessionKeys.Delivery,dn);
            }
            else
             {
                 modelName = currentProduct.Model;
             }
            BSamModel bsamModel = bsamRepository.GetBSamModel(modelName);

            CurrentSession.AddValue(ExtendSession.SessionKeys. IsBSamModel,"N");

            if (bsamModel != null)
            {
                if (this.ExistException)
                {
                    FisException e = new FisException("CHK988", new string[] { modelName });
                    e.stopWF = IsStopWF;
                    throw e;
                }
                CurrentSession.AddValue(ExtendSession.SessionKeys.IsBSamModel, "Y");
                CurrentSession.AddValue(ExtendSession.SessionKeys.BSamModel, bsamModel);
            }
            else if (NotExistException)
            {
                FisException e = new FisException("CHK983", new string[] { modelName });
                e.stopWF = IsStopWF;
                throw e;
            }
            return base.DoExecute(executionContext);
          
        }
	}
}
