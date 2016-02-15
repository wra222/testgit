
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
using IMES.DataModel;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;

public partial class DataMaintain_PartForbidRule : System.Web.UI.Page
{
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    public String UserId;
    public String Customer;
    private IPartForbidRule iPartForbidRule = null;
    
    private bool gHasHightRow = false;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            this.HiddenUserName.Value = UserId;
            iPartForbidRule = ServiceAgent.getInstance().GetMaintainObjectByName<IPartForbidRule>(WebConstant.PartForbidRuleMaintainObject);
            this.cmbCustomer.Load += new EventHandler(cmbCustomer_Load);
            if (!this.IsPostBack)
            {
                //initLabel();
                initCustomer();
                initCategory();
                bindTable(null, DEFAULT_ROWS);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
    }

    private void initLabel()
    {
       
    }

    private void initCustomer()
    {
        IList<string> list = iPartForbidRule.GetCustomer();
        this.cmbCustomerTop.Items.Clear();
        this.cmbCustomer.Items.Clear();
        foreach (string items in list)
        {
            if (items == "")
            {
                continue;
            }
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbCustomerTop.Items.Add(selectListItem);
            this.cmbCustomer.Items.Add(selectListItem);
        }
    }

    private void initCategory()
    {
        IList<string> list = iPartForbidRule.GetCategory();
        this.cmbCategoryTop.Items.Clear();
        this.cmbCategory.Items.Clear();
        this.cmbCategoryTop.Items.Add(string.Empty);
        this.cmbCategory.Items.Add(string.Empty);
        foreach (string items in list)
        {
            if (items == "")
            {
                continue;
            }
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbCategoryTop.Items.Add(selectListItem);
            this.cmbCategory.Items.Add(selectListItem);
        }
    }

    private void cmbCustomer_Load(object sender, System.EventArgs e)
    {
        initline();
    }

