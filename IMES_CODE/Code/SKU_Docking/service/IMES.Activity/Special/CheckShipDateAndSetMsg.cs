using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.QTime;
using IMES.FisObject.Common.Line;
using System.Collections.Generic;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.TestLog;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;



namespace IMES.Activity
{
    /// <summary>
    /// CheckShipDateAndSetMsg
    /// </summary>
    public partial class CheckShipDateAndSetMsg : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckShipDateAndSetMsg()
		{
			InitializeComponent();
		}

        /// <summary>
        /// IsThrowException
        /// </summary>
        public static DependencyProperty IsThrowExceptionProperty = DependencyProperty.Register("IsThrowException", typeof(bool), typeof(CheckShipDateAndSetMsg), new PropertyMetadata(false));

        /// <summary>
        /// Status of Product
        /// </summary>
        [DescriptionAttribute("CheckShipDateAndSetMsg")]
        [CategoryAttribute("CheckShipDateAndSetMsg Category")]
        [BrowsableAttribute(true)]    
        public bool IsThrowException
        {
            get
            {
                return ((bool)(base.GetValue(CheckShipDateAndSetMsg.IsThrowExceptionProperty)));
            }
            set
            {
                base.SetValue(CheckShipDateAndSetMsg.IsThrowExceptionProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
           
            DateTime today=new DateTime( DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);

            var prod = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (string.IsNullOrEmpty(prod.DeliveryNo))
            {
                IList<Delivery> dnList = dnRep.GetDeliveryListByModel(prod.Model, "00");
                if (dnList != null && dnList.Count > 0)
                {
                    var todayDnList = (from p in dnList
                                       where p.ShipDate == today
                                       select p).ToList();

                    if (todayDnList != null && todayDnList.Count > 0)
                    {
                        int station80Qty = 0;
                        IList<string> station80ProductIDList = prodRep.GetProductIDByModelStation(prod.Model, new List<string>() { "80" });
                        if (station80ProductIDList != null && station80ProductIDList.Count > 0)
                        {
                            station80Qty = station80ProductIDList.Count;
                        }

                        int dnQty = 0;
                        foreach (Delivery item in todayDnList)
                        {
                            if (station80Qty > 0)
                            {
                                dnQty = dnQty + item.Qty - prodRep.GetCombinedQtyByDN(item.DeliveryNo);
                            }
                            else
                            {
                                dnQty = dnQty + item.Qty;
                            }
                        }

                        if ((dnQty - station80Qty) > 0)
                        {
                            if (this.IsThrowException)
                            {
                                throw new FisException("CHK1075", new string[] { todayDnList[0].DeliveryNo, today.ToString("yyyy-MM-dd") });
                            }
                            else
                            {
                                CurrentSession.AddValue(ExtendSession.SessionKeys.WarningMsg, string.Format("此台机器為船務:{0} 出貨日:{1}", todayDnList[0].DeliveryNo, today.ToString("yyyy-MM-dd")));
                            }
                        }
                    }
                }
                else
                {
                    if (!prod.IsBT && this.Station.StartsWith("69"))
                    {
                        CurrentSession.AddValue(ExtendSession.SessionKeys.WarningMsg, string.Format("此机型:{0}暂时没有可结合的船务资料，请确认是否为多投", prod.Model));
                    }
                }
            }           

            return base.DoExecute(executionContext);
        }
	}
}
