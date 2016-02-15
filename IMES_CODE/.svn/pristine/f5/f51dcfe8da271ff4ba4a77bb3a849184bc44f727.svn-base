
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
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.CartonSSCC;

namespace IMES.Activity
{

    /// <summary>
    /// 
    /// </summary>
    public partial class CheckPrintType : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public CheckPrintType()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Delivery currentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            CurrentSession.AddValue(Session.SessionKeys.InfoValue, 0);

            string dnFlag = (string)currentDelivery.GetExtendedProperty("Flag");
            if (!String.IsNullOrEmpty(dnFlag) && dnFlag == "N")
            {
                string PdfFileName = currentDelivery.DeliveryNo + "-" + currentProduct.CartonSN + "-[BoxShipLabel].pdf";
                CurrentSession.AddValue("PDFFileName", PdfFileName);


                int reFlag = (int)CurrentSession.GetValue("isRePrint");
                if (reFlag == 1)
                {
                    string templatename = "";
                    ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
                    
                    //HP_EDI.dbo.op_TemplateCheck '"&DN&"','Box Ship Label'
                    templatename = cartRep.GetTemplateNameViaCallOpTemplateCheck(currentProduct.DeliveryNo, "Box Ship Label");
                    //Not found template of this DN: "&DN
                    if (templatename == "ERROR")
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentProduct.DeliveryNo);
                        ex = new FisException("PAK132", erpara);//Not found template of this DN: &DN
                        throw ex;
                    }
                    CurrentSession.AddValue("TemplateName", templatename);
                }
            }
            else
            {
                //b).DN的Flag属性值不等于N时，原先用batch file打印，新系统用模板方式打印
                CurrentSession.AddValue(Session.SessionKeys.InfoValue, 1);
            }
            return base.DoExecute(executionContext);
        }
    }
}