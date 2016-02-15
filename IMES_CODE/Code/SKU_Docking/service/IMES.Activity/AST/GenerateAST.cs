/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Online Generate AST Page
* UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2011/11/21 
* UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2011/11/21            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-21   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0495, Jessica Liu, 2012-2-22
*/

using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Part;
using System.Collections;
using IMES.DataModel;
//2012-7-19
using IMES.Infrastructure.UnitOfWork;
using System.Text.RegularExpressions;
using IMES.Common;


namespace IMES.Activity
{
    /// <summary>
    /// 产生Asset SN
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
    ///         1.生成序号
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK200，CHK201
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
    ///         Insert Product_Part
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    ///         IPartRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateAST : BaseActivity
	{
        //2012-7-19
        private static object _syncRoot_GetSeq = new object();

        ///<summary>
        ///</summary>
        public GenerateAST()
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
             Session session = CurrentSession;
             ActivityCommonImpl utl = ActivityCommonImpl.Instance;

             var currenProduct = utl.IsNull<IProduct>(session,Session.SessionKeys.Product);
             string customerSN = (string)session.GetValue(Session.SessionKeys.CustSN);
             if (string.IsNullOrEmpty(customerSN))
             {
                 customerSN = currenProduct.CUSTSN;
             }
             //0001323: 特殊资产编号
           
             
             IList<IPart> needGenAstPartList = utl.IsNull<IList<IPart>>(session, Session.SessionKeys.NeedGenAstPartList);
             IPart part = null;

             if (needGenAstPartList.Count > 0)
             {
                 part = needGenAstPartList.FirstOrDefault();
             }

             if (part == null)
             {
                 throw new FisException("CHK205", new List<string> { customerSN });
             }

             string custNum3 = null;
             string custNum = utl.GenAst.AssignAstNumber(session, part, currenProduct, this.Station, this.Customer, this.Editor, out  custNum3);
            
            #region disable code move to utl
             //IList<ConstValueInfo> cvInfoList = null;
             //ConstValueInfo cvInfo = null;
             //if (utl.TryConstValue("PreFixSNAST", part.PN, out cvInfoList, out cvInfo))
             //{
             //    if (string.IsNullOrEmpty(currenProduct.CUSTSN))
             //    {
             //        throw new FisException("CQCHK1108", new string[] { currenProduct.ProId });
             //    }
             //    else
             //    {
             //        custNum = cvInfo.value.Trim() + currenProduct.CUSTSN;
             //    }
             //}
             //else
             //{
             //    #region assign asset number
             //    string astPo = (string)session.GetValue("AstPo");

             //    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
             //    string cust = string.Empty;
             //    string cust3 = string.Empty;
                 
             //    if (string.IsNullOrEmpty(astPo))
             //    {
             //        cust = currenProduct.ModelObj.GetAttribute("Cust");
             //    }
             //    else
             //    {
             //        cust = astPo;
             //        cust3 = currenProduct.ModelObj.GetAttribute("Cust3");
             //    }

             //    if (!string.IsNullOrEmpty(cust))
             //    {
             //        //custNum = getAstNumber(session, utl, currenProduct, cust);
             //        custNum = utl.GetAstNumber(session,  currenProduct, cust, this.Station, this.Customer, this.Editor);
             //    }

             //    //检查Declare @Cust3 = ModelInfo.Value (Conditon: Model=#Prodocut.Model and Name = ‘Cust3’) ，
             //    //若@Cust3 不为空且不为Null，则执行下面AST的分配工作：
             //    if (!string.IsNullOrEmpty(cust3))
             //    {
             //        //custNum3 = getAstNumber(session, utl, currenProduct, cust3);
             //        custNum3 = utl.GetAstNumber(session, currenProduct, cust3, this.Station, this.Customer, this.Editor);
             //    }

             //    #region disable code
             //    //custNum = ActivityCommonImpl.Instance.CheckAndGetUsedAst(session, currenProduct.ProId, cust, "AST", this.Station, this.Editor);
             //    //if (custNum == null)
             //    //{
             //    //    custNum = ActivityCommonImpl.Instance.CheckAndSetReleaseAstNumber(currenProduct.ProId, cust, "AST", this.Station, this.Editor);
             //    //}

             //    //if (string.IsNullOrEmpty(custNum))
             //    //{
             //    //    custNum = GenerateCodeNew(currenProduct, customerSN, cust);

             //    //    // Checksum
             //    //    custNum = ActivityCommonImpl.Instance.GetAstChecksum(cust, custNum);

             //    //    custNum = ActivityCommonImpl.Instance.CheckAndAddPreFixDateAst(cust, custNum);

             //    //    ConstValueInfo info = new ConstValueInfo();
             //    //    info.type = "AST";
             //    //    info.name = cust;

             //    //    IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
             //    //    if (retList != null && retList.Count != 0)
             //    //    {
             //    //        if (string.IsNullOrEmpty(retList[0].value) == false)
             //    //        {
             //    //            custNum += retList[0].value;
             //    //        }
             //    //    }

             //    //    ActivityCommonImpl.Instance.InsertCombinedAstNumber(currenProduct.ProId, cust, "AST", custNum, this.Station, this.Editor);
             //    //}
             //    ////检查Declare @Cust3 = ModelInfo.Value (Conditon: Model=#Prodocut.Model and Name = ‘Cust3’) ，
             //    ////若@Cust3 不为空且不为Null，则执行下面AST的分配工作：
             //    //if (string.IsNullOrEmpty(astPo))
             //    //{
             //    //    string cust3 = currenProduct.ModelObj.GetAttribute("Cust3");
             //    //    if (!string.IsNullOrEmpty(cust3))
             //    //    {
             //    //        custNum3 = ActivityCommonImpl.Instance.CheckAndGetUsedAst(session, currenProduct.ProId, cust3, "AST", this.Station, this.Editor);

             //    //        if (custNum3 == null)
             //    //        {
             //    //            custNum3 = ActivityCommonImpl.Instance.CheckAndSetReleaseAstNumber(currenProduct.ProId, cust3, "AST", this.Station, this.Editor);
             //    //        }

             //    //        if (string.IsNullOrEmpty(custNum3))
             //    //        {
             //    //            custNum3 = GenerateCodeNew(currenProduct, customerSN, cust3);

             //    //            // Checksum
             //    //            custNum3 = ActivityCommonImpl.Instance.GetAstChecksum(cust3, custNum3);

             //    //            ConstValueInfo info3 = new ConstValueInfo();
             //    //            info3.type = "AST";
             //    //            info3.name = cust3;
             //    //            IList<ConstValueInfo> retList3 = partRepository.GetConstValueInfoList(info3);
             //    //            if (retList3 != null && retList3.Count != 0)
             //    //            {
             //    //                if (string.IsNullOrEmpty(retList3[0].value) == false)
             //    //                {
             //    //                    custNum3 += retList3[0].value;
             //    //                }
             //    //            }
             //    //            ActivityCommonImpl.Instance.InsertCombinedAstNumber(currenProduct.ProId, cust3, "AST", custNum3, this.Station, this.Editor);
             //    //        }
             //    //    }
             //    //}
             //    #endregion
             //    #endregion
             //}
             #endregion

             CurrentSession.AddValue("AssetSN", custNum??"");
            CurrentSession.AddValue("AssetSN3", custNum3??"");

            return base.DoExecute(executionContext);
        }

