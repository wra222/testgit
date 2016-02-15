
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Station.Interface.StationIntf;
using System.IO;

public partial class FA_FAIPAKRelease : System.Web.UI.Page
{
    public string[] GvQueryColumnName = { "Model","ModelType","PAKQty","inPAKQty","StartDate",
                                            "State","guid","UploadFile","Editor","Cdt",
                                            "Udt","ApprovalStatusID"};
    public int[] GvQueryColumnNameWidth = { 100,50,50,50,150,
                                              50,50,120,90,150,
                                              150,10};
    //public string[] Gv2QueryColumnName = { "Model","Department","Status","guid","UploadFileName",
    //                                         "Comment","Editor","Udt"};
    //public int[] Gv2QueryColumnNameWidth = { 100,50,70,20,110,
    //                                           330,90,160};

    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";
    private IFAIPAKRelease iFAIPAKRelease = ServiceAgent.getInstance().GetObjectByName<IFAIPAKRelease>(WebConstant.FAIPAKReleaseObject);
    private const int DEFAULT_ROWS = 6;
    private const int COL_NUM = 9;
    private const int COL_NUM2 = 8;
    public String UserId;
    public String Customer;
    public String GuidCode;
    public String AccountId;
    public String Login;
    public String Department;
    public String FileFolder;
    public String errorMsgTime;
    public String errorMsgQty;
    //public String ReUpload;
    public string today;
    //public String rootPath = "d:\\temp\\";

