// INVENTEC corporation (c)2011 all rights reserved. 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-27   210003                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
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
using IMES.FisObject.Common.Line;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.MO;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 请参考.\Common\Common Rule.docx 文档中的相关描述.没找到相应的文档。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.MB
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class GetPiloMoInfo : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public GetPiloMoInfo()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.DNInfoValue
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string stage =(string) CurrentSession.GetValue("Stage");
            if (string.IsNullOrEmpty(stage))
            { throw new FisException("Error in GetPiloMoInfo!! No stage info in session"); }
            string mo = "";
            if (stage == "SA" || stage == "SMT")
            {
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                var mb = mbRepository.Find(Key);
                if (mb == null)
                {
                    var ex = new FisException("SFC001", new string[] { Key });
                    throw ex;
                }
               IList<IMES.FisObject.PCA.MB.MBInfo> lst= mb.MBInfos.Where(x => x.InfoType == "PilotMo").ToList();
               if (lst != null && lst.Count > 0)
               { mo = lst[0].InfoValue; }
               CurrentSession.AddValue(Session.SessionKeys.MBSN,Key);
            }
            else
            {
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct currentProduct = null;
                currentProduct = productRepository.GetProductByIdOrSn(this.Key);
                if (currentProduct == null)
                {
                    var ex = new FisException("SFC002", new string[] { Key });
                    throw ex;
                }
               IList<IMES.FisObject.FA.Product.ProductInfo> lst=  currentProduct.ProductInfoes.Where(x => x.InfoType == "PilotMo").ToList();
               if (lst != null && lst.Count > 0)
               { mo = lst[0].InfoValue; }
               CurrentSession.AddValue(Session.SessionKeys.CustSN, currentProduct.CUSTSN);
               CurrentSession.AddValue(Session.SessionKeys.ProductID, currentProduct.ProId);

            }
            if (mo != "")
            {
                IMORepository iMORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
                PilotMoInfo pilotMo = iMORepository.GetPilotMo(mo);
                if (pilotMo == null)
                { throw new FisException("This Mo:[" + mo + "] is not found"); }
                else
                { CurrentSession.AddValue(Session.SessionKeys.PilotMo, pilotMo); }
            }
            else
            {
                throw new FisException("This product or mb had not combined pilot mo");
            }
            return base.DoExecute(executionContext);
        }

       
	}
}
