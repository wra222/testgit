using System;
using System.ComponentModel;
using System.Collections;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.TestLog;
using System.Collections.Generic;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Station;


namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UpdateProductSNCLeanRoom : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public UpdateProductSNCLeanRoom()
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
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
           // IProduct currentProduct = null;
            RepairDefect defect = null;
            string ProductID = "";
            IProduct CurrentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            defect = (RepairDefect)CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);
            if (defect == null)
            {
                List<string> errpara = new List<string>();

                errpara.Add("CurrentRepairdefect");
                FisException e = new FisException("CHK194", errpara);
                e.stopWF = this.IsStopWF;
                throw e;
            }
            // 0001494 用旧LCM CT 查询Product.CUSTSN,如果有值，才需要把新的LCM 更新到CUSTSN 栏位。
            if (productRepository.GetProductByIdOrSn(defect.OldPartSno) != null)
            {
                if (CurrentProduct == null)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(ProductID);
                    FisException e = new FisException("SFC002", errpara);
                    e.stopWF = this.IsStopWF;
                    throw e;
                }
                CurrentProduct.CUSTSN = defect.NewPartSno;

                productRepository.Update(CurrentProduct, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
        }


        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(UpdateProductSNCLeanRoom), new PropertyMetadata(true));

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("InArguments Of SFC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(UpdateProductSNCLeanRoom.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(UpdateProductSNCLeanRoom.IsStopWFProperty, value);
            }
        }
	}
}
