// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-08-01   xuyunfeng                    create
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace IMES.Activity
{
    /// <summary>
    /// POSTEL label的列印条件及方法
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
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
    ///         Session.Product
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
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class CheckPostelLabelPrint : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckPostelLabelPrint()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check RMN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueInfo> cvInfo = new List<ConstValueInfo>();
            
            ConstValueInfo cvCond = new ConstValueInfo();
            cvCond.type = "PD PA Label 1";
            //cvCond.name = product.Status.Line.Substring(0, 1);
            cvInfo = partRepository.GetConstValueInfoList(cvCond);

            //CurrentSession.AddValue("POSTEL Label", "");
            IList<IBOMNode> bomNodeList = (IList<IBOMNode>)CurrentSession.GetValue(Session.SessionKeys.SessionBom);
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            string pno = string.Empty;
            pno = curProduct.Model;
            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(pno);
            IList<IBOMNode> bomList = curBom.FirstLevelNodes;
            List<String> strLabel = new List<string>();
            //Check ModelBOM 中Model 直接下阶的所有Descr =Postel Label
            //bool isPostel = false;
            //CurrentSession.AddValue("POSTEL label", "");
            foreach (IBOMNode node in bomList)
            {
                foreach (ConstValueInfo tmp in cvInfo)
                {
                    if (tmp.value.ToUpper() == node.Part.Descr.ToUpper())
                    {
                        strLabel.Add(tmp.name);
                    }
                }
            }
            //foreach (string s in strLabel)
            //{
            //    CurrentSession.AddValue(s, s);
            //}
            CurrentSession.AddValue("PDPA1LabelType", strLabel);

            return base.DoExecute(executionContext);
        }
	}
}
