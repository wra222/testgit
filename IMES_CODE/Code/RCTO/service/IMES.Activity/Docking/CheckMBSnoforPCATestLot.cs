// INVENTEC corporation (c)2009 all rights reserved. 
// Description: PCA Test StationLot For Docking 检查MBSNO，处理15种异常情况
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-25   Kaisheng                     create
// Known issues:
using System;
using System.Collections.Generic;
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Repair;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// PCA Test Station 检查MBSNO，处理15种异常情况
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PCA Test Station For Docking
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         Check MB Sno
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    ///         IMB
    /// </para> 
    /// </remarks>
    public partial class CheckMBSnoforPCATestLot : BaseActivity
	{

        /// <summary>
        /// 构造函数
        /// </summary>
		public CheckMBSnoforPCATestLot()
		{
			InitializeComponent();
		}

        /// <summary>
        /// PCA Test Station 检查MBSNO，处理15种异常情况
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            CurrentSession.AddValue("HavePromptinfo", "NO!");
            CurrentSession.AddValue("NewTerstLogremark", "");

            //--------------------------------------------------
            //1.是否存在重复的MB Sno 		
            //select @Count = COUNT(*) from PCB nolock where PCBNo = @MBSno 若@Count >1，则报错：“MBSno 重复”	
            if (currentMBRepository.GetCountOfPCB(currentMB.Sn) > 1)
            {
                List<string> errpara1 = new List<string>();
                errpara1.Add(currentMB.Sn);
                throw new FisException("CHK159", errpara1);
            }
            /*
            if ((currentMB.MAC == null) || (currentMB.MAC.Trim() == ""))
            {
                List<string> errparamacnull = new List<string>();
                errparamacnull.Add(currentMB.Sn);
                throw new FisException("CHK033", errparamacnull);
            }
            */
            //Modify 2012/2/22 Kaisheng： 
            //Reason：需求变更  7.1 去除MAC的Check
            //-------------------------------------------------------------------------------------------------------
            //是否存在重复的MAC，需要报告错误：“Duplicate MAC Address!”
            //select @Count = COUNT(*) from PCB nolock where MAC in (select MAC from PCB nolock where PCBNo=@MBSno)
            /*
            IList<IMB> maclist = currentMBRepository.GetMBListByMAC(currentMB.MAC);
            //if (currentMBRepository.GetMBListByMAC(currentMB.MAC).Count > 1)
            if ((maclist != null) && (maclist.Count > 1))
            {
                List<string> errpara2 = new List<string>();
                //errpara2.Add(currentMB.Sn);
                errpara2.Add(currentMB.MAC);
                throw new FisException("CHK032", errpara2);
            }
            */
            //--------------------------------------BEGIN-------------------------------------------------------------
            //Modify 2012/04/11 Kaisheng:UC update
            // 2.	若检测站为‘15,16,17’，检查是否有ICT Input的成功过站记录
            // select * from PCBLog nolock where Station = '10' and Status = '1' and PCBNo = @MBSno
            // 若不存在Station=10,Status=1的过站记录，则报错：“请先刷ICT Input” --PAK078(该MB需先去做ICT测试，刷ICT Input！)
            //if ((this.Station == "15") || (this.Station == "16") || (this.Station == "17"))
            //{
                IList<MBLog> resultLogList = currentMBRepository.GetMBLog(currentMB.Sn, "10", 1);
                if (resultLogList == null || resultLogList.Count == 0)
                {
                    List<string> errpara2 = new List<string>();
                    //errpara2.Add(currentMB.Sn);
                    errpara2.Add(currentMB.Sn);
                    throw new FisException("PAK078", errpara2);
                }

            //}

            //-------------------------------------- END--------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------
            //3.	是否在修护区   若存在记录，则报错：“请先将MBSno刷出修护区”
            //select * from PCBRepair nolock where PCBNo = @MBSno and Status = '0'
            Repair repairreturn = currentMB.GetCurrentRepair();
            if (repairreturn != null)
            {
                List<string> errpara3 = new List<string>();
                errpara3.Add(currentMB.Sn);
                throw new FisException("CHK220", errpara3);
            }
            //4.	是否结合主机,若存在记录，则报错：“此MB已经结合了主机”
            //select * from IMES2012_FA..Product nolock where PCBID = @MBSno
            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            if (ProductRepository.IfBindPCB(currentMB.Sn))
            {
                List<string> errpara4 = new List<string>();
                errpara4.Add(currentMB.Sn);
                throw new FisException("CHK221", errpara4);
            }

            //------------------------------------------------------------------------------------------------
            //currentMBRepository.Update(currentMB, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
	}
}
