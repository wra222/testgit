/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PAQC抽樣
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2011-03-15   Tong.Zhi-Yong                implement DoExecute method
 * Known issues:
 */
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 抽樣
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      PAQC Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// QCRatio表中维护了抽样规则 PAQC: QCRatio.PAQCRatio
    /// PAQC Sampling 算法与EOQC Sampling 算法相同，不同的是PAQC Sample Ration 取自QCRatio.PAQCRatio 栏位
    /// EBook 先按照Model 获取PAQCRatio，如果没有，再按照Family 获取
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.RandomInspectionStation
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IFamilyRepository
    ///         IModelRepository
    /// </para> 
    /// </remarks>
	public partial class VotePAQC: BaseActivity
	{
        private const string PAQC = "PAQC";
        private const string SKIP = "SKIP";

        /// <summary>
        /// 构造函数
        /// </summary>
		public VotePAQC()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 抽樣
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                IFamilyRepository ifr = null;
                IModelRepository imr = null;
                QCRatio ratio = null;
                int paqc = -1;
                int cnt = -1;
                Model modelObj = null;
                IList<ProductQCStatus> statusList = null;
                ifr = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>(); 
                imr = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                
                modelObj = product.ModelObj;

                if (modelObj == null)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(product.Model);
                    throw new FisException("CHK039", errpara);
                }

                //EBook 先按照Model 获取EOQCRatio，如果没有，再按照Family 获取
                ratio = ifr.GetQCRatio(modelObj.ModelName);   
                                             
                if (ratio == null)
                {
                    ratio = ifr.GetQCRatio(modelObj.FamilyName);
                }
                             
                if (ratio == null)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK040", errpara);
                }

                cnt = imr.GetSampleCount(product.Status.Line, product.Model, "PAQC");
                
                paqc = ratio.PAQCRatio;

                //get inspection station
                CurrentSession.AddValue(Session.SessionKeys.RandomInspectionStation, getInspectionStation(cnt,paqc));

                return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string getInspectionStation(int cnt,int paqc)
        {
            if (paqc != 0)
            {
                if (cnt % paqc == 0)
                {
                    return PAQC;
                }
            }

            return SKIP;
        }
	}
}
