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

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ShipLabelPrint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShipLabelPrint()
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
            Delivery currentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            CurrentSession.AddValue(Session.SessionKeys.InfoValue, 0);
            string dnFlag = (string)currentDelivery.GetExtendedProperty("Flag");
            if (!String.IsNullOrEmpty(dnFlag) && dnFlag == "N")
            {
                string PdfFileName = currentDelivery.DeliveryNo + "-" + currentProduct.CUSTSN + "-[BoxShipLabel].pdf";

                string line = currentProduct.Status.Line;
                if (!String.IsNullOrEmpty(line))
                {
                    string path = "\\" + line.Substring(0, 1) + "\\" + PdfFileName;
                    CurrentSession.AddValue("PDFFileName", path);
                }

                //Mantis 972

                int btFlag = (int)CurrentSession.GetValue(Session.SessionKeys.IsBT);
                if (btFlag == 1 || btFlag == 0)
                {
                    IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                    try
                    {
                        //得到模板：
                        IList<string> docnumList = new List<string>();
                        IList<string> tempList = new List<string>();
                        bool bFind = false;
                        //select @doc_set_number = DOC_SET_NUMBER  from [PAK.PAKComn] where left(InternalID,10) = @InternalID的前10位
                        docnumList = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(currentDelivery.DeliveryNo.Substring(0, 10));
                        foreach (string num in docnumList)
                        {
                            //SELECT @templatename = XSL_TEMPLATE_NAME FROM [PAK.PAKRT] WHERE DOC_CAT = @doctpye AND DOC_SET_NUMBER = @doc_set_number
                            tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer("Box Ship Label", num);
                            if (tempList != null && tempList.Count != 0)
                            {
                                bFind = true;
                                foreach (string atemp in tempList)
                                {
                                    CurrentSession.AddValue("ShiptoTemplate", atemp);
                                    break;
                                }
                                break;
                            }
                        }
                        if (bFind == false)
                        {
                            //FOR test
                            //CurrentSession.AddValue("ShiptoTemplate", "Template_IN_Commercial_CTO_All_Box_Ship_Label_20110901.xslt");
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(currentDelivery.DeliveryNo);
                            ex = new FisException("CHK874", new string[] { });
                            throw ex;
                        }
                    }
                    catch (FisException e)
                    {
                        throw e;
                    }
                    catch (Exception e)
                    {
                        throw new SystemException(e.Message);
                    }
                }
                /*        打印ShipTo Label
    a).DN的Flag属性值等于N时，是Edits打印：
    PdfFilename= @dn&"-"& @unitsn&"-[BoxShipLabel].pdf"
	set Fs = CreateObject("Scripting.FileSystemObject")
	if Fs.FileExists("c:\FIS\pdfprintlist.txt") then
	   FS.DeleteFile "c:\FIS\pdfprintlist.txt"
	end if
	set File=Fs.CreateTextFile("c:\FIS\pdfprintlist.txt",true)
	File.WriteLine "\\192.168.144.7\OUT\pdf\"&left(CmbPL.value ,1)&"\"&PdfFilename
	File.close
	set   obj=createobject("wscript.shell") 
	cmdpdfprint = "C:\FIS\PDFPrint.exe C:\FIS\pdfprintlist.txt 100"
	obj.run   "cmd   /c   "&cmdpdfprint,vbhide
    set   obj=nothing
                 */
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

