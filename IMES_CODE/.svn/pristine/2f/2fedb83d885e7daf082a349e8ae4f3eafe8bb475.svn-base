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
    /// CheckMBData
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PCA Test Station 
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
    public partial class CheckMBData : BaseActivity
	{

        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckMBData()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check MB Data
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            CurrentSession.AddValue("HavePromptinfo", "NO!");
            CurrentSession.AddValue("NewTerstLogremark", "");

            //2.28.	MBCode:若第6码为’M’，则取MBSN前3码为MBCode，若第5码为’M’，则取前2码
            //CheckCode:若MBSN的第5码为’M’，则取MBSN的第6码，否则取第7码

            string strMBCode =  currentMB.Sn.Substring(0, 2);            
            //--------------------------------------------------
            //1.是否存在重复的MB Sno 		
            //select @Count = COUNT(*) from PCB nolock where PCBNo = @MBSno 若@Count >1，则报错：“MBSno 重复”	
            if (currentMBRepository.GetCountOfPCB(currentMB.Sn) > 1)
            {
                List<string> errpara1 = new List<string>();
                errpara1.Add(currentMB.Sn);
                throw new FisException("CHK159", errpara1);
            }
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
            //a.	Model是否存在，若存在获取@Model（PCB.PCBModelID, Condition: PCB.PCBNo=@MBSno）
            if ((currentMB.Model == null) || (currentMB.Model == ""))
            {
                List<string> errpara5 = new List<string>();
                errpara5.Add(currentMB.Sn);
                throw new FisException("CHK222", errpara5);
            }
            //b.	Family是否存在，若存在获取@family(GetData..Part.Descr, Contidion: GetData..Part.PartNo=@Model)
            //IMES.FisObject.PCA.MB.MB.get_Family() 位置 G:\imes-sa\kernel\IMES.FisObject\PCA\MB\MB.cs:行号 160
            try
            {
                if ((currentMB.Family == null) || (currentMB.Family == ""))
                {
                    List<string> errpara6 = new List<string>();
                    errpara6.Add(currentMB.Sn);
                    throw new FisException("CHK223", errpara6);
                }
            }
            catch (FisException e)
            {
                List<string> errparaFam = new List<string>();
                errparaFam.Add(currentMB.Sn);
                errparaFam.Add(e.mErrcode);
                throw new FisException("CHK223", errparaFam);
            }
            try
            {
                if (currentMB.IsVB)
                {
                    CurrentSession.AddValue("IsMBOKVGA", true);

                }
                else
                {
                    CurrentSession.AddValue("IsMBOKVGA", false);
                }
            }
            catch (FisException)
            {
                CurrentSession.AddValue("IsMBOKVGA", false);
            }
            return base.DoExecute(executionContext);
        }
	}
}
