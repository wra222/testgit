/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckDataForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-16                  (Reference Ebook SourceCode) Create
* A. 刷入的是DN
*  exists (select * from [PAK_PAKComn] nolock where left(InternalID,10)=@DN)
* B.刷入的是Shipment
* exists(select * from v_Shipment_PAKComn nolock where CONSOL_INVOICE=@DN or SHIPMENT = @DN)
* C.输入的是WayBill Number
* exists(select * from v_Shipment_PAKComn nolock where WAYBILL_NUMBER=@DN )
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
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
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// Check Data: DN  or   Shipment   or  Waybill
    /// </summary>
    public partial class CreateFileForScaningList : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateFileForScaningList()
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
            string type = (string)CurrentSession.GetValue("VCode");
            string data = (string)CurrentSession.GetValue("DeliveryNo");
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            string doc_type = (string)CurrentSession.GetValue("COASN");

            IList<string> interID=null;
            IList<string> pdfPara = new List<string>();
            IList<string> xmlPara = new List<string>();
            string path = "test";

            if (type == "DN")
            {
                interID = repPizza.GetInternalIdsFromPakDashPakComnByLikeInternalID(data);
            //    return base.DoExecute(executionContext);
            }

            if (type == "Shipment")
            {
                interID = repPizza.GetInternalIDsFromVShipmentPakComnListByConsolInvoiceOrShipmentAndRegion(data, "EMEA");
            }
            //文件名：  @deliveryno &"-["& replace(@doc_type," ","")&"].xml"
            string XmlFilename = interID[0] + "-[" + doc_type.Replace(" ", "") + "].xml";
            
           ///////////////////**** xml */
            xmlPara.Add(interID[0]);
            xmlPara.Add(XmlFilename);
            //xmlPara.Add(doc_type);

            //// get 模板
            IList<string> template;
            IList<string> template_id = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(interID[0].Substring(0,10));
            if (template_id.Count == 0)
            {
                List<string> errpara = new List<string>();
                errpara.Add(interID[0].Substring(0, 10));
                errpara.Add(data);
                FisException ex = new FisException("CHK276", errpara);

                throw ex;
            }
            else {
                template = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer(doc_type, template_id[0]);
                if (template.Count == 0)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(interID[0].Substring(0, 10));
                    errpara.Add(data);
                    FisException ex = new FisException("CHK276", errpara);

                    throw ex;
                }
            }
            IList<VPakComnInfo> tmpPdf = repPizza.GetVPakComnList(interID[0]);

            // pdf 文件名 PdfFilename = @delivery&'-'&@shipment&'-'&@waybill &"-["& replace(cmbdoctpye.value," ","")&"].pdf"
            string pdfFileName = string.Empty;
            string dn = tmpPdf[0].internalID.Substring(0, 10);
            string shipment = tmpPdf[0].shipment;
            string waybill = tmpPdf[0].waybill_number;
            if (shipment == null)
                shipment = "";
            if (waybill == null)
                waybill = "";

            pdfFileName = dn.TrimEnd() + "-" + shipment.TrimEnd() + "-" + waybill.TrimEnd() + "-[" + doc_type.Replace(" ", "") + "].pdf";


            pdfPara.Add(dn);
            pdfPara.Add(shipment);
            pdfPara.Add(waybill);
            pdfPara.Add(pdfFileName);
            pdfPara.Add(template[0]);

            //get path
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> customerLst = new List<string>();
            customerLst = partRepository.GetValueFromSysSettingByName("OAEditsImage");

            CurrentSession.AddValue(Session.SessionKeys.GiftScanPartList, xmlPara); //xml file para
            CurrentSession.AddValue(Session.SessionKeys.GiftPartNoList, pdfPara); //pdf file para
            if (customerLst.Count > 0)
                path = customerLst[0];

            CurrentSession.AddValue(Session.SessionKeys.CN, path);

          
            //xmlPara.Add()
            //CurrentSession.AddValue(CurrentSession.Sessionk)
            ////没有完成
            return base.DoExecute(executionContext);

        }      
    }
}

