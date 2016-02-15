
using System;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Part;
using System.Linq;
using IMES.FisObject.Common.FisBOM;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using System.ComponentModel;
using IMES.FisObject.PAK.StandardWeight;
namespace IMES.Activity
{
    /// <summary>
    ///CheckModelWeight
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Product
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class CheckModelWeight : BaseActivity
	{
		///<summary>
		///</summary>
        public CheckModelWeight()
		{
			InitializeComponent();
		}
        /// <summary>
        /// Write Relase Code
        /// </summary>
        public static DependencyProperty NoExistModelWeightThrowErrorProperty = DependencyProperty.Register("NoExistModelWeightThrowError",
                                                                                                                                    typeof(bool),
                                                                                                                                    typeof(CheckModelWeight),
                                                                                                                                    new PropertyMetadata(true));

        /// <summary>
        /// Release Hold Code
        /// </summary>
        [DescriptionAttribute("NoExistModelWeightThrowError")]
        [CategoryAttribute("NoExistModelWeightThrowError")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NoExistModelWeightThrowError
        {
            get
            {
                return ((bool)(base.GetValue(CheckModelWeight.NoExistModelWeightThrowErrorProperty)));
            }
            set
            {
                base.SetValue(CheckModelWeight.NoExistModelWeightThrowErrorProperty, value);
            }
        }

        /// <summary>
        /// 重量异常时是否需要记录ForceNwc
        /// </summary>
        public static DependencyProperty WeightErrorForceNwcProperty = DependencyProperty.Register("WeightErrorForceNwc", typeof(bool), typeof(CheckModelWeight), new PropertyMetadata(false));

        /// <summary>
        /// 重量异常时是否需要记录ForceNwc
        /// </summary>
        [DescriptionAttribute("WeightErrorForceNwc")]
        [CategoryAttribute("WeightErrorForceNwc Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool WeightErrorForceNwc
        {
            get
            {
                return ((bool)(base.GetValue(CheckModelWeight.WeightErrorForceNwcProperty)));
            }
            set
            {
                base.SetValue(CheckModelWeight.WeightErrorForceNwcProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();
            DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(product.Model);
            this.CurrentSession.AddValue("ErrorMsgWithNoModelWeight", "");
            if (modelWeight == null || modelWeight.Rows.Count == 0)
            {
                if (NoExistModelWeightThrowError)
                {
                    if (WeightErrorForceNwc)
                    {
                        SaveForceNWC(product);
                    }
                    FisException e = new FisException("CQCHK0046 ", new string[] { });
                    e.stopWF = true;
                    throw e;
                }
                else
                {
                    this.CurrentSession.AddValue("ErrorMsgWithNoModelWeight", "机器没有标准重量,请维护标准重量");
                }
            }
         
           return base.DoExecute(executionContext);
        }
        private void SaveForceNWC(IProduct p)
        {
            if (p != null)
            {
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<IMES.DataModel.ConstValueTypeInfo> lstConst = partRep.GetConstValueTypeList("UnitWeightErrorToSortingLine");
                string pdline = p.Status.Line;
                if (!string.IsNullOrEmpty(pdline) && lstConst.Any(x => x.value.Equals(pdline.Substring(0, 1))))
                {

                    ForceNWCInfo cond = new ForceNWCInfo();
                    cond.productID = p.ProId;
                    string preStation = p.Status.StationId;
                    if (partRep.CheckExistForceNWC(cond))
                    {
                        partRep.UpdateForceNWCByProductID("PKCK", preStation, p.ProId);
                    }
                    else
                    {
                        ForceNWCInfo newinfo = new ForceNWCInfo();
                        newinfo.editor = this.Editor;
                        newinfo.forceNWC = "PKCK";
                        newinfo.preStation = preStation;
                        newinfo.productID = p.ProId;
                        partRep.InsertForceNWC(newinfo);

                    }
                }
            }
        }
	}
}
