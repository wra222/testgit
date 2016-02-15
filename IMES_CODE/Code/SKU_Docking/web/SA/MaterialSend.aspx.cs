using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.StationIntf;
using System.IO;
public partial class SA_MaterialSend : System.Web.UI.Page
{
    private const int DEFAULT_ROWS = 15;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    IMaterialRequest imaterialrequest = ServiceAgent.getInstance().GetObjectByName<IMaterialRequest>(WebConstant.SAMBRepairControl);

    protected void Page_Load(object sender, EventArgs e)
    {
       // this.cmdstage.AutoPostBack = true;
        //this.cmdstage.SelectedIndexChanged += new EventHandler(cmdstage_Selected);
        UserId = Master.userInfo.UserId;
        Customer = Master.userInfo.Customer;
        if (!this.IsPostBack)
        {
            string[] stagelist = (Request["StageList"] ?? "SA,FA,PAK").Split(',', '~');
            string query = Request["Query"] ?? "N";
            try
            {
                this.cmdstage.Items.Add("");
                foreach (string stage in stagelist)
                {

                    this.cmdstage.Items.Add(stage);

                }
                if (query == "Y")
                {
                    this.btdel.Disabled = true;
                    this.btnAdd.Disabled = true;
                }
                BindAllGridview(null, 10);

            }

            catch (Exception ee)
            {
                showErrorMessage(ee.Message);
                return;
            }


        }

    }
  
