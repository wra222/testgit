
using System;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Part;
using System.Linq;
using IMES.FisObject.Common.FisBOM;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
namespace IMES.Activity
{
    /// <summary>
    ///CheckAndSetColorMsg
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Product
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
    public partial class CheckAndSetColorMsg : BaseActivity
	{
		///<summary>
		///</summary>
        public CheckAndSetColorMsg()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            CurrentSession.AddValue("SIMCardAlertMessage", "");
            string color = "";
            IList<string> rList = new List<string>();
            IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
            IHierarchicalBOM bom = ibomRepository.GetHierarchicalBOMByModel(product.Model);
            IList<IBOMNode> bomNodeLst = bom.FirstLevelNodes;
            var lst1=(from y in bomNodeLst 
                              where y.Part.BOMNodeType=="PL" && y.Part.Descr=="JGS" select y.Part).ToList();
               IList<string> inforList;
               IList<string> descrList;
               foreach (IMES.FisObject.Common.Part.IPart part in lst1)
               {
                     inforList=part.Attributes.Where(x => x.InfoType == "Infor").Select(x => x.InfoValue).ToList();
                     descrList= part.Attributes.Where(x => x.InfoType == "Descr").Select(x => x.InfoValue).ToList();
                    if (inforList.Count > 0 && descrList.Count>0)
                    {
                       rList.Add(descrList[0].Trim() + ":" + inforList[0].Trim());
                    }
               }
               if (rList.Count > 0)
               {
                 color=  string.Join(",", rList.ToArray());
               }
               CurrentSession.AddValue("CoverColorMsg", color);
               if (string.IsNullOrEmpty(color))
               {
                  FisException e = new FisException("CQCHK1006 ", new string[] { });
                   e.stopWF = true;
                   throw e;
                 }
               //mantis 0001431
               var simpartno = (from y in bomNodeLst
                                where y.Part.BOMNodeType == "PL" && y.Part.PN == "6060B00SIM01" 
                           select y.Part).ToList();
               if (simpartno != null && simpartno.Count > 0)
               {
                   CurrentSession.AddValue("SIMCardAlertMessage", "Y");
               }
              return base.DoExecute(executionContext);
        }
	}
}
