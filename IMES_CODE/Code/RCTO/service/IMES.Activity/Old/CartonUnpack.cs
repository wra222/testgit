// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 根据carton号码将其所有product的Delivery，Carton设定为空
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-05-04   Yuan XiaoWei                 create 
//                                           ITC-1155-0059
// 2011-03-11   Lucy Liu                     Unpack Carton时只清空Carton
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.PCA.TestBoxDataLog;

namespace IMES.Activity
{
    /// <summary>
    /// 根据carton号码将其所有product的Delivery，Carton设定为空
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Carton Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Carton,根据carton号码将其所有product的Delivery，Carton设定为空
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Carton
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
    ///         根据carton号码将其所有product的Delivery，Carton设定为空 
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    /// </para> 
    /// </remarks>
    public partial class CartonUnpack : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CartonUnpack()
		{
			InitializeComponent();
		}



        /// <summary>
        /// 执行根据Carton修改所有属于该Carton的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string currentCarton = (string)CurrentSession.GetValue(Session.SessionKeys.Carton);
            if (currentCarton == null||currentCarton=="")
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK109", errpara);
            }

            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            currentProductRepository.CartonUnpackDefered(CurrentSession.UnitOfWork, currentCarton);


            IList<string> ProductCustSNList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductCustSNList);
            currentProductRepository.UpdateTestBoxDataLogListForUnpackCartonDefered(CurrentSession.UnitOfWork, ProductCustSNList);

            //string custSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
            //currentProductRepository.UpdateTestBoxDataLogForUnpackCartonDefered(CurrentSession.UnitOfWork, custSN);
            return base.DoExecute(executionContext);
        }
	}
}
