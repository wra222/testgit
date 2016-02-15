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
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.Carton;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Activity
{
    /// <summary>
    /// Write PrintLog base on PrintItemList data
    /// </summary>
    public partial class WritePrintLogList : BaseActivity
	{
        /// <summary>
        /// Write PrintLog base on PrintItemList data
        /// </summary>
		public WritePrintLogList()
		{
			InitializeComponent();
		}
        /// <summary>
        /// Write PrintLog base on PrintItemList data
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var PrintItemList = (IList<PrintItem>)CurrentSession.GetValue(Session.SessionKeys.PrintItems);

            //0.No Print Item List do nothing
            if (PrintItemList == null || PrintItemList.Count == 0)
            {
                return base.DoExecute(executionContext);
            }
            string sn=GetBeginNo();
            if (string.IsNullOrEmpty(sn))
            {
                FisException ex = new FisException("CHK1028", new string[] {BeginAndEndNoFrom.ToString()});
                throw ex;
            }

            IPrintLogRepository PrintRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();

            foreach (PrintItem item in PrintItemList)
            {
                
                var log  = new PrintLog
                {
                    Name = item.LabelType,
                    BeginNo = sn,
                    EndNo = sn,
                    Descr = sn,
                    Station=this.Station,
                    LabelTemplate = (item.TemplateName??""),
                    Editor = this.Editor
                };

                PrintRep.Add(log, CurrentSession.UnitOfWork);
            }
           
            return base.DoExecute(executionContext);
        }


        /// <summary>
        /// Get BeginAndEndNoFrom Product or PCB 
        /// </summary>     
        public enum NumberFromEnum
        {
            /// <summary>
            /// ProductID
            /// </summary>
            ProId = 1,
            /// <summary>
            /// CustomSN
            /// </summary>
            CUSTSN,
            /// <summary>
            /// MO
            /// </summary>
            MO,
            /// <summary>
            /// DeliveryNo
            /// </summary>
            DeliveryNo,
            /// <summary>
            /// PalletNo
            /// </summary>
            PalletNo,
            /// <summary>
            /// CartonSN
            /// </summary>
            CartonSN,
            /// <summary>
            /// PizzaID
            /// </summary>
            PizzaID,
              /// <summary>
            /// Model
            /// </summary>
            Model,
            /// <summary>
            /// Family
            /// </summary>
            Family,
            /// <summary>
            /// PCBNo
            /// </summary>
            PCBNo,
            /// <summary>
            /// PCBModel
            /// </summary>
            PCBModel,
            /// <summary>
            /// PCBFamily
            /// </summary>
            PCBFamily,
              /// <summary>
            /// SMTMO
            /// </summary>
            SMTMO
          
        }

        private string GetBeginNo()
        {
            IMB MB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            Product Prod = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            //Pallet curPallet =  CurrentSession.GetValue(Session.SessionKeys.Pallet) as Pallet;   
            //Carton carton = CurrentSession.GetValue(Session.SessionKeys.Carton) as Carton;
            //Delivery curDelivery = CurrentSession.GetValue(Session.SessionKeys.Delivery) as Delivery;
            string begNo = "";

           if  (BeginAndEndNoFrom==NumberFromEnum.PCBModel)
           {
              begNo = MB.PCBModelID;
           }else if (BeginAndEndNoFrom==NumberFromEnum.PCBNo ) {
               
                    begNo = MB.Sn; //
           }
          else if (BeginAndEndNoFrom== NumberFromEnum.PCBFamily ){
                    begNo = MB.ModelObj.Family;
          }
           else if (BeginAndEndNoFrom == NumberFromEnum.SMTMO)
           {
               begNo = MB.SMTMO;
           }
           else if (Prod==null && BeginAndEndNoFrom == NumberFromEnum.PizzaID)
           {
               Pizza pizza = (Pizza)CurrentSession.GetValue(Session.SessionKeys.Pizza);
               begNo = pizza.PizzaID;
           }
           else
           {
               begNo = (string)Prod.GetProperty(BeginAndEndNoFrom.ToString());
           }
                   
           return begNo;

        }
        /// <summary>
        /// Get BeginNo  And EndNo 
        ///  </summary>
        public static DependencyProperty BeginAndEndNoFromProperty = DependencyProperty.Register("BeginAndEndNoFrom", typeof(NumberFromEnum), typeof(WritePrintLogList), new PropertyMetadata(NumberFromEnum.ProId));

        /// <summary>
        /// 绑定到 Begin And End Number get from which is object
        /// </summary>
        [DescriptionAttribute("BeginAndEndNoFrom")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public NumberFromEnum BeginAndEndNoFrom
        {
            get
            {
                return ((NumberFromEnum)(base.GetValue(WritePrintLogList.BeginAndEndNoFromProperty)));
            }
            set
            {
                base.SetValue(WritePrintLogList.BeginAndEndNoFromProperty, value);
            }
        }
	}
}
