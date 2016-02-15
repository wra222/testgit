
using System;
using System.Collections;
using System.Collections.Generic;
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
using IMES.Infrastructure;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;

public partial class DataMaintain_FAIModelRule : System.Web.UI.Page
{
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 13;
    public String UserId;
    public String Customer;
    private IFAIModelRule iFAIModelRule = null;
    
    private bool gHasHightRow = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            this.HiddenUserName.Value = UserId;
            Customer = Master.userInfo.Customer;
            iFAIModelRule = ServiceAgent.getInstance().GetMaintainObjectByName<IFAIModelRule>(WebConstant.FAIModelRule);
            if (!this.IsPostBack)
            {
                initLabel();
                
                bindTable(null);
                
                bindFamilyFromFamilyInfo();
                bindFamily();
                bindModelType();
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null);
        }
        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
    }

    private void initLabel()
    {
        this.btnQuery.Value=this.GetLocalResourceObject(Pre+"_btnQuery").ToString();
        this.lblMasterLabelList.Text=this.GetLocalResourceObject(Pre+"_lblMasterLabelList").ToString();
        this.btnDelete.Value=this.GetLocalResourceObject(Pre+"_btnDelete").ToString();
        this.btnSave.Value=this.GetLocalResourceObject(Pre+"_btnSave").ToString();
    }

    private void bindFamilyFromFamilyInfo()
    {
        drpFamilyFromFamilyInfo.Items.Clear();
        drpFamilyFromFamilyInfo.Items.Add(new ListItem(""));
        DataTable dt1 = iFAIModelRule.GetFamilyFromFamilyInfo();
        if (dt1 != null && dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                drpFamilyFromFamilyInfo.Items.Add(new ListItem(dt1.Rows[i][0].ToString()));
            }
        }
    }

    private void bindFamily()
    {
        drpFamily.Items.Clear();
        drpFamily.Items.Add(new ListItem(""));
        DataTable dt1 = iFAIModelRule.GetFamily(this.Customer);
        if (dt1 != null && dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                drpFamily.Items.Add(new ListItem(dt1.Rows[i][0].ToString()));
            }
        }
    }

    private void bindModelType()
    {
        drpModelType.Items.Add(new ListItem(""));
        IList<string> lst = iFAIModelRule.GetModelType();
        foreach (string modelType in lst)
            drpModelType.Items.Add(new ListItem(modelType));
    }

    private void bindTable(DataTable dt1)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("ID");
		dt.Columns.Add("Family");
		dt.Columns.Add("ModelType");
        dt.Columns.Add("MoLimitQty");
		dt.Columns.Add("Editor");
		dt.Columns.Add("Cdt");
		dt.Columns.Add("Udt");
        if (dt1 != null && dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dr = dt.NewRow();
				for (int j=0; j<7; j++)
					dr[j] = dt1.Rows[i][j];

                dt.Rows.Add(dr);
            }
            for (int j = dt.Rows.Count; j < DEFAULT_ROWS; j++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();

        if (dt1 != null && dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
			{
				gd.Rows[i].Cells[5].Text = DateTime.Parse(gd.Rows[i].Cells[5].Text).ToString("yyyy/MM/dd");
                gd.Rows[i].Cells[6].Text = DateTime.Parse(gd.Rows[i].Cells[6].Text).ToString("yyyy/MM/dd");
			}
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

    protected void Query_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            DataTable dt = iFAIModelRule.Query(drpFamilyFromFamilyInfo.SelectedValue);
            bindTable(dt);
			
			ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "Query", "QueryComplete();", true);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null);
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
            iFAIModelRule.Save(hidSelectedID.Value, drpFamily.SelectedValue, drpModelType.SelectedValue, txtMOLimitQty.Text, UserId);

            DataTable dt = iFAIModelRule.Query(drpFamilyFromFamilyInfo.SelectedValue);
            bindTable(dt);

            bindFamilyFromFamilyInfo();

            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "AddUpdate", "QueryComplete();", true);//
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
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
            iFAIModelRule.Delete(hidSelectedID.Value, UserId);

            DataTable dt = iFAIModelRule.Query(drpFamilyFromFamilyInfo.SelectedValue);
            bindTable(dt);

            bindFamilyFromFamilyInfo();

            ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "Delete", "QueryComplete();", true);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
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
        gd.HeaderRow.Cells[0].Width = Unit.Pixel(0);
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[2].Width = Unit.Pixel(50);
        gd.HeaderRow.Cells[3].Width = Unit.Pixel(60);
        gd.HeaderRow.Cells[4].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[5].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[6].Width = Unit.Pixel(100);
    }
    
}
