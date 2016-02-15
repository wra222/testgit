// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo号码，UnPack属于DeliveryNo的所有Product
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21                   create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Part;
using System;
using IMES.FisObject.Common.Station;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 用于UnPack属于DeliveryNo的所有Product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取DeliveryNo，调用ProductRepository的Update方法
    ///           update Product set DeliveryNo='',PalletNo='',CartonSN='',CartonWeight=0.0
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
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
    ///         更新Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class GetStagAndSaveSn : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetStagAndSaveSn()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            string  stag =(string) CurrentSession.GetValue(Session.SessionKeys.CN);
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string sn = currentProduct.CUSTSN;
            string prodid = currentProduct.ProId;
           
            if (stag == "S")
            {
                //NumControl nc = new NumControl(-1, "CPQSNO", "", sn, "HP");

               // numCtrlRepository.InsertNumControlDefered(CurrentSession.UnitOfWork, nc);

                string line = default(string);
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
               
                //IMES.FisObject.Common.Station
                StationStatus status = (StationStatus)(1);
                line = string.IsNullOrEmpty(this.Line) ? currentProduct.Status.Line : this.Line;

                    var productLog = new ProductLog
                    {
                        Model = currentProduct.Model,
                        Status = status,
                        Editor = Editor,
                        Line = line,
                        Station = "58",
                        Cdt = DateTime.Now
                    };

                    currentProduct.AddLog(productLog);
                    productRepository.Update(currentProduct, CurrentSession.UnitOfWork);
               
            }

            if (stag == "T")
            {       
               // void InsertForceNWC(ForceNWCInfo item);
               //  bool CheckExistForceNWC(ForceNWCInfo condition);
                ForceNWCInfo cond = new ForceNWCInfo();
                cond.productID = currentProduct.ProId;
                bool bExist = partRep.CheckExistForceNWC(cond);
                if (bExist == true)
                {
                    partRep.UpdateForceNWCByProductID("58", "59", currentProduct.ProId);
                }
                else
                {
                    ForceNWCInfo newinfo = new ForceNWCInfo();
                    newinfo.editor = this.Editor;
                    newinfo.forceNWC = "58";
                    newinfo.preStation = "59";
                    newinfo.productID = currentProduct.ProId;
                    partRep.InsertForceNWC(newinfo);
                }
            }

            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, prodid);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "MasterLabel");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);



            return base.DoExecute(executionContext);
        }
	}
}
