using System;
using System.Data;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.Web.UI;
using IMES.Maintain.Interface.MaintainIntf;
using System.Web.UI.WebControls;
using System.Web;
using IMES.DataModel;
using System.Collections.Generic;
using System.Linq;

public partial class LabelTypeRuleDialog : System.Web.UI.Page
{
    private ILabelTypeRuleMaintain iLabelTypeRuleMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<ILabelTypeRuleMaintain>(WebConstant.LabelTypeRuleObject);
    private IConstValueMaintain iConstValue = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueMaintain>(WebConstant.ConstValueMaintainObject);
    private const int DEFAULT_ROWS = 12;
    private const int COL_NUM = 5;
    public String userName;
    public string LabelType;
    public string labelTypeName;
    public string labelList;
    public string BomLevel;
    protected void Page_Load(object sender, EventArgs e)
    {
        LabelType = Request.QueryString["LabelType"];
        BomLevel = Request.QueryString["BomLevel"];
        int index = LabelType.IndexOf('_');
        labelList = LabelType.Substring(0, index);
        labelTypeName = LabelType.Substring(index + 1);
        userName = Request.QueryString["UserName"];
        if (!this.IsPostBack)
        {
            
            this.HiddenUserName.Value = userName;
            //string ReturnValue = Request.QueryString["ReturnValue"];
            initLabel();
            if (LabelType != "")
            {
                IList<ConstValueInfo> List = iConstValue.GetConstValueListByType(LabelType);
                if (List == null || List.Count == 0)
                {
                    bindTable(null, DEFAULT_ROWS);
                }
                else
                {
                    bindTable(List, DEFAULT_ROWS);
                }
            }
            else
            {
                showErrorMessage("沒有傳入LabelType...");
            }
        }
    }

    private void initLabel()
    {
        this.lblLebelTypeName.Text = labelTypeName;
        this.lblList.Text = labelList + " List";
        this.lblInfoName.Text = "InfoName:";
        this.lblInfoValue.Text = "InfoValue:";
        this.lblDescription.Text = "Description:";
        this.btnSave.Value = "Save";
        this.btnDelete.Value = "Delete";
        this.btnClose.Value = "Close";
    }

    private Boolean ShowList()
    {
        try
        {
            IList<ConstValueInfo> List = iConstValue.GetConstValueListByType(LabelType);
            if (List == null || List.Count == 0)
            {
                this.hidLabelType.Value = "";
                switch (labelList)
                {
                    case "Model":
                        iLabelTypeRuleMaintain.UpdateAndInsertModelConstValue(labelTypeName, "", userName);
                        this.hidLabelType.Value = LabelType;
                        break;
                    case "Delivery":
                        iLabelTypeRuleMaintain.UpdateAndInsertDeliveryConstValue(labelTypeName, "", userName);
                        this.hidLabelType.Value = LabelType;
                        break;
                    case "PartInfo":
                        if (BomLevel != "")
                        {
                            iLabelTypeRuleMaintain.UpdateAndInsertPartConstValue(labelTypeName, Convert.ToInt32(BomLevel), "", userName);
                            this.hidLabelType.Value = LabelType;
                        }
                        else
                        {
                            showErrorMessage("沒有傳入BomLevel...");
                        }
                        break;
                    default:
                        break;
                }
                this.updatePanel1.Update();
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(List, DEFAULT_ROWS);
            }
        }
        catch (FisException ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        ConstValueInfo item = new ConstValueInfo();
        
        try
        {
            switch (labelList)
            {
                case "Model":
                    UpdateAndInsertLabelTypetoConstValue();
                    iLabelTypeRuleMaintain.UpdateAndInsertModelConstValue(labelTypeName, LabelType, userName);
                    this.hidLabelType.Value = LabelType;
                    this.updatePanel1.Update();
                    break;
                case "Delivery":
                    UpdateAndInsertLabelTypetoConstValue();
                    iLabelTypeRuleMaintain.UpdateAndInsertDeliveryConstValue(labelTypeName, LabelType, userName);
                    this.hidLabelType.Value = LabelType;
                    this.updatePanel1.Update();
                    break;
                case "PartInfo":
                    if (BomLevel != "")
                    {
                        UpdateAndInsertLabelTypetoConstValue();
                        iLabelTypeRuleMaintain.UpdateAndInsertPartConstValue(labelTypeName, Convert.ToInt32(BomLevel), LabelType, userName);
                        this.hidLabelType.Value = LabelType;
                        this.updatePanel1.Update();
                    }
                    else
                    {
                        showErrorMessage("沒有傳入BomLevel...");
                    }
                    break;
                default:
                    break;
            }
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
        ShowList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + this.hidID.Value + "');DealHideWait();", true);
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            ConstValueInfo item = new ConstValueInfo();
            item.id = Convert.ToInt32(this.hidID.Value);
            iConstValue.DeleteConstValue(item);
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
        ShowList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void UpdateAndInsertLabelTypetoConstValue()
    {
        try
        {
            ConstValueInfo item = new ConstValueInfo();
            IList<ConstValueInfo> List = iConstValue.GetConstValueListByType(LabelType);
            string Name = this.txtInfoName.Value.Trim();
            int count = (from q in List
                        where q.name == Name
                        select q).Count();

            item.name = Name;
            item.type = LabelType;
            item.value = this.txtInfoValue.Value.Trim();
            item.description = this.txtDescription.Value.Trim();
            item.editor = this.HiddenUserName.Value;

            if (count == 0)
            {
                iConstValue.AddConstValue(item);
            }
            else
            {
                iConstValue.SaveConstValue(item);
            }
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
    }

    private void bindTable(IList<ConstValueInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("InfoName");
        dt.Columns.Add("InfoValue");
        dt.Columns.Add("Description");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        dt.Columns.Add("ID");
        if (list != null && list.Count != 0)
        {
            foreach (ConstValueInfo item in list)
            {
                dr = dt.NewRow();
                dr[0] = item.name.Trim();
                dr[1] = item.value.Trim();
                dr[2] = item.description.Trim();
                dr[3] = item.editor.Trim();
                dr[4] = item.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[5] = item.udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[6] = item.id;
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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(8);
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[6].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
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
}
