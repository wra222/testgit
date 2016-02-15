/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CreateFileForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-16   zhanghe               (Reference Ebook SourceCode) Create
依据刷入的code获取数据：
刷入的是DN：
select @InternalID = InternalID from v_Shipment_PAKComn nolock where left(InternalID,10)=@DN
刷入的是Shipment：
select distinct InternalID from v_Shipment_PAKComn where CONSOL_INVOICE=@DN or SHIPMENT = @DN order by InternalID
输入的是WayBill Number
select distinct InternalID from v_Shipment_PAKComn where WAYBILL_NUMBER=@DN order by InternalID

依据上面得到的数据，针对每个@InternalID作如下处理：(前10位相同的DN只需要针对第一个16位dn做一次以下的操作)
1)生成XML
  文件名：  @InternalID &"-["& replace(@doc_type," ","")&"].xml"
  调用Edits生成XML文件:
SOAPClient = createobject("MSSOAP.SoapClient30")
SOAPClient.mssoapinit("http://10.99.183.28/hpedits_temp/edits.asmx?WSDL")
SOAPClient.PackingShipmentLabel("\\10.99.183.58\OUT\PACKINGLISTXML\"&XmlFilename, @InternalID)
2)得到模板：
  select @doc_set_number = DOC_SET_NUMBER  from [PAK.PAKComn] where left(InternalID,10) = @InternalID的前10位 
select @templatename = XSL_TEMPLATE_NAME from [PAK.PAKRT] where  DOC_CAT = @doctpye and DOC_SET_NUMBER = @doc_set_number
当没有得到时，报错” Not found template of this DN: ”+ @InternalID的前10位，终止该DN的后续处理，即不打印

3)生成pdf文件名：
 select @delivery=rtrim(left(InternalID,10)),@shipment=rtrim(SHIPMENT),@waybill=rtrim(WAYBILL_NUMBER) from [v_PAKComn] where InternalID = @InternalID
