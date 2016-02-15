// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Insert ProductInfo
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-28   Kerwin                       create
// Known issues:
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.FisBOM;
using System.Collections.Generic;
using System.Linq;
using IMES.FisObject.Common.PrintLog;
using IMES.DataModel;
using IMES.Common;
namespace IMES.Activity
{
    /// <summary>
    /// Insert ProductInfo
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC Travel Card Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
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
    ///         Session.InfoValue
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
    ///         ProductInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    /// </para> 
    /// </remarks>
    public partial class WriteProductPartAndPrintLogForAsstage3 : BaseActivity
	{
       
        /// <summary>
        /// constructor
        /// </summary>
        public WriteProductPartAndPrintLogForAsstage3()
		{
			InitializeComponent();
		}

        /// <summary>
        /// write ProductInfo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            var prdRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            IProduct currentProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            IList<IPart> needCombinePartList =   utl.IsNull<IList<IPart>>(session, Session.SessionKeys.NeedCombineAstPartList);  //(IList<IPart>)session.GetValue("NeedCombineAstPartList");
            //if (needCombineAstList.Count == 0)
            //{
            //    throw new FisException("CHK205", new List<string> { currentProduct.ProId, this.Station });   // 此机器不需要print asset Label !
            //}
            IPart part = needCombinePartList.Where(x => x.BOMNodeType == "PP" && x.Descr == "Asstage-3").FirstOrDefault(); 

            //string astPartNo = CheckAsstage3(currentProduct, needCombinePartList);
            //if(!string.IsNullOrEmpty(astPartNo))
            if (part!=null &&
                !string.IsNullOrEmpty(part.PN))
            {
                
               ProductPart prdPart = new ProductPart();
               prdPart.PartSn =currentProduct.CUSTSN;
               prdPart.BomNodeType = part.BOMNodeType; // "PP";
               prdPart.PartType = part.Descr;  //"Asstage-3";
               prdPart.Editor = this.Editor;
               prdPart.Station = this.Station;
               prdPart.PartID = part.PN;//astPartNo;
               prdPart.CheckItemType = "GenPPAST3";
               
               currentProduct.AddPart(prdPart);
               prdRep.Update(currentProduct, session.UnitOfWork);
               
              //var item = new PrintLog
              //             {
              //                 Name ="Asstage-3",
              //                 BeginNo = currentProduct.CUSTSN,
              //                 EndNo = currentProduct.CUSTSN,
              //                 Descr = "",
              //                 Editor = this.Editor
              //             };
              //  var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
              //  repository.Add(item, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
        }

        private string CheckAsstage3(IProduct currentProduct, IList<IPart> needCombinePartList)
        {
            // pp (Asstage-3) 類 (BomNodeType='PP' and PartType='Asstage-3')
             //var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IPart part = null;
            //if (needCombinePartList != null)
            //{
                part = needCombinePartList.Where(x => x.BOMNodeType == "PP" && x.Descr == "Asstage-3").FirstOrDefault(); 
            //}
            //else
            //{
            //    IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            //    IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
            //    IList<IBOMNode> bomNodes = bom.FirstLevelNodes;
            //    part = bomNodes.Where(x =>x.Part.BOMNodeType =="PP"  &&  x.Part.Type == "Asstage-3").Select(y=> y.Part).FirstOrDefault();
            //}

            if (part==null)
            {
                return null;
            }
            else
            {
                return part.PN;
            }
           // return bomNodes.Any(x=>x.Part.Type=="Asstage-3");
         }
	}
}
