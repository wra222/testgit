/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for Asset Tag Label Print Page
 * UI:CI-MES12-SPEC-PAK-UI Asset Tag Label Print.docx –2011/11/28 
 * UC:CI-MES12-SPEC-PAK-UC Asset Tag Label Print.docx –2011/11/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0747, Jessica Liu, 2012-2-25
* ITC-1360-0856, Jessica Liu, 2012-3-2
* ITC-1360-1574, Jessica Liu, 2012-3-27
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




namespace IMES.Activity
{
    /// <summary>
    /// 查询有否Asset Tag Label，有则成功；没有则产生并将其与product绑定
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         Asset Tag Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         查找asset tag label数据
    ///         找不到则创建，并将其与product绑定
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
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
    ///         IPartRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateAssetTagLabel : BaseActivity
	{
        ///<summary>
        ///</summary>
        public GenerateAssetTagLabel()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 查询有否Asset Tag Label，有则成功；没有则产生并将其与product绑定
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
/*//test，2011-10-18，测试流程是否通，需去掉========
return base.DoExecute(executionContext);
//test，2011-10-18，测试流程是否通，需去掉========*/

// /* 2011-12-14，为了编译通过暂时注释掉            
            string custNum = "";

            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
		    string customerSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);

            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
            IList<IBOMNode> bomNodes = bom.GetNodesByNodeType("AT");
            List<string> partNos = new List<string>();

            foreach (IBOMNode bomNode in bomNodes)
            {
                partNos.Add(bomNode.Part.PN);
            }

            string[] partNosList = partNos.ToArray();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<IProductPart> tempProductPartList = productRepository.GetProductPartsByPartNosAndProdId(partNosList, currenProduct.ProId);
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            //--------------------------------------------------------------------------------------------------------
            //UpdateBorrowLog by Kaisheng,Zhang 2012/04/21 -UC Update:
            //取消产生Asset SN 步骤
            if (tempProductPartList.Count == 0)
            {
                custNum = "null";
            }
            else
            {
                custNum = tempProductPartList[0].PartSn;
            }
            CurrentSession.AddValue("AssetSN", custNum);
            return base.DoExecute(executionContext);
            //以下是產生Asset SN 的代碼，已註釋掉
            //-----------------------------------------------------------------------------------------------------------
            
            //if (tempProductPartList.Count == 0)
            //{
            //    var cust = currenProduct.ModelObj.GetAttribute("Cust");

            //    //ITC-1360-1574, Jessica Liu, 2012-3-27
            //    if (cust == null)
            //    {
            //        List<string> errpara = new List<string>();

            //        errpara.Add(customerSN);

            //        throw new FisException("CHK186", errpara);
            //    }

            //    IList<AssetRangeInfo> assetSNRange = ipartRepository.GetAssetRangesByCode(cust);

            //    if (assetSNRange.Count == 0)
            //    {
            //        List<string> errpara = new List<string>();

            //        errpara.Add(customerSN);

            //        throw new FisException("CHK200", errpara);
            //    }

            //    AssetRangeInfo assetRange = null;
            //    foreach (AssetRangeInfo tempAssetRange in assetSNRange)
            //    {
            //        assetRange = tempAssetRange;
            //    }

            //    //ITC-1360-0747, Jessica Liu, 2012-2-25
            //    INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            //    //string largestCustNum = numCtrlRepository.GetMaxNumber("AST", cust + "{0}");
            //    string largestCustNum = numCtrlRepository.GetMaxAssetNumber("AST", cust, "HP");
            //    if (string.IsNullOrEmpty(largestCustNum))
            //    {
            //        custNum = assetRange.begin;

            //        NumControl item = new NumControl(0, "AST", assetRange.code, custNum, "HP");
            //        //numCtrlRepository.SaveMaxNumber(item, true, string.Empty);
            //        numCtrlRepository.SaveMaxAssetNumber(item, true);
            //    }
            //    else
            //    {
            //        string numInLargestCustNum = "";
            //        string numInEnd = "";
            //        string preString = "";
            //        string preEndString = "";
            //        bool bHasNumber = false;

            //        string s = largestCustNum;
            //        int s_idx = 0;
            //        bool bFoundDigit = false;
            //        for (int i = 0; i < s.Length; i++)
            //        {
            //            if (s[i] < '0' || s[i] > '9')
            //            {
            //                bFoundDigit = false;
            //            }
            //            else if (bFoundDigit == false)
            //            {
            //                bFoundDigit = true;
            //                s_idx = i;
            //            }
            //        }

            //        if (bFoundDigit == true)
            //        {
            //            numInLargestCustNum = s.Substring(s_idx);
            //            preString = s.Substring(0, s_idx);   
            //            bHasNumber = true;
            //        }

