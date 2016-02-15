// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 用于检查SVB是否有效
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-03   Yuan XiaoWei                 create
// 2009-11-03   Yuan XiaoWei                 Modify ITC-1103-0199
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
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.PartSn;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PCA.MB;

namespace IMES.Activity
{
    /// <summary>
    /// 用于检查SVB是否有效
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于VGALabel Reprint
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从PCBInfo中查询输入的SVB是否存在，不存在报错
    ///         2.从Part表中获取SVB的FruNo和PN信息
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK063
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.FRUNO
    ///         Session.PN111
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              MBInfo
    ///              IPart
    /// </para> 
    /// </remarks>
    public partial class CheckSVBSno : BaseActivity
    {
        public CheckSVBSno()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IList<MBInfo> currentMBInfos = currentMBRepository.GetPCBInfoByTypeValue(SVB, Key);

            if (currentMBInfos == null || currentMBInfos.Count == 0)
            {
                List<string> errpara = new List<string>();
                errpara.Add(Key);
                throw new FisException("CHK063", errpara);
            }

            IMB currentMB = currentMBRepository.Find(currentMBInfos[0].PCBID);
            if (currentMB == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(Key);
                throw new FisException("CHK063", errpara);
            }


            IPartRepository currentPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart currentPart = currentPartRepository.Find(currentMB.Model);
            if (currentPart == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(Key);
                throw new FisException("CHK063", errpara);
            }
            else
            {
                string FruNo = currentPart.GetAttribute("FRUNO");

                if (String.IsNullOrEmpty(FruNo))
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(currentPart.PN);
                    throw new FisException("CHK093", errpara);
                }

                CurrentSession.AddValue(Session.SessionKeys.FRUNO, FruNo);
                CurrentSession.AddValue(Session.SessionKeys.PN111, currentPart.PN);

                //for ReprintLog Activity
                CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, Key);
                CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, Key);
                CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, PCA);
            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// InfoType 是 SVB
        /// </summary>
        private const string SVB = "SVB";

        /// <summary>
        /// InfoType 是 SVB
        /// </summary>
        private const string PCA = "PCA";
    }
}
