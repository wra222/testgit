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
using IMES.Infrastructure;
using System.Text;

public partial class RCTO_CheckCartonCT : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 12;
    //private IDefect iDefect;
    public String UserId;
    public String Customer;
    public String Station;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();
               
                bindTable(DEFAULT_ROWS);
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                Station = Request["Station"];
                //this.CmbPdLine1.Station = Station;
                //this.CmbPdLine1.Customer = "HP";//Master.userInfo.Customer;
                
                setColumnWidth();
                setFocus();
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

    private void showErrorMessage(string errorMsg)
    {
        bindTable(DEFAULT_ROWS);
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void initLabel()
    {
        //this.lblCartonSN.Text = this.GetLocalResourceObject(Pre + "_lblPDLine").ToString();
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        
    }

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("MBCT2");

        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
        }

        gd.DataSource = dt;
        gd.DataBind();
    }

    private void setColumnWidth()
    {
        //gd.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        //Set column width 
        //================================= Add Code ======================================


        //================================= Add Code End ==================================
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }
}
