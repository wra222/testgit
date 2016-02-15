/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:检查MB是不是连板的子板，且已经做了先切后测（MBCode.Type=1）的设置
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13  Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：declare @MBSno varchar(10),@tp varchar(10)
                if CHARINDEX(SUBSTRING(@MBSno,6,1),'0123456789')= 0
                begin
                    select @tp = Type from IMES2012_GetData..MBCode nolock where MBCode = LEFT(@MBSno,2)
                    if @tp ='0'  select '该MB需先去做ICT测试，刷PCA ICT Input'
                end
 * 
 */

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.PCA.MBMO;

namespace IMES.Activity
{
    /// <summary>
    /// 检查Model是否存在
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于SA： MB SPlit
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
    ///         Model 
    ///         
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
    /// 数据更新:
    ///    
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IModelRepository
    ///         IModel
    /// </para> 
    /// </remarks>
    public partial class SaveMB : BaseActivity
	{
        /// <summary>
        /// SaveMB
        /// </summary>
        public SaveMB()
		{
			InitializeComponent();
		}


        /// <summary>
        /// a.  生成 MBSno,  b.	Insert [PCB], c.	Insert [PCBStatus] d.	Insert [PCBLog] ,e.	Update [PCBStatus] ,f.	Insert [PCBLog] ,g.	Insert [PCBLog] 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            string mbsno = (string)CurrentSession.GetValue(Session.SessionKeys.MBSN).ToString();
            IMBRepository imbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            if ((bool)CurrentSession.GetValue(Session.SessionKeys.IsCheckPass))
            {
                //MBCodeDef mbcode = imbRepository.GetMBCode(mbsno.Substring(0, 2));
                MBCodeDef mbcode = new MBCodeDef();
                if (mbsno.Substring(5, 1) == "M" || mbsno.Substring(5, 1) == "B")
                    mbcode = imbRepository.GetMBCode(mbsno.Substring(0, 3));
                else
                    mbcode = imbRepository.GetMBCode(mbsno.Substring(0, 2));

                if (mbcode == null)
                {
                    erpara.Add(mbsno);
                    ex = new FisException("PAK080", erpara);     //没有找到MBSno %1 对应的PCB！
                    throw ex;
                }
                int unitQty = mbcode.Qty;
                if (unitQty<=0)
                {
                    erpara.Add(mbsno);
                    ex = new FisException("PAK081", erpara);    // MBCode数据维护不全！
                    throw ex;
                }
              

                // a.  生成 MBSno:a.	生成@unitqty[@i=1..@unitqty]个MBSno, @NewMBSno = left([刷入的MBSno],5) + @i + right([刷入的MBSno],4)

                string PCBModelID=(string)CurrentSession.GetValue(Session.SessionKeys.PCBModelID);
                string newMBSno = string.Empty;
                var MBObjectList = new List<IMB>();
                CurrentSession.AddValue(Session.SessionKeys.MBList, MBObjectList);
                var CurrentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
                IList<string> newMBSnoList = new List<string>();
                for (int i = 1; i <= unitQty; i++)
                {
                    newMBSno = mbsno.Substring(0, 5) + i.ToString() + mbsno.Substring(mbsno.Length - 4, 4);

                    //b.	Insert [PCB], PCBModelID = [Model]:
                    //c.	Insert [PCBStatus] For Every New MB, Status=’1’ Station=’09’:
                    

                    string moNo = CurrentMB.SMTMO;
                    string custSn = "";
                    string model = CurrentMB.Model;
                    string dateCode = CurrentMB.DateCode;
                    string mac = "";
                    string uuid = "";
                    string ecr = "";
                    string iecVer = "";
                    string custVer = "";
                    string cvsn = "";

                    MB mb = new MB(newMBSno, moNo, custSn, model, dateCode, mac, uuid, ecr, iecVer, custVer, cvsn, DateTime.Now, DateTime.Now);
                    MBStatus mbStatus = new MBStatus(newMBSno, this.Station, MBStatusEnum.Pass, this.Editor, this.Line, DateTime.Now, DateTime.Now);

                    mb.MBStatus = mbStatus;
                    
                    imbRepository.Add(mb, CurrentSession.UnitOfWork);
                    MBObjectList.Add(mb);

                    newMBSnoList.Add(newMBSno);                 
                }
                            
               //d.	Insert [PCBLog] For Every New MB, Status=’1’,Station=’09’: 调 WriteMBLog， 成批加入

               CurrentSession.AddValue(Session.SessionKeys.MBList, MBObjectList);

               //e.	Update [PCBStatus] For Old MB, Status =2,Station = ‘CL’ --Close : 调 UpdateMBStatus

               //f.	Insert [PCBLog] For Old MB, Status =2,Station = ‘CL’ –Close : 调 WriteMBLog， 单条加入

               // g.	Insert [IMES_GetData..PrintLog]: 


               CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "MB");
               CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, MBObjectList[0].Sn);
               CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, MBObjectList[MBObjectList.Count-1].Sn);
               CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, this.Line +" "+ PCBModelID);

               CurrentSession.AddValue(Session.SessionKeys.MBSNOList, newMBSnoList);
            }
            else
            {
                erpara.Add(mbsno);
                ex = new FisException("CHK161", erpara);     //MB号:%1错误！
                throw ex;
            }
          //  CurrentSession.AddValue(Session.SessionKeys.
                   
           

            return base.DoExecute(executionContext);
        }
	
	}
}
