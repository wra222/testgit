/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for Online Generate AST Page
 * UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2012/4/6 
 * UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2012/4/6            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-4-9   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.NumControl;

namespace IMES.Activity
{
    /// <summary>
    /// 根据BOM查询结果，判断是否需要获得AST，对此设置标志量
    /// 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         Online Generate AST
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         查询BOM并根据结果设置标志量
    ///         满足条件则获取AST
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IBOMRepository
    ///         IHierarchicalBOM
    ///         IBOMNode
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class CheckGenerateAST : BaseActivity
	{
        ///<summary>
        ///</summary>
        public CheckGenerateAST()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 根据BOM查询结果，对Special_Det表进行相应修改
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
/*//test，测试流程是否通，需去掉========
return base.DoExecute(executionContext);
//test，测试流程是否通，需去掉========*/

            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            bool GenerateFlag = false;

            //当Bom中存在 BomNodeType=’AT’  and （Descr=’ATSN5’ Or Descr=’ATSN7’ Or Descr=’ATSN3’）时才需要得到Asset SN
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
            IList<IBOMNode> bomNodes = bom.GetNodesByNodeType("AT");          
            foreach (IBOMNode bomNode in bomNodes)
            {
                //2012-7-13, Jessica Liu, 新需求：去除ATSN7的产生
                //if (bomNode.Part.Descr == "ATSN3" || bomNode.Part.Descr == "ATSN5" || bomNode.Part.Descr == "ATSN7")
                if (bomNode.Part.Descr == "ATSN3" || bomNode.Part.Descr == "ATSN5")
                {
                    GenerateFlag = true;

                    //Jessica Liu, 2012-4-16
                    CurrentSession.AddValue("DESCR", bomNode.Part.Descr);
                }               
            }

            CurrentSession.AddValue("GenerateASTFlag", GenerateFlag);
           
            return base.DoExecute(executionContext);
        }
	}
}
