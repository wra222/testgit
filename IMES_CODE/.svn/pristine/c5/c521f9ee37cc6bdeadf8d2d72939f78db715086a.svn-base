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
using System.ComponentModel;
using metas=IMES.Infrastructure.Repository._Metas;
using IMES.FisObject.Common.PrintLog;
using IMES.DataModel;
using IMES.Common; 

namespace IMES.Activity
{
    /// <summary>
    /// 寫PrintLog 或 ProductLog
    /// </summary>
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
        ///  write Print Log
        /// </summary>
        public static DependencyProperty IsWritePrintLogProperty = DependencyProperty.Register("IsWritePrintLog", typeof(bool), typeof(CheckASTPrint), new PropertyMetadata(true));

        /// <summary>
        /// write Print Log
        /// </summary>
        [DescriptionAttribute("IsWritePrintLog")]
        [CategoryAttribute("IsWritePrintLog Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsWritePrintLog
        {
            get
            {
                return ((bool)(base.GetValue(IsWritePrintLogProperty)));
            }
            set
            {
                base.SetValue(IsWritePrintLogProperty, value);
            }
        }

        /// <summary>
        ///  write Product Log
        /// </summary>
        public static DependencyProperty IsWriteProductLogProperty = DependencyProperty.Register("IsWriteProductLog", typeof(bool), typeof(CheckASTPrint), new PropertyMetadata(true));

        /// <summary>
        /// write Product Log
        /// </summary>
        [DescriptionAttribute("IsWriteProductLog")]
        [CategoryAttribute("IsWriteProductLog Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsWriteProductLog
        {
            get
            {
                return ((bool)(base.GetValue(IsWriteProductLogProperty)));
            }
            set
            {
                base.SetValue(IsWriteProductLogProperty, value);
            }
        }

        /// <summary>
        /// 产生Asset SN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            
            IProduct currenProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            //var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            //var Custsn = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
            string Custsn = currenProduct.CUSTSN;
            bool genAstFlag = (string)session.GetValue(Session.SessionKeys.GenerateASTSN ) == "Y" ?true:false;
         
            bool isCDSI = ((string)session.GetValue(Session.SessionKeys.HasCDSI)) == "Y";
            bool isCNRS = ((string)session.GetValue(Session.SessionKeys.HasCNRS)) == "Y";

           
            if (genAstFlag || isCDSI || isCNRS)
            {
              
               IList<AstDefineInfo> astDefineList = utl.IsNull<IList<AstDefineInfo>>(session, Session.SessionKeys.NeedGenAstDefineList);

                //string astCode = null;   //astDefineList.FirstOrDefault().AstCode; //astPartList.FirstOrDefault().Descr;
                //AstDefineInfo astDefine = astDefineList.FirstOrDefault();
               
                if (IsWriteProductLog)
                {
                    foreach (AstDefineInfo item in astDefineList)
                    {
                        writeProductLog(session, currenProduct, IMES.FisObject.Common.Station.StationStatus.Pass, item.AstCode);
                    }
                }

                if (isCDSI || isCNRS)
                {
                    IPart part = (IPart)session.GetValue(Session.SessionKeys.CNRSPart);
                    IPart cdsiPart = (IPart)session.GetValue(Session.SessionKeys.CDSIPart);

                    if (isCDSI)
                    {
                        part = utl.IsNull< IPart>(session, Session.SessionKeys.CDSIPart);
                    }
                    else
                    {
                        part = utl.IsNull<IPart>(session, Session.SessionKeys.CNRSPart);
                       
                    }
                    AstDefineInfo astDefine = astDefineList.Where(x => x.AstType == part.BOMNodeType && x.AstCode == part.Descr).FirstOrDefault();
                    string Ast1 = session.GetValue("AST1") as string;
                    string Ast2 = session.GetValue("AST2") as string;

                    if (IsWritePrintLog && astDefine.NeedPrint == "Y")
                    {
                        if (!string.IsNullOrEmpty(Ast1))
                        {
                            writePrintLog(session, currenProduct.ProId, currenProduct.ProId, Ast1, part.Descr);
                        }

                        if (!string.IsNullOrEmpty(Ast2))
                        {
                            writePrintLog(session, currenProduct.ProId, currenProduct.ProId, Ast2, part.Descr);
                        }
                    }
                   
                }

                if (genAstFlag)
                {
                    IList<IPart> astPartList = utl.IsNull<IList<IPart>>(session, Session.SessionKeys.NeedGenAstPartList);
                    var currentAST = (string)session.GetValue("AssetSN");
                    var currentAST3 = (string)session.GetValue("AssetSN3");
                    var assigneAstDefineList = astDefineList.Where(x =>astPartList.Any(y=> x.AstType == y.BOMNodeType && x.AstCode == y.Descr)).ToList();
                    foreach (AstDefineInfo item in assigneAstDefineList)
                    {
                        if (IsWritePrintLog && item.NeedPrint == "Y")
                        {
                            if (!string.IsNullOrEmpty(currentAST))
                            {
                                writePrintLog(session, currenProduct.ProId, currenProduct.ProId, currentAST, item.AstCode);
                            }

                            if (!string.IsNullOrEmpty(currentAST3))
                            {
                                writePrintLog(session, currenProduct.ProId, currenProduct.ProId, currentAST3, item.AstCode);
                            }
                        }
                    }
                }              
            }

            #region disable code
            //if (genAstFlag)
            //{
            //    IList<IPart> astPartList = utl.IsNull<IList<IPart>>(session, Session.SessionKeys.NeedGenAstPartList);
            //    IList<AstDefineInfo> astDefineList = utl.IsNull<IList<AstDefineInfo>>(session, Session.SessionKeys.NeedGenAstDefineList);

            //    string astCode = astPartList.FirstOrDefault().Descr;
            //    AstDefineInfo  astDefine= astDefineList.FirstOrDefault(); 

            //    if (IsWriteProductLog)
            //    {
            //        writeProductLog(session, currenProduct, IMES.FisObject.Common.Station.StationStatus.Pass, astCode);
            //    }

            //    if (currenProduct.IsCDSI)
            //    { // 有可能兩個Asset Tag
            //        string Ast1 = CurrentSession.GetValue("AST1") as string;
            //        string Ast2 = CurrentSession.GetValue("AST2") as string;

            //        if (IsWritePrintLog && astDefine.NeedPrint == "Y" )
            //        {
            //            if (!string.IsNullOrEmpty(Ast1) ) 
            //            {
            //                writePrintLog(session, currenProduct.ProId, currenProduct.ProId, Ast1, astCode);                            
            //            }

            //            if (!string.IsNullOrEmpty(Ast2) )
            //            {
            //                writePrintLog(session, currenProduct.ProId, currenProduct.ProId, Ast2, astCode);                             
            //            }
            //        }                

            //    }
            //    else
            //    {
            //        var currentAST = (string)session.GetValue("AssetSN");
            //        var currentAST3 = (string)session.GetValue("AssetSN3");
            //        if (IsWritePrintLog && astDefine.NeedPrint == "Y")
            //        {
            //            if (!string.IsNullOrEmpty(currentAST) )
            //            {
            //                writePrintLog(session, currenProduct.ProId, currenProduct.ProId, currentAST, astCode);
            //            }

            //            if (!string.IsNullOrEmpty(currentAST3) )
            //            {
            //                writePrintLog(session, currenProduct.ProId, currenProduct.ProId, currentAST3, astCode);
            //            }
            //        }
            //        #region disable code
            //        //if (astCode == "ATSN5")
            //        //{
            //        //    //throw new FisException("CHK253", new List<string> {Custsn}); 
            //        //    session.AddValue(Session.SessionKeys.HasATSN5, "Y");  //不打印
            //        //}
            //        //else
            //        //{
            //        //    if (IsWritePrintLog)
            //        //    {
            //        //        if (!string.IsNullOrEmpty(currentAST) && astDefine.NeedPrint == "Y")
            //        //        {
            //        //            writePrintLog(session, currenProduct.ProId, currenProduct.ProId, currentAST, astCode);
            //        //        }

            //        //        if (!string.IsNullOrEmpty(currentAST3) && astDefine.NeedPrint == "Y")
            //        //        {
            //        //            writePrintLog(session, currenProduct.ProId, currenProduct.ProId, currentAST3, astCode);
            //        //        }
            //        //    }
            //        //}
            //        #endregion
            //    }
            //}
            #endregion
            #region disable code
            //bool find = false;
            //IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            //IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
            //for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
            //{
            //    IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
            //    //ITC-1360-1619, Jessica Liu, 2012-4-9
            //    //if ((part.BOMNodeType == "AT") && ((part.Descr == "ATSN5") || (part.Descr == "ATSN7")))
            //    if ((part.BOMNodeType == "AT") && (part.Descr == "ATSN5"))
            //    {
            //        find = true;
            //        break;
            //    }
            //}	

            ////FisException ex;
            //List<string> erpara = new List<string>();
            //if (find == true)
            //{
            //    erpara.Add(Custsn);
            //    throw new FisException("CHK253", erpara);   
            //}
            //else {
            //    string AssetSN = CurrentSession.GetValue("AssetSN") as string;
            //    string Ast1 = CurrentSession.GetValue("AST1") as string;
            //    string Ast2 = CurrentSession.GetValue("AST2") as string;
            //    bool hasTwoAST = true;
            //    if (string.IsNullOrEmpty(AssetSN))
            //    {
            //        //ITC-1360-0496,ITC-1360-0498, Jessica Liu, 2012-2-28
            //        if (string.IsNullOrEmpty(Ast1) && string.IsNullOrEmpty(Ast2))
            //        {
            //            erpara.Add(Custsn);
            //            throw new FisException("CHK203", erpara); 
            //        }
            //        else
            //        {
            //            if (string.IsNullOrEmpty(Ast1) || string.IsNullOrEmpty(Ast2))
            //            {
            //                hasTwoAST = false;
            //            }

            //            if (string.IsNullOrEmpty(Ast1))
            //            {
            //                CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenProduct.ProId);
            //                //ITC-1413-0012, Jessica Liu, 2012-6-15
            //                //CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, Ast2);
            //                CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenProduct.ProId);
            //                CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, Ast2);
            //            }
            //            else
            //            {
            //                CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenProduct.ProId);
            //                //ITC-1413-0012, Jessica Liu, 2012-6-15
            //                //CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, Ast1);
            //                CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenProduct.ProId);
            //                CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, Ast1);
            //            }
            //        }
            //    }
            //    else {
            //        CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenProduct.ProId);
            //        //ITC-1413-0012, Jessica Liu, 2012-6-15
            //        //CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, AssetSN);
            //        CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenProduct.ProId);
            //        CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, AssetSN);

            //        hasTwoAST = false;
            //    }
            //    CurrentSession.AddValue(Session.SessionKeys.PrintLogName,"AT");

            //    //ITC-1360-1619, Jessica Liu, 2012-4-9
            //    //CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ATSN3");
            //    //ITC-1413-0012, Jessica Liu, 2012-6-15
            //    //CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ATSN");

            //    CurrentSession.AddValue("HasTwoAST", hasTwoAST);

            //    //2012-5-2
            //    string ASTinfo = (string)CurrentSession.GetValue(Session.SessionKeys.PrintLogEndNo);
            //    CurrentSession.AddValue("ASTInfo", ASTinfo);
            //}
            #endregion

            return base.DoExecute(executionContext);
        }

        private void writePrintLog(Session session, string beginNo, string endNo, string descr, string templateName)
        {
            var printRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            var item = new PrintLog
            {
                Name = "AT",
                BeginNo = beginNo,
                EndNo = endNo,
                Descr = descr,
                Station = this.Station,
                LabelTemplate = templateName,                 
                Cdt = DateTime.Now,
                Editor = this.Editor
            };

            printRep.Add(item, session.UnitOfWork);
        }

        private void writeProductLog(Session session,IProduct product, IMES.FisObject.Common.Station.StationStatus status,  string line)
        {
            var prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var productLog = new ProductLog
            {
                Model = product.Model,
                Status = status,
                Editor = this.Editor,
                Line = line,
                Station = this.Station,
                Cdt = DateTime.Now
            };

            product.AddLog(productLog);
            prodRep.Update(product, session.UnitOfWork);
        }
    }
}
