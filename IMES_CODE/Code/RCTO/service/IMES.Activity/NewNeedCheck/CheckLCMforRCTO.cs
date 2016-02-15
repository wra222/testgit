
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
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    ///
    /// </summary>
    public partial class CheckLCMforRCTO : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckLCMforRCTO()
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
            //Left(Product.Model,3)=’173’
            //1)Model下阶BM阶的描述包含LCM，该Product不存在Product_Part.PartSn like ‘C%’ and Product_Part.BomNodeType=’KP’的数据，则报错：“请去结合LCM”

            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            var model = currentProduct.Model;
            if (model.IndexOf("173") == 0)
            {
                IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
                IList<MoBOMInfo> list = new List<MoBOMInfo>();
                list = ibomRepository.GetPnListByModelAndBomNodeTypeAndDescr(model, "BM", "LCM");
                if (list != null && list.Count > 0)
                {
                    IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    IList<ProductPart> partList = currentProductRepository.GetProductPartByBomNodeTypeAndPartSnLike(currentProduct.ProId, "KP", "C");

                    if (partList == null || partList.Count <= 0)
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        ex = new FisException("CHK948", erpara);
                        throw ex;

                    }

                }
            }
            
            return base.DoExecute(executionContext);
        }
    }
}

