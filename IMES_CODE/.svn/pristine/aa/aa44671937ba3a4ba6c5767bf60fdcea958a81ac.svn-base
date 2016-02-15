/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Combine AST Page
* UI:CI-MES12-SPEC-FA-UI Combine AST .docx –2011/12/5 
* UC:CI-MES12-SPEC-FA-UC Combine AST .docx –2011/12/5            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-2   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-1649, Jessica Liu, 2012-4-10
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM; 
using IMES.FisObject.Common.Part;   
using System.Linq;  


namespace IMES.Activity
{
    /// <summary>
    /// Combine AST的卡站处理
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         Combine AST
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         Ast Length输入与否判断
    ///         检查是否需要检查
    ///         检查是否结合资产标签
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK204
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         ProdidOrCustsn
    ///         ASTLength
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         AST 或者 空（未结合资产标签时）
    ///         Session.SessionKeys.Product
    ///         Part
    ///         ErrorFlag
    ///         ImageURL
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class CombineASTBlockStation : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CombineASTBlockStation()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Combine AST的卡站处理
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            var ProdidOrCustsn = (string)CurrentSession.GetValue("ProdidOrCustsn");

 
            //检查是否需要检查 通过Model 检查 bom中BomNodeType=’AT’  Descr=’ATSN1’ 判断是否有存在ATSN1，如都不存在,提示“此机器不需要结合资产标签”，清空刷入的Prodid,不做任何后续处理
            bool isExist = false;   //判断是否存在ATSN1
            IList<string> ATPNList = new List<string>();
        
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
            for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
            {
                IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                if ((part.BOMNodeType == "AT") && (part.Descr == "ATSN1"))
                {
                    isExist = true;

                    ATPNList.Add(part.PN);  //插入AT PN列表

                    break;
                }
            }

            /* 2012-7-16, Jessica Liu, 新需求：增加非AT1的提示
            if (isExist == false)
            {
                List<string> errpara = new List<string>();

                errpara.Add(ProdidOrCustsn);

                throw new FisException("CHK204", errpara);  
            }
            */
            //检查Model直接下阶是否带BomNodeType=’PP’的料，若存在，则报错：“请去Online Generate AST打印PP类标签”
            //检查Model直接下阶是否带BomNodeType=’AT’ and Descr = ‘ATSN3’的料，若存在，则报错：“请去Online Genrate AST打印ATSN3标签”
            //检查Model直接下阶是否带BomNodeType=’AT’ and Descr = ‘ATSN5’的料，若存在，则报错：“请去Online Genrate AST打印ATSN5标签”
            //若以上数据都不存在，则提示“此机器不需要结合资产标签”，清空刷入的Prodid,不做任何后续处理
            if (isExist == false)
            {
                bool isPPExist = false;
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    IPart temppart1 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                    if (temppart1.BOMNodeType == "PP")
                    {
                        isPPExist = true;
                        break;
                    }
                }
                bool isATSN3Exist = false;
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    IPart temppart2 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                    if ((temppart2.BOMNodeType == "AT") && (temppart2.Descr == "ATSN3"))
                    {
                        isATSN3Exist = true;
                        break;
                    }
                }
                bool isATSN5Exist = false;
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    IPart temppart3 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                    if ((temppart3.BOMNodeType == "AT") && (temppart3.Descr == "ATSN5"))
                    {
                        isATSN5Exist = true;
                        break;
                    }
                }
            
                if (isPPExist == true)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(ProdidOrCustsn);
                    throw new FisException("CHK523", errpara);  //“请去Online Generate AST打印PP类标签”
                }
                else if (isATSN3Exist == true)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(ProdidOrCustsn);
                    throw new FisException("CHK914", errpara);  //“请去Online Genrate AST打印ATSN3标签”
                }
                else if (isATSN5Exist == true)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(ProdidOrCustsn);
                    throw new FisException("CHK915", errpara);  //“请去Online Genrate AST打印ATSN5标签”
                }
                else 
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(ProdidOrCustsn);
                    throw new FisException("CHK204", errpara);  //“此机器不需要结合资产标签”
                }
            }
            
            
            string AST = "";
            /* 2012-9-4, Jessica Liu, UC需求变更
            IProductPart tempPart = null;
            foreach (IProductPart tempProductPart in currentProduct.ProductParts)
            {
                foreach (string pn in ATPNList)
                {
				    if (tempProductPart.PartID == pn)
				    {
                        AST = tempProductPart.PartSn;
                        tempPart = tempProductPart;
                        break;  
				    }
                }
            }
            */
            //Merge Code to Sku Kernel
            bool hasInfo = bomRep.CheckIfExistProductPartWithBomForRCTO("ATSN1", currentProduct.Model, currentProduct.ProId);
            if (hasInfo == false)
            {
                AST = "ASK";
            }


            //2012-4-10, for image
            int errorFlag = 0;  //0=no error; 1=MN2错误; 2=No AST
            string imageUrl = "";
            IList<string> PNList = new List<string>();
            string strMN2 = currentProduct.GetModelProperty("MN2") as string;
            if (string.IsNullOrEmpty(strMN2))
            {
                errorFlag = 1;
            }

            if (errorFlag == 0)
            {
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    IPart part2 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                    if ((part2.BOMNodeType == "PP") || ((part2.BOMNodeType == "AT") && ((part2.Descr == "ATSN1") || (part2.Descr == "ATSN2" || (part2.Descr == "ATSN3")))))
                    {
                        PNList.Add(part2.PN);
                    }
                }

                if ((PNList != null) || (PNList.Count == 0))
                {
                    for (int i = 1; i < PNList.Count; i++)
                    {
                        string t = PNList[i];
                        int j = i;
                        while ((j > 0) && string.Compare(PNList[j - 1], t, false) > 0)
                        {
                            PNList[j] = PNList[j - 1];
                            --j;
                        }

                        PNList[j] = t;
                    }
                }
                else
                {
                    errorFlag = 2;
                }

                imageUrl += strMN2;

                foreach (string pn2 in PNList)
                {
                    imageUrl += pn2;
                }
            }

            CurrentSession.AddValue("AST", AST);
            //2012-9-4, Jessica Liu, UC需求变更
            //CurrentSession.AddValue("Part", tempPart);

            //ITC-1360-1649, Jessica Liu, 2012-4-10
            CurrentSession.AddValue("ErrorFlag", errorFlag.ToString());
            CurrentSession.AddValue("ImageURL", imageUrl);

            return base.DoExecute(executionContext);
        }
	}
}
