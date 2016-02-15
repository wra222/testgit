using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Misc;
using IMES.DataModel;
using Metas = IMES.Infrastructure.Repository._Metas;
using System.ComponentModel;
using IMES.Infrastructure.Extend;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// Get AstDefine Information
    /// </summary>
    public partial class GetAstDefineInfo : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetAstDefineInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  CheckCombinedProductPart
        /// </summary>
        public static DependencyProperty CheckCombinedProductPartProperty = DependencyProperty.Register("CheckCombinedProductPart", typeof(bool), typeof(GetAstDefineInfo), new PropertyMetadata(true));

        /// <summary>
        /// write Print Log
        /// </summary>
        [DescriptionAttribute("CheckCombinedProductPart")]
        [CategoryAttribute("GetAstDefineInfo Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool CheckCombinedProductPart
        {
            get
            {
                return ((bool)(base.GetValue(CheckCombinedProductPartProperty)));
            }
            set
            {
                base.SetValue(CheckCombinedProductPartProperty, value);
            }
        }

        /// <summary>
        ///  FilterPassProductLog
        /// </summary>
        public static DependencyProperty FilterPassProductLogProperty = DependencyProperty.Register("FilterPassProductLog", typeof(bool), typeof(GetAstDefineInfo), new PropertyMetadata(true));

        /// <summary>
        /// FilterPassProductLog
        /// </summary>
        [DescriptionAttribute("FilterPassProductLog")]
        [CategoryAttribute("GetAstDefineInfo Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool FilterPassProductLog
        {
            get
            {
                return ((bool)(base.GetValue(FilterPassProductLogProperty)));
            }
            set
            {
                base.SetValue(FilterPassProductLogProperty, value);
            }
        }

        /// <summary>
        ///  Need Throw Error
        /// </summary>
        public static DependencyProperty NeedThrowErrorProperty = DependencyProperty.Register("NeedThrowError", typeof(bool), typeof(GetAstDefineInfo), new PropertyMetadata(true));

        /// <summary>
        /// NeedThrowError
        /// </summary>
        [DescriptionAttribute("NeedThrowError")]
        [CategoryAttribute("NeedThrowError Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NeedThrowError
        {
            get
            {
                return ((bool)(base.GetValue(NeedThrowErrorProperty)));
            }
            set
            {
                base.SetValue(NeedThrowErrorProperty, value);
            }
        }

        /// <summary>
        /// Get AstDefine Information
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
           
            IProduct prod = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);

            var prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(prod.Model);
            IList<IBOMNode> bomNode = bom.FirstLevelNodes;

            List<string> erpara = new List<string>();

            IList<AstDefineInfo> astDefineInfoList = miscRep.GetData<Metas.AstDefine, AstDefineInfo>(null);
            if (astDefineInfoList == null || astDefineInfoList.Count == 0)
            {
                throw new FisException("CQCHK1085", erpara);
            }            

             #region 檢查 BOM 是否有BomNodeType 為 AT及 PP類  

             if (!bomNode.Any(x => astDefineInfoList.Any(y => y.AstType == x.Part.BOMNodeType)))
             {                
                 DecideThrowError(session, new FisException("CHK205", erpara));  // 此机器不需要Online Generate AST!
                 return base.DoExecute(executionContext);                
             }
            #endregion 

             #region 檢查BOM跟AstDefine 是否有需要打印的資產標籤
             var combineAstDefineList = astDefineInfoList.Where(x => utl.UPS.DecideCombineStation(prod, x) == this.Station).ToList();
             if (combineAstDefineList == null || combineAstDefineList.Count == 0)
             {
                 throw  new FisException("CQCHK1087", new List<string> { prod.ProId, this.Station });   // 此机器不需要print asset Label !
                       
             }

             var needCombineAstList = combineAstDefineList.Join(bomNode, 
                                                                                                x => x.AstType+x.AstCode, 
                                                                                                y => y.Part.BOMNodeType+y.Part.Descr, 
                                                                                                (x1, y1) => new { AstDefine = x1, AstPart = y1.Part }).ToList();
             if (needCombineAstList.Count == 0)
             {
                 DecideThrowError(session, new FisException("CQCHK1087", new List<string> { prod.ProId, this.Station }));   // 此机器不需要print asset Label !
                 return base.DoExecute(executionContext);                  
             }

             #endregion

             #region 檢查此站需產生序號並且排除已結合序號的機器
             IList<IProductPart> productPartList = prod.ProductParts;
             if (this.CheckCombinedProductPart && 
                 productPartList != null  && 
                 productPartList.Count>0)
             {
                 var needBindPartNoList = needCombineAstList.Where(x => x.AstDefine.NeedScanSN=="Y"  || 
                                                                                                           (x.AstDefine.NeedAssignAstSN == "Y" &&
                                                                                                           utl.UPS.DecideAssignAstStation(prod, x.AstDefine) == this.Station)).ToList();

                 
                 if ( needBindPartNoList.Count>0)
                 {
                     var bindedAstSNDefineList = needBindPartNoList.Where(x=>productPartList.Any(y => y.PartType == x.AstDefine.AstCode &&
                                                                                                                                              y.PartID == x.AstPart.PN))
                                                                                      .Select(z=> z.AstDefine).ToList();
                     if (bindedAstSNDefineList.Count > 0)
                     {
                         if (needCombineAstList.Count == bindedAstSNDefineList.Count)
                         {
                             DecideThrowError(session, new FisException("CHK242", new string[] { string.Join(",", needBindPartNoList.Select(x=>x.AstDefine.AstType+ "/"+ x.AstDefine.AstCode).ToArray()) }));
                            return base.DoExecute(executionContext);      
                            
                         }
                         //過濾以結合Ast SN
                         needCombineAstList = needCombineAstList.Where(x => !bindedAstSNDefineList.Any(y => y.AstCode == x.AstDefine.AstCode && y.AstType == x.AstDefine.AstType)).ToList();
                         if (needCombineAstList.Count == 0)
                         {
                             DecideThrowError(session, new FisException("CHK242", new string[] { string.Join(",", needBindPartNoList.Select(x => x.AstDefine.AstType + "/" + x.AstDefine.AstCode).ToArray()) }));
                             return base.DoExecute(executionContext); 
                         }
                     }
                     //bool allBinded = needBindPartNoList.All(x => productPartList.Any(y => y.PartID == x));
                     //if (allBinded)
                     //{
                     //    throw new FisException("CHK242", new string[] { string.Join(",", needBindPartNoList.ToArray()) });
                     //}
                 }
             }
            #endregion                

             #region 檢查此站非產生序號及非刷入序號的資產標籤機器是否有過此站記錄
             IList<ProductLog> logList = prod.ProductLogs;
             if (this.FilterPassProductLog &&
                 logList != null &&
                 logList.Count > 0)
             {
                 //var passedAstList = (from p in logList
                 //                     join c in needCombineAstList on p.Station + p.Line equals c.AstDefine.CombineStation + c.AstDefine.AstCode
                 //                     select c.AstDefine).ToList();
                 //排除重流機器,只檢查非分配Ast SN 機器的 ProductLog
                 var passedAstList = needCombineAstList.Where(x => x.AstDefine.NeedAssignAstSN == "N" &&
                                                                                                  x.AstDefine.NeedScanSN == "N"  &&
                                                                                                  logList.Any(y => y.Station == utl.UPS.DecideCombineStation(prod, x.AstDefine) &&
                                                                                                                         y.Line == x.AstDefine.AstCode))
                                                                              .ToList();
                 if (passedAstList.Count == needCombineAstList.Count)
                 {
                     //throw 此機器%1在%2站需打印的資產標籤都已打印過站,請至%2站重印
                     throw new FisException("CQCHK1086", new List<string> { prod.ProId, this.Station });
                 }

                 if (passedAstList.Count > 0)
                 {
                     //過濾已打印
                     needCombineAstList = needCombineAstList.Where(x => !passedAstList.Any(y => y.AstDefine.AstCode == x.AstDefine.AstCode)).ToList();
                 }

                 if (needCombineAstList.Count == 0)
                 {
                     //throw 此機器%1在%2站需打印的資產標籤都已打印過站,請至%2站重印
                     throw new FisException("CQCHK1086", new List<string> { prod.ProId, this.Station });
                 }
             }


             #endregion

            #region 檢查非此站產生SN, 需先綁定產生AST SN才能打並排除未綁定
            
            var checkAssignedAstSn = needCombineAstList.Where(x =>
            {
                if (this.CheckCombinedProductPart)
                {
                    return x.AstDefine.NeedAssignAstSN == "Y" &&
                            utl.UPS.DecideAssignAstStation(prod, x.AstDefine) != this.Station &&
                            x.AstDefine.NeedPrint=="Y";
                }
                else //for reprint case
                {
                    return (x.AstDefine.NeedAssignAstSN == "Y" ||
                               x.AstDefine.NeedScanSN=="Y") && 
                               x.AstDefine.NeedPrint=="Y";
                }
            }).ToList();

            if (checkAssignedAstSn != null && 
                checkAssignedAstSn.Count > 0)
            {
                if (productPartList != null && productPartList.Count > 0)
                {
                    var NotAssignedAstSn = checkAssignedAstSn.Where(x => !productPartList.Any(y => x.AstPart.Descr == y.PartType &&
                                                                                                    x.AstPart.BOMNodeType == y.BomNodeType)).ToList();
                    if (NotAssignedAstSn != null &&
                        NotAssignedAstSn.Count() > 0)
                    {
                        if (NotAssignedAstSn.Count() == needCombineAstList.Count())
                        {
                            throw new FisException("CQCHK1088", new List<string> { prod.ProId, string.Join(",", checkAssignedAstSn.Select(x => x.AstPart.Descr).ToArray()) });
                        }
                        else  //剔除未結合
                        {
                            needCombineAstList = needCombineAstList.Where(x => !NotAssignedAstSn.Any(y => y.AstDefine.AstType == x.AstDefine.AstType &&
                                                                                                                                                            y.AstDefine.AstCode == x.AstDefine.AstCode)).ToList();
                            if (needCombineAstList == null || needCombineAstList.Count == 0)
                            {
                                throw new FisException("CQCHK1088", new List<string> { prod.ProId, string.Join(",", checkAssignedAstSn.Select(x => x.AstPart.Descr).ToArray()) });
                            }
                        }
                    }
                }
                else  //都未結合 Product_Part
                {

                    if (checkAssignedAstSn.Count() == needCombineAstList.Count())
                    {
                        throw new FisException("CQCHK1088", new List<string> { prod.ProId, string.Join(",", checkAssignedAstSn.Select(x => x.AstPart.Descr).ToArray()) });
                    }
                    else  //剔除未結合
                    {
                        needCombineAstList = needCombineAstList.Where(x => !checkAssignedAstSn.Any(y => y.AstDefine.AstType == x.AstDefine.AstType &&
                                                                                                                                                        y.AstDefine.AstCode == x.AstDefine.AstCode)).ToList();
                        if (needCombineAstList == null || needCombineAstList.Count == 0)
                        {
                            throw new FisException("CQCHK1088", new List<string> { prod.ProId, string.Join(",", checkAssignedAstSn.Select(x => x.AstPart.Descr).ToArray()) });
                        }
                    }
                }
            } 
            #endregion

            #region 檢查未在此站產生資產標籤序號是否有過此站記錄機器及過濾過站記錄
            if (this.FilterPassProductLog &&
                logList != null &&
                logList.Count > 0)
            {
                //var passedAstList = (from p in logList
                //                     join c in needCombineAstList on p.Station + p.Line equals c.AstDefine.CombineStation + c.AstDefine.AstCode
                //                     select c.AstDefine).ToList();
                //排除重流機器,只檢查非分配Ast SN 機器的 ProductLog
                var passedAstList = needCombineAstList.Where(x => x.AstDefine.NeedAssignAstSN == "Y" &&
                                                                                                 utl.UPS.DecideAssignAstStation(prod, x.AstDefine) != this.Station &&
                                                                                                 logList.Any(y => y.Station == utl.UPS.DecideCombineStation(prod, x.AstDefine) &&
                                                                                                                        y.Line == x.AstDefine.AstCode))
                                                                             .ToList();
                if (passedAstList.Count == needCombineAstList.Count)
                {
                    //throw 此機器%1在%2站需打印的資產標籤都已打印過站,請至%2站重印
                    throw new FisException("CQCHK1086", new List<string> { prod.ProId, this.Station });
                }

                if (passedAstList.Count > 0)
                {
                    //過濾已打印
                    needCombineAstList = needCombineAstList.Where(x => !passedAstList.Any(y => y.AstDefine.AstCode == x.AstDefine.AstCode)).ToList();
                }

                if (needCombineAstList.Count == 0)
                {
                    //throw 此機器%1在%2站需打印的資產標籤都已打印過站,請至%2站重印
                    throw new FisException("CQCHK1086", new List<string> { prod.ProId, this.Station });
                }
            }


            #endregion

           
            IList<IPart> needCombineAstPartList = needCombineAstList.Select(x => x.AstPart).ToList();
            IList<AstDefineInfo> needCombineAstDefineList = needCombineAstList.Select(x => x.AstDefine).ToList();
            IList<string> imageUrlList = new List<string>();
            IList<string> esopastlist = utl.ConstValueType("ATESOPPN", "").Select(x => x.value).ToList();
            IList<IPart> needCombineAstPartListimage = needCombineAstList.Where(p => esopastlist.Any(y => y == p.AstDefine.AstCode)).Select(x => x.AstPart).ToList();

            #region Get SOP JPG File Name
            bool hasMN2 = true;  //0=no error; 1=MN2错误; 2=No AST           
            IList<string> PNList = new List<string>();
            string strMN2 = prod.GetModelProperty("MN2") as string;
            if (string.IsNullOrEmpty(strMN2))
            {
                hasMN2 = false;
            }

            if (hasMN2)
            {
                var pnList = needCombineAstPartListimage.Select(x => x.PN).Distinct().OrderBy(y => y).ToList();
                string imageUr = strMN2;
                foreach (string pn in pnList)
                {
                    imageUr =imageUr+pn;
                }
                imageUrlList.Add(imageUr); 
            }

            #endregion

            bool hasGenAst = false;
            hasGenAst = needCombineAstList.Any(x => x.AstDefine.NeedAssignAstSN == "Y" &&
                                                                              utl.UPS.DecideAssignAstStation(prod, x.AstDefine) == this.Station);

            session.AddValue( Session.SessionKeys.NeedCombineAstDefineList, needCombineAstDefineList);
            //session.AddValue("NeedCombineAstCodeList", needCombineAstDefineList.Select(x => x.AstCode).ToList());
            session.AddValue(Session.SessionKeys.NeedCombineAstPartList, needCombineAstPartList);
            //if (needCombineAstDefineList.Any(x => x.AstLocation.ToLower() == "shipping"))
            //{
            //    session.AddValue("HasShippingAst", "Y");
            //}
            //session.AddValue("GenerateASTFlag", hasGenAst);
            //session.AddValue(Session.SessionKeys.GenerateASTSN, hasGenAst ? "Y" : "N");
            if (hasGenAst)
            {
                //session.AddValue("DESCR", needCombineAstList.Where(y => y.AstDefine.NeedAssignAstSN == "Y" && y.AstDefine.AssignAstSNStation== this.Station)
                //                                                                          .Select(x => x.AstDefine.AstCode).FirstOrDefault());
                //session.AddValue(Session.SessionKeys.NeedGenAstPartList, needCombineAstList.Where(y => y.AstDefine.NeedAssignAstSN == "Y" &&
                //                                                                                                                                                utl.DecideAssignAstStation(prod, y.AstDefine) == this.Station)
                //                                                                                                                             .Select(x => x.AstPart).ToList());
                //session.AddValue(Session.SessionKeys.NeedGenAstDefineList, needCombineAstList.Where(y => y.AstDefine.NeedAssignAstSN == "Y" &&
                //                                                                                                                                                utl.DecideAssignAstStation(prod, y.AstDefine) == this.Station)
                //                                                                                                                             .Select(x => x.AstDefine).ToList());

                IList<IPart> needGenAstPartList = needCombineAstList.Where(y => y.AstDefine.NeedAssignAstSN == "Y" &&
                                                                                                                                                                utl.UPS.DecideAssignAstStation(prod, y.AstDefine) == this.Station)
                                                                                                                                             .Select(x => x.AstPart).ToList();
                IList<AstDefineInfo> needGenAstDefineList = needCombineAstList.Where(y => y.AstDefine.NeedAssignAstSN == "Y" &&
                                                                                                                                                                utl.UPS.DecideAssignAstStation(prod, y.AstDefine) == this.Station)
                                                                                                                                             .Select(x => x.AstDefine).ToList();
                utl.UPS.checkGenAstDefineAndPart(session, prod, needGenAstDefineList, needGenAstPartList, this.Station);

            }
           

            if (needCombineAstDefineList.Any(x => x.HasUPSAst == "Y"))
            {
               // session.AddValue(Session.SessionKeys.IsUPSDevice, prod.IsUPSDevice ? "Y" : "N");
                session.AddValue(Session.SessionKeys.IsUPSDevice, "Y");//mantis 0001638
            }
            else
            {
                session.AddValue(Session.SessionKeys.IsUPSDevice, "N");
            }
            
            //session.AddValue(Session.SessionKeys.HasCDSI, prod.IsCDSI ?"Y":"N");
            session.AddValue(Session.SessionKeys.HasMN2, hasMN2 ? "Y" : "N");
            session.AddValue(Session.SessionKeys.ImageFileList, imageUrlList);

            return base.DoExecute(executionContext);
        }        

       
        private void DecideThrowError(Session session,  FisException ex)
        {           
            if (this.NeedThrowError)
            {
                throw ex;   
            }
            else
            {
                session.AddValue(ExtendSession.SessionKeys.WarningMsg, ex.mErrmsg);                
            }           
        }
    }
}
