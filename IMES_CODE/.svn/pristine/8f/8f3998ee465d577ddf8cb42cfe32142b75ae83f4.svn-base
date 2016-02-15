/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for RCTO MB Change Page
* UI:CI-MES12-SPEC-SA-UI RCTO MB Change.docx –2012/6/15 
* UC:CI-MES12-SPEC-SA-UC RCTO MB Change.docx –2012/6/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-8-1    Jessica Liu           Create
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
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.PCA.MB;

namespace IMES.Activity
{
    /// <summary>
    /// Check MB SNo for RCTO MB Change
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         RCTO MB Change
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.检查MB 是否不良
    ///         3.检查MB的当前状态(PCBStatus.Status)，若为“0”，则报错：“该MB有Fail，请先修复后再重流”
    ///         4.检查MB的当前站（PCBStatus.Station），若为“S9、20、21、22、23”，则报错：“修复中，请修复完毕后再重流”；若为“CL”，则报错：“该MB已切割”；若为“28”，则报错：“该MB已经报废，不能再使用”
    ///         5.检查MB是否已经结合
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.MB
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///             IMBRepository
    ///             IProductRepository 
    /// </para> 
    /// </remarks>
    public partial class CheckMBSNoForMBChange : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckMBSNoForMBChange()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 产生Asset SN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            if (currentMB == null)
            {
                throw new NullReferenceException("MB in session is null");
            }

            //检查MB 是否不良，若存在记录，则报错：“此MB有不良，请去修复”
            IMBRepository CurrentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            bool ExistDefect = CurrentMBRepository.ExistPCBRepair(currentMB.Sn,0);
            if (ExistDefect) 
            {
                throw new FisException("BOR005", new string[] { });
            }
            
            //检查MB的当前状态(PCBStatus.Status)，若为“0”，则报错：“该MB有Fail，请先修复后再重流”
            if (currentMB.MBStatus.Status == MBStatusEnum.Fail)
            {
                throw new FisException("ICT019", new string[] { });
            }

            //检查MB的当前站（PCBStatus.Station），若为“S9、20、21、22、23”，则报错：“修复中，请修复完毕后再重流”；
            //若为“CL”，则报错：“该MB已切割”；
            //若为“28”，则报错：“该MB已经报废，不能再使用”
            if (currentMB.MBStatus.Station == "S9"
                || currentMB.MBStatus.Station == "20"
                || currentMB.MBStatus.Station == "21"
                || currentMB.MBStatus.Station == "22"
                || currentMB.MBStatus.Station == "23"
                )
            {
                var ex = new FisException("CHK854", new string[] { });
                throw ex;
            }
            if (currentMB.MBStatus.Station == "CL")
            {
                var ex = new FisException("ICT022", new string[] { });  //该MB已经切分，不能再使用！
                throw ex;
            }
            if (currentMB.MBStatus.Station == "28")
            {
                var ex = new FisException("ICT012", new string[] { });
                throw ex;
            }

            //检查MB是否已经结合，若存在，则报错：“该MB已经结合，不能重投”
            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var productByMBSn = currentProductRepository.GetProductByMBSn(currentMB.Sn);
            if (productByMBSn != null)
            {
                var ex = new FisException("CHK855", new string[] { });
                throw ex;
            }


            //2012-9-7, Jessica Liu
            //若MBSN的CheckCode为’R’，则报错：“该MBSN：XXXXXXXXXX 已经为RCTO了，不需要刷此站”
            //CheckCode：若MBSN的第5码为’M’，则取MBSN的第6码，否则取第7码；CheckCode为数字，则为子板，为’R’，则为RCTO
            string newMB = currentMB.Sn;
            bool checkCodeIsR = false;
            if (newMB.Substring(4, 1) == "M")
            {
                if (newMB.Substring(5, 1) == "R")
                {
                    checkCodeIsR = true;
                }
            }
            else
            {
                if (newMB.Substring(6, 1) == "R")
                {
                    checkCodeIsR = true;
                }
            }

            if (checkCodeIsR == true)
            {
                List<string> errpara = new List<string>();
                errpara.Add(currentMB.Sn);
                throw new FisException("CHK956", errpara);
            }


            return base.DoExecute(executionContext);
        }
    }
}