    protected void gd_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < gd.Rows.Count; i++)
        {

            if (gd.Rows[i].Cells[0].Text.Trim().Equals("&nbsp;")) //ID
            {
                gd.Rows[i].Cells[15].Controls[1].Visible = false;
            }
            else
            {
                gd.Rows[i].Cells[15].Controls[1].Visible = true;
            }

        }
    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      //  e.Row.Cells[0].Style.Add("display", "none");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
                //CheckBox chkSelect = e.Row.FindControl("chk") as CheckBox;
                //DataRowView drv = e.Row.DataItem as DataRowView;
                //string id = drv["ID"].ToString();
                //chkSelect.Attributes.Add("onclick", string.Format("setSelectVal(this,'{0}');", id));

            }

        }
    }
    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        return errorMsg;
    }
    protected void cmdstage_Selected(object sender, EventArgs e)
    {
        string stage = this.cmdstage.SelectedValue.Trim();
        this.cmbPdLine.Customer = Customer;
        this.cmbPdLine.Stage = stage;
        this.cmbPdLine.refresh();
        this.UpdatePanel6.Update();
    }
    protected void btupdate_ServerClick(object sender, EventArgs e)
    {
        try
        {

            string id = this.hidDeleteID.Value.ToString();
            if (string.IsNullOrEmpty(id))
            {
                throw new FisException("This ID is not Exists");
            }
            string remark=txtremark.Text.Trim();
            IList<string> inputidList = new List<string>();
            IList<string> erroridlist = new List<string>();
            string[] strList = id.Split(',');
            foreach (string item in strList)
            {
                if (item == "")
                {
                    continue;
                }
                inputidList.Add(item.ToUpper());
            }
            if (inputidList.Count > 0)
            {
                foreach (string deleteid in inputidList)
                {
                    int delID = int.Parse(deleteid);

                    IList<MBRepairControlInfo> queryret = imaterialrequest.GeMaterialRequest(new MBRepairControlInfo { ID = delID });
                    if (queryret.Where(x => x.Status != "Waiting").Any())
                    {
                        erroridlist.Add(delID.ToString());
                        continue;
                    }
                    else
                    {
                        imaterialrequest.UpdateMBRepairControl(new MBRepairControlInfo { ID = delID, Status = "Approve", Remark = remark ,Udt=DateTime.Now});
                    }
                   
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "Delete", "unpdateComplete();", true);
                Query();


            }
            else
            {
                throw new FisException("This ID is not Exists");
            }

         
        }
        catch (FisException fex)
        {

            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {

        }
    }
    protected void btquery_ServerClick(object sender, EventArgs e)
    {
        Query();
            
    }
    protected void Query()
    {
        string stage = cmdstage.SelectedValue.Trim();
        string pdline = cmbPdLine.InnerDropDownList.SelectedValue.Trim();
        string partno = txtpartno.Text.Trim();
        string family = txtfamily.Text.Trim();
        string staus = cmdstatus.SelectedValue.Trim();
        DateTime begin = Convert.ToDateTime(selectfromdate.Value);
        DateTime end = Convert.ToDateTime(selecttodate.Value);
        MBRepairControlInfo condition = new MBRepairControlInfo();
        condition.Stage = stage;
        condition.MaterialType = "Component";
        if (!string.IsNullOrEmpty(pdline))
        {
            condition.Line = pdline;
        }
        if (!string.IsNullOrEmpty(partno))
        {
            condition.PartNo = partno;
        }
        if (!string.IsNullOrEmpty(family))
        {
            condition.Family = family;
        }
        if (!string.IsNullOrEmpty(staus))
        {
            condition.Status = staus;
        }
        MBRepairControlInfo betweencondition = new MBRepairControlInfo { Cdt = DateTime.Now };
        IList<MBRepairControlInfo> ControlInfo = imaterialrequest.GeMaterialRequest(condition, betweencondition, "Cdt", begin, end);
        if (ControlInfo.Count > 0)
        {
            BindAllGridview(ControlInfo, 10);
        }
        else
        {
            showErrorMessage("无数据！");
            BindAllGridview(null, 10);
        }
    }
    protected void BindAllGridview(IList<MBRepairControlInfo> list, int defaultRow)
    {
        if (list != null)
        {
            BindGridview_wait(list.Where(x => x.Status == "Waiting").OrderBy(y => y.Udt).ToList(), defaultRow);
            BindGridview_approve(list.Where(x => x.Status == "Approve").OrderByDescending(y => y.Udt).ToList(), defaultRow);
        }
        else
        {
            BindGridview_wait(null, defaultRow);
            BindGridview_approve(null, defaultRow);
        }

    }
    protected void btnToExcel_ServerClick(object sender, System.EventArgs e)
    {
        DataTable dt = GetNullTable();
        if (gd.Rows.Count>0)
        {
            for (int j = 0; j < gd.Rows.Count; j++)
            {
                DataRow dr = dt.NewRow();

                if (gd.Rows[j].Cells[0].Text.Trim() == "&nbsp;" || string.IsNullOrEmpty(gd.Rows[j].Cells[0].Text.Trim()))
                {
                    break;
                }
                for (int i = 0; i < gd.HeaderRow.Cells.Count-1; i++)
                {
                    dr[i] = gd.Rows[j].Cells[i].Text.Trim();
                }

                dt.Rows.Add(dr);
            }
           
        }
        if (gd2.Rows.Count > 0)
        {
            for (int j = 0; j < gd2.Rows.Count; j++)
            {
                DataRow dr = dt.NewRow();
                if (gd2.Rows[j].Cells[0].Text.Trim() == "&nbsp;" || string.IsNullOrEmpty(gd2.Rows[j].Cells[0].Text.Trim()))
                {
                    break;
                }
                for (int i = 0; i < gd2.HeaderRow.Cells.Count; i++)
                {
                    dr[i] = gd2.Rows[j].Cells[i].Text.Trim();
                }

                dt.Rows.Add(dr);
            }
        
        }
        if (dt.Rows.Count > 0)
        {
            DataTable2Excel(dt);
        }
        else
        {
            showErrorMessage("无数据可导出！");
        }
    }
   
    private void BindGridview_wait(IList<MBRepairControlInfo> list, int defaultRow)
    {

        DataTable dt = GetNullTable();
        DataRow dr = null;
        if (list != null && list.Count != 0)
        {
            foreach (MBRepairControlInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ID;
                dr[1] = temp.PartNo;
                dr[2] = temp.Stage;
                dr[3] = temp.Family;
                dr[4] = temp.PCBModelID;
                dr[5] = temp.Line;
                dr[6] = temp.Location;
                dr[7] = temp.Qty;
                dr[8] = temp.Status;
                dr[9] = temp.Loc;
                dr[10] = temp.AssignUser;
                dr[11] = temp.Remark;
                dr[12] = temp.Editor;
                dr[13] = temp.Cdt;
                dr[14] = temp.Udt;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }


        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }


        }
        this.waitqty.Text = list == null ? "0" : list.Count.ToString();
        gd.DataSource = dt;
        gd.DataBind();
        this.UpdatePanel1.Update();
        setColumnWidth_wait();
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;", true);
    }
    private void BindGridview_approve(IList<MBRepairControlInfo> list, int defaultRow)
    {
        DataTable dt = GetNullTable();
        DataRow dr = null;
        if (list != null && list.Count != 0)
        {
            foreach (MBRepairControlInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ID;
                dr[1] = temp.PartNo;
                dr[2] = temp.Stage;
                dr[3] = temp.Family;
                dr[4] = temp.PCBModelID;
                dr[5] = temp.Line;
                dr[6] = temp.Location;
                dr[7] = temp.Qty;
                dr[8] = temp.Status;
                dr[9] = temp.Loc;
                dr[10] = temp.AssignUser;
                dr[11] = temp.Remark;
                dr[12] = temp.Editor;
                dr[13] = temp.Cdt;
                dr[14] = temp.Udt;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }


        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }


        }
        approveqty.Text = list==null? "0":list.Count.ToString();
        gd2.DataSource = dt;
        gd2.DataBind();
        this.UpdatePanel2.Update();
        setColumnWidth_approve();
       
    }
    private void setColumnWidth_wait()
    {
        this.gd.HeaderRow.Cells[0].Text = "ID";
        this.gd.HeaderRow.Cells[1].Text = "PartNo";
        this.gd.HeaderRow.Cells[2].Text = "Stage";
        this.gd.HeaderRow.Cells[3].Text = "Family";
        this.gd.HeaderRow.Cells[4].Text = "PCBModelID";
        this.gd.HeaderRow.Cells[5].Text = "Line";
        this.gd.HeaderRow.Cells[6].Text = "Location";
        this.gd.HeaderRow.Cells[7].Text = "Qty";
        this.gd.HeaderRow.Cells[8].Text = "Status";
        this.gd.HeaderRow.Cells[9].Text = "Loc";
        this.gd.HeaderRow.Cells[10].Text = "AssignUser";
        this.gd.HeaderRow.Cells[11].Text = "Remark";
        this.gd.HeaderRow.Cells[12].Text = "Editor";
        this.gd.HeaderRow.Cells[13].Text = "Cdt";
        this.gd.HeaderRow.Cells[14].Text = "Udt";
       // this.gd.HeaderRow.Cells[15].Text = "Select";

        gd.HeaderRow.Cells[0].Width = Unit.Parse("30");
        gd.HeaderRow.Cells[1].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[2].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[3].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[4].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[5].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[6].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[7].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[8].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[9].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[10].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[11].Width = Unit.Parse("100");
        gd.HeaderRow.Cells[12].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[13].Width = Unit.Parse("130");
        gd.HeaderRow.Cells[14].Width = Unit.Parse("130");
        gd.HeaderRow.Cells[15].Width = Unit.Parse("30");
    }
    private void setColumnWidth_approve()
    {
        gd2.HeaderRow.Cells[0].Width = Unit.Parse("30");
        gd2.HeaderRow.Cells[1].Width = Unit.Parse("90");
        gd2.HeaderRow.Cells[2].Width = Unit.Parse("50");
        gd2.HeaderRow.Cells[3].Width = Unit.Parse("90");
        gd2.HeaderRow.Cells[4].Width = Unit.Parse("90");
        gd2.HeaderRow.Cells[5].Width = Unit.Parse("50");
        gd2.HeaderRow.Cells[6].Width = Unit.Parse("90");
        gd2.HeaderRow.Cells[7].Width = Unit.Parse("50");
        gd2.HeaderRow.Cells[8].Width = Unit.Parse("50");
        gd2.HeaderRow.Cells[9].Width = Unit.Parse("50");
        gd2.HeaderRow.Cells[10].Width = Unit.Parse("50");
        gd2.HeaderRow.Cells[11].Width = Unit.Parse("100");
        gd2.HeaderRow.Cells[12].Width = Unit.Parse("50");
        gd2.HeaderRow.Cells[13].Width = Unit.Parse("130");
        gd2.HeaderRow.Cells[14].Width = Unit.Parse("130");
    }
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("SetGrDivHeight();ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private void showSuccessMessage(string msg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        msg = replaceSpecialChart(msg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("onSaveSucess('" + msg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private  void DataTable2Excel(System.Data.DataTable dtData)
    {
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        MemoryStream ms = ExcelManager.ExportDataTableToExcel(dtData);
        curContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=MaterialControlReport.xls"));
        curContext.Response.BinaryWrite(ms.ToArray());
        ms.Close();
        ms.Dispose();
    }
    public DataTable GetNullTable()
    {
        DataTable dt = new DataTable();

          dt.Columns.Add("ID");
           dt.Columns.Add("PartNo");
            dt.Columns.Add("Stage");
            dt.Columns.Add("Family");
            dt.Columns.Add("PCBModelID");
            dt.Columns.Add("Line");
            dt.Columns.Add("Location");
            dt.Columns.Add("Qty");
            dt.Columns.Add("Status");
            dt.Columns.Add("Loc");
            dt.Columns.Add("AssignUser");
            dt.Columns.Add("Remark");
            dt.Columns.Add("Editor");
            dt.Columns.Add("Cdt");
            dt.Columns.Add("Udt");
        return dt;

    }
}