        #region mark by Vincent
        //private string GenerateCode(string cust)
        //{
        //    try
        //    {
        //        var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
        //        string customerSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);

        //        string custNum = "";

        //        IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        //        IList<AssetRangeInfo> assetSNRange = ipartRepository.GetAssetRangesByCode(cust);

        //        if (assetSNRange.Count == 0)
        //        {
        //            List<string> errpara = new List<string>();

        //            errpara.Add(customerSN);

        //            throw new FisException("CHK200", errpara);
        //        }

        //        AssetRangeInfo assetRange = null;
        //        foreach (AssetRangeInfo tempAssetRange in assetSNRange)
        //        {
        //            assetRange = tempAssetRange;
        //        }

        //        SqlTransactionManager.Begin();  //2012-7-19
        //        lock (_syncRoot_GetSeq)         //2012-7-19
        //        {
        //            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
        //            //ITC-1360-0495, Jessica Liu, 2012-2-22
        //            //string largestCustNum = numCtrlRepository.GetMaxNumber("AST", cust + "{0}");
        //            string largestCustNum = numCtrlRepository.GetMaxAssetNumber("AST", cust, "HP");

        //            if (string.IsNullOrEmpty(largestCustNum))
        //            {
        //                custNum = assetRange.begin;

        //                NumControl item = new NumControl(0, "AST", assetRange.code, custNum, "HP");

