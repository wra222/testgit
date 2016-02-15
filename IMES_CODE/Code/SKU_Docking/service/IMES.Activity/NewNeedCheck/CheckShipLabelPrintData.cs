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

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckShipLabelPrintData : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckShipLabelPrintData()
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
            CurrentSession.AddValue(Session.SessionKeys.ifElseBranch, 1);
            //ITC-1360-0796
            CurrentSession.AddValue(Session.SessionKeys.labelBranch, 0);
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string model = currentProduct.Model;
            if (model != null)
            {
                //出拉美的机器(Model的第10位、第11位等于16、DM、D0、D3时)，
                //根据ProductID得到Product.PizzaID和  (ProductInfo.InfoValue where InfoType=’KIT2’)，
                //即与Product绑定的pizaa；
                //在Pizza_Part中得到与pizza绑定的part，只获取Pizza_Part.Value是14位且第一位为5 or W的parts
                string tmp = model.Substring(9, 2);
                if (tmp == "16" || tmp == "DM" || tmp == "D0" || tmp == "D3")
                {
                    string pizzaID = currentProduct.PizzaID;
                    string kit2 = (string)currentProduct.GetExtendedProperty("KIT2");
                    //ITC-1360-0791
                    bool bExist = false;
                    IPizzaRepository repPizza =
                        RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                    //pizzaID:
                    if (!string.IsNullOrEmpty(pizzaID))
                    {
                        IList<PizzaPart> plist = new List<PizzaPart>();
                        plist = repPizza.GetPizzaPartsByValueLike(14, "5", pizzaID);
                        if (plist != null && plist.Count > 0)
                        {
                            foreach (PizzaPart part in plist)
                            {
                                IList<ACAdapMaintainInfo> acalist = new List<ACAdapMaintainInfo>();
                                acalist = repPizza.GetACAdapMaintainListByAssemb(part.PartSn.Substring(0, 5));
                                if (acalist != null && acalist.Count > 0)
                                {
                                    bExist = true;
                                    break;
                                }
                            }
                        }
                        if (bExist == false)
                        {
                            plist = repPizza.GetPizzaPartsByValueLike(14, "W", pizzaID);
                            if (plist != null && plist.Count > 0)
                            {
                                foreach (PizzaPart part in plist)
                                {
                                    IList<ACAdapMaintainInfo> acalist = new List<ACAdapMaintainInfo>();
                                    acalist = repPizza.GetACAdapMaintainListByAssemb(part.PartSn.Substring(0, 5));
                                    if (acalist != null && acalist.Count > 0)
                                    {
                                        bExist = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //kit2:
                    if (bExist == false)
                    {
                        if (!String.IsNullOrEmpty(kit2))
                        {
                            IList<PizzaPart> klist = new List<PizzaPart>();
                            klist = repPizza.GetPizzaPartsByValueLike(14, "5", kit2);
                            if (klist != null && klist.Count > 0)
                            {
                                foreach (PizzaPart part in klist)
                                {
                                    IList<ACAdapMaintainInfo> acalist = new List<ACAdapMaintainInfo>();
                                    acalist = repPizza.GetACAdapMaintainListByAssemb(part.PartSn.Substring(0, 5));
                                    if (acalist != null && acalist.Count > 0)
                                    {
                                        bExist = true;
                                        break;
                                    }
                                }
                            }
                            if (bExist == false)
                            {
                                klist = repPizza.GetPizzaPartsByValueLike(14, "W", kit2);
                                if (klist != null && klist.Count > 0)
                                {
                                    foreach (PizzaPart part in klist)
                                    {
                                        IList<ACAdapMaintainInfo> acalist = new List<ACAdapMaintainInfo>();
                                        acalist = repPizza.GetACAdapMaintainListByAssemb(part.PartSn.Substring(0, 5));
                                        if (acalist != null && acalist.Count > 0)
                                        {
                                            bExist = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }//pizzaid no find, check in kit2
                    if (bExist == false)
                    {
                        CurrentSession.AddValue(Session.SessionKeys.ifElseBranch, 0);
                        FisException ex;
                        List<string> erpara = new List<string>();
                        ex = new FisException("CHK838", erpara);
                        throw ex;
                    }
                    CurrentSession.AddValue(Session.SessionKeys.labelBranch, 1);
                }//tmp == "16" || tmp == "DM" || tmp == "D0" || tmp == "D3"
            }//model!=null
            //CurrentSession.AddValue(Session.SessionKeys.labelBranch, 1);
            
            return base.DoExecute(executionContext);
        }

    }
}

