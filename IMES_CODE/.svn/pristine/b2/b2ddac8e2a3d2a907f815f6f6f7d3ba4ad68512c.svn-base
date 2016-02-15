/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: 检查输入的VGA Sno 是否存在
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-01-31   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 检查输入的VGA Sno 是否存在
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      VGA Label print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         检查输入的VGA Sno 是否存在，如果不存在，则报告错误：“Not Found VGA/B Information!!”/“没有找到VGA/B 信息！！”
    ///         如果该VGA Sno 已经生成SVB Sno，则报告错误：“已经列印过该VGA Board 的流水号!!
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///      workflow Key(VGA SN)
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.FRUNO, Session.SessionKeys.SVBCode
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    ///         IPartRepository
    ///         IMB
    /// </para> 
    /// </remarks>

    public partial class CheckVGASN : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckVGASN()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 检查输入的VGA Sno 是否存在
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMBRepository imr = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IPartRepository ipr = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IMB vga = imr.Find(Key);
            IList<MBInfo> vgaInfos = null;
            IPart part = null;
            IList<PartInfo> lstPartInfo = null;
            bool findFlag = false;

            if (vga == null)
            {
                //“Not Found VGA/B Information!!”/“没有找到VGA/B 信息！！”
                List<string> errpara = new List<string>();
                throw new FisException("CHK059", errpara);
            }

            CurrentSession.AddValue(Session.SessionKeys.VGA, vga);

            vgaInfos = vga.MBInfos;

            if (vgaInfos != null)
            {
                foreach (MBInfo item in vgaInfos)
                {
                    if (item.InfoType != null && string.Compare(item.InfoType.Trim(), "SVB", true) == 0)
                    {
                        //已经列印过该VGA Board 的流水号!!
                        List<string> errpara = new List<string>();
                        throw new FisException("CHK060", errpara);
                    }
                }
            }

            //set SVBCode for acquire svb sno
            part = ipr.Find(vga.Model);

            if (part == null || string.IsNullOrEmpty(part.GetProperty("FRUNO")))
            {
                //“Not Found FRUNO Information!！”/“没有找到FRUNO 信息！！”
                List<string> errpara = new List<string>();
                throw new FisException("CHK061", errpara);
            }

            CurrentSession.AddValue(Session.SessionKeys.FRUNO, part.GetProperty("FRUNO"));

            lstPartInfo = part.Attributes;

            if (lstPartInfo == null || lstPartInfo.Count == 0)
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK062", errpara);
            }

            foreach (PartInfo item in lstPartInfo)
            {
                if (string.Compare(item.InfoType, "SVB", true) == 0)
                {
                    findFlag = true;
                    CurrentSession.AddValue(Session.SessionKeys.SVBCode, item.InfoValue);
                    break;
                }
            }

            if (!findFlag)
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK062", errpara);                
            }

            return base.DoExecute(executionContext);
        }
	
	}
}
