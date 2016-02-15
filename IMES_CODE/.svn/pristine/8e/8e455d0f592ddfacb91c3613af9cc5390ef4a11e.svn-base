/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 检查MB的model是否hold
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于sa各需要站
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
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
    ///         Session.MB
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///        
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IBOMRepository
    /// </para> 
    /// </remarks>
    public partial class ModelHold : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ModelHold()
        {
            InitializeComponent();

        }
        /// <summary>
        /// get 1397 from session, then check
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mb =(IMB) CurrentSession.GetValue(Session.SessionKeys.MB);
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            string modelNo = string.Empty;

            if (null == mb)
            {
                modelNo = CurrentSession.GetValue(Session.SessionKeys._1397No).ToString();
            }
            else
            {
                modelNo = mb.MB1397;
            }
            //string modelNo = CurrentSession.GetValue(Session.SessionKeys._1397No).ToString();
            if (!string.IsNullOrEmpty(modelNo))
            {
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model model = modelRep.Find(modelNo);
                if (model.Status == "0")
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK034", erpara);
                }
            }
            return base.DoExecute(executionContext);
        }
    }
}
