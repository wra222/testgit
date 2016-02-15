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
using IMES.FisObject.PAK.Pizza;
using IMES.DataModel;
using IMES.FisObject.Common.MO;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UpdateMPBTOrder : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateMPBTOrder()
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
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var productList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as List<IProduct>;
            if (productList == null)
            {
                throw new NullReferenceException("ProdList in session is null");
            }
            int b = (int)CurrentSession.GetValue("BTProList");
            if (b == 1)
            {                
                var currentMO = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);
                string modelName = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
                IList<MpBtOrderInfo> infos = new List<MpBtOrderInfo>();
                infos = repPizza.GetMpBtOrderInfoList(modelName);
                MpBtOrderInfo temp = new MpBtOrderInfo();
                int inputQty = (int)CurrentSession.GetValue(Session.SessionKeys.Qty);
                //ITC-1360-1298
                //foreach (MpBtOrderInfo temp in infos)
                if(infos != null && infos.Count > 0)
                {
                    temp = infos[infos.Count - 1];
                    if (temp.qty - temp.prtQty >= inputQty)
                    {
                        foreach (var product in productList)
                        {
                            ProductBTInfo item = new ProductBTInfo();
                            item.productID = product.ProId;
                            item.bt = temp.bt;
                            item.editor = this.Editor;
                            productRepository.InsertProductBT(item);
                        }
                        MpBtOrderInfo setValue = new MpBtOrderInfo();
                        //setValue.prtQty = temp.prtQty + inputQty;                        
                        setValue.bt = temp.bt;
                        setValue.id = temp.id;
                        repPizza.UpdateForIncreasePrtQty(setValue, inputQty);
                    }
                    else
                    {
                        int n = temp.qty - temp.prtQty;
                        for (int i = 0; i < n; i++)
                        {
                            ProductBTInfo item = new ProductBTInfo();
                            item.productID = productList[i].ProId;
                            item.bt = temp.bt;
                            item.editor = this.Editor;
                            productRepository.InsertProductBT(item);
                        }
                        MpBtOrderInfo setValue = new MpBtOrderInfo();
                        setValue.prtQty = temp.qty;
                        MpBtOrderInfo cond = new MpBtOrderInfo();
                        cond.bt = temp.bt;
                        cond.id = temp.id;
                        repPizza.UpdateMpBtOrder(setValue, cond);
                    }
                }
            }            

            return base.DoExecute(executionContext);
        }

    }
}

