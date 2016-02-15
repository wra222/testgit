// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   Kerwin                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.Common;

namespace IMES.Activity
{

    /// <summary>
    /// 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Asset Tag Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.判断当前Product的Product.DeliveryNo 在DeliveryInfo分别查找InfoType= CustPo / IECSo对应的InfoValue，
    ///           其中某一个得不到时，都需要报错:DN資料不全CustPo/IECSo,請檢查一下船務
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK007
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckDNForAssetTagPrint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckDNForAssetTagPrint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断当前Product的Product.DeliveryNo 在DeliveryInfo分别查找InfoType= CustPo / IECSo对应的InfoValue，
        /// 其中某一个得不到时，都需要报错:DN資料不全CustPo/IECSo,請檢查一下船務
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IList<AstDefineInfo> astDefineInfoList = utl.IsNull<IList<AstDefineInfo>>(session, Session.SessionKeys.NeedCombineAstDefineList);
            IList<IPart> astPartList = utl.IsNull<IList<IPart>>(session, Session.SessionKeys.NeedCombineAstPartList);
            bool needCheckDN = false;
            bool allShippingAst = false;          
            //IList<AstDefineInfo> removeAstDefineInfoList = null;
            if (astDefineInfoList.Count > 0 &&
                utl.HasShippingAstTag(astDefineInfoList))
            {
                needCheckDN = true;
                allShippingAst = utl.AllShippingAstTag(astDefineInfoList);               
            }
            //从Session里取得Product对象
            Product CurrentProduct = (Product)session.GetValue(Session.SessionKeys.Product);

            /* 2012-9-3, Jessica Liu, for UC Change
            IDeliveryRepository currentDNRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            string CustPo = currentDNRepository.GetDeliveryInfoValue(CurrentProduct.DeliveryNo, "CustPo");
            if (string.IsNullOrEmpty(CustPo))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(CurrentProduct.ProId);
                ex = new FisException("PAK007", erpara);
                throw ex;
            }

            string IECSo = currentDNRepository.GetDeliveryInfoValue(CurrentProduct.DeliveryNo, "IECSo");
            if (string.IsNullOrEmpty(IECSo))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(CurrentProduct.ProId);
                ex = new FisException("PAK007", erpara);
                throw ex;
            }
            */

            //Update by Kaisheng,Zhang -2012/04/21 
            //UC Update:
            //------------------------------------------------
            //E.	如果该Product 不需要列印Asset Tag Label，则报告错误：“该产品不需要列印Asset Tag Label!”
            //條件：如果ModelBOM 的直接下阶不存在BomNodeType = 'AT'，并且Descr in ('ATSN4','ATSN7','ATSN8') 的Part 时，则该Product 不需要列印Asset Tag Label
            
            #region disable code
            //IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            //IHierarchicalBOM curBOM = null;
            //bool printFlag = false;
            //curBOM = bomRepository.GetHierarchicalBOMByModel(CurrentProduct.Model);
            //IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
            //bomNodeLst = curBOM.FirstLevelNodes;
            //if (bomNodeLst != null && bomNodeLst.Count > 0)
            //{
            //    foreach (IBOMNode ibomnode in bomNodeLst)
            //    {  //(BomNodeType='PP' and PartType='Asstage-3' CQ Mantis 0000321
            //        if (
            //              (ibomnode.Part.Type == "Asstage-3" && ibomnode.Part.BOMNodeType=="PP") ||
            //              (
            //                (ibomnode.Part.BOMNodeType.ToUpper() == "AT") && ((ibomnode.Part.Descr.ToUpper() == "ATSN4")||
            //                (ibomnode.Part.Descr.ToUpper() == "ATSN7")||(ibomnode.Part.Descr.ToUpper() == "ATSN8"))
            //              )
            //           )
            //        {
            //            printFlag = true;
            //            break;
            //        }
            //    }
            //}
            //if (printFlag == false)
            //{
            //    FisException ex;
            //    List<string> erpara = new List<string>();
            //    erpara.Add(this.Key);
            //    ex = new FisException("PAK097", erpara);// No need to print Asset Tag Label!--modify code
            //    throw ex;
            //}
            #endregion

            //------------------------------------------------

            //2012-9-6, Jessica Liu
            if (needCheckDN)
            {
                if (string.IsNullOrEmpty(CurrentProduct.DeliveryNo))
                {
                    if (allShippingAst)
                    {
                        //List<string> errpara = new List<string>();

                        //errpara.Add(CurrentProduct.CUSTSN);

                        throw new FisException("CHK955", new List<string> { CurrentProduct.CUSTSN });  //该Product尚未结合Delivery,不能列印Asset Label!
                    }
                    else   //剔除Shipping這的打印
                    {
                        astDefineInfoList = astDefineInfoList.Where(x => !utl.IsShippingAstTag(x)).ToList();
                        if (astDefineInfoList.Count == 0)
                        {
                            throw new FisException("CHK955", new List<string> { CurrentProduct.CUSTSN });
                        }
                        astPartList = astPartList.Where(x => astDefineInfoList.Any(y => y.AstCode == x.Descr)).ToList();
                        session.AddValue(Session.SessionKeys.NeedCombineAstDefineList, astDefineInfoList);
                        session.AddValue(Session.SessionKeys.NeedCombineAstPartList, astPartList);
                        //session.AddValue("NeedShippingAstDefineList", removeAstDefineInfoList);
                    }
                }
            }
            //ITC-1423-0024, Jessica Liu, 2012-10-12
            //如果该Product 已经列印过，则报告错误：“该Product 已经列印过Asset Tag Label，请使用Reprint 功能!”
            //ProductLog存在Station='PKAT' and Status=1 and Line= 'ATSN Print'的记录，则表明已经列印过

            #region disable code
            //IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //IList<ProductLog> logList = productRepository.GetProductLogs("PKAT", 1, "ATSN Print", CurrentProduct.ProId);
            //if (logList != null && logList.Count > 0)
            //{
            //    throw new FisException("CHK961", new string[] { });
            //}
            #endregion

            return base.DoExecute(executionContext);
        }

    }
}

