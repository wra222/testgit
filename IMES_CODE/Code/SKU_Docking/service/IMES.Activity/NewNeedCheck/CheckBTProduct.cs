using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.MO;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckBTProduct : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBTProduct()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 单条还是成批插入
        /// </summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", typeof(bool), typeof(CheckBTProduct));

        /// <summary>
        /// 单条还是成批插入,Session.SessionKeys.ProdList
        /// </summary>
        [DescriptionAttribute("IsSingle")]
        [CategoryAttribute("IsSingle Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingle
        {
            get
            {
                return ((bool)(base.GetValue(CheckBTProduct.IsSingleProperty)));
            }
            set
            {
                base.SetValue(CheckBTProduct.IsSingleProperty, value);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            if (IsSingle)
            {
                Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                bool btFlag = false;
                btFlag = productRepository.CheckExistProductBT(CurrentProduct.ProId);
                if (btFlag)
                {
                    CurrentSession.AddValue(Session.SessionKeys.IsBT, 1);
                }
                else
                {
                    CurrentSession.AddValue(Session.SessionKeys.IsBT, 0);
                }                
            }
            else
            {
                IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                var currentMO = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);
                string modelName = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
                bool bFlag = false;
                if (!String.IsNullOrEmpty(modelName))
                {
                    bFlag = repPizza.CheckExistMpBtOrder(modelName);
                    if (bFlag == true)
                    {
                        CurrentSession.AddValue("BTProList", 1);
                    }
                    else
                    {
                        CurrentSession.AddValue("BTProList", 0);
                    }
                }
                else
                {
                    CurrentSession.AddValue("BTProList", 0);
                }
                
            }
            return base.DoExecute(executionContext);
        }

    }
}

