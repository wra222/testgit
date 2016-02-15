using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class FA_MaterialReturn : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    private const int DEFAULT_ROWS = 9;
    public String UserId;
    public String Customer;
    public String Station;
    public String ReturnType;
    public String MaterialType;
    private IMaterialReturn iMaterialReturn;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iMaterialReturn = ServiceAgent.getInstance().GetObjectByName<IMaterialReturn>(WebConstant.MaterialReturnObject);
            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(DEFAULT_ROWS);

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                //Station = Request["Station"];
                ReturnType = Request["ReturnType"];
                MaterialType = Request["MaterialType"];
                initcmbMaterialType();
                if ((Station ==null) ||(Station ==""))
                {
                    this.cmbPdLine.Stage = "FA";
                }
                else
                {
                    this.cmbPdLine.Station = Station;
                }
                this.cmbPdLine.Customer = Customer;

            }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.Message);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void initLabel()
    {
        this.lbPDLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lbCollection.Text = this.GetLocalResourceObject(Pre + "_lblCollectionData").ToString();
    }

    private void initcmbMaterialType()
    {
        this.cmbMaterialType.Items.Clear();
        this.cmbMaterialType.Items.Add(string.Empty);
        IList<ConstValueTypeInfo> list = iMaterialReturn.GetMaterialType("MaterialType", MaterialType);
        foreach (ConstValueTypeInfo items in list)
        {
            if (items.value != "")
            {
                this.cmbMaterialType.Items.Add(new ListItem { Text = items.value, Value = items.value });
            }
        }
    }

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("CT");
        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
        }
        GridViewExt1.DataSource = dt;
        GridViewExt1.DataBind();
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }

    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
    }

    private void callInputRun()
    {
        String script = "<script language='javascript'> getAvailableData('input'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "callInputRun", script, false);
    }

}
