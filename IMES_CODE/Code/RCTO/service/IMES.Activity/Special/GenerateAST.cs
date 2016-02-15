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
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string customerSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);

            string custNum = "";
            string cust = currenProduct.ModelObj.GetAttribute("Cust");
            //if(string.IsNullOrEmpty(cust)){
            //    return base.DoExecute(executionContext);
            //}

            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<AssetRangeInfo> assetSNRange = ipartRepository.GetAssetRangesByCode(cust);

            if (assetSNRange.Count == 0)
            {
                List<string> errpara = new List<string>();

                errpara.Add(customerSN);

                throw new FisException("CHK200", errpara);
            }

            AssetRangeInfo assetRange = null;
            foreach (AssetRangeInfo tempAssetRange in assetSNRange)
            {
                assetRange = tempAssetRange;
            }

            try         //2012-7-19
            {
                SqlTransactionManager.Begin();  //2012-7-19
                lock (_syncRoot_GetSeq)         //2012-7-19
                {
                    INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                    //ITC-1360-0495, Jessica Liu, 2012-2-22
                    //string largestCustNum = numCtrlRepository.GetMaxNumber("AST", cust + "{0}");
                    string largestCustNum = numCtrlRepository.GetMaxAssetNumber("AST", cust, "HP");

                    if (string.IsNullOrEmpty(largestCustNum))
                    {
                        custNum = assetRange.begin;

                        NumControl item = new NumControl(0, "AST", assetRange.code, custNum, "HP");
                        
                        //numCtrlRepository.SaveMaxNumber(item, true, string.Empty);
                        numCtrlRepository.SaveMaxAssetNumber(item, true);

                        //2012-7-19
                        SqlTransactionManager.Commit();
                    }
                    else
                    {
                        string numInLargestCustNum = "";
                        string numInEnd = "";
                        string preString = "";
                        string preEndString = "";
                        bool bHasNumber = false;

                        string s = largestCustNum;
                        int s_idx = 0;
                        bool bFoundDigit = false;
                        for (int i = 0; i < s.Length; i++)
                        {
                            if (s[i] < '0' || s[i] > '9')
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
                            numInLargestCustNum = s.Substring(s_idx);
                            preString = s.Substring(0, s_idx);   
                            bHasNumber = true;
                        }

                        string t = assetRange.end;
                        int t_idx = 0;
                        bool bFoundDigit2 = false;
                        for (int i = 0; i < t.Length; i++)
                        {
                            if (t[i] < '0' || t[i] > '9')
                            {
                                bFoundDigit2 = false;
                            }
                            else if (bFoundDigit2 == false)
                            {
                                bFoundDigit2 = true;
                                t_idx = i;
                            }
                        }

                        if ((bFoundDigit2 == true) && (bHasNumber == true))
                        {
                            numInEnd = t.Substring(t_idx);
                            preEndString = t.Substring(0, t_idx);
                        }
                        else
                        {
                            bHasNumber = false;
                        }

                        if (bHasNumber == true)
                        {
                            if (preString == preEndString)
                            {
                                long largestNum = long.Parse(numInLargestCustNum);
                                long end = long.Parse(numInEnd);

                                if (largestNum >= end)
                                {
                                    List<string> errpara = new List<string>();

                                    errpara.Add(customerSN);

                                    throw new FisException("CHK201", errpara);
                                }
                                else
                                {
                                    largestNum++;
                                    //custNum = preString + largestNum.ToString();
                                    int largestNumCount = numInLargestCustNum.Length;
                                    string strNewLargestCustNum = largestNum.ToString();
                                    int newlargestNumCount = strNewLargestCustNum.Length;
                                    string strPreZero = "";
                                    if (numInLargestCustNum.Substring(0, 1) == "0")
                                    {
                                        for (int i = 0; i < (largestNumCount - newlargestNumCount); i++)
                                        {
                                            strPreZero += "0";
                                        }

                                        strNewLargestCustNum = strPreZero + strNewLargestCustNum;
                                    }
                                    custNum = preString + strNewLargestCustNum;

                                    NumControl item = new NumControl(0, "AST", assetRange.code, custNum, "HP");
                                    //numCtrlRepository.SaveMaxNumber(item, false, string.Empty);
                                    numCtrlRepository.SaveMaxAssetNumber(item, false);

                                    //2012-7-19
                                    SqlTransactionManager.Commit();
                                }
                            }
                            else
                            {
                                List<string> errpara = new List<string>();

                                errpara.Add(customerSN);

                                throw new FisException("CHK201", errpara);
                            }                        
                        }
                        else
                        {
                            List<string> errpara = new List<string>();

                            errpara.Add(customerSN);

                            throw new FisException("CHK201", errpara);
                        }                    
                    }
                }
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
            /*
            else
            {
                long largestNum = long.Parse(largestCustNum);
                long end = long.Parse(assetRange.end);

                if (largestNum == end)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(customerSN);

                    throw new FisException("CHK201", errpara);
                }

                largestNum++;
                custNum = largestNum.ToString();

                NumControl item = new NumControl(0, "AST", assetRange.code, custNum, "HP");
                //numCtrlRepository.SaveMaxNumber(item, false, string.Empty);
                numCtrlRepository.SaveMaxAssetNumber(item, false);
            } 
            */

            /* 2012-7-14, Jessica Liu, for mantis
            if (cust == "SCUSTA-1")
            {
                custNum = "000" + custNum.Trim() + "00";
            }
            else if (cust == "CUSTW-1")
            {
                custNum = custNum.Trim() + " HQ P47";
            }
            else if (cust == "CUSTW-2")
            {
                custNum = custNum.Trim() + " HQ P49";
            }
            */
            if (cust == "SCUSTA-1")
            {
                custNum = "000" + custNum.Trim() + "00";
            }
            ConstValueInfo info = new ConstValueInfo();
            info.type = "AST";
            info.name = cust;
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
            if (retList != null && retList.Count != 0)
            {
                if (string.IsNullOrEmpty(retList[0].value) == false)
                {
                    custNum += retList[0].value;
                }
            }

            CurrentSession.AddValue("AssetSN", custNum);

            return base.DoExecute(executionContext);
        }
	}
}
