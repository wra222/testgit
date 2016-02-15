/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Combine AST Page
* UI:CI-MES12-SPEC-FA-UI AFT MVS
* UC:CI-MES12-SPEC-FA-UC AFT MVS         
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-07-13   itc202017           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Part;
using System.Linq;
using IMES.DataModel;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.Misc;
using Metas = IMES.Infrastructure.Repository._Metas;
using System.ComponentModel;
using IMES.Common;

namespace IMES.Activity
{
   
    /// <summary>
    /// CheckAndGenerateATSN
    /// </summary>
    public partial class CheckAndGenerateATSN : BaseActivity
    {
        private static object _syncRoot_GetSeq = new object();
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckAndGenerateATSN()
        {
            InitializeComponent();
        }

        
        /// <summary>
        /// Generate ATSN7 label
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IProduct curProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);

            bool needASTSN= ((string)session.GetValue(Session.SessionKeys.GenerateASTSN))=="Y";
            bool isCDSI = ((string)session.GetValue(Session.SessionKeys.HasCDSI)) == "Y";
            bool isCNRS = ((string)session.GetValue(Session.SessionKeys.HasCNRS)) == "Y";
            string avPartNo = (string)session.GetValue(Session.SessionKeys.AVPartNo)??"";
            if (isCDSI)
            {
                IPart cdsiPart = utl.IsNull<IPart>(session, Session.SessionKeys.CDSIPart);
                combineCDSI(session, curProduct, cdsiPart, "CDSI", avPartNo);             
            }
            else if (isCNRS)  // AstType 為 'ATSN3'
            {
                IPart cnrsPart = utl.IsNull<IPart>(session, Session.SessionKeys.CNRSPart);
                combineCDSI(session, curProduct, cnrsPart, "CNRS", avPartNo);               
            }