    private const string templetFile = "FAIPAKReleaseReport.xls";
    private const string outputFile = ".xls";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Department = Request["Department"] ?? "OQC";
            FileFolder = ConfigurationManager.AppSettings["FAIUploadFile"].ToString() == "" ? "D:\\IMES_HP\\UploadFile\\FAI\\" : ConfigurationManager.AppSettings["FAIUploadFile"].ToString();
            //ReUpload = Request["ReUpload"] ?? "N";
            //this.hidReUpload.Value = ReUpload;
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
            {
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                AccountId = Master.userInfo.AccountId.ToString();
                Login = Master.userInfo.Login;
            }
            if (!this.IsPostBack)
            {
                today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                this.btnSave.Disabled = true;
                this.btnRelease.Disabled = true;
                //this.btnUpload.Disabled = true;
                //this.btnExport.Disabled = true;
                //this.txtPAKQty.Enabled = false;
                
                bindTable(null);
                //bindTable2(null);
            }
            initLabel();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }
   
    private void initLabel()
    {
        this.Panel3.GroupingText = "FAI PAK Release List:";
        errorMsgTime = GetLocalResourceObject(Pre + "msgTime").ToString();
        errorMsgQty = GetLocalResourceObject(Pre + "msgQty").ToString();
        //注意開始時間不能晚於現在時間！

        //"PAK Qty 不可小於 {0} !!"

    }

    //private void initDepartmentselect(string model)
    //{
    //    IList<string> list = iFAIFARelease.GetDepartmentList(model);
    //    foreach (string item in list)
    //    {
    //        this.cmbDepartment.Items.Add(new ListItem { Text = item, Value = item });
    //    }
    //    if (list.Contains(Department))
    //    {
    //        this.cmbDepartment.SelectedValue = Department;
    //    }
    //    else
    //    {
    //        this.cmbDepartment.SelectedIndex = 0;
    //    }
        
    //    this.updatePanel4.Update();
    //}

    private void showErrorMessage(string errorMsg)
    {
        bindTable(null);
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void bindTable(DataTable list)
    {
        DataTable dt = initTable();
        DataRow dr = null;
        if (list != null && list.Rows.Count != 0)
        {
            foreach (DataRow temp in list.Rows)
            {
                dr = dt.NewRow();
                dr = dt.NewRow();
                dr[0] = temp["Model"];
                dr[1] = temp["ModelType"];
                dr[2] = temp["PAKQty"];
                dr[3] = temp["inPAKQty"];
                DateTime time = Convert.ToDateTime(temp["PAKStartDate"]);
                dr[4] = time.ToString("yyyy-MM-dd HH:mm:ss");
                dr[5] = temp["PAKState"];
                dr[6] = temp["guid"];
                dr[7] = temp["UploadFileName"];
                dr[8] = temp["Editor"];
                time = Convert.ToDateTime(temp["Cdt"]);
                dr[9] = time.ToString("yyyy-MM-dd HH:mm:ss");
                time = Convert.ToDateTime(temp["Udt"]);
                dr[10] = time.ToString("yyyy-MM-dd HH:mm:ss");
                dr[11] = temp["ApprovalStatusID"];
                dt.Rows.Add(dr);
            }

            for (int i = dt.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }
        else
        {
            dt = getNullDataTable(DEFAULT_ROWS);
        }
        gd.DataSource = dt;
        gd.DataBind();
        InitGridView();
        this.UpdatePanel1.Update();
    }

    //private void bindTable2(DataTable list)
    //{
    //    DataTable dt = initTable2();
    //    DataRow dr = null;
    //    if (list != null && list.Rows.Count != 0)
    //    {
    //        foreach (DataRow temp in list.Rows)
    //        {
    //            dr = dt.NewRow();
    //            dr[0] = temp["Model"];
    //            dr[1] = temp["Department"];
    //            dr[2] = temp["Status"];
    //            dr[3] = temp["guid"];
    //            dr[4] = temp["UploadFileName"];
    //            dr[5] = temp["Comment"];
    //            dr[6] = temp["Editor"];
    //            dr[7] = temp["Udt"].ToString();
    //            dt.Rows.Add(dr);

                
    //        }
    //        for (int i = dt.Rows.Count; i < DEFAULT_ROWS; i++)
    //        {
    //            dr = dt.NewRow();
    //            dt.Rows.Add(dr);
    //        }
    //    }
    //    else
    //    {
    //        dt = getNullDataTable2(DEFAULT_ROWS);
    //    }
    //    this.txtComment.Text = "";
    //    this.updatePanel6.Update();
    //    gd2.DataSource = dt;
    //    gd2.DataBind();
    //    InitGridView();
    //    this.UpdatePanel2.Update();
    //}

    private void InitGridView()
    {
        for (int i = 0; i < GvQueryColumnNameWidth.Count(); i++)
        {
            gd.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
        }
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            for (int k = 0; k < COL_NUM; k++)
            {
                newRow[k] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        for (int i = 0; i < GvQueryColumnName.Count(); i++)
        {
            retTable.Columns.Add(GvQueryColumnName[i], System.Type.GetType("System.String"));
        }
        return retTable;
    }

    protected void btnQuery_ServerClick(object sender, EventArgs e)
    {
        
        try
        {
            string state = this.cmbState.SelectedValue.ToString().Trim();
            string model = this.txtModel.Text.Trim();
            DataTable dt = iFAIPAKRelease.GetDepartmentApproveStatus(model, Department, state);
            bindTable(dt);
            this.UpdatePanel1.Update();
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Query", "setNewItemValue();", true);
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    protected void btnToExcel_ServerClick(object sender, System.EventArgs e)
    {
        DataTable2Excel(GetExcelContents());
    }

    private DataTable GetExcelContents()
    {
        DataTable dt = initTable();
        DataRow dr = null;

        for (int i = 0; i < gd.Rows.Count; i++)
        {
            if (!String.IsNullOrEmpty(gd.Rows[i].Cells[0].Text.Trim())
                && !(gd.Rows[i].Cells[0].Text.Trim().ToLower() == "&nbsp;"))
            {
                dr = dt.NewRow();
                dr[0] = gd.Rows[i].Cells[0].Text.Trim();
                dr[1] = gd.Rows[i].Cells[1].Text.Trim();
                dr[2] = gd.Rows[i].Cells[2].Text.Trim();
                dr[3] = gd.Rows[i].Cells[3].Text.Trim();
                dr[4] = gd.Rows[i].Cells[4].Text.Trim();
                dr[5] = gd.Rows[i].Cells[5].Text.Trim();
                dr[6] = gd.Rows[i].Cells[6].Text.Trim();
                dr[7] = gd.Rows[i].Cells[7].Text.Trim();
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    private static void DataTable2Excel(System.Data.DataTable dtData)
    {
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        MemoryStream ms = ExcelManager.ExportDataTableToExcel(dtData);
        curContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=FAIPAKReleaseReport.xls"));
        curContext.Response.BinaryWrite(ms.ToArray());
        ms.Close();
        ms.Dispose();
    }

    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        try
        {

            
            string model = this.hidmodel.Value;
            string approvalStatusID = this.hidapprovalStatusID.Value;
            int pakQtyInput = Convert.ToInt32(this.txtPAKQty.Text.Trim());
            int pakQtyOld = Convert.ToInt32(this.hidPAKQty.Value);
            string shipdate = this.hidStartDate.Value;
            DateTime now = DateTime.Now;
            DateTime time = Convert.ToDateTime(shipdate);
            if (this.hidOldStartDate.Value != this.hidStartDate.Value)
            {
                if (now > time)
                {
                    throw new FisException(errorMsgTime);
                }
            }

            if (pakQtyInput < pakQtyOld)
            {
                throw new FisException(errorMsgQty + " " + pakQtyOld);
            }
            iFAIPAKRelease.SaveStatus(model, approvalStatusID, pakQtyInput, shipdate, UserId);
            btnQuery_ServerClick(sender, e);
            showsuccessMsg();
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    protected void btnRelease_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string model = this.hidmodel.Value;
            string approvalStatusID = this.hidapprovalStatusID.Value;
            //string comment = this.txtPAKQty.Text.Trim();
            iFAIPAKRelease.ReleaseStatus(model, approvalStatusID, UserId);
            btnQuery_ServerClick(sender, e);
            showsuccessMsg();
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string filename = "";
        string path = "";
        string guid = "";
        string oldGUID = "";
        string oldFileName = "";
        try
        {
            beginWaitingCoverDiv();
            if (FileUpload1.HasFile)
            {
                filename = this.FileUpload1.FileName;
                guid = Guid.NewGuid().ToString();
                oldGUID = this.hidoldguid.Value;
                oldFileName = this.hidoldfilename.Value;
                string model = this.hidmodel.Value;
                path = FileFolder + Department + "\\" + model + "\\";
                //存至Disk
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //檢查是否存在

                if (System.IO.File.Exists(string.Format(path + "{0}~{1}", oldGUID, oldFileName)))
                {
                    System.IO.File.Delete(string.Format(path + "{0}~{1}", oldGUID, oldFileName));
                }

                iFAIPAKRelease.InsertUpLoadFileInfo(model, Department, guid, filename, UserId);
                this.FileUpload1.SaveAs(string.Format(path + "{0}~{1}", guid, filename));
            }
            //this.gd2.DataBind();
            btnQuery_ServerClick(sender, e);
        }
        catch (FisException fex)
        {
            System.IO.File.Delete(string.Format(path + "{0}~{1}", guid, filename));
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            System.IO.File.Delete(string.Format(path + "{0}~{1}", guid, filename));
            showErrorMessage(ex.Message);
            return;
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[6].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
        e.Row.Cells[11].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    if (i == 6)
                    {
                        string guid = e.Row.Cells[i].Text.Trim();
                        this.hidoldguid.Value = guid;
                        string filename = e.Row.Cells[7].Text.Trim();
                        this.hidoldfilename.Value = filename;
                        string department = Department;//e.Row.Cells[5].Text.Trim();
                        string model = e.Row.Cells[0].Text.Trim();
                        HyperLink link = new HyperLink();
                        link.Text = filename;
                        link.NavigateUrl = string.Format(@"Service/filedownload.ashx?guid={0}&Department={1}&Model={2}&FileFolder={3}", guid, department, model, FileFolder);
                        e.Row.Cells[i + 1].Controls.Add(link);
                    }
                    if (i == 1 && e.Row.Cells[i].Text.Trim() == Department)
                    {
                        this.txtPAKQty.Text = e.Row.Cells[2].Text.Trim();
                        this.updatePanel4.Update();
                    }
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;

                    
                }
            }
        }
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updHidden, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updHidden, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }

    private void showsuccessMsg()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("showsuccessMsg();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updHidden, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }
}
