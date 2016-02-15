/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for Asset Tag Label Print Page
 * UI:CI-MES12-SPEC-PAK-UI Asset Tag Label Print.docx –2012/2/28 
 * UC:CI-MES12-SPEC-PAK-UC Asset Tag Label Print.docx –2012/2/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0857, Jesscia Liu, 2012-2-28 
* ITC-1360-1761, Jesscia Liu, 2012-4-24 
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
    /// 根据BOM查询结果，对Special_Det表进行相应修改
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         Asset Tag Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         查询BOM
    ///         满足条件则进行Special_Det表的判断修改
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
    ///         AssetSN
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
    ///         Modify Special_Det
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IBOMRepository
    ///         IHierarchicalBOM
    ///         IBOMNode
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class CheckBOMAndModifySpecialDet : BaseActivity
	{
        ///<summary>
        ///</summary>
        public CheckBOMAndModifySpecialDet()
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
            var AssetSN = (string)CurrentSession.GetValue("AssetSN");

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            bool isExist = false;

            //当BOM中存在BOMNodeType=”AT”(Asset Tag)，其对应的Part.PartType=’ATSN7’ or ‘ATSN8’时，进行表修改
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
            IList<IBOMNode> bomNodes = bom.GetNodesByNodeType("AT");          
            foreach (IBOMNode bomNode in bomNodes)
            {
                //if (bomNode.Part.Type == "ATSN7" || bomNode.Part.Type == "ATSN8")
                if (bomNode.Part.Descr == "ATSN7" || bomNode.Part.Descr == "ATSN8")
                {
                    //ITC-1360-1761, Jesscia Liu, 2012-4-24
                    if (AssetSN == "null")
                    {
                        if (bomNode.Part.Descr == "ATSN8")
                        {
                            AssetSN = bomNode.Part.PN;
                        }
                    }

                    //ITC-1360-0857, Jesscia Liu, 2012-2-28
                    //isExist = productRepository.CheckExistSpecialDet(bomNode.Part.Type, currenProduct.ProId);
                    isExist = productRepository.CheckExistSpecialDet(bomNode.Part.Descr, currenProduct.ProId);
                    
                    if (isExist == true)
                    {
                        //若存在，则Update Sno1=@astsn,Udt=getdate()
                        //productRepository.UpdateSpecialDetSno1(AssetSN, bomNode.Part.Type, currenProduct.ProId);
                        productRepository.UpdateSpecialDetSno1(AssetSN, bomNode.Part.Descr, currenProduct.ProId);
                    }
                    else
                    {
                        //否则Insert(Tp是’ATSN7’ or ‘ATSN8’)
                        SpecialDetInfo sd = new SpecialDetInfo();
                        sd.sno1 = AssetSN;
                        sd.snoId = currenProduct.ProId;
                        sd.tp = bomNode.Part.Descr;//bomNode.Part.Type;
                        productRepository.AddSpecialDetInfo(sd);
                    }
                }               
            }
           
            return base.DoExecute(executionContext);
        }
	}
}
