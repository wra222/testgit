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
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using System.Text;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;

public partial class DataMaintain_SmallBoardDefine : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    public string UserId="";
    public String Customer;
    private ISmallBoardDefine iSmallBoardDefine = null;
    private string gFamily = string.Empty;
    private string gMbCode = string.Empty;
    private string gECR = string.Empty;
    private bool gHasHightRow = false;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iSmallBoardDefine = ServiceAgent.getInstance().GetMaintainObjectByName<ISmallBoardDefine>(WebConstant.ISmallBoardDefine);
            UserId = Master.userInfo.UserId;   //
            Customer = Request["Customer"] ?? "HP";
            if (!this.IsPostBack)
            {
                initMBType();
                initFamily();
                initPartNo();
                initLabel();
                //bindTable(null, DEFAULT_ROWS);
                btnFamilyChange_ServerClick(sender, e);
                setColumnWidth();
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
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "InitControl", "initContorls();", true);
    }

    private void initLabel()
    {
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblFamily2.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnAdd.Value = "Add";
        this.lblEcrVersionList.Text = this.GetLocalResourceObject(Pre + "_lblEcrVersionList").ToString();
    }

    private void initFamily()
    {
        try
        {
            this.cmbFamilyTop.Items.Clear();
            this.cmbFamily.Items.Clear();
            this.cmbFamilyTop.Items.Add("ALL");
            this.cmbFamily.Items.Add(string.Empty);
            IList<string> list = iSmallBoardDefine.GetFamilyList(Customer);
            foreach (string item in list)
            {
                ListItem value = new ListItem();
                value.Text = item;
                value.Value = item;
                this.cmbFamilyTop.Items.Add(value);
                this.cmbFamily.Items.Add(value);
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
    }

    private void initMBType()
    {
        try
        {
            this.cmbMBType.Items.Clear();
            this.cmbMBType.Items.Add(string.Empty);
            IList<MBType> list = iSmallBoardDefine.GetMBTypeList();
            foreach (MBType item in list)
            {
                ListItem value = new ListItem();
                value.Text = item.Descr;
                value.Value = item.Value;
                this.cmbMBType.Items.Add(value);
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
    }

    private void initPartNo()
    {
        try
        {
            this.cmbPartNo.Items.Clear();
            IList<string> list = iSmallBoardDefine.GetPartNoList();
            foreach (string item in list)
            {
                ListItem value = new ListItem();
                value.Text = item;
                value.Value = item;
                this.cmbPartNo.Items.Add(value);
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
    }

   
    private void bindTable(IList<SmallBoardDefineInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("ID");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colFamily").ToString());
        dt.Columns.Add("MBType");
        dt.Columns.Add("PartNo");
        dt.Columns.Add("Qty");
        dt.Columns.Add("MaxQty");
        dt.Columns.Add("Priority");
        dt.Columns.Add("ECR");
        dt.Columns.Add("IECVer");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCreateDate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUpdateDate").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (SmallBoardDefineInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ID;
                dr[1] = temp.Family;
                dr[2] = temp.MBType;
                dr[3] = temp.PartNo;
                dr[4] = temp.Qty;
                dr[5] = temp.MaxQty;
                dr[6] = temp.Priority;
                dr[7] = temp.ECR;
                dr[8] = temp.IECVer;
                dr[9] = temp.Editor;
                dr[10] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[11] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
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
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
    }

    protected void gd_DataBound(object sender, EventArgs e)
    {
        if (!gHasHightRow)
        {
            changeSelectedIndex("-1", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }
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

    protected void btnLoadFirstDataList_ServerClick(Object sender,EventArgs e) 
    {
        btnFamilyChange_ServerClick(sender,e);
    }

    protected void btnFamilyChange_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string family = this.cmbFamilyTop.SelectedValue.Trim();
            IList<SmallBoardDefineInfo> lstInfo = iSmallBoardDefine.GetSmallBoardDefineInfo(family);
            bindTable(lstInfo, DEFAULT_ROWS);
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
        string msgHasExistRecord = GetLocalResourceObject(Pre + "_msgHasExistRecord").ToString();
        try
        {
            IList<SmallBoardDefineInfo> lstSmallInfo = null;
            string family = this.hidFamily2.Value;
            string mbType = this.cmbMBType.SelectedItem.Value;
            string partNo = this.cmbPartNo.SelectedItem.Value;
            string qty = this.txtQty.Value.Trim();
            string maxQty = this.txtMaxQty.Value.Trim();
            string priority = this.txtPriority.Value.Trim();
            string ecr = this.txtECR.Value.Trim();
            string iec = this.txtIECVersion.Value.Trim();
            string oldFamily = this.cmbFamilyTop.SelectedValue;
            string id = this.hidDeleteID.Value;

            if (string.IsNullOrEmpty(id))
            {
                showErrorMessage("ID Error");
                return;
            }
            SmallBoardDefineInfo smallDefine = new SmallBoardDefineInfo();
            smallDefine.Family = family;
            smallDefine.MBType = mbType;
            smallDefine.PartNo = partNo;
            smallDefine.Qty = int.Parse(qty);
            smallDefine.MaxQty = int.Parse(maxQty);
            smallDefine.Priority = int.Parse(priority);
            smallDefine.ECR = ecr;
            smallDefine.IECVer = iec;
            smallDefine.Editor = UserId;
            smallDefine.ID = int.Parse(id);
            iSmallBoardDefine.SaveSmallBoardDefineInfo(smallDefine);
            lstSmallInfo = iSmallBoardDefine.GetSmallBoardDefineInfo(oldFamily);
            bindTable(lstSmallInfo, DEFAULT_ROWS);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        finally
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "resetTableHeight", "resetTableHeight();", true);
            hideWait();
        }
    }

    protected void Add_ServerClick(Object sender, EventArgs e)
    {
        string msgHasExistRecord = GetLocalResourceObject(Pre + "_msgHasExistRecord").ToString();
        try
        {
            IList<SmallBoardDefineInfo> lstSmallInfo = null;
            string family = this.hidFamily2.Value;
            string mbType = this.cmbMBType.SelectedItem.Value;
            string partNo = this.cmbPartNo.SelectedItem.Value;
            string qty = this.txtQty.Value.Trim();
            string maxQty = this.txtMaxQty.Value.Trim();
            string priority = this.txtPriority.Value.Trim();
            string ecr = this.txtECR.Value.Trim();
            string iec = this.txtIECVersion.Value.Trim();
            string oldFamily = this.cmbFamilyTop.SelectedValue;
            SmallBoardDefineInfo smallDefine = new SmallBoardDefineInfo();
            smallDefine.Family = family;
            smallDefine.MBType = mbType;
            smallDefine.PartNo = partNo;
            smallDefine.Qty = int.Parse(qty);
            smallDefine.MaxQty = int.Parse(maxQty);
            smallDefine.Priority = int.Parse(priority);
            smallDefine.ECR = ecr;
            smallDefine.IECVer = iec;
            smallDefine.Editor = UserId;
            
            iSmallBoardDefine.AddSmallBoardDefineInfo(smallDefine);
            lstSmallInfo = iSmallBoardDefine.GetSmallBoardDefineInfo(oldFamily);
            bindTable(lstSmallInfo, DEFAULT_ROWS);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        finally
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "resetTableHeight", "resetTableHeight();", true);
            hideWait();
        }
    }

    protected void Delete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            IList<SmallBoardDefineInfo> lstSmallInfo = null;
            string oldFamily = this.cmbFamilyTop.SelectedValue;
            SmallBoardDefineInfo deleteItem = new SmallBoardDefineInfo();
            deleteItem.ID = int.Parse(this.hidDeleteID.Value);
            iSmallBoardDefine.DeleteSmallBoardDefineInfo(deleteItem);
            lstSmallInfo = iSmallBoardDefine.GetSmallBoardDefineInfo(oldFamily);
            bindTable(lstSmallInfo, DEFAULT_ROWS);
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
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");HideWait();");
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

    private void changeSelectedIndex(string index, string family, string mbcode, string ecr, string iecV, string custV)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("selectedRowIndex=" + index + ";");
        scriptBuilder.AppendLine("document.getElementById('" + cmbFamily.ClientID + "').value='';");
        scriptBuilder.AppendLine("document.getElementById('" + cmbMBType.ClientID + "').value='';");
        scriptBuilder.AppendLine("document.getElementById('" + txtQty.ClientID + "').value='';");
        scriptBuilder.AppendLine("document.getElementById('" + txtMaxQty.ClientID + "').value='';");
        scriptBuilder.AppendLine("document.getElementById('" + txtPriority.ClientID + "').value='';");
        if (string.Compare(index, "-1", true) == 0)
        {
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=true;");
        }
        else
        {
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=false;");
        }
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "changeSelectedIndex", scriptBuilder.ToString(), false);
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
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(10);
    }
}
