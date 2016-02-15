using System;
using System.Web;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Reflection;
using System.Web.Configuration;
using Inventec.HPEDITS.XmlCreator;
using Inventec.HPEDITS.XmlCreator.Database;
using log4net;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Text;

/// <summary>
/// Summary description for EDITS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class EDITS : System.Web.Services.WebService
{
    private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    static string fopCfgXml = WebConfigurationManager.AppSettings["FOPCfgXml"];

    static string SMTP = getConfiguration("SMTP");
    static string[] mailList = getConfiguration("mailReceiver").Split(';');
    static string mailSender = getConfiguration("mailSender");
    static string mailSite = getConfiguration("mailSite");

    private static string getConfiguration(string key)
    {
        string configValue = System.Configuration.ConfigurationManager.AppSettings[key];
        if (configValue == null)
        {
            configValue = string.Empty;
        }
        return configValue.Trim();
    }

    private void Mail(string title, string body)
    {
        try
        {
            MailMessage message = new MailMessage();
            MailAddress from = new MailAddress(mailSender, "Edits System");
            message.From = from;

            string addr = "";
            foreach (String s in mailList)
            {
                addr = s;
                if (s.IndexOf("@") == -1)
                {
                    addr = s + "@inventec.com.cn";
                }
                //logger.Info("mail receiver: " + addr);
                message.To.Add(new MailAddress(addr));
            }

            message.IsBodyHtml = false;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Body = body;
            message.Subject = mailSite + " Edits msg: " + title;
            //logger.Info("Smpt: " + SMTP);
            SmtpClient smtpClient = new SmtpClient(SMTP, 25);
            //   smtpClient.Credentials
            smtpClient.Send(message);
        }
        catch (Exception err)
        {
            RecordError("Mail", ref err, false);
        }
    }

    private string RecordError(string method, ref Exception err, bool doMail)
    {
        string res ="";
        StringBuilder sb = new StringBuilder(512);
        sb.Append(method);
        if (err != null)
        {
            res = err.Message.ToString();
            sb.Append(" ").Append(res).Append(Environment.NewLine).Append(err.Message).Append(Environment.NewLine).Append(Environment.NewLine).Append(err.StackTrace);
        }
        logger.Error(sb.ToString());
        if (doMail)
            Mail("err in " + method, sb.ToString());
        return res;
    }

    private string RecordError(string method, ref Exception err)
    {
        return RecordError(method, ref err, true);
    }

    public EDITS()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public EDITSResponse BoxShipLabel(string serialNO, string boxID)
    //public string BoxShipLabel(string serialNO, string boxID)
    {
        logger.Info("BoxShipLabel begin; serialNO=" + serialNO + " , boxID=" + boxID);
        //string FilePath=@"D:\hpedits\uploadfiles\aa.xml";

        EDITSResponse response = new EDITSResponse();
        BoxLabelXmlCreator xmlCreator = new BoxLabelXmlCreator();
        string whereClause="WHERE SERIAL_NUM='"+serialNO.Trim()+"'";
        List<string> fields=new List<string>();
        fields.Add("InternalID");
        DataTable readTable=DBFactory.PopulateTempTable("[PAK_PackkingData]",whereClause,fields);
        if (readTable.Rows.Count > 0)
        {
            string internalID = readTable.Rows[0]["InternalID"].ToString();
            string concatenatedID = internalID.Trim() + @"/" + boxID.Trim();
            //string concatenatedID = internalID.Trim() + @"/" + serialNO.Trim();
            xmlCreator.LoadBoxLabelDatabaseData(concatenatedID);
            XmlDataDocument dataDocument = xmlCreator.GetXML(true);
            response.XML = dataDocument.InnerXml;
            //xmlCreator.WriteXml(FilePath, true);
            //return internalID.Trim();
        }
        logger.Info("BoxShipLabel end");
        //return "";
        return response;
    }

    [WebMethod]
    public string PackingLabel(string FilePH, string Dn)
    {
        logger.Info("PackingLabel begin; FilePH=" + FilePH + " , Dn=" + Dn);
        XmlCreator xmlCreator = null;
        xmlCreator = new PackListXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadData(Dn.ToString()+@"/"+"");
            xmlCreator.WriteXml(FilePH, false);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("PackingLabel", ref err);
        }
        finally
        {
            logger.Info("PackingLabel end");
        }
        return res;
    }   

    /*
    [WebMethod]
    public string PackingShipmentLabel(string FilePH, string Dn)
    {
        return PackingShipmentLabel(FilePH, Dn, "");
    }
    */

    [WebMethod]
    public string PackingShipmentLabel(string FilePH, string Dn, string Tp )
    {
        logger.Info("PackingShipmentLabel begin; FilePH=" + FilePH + " , Dn=" + Dn + " , Tp=" + Tp);
        XmlCreator xmlCreator = null;
        xmlCreator = new PackListShipmentXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadData(Dn.ToString().Trim() + @"/" + Tp.ToString().Trim());
            xmlCreator.WriteXml(FilePH, false);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("PackingShipmentLabel", ref err);
        }
        finally
        {
            logger.Info("PackingShipmentLabel end");
        }
        return res;
    }
    [WebMethod]
    public string BoxShipToShipmentMRPLabel(string FilePH, string Dn, string SerialNum, string mrp)
    {
        logger.Info("BoxShipToShipmentMRPLabel begin; FilePH=" + FilePH + " , Dn=" + Dn + " , SerialNum=" + SerialNum);
        BoxShipmentMRPXMLCreator xmlCreator = null;
        xmlCreator = new BoxShipmentMRPXMLCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadBoxLabelDatabaseData(Dn.ToString().Trim() + @"/" + SerialNum.ToString().Trim() + @"/" + mrp.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("BoxShipToShipmentMRPLabel", ref err);
        }
        finally
        {
            logger.Info("BoxShipToShipmentMRPLabel end");
        }
        return res;
    }

    [WebMethod]
    public string BoxShipToLabel(string FilePH, string Dn, string SerialNum)
    {
        logger.Info("BoxShipToLabel begin; FilePH=" + FilePH + " , Dn=" + Dn + " , SerialNum=" + SerialNum);
        BoxLabelXmlCreator xmlCreator = null;
        xmlCreator = new BoxLabelXmlCreator(); 
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadBoxLabelDatabaseData(Dn.ToString().Trim() + @"/" + SerialNum.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("BoxShipToLabel", ref err);
        }
        finally
        {
            logger.Info("BoxShipToLabel end");
        }
        return res;
    }

    [WebMethod]
    public string BoxShipToShipmentLabel(string FilePH, string Dn, string SerialNum)
    {
        logger.Info("BoxShipToShipmentLabel begin; FilePH=" + FilePH + " , Dn=" + Dn + " , SerialNum=" + SerialNum);
        BoxLabelShipmentXmlCreator xmlCreator = null;
        xmlCreator = new BoxLabelShipmentXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadBoxLabelDatabaseData(Dn.ToString().Trim() + @"/" + SerialNum.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("BoxShipToShipmentLabel", ref err);
        }
        finally
        {
            logger.Info("BoxShipToShipmentLabel end");
        }
        return res;
    }

    [WebMethod]
    public string PalletALabel(string FilePH, string Dn,string Pallet)
    {
        logger.Info("PalletALabel begin; FilePH=" + FilePH + " , Dn=" + Dn + " , Pallet=" + Pallet);
        PalletAXmlCreator xmlCreator = null;
        xmlCreator = new PalletAXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadPalletADatabaseData(Dn.ToString().Trim() + @"/" + Pallet.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("PalletALabel", ref err);
        }
        finally
        {
            logger.Info("PalletALabel end");
        }
        return res;
    }

    [WebMethod]
    public string PalletAShipmentLabel(string FilePH, string Dn, string Pallet)
    {
        logger.Info("PalletAShipmentLabel begin; FilePH=" + FilePH + " , Dn=" + Dn + " , Pallet=" + Pallet);
        PalletAShipmentXmlCreator xmlCreator = null;
        xmlCreator = new PalletAShipmentXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadPalletADatabaseData(Dn.ToString().Trim() + @"/" + Pallet.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("PalletAShipmentLabel", ref err);
        }
        finally
        {
            logger.Info("PalletAShipmentLabel end");
        }
        return res;
    }

    [WebMethod]
    public string PalletBLabel(string FilePH, string Dn, string Pallet)
    {
        logger.Info("PalletBLabel begin; FilePH=" + FilePH + " , Dn=" + Dn + " , Pallet=" + Pallet);
        PalletBXmlCreator xmlCreator = null;
        xmlCreator = new PalletBXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadPalletBDatabaseData(Dn.ToString().Trim() + @"/" + Pallet.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("PalletBLabel", ref err);
        }
        finally
        {
            logger.Info("PalletBLabel end");
        }
        return res;
    }

    [WebMethod]
    public string PalletBShipmentLabel(string FilePH, string Dn, string Pallet)
    {
        logger.Info("PalletBShipmentLabel begin; FilePH=" + FilePH + " , Dn=" + Dn + " , Pallet=" + Pallet);
        PalletBShipmentXmlCreator xmlCreator = null;
        xmlCreator = new PalletBShipmentXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadPalletBDatabaseData(Dn.ToString().Trim() + @"/" + Pallet.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("PalletBShipmentLabel", ref err);
        }
        finally
        {
            logger.Info("PalletBShipmentLabel end");
        }
        return res;
    }

	[WebMethod]
    public string BsamPalletBShipmentLabel(string FilePH, string Dn, string Pallet)
    {
        logger.Info("BsamPalletBShipmentLabel begin; FilePH=" + FilePH + " , Dn=" + Dn + " , Pallet=" + Pallet);
        PalletBShipmentBsamXmlCreator xmlCreator = null;
        xmlCreator = new PalletBShipmentBsamXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadPalletBDatabaseData(Dn.ToString().Trim() + @"/" + Pallet.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("BsamPalletBShipmentLabel", ref err);
        }
        finally
        {
            logger.Info("BsamPalletBShipmentLabel end");
        }
        return res;
    }

    [WebMethod]
    public string WayBillList(string FilePH, string Dn)
    {
        logger.Info("WayBillList begin; FilePH=" + FilePH + " , Dn=" + Dn);
        XmlCreator xmlCreator = null;
        //xmlCreator = new WayBillXmlCreator();
        xmlCreator = new WayBillShipmentXmlCreator();

        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadData(Dn.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("WayBillList", ref err);
        }
        finally
        {
            logger.Info("WayBillList end");
        }
        return res;
    }

    [WebMethod]
    public string WayBillShipmentList(string FilePH, string Dn)
    {
        logger.Info("WayBillShipmentList begin; FilePH=" + FilePH + " , Dn=" + Dn);
        XmlCreator xmlCreator = null;
       // xmlCreator = new WayBillShipmentXmlCreator();
        xmlCreator = new WayBillShipmentXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadData(Dn.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("WayBillShipmentList", ref err);
        }
        finally
        {
            logger.Info("WayBillShipmentList end");
        }
        return res;
    }

    [WebMethod(Description = "Master_Waybill")]
    public string MasterWaybillList(string FilePH, string Dn)
    {
        logger.Info("MasterWaybillList begin; FilePH=" + FilePH + " , Dn=" + Dn);
        MasterWaybillShipmentXmlCreator xmlCreator = null;
        xmlCreator = new MasterWaybillShipmentXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadData(Dn.ToString().Trim() + @"/" + "");
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("MasterWaybillList", ref err);
        }
        finally
        {
            logger.Info("MasterWaybillList end");
        }
        return res;
    }
	
	[WebMethod(Description = "QVC_packList")]
    public string PackingShipment_QVCLabel(string FilePH, string Dn)
    {
        logger.Info("PackingShipment_QVCLabel begin; FilePH=" + FilePH + " , Dn=" + Dn);
		PackListShipmentXmlCreator_QVC xmlCreator = null;
        xmlCreator = new PackListShipmentXmlCreator_QVC();
        string res = null;
        try
        {
            xmlCreator.LoadData(Dn.ToString().Trim() + @"/" + "", FilePH);
            //xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
		finally
        {
            logger.Info("PackingShipment_QVCLabel end");
        }
        return res;

    }

	[WebMethod]
    public string HouseWaybills(string FilePH, string Dn)
    {
        logger.Info("HouseWaybills begin; FilePH=" + FilePH + " , Dn=" + Dn);
        XmlCreator xmlCreator = null;
        xmlCreator = new HouseWaybillsXmlCreator();
        string res = null;
        try
        {
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadData(Dn.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = RecordError("HouseWaybills", ref err);
        }
        finally
        {
            logger.Info("HouseWaybills end");
        }
        return res;
    }


    [WebMethod]
    public string GenPosXML(string FilePH, string truckId, string shipDate)
    {
        logger.Info("GenPosXML begin; FilePH=" + FilePH + " , truckId=" + truckId + " , shipDate=" + shipDate);
        XmlCreator xmlCreator = null;
        xmlCreator = new POSXmlCreator();
        string res = null;
        try
        {
            string v = truckId + @"@" + shipDate;
            ChkAndCreatePath(FilePH);
            xmlCreator.LoadData(v);
            xmlCreator.WriteXml(FilePH, false);
            res = truckId;
        }
        catch (Exception err)
        {
            res = RecordError("GenPosXML", ref err);
        }
        finally
        {
            logger.Info("GenPosXML end");
        }
        return res;
    }

    private void ChkAndCreatePath(string f)
    {
        try
        {
            string p = Path.GetDirectoryName(f);
            if (!Directory.Exists(p))
            {
                Directory.CreateDirectory(p);
            }
        }
        catch (Exception ex)
        {
            RecordError("ChkAndCreatePath", ref ex);
        }
    }

    [WebMethod]
    public bool GenPDF(string xslFile, string xmlFile, string pdfFile, ref string ErrText)
    {
        logger.Info("GenPDF begin; xslFile=" + xslFile + " , xmlFile=" + xmlFile + " , pdfFile=" + pdfFile);
        try
        {
            pdfFile = ResolveLocalPath(pdfFile);
            return GenFile(xslFile, xmlFile, pdfFile, ref ErrText, "pdf");
        }
        finally
        {
            logger.Info("GenPDF end; xslFile=" + xslFile + " , xmlFile=" + xmlFile + " , pdfFile=" + pdfFile);
        }
    }

    [WebMethod]
    public bool GenFO(string xslFile, string xmlFile, string foFile, ref string ErrText)
    {
        logger.Info("GenFO begin; xslFile=" + xslFile + " , xmlFile=" + xmlFile + " , foFile=" + foFile);
        try
        {
            return GenFile(xslFile, xmlFile, foFile, ref ErrText, "foout");
        }
        finally
        {
            logger.Info("GenFO end; xslFile=" + xslFile + " , xmlFile=" + xmlFile + " , foFile=" + foFile);
        }
    }

    static private bool IsLocalHost(string input)
    {
        IPAddress[] host;
        //get host addresses
        try { host = Dns.GetHostAddresses(input); }
        catch (Exception) { return false; }

        //get local adresses
        IPAddress[] local = Dns.GetHostAddresses(Dns.GetHostName());

        //check if local
        //return host.Any(hostAddress => IPAddress.IsLoopback(hostAddress) || local.Contains(hostAddress));
        for (int i = 0; i < host.Length; i++)
        {
            IPAddress hostAddress = host[i];
            if (IPAddress.IsLoopback(hostAddress))
                return true;
            for (int j = 0; j < local.Length; j++)
            {
                IPAddress localAddress = local[j];
                if (hostAddress.Equals(localAddress))
                    return true;
            }
        }
        return false;
    }

    public string ResolveLocalPath(string UNC)
    {
        try
        {
            //logger.Debug("ResolveLocalPath begin " + UNC);
            Uri uri = new Uri(UNC);
            if (uri != null && uri.IsUnc && (uri.IsLoopback || IsLocalHost(uri.Host)) && (uri.Segments!=null && uri.Segments.Length>1))
            {
                string sql = "SELECT Path FROM Win32_share WHERE Name = '{0}'";
                string sPath = Path.GetDirectoryName(uri.Segments[1]);
                //logger.Debug("sPath="+sPath);
                sql = string.Format(sql, sPath);

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(sql))
                {
                    if (searcher != null) {
                        ManagementObjectCollection coll = searcher.Get();
                        if (coll!=null && coll.Count == 1)
                        {
                            foreach (ManagementObject share in coll)
                            {
                                object p = share["Path"];
                                if (p != null)
                                {
                                    //logger.Debug("Path=" + share["Path"].ToString());
                                    string sOtherPathFile = share["Path"].ToString() + UNC.Substring(UNC.IndexOf(sPath) + sPath.Length);
                                    logger.Debug("ResolveLocalPath from " + UNC + " to " + sOtherPathFile);
                                    return sOtherPathFile;
                                }
                                else
                                {
                                    foreach (PropertyData prop in share.Properties)
                                    {
                                        logger.Debug("ResolveLocalPath list: name=" + prop.Name + " , value=" + prop.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            RecordError("PackingShipmentLabel", ref ex);
        }
        finally
        {
            //logger.Debug("ResolveLocalPath end");
        }
        return UNC;
    }

    public bool GenFile(string xslFile, string xmlFile, string targetFile, ref string ErrText, string type)
    {
        string actName = "Gen" + type;
        string fileName = type + "File";
        bool result = false;
        Exception e = null;
        try
        {
            if (!File.Exists(xslFile))
            {
                ErrText = actName + ", xslFile not found: " + xslFile;
                RecordError(ErrText, ref e);
                return false;
            }
            if (!File.Exists(xmlFile))
            {
                ErrText = actName + ", xmlFile not found: " + xmlFile;
                RecordError(ErrText, ref e);
                return false;
            }
            if (!File.Exists(fopCfgXml))
            {
                ErrText = actName + ", fopCfgXml not found: " + fopCfgXml;
                RecordError(ErrText, ref e);
                return false;
            }
            //if (File.Exists(targetFile))
            //{
            //    ErrText = fileName + " existed:\n + targetFile;
            //    return false;
            //}
            ChkAndCreatePath(targetFile);

            logger.Debug(actName + " begin xslFile=" + xslFile + " , xmlFile=" + xmlFile + " , " + fileName + "=" + targetFile);

            string[] args = new string[] {
            // setup config file
            "-c", fopCfgXml,
            "-xml", xmlFile,
            "-xsl", xslFile,
            "-"+type, targetFile };

            InvokeFOP(args);

            if (!File.Exists(targetFile))
            {
                ErrText = fileName + " not created:\n" + targetFile;
                result = false;
            }
            result = true;
        }
        catch (Exception ex)
        {
            RecordError(actName, ref ex);
            ErrText = ex.Message;
        }
        finally
        {
            logger.Debug(actName + " end");
        }

        return result;
    }

    void InvokeFOP(string[] args)
    {
        bool dependent = org.apache.fop.cli.Main.checkDependencies();
        if (dependent)
        {
            org.apache.fop.cli.CommandLineOptions options = null;
            org.apache.fop.apps.FOUserAgent foUserAgent = null;
            java.io.OutputStream outx = null;

            try
            {
                options = new org.apache.fop.cli.CommandLineOptions();
                options.parse(args);

                Type options_type = typeof(org.apache.fop.cli.CommandLineOptions);
                System.Reflection.MethodInfo mi = options_type.GetMethod("getFOUserAgent",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);

                //foUserAgent = options.getFOUserAgent();
                foUserAgent = (org.apache.fop.apps.FOUserAgent)mi.Invoke(
                    options, new object[] { });

                mi = options_type.GetMethod("getOutputFormat",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);

                //String outputFormat = options.getOutputFormat();
                String outputFormat = (String)mi.Invoke(
                    options, new object[] { });

                try
                {
                    if (options.getOutputFile() != null)
                    {
                        outx = new java.io.BufferedOutputStream(
                            new java.io.FileOutputStream(options.getOutputFile()));
                        foUserAgent.setOutputFile(options.getOutputFile());
                    }

                    if (!org.apache.fop.apps.MimeConstants.__Fields.MIME_XSL_FO.Equals(outputFormat))
                    {
                        options.getInputHandler().renderTo(foUserAgent, outputFormat, outx);
                    }
                    else
                    {
                        options.getInputHandler().transformTo(outx);
                    }
                }
                finally
                {
                    org.apache.commons.io.IOUtils.closeQuietly(outx);
                }
            }
            catch (Exception ex)
            {
                if (options != null && options.getOutputFile() != null)
                {
                    options.getOutputFile().delete();
                }
                throw ex;
            }
        }
    }

}

[Serializable]
public class EDITSResponse
{
    public string DOC_SET_NUMBER;
    public string XML;
    public EDITSResponse()
    {
        this.DOC_SET_NUMBER = string.Empty;
        this.XML = string.Empty;
    }
    public EDITSResponse(string docSetNumber, string xml)
    {
        this.DOC_SET_NUMBER = docSetNumber;
        this.XML = xml;
    }
}