PdfFilename = @delivery+'-'+@shipment+'-'+@waybill &"-["& replace(cmbdoctpye.value," ","")&"].pdf"
4)生成pdf文件
cmd = "C:\Program"&chr(34)&" "&chr(34)&"Files\Altova\FOP-0.93\fop -xml \\10.99.183.58\OUT\PACKINGLISTXML\"&XmlFilename&" -xsl \\10.99.183.58\OUT\"&template&" -pdf \\10.99.183.58\out\PACKINGLISTPDF\"&PdfFilename
set   obj=createobject("wscript.shell")  
obj.run   "cmd   /c   "&cmd,vbhide
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
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
using IMES.FisObject.PAK.Paking;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Activity.Special
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CreateFileForPackingList : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateFileForPackingList()
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

            string dtype = (string)CurrentSession.GetValue("Doc_type");
            IList<string> internalId7 = new List<string>();
            switch (InputType)
            {
                case InputTypeEnum.DN:
                    string dn = (string)CurrentSession.GetValue("Data");
                    //select @InternalID = InternalID from v_Shipment_PAKComn nolock where left(InternalID,10)=@DN
                    internalId7 = repPizza.GetInternalIDsFromVShipmentPakComnListByLikeInternalID(dn.Substring(0, 10));
                    CreatePdfAndXmlByDN(internalId7, dn, dtype);
                    break;
                    //6.18 BOL
                case InputTypeEnum.BOLDN:
                    string boldn = (string)CurrentSession.GetValue("Data");
                    //select @InternalID = InternalID from v_Shipment_PAKComn nolock where left(InternalID,10)=@DN
                    internalId7 = repPizza.GetInternalIDsFromVShipmentPakComnListByLikeInternalID(boldn.Substring(0, 10));
                    CreatePdfAndXmlByDN_FORBOL(internalId7, boldn, dtype);
                    break;
                    //6.18 BOL
                case InputTypeEnum.Shipment:
                    string shipment = (string)CurrentSession.GetValue("Data");
                    //select distinct InternalID from v_Shipment_PAKComn where CONSOL_INVOICE=@DN or SHIPMENT = @DN order by InternalID
                    internalId7 = repPizza.GetInternalIDsFromVShipmentPakComnListByConsolInvoiceOrShipment(shipment);
                    CreatePdfAndXmlByDN(internalId7, shipment, dtype);            
                    break;
                case InputTypeEnum.Waybill:
                    string waybill = (string)CurrentSession.GetValue("Data");
                    //select distinct InternalID from v_Shipment_PAKComn where WAYBILL_NUMBER=@DN order by InternalID
                    internalId7 = repPizza.GetInternalIDsFromVShipmentPakComnListByWaybillNo(waybill);
                    CreatePdfAndXmlByDN(internalId7, waybill, dtype);
                    break;
                case InputTypeEnum.DNForPL:
                    string dnForPL = (string)CurrentSession.GetValue("Data");
                    string doc_type = (string)CurrentSession.GetValue("Doc_type");

                    //如果Doc_Type 如果选择ALL,会针对DOC_Type List中的除了ALL 外的每一个选项，
                    //列印出来其对应的label，针对选中的Doc_Type List中的每种type作以下处理；

                    //如果Doc_Type 选择非ALL 选项，则针对选中的Doc_Type 做以下处理：
                    IList<string> processList = new List<string>();
                    if (doc_type == "ALL")
                    {
                        processList.Clear();
                        processList = (IList<string>)CurrentSession.GetValue("Doc_Type_List");
                    }
                    else
                    {
                        processList.Clear();
                        processList.Add(doc_type);
                    }



                    //针对Doc_Type List中的每种type作以下处理：
                    //IList<string> docList = new List<string>();
                    //docList = repPizza.GetDocCatFromPakDotParRt("Pack List", "Pack List- Transportation");                    
                    ArrayList pdfAndTemp = new ArrayList();

                    //string temp = doc_type;
                    foreach (string temp in processList)
                    {
                        //1.生成XML文件名：  @dn &"-["& replace(@doc_type," ","")&"].xml"
                        string xmlFileName = dnForPL + "-[" + temp.Replace(" ", "") + "].xml";
                        //调用Edits生成XML文件:
                        //SOAPClient = createobject("MSSOAP.SoapClient30")
                        //SOAPClient.mssoapinit("http://10.99.183.28/hpedits/edits.asmx?WSDL")
                        //SOAPClient.PackingShipmentLabel("\\ics-5b3f-backup\OUT\PACKINGLISTXML\"&XmlFilename, @dn(16位dn),””)
                        //2.得到模板：
                        //select @doc_set_number = DOC_SET_NUMBER  from [PAK.PAKComn] where left(InternalID,10) = @dn的前10位
                        IList<string> docsetnumberList = new List<string>();
                        IList<string> tempList = new List<string>();
                        docsetnumberList = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(dnForPL.Substring(0, 10));
                        bool bFindTemplate = false;
                        foreach (string d in docsetnumberList)
                        {
                            //select @templatename = XSL_TEMPLATE_NAME from [PAK.PAKRT] where  DOC_CAT = @doctpye and DOC_SET_NUMBER = @doc_set_number
                            tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer(temp, d);
                            if (tempList == null || tempList.Count == 0)
                            {
                                continue;
                            }
                            else
                            {
                                bFindTemplate = true;
                                break;
                            }
                        }
                        //ITC-1360-1091 注释掉为了测试
                        if (bFindTemplate == false)
                        {
                            //FORTEST
                            FisException e;
                            List<string> erpara2 = new List<string>();
                            erpara2.Add(dnForPL.Substring(0, 10));
                            e = new FisException("CHK836", erpara2);
                            throw e;
                        }
                        //3.生成pdf文件名
                        IList<string> pdfFileList = new List<string>();
                        pdfFileList.Clear();
                        IList<VPakComnInfo>  vpakList = new List<VPakComnInfo>();
                        //select @delivery=rtrim(left(InternalID,10)),@shipment=rtrim(SHIPMENT),@waybill=rtrim(WAYBILL_NUMBER) from [v_PAKComn] where InternalID = @dn
                        vpakList = repPizza.GetVPakComnList(dnForPL);
                        foreach (VPakComnInfo v in vpakList)
                        {
                            //PdfFilename = @delivery+'-'+@shipment+'-'+@waybill &"-["& replace(cmbdoctpye.value," ","")&"].pdf"
                            //if (!String.IsNullOrEmpty(v.internalID) && !String.IsNullOrEmpty(v.shipment)
                            //        && !String.IsNullOrEmpty(v.waybill_number))
                            {
                                string pdfFileName = string.Empty;
                                if (String.IsNullOrEmpty(v.shipment) && String.IsNullOrEmpty(v.waybill_number))
                                {
                                    pdfFileName = dnForPL.TrimEnd() + "-" + "-" + "-[" + temp.Replace(" ", "") + "].pdf";
                                }
                                else if (String.IsNullOrEmpty(v.shipment) && !String.IsNullOrEmpty(v.waybill_number))
                                {
                                    pdfFileName = dnForPL.TrimEnd() + "-" + "-" + v.waybill_number.TrimEnd() + "-[" + temp.Replace(" ", "") + "].pdf";
                                }
                                else if (!String.IsNullOrEmpty(v.shipment) && String.IsNullOrEmpty(v.waybill_number))
                                {
                                    pdfFileName = dnForPL.TrimEnd() + "-" + v.shipment.TrimEnd() + "-" + "-[" + temp.Replace(" ", "") + "].pdf";
                                }
                                else
                                {
                                    pdfFileName = dnForPL.TrimEnd() + "-" + v.shipment.TrimEnd() + "-" + v.waybill_number.TrimEnd() + "-[" + temp.Replace(" ", "") + "].pdf";
                                }
                                //4.)生成pdf文件
                                //cmd = "C:\Program"&chr(34)&" "&chr(34)&"Files\Altova\FOP-0.93\fop -xml 
                                //\\10.99.183.58\OUT\PACKINGLISTXML\"&XmlFilename&" -xsl \\10.99.183.58\OUT\"&template&" -pdf C:\FIS\"&PdfFilename
                                //set   obj=createobject("wscript.shell") 
                                //obj.run   "cmd   /c   "&cmd,vbhide
                                pdfFileList.Add(pdfFileName);
                            }   
                        }
                        ArrayList one = new ArrayList();
                        one.Add(dnForPL);//doc_type
                        one.Add(xmlFileName);
                        one.Add(tempList);
                        one.Add(pdfFileList);

                        pdfAndTemp.Add(one);
                    }//for
                    CurrentSession.AddValue("PDFAndTemp", pdfAndTemp);
                    break;
                default:
                    FisException ex1;
                    List<string> erpara1 = new List<string>();
                    erpara1.Add((string)CurrentSession.GetValue("Data"));
                    ex1 = new FisException("CHK816", erpara1);
                    //ex1.stopWF = false;
                    throw ex1;
                //break;
            }
            return base.DoExecute(executionContext);
        }

        private void CreatePdfAndXmlByDN(IList<string> internalId, string data, string type)
        {            
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            try
            {
                IList<string> xmlFileList = new List<string>();
                int xmlFileCount = 0;
                
                int pdfFileCount = 0;
                ArrayList pdfAndTemp = new ArrayList();

                foreach (string temp in internalId)
                {
                    IList<string> pdfFileList = new List<string>();
                    pdfFileList.Clear();
                    bool b10 = false;
                    if (String.IsNullOrEmpty(temp) || temp.Length < 10)
                        continue;

                    foreach (string t in xmlFileList)
                    {
                        if (temp.Substring(0, 10) == t.Substring(0, 10))
                        {
                            b10 = true;
                            break;
                        }
                    }
                    if (b10 == true)
                        continue;
                    else
                    {
                        //Create XML File!                        
                        //文件名：  @InternalID &"-["& replace(@doc_type," ","")&"].xml"
                        string XmlFilename = temp + "-[" + type.Replace(" ", "") + "].xml";
                        //调用Edits生成XML文件:
                        //SOAPClient = createobject("MSSOAP.SoapClient30")
                        //SOAPClient.mssoapinit("http://10.99.183.28/hpedits_temp/edits.asmx?WSDL")
                        //SOAPClient.PackingShipmentLabel("\\10.99.183.58\OUT\PACKINGLISTXML\"&XmlFilename, @InternalID)

                        xmlFileList.Add(XmlFilename);
                        xmlFileCount++;

                        //得到模板：
                        IList<string> docnumList = new List<string>();
                        IList<string> tempList = new List<string>();
                        bool bFind = false;
                        //select @doc_set_number = DOC_SET_NUMBER  from [PAK.PAKComn] where left(InternalID,10) = @InternalID的前10位
                        docnumList = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(temp.Substring(0, 10));
                        foreach (string num in docnumList)
                        {
                            //SELECT @templatename = XSL_TEMPLATE_NAME FROM [PAK.PAKRT] WHERE DOC_CAT = @doctpye AND DOC_SET_NUMBER = @doc_set_number
                            tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer(type, num);
                            if (tempList != null && tempList.Count != 0)
                            {
                                bFind = true;
                                break;
                            }
                        }
                        if (bFind == false)
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(temp);                            
                            ex = new FisException("CHK834", erpara);
                            //ex.stopWF = false;
                            throw ex;
                        }
                        //生成pdf文件名:
                        IList<VPakComnInfo> pList = new List<VPakComnInfo>();
                        string pdfFileName = string.Empty;
                        //select @delivery=rtrim(left(InternalID,10)),@shipment=rtrim(SHIPMENT),@waybill=rtrim(WAYBILL_NUMBER) from [v_PAKComn] where InternalID = @InternalID
                        pList = repPizza.GetVPakComnList(temp);
                        foreach (VPakComnInfo v in pList)
                        {
                            //if (!String.IsNullOrEmpty(v.internalID) && !String.IsNullOrEmpty(v.shipment) 
                            //        && !String.IsNullOrEmpty(v.waybill_number) && v.internalID.Length >= 10)
                            {
                                if (String.IsNullOrEmpty(v.shipment) && String.IsNullOrEmpty(v.waybill_number))
                                {
                                    pdfFileName = v.internalID.Substring(0, 10).TrimEnd() + "-" + "-" + "-[" + type.Replace(" ", "") + "].pdf";
                                }
                                else if (String.IsNullOrEmpty(v.shipment) && !String.IsNullOrEmpty(v.waybill_number))
                                {
                                    pdfFileName = v.internalID.Substring(0, 10).TrimEnd() + "-" + "-" + v.waybill_number.TrimEnd() + "-[" + type.Replace(" ", "") + "].pdf";
                                }
                                else if (!String.IsNullOrEmpty(v.shipment) && String.IsNullOrEmpty(v.waybill_number))
                                {
                                    pdfFileName = v.internalID.Substring(0, 10).TrimEnd() + "-" + v.shipment.TrimEnd() + "-" + "-[" + type.Replace(" ", "") + "].pdf";
                                }
                                else
                                {
                                    pdfFileName = v.internalID.Substring(0, 10).TrimEnd() + "-" + v.shipment.TrimEnd() + "-" + v.waybill_number.TrimEnd() + "-[" + type.Replace(" ", "") + "].pdf";
                                }
                                //pdfFileName = @delivery+'-'+@shipment+'-'+@waybill &"-["& replace(cmbdoctpye.value," ","")&"].pdf"
                                //pdfFileName = v.internalID.Substring(0, 10).TrimEnd() + "-" + v.shipment.TrimEnd() + "-" + v.waybill_number.TrimEnd() + "-[" + type.Replace(" ", "") + "].pdf";
                                //pdfFileName = v.internalID.Substring(0, 10).TrimEnd() +  "-[" + type.Replace(" ", "") + "].pdf";
                                //生成pdf
                                //cmd = "C:\Program"&chr(34)&" "&chr(34)&"Files\Altova\FOP-0.93\fop -xml \\10.99.183.58\OUT\PACKINGLISTXML\"&XmlFilename&" -xsl \\10.99.183.58\OUT\"&template&" -pdf \\10.99.183.58\out\PACKINGLISTPDF\"&PdfFilename
                                //set   obj=createobject("wscript.shell")  
                                //obj.run   "cmd   /c   "&cmd,vbhide
                                pdfFileList.Add(pdfFileName);
                                pdfFileCount++;
                            }
                        }
                        ArrayList one = new ArrayList();
                        one.Add(temp);
                        one.Add(XmlFilename);
                        one.Add(tempList);
                        one.Add(pdfFileList);
                        

                        pdfAndTemp.Add(one);
                    }//else
                }//for
                CurrentSession.AddValue("PDFAndTemp", pdfAndTemp);
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {                
            }

        }

        //6.18 BOL
        private void CreatePdfAndXmlByDN_FORBOL(IList<string> internalId, string data, string type)
        {
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            try
            {
                IList<string> xmlFileList = new List<string>();
                int xmlFileCount = 0;

                int pdfFileCount = 0;
                ArrayList pdfAndTemp = new ArrayList();

                foreach (string temp in internalId)
                {
                    IList<string> pdfFileList = new List<string>();
                    pdfFileList.Clear();
                    bool b10 = false;
                    if (String.IsNullOrEmpty(temp) || temp.Length < 10)
                        continue;

                    foreach (string t in xmlFileList)
                    {
                        if (temp.Substring(0, 10) == t.Substring(0, 10))
                        {
                            b10 = true;
                            break;
                        }
                    }
                    if (b10 == true)
                        continue;
                    else
                    {
                        //Create XML File!                        
                        //文件名：  @InternalID &"-["& replace(@doc_type," ","")&"].xml"
                        string XmlFilename = temp + "-[" + type.Replace(" ", "") + "].xml";
                        //调用Edits生成XML文件:
                        //SOAPClient = createobject("MSSOAP.SoapClient30")
                        //SOAPClient.mssoapinit("http://10.99.183.28/hpedits_temp/edits.asmx?WSDL")
                        //SOAPClient.PackingShipmentLabel("\\10.99.183.58\OUT\PACKINGLISTXML\"&XmlFilename, @InternalID)

                        xmlFileList.Add(XmlFilename);
                        xmlFileCount++;

                        //得到模板：
                        IList<string> docnumList = new List<string>();
                        IList<string> tempList = new List<string>();
                        bool bFind = false;
                        //select @doc_set_number = DOC_SET_NUMBER  from [PAK.PAKComn] where left(InternalID,10) = @InternalID的前10位
                        docnumList = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(temp.Substring(0, 10));
                        foreach (string num in docnumList)
                        {
                            //SELECT @templatename = XSL_TEMPLATE_NAME FROM [PAK.PAKRT] WHERE DOC_CAT = @doctpye AND DOC_SET_NUMBER = @doc_set_number
                            tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer(type, num);
                            if (tempList != null && tempList.Count != 0)
                            {
                                bFind = true;
                                break;
                            }
                        }
                        if (bFind == false)
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(temp);
                            ex = new FisException("CHK834", erpara);
                            //ex.stopWF = false;
                            throw ex;
                        }
                        //生成pdf文件名:
                        IList<VPakComnInfo> pList = new List<VPakComnInfo>();
                        string pdfFileName = string.Empty;
                        //select @delivery=rtrim(left(InternalID,10)),@shipment=rtrim(SHIPMENT),@waybill=rtrim(WAYBILL_NUMBER) from [v_PAKComn] where InternalID = @InternalID
                        pList = repPizza.GetVPakComnList(temp);
                        foreach (VPakComnInfo v in pList)
                        {
                            //if (!String.IsNullOrEmpty(v.internalID) && !String.IsNullOrEmpty(v.shipment) 
                            //        && !String.IsNullOrEmpty(v.waybill_number) && v.internalID.Length >= 10)
                            {
                                if (String.IsNullOrEmpty(v.shipment) && String.IsNullOrEmpty(v.waybill_number))
                                {
                                    pdfFileName = v.internalID.Substring(0, 10).TrimEnd() + "-" + "-" + "-[" + type.Replace(" ", "") + "].pdf";
                                }
                                else if (String.IsNullOrEmpty(v.shipment) && !String.IsNullOrEmpty(v.waybill_number))
                                {
                                    pdfFileName = v.internalID.Substring(0, 10).TrimEnd() + "-" + "-" + v.waybill_number.TrimEnd() + "-[" + type.Replace(" ", "") + "].pdf";
                                }
                                else if (!String.IsNullOrEmpty(v.shipment) && String.IsNullOrEmpty(v.waybill_number))
                                {
                                    pdfFileName = v.internalID.Substring(0, 10).TrimEnd() + "-" + v.shipment.TrimEnd() + "-" + "-[" + type.Replace(" ", "") + "].pdf";
                                }
                                else
                                {
                                    pdfFileName = v.internalID.Substring(0, 10).TrimEnd() + "-" + v.shipment.TrimEnd() + "-" + v.waybill_number.TrimEnd() + "-[" + type.Replace(" ", "") + "].pdf";
                                }
                                //pdfFileName = @delivery+'-'+@shipment+'-'+@waybill &"-["& replace(cmbdoctpye.value," ","")&"].pdf"
                                //pdfFileName = v.internalID.Substring(0, 10).TrimEnd() + "-" + v.shipment.TrimEnd() + "-" + v.waybill_number.TrimEnd() + "-[" + type.Replace(" ", "") + "].pdf";
                                //pdfFileName = v.internalID.Substring(0, 10).TrimEnd() +  "-[" + type.Replace(" ", "") + "].pdf";
                                //生成pdf
                                //cmd = "C:\Program"&chr(34)&" "&chr(34)&"Files\Altova\FOP-0.93\fop -xml \\10.99.183.58\OUT\PACKINGLISTXML\"&XmlFilename&" -xsl \\10.99.183.58\OUT\"&template&" -pdf \\10.99.183.58\out\PACKINGLISTPDF\"&PdfFilename
                                //set   obj=createobject("wscript.shell")  
                                //obj.run   "cmd   /c   "&cmd,vbhide
                                pdfFileList.Add(pdfFileName);
                                pdfFileCount++;
                            }
                        }
                        ArrayList one = new ArrayList();
                        one.Add(temp);
                        one.Add(XmlFilename);
                        one.Add(tempList);
                        one.Add(pdfFileList);


                        pdfAndTemp.Add(one);
                    }//else
                }//for

                ArrayList bol = (ArrayList)CurrentSession.GetValue("BOLPdfList");
                bol.Add(pdfAndTemp);
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
            }

        }
        //6.18 BOL

        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        public static DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(InputTypeEnum), typeof(CreateFileForPackingList));

        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        [DescriptionAttribute("InputType")]
        [CategoryAttribute("InArugment Of CreateFileForPackingList")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public InputTypeEnum InputType
        {
            get
            {
                return ((InputTypeEnum)(base.GetValue(CreateFileForPackingList.InputTypeProperty)));
            }
            set
            {
                base.SetValue(CreateFileForPackingList.InputTypeProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        public enum InputTypeEnum
        {
            /// <summary>
            /// 
            /// </summary>
            DN = 0,
            /// <summary>
            /// 
            /// </summary>
            Shipment = 1,
            /// <summary>
            /// 
            /// </summary>
            Waybill = 2,
            /// <summary>
            /// 
            /// </summary>
            DNForPL = 3,
            /// <summary>
            /// 
            /// </summary>
            BOLDN = 4,
        }
    }
}
