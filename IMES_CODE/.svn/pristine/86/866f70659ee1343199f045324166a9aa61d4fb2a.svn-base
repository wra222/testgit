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
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// Get Generate AST SN AstDefine Information
    /// </summary>
    public partial class GetGenAstSNDefineInfo : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetGenAstSNDefineInfo()
        {
            InitializeComponent();
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

            session.AddValue(Session.SessionKeys.GenerateASTSN,  "N");

             #region 檢查 BOM 為 AT及 PP類 Descr=ATSN1,2,3,5,7,8 

             if (!bomNode.Any(x => astDefineInfoList.Any(y => y.AstType == x.Part.BOMNodeType)))
             {
                 //throw new FisException("CHK205", erpara);   // 此机器不需要Online Generate AST!
                 return base.DoExecute(executionContext);
             }
            #endregion  
            
             bool IsUPSDevice = prod.IsUPSDevice;

             #region 檢查BOM跟AstDefine 是否有需的產生資產標籤序號
             var combineAstDefineList = astDefineInfoList.Where(x => x.NeedAssignAstSN=="Y" &&
                                                                                                    utl.UPS.DecideAssignAstStation(prod, x) == this.Station).ToList();
             if (combineAstDefineList == null || combineAstDefineList.Count == 0)
             {
                 //throw new FisException("CQCHK1087", new List<string> { prod.ProId, this.Station });   // 此机器不需要print asset Label !
                 return base.DoExecute(executionContext);
             }

             var needCombineAstList = combineAstDefineList.Join(bomNode, 
                                                                                                x => x.AstType+x.AstCode, 
                                                                                                y => y.Part.BOMNodeType+y.Part.Descr, 
                                                                                                (x1, y1) => new { AstDefine = x1, AstPart = y1.Part }).ToList();
             if (needCombineAstList.Count == 0)
             {
                 //throw new FisException("'CQCHK1087'", new List<string> { prod.ProId, this.Station });   // 此机器不需要print asset Label !
                 return base.DoExecute(executionContext);
             }

             #endregion

             #region 檢查結合序號
             IList<IProductPart> productPartList = prod.ProductParts;
             if (productPartList != null  && 
                 productPartList.Count>0)
             {                 
                 var bindedAstSNDefineList = needCombineAstList.Where(x => productPartList.Any(y => y.PartType == x.AstDefine.AstCode &&
                                                                                                                                          y.PartID == x.AstPart.PN)).ToList();
                                                                                 
                 if (bindedAstSNDefineList.Count > 0)
                 {
                     if (needCombineAstList.Count == bindedAstSNDefineList.Count)  //全部都分配SN
                     {
                         //throw new FisException("CHK242", new string[] { string.Join(",", needBindPartNoList.Select(x=>x.AstDefine.AstType+ "/"+ x.AstDefine.AstCode).ToArray()) });
                         return base.DoExecute(executionContext);
                     }
                     //過濾以結合Ast SN
                     needCombineAstList = needCombineAstList.Where(x => !bindedAstSNDefineList.Any(y => y.AstDefine.AstCode == x.AstDefine.AstCode && 
                                                                                                                                                            y.AstDefine.AstType == x.AstDefine.AstType)).ToList();
                     if (needCombineAstList.Count == 0)
                     {
                         //throw new FisException("CHK242", new string[] { string.Join(",", needBindPartNoList.Select(x => x.AstDefine.AstType + "/" + x.AstDefine.AstCode).ToArray()) });
                         return base.DoExecute(executionContext);
                     }
                 }                  
             }
            #endregion

             if (needCombineAstList.Count > 0)
             {               
                 IList<IPart> needGenAstPartList = needCombineAstList.Select(x => x.AstPart).ToList();
                 IList<AstDefineInfo> needGenAstDefineList = needCombineAstList.Select(x => x.AstDefine).ToList();

                 utl.UPS.checkGenAstDefineAndPart(session, prod, needGenAstDefineList, needGenAstPartList,this.Station);

                 #region disable code and move code to utl
                 //session.AddValue(Session.SessionKeys.HasCDSI, "N");
                 //session.AddValue(Session.SessionKeys.HasCNRS, "N");
                 //if (prod.IsCDSI)
                 //{
                 //    #region 檢查CDSI 機器及過濾AV料號
                 //    string atsnav = (string)prod.GetModelProperty("ATSNAV");
                 //    if (!string.IsNullOrEmpty(atsnav))  //CDSI case
                 //    {
                 //        IPart cdsiPart = null;
                 //        if (needGenAstPartList.Count == 1)
                 //        {
                 //            cdsiPart = needCombineAstList[0].AstPart;
                 //        }
                 //        else
                 //        {
                 //            cdsiPart = needGenAstPartList.Where(x => x.Attributes.Any(y => y.InfoType == "AV" && y.InfoValue == atsnav))
                 //                                                           .FirstOrDefault();
                 //        }
                 //        if (cdsiPart == null)
                 //        {
                 //            throw new FisException("CHK522", new string[] { prod.ProId });
                 //        }

                 //        //過濾CDSI Part No & AstDefine
                 //        needGenAstPartList = needGenAstPartList.Where(x => x.BOMNodeType != cdsiPart.BOMNodeType || x.Descr != cdsiPart.Descr).ToList();
                 //        needGenAstDefineList = needGenAstDefineList.Where(x => needGenAstPartList.Any(y => y.BOMNodeType == x.AstType && y.Descr == x.AstCode)).ToList();

                 //        session.AddValue(Session.SessionKeys.HasCDSI, "Y");
                 //        session.AddValue(Session.SessionKeys.AVPartNo, atsnav);
                 //        session.AddValue(Session.SessionKeys.CDSIPart, cdsiPart);
                 //    }
                 //    else //CNRS case
                 //    {
                 //        IPart cnrsPart = null;
                 //        if (needGenAstPartList.Count == 1)
                 //        {
                 //            cnrsPart = needGenAstPartList[0];
                 //        }
                 //        else
                 //        {
                 //            cnrsPart = needGenAstPartList.Where(x => x.BOMNodeType == "AT" && x.Descr == "ATSN3").FirstOrDefault();
                 //        }
                 //        if (cnrsPart == null)
                 //        {
                 //            throw new FisException("CQCHK1092", new string[] { prod.ProId, "ATSN3" });
                 //        }

                 //        //過濾CNRS Part No & AstDefine
                 //        needGenAstPartList = needGenAstPartList.Where(x => x.BOMNodeType != cnrsPart.BOMNodeType || x.Descr != cnrsPart.Descr).ToList();
                 //        needGenAstDefineList = needGenAstDefineList.Where(x => needGenAstPartList.Any(y => y.BOMNodeType == x.AstType && y.Descr == x.AstCode)).ToList();
                 //        session.AddValue(Session.SessionKeys.AVPartNo, cnrsPart.Attributes.Where(x => x.InfoType == "AV").
                 //                                                                                      Select(y => y.InfoValue).FirstOrDefault());
                 //        session.AddValue(Session.SessionKeys.HasCNRS, "Y");
                 //        session.AddValue(Session.SessionKeys.CNRSPart, cnrsPart);

                 //    }
                 //    #endregion
                 //}

                 //if (needGenAstPartList.Count > 0 && needGenAstDefineList.Count > 0)
                 //{
                 //    bool hasPart = needGenAstDefineList.All(x => needGenAstPartList.Any(y => y.BOMNodeType == x.AstType && y.Descr == x.AstCode));
                 //    if (!hasPart)
                 //    {
                 //        //throw new FisException("CQCHK1092", new List<string> { prod.ProId,string.Join(",", needGenAstDefineList.Select(x=>x.AstCode).ToArray())});
                 //        return base.DoExecute(executionContext);
                 //    }

                 //    if (needGenAstDefineList.Any(x => x.HasUPSAst == "Y"))
                 //    {
                 //        session.AddValue(Session.SessionKeys.IsUPSDevice, prod.IsUPSDevice ? "Y" : "N");
                 //    }
                 //    else
                 //    {
                 //        session.AddValue(Session.SessionKeys.IsUPSDevice, "N");
                 //    }

                 //    session.AddValue(Session.SessionKeys.GenerateASTSN, "Y");
                 //    //session.AddValue(Session.SessionKeys.NeedGenAstPartList, needCombineAstList.Select(x => x.AstPart).ToList());
                 //    //session.AddValue(Session.SessionKeys.NeedGenAstDefineList, needCombineAstList.Select(x => x.AstDefine).ToList());

                 //    session.AddValue(Session.SessionKeys.NeedGenAstPartList, needGenAstPartList);
                 //    session.AddValue(Session.SessionKeys.NeedGenAstDefineList, needGenAstDefineList);

                 //}
                 #endregion

             }           
            
           

            return base.DoExecute(executionContext);
        }
        
    }
}
