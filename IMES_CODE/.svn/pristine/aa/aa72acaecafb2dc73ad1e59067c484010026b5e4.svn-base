/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UI SN Check.docx –2011/10/20 
 * UC:CI-MES12-SPEC-PAK-UC SN Check.docx –2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-20   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：判断是否需要检查Asset Tag SN Check：	Select b.Tp,Type,@message=Message,Sno1, @lwc=L_WC, @mwc=M_WC from Special_Det a,Special_Maintain b where a.SnoId=@Productid and b.Type=a.Tp and b.SWC=”68”得到的Tp=”K”的记录时，表示需要检查Asset Tag SN Check是否做过 (目前只考虑得到一条记录的情况??)-数据接口尚未定义（in Activity：CheckAssetTagSN）
* UC 具体业务：当BOM(存在PartType=ALC and BomNodeType=PL的part) 且model<>PC4941AAAAAY时，表示有ALC，这时没有真正的Pizza盒-数据接口尚未定义（in Activity：CheckSNIdentical）
*/

using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 检查是否做Asset Tag SN check,需要则判断是否做过check
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FA站Offline print CT, FRU IECSNO 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1. 判断是否需要检查Asset Tag SN Check是否已做过
    ///			2. 需要，则检查Asset Tag SN Check是否已做过
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.CustSN
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    ///
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    ///         
    /// </para> 
    /// </remarks>
	public partial class CheckAssetTagSN: BaseActivity
	{
       
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckAssetTagSN()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 检查是否做Asset Tag SN check做过与否的检查,需要则判断是否做过check
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            string customerSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);    
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            //(2012-07-21 UC updated)判断是否需要列印Asset Tag Label，如果需要列印，则需要检查是否已经列印过
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM curBOM = null;
            bool printFlag = false;
            curBOM = bomRepository.GetHierarchicalBOMByModel(currenProduct.Model);
            IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
            bomNodeLst = curBOM.FirstLevelNodes;
            if (bomNodeLst != null && bomNodeLst.Count > 0)
            {
                foreach (IBOMNode ibomnode in bomNodeLst)
                {
                    if ((ibomnode.Part.BOMNodeType.ToUpper() == "AT") && ((ibomnode.Part.Descr.ToUpper() == "ATSN7") || (ibomnode.Part.Descr.ToUpper() == "ATSN7") || (ibomnode.Part.Descr.ToUpper() == "ATSN8")))
                    {
                        printFlag = true;
                        break;
                    }
                }
            }
       
            if (printFlag)
            {
      
                IList<ProductLog> logList= currenProduct.ProductLogs;
                //Marked by Benson at 2013/03/11
                //var newstLog = (from p in logList orderby p.Cdt descending select p).First();
                //if (newstLog.Station != "PKAT" || newstLog.Status!=IMES.FisObject.Common.Station.StationStatus.Pass)
                //{ throw new FisException("PAK143", new string[] { }); }
                //Marked by Benson at 2013/03/11

                //****Add by Benson at 2013/03/11
             
                var Log81 = (from p in logList
                                     where p.Station == "81" &&
                                         p.Status == IMES.FisObject.Common.Station.StationStatus.Pass
                                     orderby p.Cdt descending
                                     select p);


                var LogPKAT = (from p in logList
                                           where p.Station == "PKAT" &&
                                             p.Status == IMES.FisObject.Common.Station.StationStatus.Pass
                                           orderby p.Cdt descending
                                           select p);

                if (LogPKAT.Count() > 0)
                {
                     if (Log81.Count() > 0)
                    {
                         if (LogPKAT.First().Cdt < Log81.First().Cdt)
                        {
                            throw new FisException("PAK143", new string[] { });
                        }

                    }
                }
                else
                {
                    throw new FisException("PAK143", new string[] { });
                }
                //****Add by Benson at 2013/03/11
              
                

                //   IList<ProductLog> logList = productRepository.GetProductLogs("PKAT", 1, "ATSN Print", currenProduct.ProId);
           
                //if (logList == null || logList.Count <= 0)
                //{
                //    throw new FisException("PAK143", new string[] { });
                //}
            }

            IList<SpecialCombinationInfo> SCInfoList = productRepository.GetSpecialDetSpecialMaintainInfoList(currenProduct.ProId, "68");
            foreach (SpecialCombinationInfo SCInfo in SCInfoList)
            {
                if (SCInfo.maintainInfo.tp == "K")
                {
                    string lwc = SCInfo.maintainInfo.l_WC;
                    string mwc = SCInfo.maintainInfo.m_WC;
                    string message = SCInfo.maintainInfo.message;

                    if (lwc != "")
				    {   					  					
					    IList<ProductLog> tempProductLogList = productRepository.GetProductLogs(currenProduct.ProId, lwc);

                        if (tempProductLogList ==null ||tempProductLogList.Count == 0)	
					    {
						    throw new FisException(message);  
					    }					
				    }

				    if (mwc != "")
				    {				
                        if (currenProduct.Status.StationId != mwc) 
					    {
                            throw new FisException(message); 
					    }
				    }
                }
            }

            return base.DoExecute(executionContext);
        }
	}
}