            if (needASTSN )
            {
                IList<IPart> partList = utl.IsNull<IList<IPart>>(session, Session.SessionKeys.NeedGenAstPartList);
                IList<AstDefineInfo> astDefineList = utl.IsNull<IList<AstDefineInfo>>(session, Session.SessionKeys.NeedGenAstDefineList);

                if (utl.UPS.checkUPSDeviceInAssignStation(curProduct, astDefineList, this.Station))
                {
                    return base.DoExecute(executionContext);
                }
                bool hasAssignAst = false;
                foreach (AstDefineInfo astDefine in astDefineList)
                {
                    IPart part = partList.Where(x => x.BOMNodeType == astDefine.AstType && x.Descr == astDefine.AstCode).FirstOrDefault();
                    if (part == null)
                    {
                        continue;
                    }
                    string custNum3 = null;
                    string custNum = utl.GenAst.AssignAstNumber(session, part, curProduct, this.Station, this.Customer, this.Editor, out  custNum3);
                   
                    #region dsable code move to utl
                    ////0001323: 特殊资产编号
                    //IList<ConstValueInfo> cvInfoList =null;
                    //ConstValueInfo cvInfo =null;
                    //string custNum = null;
                    //string custNum3 = null;
                    //if (utl.TryConstValue("PreFixSNAST", part.PN, out cvInfoList, out cvInfo))
                    //{

                    //    if (string.IsNullOrEmpty(curProduct.CUSTSN))
                    //    {
                    //        throw new FisException("CQCHK1108", new string[] { curProduct.ProId });
                    //    }
                    //    else
                    //    {
                    //        custNum = cvInfo.value.Trim() + curProduct.CUSTSN;
                    //    }
                        
                    //}
                    //else
                    //{
                    //    #region 產生ATSN 需要
                    //    string astPo = (string)session.GetValue("AstPo");
                    //    string cust = null;
                    //    string cust3 = null;
                    //    if (string.IsNullOrEmpty(astPo))
                    //    {
                    //        cust = astPo;
                    //    }
                    //    else
                    //    {
                    //        cust = curProduct.ModelObj.GetAttribute("Cust");
                    //        cust3 = curProduct.ModelObj.GetAttribute("Cust3");
                    //    }

                    //    if (!string.IsNullOrEmpty(cust))
                    //    {
                    //        //custNum=getAstNumber(session, utl, curProduct, cust);
                    //        custNum = utl.GetAstNumber(session,  curProduct, cust,this.Station,this.Customer,this.Editor);
                    //    }

                    //    //检查Declare @Cust3 = ModelInfo.Value (Conditon: Model=#Prodocut.Model and Name = ‘Cust3’) ，
                    //    //若@Cust3 不为空且不为Null，则执行下面AST的分配工作：
                    //    if (!string.IsNullOrEmpty(cust3))
                    //    {
                    //        //custNum3 = getAstNumber(session, utl, curProduct, cust3);
                    //        custNum3 = utl.GetAstNumber(session, curProduct, cust3, this.Station, this.Customer, this.Editor);  
                    //    }
                    //    #endregion
                    //}
                    #endregion
                   
                    //保存product和Asset SN的绑定关系

                    if (!string.IsNullOrEmpty(custNum))
                    {
                        IProductPart assetTag = new ProductPart();
                        assetTag.ProductID = curProduct.ProId;
                        assetTag.PartID = part.PN;
                        assetTag.PartType = part.Descr;
                        assetTag.Iecpn = "";
                        assetTag.CustomerPn = part.GetAttribute("AV") ?? string.Empty;
                        assetTag.PartSn = custNum;
                        assetTag.Station = Station;
                        assetTag.Editor = Editor;
                        assetTag.Cdt = DateTime.Now;
                        assetTag.Udt = DateTime.Now;
                        assetTag.BomNodeType = part.BOMNodeType;
                        assetTag.CheckItemType = "GenASTSN";
                        curProduct.AddPart(assetTag);
                        productRepository.Update(curProduct, session.UnitOfWork);
                    }

                    if (!string.IsNullOrEmpty(custNum3))
                    {
                        IProductPart assetTag = new ProductPart();
                        assetTag.ProductID = curProduct.ProId;
                        assetTag.PartID = part.PN;
                        assetTag.PartType = part.Descr;
                        assetTag.Iecpn = "";
                        assetTag.CustomerPn = part.GetAttribute("AV") ?? string.Empty;
                        assetTag.PartSn = custNum3;
                        assetTag.Station = Station;
                        assetTag.Editor = Editor;
                        assetTag.Cdt = DateTime.Now;
                        assetTag.Udt = DateTime.Now;
                        assetTag.BomNodeType = part.BOMNodeType;
                        assetTag.CheckItemType = "GenASTSN";
                        curProduct.AddPart(assetTag);
                        productRepository.Update(curProduct, session.UnitOfWork);
                    }

                    session.AddValue("AssetSN", custNum ?? "");
                    session.AddValue("AssetSN3", custNum3 ?? "");
                    hasAssignAst = true;
               }

                if (!hasAssignAst)
                {
                    throw new FisException("CQCHK1093", new string[] { curProduct.ProId,  string.Join(",", astDefineList.Select(x=>x.AstCode).ToArray())});
                }
            }
            return base.DoExecute(executionContext);
        }
        
        private void ParseAst(string ast, out string prePart, out string numPart)
        {
            int s_idx = 0;
            bool bFoundDigit = false;

            for (int i = 0; i < ast.Length; i++)
            {
                if (ast[i] < '0' || ast[i] > '9')
                {
                    bFoundDigit = false;
                }
                else if (bFoundDigit == false)
                {
                    bFoundDigit = true;
                    s_idx = i;
                }
            }

            if (bFoundDigit == true)
            {
                numPart = ast.Substring(s_idx);
                prePart = ast.Substring(0, s_idx);
            }
            else
            {
                numPart = "";
                prePart = ast;
            }

            return;
        }
        
        private void combineCDSI(Session session,IProduct product, IPart part, string checkItemType, string avPartNo )
            {
                var prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                string AST1 = "";
                CdsiastInfo cdi = new CdsiastInfo();
                cdi.tp = "ASSET_TAG";
                cdi.snoId = product.ProId;
                IList<CdsiastInfo> cdsiastInfoList = prodRep.GetCdsiastInfoList(cdi);
                if (cdsiastInfoList != null && cdsiastInfoList.Count > 0)
                {
                    AST1 = cdsiastInfoList[0].sno;
                }

                string AST2 = "";
                CdsiastInfo cdi2 = new CdsiastInfo();
                cdi2.tp = "ASSET_TAG2";
                cdi2.snoId = product.ProId;
                IList<CdsiastInfo> cdsiastInfoList2 = prodRep.GetCdsiastInfoList(cdi2);
                if (cdsiastInfoList2 != null && cdsiastInfoList2.Count > 0)
                {
                    AST2 = cdsiastInfoList2[0].sno;
                }

                if ((AST1 == "") && (AST2 == ""))
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK203", errpara);
                }
                else
                {
                    //不为空则存入
                    //保存product和Asset SN的绑定关系
                   if (AST1 != "")
                   {
                           IProductPart assetTag1 = new ProductPart();
                            assetTag1.ProductID = product.ProId;
                            assetTag1.PartID = part.PN;
                            assetTag1.PartType = part.Descr;
                            assetTag1.Iecpn = "";
                            assetTag1.CustomerPn = avPartNo;
                            assetTag1.PartSn = AST1;
                            assetTag1.Station = this.Station;
                            assetTag1.Editor = this.Editor;
                            assetTag1.Cdt = DateTime.Now;
                            assetTag1.Udt = DateTime.Now;
                            assetTag1.BomNodeType = part.BOMNodeType;
                            assetTag1.CheckItemType = checkItemType;

                            product.AddPart(assetTag1);
                            prodRep.Update(product, session.UnitOfWork);
                    }

                    if (AST2 != "")
                    {
                        IProductPart assetTag2 = new ProductPart();
                        assetTag2.ProductID = product.ProId;
                        assetTag2.PartID = part.PN;
                        assetTag2.PartType = part.Descr;
                        assetTag2.Iecpn = "";
                        assetTag2.CustomerPn = avPartNo;
                        assetTag2.PartSn = AST2;
                        assetTag2.Station = this.Station;
                        assetTag2.Editor = this.Editor;
                        assetTag2.Cdt = DateTime.Now;
                        assetTag2.Udt = DateTime.Now;
                        assetTag2.BomNodeType = part.BOMNodeType;
                        assetTag2.CheckItemType = checkItemType;

                        product.AddPart(assetTag2);
                        prodRep.Update(product, session.UnitOfWork);
                    }

                    session.AddValue("AST1", AST1);
                    session.AddValue("AST2", AST2);

                    //2012-5-2
                    string ASTinfo = "";
                    if (!string.IsNullOrEmpty(AST1) && !string.IsNullOrEmpty(AST2))
                    {
                        ASTinfo += AST1 + ", " + AST2;
                    }
                    else if (!string.IsNullOrEmpty(AST1) && string.IsNullOrEmpty(AST2))
                    {
                        ASTinfo = AST1;
                    }
                    else if (string.IsNullOrEmpty(AST1) && !string.IsNullOrEmpty(AST2))
                    {
                        ASTinfo = AST2;
                    }
                    session.AddValue("ASTInfo", ASTinfo);
                }
            }

        #region disable code; move it into utl
        //private string GenerateCodeNew(string cust)
        //    {
        //        try
        //        {
        //            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
        //            string customerSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);

        //            string custNum = "";
        //            string numType = "AST";

        //            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

        //            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

        //            SqlTransactionManager.Begin();
        //            lock (_syncRoot_GetSeq)
        //            {
        //                AssetRangeCodeInfo currentRange = ipartRepository.GetAssetRangeByStatus(cust, new string[] { "A", "R" });

        //                if (currentRange == null)
        //                {
        //                    throw new FisException("CHK200", new string[] { customerSN });
        //                }

        //                NumControl currentMaxNum = numCtrlRepository.GetMaxValue(numType, cust);

        //                if (currentMaxNum == null)
        //                {
        //                    #region 第一次產生Serial Number
        //                    //Check new Range
        //                    CheckAssetNum(customerSN,
        //                                                 currentRange.Begin, currentRange.End);
        //                    currentMaxNum = new NumControl();
        //                    currentMaxNum.NOName = cust;
        //                    currentMaxNum.NOType = numType;
        //                    currentMaxNum.Value = currentRange.Begin;
        //                    currentMaxNum.Customer = this.Customer;

        //                    IUnitOfWork uof = new UnitOfWork();
        //                    numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
        //                    if (currentMaxNum.Value == currentRange.End)
        //                    {
        //                        ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
        //                    }
        //                    else
        //                    {
        //                        ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
        //                    }
        //                    uof.Commit();
        //                    SqlTransactionManager.Commit();
        //                    custNum = currentMaxNum.Value;
        //                    #endregion
        //                }
        //                else
        //                {
        //                    #region no need check endNumber
        //                    //if (currentMaxNum.Value == currentRange.End)
        //                    //{
        //                    //    ipartRepository.SetAssetRangeStatus(currentRange.ID, "C");
        //                    //    currentRange = ipartRepository.GetAssetRangeByStatus(cust, new string[] { "R" });
        //                    //    if (currentRange == null ||
        //                    //        currentMaxNum.Value.Equals(currentRange.Begin) ||
        //                    //        currentMaxNum.Value.Equals(currentRange.End))
        //                    //    {
        //                    //        throw new FisException("CHK200", new string[] { customerSN });
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        #region 更換新Range產生Serial Number
        //                    //        //Check new Range
        //                    //        CheckAssetNum(customerSN, currentRange.Begin, currentRange.End);
        //                    //        IUnitOfWork uof = new UnitOfWork();
        //                    //        currentMaxNum.Value = currentRange.Begin;
        //                    //        numCtrlRepository.Update(currentMaxNum, uof);
        //                    //        ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
        //                    //        uof.Commit();
        //                    //        SqlTransactionManager.Commit();
        //                    //        custNum = currentMaxNum.Value;
        //                    //        #endregion
        //                    //    }
        //                    //}
        //                    #endregion

        //                    if (currentRange.Status == "R" &&
        //                       checkNewRange(currentMaxNum.Value, currentRange.Begin, currentRange.End))
        //                    {
        //                        #region 更換新Range產生Serial Number
        //                        //Check new Range
        //                        CheckAssetNum(customerSN, currentRange.Begin, currentRange.End);
        //                        IUnitOfWork uof = new UnitOfWork();
        //                        currentMaxNum.Value = currentRange.Begin;
        //                        numCtrlRepository.Update(currentMaxNum, uof);
        //                        if (currentRange.Begin.CompareTo(currentRange.End) <  0)
        //                        {
        //                            ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
        //                        }
        //                        else
        //                        {
        //                            ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
        //                        }                           
        //                        uof.Commit();
        //                        SqlTransactionManager.Commit();
        //                        custNum = currentMaxNum.Value;
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        currentMaxNum.Value = GenNextAssetNum(customerSN, currentMaxNum.Value, currentRange.End);
        //                        IUnitOfWork uof = new UnitOfWork();
        //                        numCtrlRepository.Update(currentMaxNum, uof);
        //                        if (currentMaxNum.Value == currentRange.End)
        //                        {
        //                            ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
        //                        }
        //                        else
        //                        {
        //                            ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
        //                        }
        //                        uof.Commit();
        //                        SqlTransactionManager.Commit();
        //                        custNum = currentMaxNum.Value;
        //                    }
        //                }
        //            }

        //            if (cust == "SCUSTA-1")
        //            {
        //                custNum = "000" + custNum + "00";
        //            }

        //            return custNum;
        //        }
        //        catch (Exception e)     //2012-7-19
        //        {
        //            SqlTransactionManager.Rollback();
        //            throw e;
        //        }
        //        finally                 //2012-7-19
        //        {
        //            SqlTransactionManager.Dispose();
        //            SqlTransactionManager.End();
        //        }
        //    }

        //private string GenNextAssetNum(string custSN, string lastNum, string rangeMaxNum)
        //    {

        //        //Get last num PreStr & Sequence number
        //        string lastAssetNum = "";
        //        string lastPreString = "";
        //        string EndNum = "";
        //        string preEndString = "";

        //        Match lastNumMatch = null;
        //        Match endNumMatch = null;
        //        CheckAssetNum(custSN, lastNum, rangeMaxNum, ref lastNumMatch, ref endNumMatch);

        //        lastAssetNum = lastNumMatch.Value;
        //        lastPreString = lastNum.Substring(0, lastNumMatch.Index);
        //        EndNum = endNumMatch.Value;
        //        preEndString = rangeMaxNum.Substring(0, endNumMatch.Index);
        //        long largestNum = long.Parse(lastAssetNum);
        //        long end = long.Parse(EndNum);
        //        largestNum++;
        //        int largestNumCount = lastAssetNum.Length;
        //        string strNewLargestCustNum = largestNum.ToString().PadLeft(largestNumCount, '0');
        //        return preEndString + strNewLargestCustNum;

        //    }

        //private void CheckAssetNum(string custSN,
        //                                                    string lastNum,
        //                                                    string rangeMaxNum,
        //                                                    ref Match lastNumMatch,
        //                                                    ref Match endNumMatch)
        //    {

        //        string lastPreString = "";
        //        string preEndString = "";

        //        if (lastNum.Length != rangeMaxNum.Length)
        //        {
        //            throw new FisException("CHK201", new string[] { custSN });
        //        }

        //        MatchCollection matches = Regex.Matches(lastNum, @"\d+");
        //        if (matches.Count == 0)
        //        {
        //            throw new FisException("CHK201", new string[] { custSN });
        //        }
        //        lastNumMatch = matches[matches.Count - 1];

        //        matches = Regex.Matches(rangeMaxNum, @"\d+");
        //        if (matches.Count == 0)
        //        {
        //            throw new FisException("CHK201", new string[] { custSN });
        //        }
        //        endNumMatch = matches[matches.Count - 1];

        //        if (lastNumMatch.Index != endNumMatch.Index ||
        //            lastNumMatch.Length != endNumMatch.Length)
        //        {
        //            throw new FisException("CHK201", new string[] { custSN });
        //        }

        //        //最後不適數字
        //        if (lastNum.Length != (lastNumMatch.Index + lastNumMatch.Length))
        //        {
        //            throw new FisException("CHK201", new string[] { custSN });
        //        }

        //        lastPreString = lastNum.Substring(0, lastNumMatch.Index);
        //        preEndString = rangeMaxNum.Substring(0, endNumMatch.Index);

        //        if (lastPreString != preEndString)
        //        {
        //            throw new FisException("CHK201", new string[] { custSN });
        //        }

        //        if (lastNumMatch.Value.CompareTo(endNumMatch.Value) == 1)
        //        {
        //            throw new FisException("CHK201", new string[] { custSN });
        //        }

        //    }

        //private void CheckAssetNum(string custSN,
        //                                                    string lastNum,
        //                                                    string rangeMaxNum)
        //    {
        //        Match lastNumMatch = null;
        //        Match endNumMatch = null;
        //        CheckAssetNum(custSN, lastNum, rangeMaxNum, ref lastNumMatch, ref endNumMatch);
        //    }

        //private bool checkNewRange(string maxNum, string beginNum, string endNum)
        //    {

        //        int iBegin = beginNum.CompareTo(maxNum);
        //        int iEnd = endNum.CompareTo(maxNum);
        //        if ((iBegin == 1 && iEnd == -1 && maxNum.Length == endNum.Length) || iBegin == 0 || iEnd == 0)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //private string getAstNumber(Session session, ActivityCommonImpl utl, IProduct curProduct, string cust)
        //{
        //    string custNum = string.Empty;
        //    custNum = utl.CheckAndGetUsedAst(session, curProduct.ProId, cust, "AST", this.Station, this.Editor);
        //    if (custNum == null)
        //    {
        //        custNum = utl.CheckAndSetReleaseAstNumber(curProduct.ProId, cust, "AST", this.Station, this.Editor);
        //    }
        //    if (string.IsNullOrEmpty(custNum))
        //    {
        //        custNum = GenerateCodeNew(cust);

        //        // Checksum
        //        custNum = utl.GetAstChecksum(cust, custNum);

        //        custNum = utl.CheckAndAddPreFixDateAst(cust, custNum);

        //        //Check postfix 
        //        custNum = utl.CheckAndAddPostFixAst(cust, custNum);
        //        //ConstValueInfo cond = new ConstValueInfo();
        //        //cond.type = "AST";
        //        //cond.name = cust;
        //        //IList<ConstValueInfo> valList = partRepository.GetConstValueInfoList(cond);
        //        //if (valList.Count > 0 && !String.IsNullOrEmpty(valList[0].value))
        //        //{
        //        //    custNum += valList[0].value.Trim();
        //        //}
        //        utl.InsertCombinedAstNumber(curProduct.ProId, cust, "AST", custNum, this.Station, this.Editor);
        //    }
        //    return custNum;
        //}
        #endregion



       
       

    }
}
