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
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<string> pnList = CurrentSession.GetValue("PnListOfATSN7") as IList<string>;
            bool bCDSI = (bool)CurrentSession.GetValue("bCDSI");

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

                            curProduct.AddPart(assetTag1);
                            productRepository.Update(curProduct, CurrentSession.UnitOfWork);
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

                            curProduct.AddPart(assetTag2);
                            productRepository.Update(curProduct, CurrentSession.UnitOfWork);
                        }
                    }
                }
            }
            else    //Not CDSI
            {
                string custNum = "";

                string cust = curProduct.ModelObj.GetAttribute("Cust");
                IList<AssetRangeInfo> assetRangeList = partRepository.GetAssetRangesByCode(cust);
                if (assetRangeList.Count == 0)  //No AssetRange found
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK200", errpara);
                }
                AssetRangeInfo assetRange = assetRangeList[0];

                if (assetRange.begin.Length != assetRange.end.Length)   //Length of Begin and End mismatch
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK201", errpara);
                }
                
                try
                {
                    SqlTransactionManager.Begin();
                    lock (_syncRoot_GetSeq)
                    {
                        INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                        string largestCustNum = numCtrlRepository.GetMaxAssetNumber("AST", cust, "HP");

                        if (string.IsNullOrEmpty(largestCustNum))
                        {
                            custNum = assetRange.begin;
                            //Save current custNum to numControl
                            NumControl item = new NumControl(0, "AST", assetRange.code, custNum, "HP");
                            numCtrlRepository.SaveMaxAssetNumber(item, true);
                            SqlTransactionManager.Commit();
                        }
                        else
                        {
                            if (largestCustNum.Length != assetRange.begin.Length)   //Length of largestCustNum and assetRange.begin mismatch
                            {
                                List<string> errpara = new List<string>();
                                throw new FisException("CHK201", errpara);
                            }

                            string prePartCur = "";
                            string numPartCur = "";
                            ParseAst(largestCustNum, out prePartCur, out numPartCur);
                            if (numPartCur == "")
                            {
                                List<string> errpara = new List<string>();
                                throw new FisException("CHK201", errpara);
                            }

                            string prePartMax = "";
                            string numPartMax = "";
                            ParseAst(assetRange.end, out prePartMax, out numPartMax);
                            if (numPartMax == "")
                            {
                                List<string> errpara = new List<string>();
                                throw new FisException("CHK201", errpara);
                            }

                            if (prePartCur != prePartMax)   //Prefix of largestCustNum and assetRange.end mismatch
                            {
                                List<string> errpara = new List<string>();
                                throw new FisException("CHK201", errpara);
                            }

                            long numCur = long.Parse(numPartCur);
                            long numMax = long.Parse(numPartMax);
                            if (numCur >= numMax)
                            {
                                List<string> errpara = new List<string>();
                                throw new FisException("CHK201", errpara);
                            }

                            numCur++;

                            custNum = prePartCur;
                            int fillZeroCount = numPartCur.Length - numCur.ToString().Length;
                            for (int i = 0; i < fillZeroCount; i++)
                            {
                                custNum += "0";
                            }
                            custNum += numCur.ToString();

                            //Save current custNum to numControl
                            NumControl item = new NumControl(0, "AST", assetRange.code, custNum, "HP");
                            numCtrlRepository.SaveMaxAssetNumber(item, false);
                            SqlTransactionManager.Commit();
                        }
                    }
                }
                catch (Exception e)
                {
                    SqlTransactionManager.Rollback();
                    throw e;
                }
                finally
                {
                    SqlTransactionManager.Dispose();
                    SqlTransactionManager.End();
                }

                //Update custNum
                if (cust == "SCUSTA-1")
                {
                    custNum = "000" + custNum.Trim() + "00";
                }
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

                ConstValueInfo cond = new ConstValueInfo();
                cond.type = "AST";
                cond.name = cust;
                IList<ConstValueInfo> valList = partRepository.GetConstValueInfoList(cond);
                if (valList.Count > 0 && !String.IsNullOrEmpty(valList[0].value))
                {
                    custNum += valList[0].value.Trim();
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

                    curProduct.AddPart(assetTag);
                    productRepository.Update(curProduct, CurrentSession.UnitOfWork);
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
    }
}
