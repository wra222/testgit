/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for Asset Tag Label Reprint Page
 * UI:CI-MES12-SPEC-PAK-UI Asset Tag Label Print.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC Asset Tag Label Print.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* ITC-1360-1329, Jessica Liu, 2012-3-12
*/

using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using IMES.FisObject.Common.Misc;   
//using IMES.FisObject.Common.NumControl;
using Metas = IMES.Infrastructure.Repository._Metas;
using IMES.Common;


namespace IMES.Activity
{
    /// <summary>
    /// 判断当前Customer SN对应的Asset Tag Label能否支持再打印
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Asset Tag Label RePrint
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.判断当前Customer SN对应的Asset Tag Label能否支持再打印
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK206
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.custsn
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.PrintLogDescr
    ///         Session.PrintLogBegNo
    ///         Session.PrintLogEndNo
    ///         Session.PrintLogName
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
    public partial class CheckAssetSNReprint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckAssetSNReprint()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// check Asset Tag Label是否可以重印
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;

            IProduct currentProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
             //Product currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product; 
            
           // var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //string currentStation = "PKAT"; //2012-7-16,change "81" to "PKAT"
            //var currentStatus = 1;
            //string currentLine = "ATSN Print";
            //ITC-1360-1329, Jessica Liu, 2012-3-12
            //IList<ProductLog> productLogList = productRepository.GetProductLogs(currentStation, currentStatus, currentLine);
            //IList<ProductLog> productLogList = productRepository.GetProductLogs(currentStation, currentStatus, currentLine, currentProduct.ProId);
            IList<ProductLog> productLogList = currentProduct.ProductLogs;
            productLogList = productLogList.Where(x => x.Station == this.Station && 
                                                                        x.Status == IMES.FisObject.Common.Station.StationStatus.Pass).ToList();
            if (productLogList.Count == 0)     
            {               
                List<string> errpara = new List<string>();

                errpara.Add(currentProduct.CUSTSN);

                throw new FisException("CHK206", errpara); //？？？需要修改
            }
            //else if (string.IsNullOrEmpty(currentProduct.DeliveryNo))   //2012-9-6, Jessica Liu
            //{
            //    List<string> errpara = new List<string>();

            //    errpara.Add(currentProduct.CUSTSN);

            //    throw new FisException("CHK955", errpara);  //该Product尚未结合Delivery,不能列印Asset Label!
            //}
            else 
            {
                IList<AstDefineInfo> needCombineAstList = utl.IsNull<IList<AstDefineInfo>>(session, Session.SessionKeys.NeedCombineAstDefineList);
                if (needCombineAstList.Count == 0)
                {
                    throw new FisException("CHK205", new List<string> { currentProduct.ProId, this.Station });   // 此机器不需要print asset Label !
                }

                #region Check Shipping Ast Label need DeliveryNo
               // if (needCombineAstList.Any(x => x.AstLocation.ToLower() == "shipping"))
                if (utl.HasShippingAstTag(needCombineAstList))
                {
                    if (string.IsNullOrEmpty(currentProduct.DeliveryNo))   //2012-9-6, Jessica Liu
                    {
                        List<string> errpara = new List<string>();

                        errpara.Add(currentProduct.CUSTSN);

                        throw new FisException("CHK955", errpara);  //该Product尚未结合Delivery,不能列印Asset Label!
                    }
                }
                #endregion
               

                #region print log session
                string partSn = "";
                foreach (IProductPart combinedPart in currentProduct.ProductParts)
                {
                    //if (tempProductPart.BomNodeType == "AT" && productLogList.Any(x=>x.Line ==tempProductPart.PartType || 
                    //                                                                                                     x.Line == currentLine ||
                    //                                                                                                     x.Line == currentProduct.Status.Line))
                    if (needCombineAstList.Any(x => (x.HasCDSIAst=="Y" || x.NeedAssignAstSN == "Y")
                                                                       && x.AstCode == combinedPart.PartType))
                    {
                        partSn = combinedPart.PartSn;
                        break;
                    }
                }

                session.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                session.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);

                //UC让自定义一个
                session.AddValue(Session.SessionKeys.PrintLogName, string.Join(",", needCombineAstList.Select(x => x.AstCode).ToArray())); 
                //UC指出写为空
                session.AddValue(Session.SessionKeys.PrintLogDescr,  partSn);                
              
                #endregion
            }

            return base.DoExecute(executionContext);


            /*
            Product currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product;
            
            PrintLog condition = new PrintLog();
            condition.BeginNo = currentProduct.ProId;
            condition.Name ="AT";
            condition.Descr ="ASTN3";
            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            IList<PrintLog> PrintLogList = repository.GetPrintLogListByCondition(condition);
            if (PrintLogList == null || PrintLogList.Count == 0)
            {
                List<string> errpara = new List<string>();

                errpara.Add(currentProduct.CUSTSN);

                throw new FisException("CHK206", errpara);
            }
            else { 
                CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, PrintLogList[0].EndNo);

                CurrentSession.AddValue(Session.SessionKeys.PrintLogName,"AT");
                
                CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ATSN3");
            }


            return base.DoExecute(executionContext);
            */
        }
    }
}