        //                //numCtrlRepository.SaveMaxNumber(item, true, string.Empty);
        //                numCtrlRepository.SaveMaxAssetNumber(item, true);

        //                //2012-7-19
        //                SqlTransactionManager.Commit();
        //            }
        //            else
        //            {
        //                string numInLargestCustNum = "";
        //                string numInEnd = "";
        //                string preString = "";
        //                string preEndString = "";
        //                bool bHasNumber = false;

        //                string s = largestCustNum;
        //                int s_idx = 0;
        //                bool bFoundDigit = false;
        //                for (int i = 0; i < s.Length; i++)
        //                {
        //                    if (s[i] < '0' || s[i] > '9')
        //                    {
        //                        bFoundDigit = false;
        //                    }
        //                    else if (bFoundDigit == false)
        //                    {
        //                        bFoundDigit = true;
        //                        s_idx = i;
        //                    }
        //                }

        //                if (bFoundDigit == true)
        //                {
        //                    numInLargestCustNum = s.Substring(s_idx);
        //                    preString = s.Substring(0, s_idx);
        //                    bHasNumber = true;
        //                }

        //                string t = assetRange.end;
        //                int t_idx = 0;
        //                bool bFoundDigit2 = false;
        //                for (int i = 0; i < t.Length; i++)
        //                {
        //                    if (t[i] < '0' || t[i] > '9')
        //                    {
        //                        bFoundDigit2 = false;
        //                    }
        //                    else if (bFoundDigit2 == false)
        //                    {
        //                        bFoundDigit2 = true;
        //                        t_idx = i;
        //                    }
        //                }

        //                if ((bFoundDigit2 == true) && (bHasNumber == true))
        //                {
        //                    numInEnd = t.Substring(t_idx);
        //                    preEndString = t.Substring(0, t_idx);
        //                }
        //                else
        //                {
        //                    bHasNumber = false;
        //                }

        //                if (bHasNumber == true)
        //                {
        //                    if (preString == preEndString)
        //                    {
        //                        long largestNum = long.Parse(numInLargestCustNum);
        //                        long end = long.Parse(numInEnd);

        //                        if (largestNum >= end)
        //                        {
        //                            List<string> errpara = new List<string>();

        //                            errpara.Add(customerSN);

        //                            throw new FisException("CHK201", errpara);
        //                        }
        //                        else
        //                        {
        //                            largestNum++;
        //                            //custNum = preString + largestNum.ToString();
        //                            int largestNumCount = numInLargestCustNum.Length;
        //                            string strNewLargestCustNum = largestNum.ToString();
        //                            int newlargestNumCount = strNewLargestCustNum.Length;
        //                            string strPreZero = "";
        //                            if (numInLargestCustNum.Substring(0, 1) == "0")
        //                            {
        //                                for (int i = 0; i < (largestNumCount - newlargestNumCount); i++)
        //                                {
        //                                    strPreZero += "0";
        //                                }

        //                                strNewLargestCustNum = strPreZero + strNewLargestCustNum;
        //                            }
        //                            custNum = preString + strNewLargestCustNum;

        //                            NumControl item = new NumControl(0, "AST", assetRange.code, custNum, "HP");
        //                            //numCtrlRepository.SaveMaxNumber(item, false, string.Empty);
        //                            numCtrlRepository.SaveMaxAssetNumber(item, false);

        //                            //2012-7-19
        //                            SqlTransactionManager.Commit();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        List<string> errpara = new List<string>();

        //                        errpara.Add(customerSN);

        //                        throw new FisException("CHK201", errpara);
        //                    }
        //                }
        //                else
        //                {
        //                    List<string> errpara = new List<string>();

        //                    errpara.Add(customerSN);

        //                    throw new FisException("CHK201", errpara);
        //                }
        //            }
        //        }

        //        if (cust == "SCUSTA-1")
        //        {
        //            custNum = "000" + custNum.Trim() + "00";
        //        }

