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
    /// Generate ATSN7 label
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      AFT MVS
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         详见UC
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK203
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    ///         ProdidOrCustsn
    ///         DESCR
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>

    public partial class GenerateATSN7 : BaseActivity
    {
        private static object _syncRoot_GetSeq = new object();
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateATSN7()
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
           // Product curProduct = (Product)session.GetValue(Session.SessionKeys.Product);
           // IList<string> pnList = session.GetValue("PnListOfATSN7") as IList<string>;
            IList<string> pnList = utl.IsNull<IList<string>>(session, "PnListOfATSN7");
            bool bCDSI = false;
            if (session.GetValue("bCDSI") != null)
            {
                bCDSI = (bool)session.GetValue("bCDSI");
            }

            if (pnList.Count == 0)
            {
                throw new FisException("CQCHK1089", new List<string> { curProduct.ProId,"BOM", "ATSN7" });               
            }

            if (bCDSI == true)
            {
                string AST1 = "";
                CdsiastInfo cdi = new CdsiastInfo();
                cdi.tp = "ASSET_TAG";
                cdi.snoId = curProduct.ProId;
                IList<CdsiastInfo> cdsiastInfoList = productRepository.GetCdsiastInfoList(cdi);
                if (cdsiastInfoList != null && cdsiastInfoList.Count > 0)
                {
                    AST1 = cdsiastInfoList[0].sno;
                }

                string AST2 = "";
                CdsiastInfo cdi2 = new CdsiastInfo();
                cdi2.tp = "ASSET_TAG2";
                cdi2.snoId = curProduct.ProId;
                IList<CdsiastInfo> cdsiastInfoList2 = productRepository.GetCdsiastInfoList(cdi2);
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
                    foreach (string tmp in pnList)
                    {
                        if (AST1 != "")
                        {
                            IProductPart assetTag1 = new ProductPart();
                            assetTag1.ProductID = curProduct.ProId;
                            assetTag1.PartID = tmp;
                            assetTag1.PartType = "ATSN7";
                            assetTag1.Iecpn = "";
                            assetTag1.CustomerPn = "";
                            assetTag1.PartSn = AST1;
                            assetTag1.Station = Station;
                            assetTag1.Editor = Editor;
                            assetTag1.Cdt = DateTime.Now;
                            assetTag1.Udt = DateTime.Now;
                            assetTag1.BomNodeType = "AT";
                            assetTag1.CheckItemType = "CDSI";

                            curProduct.AddPart(assetTag1);
                            productRepository.Update(curProduct,session.UnitOfWork);
                        }

                        if (AST2 != "")
                        {
                            IProductPart assetTag2 = new ProductPart();
                            assetTag2.ProductID = curProduct.ProId;
                            assetTag2.PartID = tmp;
                            assetTag2.PartType = "ATSN7";
                            assetTag2.Iecpn = "";
                            assetTag2.CustomerPn = "";
                            assetTag2.PartSn = AST2;
                            assetTag2.Station = Station;
                            assetTag2.Editor = Editor;
                            assetTag2.Cdt = DateTime.Now;
                            assetTag2.Udt = DateTime.Now;
                            assetTag2.BomNodeType = "AT";
                            assetTag2.CheckItemType = "CDSI";

                            curProduct.AddPart(assetTag2);
                            productRepository.Update(curProduct, session.UnitOfWork);
                        }
                    }
                }
            }
            else    //Not CDSI
            {                
                #region 產生ATSN7 需要
                    string cust = curProduct.ModelObj.GetAttribute("Cust");
                    string custNum = ActivityCommonImpl.Instance.AstNum.CheckAndGetUsedAst(session, curProduct.ProId, cust, "AST", this.Station, this.Editor);
                    if (custNum == null)
                    {
                        custNum = ActivityCommonImpl.Instance.AstNum.CheckAndSetReleaseAstNumber(curProduct.ProId, cust, "AST", this.Station, this.Editor);
                    }
                    if (string.IsNullOrEmpty(custNum))
                    {
                        custNum = GenerateCodeNew(cust);

                        //Update custNum
                        //if (cust == "SCUSTA-1")
                        //{
                        //    custNum = "000" + custNum.Trim() + "00";
                        //}
                        /*
                        else if (cust == "CUSTW-1")
                        {
                            custNum = custNum.Trim() + " HQ P47";
                        }
                        else if (cust == "CUSTW-2")
                        {
                            custNum = custNum.Trim() + " HQ P49";
                        }
                         */

                        // Checksum
                        custNum = ActivityCommonImpl.Instance.GetAstChecksum(cust, custNum);

                        custNum = ActivityCommonImpl.Instance.CheckAndAddPreFixDateAst(cust, custNum);

                        ConstValueInfo cond = new ConstValueInfo();
                        cond.type = "AST";
                        cond.name = cust;
                        IList<ConstValueInfo> valList = partRepository.GetConstValueInfoList(cond);
                        if (valList.Count > 0 && !String.IsNullOrEmpty(valList[0].value))
                        {
                            custNum += valList[0].value.Trim();
                        }
                        ActivityCommonImpl.Instance.AstNum.InsertCombinedAstNumber(curProduct.ProId, cust, "AST", custNum, this.Station, this.Editor);
                    }
                    //保存product和Asset SN的绑定关系
                    foreach (string tmp in pnList)
                    {
                        IProductPart assetTag = new ProductPart();
                        assetTag.ProductID = curProduct.ProId;
                        assetTag.PartID = tmp;
                        assetTag.PartType = "ATSN7";
                        assetTag.Iecpn = "";
                        assetTag.CustomerPn = "";
                        assetTag.PartSn = custNum;
                        assetTag.Station = Station;
                        assetTag.Editor = Editor;
                        assetTag.Cdt = DateTime.Now;
                        assetTag.Udt = DateTime.Now;
                        assetTag.BomNodeType = "AT";
                        assetTag.CheckItemType = "GenASTSN";
                        curProduct.AddPart(assetTag);
                        productRepository.Update(curProduct, session.UnitOfWork);
                    }
                    #endregion              
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


        private string GenerateCodeNew(string cust)
        {
            try
            {
                var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                string customerSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);

                string custNum = "";
                string numType = "AST";

                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    AssetRangeCodeInfo currentRange = ipartRepository.GetAssetRangeByStatus(cust, new string[] { "A", "R" });

                    if (currentRange == null)
                    {
                        throw new FisException("CHK200", new string[] { customerSN });
                    }

                    NumControl currentMaxNum = numCtrlRepository.GetMaxValue(numType, cust);

                    if (currentMaxNum == null)
                    {
                        #region 第一次產生Serial Number
                        //Check new Range
                        CheckAssetNum(customerSN,
                                                     currentRange.Begin, currentRange.End);
                        currentMaxNum = new NumControl();
                        currentMaxNum.NOName = cust;
                        currentMaxNum.NOType = numType;
                        currentMaxNum.Value = currentRange.Begin;
                        currentMaxNum.Customer = this.Customer;

                        IUnitOfWork uof = new UnitOfWork();
                        numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
                        if (currentMaxNum.Value == currentRange.End)
                        {
                            ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
                        }
                        else
                        {
                            ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
                        }
                        uof.Commit();
                        SqlTransactionManager.Commit();
                        custNum = currentMaxNum.Value;
                        #endregion
                    }
                    else
                    {
                        #region no need check endNumber
                        //if (currentMaxNum.Value == currentRange.End)
                        //{
                        //    ipartRepository.SetAssetRangeStatus(currentRange.ID, "C");
                        //    currentRange = ipartRepository.GetAssetRangeByStatus(cust, new string[] { "R" });
                        //    if (currentRange == null ||
                        //        currentMaxNum.Value.Equals(currentRange.Begin) ||
                        //        currentMaxNum.Value.Equals(currentRange.End))
                        //    {
                        //        throw new FisException("CHK200", new string[] { customerSN });
                        //    }
                        //    else
                        //    {
                        //        #region 更換新Range產生Serial Number
                        //        //Check new Range
                        //        CheckAssetNum(customerSN, currentRange.Begin, currentRange.End);
                        //        IUnitOfWork uof = new UnitOfWork();
                        //        currentMaxNum.Value = currentRange.Begin;
                        //        numCtrlRepository.Update(currentMaxNum, uof);
                        //        ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
                        //        uof.Commit();
                        //        SqlTransactionManager.Commit();
                        //        custNum = currentMaxNum.Value;
                        //        #endregion
                        //    }
                        //}
                        #endregion

                        if (currentRange.Status == "R" &&
                           checkNewRange(currentMaxNum.Value, currentRange.Begin, currentRange.End))
                        {
                            #region 更換新Range產生Serial Number
                            //Check new Range
                            CheckAssetNum(customerSN, currentRange.Begin, currentRange.End);
                            IUnitOfWork uof = new UnitOfWork();
                            currentMaxNum.Value = currentRange.Begin;
                            numCtrlRepository.Update(currentMaxNum, uof);
                            if (currentRange.Begin.CompareTo(currentRange.End) <  0)
                            {
                                ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
                            }
                            else
                            {
                                ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
                            }                           
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            custNum = currentMaxNum.Value;
                            #endregion
                        }
                        else
                        {
                            currentMaxNum.Value = GenNextAssetNum(customerSN, currentMaxNum.Value, currentRange.End);
                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            if (currentMaxNum.Value == currentRange.End)
                            {
                                ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
                            }
                            else
                            {
                                ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            custNum = currentMaxNum.Value;
                        }
                    }
                }

                if (cust == "SCUSTA-1")
                {
                    custNum = "000" + custNum + "00";
                }

                return custNum;
            }
            catch (Exception e)     //2012-7-19
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally                 //2012-7-19
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        private string GenNextAssetNum(string custSN, string lastNum, string rangeMaxNum)
        {

            //Get last num PreStr & Sequence number
            string lastAssetNum = "";
            string lastPreString = "";
            string EndNum = "";
            string preEndString = "";

            Match lastNumMatch = null;
            Match endNumMatch = null;
            CheckAssetNum(custSN, lastNum, rangeMaxNum, ref lastNumMatch, ref endNumMatch);

            lastAssetNum = lastNumMatch.Value;
            lastPreString = lastNum.Substring(0, lastNumMatch.Index);
            EndNum = endNumMatch.Value;
            preEndString = rangeMaxNum.Substring(0, endNumMatch.Index);
            long largestNum = long.Parse(lastAssetNum);
            long end = long.Parse(EndNum);
            largestNum++;
            int largestNumCount = lastAssetNum.Length;
            string strNewLargestCustNum = largestNum.ToString().PadLeft(largestNumCount, '0');
            return preEndString + strNewLargestCustNum;

        }

        private void CheckAssetNum(string custSN,
                                                        string lastNum,
                                                        string rangeMaxNum,
                                                        ref Match lastNumMatch,
                                                        ref Match endNumMatch)
        {

            string lastPreString = "";
            string preEndString = "";

            if (lastNum.Length != rangeMaxNum.Length)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }

            MatchCollection matches = Regex.Matches(lastNum, @"\d+");
            if (matches.Count == 0)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }
            lastNumMatch = matches[matches.Count - 1];

            matches = Regex.Matches(rangeMaxNum, @"\d+");
            if (matches.Count == 0)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }
            endNumMatch = matches[matches.Count - 1];

            if (lastNumMatch.Index != endNumMatch.Index ||
                lastNumMatch.Length != endNumMatch.Length)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }

            //最後不適數字
            if (lastNum.Length != (lastNumMatch.Index + lastNumMatch.Length))
            {
                throw new FisException("CHK201", new string[] { custSN });
            }

            lastPreString = lastNum.Substring(0, lastNumMatch.Index);
            preEndString = rangeMaxNum.Substring(0, endNumMatch.Index);

            if (lastPreString != preEndString)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }

            if (lastNumMatch.Value.CompareTo(endNumMatch.Value) == 1)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }

        }

        private void CheckAssetNum(string custSN,
                                                        string lastNum,
                                                        string rangeMaxNum)
        {
            Match lastNumMatch = null;
            Match endNumMatch = null;
            CheckAssetNum(custSN, lastNum, rangeMaxNum, ref lastNumMatch, ref endNumMatch);
        }

        private bool checkNewRange(string maxNum, string beginNum, string endNum)
        {

            int iBegin = beginNum.CompareTo(maxNum);
            int iEnd = endNum.CompareTo(maxNum);
            if ((iBegin == 1 && iEnd == -1 && maxNum.Length == endNum.Length) || iBegin == 0 || iEnd == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

       

    }
}
