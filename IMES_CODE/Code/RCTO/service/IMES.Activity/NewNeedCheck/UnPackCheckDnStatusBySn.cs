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
using IMES.FisObject.PAK.DN;
using System.Collections.Generic;


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
    public partial class UnPackCheckDnStatusBySn : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackCheckDnStatusBySn()
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
            IProduct defaultProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string deliveryNo = defaultProduct.DeliveryNo;
            if (deliveryNo=="")
            {
               /* List<string> errpara = new List<string>();
                errpara.Add("no deliveryNo");
                throw new FisException("CHK107", errpara);*/
                return base.DoExecute(executionContext);
            }
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            Delivery CurrentDelivery = DeliveryRepository.Find(deliveryNo);
            if (CurrentDelivery == null)
            {
               /* List<string> errpara = new List<string>();
                errpara.Add(deliveryNo);
                throw new FisException("CHK107", errpara);*/
                return base.DoExecute(executionContext);
            }
            CurrentSession.AddValue(Session.SessionKeys.Delivery, CurrentDelivery);
            if (CurrentDelivery.Status == "98") {
                List<string> errpara = new List<string>();
                errpara.Add("已经上传SAP，不能Unpack!");
                throw new FisException("CHK290", errpara);
            }
            return base.DoExecute(executionContext);
        }
	}
}
