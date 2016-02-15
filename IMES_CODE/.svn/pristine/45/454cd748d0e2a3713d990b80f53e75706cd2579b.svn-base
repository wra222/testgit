/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: for set child mb sno for muti mb
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
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
using IMES.FisObject.PCA.MBModel;
using IMES.DataModel;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 更新MB状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于以MB为主线对象的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据MBSN得到ECR;
    ///         2.更新MB对象的ECR信息;
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
    /// 数据更新:
    ///         update PCB
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IMBRepository
    ///         
    /// </para> 
    /// </remarks>
    public partial class UpdateMBECR : BaseActivity
    {     

        ///<summary>
        /// 构造函数
        ///</summary>
        public UpdateMBECR()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查并更新MB对象
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mb = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
            bool ismassProduce = (bool)CurrentSession.GetValue(Session.SessionKeys.IsMassProduction ) ;
            //b.	如果MB Sno 在PCB 表中的记录的ECR 数据为空，则报告错误：“该主板尚未通过ICT Input，不能重流！！”
            if (string.IsNullOrEmpty(mb.ECR))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(mb.Sn);
                ex = new FisException("CHK087", erpara);
                throw ex;  
            }
            string ECR = CurrentSession.GetValue(Session.SessionKeys.ECR).ToString();
           
            mb.ECR = ECR;

            //if (ismassProduce)
            //{
            //    string mbcode = mb.ModelObj.Mbcode;
            //    IMBModelRepository mbmodelRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
            //    IList<EcrVersionInfo> ecrinfo = mbmodelRepository.getEcrVersionsByEcrAndMbcode(ECR, mbcode);
            //    if (ecrinfo == null || ecrinfo.Count < 1)
            //    {
            //        //'ECR 数据错误'
            //        //CHK086
            //        FisException ex;
            //        List<string> erpara = new List<string>();
            //        ex = new FisException("CHK086", erpara);
            //        throw ex;
            //    }

            //    mb.IECVER = ecrinfo[0].IECVer;
            //    mb.CUSTVER = ecrinfo[0].CustVer;
            //}
            //else
            //{
            //    mb.IECVER = CurrentSession.GetValue(Session.SessionKeys.IECVersion).ToString() ;
            //    mb.CUSTVER = CurrentSession.GetValue(Session.SessionKeys.CustomVersion).ToString();
            //}

            mb.IECVER = CurrentSession.GetValue(Session.SessionKeys.IECVersion).ToString();
            mb.CUSTVER = CurrentSession.GetValue(Session.SessionKeys.CustomVersion).ToString();


            //5. 解掉PCB_Part 中的绑定资料，以及CVSN
           //UPDATE IMES_PCA..PCB SET CVSN = '' WHERE PCBNo = @PCBNo
            //DELETE FROM IMES_PCA..PCB_Part WHERE PCBNo = @PCBNo  
            mb.CVSN="";
            mb.RemoveAllParts();
            //mb.MBParts = new List<ProductPart>(); 
           // mb.Udt = DateTime.Now;
            string datecode = CurrentSession.GetValue(Session.SessionKeys.DateCode).ToString().Trim();
            if (!string.IsNullOrEmpty(datecode))
            {
                mb.DateCode = datecode;
            }
            var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            mbRepository.Update(mb, CurrentSession.UnitOfWork);
   
            return base.DoExecute(executionContext);
        }
    }
}
