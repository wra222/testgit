/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Check AST Combine activity for AFTMVS Page
 *                 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc202017             Create
 * Known issues:
*/
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// Check if AST combined.
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC AFT MVS.docx
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     详见UC
    /// </para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.RandomInspectionStation
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IFamilyRepository
    ///         IModelRepository
    /// </para> 
    /// </remarks>
    public partial class CheckASTCombine : BaseActivity
    {
        ///<summary>
        ///</summary>
        public CheckASTCombine()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check if AST combined.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        /*
         * Answer to: ITC-1360-1064
         * Description: Improve function of CheckASTCombine.
         */
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProduct CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string ast = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);
            string mps = (string)CurrentSession.GetValue(Session.SessionKeys.MatchedParts);
            string crc = (string)CurrentSession.GetValue("CRCOfProductID");
            if (mps.Contains("," + ast + ","))
            {
                List<string> errpara = new List<string>();
                errpara.Add(ast);
                FisException e = new FisException("CHK258", errpara);//already match
                e.stopWF = false;
                throw e;
            }

            //Check if matches Customer SN
            bool bCheckedCustSN = (bool)CurrentSession.GetValue("bMatchSN");
            if (!bCheckedCustSN)
            {
                if (ast == CurrentProduct.CUSTSN)
                {
                    CurrentSession.AddValue(Session.SessionKeys.MatchedCheckItem, "Customer SN");
                    CurrentSession.AddValue("bMatchSN", true);
                    CurrentSession.AddValue(Session.SessionKeys.MatchedParts, mps + ast + ",");
                    return base.DoExecute(executionContext);
                }
            }

            //Check if matches Product ID
            bool bCheckedProdID = (bool)CurrentSession.GetValue("bMatchID");
            if (!bCheckedProdID)
            {
                if (ast == (crc + CurrentProduct.ProId))
                {
                    CurrentSession.AddValue(Session.SessionKeys.MatchedCheckItem, "Product ID");
                    CurrentSession.AddValue("bMatchID", true);
                    CurrentSession.AddValue(Session.SessionKeys.MatchedParts, mps + ast + ",");
                    return base.DoExecute(executionContext);
                }
            }

            //Check if matches other AST
            IList<BomItemInfo> sessionBOM = CurrentSession.GetValue(Session.SessionKeys.SessionBom) as List<BomItemInfo>;

            bool bFound = false;
            foreach (BomItemInfo ele in sessionBOM)
            {
                if (ele.description == "ATSN1" || ele.description == "ATSN3")
                {
                    foreach (PartNoInfo info in ele.parts)
                    {
                        if (info.id == ast)
                        {
                            CurrentSession.AddValue(Session.SessionKeys.MatchedCheckItem, ele.PartNoItem);
                            CurrentSession.AddValue(Session.SessionKeys.MatchedParts, mps + ast + ",");
                            bFound = true;
                            ele.parts.Remove(info);
                            break;
                        }
                    }
                }
                else    //PP,ATSN2,Smart card reader,Master Label,ICASA Label
                {
                    string thisPN = "," + ele.PartNoItem + ",";
                    string toComp = "," + ast + ",";
                    if (thisPN.Contains(toComp))
                    {
                        if (ele.qty <= ele.scannedQty)
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(ast);
                            FisException e = new FisException("CHK259", errpara);//enough
                            e.stopWF = false;
                            throw e;
                        }
                        else
                        {
                            CurrentSession.AddValue(Session.SessionKeys.MatchedCheckItem, ele.PartNoItem);
                            BomItemInfo tmp = ele;
                            sessionBOM.Remove(ele);
                            tmp.scannedQty++;
                            sessionBOM.Add(tmp);
                            bFound = true;
                            break;
                        }
                    }
                }
                if (bFound) break;
            }

            if (!bFound)
            {
                FisException e = new FisException("CHK199", new string[] { });
                e.stopWF = false;
                throw e;
            }

            return base.DoExecute(executionContext);
        }
    }
}
