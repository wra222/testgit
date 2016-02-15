/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Online Generate AST Page
* UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2012/4/9 
* UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2012/4/9            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-21   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-1619, Jessica Liu, 2012-4-9
* ITC-1413-0012, Jessica Liu, 2012-6-15
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

namespace IMES.Activity
{
    /// <summary>
    /// 检查AST是否需要打印
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Online Generate AST
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.生成序号
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    ///         Session.SessionKeys.CustSN
    ///         AssetSN
    ///         AST1
    ///         AST2
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.PrintLogBegNo
    ///         Session.SessionKeys.PrintLogEndNo
    ///         Session.SessionKeys.PrintLogName
    ///         Session.SessionKeys.PrintLogDescr
    ///         HasTwoAST
    ///         ASTInfo
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              FisBOM
    ///              NumControl
    ///              Part
    /// </para> 
    /// </remarks>
    public partial class CheckASTPrint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckASTPrint()
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
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            var Custsn = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);

            bool find = false;
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
            for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
            {
                IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                //ITC-1360-1619, Jessica Liu, 2012-4-9
                //if ((part.BOMNodeType == "AT") && ((part.Descr == "ATSN5") || (part.Descr == "ATSN7")))
                if ((part.BOMNodeType == "AT") && (part.Descr == "ATSN5"))
                {
                    find = true;
                    break;
                }
            }	

            //FisException ex;
            List<string> erpara = new List<string>();
            if (find == true)
            {
                erpara.Add(Custsn);
                throw new FisException("CHK253", erpara);   
            }
            else {
                string AssetSN = CurrentSession.GetValue("AssetSN") as string;
                string Ast1 = CurrentSession.GetValue("AST1") as string;
                string Ast2 = CurrentSession.GetValue("AST2") as string;
                bool hasTwoAST = true;
                if (string.IsNullOrEmpty(AssetSN))
                {
                    //ITC-1360-0496,ITC-1360-0498, Jessica Liu, 2012-2-28
                    if (string.IsNullOrEmpty(Ast1) && string.IsNullOrEmpty(Ast2))
                    {
                        erpara.Add(Custsn);
                        throw new FisException("CHK203", erpara); 
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Ast1) || string.IsNullOrEmpty(Ast2))
                        {
                            hasTwoAST = false;
                        }

                        if (string.IsNullOrEmpty(Ast1))
                        {
                            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenProduct.ProId);
                            //ITC-1413-0012, Jessica Liu, 2012-6-15
                            //CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, Ast2);
                            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenProduct.ProId);
                            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, Ast2);
                        }
                        else
                        {
                            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenProduct.ProId);
                            //ITC-1413-0012, Jessica Liu, 2012-6-15
                            //CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, Ast1);
                            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenProduct.ProId);
                            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, Ast1);
                        }
                    }
                }
                else {
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenProduct.ProId);
                    //ITC-1413-0012, Jessica Liu, 2012-6-15
                    //CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, AssetSN);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenProduct.ProId);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, AssetSN);

                    hasTwoAST = false;
                }
                CurrentSession.AddValue(Session.SessionKeys.PrintLogName,"AT");

                //ITC-1360-1619, Jessica Liu, 2012-4-9
                //CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ATSN3");
                //ITC-1413-0012, Jessica Liu, 2012-6-15
                //CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ATSN");

                CurrentSession.AddValue("HasTwoAST", hasTwoAST);

                //2012-5-2
                string ASTinfo = (string)CurrentSession.GetValue(Session.SessionKeys.PrintLogEndNo);
                CurrentSession.AddValue("ASTInfo", ASTinfo);
            }

            return base.DoExecute(executionContext);
        }
    }
}
