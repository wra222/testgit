
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.FisObject.Common.Line;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using System.Collections.Generic;
using System;
using IMES.FisObject.Common.Model;
using System.ComponentModel;

namespace IMES.Activity
{
    /// <summary>
    ///  
    /// </summary>
    /// <remarks>
   /// </remarks>
    public partial class CheckAndSetPAKFAIModel: BaseActivity
    {
        /// <summary>
        ///
        /// </summary>
        public CheckAndSetPAKFAIModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// store InfoType
        /// </summary>
        public static DependencyProperty StoreProdInfoTypeProperty = DependencyProperty.Register("StoreProdInfoType", typeof(string), typeof(CheckAndSetPAKFAIModel), new PropertyMetadata("FAIinPAK"));

        /// <summary>
        ///Store InfoType
        /// </summary>
        [DescriptionAttribute("StoreProdInfoType")]
        [CategoryAttribute("StoreProdInfoType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string StoreProdInfoType
        {
            get
            {
                return ((string)(base.GetValue(StoreProdInfoTypeProperty)));
            }
            set
            {
                base.SetValue(StoreProdInfoTypeProperty, value);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
           IProduct prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
           IList<IProduct> lstProd=new List<IProduct>{prod};
           modelRep.CheckAndSetInPAKQtyWithTransDefered(this.CurrentSession.UnitOfWork, prod.Model, 1, lstProd, this.StoreProdInfoType,"Y",this.Editor);
           return base.DoExecute(executionContext);
        }
    }
}
