using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;
using System.IO;


public partial class PAK_PackingList : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string filename = "";
    IPackingList iPackingList = ServiceAgent.getInstance().GetObjectByName<IPackingList>(WebConstant.PackingListObject);
    private int fullRowCount = 14;
    public String UserId;
    public String Customer;
    public String PrintType;
    public string Station;
    public ArrayList uploadArray = new ArrayList();
    IShipToCartonLabel iShipToCartonLabel =
                    ServiceAgent.getInstance().GetObjectByName<IShipToCartonLabel>(WebConstant.ShipToCartonLabelObject);


    protected void Page_Load(object sender, EventArgs e)
    {
		if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
        }
		
        if (!Page.IsPostBack)
        {
            Station = Request["Station"];
            PrintType = Request["Type"];
            //pcode = Request["PCode"];
            this.gridview.DataSource = getNullDataTable(fullRowCount);
            this.gridview.DataBind();
            initTableColumnHeader();
            InitLabel();
            this.cmbDocType.Service = "PackingList";

            //IList<string> result = new List<string>();
            //result = iPackingList.WFStart("", Station, UserId, Customer);
            //this.hidSessionKey.Value = result[0].ToString();

            //UserId = Master.userInfo.UserId;
            //Customer = Master.userInfo.Customer;
            //FotTest
            //Station = "PL1";
            this.hidStation.Value = Station;
            this.hidEditor.Value = UserId;
            this.hidCustomer.Value = Customer;
            this.hidPrintType.Value = PrintType;
        }
        IList<string> addr = iShipToCartonLabel.GetEditAddr("OAEditsImage");
        //IList<string> addr = iShipToCartonLabel.GetEditAddr("EditsFISAddr_zh");
        if (addr != null && addr.Count > 0)
        {
            this.addr.Value = addr[0].ToString();
            //this.addr.Value.ToString().Replace('\', );
        }
        else
        {
            this.addr.Value = "";
        }

        //XML
        IList<string> xmlpath = iShipToCartonLabel.GetEditAddr("OAEditsXML");
        if (xmlpath != null && xmlpath.Count > 0)
        {
            this.editsxml.Value = xmlpath[0].ToString();
        }
        else
        {
            this.editsxml.Value = "";
        }

        //Template
        IList<string> temppath = iShipToCartonLabel.GetEditAddr("OAEditsTemplate");
        if (temppath != null && temppath.Count > 0)
        {
            this.editstemp.Value = temppath[0].ToString();
        }
        else
        {
            this.editstemp.Value = "";
        }

        //pdf
        IList<string> pdfpath = iShipToCartonLabel.GetEditAddr("OAEditsPDF");
        if (pdfpath != null && pdfpath.Count > 0)
        {
            this.editspdf.Value = pdfpath[0].ToString();
        }
        else
        {
            this.editspdf.Value = "";
        }
        //editspath
        IList<string> path3 = iShipToCartonLabel.GetEditAddr("OAEditsURL");
        if (path3 != null && path3.Count > 0)
        {
            this.editspath.Value = path3[0].ToString();
        }
        else
        {
            this.editspath.Value = "";
        }
        //fop path
        IList<string> path1 = iShipToCartonLabel.GetEditAddr("FOPFullFileName");
        if (path1 != null && path1.Count > 0)
        {
            this.foppath.Value = path1[0].ToString();
        }
        else
        {
            this.foppath.Value = "";
        }
        //exe path
        IList<string> path2 = iShipToCartonLabel.GetEditAddr("PDFPrintPath");
        if (path2 != null && path2.Count > 0)
        {
            this.exepath.Value = path2[0].ToString();
        }
        else
        {
            this.exepath.Value = "";
        }
    }

    protected void hidbtn_Close(Object sender, EventArgs e)
    {
        //string key = this.hidSessionKey.Value.Trim();
        //iPackingList.WFCancel(key);
    }

    protected void hidbtn_Clear(Object sender, EventArgs e)
    {
        //this.cmbDocType.setSelected(0);
        //this.cmbCarrier.setSelected(0);
        //this.cmbRegion.setSelected(0);
        this.gridview.DataSource = getNullDataTable(fullRowCount);
        this.gridview.DataBind();
        initTableColumnHeader();
        updatePanel1.Update();
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    public void serclear(object sender, System.EventArgs e)
    {
        this.gridview.DataSource = getNullDataTable(fullRowCount);
        this.gridview.DataBind();
        initTableColumnHeader();
        updatePanel1.Update();        
        endWaitingCoverDiv();
    }

    public void serprint(object sender, System.EventArgs e)
    {
        //this.gridview.DataSource = getNullDataTable(fullRowCount);
        //this.gridview.DataBind();
        //initTableColumnHeader();//
        //updatePanel1.Update();

        if (this.radiox1.Checked == true)
        {
            String script = "<script language='javascript'>" + "\r\n" +
            "alert(\"X1\");" + "\r\n" +
            "</script>";
            ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alert1", script, false);
        }
        else
        {
            String script = "<script language='javascript'>" + "\r\n" +
            "alert(\"X2\");" + "\r\n" +
            "</script>";
            ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alert2", script, false);
        }

        endWaitingCoverDiv();
    }

    private class uploadDataInfo
    {
        public string internalID;
        public string xmlName;
        public IList<string> templateList;
        public IList<string> pdfList;
        public IList<string> xsdList;
    }

    private class uploadData
    {
        public string inputData;
        public IList<uploadDataInfo> info;
    };

    public void uploadOver(object sender, System.EventArgs e)
    {
        string errorMsg = "";

        try
        {
            string uploadFile = this.hiddenCostCenter.Value;
            string fullName = Server.MapPath("../") + uploadFile;
            DataTable dt = ExcelManager.getExcelSheetData(fullName);
            int i = 0;
            string DNShipment = "";
            bool bEmptyFile = true;
            string allErrors = string.Empty;
            IList<string> ret = new List<string>();

            this.gridview.DataSource = getNullDataTable(fullRowCount);
            this.gridview.DataBind();
            initTableColumnHeader();
            updatePanel1.Update();

            IList<uploadData> returnList = new List<uploadData>();
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                if (i == 1)
                {
                    continue;
                }
                else if (i == 1002)
                {
                    break;
                }
                else
                {
                    DNShipment = dr[0].ToString();
                    if (DNShipment.Trim() == "")
                    {
                        continue;
                    }
                    else
                    {
                        bEmptyFile = false;
                        bool bExist = false;
                        string doc_type = this.cmbDocType.InnerDropDownList.SelectedValue.ToString();
                        string region = this.cmbRegion.InnerDropDownList.SelectedValue.ToString();
                        string carrier = this.cmbCarrier.InnerDropDownList.SelectedValue.ToString();

                        ArrayList serviceResult = new ArrayList();
                        try
                        {
                            serviceResult = iPackingList.PackingListForOBCheck("", Station, UserId, Customer,
                                                        DNShipment, doc_type, region, carrier, this.hidSessionKey.Value);
                                                        
                        }
                        catch (FisException ee)
                        {
                            allErrors = allErrors + ee.mErrmsg;                            
                            continue;
                        }
                        catch (Exception ex)
                        {
                            allErrors = allErrors + ex.Message;                            
                            continue;
                        }

                        //6.5
                        if (serviceResult[2].ToString() == "DN")
                        {
                            for (int j = 0; j < ret.Count; j++)
                            {
                                if (DNShipment.Substring(0, 10) == ret[j])
                                {
                                    bExist = true;
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < ret.Count; j++)
                            {
                                if (DNShipment == ret[j])
                                {
                                    bExist = true;
                                }
                            }
                        }

                        if (bExist == false)
                        {
                            //6.5
                            if (serviceResult[2].ToString() == "DN")
                            {
                                ret.Add(DNShipment.Substring(0, 10));
                            }
                            else
                            {
                                ret.Add(DNShipment);
                            }
                            this.hidRowCount.Value = ret.Count.ToString();

                            uploadData one = new uploadData();
                            //6.5
                            if (serviceResult[2].ToString() == "DN")
                            {
                                one.inputData = serviceResult[0].ToString().Substring(0, 10);
                            }
                            else
                            {
                                one.inputData = serviceResult[0].ToString();
                            }
                            one.info = new List<uploadDataInfo>();
                            //6.18 BOL
                            if (serviceResult[2].ToString() == "BOL")
                            {
                                ArrayList b1 = new ArrayList();
                                b1 = (ArrayList)serviceResult[1];
                                for (int bo = 0; bo < b1.Count; bo++)
                                {
                                    ArrayList a1 = new ArrayList();
                                    a1 = (ArrayList)b1[bo];
                                    for (int y = 0; y < a1.Count; y++)
                                    {
                                        uploadDataInfo one_info = new uploadDataInfo();
                                        one_info.internalID = (string)(((ArrayList)a1[y])[0]);
                                        one_info.xmlName = (string)(((ArrayList)a1[y])[1]);
                                        one_info.templateList = (IList<string>)(((ArrayList)a1[y])[2]);
                                        one_info.pdfList = (IList<string>)(((ArrayList)a1[y])[3]);
                                        one_info.xsdList = (IList<string>)(((ArrayList)a1[y])[4]);

                                        one.info.Add(one_info);
                                    }
                                }
                                //6.18 BOL
                            }
                            else
                            {
                                ArrayList a1 = new ArrayList();
                                a1 = (ArrayList)serviceResult[1];
                                for (int y = 0; y < a1.Count; y++)
                                {
                                    uploadDataInfo one_info = new uploadDataInfo();
                                    one_info.internalID = (string)(((ArrayList)a1[y])[0]);
                                    one_info.xmlName = (string)(((ArrayList)a1[y])[1]);
                                    one_info.templateList = (IList<string>)(((ArrayList)a1[y])[2]);
                                    one_info.pdfList = (IList<string>)(((ArrayList)a1[y])[3]);
                                    one_info.xsdList = (IList<string>)(((ArrayList)a1[y])[4]);

                                    one.info.Add(one_info);
                                }
                            }
                            returnList.Add(one);
                        }
                    }
                }
            }
            if (bEmptyFile)
            {
                //ITC-1360-1101 少资源
                errorMsg = this.GetLocalResourceObject(Pre + "_mesEmptyFile").ToString();
                writeToAlertMessage(errorMsg);
                endWaitingCoverDiv();             
            }
            else
            {
                if (allErrors != "")
                {
                    string tmp = fullName;
                    File.Delete(fullName);

                    this.gridview.DataSource = getDataTable(returnList.Count, returnList);
                    this.gridview.DataBind();
                    initTableColumnHeader();
                    updatePanel1.Update();
                    writeToAlertMessage(allErrors);
                    endWaitingCoverDiv();
                    FileProcess();
                }
                else
                {
                    string tmp = fullName;
                    File.Delete(fullName);

                    this.gridview.DataSource = getDataTable(returnList.Count, returnList);
                    this.gridview.DataBind();
                    initTableColumnHeader();
                    updatePanel1.Update();
                    FileProcess();
                    FileProcess2();
                }
            }            
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            endWaitingCoverDiv();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            endWaitingCoverDiv();
        }

    }

    private void FileProcess()
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("fileProcess();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "FileProcess", scriptBuilder.ToString(), false);
    }

    private void FileProcess2()
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("fileProcess2();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "FileProcess2", scriptBuilder.ToString(), false);
    }


    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");        
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private DataTable getDataTable(int rowCount, IList<uploadData> returnList)
    {
        DataTable dt = initTable();
        DataRow newRow;
        if (returnList != null && returnList.Count > 0)
        {
            for (int i = 0; i < returnList.Count; i++)
            {
                newRow = dt.NewRow();
                newRow["Item"] = returnList[i].inputData.ToString();
                newRow["hidCol"] = getHideColumn(returnList[i].info);
                dt.Rows.Add(newRow);
            }

            if (returnList.Count < fullRowCount)
            {
                for (int i = returnList.Count; i < fullRowCount; i++)
                {
                    newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                }
            }
        }
        else
        {
            for (int i = 0; i < fullRowCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
        }
        return dt;
    }

    private string getHideColumn(IList<uploadDataInfo> infoList)
    {
        StringBuilder builder = new StringBuilder();
        string seperator = "\u0003";

        builder.Append(infoList.Count.ToString());
        builder.Append(seperator);
        for (int i = 0; i < infoList.Count; i++)
        {
            builder.Append(infoList[i].internalID.ToString());
            builder.Append(seperator);

            builder.Append(infoList[i].xmlName.ToString());

            builder.Append(seperator);
            builder.Append(infoList[i].templateList.Count.ToString());
            builder.Append(seperator);
            //foreach (string s1 in infoList[i].templateList)
            for (int j=0; j<infoList[i].templateList.Count; j++)
            {
                //builder.Append(s1);
                string xlt = infoList[i].templateList[j];
                string xsdName = infoList[i].xsdList[j];
                xlt = xlt + "|||" + xsdName;
                builder.Append(xlt);

                builder.Append(seperator);
                builder.Append(infoList[i].pdfList.Count.ToString());
                builder.Append(seperator);
                foreach (string s2 in infoList[i].pdfList)
                {
                    builder.Append(s2);
                    builder.Append(seperator);
                }
            }            
        }
        builder.Append("END");

        return builder.ToString();
    }



    private DataTable getNullDataTable(int rowCount)
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < rowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["Item"] = String.Empty;
            newRow["hidCol"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Item", Type.GetType("System.String"));
        retTable.Columns.Add("hidCol", Type.GetType("System.String"));
        return retTable;
    }

    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Style.Add("display", "none");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }

    private void initTableColumnHeader()
    {

        this.gridview.HeaderRow.Cells[0].Text = "Item";
        //this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        //this.gridview.Columns[0].ItemStyle.Width = Unit.Pixel(80);
        //this.gridview.Columns[0].ItemStyle.Width = Unit.Pixel(40);
    }

    private void InitLabel()
    {
        this.lbDocType.Text = this.GetLocalResourceObject(Pre + "_lblDocType").ToString();
        this.lbDNShipList.Text = this.GetLocalResourceObject(Pre + "_lblDNShipList").ToString();
        this.lbx1.Text = this.GetLocalResourceObject(Pre + "_lblx1").ToString();
        this.lbx2.Text = this.GetLocalResourceObject(Pre + "_lblx2").ToString();
        this.btPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
        this.lbDNShip.Text = this.GetLocalResourceObject(Pre + "_lblDNShip").ToString();
        this.lbInserFile.Text = this.GetLocalResourceObject(Pre + "_lblInsertFile").ToString();
        //this.btUpload.Value = this.GetLocalResourceObject(Pre + "_btnUpload").ToString();
        this.lbQuery.Text = this.GetLocalResourceObject(Pre + "_lblQuery").ToString();
        this.lbFrom.Text = this.GetLocalResourceObject(Pre + "_lblFrom").ToString();
        this.lbTo.Text = this.GetLocalResourceObject(Pre + "_lblTo").ToString();
        this.lbRegion.Text = this.GetLocalResourceObject(Pre + "_lblRegion").ToString();
        this.lbCarrier.Text = this.GetLocalResourceObject(Pre + "_lblCarrier").ToString();
        this.btQuery.Value = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btClear.Value = this.GetLocalResourceObject(Pre + "_btnClear").ToString();
        //this.btquery.Value = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        
    }
}