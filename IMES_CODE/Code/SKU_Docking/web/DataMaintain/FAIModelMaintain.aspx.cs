
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

public partial class DataMaintain_FAIModelMaintain : System.Web.UI.Page
{
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 3;
    public String UserId;
    public String Customer;
    private IFAIModelMaintain iFAIModelMaintain = null;
    
    private bool gHasHightRow = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            this.HiddenUserName.Value = UserId;
            iFAIModelMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<IFAIModelMaintain>(WebConstant.FAIModelMaintain);
            if (!this.IsPostBack)
            {
                initLabel();
                
                bindTable(null);
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
        this.btnReOpen.Value=this.GetLocalResourceObject(Pre+"_btnReOpen").ToString();
        this.btnAdd.Value=this.GetLocalResourceObject(Pre+"_btnAdd").ToString();
    }

    private void bindTable(DataTable dt1)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("Model");
		dt.Columns.Add("Family");
		dt.Columns.Add("ModelType");
		dt.Columns.Add("FAQty");
		dt.Columns.Add("InFAQty");
		dt.Columns.Add("FAState");
		dt.Columns.Add("PAKQty");
		dt.Columns.Add("InPAKQty");
		dt.Columns.Add("PAKState");
		dt.Columns.Add("Editor");
		dt.Columns.Add("Cdt");
		dt.Columns.Add("Udt");
        if (dt1 != null && dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dr = dt.NewRow();
				for (int j=0; j<12; j++)
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
				gd.Rows[i].Cells[10].Text = DateTime.Parse(gd.Rows[i].Cells[10].Text).ToString("yyyy/MM/dd");
                gd.Rows[i].Cells[11].Text = DateTime.Parse(gd.Rows[i].Cells[11].Text).ToString("yyyy/MM/dd");
			}
		}

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
            DataTable dt= iFAIModelMaintain.Query(txtFAIModel.Text);
            bindTable(dt);
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

    protected void FindFAIModelInfo_ServerClick(Object sender, EventArgs e)
    {
        try
        {
			ArrayList ret = iFAIModelMaintain.GetFAIModelInfo(txtFAIModelAdd.Text);
			lblFamilyContent.Text = ret[0].ToString();
            lblModelTypeContent.Text = ret[1].ToString();
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
	
	protected void Add_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            iFAIModelMaintain.Add(txtFAIModelAdd.Text, UserId);
			
			DataTable dt= iFAIModelMaintain.Query(txtFAIModelAdd.Text);
            bindTable(dt);
			
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "AddUpdate", "resetTableHeight();AddUpdateComplete('" + ID + "');", true);//
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

    protected void ReOpen_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            iFAIModelMaintain.ReOpen(txtFAIModel.Text, UserId);

            DataTable dt = iFAIModelMaintain.Query(hidReOpenID.Value);
            bindTable(dt);
			
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "ReOpen", "ReOpenComplete();", true);
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
        gd.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[2].Width = Unit.Pixel(50);
        gd.HeaderRow.Cells[3].Width = Unit.Pixel(60);
        gd.HeaderRow.Cells[4].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[5].Width = Unit.Pixel(60);
        gd.HeaderRow.Cells[6].Width = Unit.Pixel(60);
        gd.HeaderRow.Cells[7].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[8].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[9].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[10].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[11].Width = Unit.Pixel(100);
    }
    
}
