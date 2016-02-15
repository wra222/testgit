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
    public partial class BlockbyStag : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public BlockbyStag()
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

            string sn = currentProduct.CUSTSN;
            string model = currentProduct.Model;
            string stag = string.Empty;
            stag = (string)currentProduct.GetModelProperty("STAG");
            //stag = "s";
            if (string.IsNullOrEmpty(stag))
            {
                //无需打印
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add("机器不需要贴附Master Label");
                ex = new FisException("CHK268", erpara);
                throw ex;

            }
            if (stag== "S")
            {
                //Master label上打印生成SN
                CurrentSession.AddValue(Session.SessionKeys.CN , stag); 

            }
            else if (stag == "T")
            {
                //Master label上打印不生成SN
                CurrentSession.AddValue(Session.SessionKeys.CN, stag); 
            }
            else {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add("数据维护错误！");
                ex = new FisException("CHK269", erpara);
                throw ex;
            }
            CurrentSession.AddValue(Session.SessionKeys.CN, stag);


            return base.DoExecute(executionContext);
        }
	}
}
