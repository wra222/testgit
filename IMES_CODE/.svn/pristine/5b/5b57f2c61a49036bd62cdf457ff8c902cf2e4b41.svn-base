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
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.Extend;

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
    public partial class GetQcstatusForPiaOutput : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetQcstatusForPiaOutput()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        /// 

        ////1 2 5
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
           // bool isHasDefect = (bool)CurrentSession.GetValue(Session.SessionKeys.HasDefect);
            List<string> errpara = new List<string>();
            string productID = currentProduct.ProId;

            IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //ProductLog 
            string[] tps = new string[2];
            tps[0] = "PIA";
            tps[1]= "PIA1";
            string status = "1";
            IList<ProductQCStatus> QCStatusList = new List<ProductQCStatus>();
            QCStatusList = repProduct.GetQCStatusOrderByUdtDesc(productID, tps);
            if (QCStatusList != null && QCStatusList.Count > 0)
            {
                ProductQCStatus qcStatus = QCStatusList[0];
                if (qcStatus.Remark =="1")
                {
                    status = "2"; //epia
                }
                else if (qcStatus.Status == "5")
                {
                    status = "5"; //pia
                }
                else if (qcStatus.Status == "1")
                {
                    status = "1";
                }

            }
            //CurrentSession.AddValue(Session.SessionKeys.boxId, status);
            CurrentSession.AddValue(ExtendSession.SessionKeys.FAQCStatus, status);
            return base.DoExecute(executionContext);
        }
	}
}
