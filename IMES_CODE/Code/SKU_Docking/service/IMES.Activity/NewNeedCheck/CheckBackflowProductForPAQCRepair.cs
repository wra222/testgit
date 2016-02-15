/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for QC Repair Page
* UI:CI-MES12-SPEC-FA-UC QC Repair.docx –2012/2/16 
* UC:CI-MES12-SPEC-FA-UC QC Repair.docx –2012/7/18            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-9-12   Jessica Liu           Create
* Known issues:
* TODO：
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Linq; 

namespace IMES.Activity
{
    /// <summary>
    /// 判断是否是Product回流机器
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      PAQC Repair
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.判断是否是Product回流机器
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         IsBackflowProduct
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///             
    /// </para> 
    /// </remarks>
    public partial class CheckBackflowProductForPAQCRepair : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBackflowProductForPAQCRepair()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 判断是否是Product回流机器
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string returnStation = (string)this.CurrentSession.GetValue("ReturnStation");

            bool isBackflowProductFlag = false;
            
            //select Value from SysSetting where [Name] = 'UnpackPAKStation'
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> returnValueLst = ipartRepository.GetValueFromSysSettingByName("UnpackPAKStation"); 
            
            if (returnValueLst == null || returnValueLst.Count <= 0)
            {
                isBackflowProductFlag = false;
            }
            else
            {
                /*
                foreach (string tmpValue in returnValueLst)
                {
                    if (tmpValue == returnStation)
                    {
                        isBackflowProductFlag = true;
                        break;
                    }
                }
                */
                string tmpValue = returnValueLst[0];

                string[] tmpLst = tmpValue.Split(',');
                if (tmpLst == null || tmpLst.Length <= 0)
                {
                    isBackflowProductFlag = false;
                }
                else
                {
                    for (int i = 0; i < tmpLst.Length; i++)
                    {
                        if (tmpLst[i] == returnStation)
                        {
                            isBackflowProductFlag = true;
                            break;
                        }
                    }
                }
            }
            
            CurrentSession.AddValue("IsBackflowProduct", isBackflowProductFlag);

            return base.DoExecute(executionContext);
        }
    }
}