        //        return custNum;
        //    }
        //    catch (Exception e)     //2012-7-19
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw e;
        //    }
        //    finally                 //2012-7-19
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}
        #endregion
        #region disable code ; move to utl
        //private string GenerateCodeNew(IProduct currenProduct, string customerSN, string cust)
        //{
        //    try
        //    {
               
        //        //var currenProduct = (IProduct)session.GetValue(Session.SessionKeys.Product);
        //        //string customerSN = (string)session.GetValue(Session.SessionKeys.CustSN);
             

        //        string custNum = "";                
        //        string numType = "AST";

        //        IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

        //        INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                
        //        SqlTransactionManager.Begin();
        //        lock (_syncRoot_GetSeq) 
        //        {
        //            AssetRangeCodeInfo currentRange = ipartRepository.GetAssetRangeByStatus(cust, new string[] { "A", "R" });

        //            if (currentRange == null)
        //            {
        //               throw new FisException("CHK200", new string[] { customerSN });
        //            }

        //            NumControl currentMaxNum = numCtrlRepository.GetMaxValue(numType, cust);
                     
        //            if (currentMaxNum==null)
        //            {
        //                #region 第一次產生Serial Number
        //                //Check new Range
        //                CheckAssetNum(customerSN,currentRange.Begin, currentRange.End);
        //                currentMaxNum = new NumControl();
        //                currentMaxNum.NOName = cust;
        //                currentMaxNum.NOType = numType;
        //                currentMaxNum.Value = currentRange.Begin;
        //                currentMaxNum.Customer =this.Customer;

        //                IUnitOfWork uof = new UnitOfWork();
        //                numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
        //                if (currentMaxNum.Value == currentRange.End)
        //                {
        //                    ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
        //                }
        //                else
        //                {
        //                    ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
        //                }
        //                uof.Commit();
        //                SqlTransactionManager.Commit();
        //                custNum= currentMaxNum.Value;
        //                #endregion
        //            }
        //            else
        //            {
        //                #region  don't check end number
        //                //if (currentMaxNum.Value == currentRange.End)
        //                //{
        //                //    ipartRepository.SetAssetRangeStatus(currentRange.ID, "C");
        //                //    currentRange = ipartRepository.GetAssetRangeByStatus(cust, new string[] { "R" });
        //                //    if (currentRange==null ||
        //                //        currentMaxNum.Value.Equals(currentRange.Begin) || 
        //                //        currentMaxNum.Value.Equals(currentRange.End))
        //                //    {
        //                //        throw new FisException("CHK200", new string[] { customerSN });
        //                //    }
        //                //    else
        //                //    {
        //                //        #region 更換新Range產生Serial Number
        //                //        //Check new Range
        //                //        CheckAssetNum(customerSN, currentRange.Begin, currentRange.End);
        //                //        IUnitOfWork uof = new UnitOfWork();
        //                //        currentMaxNum.Value = currentRange.Begin;
        //                //        numCtrlRepository.Update(currentMaxNum, uof);
        //                //        ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
        //                //        uof.Commit();
        //                //        SqlTransactionManager.Commit();
        //                //        custNum = currentMaxNum.Value;
        //                //        #endregion
        //                //    }
        //                //}
        //                //else 
        //                #endregion
        //                if (currentRange.Status == "R" && 
        //                    checkNewRange(currentMaxNum.Value, currentRange.Begin,currentRange.End))
        //                {
        //                    #region 更換新Range產生Serial Number
        //                    //Check new Range
        //                    CheckAssetNum(customerSN, currentRange.Begin, currentRange.End);
        //                    IUnitOfWork uof = new UnitOfWork();
        //                    currentMaxNum.Value = currentRange.Begin;
        //                    numCtrlRepository.Update(currentMaxNum, uof);
        //                    if (currentRange.Begin.CompareTo(currentRange.End)<0)
        //                    {
        //                        ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");                               
        //                    }
        //                    else
        //                    {
        //                        ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
        //                    }
        //                    uof.Commit();
        //                    SqlTransactionManager.Commit();
        //                    custNum = currentMaxNum.Value;
        //                    #endregion
        //                }
        //                else
        //                {
        //                    currentMaxNum.Value = GenNextAssetNum(customerSN, currentMaxNum.Value, currentRange.End);
        //                    IUnitOfWork uof = new UnitOfWork();
        //                    numCtrlRepository.Update(currentMaxNum, uof);
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
        //                }                       
        //            }
        //        }

        //        if (cust == "SCUSTA-1")
        //        {
        //            custNum = "000" + custNum + "00";
        //        }

        //        return custNum;
        //    }
        //    catch (Exception e)     //2012-7-19
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw e;
        //    }
        //    finally                 //2012-7-19
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}      

        //private string GenNextAssetNum(string custSN, string lastNum, string rangeMaxNum)
        //{

        //    //Get last num PreStr & Sequence number
        //    string lastAssetNum = "";
        //    string lastPreString = "";
        //    string EndNum = "";
        //    string preEndString = "";
          
        //    Match lastNumMatch =null;
        //    Match endNumMatch=null; 
        //    CheckAssetNum(custSN, lastNum, rangeMaxNum, ref lastNumMatch, ref endNumMatch);

        //    lastAssetNum=lastNumMatch.Value;
        //    lastPreString=lastNum.Substring(0,lastNumMatch.Index);
        //    EndNum=endNumMatch.Value;
        //    preEndString = rangeMaxNum.Substring(0, endNumMatch.Index);
        //    long largestNum = long.Parse(lastAssetNum);
        //    long end = long.Parse(EndNum);
        //    largestNum++;
        //    int largestNumCount = lastAssetNum.Length;
        //    string strNewLargestCustNum = largestNum.ToString().PadLeft(largestNumCount, '0');
        //    return preEndString + strNewLargestCustNum;  
           
        //}

        //private void CheckAssetNum(string custSN, 
        //                                                string lastNum, 
        //                                                string rangeMaxNum, 
        //                                                ref Match lastNumMatch,
        //                                                ref Match endNumMatch)
        //{
           
        //    string lastPreString = "";           
        //    string preEndString = "";

        //    if (lastNum.Length != rangeMaxNum.Length)
        //    {
        //        throw new FisException("CHK201", new string[] { custSN });
        //    }

        //    MatchCollection matches = Regex.Matches(lastNum, @"\d+");
        //    if (matches.Count == 0)
        //    {
        //        throw new FisException("CHK201", new string[] { custSN });
        //    }
        //    lastNumMatch = matches[matches.Count - 1];

        //    matches = Regex.Matches(rangeMaxNum, @"\d+");
        //    if (matches.Count == 0)
        //    {
        //        throw new FisException("CHK201", new string[] { custSN });
        //    }
        //    endNumMatch = matches[matches.Count - 1];

        //    if (lastNumMatch.Index != endNumMatch.Index ||
        //        lastNumMatch.Length != endNumMatch.Length)
        //    {
        //        throw new FisException("CHK201", new string[] { custSN });
        //    }

        //    //最後不適數字
        //    if (lastNum.Length != (lastNumMatch.Index + lastNumMatch.Length))
        //    {
        //        throw new FisException("CHK201", new string[] { custSN });
        //    }

        //    lastPreString = lastNum.Substring(0, lastNumMatch.Index);
        //    preEndString = rangeMaxNum.Substring(0, endNumMatch.Index);

        //    if (lastPreString != preEndString)
        //    {
        //        throw new FisException("CHK201", new string[] { custSN });
        //    }

        //    if (lastNumMatch.Value.CompareTo(endNumMatch.Value) == 1)
        //    {
        //        throw new FisException("CHK201", new string[] { custSN });
        //    }

        //}

        //private void CheckAssetNum(string custSN,
        //                                                string lastNum,
        //                                                string rangeMaxNum)
        //{
        //    Match lastNumMatch = null;
        //    Match endNumMatch = null;
        //    CheckAssetNum(custSN, lastNum, rangeMaxNum, ref lastNumMatch, ref endNumMatch);
        //}


        //private bool checkNewRange(string maxNum, string beginNum, string endNum)
        //{
           
        //    int iBegin = beginNum.CompareTo(maxNum);
        //    int iEnd = endNum.CompareTo(maxNum);
        //    if ((iBegin == 1 && iEnd == -1 && maxNum.Length == beginNum.Length) || iBegin == 0 || iEnd == 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

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
        //        custNum = GenerateCodeNew(curProduct,curProduct.CUSTSN, cust);

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
