using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;

using System.Data;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;

public partial class DataMaintain_PLTStandard : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 31;
    private IPLTStandard iPLTStandard;
    //private IRepairInfoMaintain iSelectData;
            
            
    private const int COL_NUM = 8;//7;

    
    public  string MsgSelectOne = "";
    public  string MsgDelConfirm = "";
    public string msgLength = "";
    public string msgWidth = "";
    public string msgHeight = "";
    public string MsgNotFound = "";
    public string msgPalletNoSel = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        iPLTStandard = (IPLTStandard)ServiceAgent.getInstance().GetMaintainObjectByName<IACAdaptor>(WebConstant.MaintainPLTStandardObject);
        this.ddlPalletSpec.InnerDropDownList.SelectedIndexChanged  += new EventHandler(cmbMaintainPalletSpec_Load);
        //this.ddlPalletSpec.InnerDropDownList.AutoPostBack = true;

        if(!IsPostBack)
        {
            
            MsgSelectOne = this.GetLocalResourceObject(Pre + "_MsgSelectOne").ToString();
            MsgDelConfirm = this.GetLocalResourceObject(Pre + "_MsgDelConfirm").ToString();
            msgPalletNoSel = this.GetLocalResourceObject(Pre + "_msgPalletNoSel").ToString();
            msgLength = this.GetLocalResourceObject(Pre + "_msgLength").ToString();
            msgWidth = this.GetLocalResourceObject(Pre + "_msgWidth").ToString();
            msgHeight = this.GetLocalResourceObject(Pre + "_msgHeight").ToString();
            MsgNotFound = this.GetLocalResourceObject(Pre + "_MsgNotFound").ToString();
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            List<PltstandardInfo> PltstandardLst = null;
            initLabel();
            try
            {
                PltstandardLst = (List<PltstandardInfo>)iPLTStandard.GetPLTStandardList();
            }
            catch (FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
                return;
            }
            catch(Exception ee)
            {
                showErrorMessage(ee.Message);
                return;
            }

            bindTable(PltstandardLst, DEFAULT_ROWS);
        }

    }
    private void cmbMaintainPalletSpec_Load(object sender, System.EventArgs e)
    {
        //if (!this.ddlPalletSpec.IsPostBack)
        {
            var Length = "";
            var Width = "";
            var Height = "";
            String[] strAcct = this.ddlPalletSpec.InnerDropDownList.SelectedValue.Split(("|").ToCharArray());
            if (strAcct.Length > 1)
            {
                Length = strAcct[0];
                Width = strAcct[1];
                Height = strAcct[2];
            }
            if ((Length != "") && (float.Parse(Length) != 0.0))
            {
                this.TextLength.Text = Length;
                this.TextLength.Enabled = false;
            }
            else
            {
                this.TextLength.Text = "";
                this.TextLength.Enabled = true;
            }
            if ((Width != "") && (float.Parse(Width) != 0.0))
            {
                this.TextWidth.Text = Width;
                this.TextWidth.Enabled = false;
            }
            else
            {
                this.TextWidth.Text = "";
                this.TextWidth.Enabled = true;
            }
            if ((Height != "") && (float.Parse(Height) != 0.0))
            {
                this.TextHeight.Text = Height;
                this.TextHeight.Enabled = false;
            }
            else
            {
                this.TextHeight.Text = "";
                this.TextHeight.Enabled = true;
            }
            if ((this.TextWidth.Text == "") && (this.TextWidth.Text == "") && (this.TextHeight.Text == ""))
                this.TextPalletNO.Focus();
            this.updatePanelEdit.Update();
            //ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "SetTextBox('" + Length + "','" + Width + "','" + Height + "');", true);

        }

    }
      

    protected void btnAdd_ServerClick(object sender,EventArgs e)
    {
       
        PltstandardInfo PltStd = new PltstandardInfo();
        PltStd.pltno = this.TextPalletNO.Text.Trim();
        //PltStd.len = decimal.Parse(this.TextLength.Text);
        //PltStd.wide = decimal.Parse(this.TextWidth.Text);
        //PltStd.high = decimal.Parse(this.TextHeight.Text);
        PltStd.len = decimal.Parse(this.HiddenLenght.Value);
        PltStd.wide = decimal.Parse(this.HiddenWidth.Value);
        PltStd.high = decimal.Parse(this.HiddenHeight.Value);
        
        //PltStd.cause = this.ddlCause.InnerDropDownList.SelectedValue;
        PltStd.editor = this.HiddenUserName.Value.Trim();

        string itemId="";
        try 
        {
            itemId = iPLTStandard.AddPLTStandard(PltStd);
        }
        catch(FisException fex)
        {

            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        showPLTStdList();
        itemId = replaceSpecialChart(itemId);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');HideWait();", true);
    }
   
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        string id = this.HiddenSelectedId.Value.Trim();
        try 
        {
            iPLTStandard.DeletePLTStandard(Convert.ToInt32(id));
        }
        catch(FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        showPLTStdList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "DeleteComplete();HideWait();", true);

    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {

        string itemId = this.HiddenSelectedId.Value.Trim();

        PltstandardInfo PltStd = new PltstandardInfo();
        PltStd.pltno = this.TextPalletNO.Text.Trim();
        //PltStd.len = decimal.Parse(this.TextLength.Text);
        //PltStd.wide = decimal.Parse(this.TextWidth.Text);
        //PltStd.high = decimal.Parse(this.TextHeight.Text);
        PltStd.len = decimal.Parse(this.HiddenLenght.Value);
        PltStd.wide = decimal.Parse(this.HiddenWidth.Value);
        PltStd.high = decimal.Parse(this.HiddenHeight.Value);
        //PltStd.cause = this.ddlCause.InnerDropDownList.SelectedValue;
        PltStd.editor = this.HiddenUserName.Value.Trim();

        PltStd.id = Convert.ToInt32(itemId);
        
        try
        {
            iPLTStandard.UpdatePLTStandard(PltStd);
        }
        catch(FisException fe)
        {
            showErrorMessage(fe.mErrmsg);
            return;
        }
        catch(Exception ee)
        {
            showErrorMessage(ee.Message);
            return;
        }
        showPLTStdList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');HideWait();", true);

    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[COL_NUM - 1].Attributes.Add("style", e.Row.Cells[COL_NUM - 1].Attributes["style"] + "display:none");
        for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }
        
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
   
    private void initLabel()
    {
        this.lblPalletNoQuery.Text = this.GetLocalResourceObject(Pre + "_lblPalletNoQuery").ToString();
        this.lblPalletSpec.Text = this.GetLocalResourceObject(Pre + "_lblPalletSpec").ToString();
        this.lblPalletNO.Text = this.GetLocalResourceObject(Pre + "_lblPalletNO").ToString();
        this.lblLength.Text = this.GetLocalResourceObject(Pre + "_lblLength").ToString();
        this.lblWidth.Text = this.GetLocalResourceObject(Pre + "_lblWidth").ToString();
        this.lblHeight.Text = this.GetLocalResourceObject(Pre + "_lblHeight").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();

    }
    private void bindTable(IList<PltstandardInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblPalletNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblLength").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblWidth").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblHeight").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblNextWC").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblUdt").ToString());
        dt.Columns.Add("ID");
        //Defect栏位的数据显示为@DefectCode +空格+@Descr
        //Pre WC、Cur WC和Next WC栏位的数据显示为@Station+空格+@Name
        if (list != null && list.Count != 0)
        {
            foreach (PltstandardInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.pltno;
                dr[1] = temp.len.ToString();
                dr[2] = temp.wide.ToString();
                dr[3] = temp.high.ToString();
                dr[4] = temp.editor;
                dr[5] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[6] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[7] = temp.id;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            this.hidRecordCount.Value = "";
        }
        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true); 
    }
    private Boolean showPLTStdList()
    {

        IList<PltstandardInfo> PltstandardLst = null;
        try
        {
            PltstandardLst = (List<PltstandardInfo>)iPLTStandard.GetPLTStandardList();

            if (PltstandardLst == null || PltstandardLst.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(PltstandardLst, DEFAULT_ROWS);
            }
        }
        catch (FisException fex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(fex.mErrmsg);
            return false;
        }
        catch (System.Exception ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }
    
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return errorMsg;
    }
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(14);
        //gd.HeaderRow.Cells[7].Width = Unit.Percentage(14);
    }
}
