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


public partial class PAK_ScaningList : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ScanningList iScanningList = ServiceAgent.getInstance().GetObjectByName<ScanningList>(WebConstant.ScanningListObject);

    private int fullRowCount = 14;
    public string filename = "";
    public String UserId;
    public String Customer;
    public String Station;

    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (!Page.IsPostBack)
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            this.gridview.DataSource = getNullDataTable(fullRowCount);
            this.gridview.DataBind();
            Station =Request["Station"];
            initTableColumnHeader();
            InitLabel();
        }        
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }

    private class uploadDataInfo
    {
        public string path;
        public string doc_type;
        public IList<string> xmlPara;
        public IList<string> pdfPara;
    }

    private class uploadData
    {
        public string inputData;
        public IList<uploadDataInfo> info;
    }


    public void uploadOver(object sender, System.EventArgs e)
    {
        string errorMsg = "";
        DataTable dt=null;
        string Succ_info = "Upload Success!";
        try
        {
            string uploadFile =  this.hiddenCostCenter.Value;
            string fullName = Server.MapPath("../") + uploadFile;

            dt = ExcelManager.getExcelSheetData(fullName);
            int i = 0;
            string DNShipment = "";
            bool bEmptyFile = true;
            string allErrors = string.Empty;
            IList<string> ret = new List<string>();

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
                        ArrayList serviceResult = new ArrayList();
                        string doc_type = this.CmbDocType1.InnerDropDownList.SelectedValue.ToString();
                        bEmptyFile = false;
                        bool bExist = false;
                        try
                        {
                            serviceResult =  iScanningList.ScanningListForCheck("", Station, UserId, Customer,DNShipment, doc_type);
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
                        DNShipment = (string)serviceResult[5];
                        for (int j = 0; j < ret.Count; j++)
                        {
                            if (DNShipment == ret[j])
                            {
                                bExist = true;
                            }
                        }
                        if (bExist == false)
                        {
                            ret.Add(DNShipment);
                            this.hidRowCount.Value = ret.Count.ToString();

                            uploadData one = new uploadData();
                            one.inputData = serviceResult[0].ToString();
                            one.info = new List<uploadDataInfo>();

                            ArrayList a1 = new ArrayList();
                            //a1 = (ArrayList)serviceResult[1];
                            //for (int y = 0; y < a1.Count; y++)
                            {
                                one.inputData = DNShipment;
                                uploadDataInfo one_info = new uploadDataInfo();
                                one_info.doc_type = (string)serviceResult[3];
                                one_info.path = (string)serviceResult[2];
                                one_info.xmlPara = (IList<string>)((IList<string>)serviceResult[0]);
                                one_info.pdfPara = (IList<string>)((IList<string>)serviceResult[1]);

                                one.info.Add(one_info);
                            }
                            returnList.Add(one);
                        }
                       
                    }
                }
            }
            if (bEmptyFile)
            {
                errorMsg = this.GetLocalResourceObject(Pre + "_mesEmptyFile").ToString();

                endWaitingCoverDiv();
                writeToAlertMessage(errorMsg);
               
            }
            else
            {
                if (allErrors != "")
                {
                    writeToAlertMessage(allErrors);
                    endWaitingCoverDiv();
                    FileProcess();
                }
                /*else
                {
                    writeToAlertMessage(Succ_info);
                }*/
                        string tmp = fullName;
                    File.Delete(fullName);

                    //this.gridview.DataSource = getDataTable(ret.Count, ret);
                    this.gridview.DataSource = getDataTable(returnList.Count, returnList);

                    this.gridview.DataBind();
                    initTableColumnHeader();
                    updatePanel1.Update();
                    FileProcess();
                    FileProcess2();
               
            }

        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.Message);
            endWaitingCoverDiv();
        }
        catch (Exception ex)
        {
            if (dt == null)
            {
                errorMsg = this.GetLocalResourceObject(Pre + "_mesEmptyFile").ToString();
                writeToAlertMessage(errorMsg);
            }
            else
            {
                writeToAlertMessage(ex.Message);
            }
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
    private void clearMessage()
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\""  + "\");");      
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "clearMessage", scriptBuilder.ToString(), false);
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
        if (returnList != null && returnList.Count != 0)
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
            builder.Append(infoList[i].doc_type.ToString());
            builder.Append(seperator);
            builder.Append(infoList[i].path.ToString());
            builder.Append(seperator);
            // builder.Append(infoList[i].xmlPara.
            // builder.Append(seperator);
            foreach (string s1 in infoList[i].xmlPara)
            {
                builder.Append(s1);
                builder.Append(seperator);
            }

            foreach (string s1 in infoList[i].pdfPara)
            {
                builder.Append(s1);
                builder.Append(seperator);
            }

        }
        builder.Append("END");

        return builder.ToString();
    }



    public void serclear(object sender, System.EventArgs e)
    {
        this.gridview.DataSource = getNullDataTable(fullRowCount);
        this.gridview.DataBind();
        initTableColumnHeader();
        updatePanel1.Update();
        clearMessage();
        endWaitingCoverDiv();
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

        //this.gridview.HeaderRow.Cells[0].Text = "Item";
             
    }

    private void InitLabel()
    {
        this.lbDocType.Text = this.GetLocalResourceObject(Pre + "_lblDocType").ToString();
        this.lbDNShipList.Text = this.GetLocalResourceObject(Pre + "_lblDNShipList").ToString();
        this.btPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
        this.lbDNShip.Text = this.GetLocalResourceObject(Pre + "_lblDNShip").ToString();
        this.lbInserFile.Text = this.GetLocalResourceObject(Pre + "_lblInsertFile").ToString();

        this.btClear.Value = this.GetLocalResourceObject(Pre + "_btnClear").ToString();
        
    }
}