/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Generate Customer SN For Docking Page
* UI:CI-MES12-SPEC-FA-UI Generate Customer SN For Docking.docx –2012/5/18 
* UC:CI-MES12-SPEC-FA-UC Generate Customer SN For Docking.docx –2012/5/18           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
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
using IMES.FisObject.Common.Line;

namespace IMES.Activity
{
    /// <summary>
    /// 检查Product是否有未修护的记录
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Generate Customer SN For Docking
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.生成序号
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK206
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
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
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class CheckProductForRegenerateCustSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckProductForRegenerateCustSN()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查Product是否有未修护的记录
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
            List<string> errpara = new List<string>();

            if (currentProduct.Status == null) {
                errpara.Add(currentProduct.ProId);
                throw new FisException("SFC002", errpara);
            }
            if(currentProduct.Status.Status == 0)
            {
                //Product：XXX处在Fail状态，请先去修护
                errpara.Add(currentProduct.ProId);
                throw new FisException("CHK920", errpara);
              
            }
            if (currentProduct.Repairs.Count > 0)
            {
                if (currentProduct.Repairs[0].Status == 0)
                {
                    //Product：XXX已在修护区，请先Key出
                    errpara.Add(currentProduct.ProId);
                    throw new FisException("CHK921", errpara);
                }
            }
            if (currentProduct.CUSTSN == "" || currentProduct.CUSTSN== null)
            {
                //Product：XXX还未产生过Customer SN
                errpara.Add(currentProduct.ProId);
                throw new FisException("CHK922", errpara);
            }

            if (currentProduct.Status.StationId == "73")
            {
                //Product：XXX抽中EPIA，请先刷入Q区
                errpara.Add(currentProduct.ProId);
                throw new FisException("CHK923", errpara);
            }

            Line line = lineRepository.Find(currentProduct.Status.Line);
            CurrentSession.AddValue(Session.SessionKeys.LineCode, line.Descr);

            return base.DoExecute(executionContext);
        }
    }
}
