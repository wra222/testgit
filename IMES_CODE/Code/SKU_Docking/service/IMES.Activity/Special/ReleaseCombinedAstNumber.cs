// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Close当前页面的Location
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-01   Kerwin                       create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.Part;
using System.Collections.Generic;
using System.Linq;
using IMES.DataModel;
using System;

namespace IMES.Activity
{
    /// <summary>
    /// release combined Ast Number 
    /// </summary>
    public partial class ReleaseCombinedAstNumber : BaseActivity
    {

        /// <summary>
        /// constructor
        /// </summary>
        public ReleaseCombinedAstNumber()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Close当前页面的Location
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<IProductPart> productPartList = currenProduct.ProductParts;
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

            var astPartList = productPartList.Where(x => x.BomNodeType == "AT");
            foreach (IProductPart item in astPartList)
            {
                if (!string.IsNullOrEmpty(item.PartSn))
                {
                  var CombinedAstNumberList=  prodRep.GetCombinedAstNumber(new IMES.DataModel.CombinedAstNumberInfo {  
                                                                ProductID= currenProduct.ProId,
                                                                AstType="AST",
                                                                AstNo = item.PartSn,
                                                                State="Used"
                                });
                  foreach (CombinedAstNumberInfo info in CombinedAstNumberList)
                  {
                      info.Remark = string.Format("BindStation:{0}", info.Station);
                      info.UnBindProductID= info.ProductID;
                      info.UnBindStation = this.Station;
                      info.ProductID="";
                      info.Station="";
                      info.Udt = DateTime.Now;
                      info.State="Release";
                      info.Editor = this.Editor;
                      prodRep.UpdateCombinedAstNumberDefered(CurrentSession.UnitOfWork, info);
                  }
                }
            }
            return base.DoExecute(executionContext);
        }
    }
}