            //        string t = assetRange.end;
            //        int t_idx = 0;
            //        bool bFoundDigit2 = false;
            //        for (int i = 0; i < t.Length; i++)
            //        {
            //            if (t[i] < '0' || t[i] > '9')
            //            {
            //                bFoundDigit2 = false;
            //            }
            //            else if (bFoundDigit2 == false)
            //            {
            //                bFoundDigit2 = true;
            //                t_idx = i;
            //            }
            //        }

            //        if ((bFoundDigit2 == true) && (bHasNumber == true))
            //        {
            //            numInEnd = t.Substring(t_idx);
            //            preEndString = t.Substring(0, t_idx);
            //        }
            //        else
            //        {
            //            bHasNumber = false;
            //        }

            //        if (bHasNumber == true)
            //        {
            //            if (preString == preEndString)
            //            {
            //                long largestNum = long.Parse(numInLargestCustNum);
            //                long end = long.Parse(numInEnd);

            //                if (largestNum >= end)
            //                {
            //                    List<string> errpara = new List<string>();

            //                    errpara.Add(customerSN);

            //                    throw new FisException("CHK201", errpara);
            //                }
            //                else
            //                {
            //                    largestNum++;

            //                    //ITC-1360-1323, Jessica Liu, 2012-3-10
            //                    //custNum = preString + largestNum.ToString();
            //                    int largestNumCount = numInLargestCustNum.Length;
            //                    string strNewLargestCustNum = largestNum.ToString();
            //                    int newlargestNumCount = strNewLargestCustNum.Length;
            //                    string strPreZero = "";
            //                    if (numInLargestCustNum.Substring(0, 1) == "0")
            //                    {
            //                        for (int i = 0; i < (largestNumCount - newlargestNumCount); i++)
            //                        {
            //                            strPreZero += "0";
            //                        }

            //                        strNewLargestCustNum = strPreZero + strNewLargestCustNum;
            //                    }
            //                    custNum = preString + strNewLargestCustNum;

            //                    NumControl item = new NumControl(0, "AST", assetRange.code, custNum, "HP");
            //                    //numCtrlRepository.SaveMaxNumber(item, false, string.Empty);
            //                    numCtrlRepository.SaveMaxAssetNumber(item, false);
            //                }
            //            }
            //            else
            //            {
            //                List<string> errpara = new List<string>();

            //                errpara.Add(customerSN);

            //                throw new FisException("CHK201", errpara);
            //            }                        
            //        }
            //        else
            //        {
            //            List<string> errpara = new List<string>();

            //            errpara.Add(customerSN);

            //            throw new FisException("CHK201", errpara);
            //        }                    
            //    }

            //    if (cust == "SCUSTA-1")
            //    {
            //        custNum = "000" + custNum.Trim() + "00";
            //    }
            //    else if (cust == "CUSTW-1")
            //    {
            //        custNum = custNum.Trim() + " HQ P47";
            //    }
            //    else if (cust == "CUSTW-2")
            //    {
            //        custNum = custNum.Trim() + " HQ P49";
            //    }

            //    //CurrentSession.AddValue("AssetSN", custNum);
            //    //ITC-1360-0856, Jessica Liu, 2012-3-2
            //    IProductPart newPart = new ProductPart();
            //    newPart.BomNodeType = "AT";
            //    newPart.Iecpn = string.Empty;
            //    newPart.CustomerPn = string.Empty;
            //    newPart.ProductID = currenProduct.ProId;
            //    newPart.PartID = partNosList[0]; //part.PN;
            //    newPart.PartSn = custNum;
            //    newPart.PartType = "AT";
            //    newPart.Station = "81";
            //    newPart.Editor = Editor;
            //    newPart.Cdt = DateTime.Now;
            //    newPart.Udt = DateTime.Now;
            //    currenProduct.AddPart(newPart);
            //    productRepository.Update(currenProduct, CurrentSession.UnitOfWork);
            //}
            //else
            //{
            //    custNum = tempProductPartList[0].PartSn;
            //}

            //CurrentSession.AddValue("AssetSN", custNum);


            //* ITC-1360-0856, Jessica Liu, 2012-3-2
            //IProductPart newPart = new ProductPart();
            //newPart.BomNodeType = "AT";
            //newPart.Iecpn = string.Empty; 
            //newPart.CustomerPn = string.Empty; 
            //newPart.ProductID = currenProduct.ProId;
            //newPart.PartID = partNosList[0]; //part.PN;
            //newPart.PartSn = custNum;
            //newPart.PartType = "AT";
            //newPart.Station = "81";
            //newPart.Editor = Editor;
            //newPart.Cdt = DateTime.Now;
            //newPart.Udt = DateTime.Now;
            //currenProduct.AddPart(newPart);
            //productRepository.Update(currenProduct, CurrentSession.UnitOfWork);
            //*/
           
            //return base.DoExecute(executionContext);
//*/
        }
	}
}
