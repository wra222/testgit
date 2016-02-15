// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查输入AssemblyCode的正确性
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-20   Tong.Zhi-Yong                create
// 2010-04-01   Tong.Zhi-Yong                Modify ITC-1122-0237
// 2010-05-31   Tong.Zhi-Yong                Modify ITC-1155-0146
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
using IMES.Infrastructure;
using IMES.FisObject.Common.PartSn;
using IMES.FisObject.Common.Part;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Warranty;

namespace IMES.Activity
{
    /// <summary>
    /// 检查输入AssemblyCode的正确性
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FA站Offline print CT, FRU IECSNO 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1. 通过IPartRepository的FindAssemblyCode对输入的Assembly code进行正确性验证。
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.AssemblyCode
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         For FRU IECSNO: Session.SessionKeys.PN111, Session.SessionKeys.FRUNO, Session.SessionKeys.VCode
    ///         For Offline print CT: Session.SessionKeys.DateCode
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IPartRepository
    ///         IWarrantyRepository
    /// </para> 
    /// </remarks>
	public partial class CheckAssemblyCode: BaseActivity
	{
        /// <summary>
        /// 是不是针对FRU处理
        /// </summary>
        public static DependencyProperty IsFRUProperty = DependencyProperty.Register("IsFRU", typeof(bool), typeof(CheckAssemblyCode));

        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckAssemblyCode()
		{
			InitializeComponent();
		}

        ///<summary>
        /// 是不是针对FRU处理
        ///</summary>
        [DescriptionAttribute("IsFRU")]
        [CategoryAttribute("IsFRU Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsFRU
        {
            get
            {
                return ((bool)(base.GetValue(CheckAssemblyCode.IsFRUProperty)));
            }
            set
            {
                base.SetValue(CheckAssemblyCode.IsFRUProperty, value);
            }
        }

        /// <summary>
        /// 检查输入AssemblyCode的正确性
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string assemblyCode = (string)CurrentSession.GetValue(Session.SessionKeys.AssemblyCode);
            var currentPartRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            var currentWarrantyRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
            IList<AssemblyCode> lstAssemblyCode = currentPartRepository.FindAssemblyCode(assemblyCode);
            IAssemblyCodeRepository iar = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IAssemblyCodeRepository, AssemblyCode>();

            if (lstAssemblyCode == null || lstAssemblyCode.Count == 0)
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK042", errpara);
            }

            if (IsFRU)
            {
                //ITC-1155-0146 Tong.Zhi-Yong 2010-05-31
                string partNo = getIecPNByAssemblyCode(assemblyCode);
                string fruNo = iar.GetAssemblyCodeInfo(assemblyCode, "FRUNO");//getFRUNoByPN(partNo, assemblyCode);
                string VCode = iar.GetAssemblyCodeInfo(assemblyCode, "FRUNO1");//getVCodeByPN(partNo);

                CurrentSession.AddValue(Session.SessionKeys.PN111, partNo);
                CurrentSession.AddValue(Session.SessionKeys.FRUNO, fruNo);
                CurrentSession.AddValue(Session.SessionKeys.VCode, VCode);
            }
            else
            {
                //ITC-1122-0237 Tong.Zhi-Yong 2010-04-01
                Warranty wo = null;
                wo = currentWarrantyRepository.GetWarranty(((int)CurrentSession.GetValue(Session.SessionKeys.SelectedWarrantyRuleID)));

                if (wo != null)
                {
                    CurrentSession.AddValue(Session.SessionKeys.DateCode, wo.ShipTypeCode);
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

        private string getFRUNoByPN(string partNo, string assemblyCode)
        {
            IPartRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart part = ipr.Find(partNo);

            if (part != null)
            {
                if (string.Compare(part.TypeGroup, "KP") == 0)
                {
                    return part.GetProperty("FRUNO", assemblyCode);
                }
                else
                {
                    return part.GetProperty("FRUNO");
                }
            }

            return string.Empty;
        }

        private string getVCodeByPN(string partNo)
        {
            IPartRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart part = ipr.Find(partNo);

            return part.GetAttribute("FRUNO1");
        }
	}
}
