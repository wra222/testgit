// INVENTEC corporation (c)2009 all rights reserved. 
// Description: PCA Test Station 检查保存前检查，处理15种异常情况
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-13   Kaisheng                     create
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
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
namespace IMES.Activity
{
    /// <summary>
    /// PCA Test Station 当前检查站为SMD_A或SMD_B，则检查是否已经通过此站，若通过此战，则报告错误,不良品,良品保存前数据库校验
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
    ///         不良品,良品保存前数据库校验
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
    public partial class CheckInputPassForFunTestRCTO : BaseActivity
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckInputPassForFunTestRCTO()
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
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();


            bool cpuHave = false;
            cpuHave = bomRepository.CheckIfExistDoubleBomWithPart(currentProduct.Model, "CPU", "BM", "P1");
            if (true == cpuHave)
            {
                IList<ProductPart> partList = productRep.GetProductPartByPartTypeLike(currentProduct.ProId, "%CPU");
                if (partList == null || partList.Count == 0)
                {
                    throw new FisException("CHK561", new string[] { });
                }
            }

            bool mbHave = false;
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM curBOM = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
            IList<IBOMNode> bomNodeLst = curBOM.FirstLevelNodes;

            if (bomNodeLst != null && bomNodeLst.Count > 0)
            {
                foreach (IBOMNode item in bomNodeLst)
                {
                    if (!string.IsNullOrEmpty(item.Part.BOMNodeType) &&
                          item.Part.BOMNodeType.ToUpper() == "MB")
                    {
                        mbHave = true;
                        break;
                    }
                }
            }

            if (mbHave  && String.IsNullOrEmpty(currentProduct.PCBID))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(currentProduct.ProId);
                ex = new FisException("CHK400", erpara);
                throw ex;
            }


            //从Session里取得Product对象

            Model curModel = modelRep.Find(currentProduct.Model);
            if (null != curModel && null != curModel.FamilyName && curModel.FamilyName == "JOURNEY 1.0")
            {
                IList<ProductLog> allLogs = new List<ProductLog>();
                allLogs = currentProduct.ProductLogs;
                bool exist = false;
                foreach (ProductLog tempProductLog in allLogs)
                {
                    if (tempProductLog.Station == "58" && tempProductLog.Status == StationStatus.Pass)
                    {
                        exist = true;
                        break;
                    }
                }
                if (exist == false)
                {
                    //请刷Generate Customer SN
                    throw new FisException("CHK560", new string[] { });
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
