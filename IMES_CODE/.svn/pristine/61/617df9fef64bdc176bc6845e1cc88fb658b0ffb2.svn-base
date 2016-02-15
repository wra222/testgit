
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

namespace IMES.Activity
{
    /// <summary>
    ///
    /// </summary>
    public partial class CheckProductForChangeAST : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckProductForChangeAST()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 检查ProductID是否存在69的ProductLog
        /// </summary>
        public static DependencyProperty IsNeedCheck69LogProperty = DependencyProperty.Register("IsNeedCheck69Log", typeof(bool), typeof(CheckProductForChangeAST), new PropertyMetadata(false));

        /// <summary>
        /// 检查ProductID是否存在69的ProductLog
        /// </summary>
        [DescriptionAttribute("IsNeedCheck69Log")]
        [CategoryAttribute("InArguments Of CheckProductForChangeAST")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsNeedCheck69Log
        {
            get
            {
                return ((bool)(base.GetValue(CheckProductForChangeAST.IsNeedCheck69LogProperty)));
            }
            set
            {
                base.SetValue(CheckProductForChangeAST.IsNeedCheck69LogProperty, value);
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //从Session里取得Product对象
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);


            //检查ProductID是否存在CUSTSN(Product.CUSTSN)，若CUSTSN为空或者为Null，
            //则报错：“请先产生Customer SN”
            if (String.IsNullOrEmpty(currentProduct.CUSTSN))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                ex = new FisException("CHK890", erpara);
                throw ex;
            }

            if (currentProduct.Status == null)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(currentProduct.ProId);
                ex = new FisException("SFC002", erpara);
                throw ex;
            }

            //检查ProductStatus.Status，若为’0’，则报错：“请先修护后再做AST Change”
            if (currentProduct.Status.Status == 0)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                ex = new FisException("CHK891", erpara);
                throw ex;
            }
            //检查ProductID是否存在69的ProductLog，若存在，则报错：“Product：XXX已入包装，不能更换”
            if (IsNeedCheck69Log)
            {
                IList<ProductLog> allLogs = new List<ProductLog>();
                allLogs = currentProduct.ProductLogs;
                foreach (ProductLog temp in allLogs)
                {
                    if (temp.Station == "69")
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentProduct.ProId);
                        ex = new FisException("CHK892", erpara);
                        throw ex;
                    }
                }
            }
         

            return base.DoExecute(executionContext);
        }
    }
}

