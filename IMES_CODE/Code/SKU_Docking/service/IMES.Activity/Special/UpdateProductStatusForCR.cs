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

namespace IMES.Activity.Special
{
    /// <summary>
    /// 更新Product status，增加log
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于FARepair CI-MES12-SPEC-FA-UC FA Repair
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据Session.CurrentRepairdefect取得Product对象
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.CurrentRepairdefect
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         Update ProductStatus
    ///         insert ProductLog
    ///         insert ProductTestLog
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         TestLog
    ///         
    /// </para> 
    /// </remarks>
    public partial class UpdateProductStatusForCR : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public UpdateProductStatusForCR()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Wrint Product Log
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct currentProduct = null;
            RepairDefect defect = null;
            var newStatus = new ProductStatus();
            string ProductID = "";
            
            defect = (RepairDefect)CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);
            if (defect == null)
            {
                List<string> errpara = new List<string>();

                errpara.Add("CurrentRepairdefect");
                FisException e = new FisException("CHK194", errpara);
                e.stopWF = this.IsStopWF;
                throw e; 
            }

            if (IsNewProID) 
            {
                ProductID = defect.NewPartSno;//NEW
            }
            else 
            {
                ProductID = defect.OldPartSno;//OLD
            }
            //Vincent modify this for CQ CleanRoom trace maijor id is CT
            //currentProduct = productRepository.Find(ProductID);
            currentProduct = productRepository.GetProductByIdOrSn(ProductID);
            if (currentProduct==null)
            {
                List<string> errpara = new List<string>();

                errpara.Add(ProductID);
                FisException e = new FisException("SFC002", errpara);
                e.stopWF=this.IsStopWF;
                throw e; 
            }

            ProductID = currentProduct.ProId;
            //UnpackCarton
            if (!IsNewProID)
            {
                //var currentProductObject = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                //String cartonSN = currentProduct.CartonSN;
                //String palletNo = currentProduct.PalletNo;
                //String dn = currentProduct.DeliveryNo;
                //if (currentProduct != null)
                //{
                //    productId = currentProduct.ProId;
                //}
                productRepository.BackUpProduct(ProductID, this.Editor);
                //productRepository.CopyProductToUnpackDefered(CurrentSession.UnitOfWork, cartonSN, palletNo, dn, productId, this.Editor);
                currentProduct.CartonSN = "";
            }

            //write ProductStatusEx
            IList<IMES.DataModel.TbProductStatus> stationList = productRepository.GetProductStatus(new List<string> { currentProduct.ProId });
            productRepository.UpdateProductPreStationDefered(CurrentSession.UnitOfWork, stationList);
            
            //write ProductStatus
            newStatus.ProId = currentProduct.ProId;
            newStatus.Status = Status;
            newStatus.StationId = Station;
            newStatus.Editor = Editor;
            newStatus.Line =  currentProduct.Status.Line ;
            newStatus.TestFailCount = 0;
            newStatus.ReworkCode = "";
            currentProduct.UpdateStatus(newStatus);

           //write productLog
            var productLog = new ProductLog
            {
                Model = currentProduct.Model,
                Status = Status,
                Editor = Editor,
                Line = currentProduct.Status.Line,
                Station = Station,
                Cdt = DateTime.Now
            };
            currentProduct.AddLog(productLog);

            //write ProductTestLog
            if (this.IsWriteTestLog)
            {

            TestLog.TestLogStatus status = (Status== StationStatus.Pass? TestLog.TestLogStatus.Pass: TestLog.TestLogStatus.Fail);

            TestLogDefect defectItem = new TestLogDefect(0, 0,defect.DefectCodeID, this.Editor, DateTime.Now);
            //TestLog item = new TestLog(0, currentProduct.ProId, string.IsNullOrEmpty(Line) ? currentProduct.Status.Line : Line,
            //                                            "", Station, status, "", this.TestLogAction, "", "", this.Editor, "PRD", DateTime.Now);
            IList<TestLogDefect> defectItemList = new List<TestLogDefect>();
            defectItemList.Add(defectItem);

            TestLog item = new TestLog(0, currentProduct.ProId, currentProduct.Status.Line ,
                                               "", Station, defectItemList, status, "", this.TestLogAction, "", "", this.Editor, "PRD", DateTime.Now);
                       
                
            //item.AddTestLogDefect(defectItem);

            currentProduct.AddTestLog(item);
            }
            
            productRepository.Update(currentProduct, CurrentSession.UnitOfWork);
                

            return base.DoExecute(executionContext);
        }



        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(UpdateProductStatusForCR), new PropertyMetadata(StationStatus.Pass));

        /// <summary>
        /// Status of Product
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(UpdateProductStatusForCR.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatusForCR.StatusProperty, value);
            }
        }

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(UpdateProductStatusForCR), new PropertyMetadata(true));

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
                return ((bool)(base.GetValue(UpdateProductStatusForCR.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatusForCR.IsStopWFProperty, value);
            }
        }

       

        /// <summary>
        ///  判別是new product or old product
        /// </summary>
        public static DependencyProperty IsNewProIDProperty = DependencyProperty.Register("IsNewProID", typeof(bool), typeof(UpdateProductStatusForCR), new PropertyMetadata(true));

        /// <summary>
        ///  判別是new product or old product
        /// </summary>
        [DescriptionAttribute("IsNewProID")]
        [CategoryAttribute("InArguments Of SFC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]       
        public bool IsNewProID
        {
            get
            {
                return ((bool)(base.GetValue(UpdateProductStatusForCR.IsNewProIDProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatusForCR.IsNewProIDProperty, value);
            }
        }

        

        /// <summary>
        /// 是否要WriteTestLog
        /// </summary>
        public static DependencyProperty IsWriteTestLogProperty = DependencyProperty.Register("IsWriteTestLog", typeof(bool), typeof(UpdateProductStatusForCR), new PropertyMetadata(false));


        /// <summary>
        /// IsWrite:True Or False
        /// </summary>
        [DescriptionAttribute("IsWrite")]
        [CategoryAttribute("TestLog Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsWriteTestLog
        {
            get
            {
                return ((bool)(base.GetValue(UpdateProductStatusForCR.IsWriteTestLogProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatusForCR.IsWriteTestLogProperty, value);
            }
        }


        /// <summary>
        ///WriteTestLog Action Name
        /// </summary>
        public static DependencyProperty TestLogActionProperty = DependencyProperty.Register("TestLogAction", typeof(string), typeof(UpdateProductStatusForCR), new PropertyMetadata(""));


        /// <summary>
        /// WriteTestLog Action Name
        /// </summary>
        [DescriptionAttribute("TestLogAction")]
        [CategoryAttribute("TestLog Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string TestLogAction
        {
            get
            {
                return ((string)(base.GetValue(UpdateProductStatusForCR.TestLogActionProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatusForCR.TestLogActionProperty, value);
            }
        }
	}
}
