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
using IMES.DataModel;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.DN;
using IMES.Common;


namespace IMES.Activity
{
    /// <summary>
    ///  檢查Product.Model != select ProcessModel from AliasLineModel AliasLineModel.AliasLine
    /// </summary>
    public partial class CheckAndLockUploadDelivery : BaseActivity
    {
        /// <summary>
        ///  檢查Product.Model != select ProcessModel from AliasLineModel AliasLineModel.AliasLine
        /// </summary>
        public CheckAndLockUploadDelivery()
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
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            Session session =CurrentSession;
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();

            IList<UploadDNInfo> uploadDnInfo = utl.IsNull<IList<UploadDNInfo>>(session, Session.SessionKeys.UploadDNInfoList);
            IList<string> deliveryNoList  = uploadDnInfo.Select(x=>x.DeliveryNo).ToList();
            IList<Delivery> deliveryList = dnRep.GetDeliveryListWithTrans(deliveryNoList);

            //Check Delivery Udt
            var notMatchedUdtDnList = deliveryList.Where(x => !uploadDnInfo.Any(y => y.DeliveryNo == x.DeliveryNo && y.Udt == x.Udt)).ToList();
            if (notMatchedUdtDnList.Count> 0)
            {
                throw new FisException("PAK173", new string[] { string.Join(",", notMatchedUdtDnList.Select(x=>x.DeliveryNo).ToArray()) });
            }
            //Check Delivery Status
            var notMatchedStatusDnList = deliveryList.Where(x =>x.Status!="88").ToList();
            if (notMatchedStatusDnList.Count > 0)         
            {
                throw new FisException("PAK173", new string[] { string.Join(",", notMatchedStatusDnList.Select(x => x.DeliveryNo + ":" + x.Status).ToArray()) });
            }        
            
            //Check SN Count
            if (deliveryNoList.Count > 1)
            {
                int qty = deliveryList.Sum(x => x.Qty);
                int maxQty=int.Parse(utl.GetSysSetting("UploadSNMaxCount", "1000"));
                if (qty > maxQty)
                {
                    throw new FisException("CQCHK1107", new string[] { maxQty.ToString() + " (" + qty.ToString() + ")" });
                }
            }


            //Lock Delivery status=LK88
            dnRep.UpdateMultiDeliveryForStatusChange(deliveryNoList.ToArray(), "LK");

            session.AddValue(Session.SessionKeys.DeliveryList, deliveryList);
            session.AddValue(Session.SessionKeys.DeliveryNoList, deliveryNoList);
            
            return base.DoExecute(executionContext);
        }

        
    }
}
