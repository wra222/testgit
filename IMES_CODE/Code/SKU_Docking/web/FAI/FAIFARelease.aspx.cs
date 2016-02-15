
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

public partial class FA_FAIFARelease : System.Web.UI.Page
{
    public string[] GvQueryColumnName = { "Model","Family","PlanInputDate","ModelType","FAQty",
                                            "inFAQty","FAState","Editor","Cdt","Udt"};
    public int[] GvQueryColumnNameWidth = { 90,100,70,50,50,
                                             50, 60,90,130,130};
    public string[] Gv2QueryColumnName = { "Model","Department","Status","guid","UploadFileName",
                                             "Comment","Editor","Udt"};
    public int[] Gv2QueryColumnNameWidth = { 100,50,70,20,110,
                                               330,90,160};

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IFAIFARelease iFAIFARelease = ServiceAgent.getInstance().GetObjectByName<IFAIFARelease>(WebConstant.FAIFAReleaseObject);
    private const int DEFAULT_ROWS = 5;
    private const int COL_NUM = 9;
    private const int COL_NUM2 = 8;
    public String UserId;
    public String Customer;
    public String GuidCode;
    public String AccountId;
    public String Login;
    public String Department;
    public String ReUpload;
    public String FileFolder;

    private const string templetFile = "FAIFAReleaseReport.xls";
    private const string outputFile = ".xls";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Department = Request["Department"] ?? "SIE";
            ReUpload = Request["ReUpload"] ?? "N";
            FileFolder = ConfigurationManager.AppSettings["FAIUploadFile"].ToString() == "" ? "D:\\IMES_HP\\UploadFile\\FAI\\" : ConfigurationManager.AppSettings["FAIUploadFile"].ToString();
            this.hidReUpload.Value = ReUpload;
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
            {
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                AccountId = Master.userInfo.AccountId.ToString();
                Login = Master.userInfo.Login;
            }
            if (!this.IsPostBack)
            {
                this.btnApprove.Disabled = true;
                this.btnRemove.Disabled = true;
                //this.btnUpload.Disabled = true;
                this.btnExport.Disabled = true;
                //this.cmbDepartment.Enabled = false;
                initLabel();
                bindTable(null);
                //bindTable2(null);
            }
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
        this.Panel3.GroupingText = "Pilot Mo List:";
    }

    private void showErrorMessage(string errorMsg)
    {
        bindTable(null);
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
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
                dr[0] = temp["Model"];
                dr[1] = temp["Family"];
                DateTime time = Convert.ToDateTime(temp["PlanInputDate"]);
                dr[2] = time.ToString("yyyy-MM-dd");
                dr[3] = temp["ModelType"];
                dr[4] = temp["FAQty"];
                dr[5] = temp["InFAQty"];
                dr[6] = temp["FAState"];
                dr[7] = temp["Editor"];
                time = Convert.ToDateTime(temp["Cdt"]);
                dr[8] = time.ToString("yyyy-MM-dd-HH:mm:ss");
                time = Convert.ToDateTime(temp["Udt"]);
                dr[9] = time.ToString("yyyy-MM-dd-HH:mm:ss");
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

    private void bindTable2(DataTable list)
    {
        DataTable dt = initTable2();
        DataRow dr = null;
        if (list != null && list.Rows.Count != 0)
        {
            foreach (DataRow temp in list.Rows)
            {
                dr = dt.NewRow();
                dr[0] = temp["Model"];
                dr[1] = temp["Department"];
                dr[2] = temp["Status"];
                dr[3] = temp["guid"];
                dr[4] = temp["UploadFileName"];
                dr[5] = temp["Comment"];
                dr[6] = temp["Editor"];
                dr[7] = temp["Udt"].ToString();
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
            dt = getNullDataTable2(DEFAULT_ROWS);
        }
        this.txtComment.Text = "";
        this.updatePanel6.Update();
        gd2.DataSource = dt;
        gd2.DataBind();
        InitGridView();
        this.UpdatePanel2.Update();
    }

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

    private DataTable getNullDataTable2(int j)
    {
        DataTable dt = initTable2();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            for (int k = 0; k < COL_NUM2; k++)
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

    private DataTable initTable2()
    {
        DataTable retTable = new DataTable();
        for (int i = 0; i < Gv2QueryColumnName.Count(); i++)
        {
            retTable.Columns.Add(Gv2QueryColumnName[i], System.Type.GetType("System.String"));
        }
        return retTable;
    }
    
    protected void btnQuery_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string state = this.cmbState.SelectedValue.ToString().Trim();
            string model = this.txtModel.Text.Trim();
            string from = this.selectfromdate.Value.ToString();
            string to = this.selecttodate.Value.ToString();
            DataTable ret = iFAIFARelease.GetFAIModelList(Department,model,from,to,state);
            bindTable(ret);
            this.UpdatePanel1.Update();
            
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "Query", "setNewItemValue();", true);
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

    protected void btnQuerygd2_ServerClick(object sender, EventArgs e)
    {
        try
        {
            bool approve = true;
            bool remove=true;
            string model = this.hidmodel.Value.Trim();
            string faStatus = this.hidfastatus.Value.Trim();
            string filename="";
            string status = "";
            string approvalStatusID = "";
            string approvalItemID = "";
            string isNeedUploadFile = "";
            DataTable ret = iFAIFARelease.GetDepartmentApproveStatus(model);
            foreach (DataRow item in ret.Rows)
            {
                if (item["Department"].ToString().Trim() == Department)
                {
                    isNeedUploadFile = item["IsNeedUploadFile"].ToString().Trim();
                    approvalStatusID = item["ApprovalStatusID"].ToString().Trim();
                    approvalItemID = item["ApprovalItemID"].ToString().Trim();
                    status = item["Status"].ToString().Trim();
                    filename = item["UploadFileName"].ToString().Trim();
                }
            }
            if (status == "Waiting" || status == "Option")
            {//Department
                //faStatus
                if (Department == "OQC" && (faStatus == "Approval" || faStatus == "Pilot"))
                {
                    approve = false;
                }
                else if(Department != "OQC")
                {
                    approve = false;
                }
                
            }
            if (status == "Approved" && (faStatus == "InApproval" || faStatus == "Approval"))
            {
                remove = false;
            }
            if (Department == "OQC" && faStatus == "Release")
            {
                remove = true;
            }
            this.hidfilename.Value = filename;
            this.hidapprovalStatusID.Value = approvalStatusID;
            this.hidapprovalItemID.Value = approvalItemID;
            this.hidIsNeedUploadFile.Value = isNeedUploadFile;
            bindTable2(ret);
            this.UpdatePanel2.Update();
            this.btnApprove.Disabled = approve;
            this.btnRemove.Disabled = remove;
            this.updatePanel5.Update();
            //this.gd.Height = Unit.Parse("150");
            

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
    }

    protected void btnToExcel_ServerClick(object sender, System.EventArgs e)
    {
        DataTable2Excel(GetExcelContents());
    }

    private DataTable GetExcelContents()
    {
        string state = this.cmbState.SelectedValue.ToString().Trim();
        string model = this.txtModel.Text.Trim();
        string from = this.selectfromdate.Value.ToString();
        string to = this.selecttodate.Value.ToString();
        DataTable dt = iFAIFARelease.GetExeclData(model,Department, from, to, state);
        return dt;
    }

    private static void DataTable2Excel(System.Data.DataTable dtData)
    {
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        MemoryStream ms = ExcelManager.ExportDataTableToExcel(dtData);
        curContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=FAIFAReleaseReport.xls"));
        curContext.Response.BinaryWrite(ms.ToArray());
        ms.Close();
        ms.Dispose();
    }

    protected void btnApprove_ServerClick(object sender, EventArgs e)
    {
        try
        {
            
            string model = this.hidmodel.Value;
            string approvalItemID = this.hidapprovalItemID.Value;
            string approvalStatusID = this.hidapprovalStatusID.Value;
            string comment = this.txtComment.Text.Trim();
            string family = this.hidFamily.Value;
            string isNeedUploadFile = this.hidIsNeedUploadFile.Value;
            string failname = this.hidfilename.Value;
            iFAIFARelease.CheckApprovalStatusAndChengeStatus(model, approvalStatusID, approvalItemID, comment, UserId, family, Department, isNeedUploadFile, failname);
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

    protected void btnRemove_ServerClick(object sender, EventArgs e)
    {
        try
        {
            beginWaitingCoverDiv();
            string model = this.hidmodel.Value;
            string approvalStatusID = this.hidapprovalStatusID.Value;
            string comment = this.txtComment.Text.Trim();
            iFAIFARelease.RemoveStatus(model, approvalStatusID, comment, UserId);
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
            
            string temp = this.hidNewFileName.Value;
            this.FileUpload1.ResolveUrl(temp);
            if (this.FileUpload1.HasFile)
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

                iFAIFARelease.InsertUpLoadFileInfo(model, Department, guid, filename, UserId);
                this.FileUpload1.SaveAs(string.Format(path + "{0}~{1}", guid, filename));
                showsuccessMsg();
            }
            
            btnQuerygd2_ServerClick(sender, e);
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
        //e.Row.Cells[1].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }

    protected void gd2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //this.txtComment.Text = "";
        //this.updatePanel6.Update();

        e.Row.Cells[0].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
        e.Row.Cells[3].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM2; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    if (i == 3)
                    {
                        string guid = e.Row.Cells[i].Text.Trim();
                        this.hidoldguid.Value = guid;
                        string filename = e.Row.Cells[4].Text.Trim();
                        this.hidoldfilename.Value = filename;
                        string department = e.Row.Cells[1].Text.Trim();
                        string model = e.Row.Cells[0].Text.Trim();
                        HyperLink link = new HyperLink();
                        link.Text = filename;
                        link.NavigateUrl = string.Format(@"Service/filedownload.ashx?guid={0}&Department={1}&Model={2}&FileFolder={3}", guid, department, model, FileFolder);
                        e.Row.Cells[i + 1].Controls.Add(link);
                    }
                    if (i == 1 && e.Row.Cells[i].Text.Trim() == Department)
                    {
                        string comment = e.Row.Cells[5].Text.Trim();
                        if(e.Row.Cells[5].Text.Trim() == "&nbsp;")
                        {
                            comment = "";
                        }

                        this.txtComment.Text = comment;
                        this.updatePanel6.Update();
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
