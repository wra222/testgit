using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Runtime.InteropServices;
using System.Diagnostics;

using Inventec.HPEDITS.XmlCreator;
using Inventec.HPEDITS.PDFCreator;
using Inventec.HPEDITS.XmlCreator.Database;
using System.Text;

public partial class _Default : System.Web.UI.Page 
{
    const string STPROC = "sp_ReprintProcess";
    string connectionString = ConfigurationManager.AppSettings["Database"];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //load the condition table
            /*
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //string queryString = "SELECT * FROM [PAK.PAKCT]";
                string queryString = "SELECT * FROM [CT_EDITS_DOC_SET_ID_20071220] Where DOC_SET_NUMBER!='ERROR'";
                using (SqlCommand command = new SqlCommand(queryString, conn))
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet conditionSet = new DataSet();
                        adapter.Fill(conditionSet);
                        conditionTable.DataSource = conditionSet;
                        conditionTable.DataBind();                        
                        btnPrintPDF.Visible = idPromptControl.Visible = false;
                        Session.Clear();
                    }
                    conn.Close();
                }
            }
             */
            BindConditionTable();
        }
    }

    protected void BindConditionTable()
    {
        if(gvHPComn.Rows.Count == 0)
            return;

        string REGION = ((Label)gvHPComn.Rows[0].FindControl("lblREGION")).Text;
        string ORDER_TYPE = ((Label)gvHPComn.Rows[0].FindControl("lblORDER_TYPE")).Text;
        string SALES_CHAN = ((Label)gvHPComn.Rows[0].FindControl("lblSALES_CHAN")).Text;
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(REGION, "REGION"));
        vchSet.Append(Method.BuildXML(ORDER_TYPE, "ORDER_TYPE"));
        vchSet.Append(Method.BuildXML(SALES_CHAN, "SALES_CHAN"));
        string sqlCmd = Method.GetSqlCmd(STPROC, "Select", "PAK.PAKCT", vchSet.ToString());
        DataSet conditionSet = DAO.sqlCmdDataSetSP(Constant.S_EDITSConnStr, sqlCmd);
        conditionTable.DataSource = conditionSet;
        conditionTable.DataBind();
        btnPrintPDF.Visible = false;
        btnViewXML.Visible = false;
        btnViewPdf.Visible = false;
        Session.Clear();
    }
    
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        gvHPComn.DataBind();
        BindConditionTable();
    }

    protected void conditionTable_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Show")
        {            
            //show data in reference table
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryString = "SELECT * FROM [PAK.PAKRT_2D] WHERE DOC_SET_NUMBER='" + e.CommandArgument.ToString() + "'";
                queryString += " AND ISNULL(XSL_TEMPLATE_NAME,'')!=''";
                using (SqlDataAdapter adapter = new SqlDataAdapter(queryString, conn))
                {
                    conn.Open();
                    DataSet referenceSet = new DataSet();
                    adapter.Fill(referenceSet);
                    referenceTable.DataSource = referenceSet;
                    referenceTable.DataBind();
                    referenceTable.Visible = true;
                    btnPrintPDF.Visible = false;
                    btnViewPdf.Visible = false;
                    lblDocNum.Text = e.CommandArgument.ToString();
                    conn.Close();
                    Session.Clear();
                }
            }
        }
    }
    protected void referenceTable_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Print")
        {
            //get the xslstring
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryString = "SELECT * FROM [PAK.PAKRT_2D] WHERE ID= " + e.CommandArgument.ToString();
                               
                using (SqlCommand comm = new SqlCommand(queryString, conn))
                {
                    conn.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        string xslPath = reader["XSL_TEMPLATE_NAME"].ToString();
                        string schema = reader["SCHEMA_FILE_NAME"].ToString();
                        string docCat = reader["DOC_CAT"].ToString();
                        //btnPrintPDF.Visible = true;
                        //btnViewXML.Visible = true;cm

                        if (docCat.StartsWith("Pallet Ship Label"))// || docCat.StartsWith("Box Ship Label"))
                        {
                            StringBuilder vchSet = new StringBuilder();
                            vchSet.Append(Method.BuildXML(docCat.Trim(), "CONDITION"));
                            vchSet.Append(Method.BuildXML(txtInternalID.Text.Trim(),"InternalID"));
                            string sqlCmd = Method.GetSqlCmd(STPROC, "SELECT", "PRINTID", vchSet.ToString());
                            string[] ZPC = DAO.sqlCmdArrSingleCol(Constant.S_EDITSConnStr, sqlCmd);
                            if (ZPC[0] == "CARTON")
                            {
                                gvBox.Visible = false;
                                btnPrintPDF.Visible = false;
                                btnViewXML.Visible = false;
                                btnViewPdf.Visible = false;
                                Response.Write("<script type='text/javascript'>alert('PackingType is not Parcell');</script>");
                                return;
                                
                            }
                            else
                            {
                                lblQstring.Text = sqlCmd.ToString().Trim();
                                BindData(0, lblQstring.Text.Trim());
                                gvBox.Visible = true;
                                btnPrintPDF.Visible = false;
                                btnViewXML.Visible = false;
                                btnViewPdf.Visible = false;
                            }
                        }
                        else if (docCat.StartsWith("Box Ship Label"))
                        {
                            StringBuilder vchSet = new StringBuilder();
                            vchSet.Append(Method.BuildXML(docCat.Trim(), "CONDITION"));
                            vchSet.Append(Method.BuildXML(txtInternalID.Text.Trim(),"InternalID"));
                            string sqlCmd = Method.GetSqlCmd(STPROC, "SELECT", "PRINTID", vchSet.ToString());
                            lblQstring.Text = sqlCmd.ToString().Trim();
                            BindData(0, lblQstring.Text.Trim());
                            gvBox.Visible = true;
                            btnPrintPDF.Visible = false;
                            btnViewXML.Visible = false;
                            btnViewPdf.Visible = false;
                        }
                        else
                        {
                            btnPrintPDF.Visible = true;
                            btnViewXML.Visible = true;
                            btnViewPdf.Visible = true;
                            gvBox.Visible = false;
                            
                        }
                        Session.Add("XSL", xslPath);
                        Session.Add("DOC", docCat);
                        Session.Add("SCHEMA_FILE_NAME", schema);
                    }
                    conn.Close();
                }
            }
         }
    }

    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        string pdfPath = OutputToPDF("PrintPDF");
        SetDefaultPrinter("Zebra  105SL (200dpi)");
        ProcessStartInfo startPrint = new ProcessStartInfo(pdfPath);
        startPrint.Verb = "print";
        startPrint.WindowStyle = ProcessWindowStyle.Hidden;
        Process printProcess = Process.Start(startPrint);        
        
    }

    protected void btnViewXML_Click(object sender, EventArgs e)
    {
        string sessionDoc = Session["SCHEMA_FILE_NAME"].ToString();
        XmlCreator xmlCreator = null;
        Guid docGuid = Guid.NewGuid();
        bool useOuter = true;
        if (sessionDoc.StartsWith("Schema_Box_Ship_Label"))
        {
            xmlCreator = new BoxLabelXmlCreator();
            xmlCreator.LoadData(txtInternalID.Text.Trim() + "/" + lblselect.Text.ToString());
            //xmlCreator.LoadData(txtInternalID.Text.Trim());
        }
        else if (sessionDoc.StartsWith("Schema_Pallet_Label_TypeA")) 
        {
            xmlCreator = new PalletAXmlCreator();
            xmlCreator.LoadData(txtInternalID.Text.Trim() + "/" + lblselect.Text.ToString());
        }
        else if (sessionDoc.StartsWith("Schema_Pallet_Label_TypeB"))
        {
            xmlCreator = new PalletBXmlCreator();
            xmlCreator.LoadData(txtInternalID.Text.Trim() + "/" + lblselect.Text.ToString());
        }
        else if (sessionDoc.StartsWith("Schema_Pack_List"))
        {
            useOuter = false;
            xmlCreator = new PackListXmlCreator();
            xmlCreator.LoadData(txtInternalID.Text.Trim());
        }
        else if (sessionDoc.StartsWith("Schema_Waybill"))
        {
            //xmlCreator = new WayBillXmlCreator();
            xmlCreator = new WayBill_AdditionXMLCreat_new();
            xmlCreator.LoadData(txtInternalID.Text.Trim());
        }
       /*if (!sessionDoc.StartsWith("Pallet Ship Label"))
        {
            string docType = Session["DOC"].ToString().Split("-".ToCharArray())[0];
            switch (docType)
            {
                case "Box Ship Label":
                    xmlCreator = new BoxLabelXmlCreator();
                    break;
                case "Pack List":
                    useOuter = false;
                    xmlCreator = new PackListXmlCreator();                                       
                    break;
                case "Waybill":
                    xmlCreator = new WayBillXmlCreator();
                    break;
            }
        }
        else
        {
            if (sessionDoc == "Pallet Ship Label- Pack ID Single")
            {
                xmlCreator = new PalletAXmlCreator();
            }
            else
            {
                xmlCreator = new PalletBXmlCreator();
            }
        }*/
        //xmlCreator.LoadData(txtInternalID.Text.Trim());
        string docName = ConfigurationManager.AppSettings["FilePath"] + @"\" + docGuid.ToString() + ".xml";
        //string docName = Server.MapPath(@"~\Output\" + docGuid.ToString() + ".xml");
        xmlCreator.WriteXml(docName, useOuter);

        string url = ConfigurationManager.AppSettings["FileSite"] +  docGuid.ToString() + ".xml";
        //Response.Redirect(@"~\Output\" + docGuid.ToString() + ".xml");
        OpenWindows(url,"XML");
    }

    protected void gvBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblselect.Text = gvBox.SelectedRow.Cells[1].Text.ToString().Trim();
        btnPrintPDF.Visible = true;
        btnViewXML.Visible = true;
        btnViewPdf.Visible = true;
    }
   
    protected void gvBox_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBox.EditIndex = -1;
        BindData(e.NewPageIndex, lblQstring.Text.Trim());
    }
    private void BindData(int PageExpression, string strsql)
    {
        DataSet conditionSet = DAO.sqlCmdDataSetSP(Constant.S_EDITSConnStr, strsql);
        gvBox.DataSource = conditionSet;
        gvBox.PageIndex = PageExpression;
        gvBox.DataBind();
    }

    [DllImport("winspool.drv")]
    private static extern void SetDefaultPrinter(string printerName);

    string OutputToPDF(string cmd)
    {
        string sessionDoc = Session["SCHEMA_FILE_NAME"].ToString();
        XmlCreator xmlCreator = null;
        Guid docGuid = Guid.NewGuid();
        bool useOuter = true;
        if (sessionDoc.StartsWith("Schema_Box_Ship_Label"))
        {
            //xmlCreator = new BoxLabelXmlCreator();
            xmlCreator.LoadData(txtInternalID.Text.Trim());
            xmlCreator.LoadData(txtInternalID.Text.Trim() + "/" + lblselect.Text.ToString());
        }
        else if (sessionDoc.StartsWith("Schema_Pallet_Label_TypeA"))
        {
            xmlCreator = new PalletAXmlCreator();
            xmlCreator.LoadData(txtInternalID.Text.Trim() + "/" + lblselect.Text.ToString());
        }
        else if (sessionDoc.StartsWith("Schema_Pallet_Label_TypeB"))
        {
            xmlCreator = new PalletBXmlCreator();
            xmlCreator.LoadData(txtInternalID.Text.Trim() + "/" + lblselect.Text.ToString());
        }
        else if (sessionDoc.StartsWith("Schema_Pack_List"))
        {
            useOuter = false;
            xmlCreator = new PackListXmlCreator();
            xmlCreator.LoadData(txtInternalID.Text.Trim());
        }
        else if (sessionDoc.StartsWith("Schema_Waybill"))
        {
            //xmlCreator = new WayBillXmlCreator();
            xmlCreator = new WayBill_AdditionXMLCreat_new();
            xmlCreator.LoadData(txtInternalID.Text.Trim());
        }
        string docName = ConfigurationManager.AppSettings["FilePath"] + @"\" + docGuid.ToString() + ".xml";
        //string docName = Server.MapPath(@"~\Output\" + docGuid.ToString() + ".xml");
        xmlCreator.WriteXml(docName, useOuter);

        string fopPath = ConfigurationManager.AppSettings["FOPPath"];
        FOPWrap.FOP.GeneratePDF(
            fopPath,
            docName,
            Server.MapPath(@"~\XSL\" + Session["XSL"].ToString()),
            ConfigurationManager.AppSettings["FilePath"] + @"\" + docGuid.ToString()+".pdf");
            //Server.MapPath(@"~\Output\" + docGuid.ToString() + ".pdf"));
            //return @"~\Output\" + docGuid.ToString() + ".pdf";
         if (cmd=="ViewPDF")
               return ConfigurationManager.AppSettings["FileSite"]  + docGuid.ToString() + ".pdf";
         else 
               return ConfigurationManager.AppSettings["FilePath"] + @"\" + docGuid.ToString() +".pdf";
        
        
    }

    protected void btnViewPdf_Click(object sender, EventArgs e)
    {
        string pdfPath = OutputToPDF("ViewPDF");
        OpenWindows(pdfPath,"PDF");
        //Response.Redirect(pdfPath);
    }

    protected void OpenWindows(string url,string form)
    {
        string sScript="";
        if (form=="PDF")
            sScript = @"<script language='javascript'>popUp=window.open('" + url + "', 'ViewPDF','height=650,width=800,toolbar=no,locatiion=yes,menubar=yes,scrollbars=yes,resizable=yes,left=50,top=0')</script>";
        else
            sScript = @"<script language='javascript'>popUp=window.open('" + url + "', 'ViewXML','height=650,width=800,toolbar=no,locatiion=yes,menubar=yes,scrollbars=yes,resizable=yes,left=0,top=0')</script>";
        ClientScript.RegisterStartupScript(Page.GetType(), "", sScript);
    }
}
