// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的MaterialCT,获取Material对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-03-01  Vincent
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Material;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PCA.MB;
namespace IMES.Activity
{
    /// <summary>
    /// UpdateDn87Status
    /// </summary>
    public partial class UpdateDn87Status : BaseActivity
    {
        ///<summary>
        ///UpdateDnStatus
        ///</summary>
        public UpdateDn87Status()
        {
            InitializeComponent();
        }

        /// <summary>
        /// UpdateDnStatus and Check DN Qty
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
             string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
             int qty =(int)(CurrentSession.GetValue(Session.SessionKeys.Qty)??0);
            if (this.RCTO146Category == CRTO146Category.MBCT)
            {
                IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                mbRep.CheckDnQtyAndUpdateDnStatusDefered(CurrentSession.UnitOfWork, deliveryNo, qty, "87", "CQCHK0013");
            }
            else if (this.RCTO146Category == CRTO146Category.MaterialCT)
            {
                IMaterialRepository materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
                materialRep.CheckDnQtyAndUpdateDnStatusDefered(CurrentSession.UnitOfWork, deliveryNo, qty, "87", "CQCHK0013");
            }
            else
            {
                IMaterialBoxRepository materialBoxRep = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();
                materialBoxRep.CheckDnQtyAndUpdateDnStatusDefered(CurrentSession.UnitOfWork, deliveryNo, qty, "87", "CQCHK0013");
            }

           
            return base.DoExecute(executionContext);
        }
      

        /// <summary>
        /// 输入的类型:PCB,Material,MaterialBox
        /// </summary>
        public static DependencyProperty RCTO146CategoryProperty = DependencyProperty.Register("RCTO146Category", typeof(CRTO146Category), typeof(UpdateDn87Status), new PropertyMetadata(CRTO146Category.MBCT));

        /// <summary>
        /// 输入的类型:PCB,Material,MaterialBox
        /// </summary>
        [DescriptionAttribute("RCTO146Category")]
        [CategoryAttribute("InArugment Of CreateMaterial")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public CRTO146Category RCTO146Category
        {
            get
            {
                return ((CRTO146Category)(base.GetValue(UpdateDn87Status.RCTO146CategoryProperty)));
            }
            set
            {
                base.SetValue(UpdateDn87Status.RCTO146CategoryProperty, value);
            }
        }

       
    }
    /// <summary>
    /// CRTO146Category
    /// </summary>
    public enum CRTO146Category
    {
        /// <summary>
        /// MBCT
        /// </summary>
        MBCT=1,
        /// <summary>
        /// MaterialCT
        /// </summary>
        MaterialCT,
        /// <summary>
        /// NoMaterailCT
        /// </summary>
        NoMaterailCT
    }
}