    private void initline()
    {
        string customer = this.cmbCustomer.SelectedValue.ToString();
        IList<string> list = iPartForbidRule.GetLine(customer);
        this.cmbLine.Items.Clear();
        this.cmbLine.Items.Add(new ListItem("", ""));
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbLine.Items.Add(selectListItem);
        }
    }

    [System.Web.Services.WebMethod]
    public static ArrayList initline_WEB(string customer,string line)
    {
        IPartForbidRule iPartForbidRule = ServiceAgent.getInstance().GetMaintainObjectByName<IPartForbidRule>(WebConstant.PartForbidRuleMaintainObject);
        ArrayList ret = new ArrayList();
        IList<string> lst = new List<string>();
        if (!string.IsNullOrEmpty(customer))
        {
            lst = iPartForbidRule.GetLine(customer);
        }
        ret.Add(lst);
        ret.Add(line);
        return ret;
    }

    private void bindTable(IList<PartForbidRuleInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("ID");//0
        dt.Columns.Add("Customer");
        dt.Columns.Add("Category");
        dt.Columns.Add("Line");
        dt.Columns.Add("Family");
        dt.Columns.Add("ExceptModel");
        dt.Columns.Add("BomNodeType");
        dt.Columns.Add("VendorCode");
        dt.Columns.Add("PartNo");
        dt.Columns.Add("NoticeMsg");
        dt.Columns.Add("Status");
        dt.Columns.Add("Remark");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");//14

        if (list != null && list.Count != 0)
        {
            foreach (PartForbidRuleInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ID;
                dr[1] = temp.Customer;
                dr[2] = temp.Category;
                dr[3] = temp.Line;
                dr[4] = temp.Family;
                dr[5] = temp.ExceptModel;
                dr[6] = temp.BomNodeType;
                dr[7] = temp.VendorCode;
                dr[8] = temp.PartNo;
                dr[9] = temp.NoticeMsg;
                dr[10] = temp.Status;
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
        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Style.Add("display", "none");
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

    protected void Query_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string customer = this.cmbCustomerTop.SelectedValue.ToString().Trim();
            string category = this.cmbCategoryTop.SelectedValue.ToString().Trim();
            string line = this.txtLineTop.Value.ToString().Trim();
            string family = this.txtFamilyTop.Value.ToString().Trim();
            PartForbidRuleInfo condition = new PartForbidRuleInfo();
            if (customer != "")
            {
                condition.Customer = customer; 
            }
            if (category != "")
            {
                condition.Category = category;
            }
            if (line != "")
            {
                condition.Line = line;
            }
            if (family != "")
            {
                condition.Family = family;
            }
            IList<PartForbidRuleInfo> PartForbidRuleList = iPartForbidRule.GetPartForbidRule(condition);
            bindTable(PartForbidRuleList, DEFAULT_ROWS);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
        finally
        {
            hideWait();
        }
    }

    protected void Save_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            IList<PartForbidRuleInfo> PartForbidRuleList = null;
            string customer = this.cmbCustomer.SelectedValue.ToString();
            string category = this.cmbCategory.SelectedValue.ToString();
            string line = "";// this.cmbLine.SelectedValue.ToString();
            if (this.cmbLine.SelectedValue != "")
            {
                line = this.cmbLine.SelectedValue;
            }
            else if (this.hidLine.Value != "")
            {
                line = this.hidLine.Value;
            }
            string family = this.txtFamily.Value.ToString().Trim();
            string exceptMode = this.txtExceptModel.Value.ToString().Trim();
            string bomnodetype = this.txtBomNodeType.Value.ToString().Trim();
            string vendorcode = this.txtVendorCode.Value.ToString().Trim();
            string partno = this.txtPartNo.Value.ToString().Trim();
            string noticemsg = this.txtNoticeMsg.Value.ToString().Trim();
            string remark = this.txtRemark.Value.ToString().Trim();
            string id = this.hidDeleteID.Value;
            if (string.IsNullOrEmpty(bomnodetype))
            {
                throw new FisException("Please Input BomNodeType...");
            }
            PartForbidRuleInfo PartForbidInfo = new PartForbidRuleInfo();
            PartForbidInfo.Customer = customer;
            PartForbidInfo.Category = category;
            PartForbidInfo.Line = line;
            PartForbidInfo.Family = family;
            PartForbidInfo.ExceptModel = exceptMode;
            PartForbidInfo.BomNodeType = bomnodetype;
            PartForbidInfo.VendorCode = vendorcode;
            PartForbidInfo.PartNo = partno;
            PartForbidInfo.NoticeMsg = noticemsg;
            PartForbidInfo.Status = "Enable";
            PartForbidInfo.Remark = remark;
            PartForbidInfo.Editor = UserId;
            PartForbidInfo.Cdt = DateTime.Now;
            PartForbidInfo.Udt = DateTime.Now;

            if (!string.IsNullOrEmpty(id))
            {
                PartForbidInfo.ID = int.Parse(id);
                iPartForbidRule.UpdatePartForbidRule(PartForbidInfo);
            }
            else
            {
                iPartForbidRule.InsertPartForbidRule(PartForbidInfo);
            }
            PartForbidRuleList = iPartForbidRule.GetPartForbidRule(new PartForbidRuleInfo { Customer = customer });
            long ID = PartForbidRuleList[0].ID;
            bindTable(PartForbidRuleList, DEFAULT_ROWS);
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + ID + "');", true);//
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
        finally
        {
            hideWait();
        }
    }

    protected void Delete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            IList<PartForbidRuleInfo> PartForbidRuleList = null;
            string id = this.hidDeleteID.Value.ToString();
            if (string.IsNullOrEmpty(id))
            {
                throw new FisException("This ID is not Exists");
            }
            int delID = int.Parse(id);
            iPartForbidRule.DeletePartForbidRule(delID);

            PartForbidRuleList = iPartForbidRule.GetPartForbidRule(new PartForbidRuleInfo{Customer = this.cmbCustomerTop.SelectedValue.ToString() });



            bindTable(PartForbidRuleList, DEFAULT_ROWS);
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "Delete", "DeleteComplete();", true);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
        finally
        {
            hideWait();
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void showAlertErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("alert(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "showAlertErrorMessage", scriptBuilder.ToString(), false);
    }

    private void changeSelectedIndex(string index, string family, string vc,string code)
    {
        
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        
        scriptBuilder.AppendLine("HideWait();");
        
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[12].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[13].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[14].Width = Unit.Percentage(10);
    }
    
}
