// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的ProductID,获取Product对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-02-15   Vincenti                 create
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.DataModel;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.Misc;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.Model;
using IMES.Common;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckStationAssignCUSTSN : BaseActivity
    {
        ///<summary>
        ///</summary>
        public CheckStationAssignCUSTSN()
        {
            InitializeComponent();
        }

        /// <summary>
        /// InputType
        /// </summary>
        public static DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(EnumInputType), typeof(CheckStationAssignCUSTSN), new PropertyMetadata(EnumInputType.Product));

        /// <summary>
        /// InputType
        /// </summary>
        [DescriptionAttribute("InputType")]
        [CategoryAttribute("InputType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public EnumInputType InputType
        {
            get
            {
                return ((EnumInputType)(base.GetValue(InputTypeProperty)));
            }
            set
            {
                base.SetValue(InputTypeProperty, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public enum EnumInputType
        {
            /// <summary>
            /// 
            /// </summary>
             Product=1,
            /// <summary>
            /// 
            /// </summary>
            ModelName
        }

        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            Session session =CurrentSession;
            string custSN = null;
            string family = null;
            if (this.InputType == EnumInputType.Product)
            {
                IProduct prod = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
                custSN = prod.CUSTSN;
                family = prod.Family;
            }
            else
            {
                string modelName = utl.IsNullOrEmpty<string>(session, Session.SessionKeys.ModelName);
                var modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                Model modelObj = modelRep.Find(modelName);
                family = modelObj == null ? "" : modelObj.FamilyName;
            }
            
            
            IList<ConstValueInfo> cvInfoList =null;
            ConstValueInfo    matchFamilyRE = null;
            session.AddValue("NeedAssignCUSTSN", "N");
            if (utl.TryConstValue("AssignCUSTSNStation", this.Station, out cvInfoList, out matchFamilyRE))
            {
                if (string.IsNullOrEmpty(custSN) && 
                    !string.IsNullOrEmpty(matchFamilyRE.value) )
                {
                    if (Regex.IsMatch(family, matchFamilyRE.value))
                    {
                        session.AddValue("NeedAssignCUSTSN", "Y");
                    }
                }
            }
           
            return base.DoExecute(executionContext);
        }      
       
    }
}
