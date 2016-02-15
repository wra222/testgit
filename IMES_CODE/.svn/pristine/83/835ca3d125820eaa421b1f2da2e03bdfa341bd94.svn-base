/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:  UC:CI-MES12-SPEC-FA-UI Generate Customer SN
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-12-22   207013            Create 
 * ITC-1360-1211 修正ReprintLog中Name
 * 
 * Known issues:Any restrictions about this file 
 */


using System.Workflow.ComponentModel;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.DN;
using System.Linq;
using IMES.FisObject.Common.FisBOM;
namespace IMES.Activity
{

    public partial class CheckRoyaltyPaper : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckRoyaltyPaper()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            Delivery dn = (Delivery)session.GetValue(Session.SessionKeys.Delivery);
            if (dn == null)
            {
                return base.DoExecute(executionContext);
            }
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            if (dn.PoNo.StartsWith("BF3") || dn.PoNo.StartsWith("BFD"))
            {
                IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(dn.ModelName);
                IList<IBOMNode> P1BomNodeList = bom.GetFirstLevelNodesByNodeType("P1");
                bool needpaper = false;
                if (P1BomNodeList != null)
                {
                    foreach (IBOMNode bomNode in P1BomNodeList)
                    {
                        if (bomNode.Part.Descr.ToUpper().Contains("ROYALTY PAPER"))
                        {
                            needpaper = true;
                            break;
                        }
                    }
                }
                if (needpaper)
                {
                    Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                    if (!curProduct.ProductParts.Any(x => x.PartType.StartsWith("Royalty Paper")))
                    {
                        FisException e = new FisException("PAK184", new string[] { });
                        e.stopWF = true;
                        throw e;
                    }
                }
            }
            return base.DoExecute(executionContext);
        }
    }
}
