using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Inventec.HPEDITS.XmlCreator;
using Inventec.HPEDITS.PDFCreator;
using Inventec.HPEDITS.XmlCreator.Database;
using System.Text;
using System.Reflection;
using log4net;

public partial class Order_Test : System.Web.UI.Page
{
    private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static string PDF = "PDF";
    private static string FO = "FO";

    private bool DoGenFile(string type, string xsl, string xml, string target)
    {
        bool result = false;
        logger.Info("DoGenFile " + type + " begin; xsl=" + xsl + " , xml=" + xml + " , target=" + target);
        using (localhost.EDITS c = new localhost.EDITS())
        {
            string errmsg = "";
            if (PDF.Equals(type))
                result = c.GenPDF(xsl, xml, target, ref errmsg);
            else
                result = c.GenFO(xsl, xml, target, ref errmsg);
        }
        logger.Info("DoGenFile " + type + " end");
        return result;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
 
    }
    protected void Button_Qry_Click(object sender, EventArgs e)
    {

    }

    protected void SNVIEW_SelectedIndexChanged(object sender, EventArgs e)
    {
        XmlCreator xmlCreator = null;//实例化xmlcreator
        bool useOuter = true;
        string version = "";
        string Pallet_Id = SNVIEW.SelectedRow.Cells[1].Text.ToString().Trim();
        string Serial_Num = SNVIEW.SelectedRow.Cells[2].Text.ToString().Trim();
            //String value = DOCVIEW.Columns[1];
            //string doc = DOCVIEW.Rows[1];
            //Response.Write("<script type='text/javascript'>alert('"+ DOCVIEW.Rows[1] +"');</script>");
        DataConnLib con1 = new DataConnLib(ConfigurationManager.AppSettings["Database"]);
        try
        {
            string query = "select top 1 (VERSION) from [PAK.PAKRT_Log] nolock order by CREATE_DATE desc";
            con1.SqlQueComm(query, "VERSION");
            version = con1.Ds.Tables[0].Rows[0][0].ToString().Trim(); 
        }
        catch (System.Exception err) 
        {
            System.Windows.Forms.MessageBox.Show(err.Message.ToString());
        }
        DataConnLib con = new DataConnLib(ConfigurationManager.AppSettings["Database"]);
        try 
        {
            string query = "select distinct b.DOC_CAT,b.DOC_SET_NUMBER, b.XSL_TEMPLATE_NAME, a.PO_NUM, b.SCHEMA_FILE_NAME,a.CARTON_QTY from [PAK_PAKComn] a,[PAK.PAKRT] b where a.DOC_SET_NUMBER = b.DOC_SET_NUMBER   collate Chinese_Taiwan_Stroke_90_BIN  and a.InternalID = '" + BOX_DN.Text.ToString().Trim() + "'";//collate Chinese_Taiwan_Stroke_90_BIN 
            con.SqlQueComm(query,"DOCTable");
            if (con.Ds.Tables.Count > 0) 
            {   
                if (con.Ds.Tables[0].Rows.Count > 0)
                {
                    string dateyear = System.DateTime.Now.Year.ToString();
                    string datemonth = System.DateTime.Now.Month.ToString().Trim();
                    if (datemonth.Length == 1)
                    {
                        datemonth = "0" + datemonth;
                    }
                    string dateday = System.DateTime.Now.Day.ToString().Trim();
                    if (dateday.Length == 1)
                    {
                        dateday = "0" + dateday;
                    }
                    if (RadioButton_Step1.Checked)
                    {
                        Label_Messege.Text = "ZipFileName: Inventec_PRE_MTP_TEST_DATE_" + dateyear + datemonth + dateday + "_EDITS_REVISION_" + dateyear + version;
                    }
                    else if (RadioButton_Step2.Checked)
                    {
                        Label_Messege.Text = "ZipFileName: Inventec_MTP_TEST_DATE_" + dateyear + datemonth + dateday + "_EDITS_REVISION_" + dateyear + version;
                    }
                    else
                    {
                        Label_Messege.Text = "ZipFileName: Inventec_PROD_CHECK_DATE_" + dateyear + datemonth + dateday + "_EDITS_REVISION_" + dateyear +version;
                    }
                    #region step1&step2
                    if (RadioButton_Step1.Checked || RadioButton_Step2.Checked)
                    {
                       // for (int i = 0; i <= con.Ds.Tables[0].Rows.Count; i++)
                        for (int i = 0; i < con.Ds.Tables[0].Rows.Count; i++)
                        {
                            bool isBsam = false;
                            if (con.Ds.Tables[0].Rows[i][4].ToString().ToUpper().IndexOf("BSAM") > 0)
                                isBsam = true;
                            if (int.Parse(con.Ds.Tables[0].Rows[i][5].ToString()) > 1)                         
                            {
                                if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label_Tablet_MRP")
                                {
                                    useOuter = true;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new BoxShipmentMRPXMLCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num + "/" + "M" + Serial_Num);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);


                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);

                                }
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label_Tablet_Wholesale")
                                {
                                    useOuter = true;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new BoxLabelShipmentXmlCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);


                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);

                                }
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label" && isBsam)
                                {
                                    useOuter = true;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new BoxLabelShipmentXmlCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);


                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);

                                }
                            }
                            else 
                            {
                                if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label_Single Pack")
                                {
                                    useOuter = true;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new BoxLabelShipmentXmlCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);


                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);

                                }
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label")
                                {
                                    useOuter = true;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new BoxLabelShipmentXmlCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);


                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);

                                }
                                #region mark
                                /*
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label_Tablet_MRP")
                                {
                                    useOuter = true;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new BoxShipmentMRPXMLCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num + "/" + "M" + Serial_Num);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);


                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);

                                }
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label_Tablet_Wholesale")
                                {
                                    useOuter = true;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new BoxLabelShipmentXmlCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);


                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);

                                }
                                 **/
                                #endregion
                            }
                                if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Pallet Ship Label- Pack ID Single")
                                {
                                    useOuter = true;
                                    //////if (Pallet_Id.Substring(0,2).ToString() == "NA" || Pallet_Id.Substring(0,2).ToString() == "BA")
                                    //////{
                                    //////    continue;
                                    //////}
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    if (filename_XML.IndexOf("TypeA") != -1)
                                    {
                                        xmlCreator = new PalletAShipmentXmlCreator();
                                    }
                                    else
                                    {
                                        if (isBsam)
                                            xmlCreator = new PalletBShipmentBsamXmlCreator();
                                        else
                                            xmlCreator = new PalletBShipmentXmlCreator();
                                    }
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Pallet_Id);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);

                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);

                                }
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Pack List- Transportation")
                                {
                                    useOuter = false;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    #region Modify by 20130312
                                    //xmlCreator = new PackListShipmentXmlCreator();
                                    if (con.Ds.Tables[0].Rows[i][2].ToString().Contains("Rail"))
                                    {
                                        useOuter = true;
                                        //xmlCreator = new MasterWaybillShipmentXmlCreator();
                                        xmlCreator = new MasterWaybillShipmentXmlCreator();
                                    }
                                    else
                                    {
                                        xmlCreator = new PackListShipmentXmlCreator();
                                    }
                                    #endregion
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/Pack List- Transportation");
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);

                                    ///*
                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                                    //*/ 
                                }
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Pack List- Shipment")
                                {
                                    useOuter = false;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;


                                    if (isBsam)
                                    {
                                        xmlCreator = new HouseWaybillsXmlCreator();
                                        xmlCreator.LoadData(BOX_DN.Text.Trim());
                                        xmlCreator.WriteXml(docName_XML, true);
                                    }
                                    else if (con.Ds.Tables[0].Rows[i][2].ToString().Contains("QVC")) //add for QVC 201305
                                    {
                                        xmlCreator = new PackListShipmentXmlCreator_QVC();
                                        //string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                        xmlCreator.LoadData(BOX_DN.Text.Trim() + "/Pack List- Shipment", docName_XML);

                                    }
                                    else
                                    {
                                        if (con.Ds.Tables[0].Rows[i][2].ToString().Contains("Rail"))
                                        {
                                            useOuter = true;
                                            //xmlCreator = new MasterWaybillShipmentXmlCreator();
                                            xmlCreator = new MasterWaybillShipmentXmlCreator();
                                        }
                                        else
                                        {
                                            xmlCreator = new PackListShipmentXmlCreator();
                                        }
                                        xmlCreator.LoadData(BOX_DN.Text.Trim() + "/Pack List- Shipment");
                                        //string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                        xmlCreator.WriteXml(docName_XML, useOuter);
                                    }
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);
                                    ///*
                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                                    //*/
                                }
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Pack List- Addition")
                                {
                                    useOuter = false;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new PackListShipmentXmlCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/Pack List- Addition");
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);

                                    ///*
                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                                    //*/
                                }
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Waybill- Addition")
                                {
                                    useOuter = true;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    //xmlCreator = new WayBillShipmentXmlCreator();
                                    xmlCreator = new WayBillShipmentXmlCreator();

                                    xmlCreator.LoadData(BOX_DN.Text.Trim());
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);
                                    ///*
                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                                    //*/ 
                                }
                                
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "POS")
                                {
                                    useOuter = false;
                                    string filename_XML = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = DropDownList_ID.Text + "_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    //xmlCreator = new WayBillShipmentXmlCreator();
                                    string query_step3 = "select a.TruckID,[Date]   from HPIMES.dbo.Forwarder a,HPIMES.dbo.MAWB b where a.MAWB=b.MAWB or a.MAWB=b.HAWB and b.Delivery= '" + BOX_DN.Text.ToString().Trim() + "'";//collate Chinese_Taiwan_Stroke_90_BIN 
                                    con.SqlQueComm(query_step3, "Truck");
                                    if (con.Ds.Tables["Truck"].Rows.Count < 1)
                                    {
                                        Exception ex = new Exception("NO MAWB Or Forwarder Data!");
                                        throw ex;

                                    }

                                    xmlCreator = new POSXmlCreator();
                                    string TruckInfo = "";
                                    TruckInfo = con.Ds.Tables["Truck"].Rows[0][0] + @"@" + con.Ds.Tables["Truck"].Rows[0][1];
                                    xmlCreator.LoadData(TruckInfo);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);
                                    ///*
                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                                    //*/ 
                                }
                        
                        }
                    }
                    #endregion
                    #region step3
                    else if (RadioButton_Step3.Checked)
                    {
                        //for (int i = 0; i <= con.Ds.Tables[0].Rows.Count; i++)
                        for (int i = 0; i < con.Ds.Tables[0].Rows.Count; i++)
                        {
                            bool isBsam = false;
                            if (con.Ds.Tables[0].Rows[i][4].ToString().ToUpper().IndexOf("BSAM") > 0)
                                isBsam = true;
                            if (int.Parse(con.Ds.Tables[0].Rows[i][5].ToString()) > 1)
                            {
                                //if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label_Over Pack_Gift Box")
                                if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label_Tablet_MRP")
                                {
                                    useOuter = true;
                                    string filename_XML = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new BoxShipmentMRPXMLCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num + "/" + "M" + Serial_Num);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);


                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);

                                }
                               // else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label_Over Pack_Wholesale")
                                else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label_Tablet_Wholesale")
                                {
                                    useOuter = true;
                                    string filename_XML = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new BoxLabelShipmentXmlCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);


                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);

                                }
                                if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label"&&isBsam)
                                {
                                    useOuter = true;
                                    string filename_XML = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                    string filename_PDF = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                    string filename_FO = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                    xmlCreator = new BoxLabelShipmentXmlCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num);
                                    string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                    string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                    string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                    string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                    //FOPWrap.FOP.GeneratePDF(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_PDF);
                                    //FOPWrap.FOP.GenerateFo(
                                    //    fopPath,
                                    //    docName_XML,
                                    //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    //    docName_FO);
                                    DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                    DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                                }
                            }
                           else
                            {
                                if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label" || con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Box Ship Label_Single Pack")
                             {
                                useOuter = true;
                                string filename_XML = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                string filename_PDF = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                string filename_FO = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                xmlCreator = new BoxLabelShipmentXmlCreator();
                                xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Serial_Num);
                                string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                xmlCreator.WriteXml(docName_XML, useOuter);
                                string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                //FOPWrap.FOP.GeneratePDF(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_PDF);
                                //FOPWrap.FOP.GenerateFo(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_FO);
                                DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                             }
                            }
                             if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Pallet Ship Label- Pack ID Single")
                            {
                                useOuter = true;
                                //if (Pallet_Id.Substring(0, 2).ToString() == "NA" || Pallet_Id.Substring(0, 2).ToString() == "BA")
                                //{
                                //    continue;
                                //}
                                string filename_XML = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                string filename_PDF = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                string filename_FO = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                if (filename_XML.IndexOf("TypeA") != -1)
                                {
                                    xmlCreator = new PalletAShipmentXmlCreator();
                                }
                                else
                                {
                                    if (isBsam)
                                        xmlCreator = new PalletBShipmentBsamXmlCreator();
                                    else
                                        xmlCreator = new PalletBShipmentXmlCreator();
                                }
                                xmlCreator.LoadData(BOX_DN.Text.Trim() + "/" + Pallet_Id);
                                string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                xmlCreator.WriteXml(docName_XML, useOuter);
                                string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                //FOPWrap.FOP.GeneratePDF(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_PDF);
                                //FOPWrap.FOP.GenerateFo(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_FO);
                                DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                            }
                            else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Pack List- Transportation")
                            {
                                useOuter = false;
                                string filename_XML = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                string filename_PDF = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                string filename_FO = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                //xmlCreator = new PackListShipmentXmlCreator();
                                if (con.Ds.Tables[0].Rows[i][2].ToString().Contains("Rail"))
                                {
                                    useOuter = true;
                                    xmlCreator = new MasterWaybillShipmentXmlCreator();
                                }
                                else
                                {
                                    xmlCreator = new PackListShipmentXmlCreator();
                                }
                                xmlCreator.LoadData(BOX_DN.Text.Trim() + "/Pack List- Transportation");
                                string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                xmlCreator.WriteXml(docName_XML, useOuter);
                                string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                //FOPWrap.FOP.GeneratePDF(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_PDF);
                                //FOPWrap.FOP.GenerateFo(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_FO);
                                DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                            }
                            else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Pack List- Shipment")
                            {
                                useOuter = false;
                                string filename_XML = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                string filename_PDF = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                string filename_FO = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;

                                if (isBsam)
                                {
                                    xmlCreator = new HouseWaybillsXmlCreator();
                                    xmlCreator.LoadData(BOX_DN.Text.Trim());
                                    xmlCreator.WriteXml(docName_XML, true);
                                }
                                else if (con.Ds.Tables[0].Rows[i][2].ToString().Contains("QVC")) //add for QVC 201305
                                {
                                    xmlCreator = new PackListShipmentXmlCreator_QVC();
                                    //string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/Pack List- Shipment", docName_XML);

                                }
                                else
                                {
                                    if (con.Ds.Tables[0].Rows[i][2].ToString().Contains("Rail"))
                                    {
                                        useOuter = true;
                                        xmlCreator = new MasterWaybillShipmentXmlCreator();
                                    }
                                    else
                                    {
                                        xmlCreator = new PackListShipmentXmlCreator();
                                    }
                                    xmlCreator.LoadData(BOX_DN.Text.Trim() + "/Pack List- Shipment");
                                    //string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                    xmlCreator.WriteXml(docName_XML, useOuter);
                                }
                                string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                //FOPWrap.FOP.GeneratePDF(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_PDF);
                                //FOPWrap.FOP.GenerateFo(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_FO);
                                DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                                #region mark
                                /*
                                //xmlCreator = new PackListShipmentXmlCreator();
                                if (con.Ds.Tables[0].Rows[i][2].ToString().Contains("Rail"))
                                {
                                    useOuter = true;
                                    xmlCreator = new MasterWaybillShipmentXmlCreator();
                                }
                                else if (con.Ds.Tables[0].Rows[i][2].ToString().Contains("QVC"))
                                {
                                    xmlCreator = new PackListShipmentXmlCreator_QVC();
                                }
                                else
                                {
                                    xmlCreator = new PackListShipmentXmlCreator();
                                }
                              //  xmlCreator.LoadData(BOX_DN.Text.Trim());
                                xmlCreator.LoadData(BOX_DN.Text.Trim() + "/Pack List- Shipment");
                                string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                xmlCreator.WriteXml(docName_XML, useOuter);
                                string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                FOPWrap.FOP.GeneratePDF(
                                    fopPath,
                                    docName_XML,
                                    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    docName_PDF);
                                FOPWrap.FOP.GenerateFo(
                                    fopPath,
                                    docName_XML,
                                    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                    docName_FO);*/
                                #endregion
                            }
                            else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Pack List- Addition")
                            {
                                useOuter = false;
                                string filename_XML = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                string filename_PDF = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                string filename_FO = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                xmlCreator = new PackListShipmentXmlCreator();
                                //xmlCreator.LoadData(BOX_DN.Text.Trim() + "/Pack List- Addition");
                                xmlCreator.LoadData(BOX_DN.Text.Trim() + "/Pack List- Addition");
                                string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                xmlCreator.WriteXml(docName_XML, useOuter);
                                string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                //FOPWrap.FOP.GeneratePDF(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_PDF);
                                //FOPWrap.FOP.GenerateFo(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_FO);
                                DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                            }
                            else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "Waybill- Addition")
                            {
                                useOuter = true;
                                string filename_XML = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                                string filename_PDF = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                                string filename_FO = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                                //xmlCreator = new WayBillShipmentXmlCreator();
                                xmlCreator = new WayBillShipmentXmlCreator();
                                xmlCreator.LoadData(BOX_DN.Text.Trim() );
                                string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                                xmlCreator.WriteXml(docName_XML, useOuter);
                                string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                                string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                                string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                                //FOPWrap.FOP.GeneratePDF(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_PDF);
                                //FOPWrap.FOP.GenerateFo(
                                //    fopPath,
                                //    docName_XML,
                                //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                                //    docName_FO);
                                DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                                DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                            }
                             #region POS
                             
                             else if (con.Ds.Tables[0].Rows[i][0].ToString().Trim() == "POS")
                           {
                               useOuter = false;
                               string filename_XML = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".xml");
                               string filename_PDF = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".pdf");
                               string filename_FO = "PC_" + con.Ds.Tables[0].Rows[i][1].ToString() + "_" + con.Ds.Tables[0].Rows[i][3].ToString() + "_" + con.Ds.Tables[0].Rows[i][2].ToString().Replace(".xslt", ".fo");
                               //xmlCreator = new WayBillShipmentXmlCreator();
                               string query_step3 = "select a.TruckID,[Date]   from HPIMES.dbo.Forwarder a,HPIMES.dbo.MAWB b where a.MAWB=b.MAWB or a.MAWB=b.HAWB and b.Delivery= '" + BOX_DN.Text.ToString().Trim() + "'";//collate Chinese_Taiwan_Stroke_90_BIN 
                               con.SqlQueComm(query_step3, "Truck");
                               if (con.Ds.Tables["Truck"].Rows.Count < 1)
                               {
                                   Exception ex = new Exception("NO MAWB Or Forwarder Data!");
                                   throw ex;
                                  
                               }

                               xmlCreator = new POSXmlCreator();
                               string TruckInfo = "";
                               TruckInfo = con.Ds.Tables["Truck"].Rows[0][0] + @"@" + con.Ds.Tables["Truck"].Rows[0][1];
                               xmlCreator.LoadData(TruckInfo);
                               string docName_XML = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_XML;
                               xmlCreator.WriteXml(docName_XML, useOuter);
                               string docName_PDF = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_PDF;
                               string docName_FO = ConfigurationManager.AppSettings["FilePath"] + @"\" + filename_FO;
                               string fopPath = ConfigurationManager.AppSettings["FOPPath"];
                               //FOPWrap.FOP.GeneratePDF(
                               //    fopPath,
                               //    docName_XML,
                               //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                               //    docName_PDF);
                               //FOPWrap.FOP.GenerateFo(
                               //    fopPath,
                               //    docName_XML,
                               //    Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()),
                               //    docName_FO);
                               DoGenFile(PDF, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_PDF);
                               DoGenFile(FO, Server.MapPath(@"~\XSL\" + con.Ds.Tables[0].Rows[i][2].ToString().Trim()), docName_XML, docName_FO);
                           }
                          
                             #endregion
                        }
                    }
                    #endregion
                    //Response.Write("<script type='text/javascript'>alert('" + con.Ds.Tables[0].Rows[0][0] + "');</script>");
                }
            }
        }
        catch (System.Exception err)
        {
           // MessageBox.Show(err.Message.ToString());
            logger.Error(err.Message + Environment.NewLine + err.ToString() + Environment.NewLine + err.InnerException);
            Response.Write(err);
        }

    }

    class DataConnLib : System.ComponentModel.Component
    {
        private DataSet ds = new DataSet();
        private string strConn;

        public DataConnLib(string conn)
        {
            this.strConn = conn;
        }

        public int SqlQueSP(string strSPName, string strTabName, System.Collections.ArrayList Paras)
        {
            int trVal = 0;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(this.strConn);
            System.Data.SqlClient.SqlCommand comm = new SqlCommand(strSPName, conn);
            comm.CommandType = CommandType.StoredProcedure;
            System.Data.SqlClient.SqlDataAdapter da = new SqlDataAdapter(comm);
            if (Paras.Count > 0)
            {
                for (int i = 0; i < Paras.Count; i++)
                {
                    comm.Parameters.Add((System.Data.SqlClient.SqlParameter)Paras[i]);
                }
            }
            try
            {
                conn.Open();
                trVal = da.Fill(this.ds, strTabName);
                conn.Close();
                comm.Dispose();
                conn.Dispose();
                da.Dispose();
            }
            catch (System.Data.SqlClient.SqlException err)
            {

            }
            return trVal;
        }

        public int SqlExecSP(string strSPName, System.Collections.ArrayList Paras)
        {
            int trVal = 0;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(this.strConn);
            System.Data.SqlClient.SqlCommand comm = new SqlCommand(strSPName, conn);
            comm.CommandType = CommandType.StoredProcedure;
            if (Paras.Count > 0)
            {
                for (int i = 0; i < Paras.Count; i++)
                {
                    comm.Parameters.Add((System.Data.SqlClient.SqlParameter)Paras[i]);
                }
            }
            try
            {
                conn.Open();
                trVal = comm.ExecuteNonQuery();
                conn.Close();
                comm.Dispose();
                conn.Dispose();

            }
            catch (System.Data.SqlClient.SqlException err)
            {

            }
            return trVal;

        }

        public int SqlQueComm(string strCommName, string strTabName)
        {
            int trVal = 0;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(this.strConn);
            System.Data.SqlClient.SqlCommand comm = new SqlCommand(strCommName, conn);
            comm.CommandType = CommandType.Text;
            System.Data.SqlClient.SqlDataAdapter da = new SqlDataAdapter(comm);
            try
            {
                conn.Open();
                trVal = da.Fill(this.ds, strTabName);
                conn.Close();
                comm.Dispose();
                conn.Dispose();
                da.Dispose();

            }
            catch (System.Data.SqlClient.SqlException err)
            {

            }
            return trVal;

        }

        public int SqlExecComm(string strCommName)
        {
            int trVal = 0;
            System.Data.SqlClient.SqlConnection conn = new SqlConnection(this.strConn);
            System.Data.SqlClient.SqlCommand comm = new SqlCommand(strCommName, conn);
            comm.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                trVal = comm.ExecuteNonQuery();
                conn.Close();
                comm.Dispose();
                conn.Dispose();

            }
            catch (System.Data.SqlClient.SqlException err)
            {

            }
            return trVal;

        }

        public DataSet Ds
        {
            set
            {
                this.ds = value;
            }
            get
            {
                return this.ds;
            }
        }
    }
    protected void BOX_DN_TextChanged(object sender, EventArgs e)
    {

    }
}
