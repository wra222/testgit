/* INVENTEC corporation (c)2010 all rights reserved. 
 * Description: 產生CT號
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2010-02-09   Tong.Zhi-Yong                implement DoExecute method
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
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 產生CT號相关逻辑
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FRU IECSNO print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.Insert PartSN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.CTList
    ///         Session.SessionKeys.VendorSN
    ///         Session.SessionKeys.VendorDCode
    ///         Session.SessionKeys.DCode
    ///         Session.SessionKeys.AssemblyCode
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.PN111
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         PartSN
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IPartSnRepository
    /// </para> 
    /// </remarks>
	public partial class GenerateFRUIECSNO: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateFRUIECSNO()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 產生CT號相关逻辑
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            PartSn partSNObj = null;
            IPartSnRepository ipsr = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
            List<string> lstCT = (List<string>)CurrentSession.GetValue(Session.SessionKeys.CTList);
            string iecSn = string.Empty;
            string iecPn = string.Empty;
            string partType = string.Empty;
            string vendorSN = string.Empty;
            string vendorDCCode = (string)CurrentSession.GetValue(Session.SessionKeys.VendorDCode);
            string vCode = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);
            string pn151 = (string)CurrentSession.GetValue(Session.SessionKeys.PartNo);
            string dateCode = (string)CurrentSession.GetValue(Session.SessionKeys.DateCode);
            string assemblyCode = (string)CurrentSession.GetValue(Session.SessionKeys.AssemblyCode);

            iecPn = getIecPNByAssemblyCode(assemblyCode);
            partType = getPartTypeByPn(iecPn);

            CurrentSession.AddValue(Session.SessionKeys.PN111, iecPn);

            if (lstCT != null && lstCT.Count != 0)
            {
                foreach (string temp in lstCT)
                {
                    iecSn = temp;
                    partSNObj = new PartSn(iecSn, iecPn, partType, vendorSN, vendorDCCode, vCode, pn151, Editor, dateCode);

                    ipsr.Add(partSNObj, CurrentSession.UnitOfWork);
                }
            }

            return base.DoExecute(executionContext);
        }

        private string getIecPNByAssemblyCode(string assemblyCode)
        {
            IPartRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<AssemblyCode> lstAssem = ipr.FindAssemblyCode(assemblyCode);

            return (lstAssem != null && lstAssem.Count != 0 ? lstAssem[0].Pn : string.Empty);
        }

        private string getPartTypeByPn(string iecPn)
        {
            IPartRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart part = ipr.Find(iecPn);
            
            return (part != null ? part.Type : string.Empty);
        }
	}
}
