/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for    
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* Known issues:
* TODO：
*/

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
//using IMES.Station.Interface.StationIntf;
//using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class FAI_FAIOutput : System.Web.UI.Page
{
    public const int DEFAULT_ROWS = 12;
	public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {    
           //2012-4-16
           UserId = Master.userInfo.UserId;
           Customer = Master.userInfo.Customer;

            if (!this.IsPostBack)
            {
                initLabel();
                this.hiddenStation.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];
                bindEmptyTable(DEFAULT_ROWS);
				
                this.txtDataEntry.Focus();
            }
       }
       catch (FisException ex)
       {
           writeToAlertMessage(ex.mErrmsg);
       }
       catch (Exception ex)
       {
           writeToAlertMessage(ex.Message);
       }
    }

    private void initLabel()
    {
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
    }

    public void btnHidden_Click(object sender, System.EventArgs e)
    {
        try
        {
            //ÖØÖÃÎÄ±¾¿ò
            this.lblDataEntry.Text = "";
            this.txtDataEntry.Focus();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
	
	private void bindEmptyTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("Defect Code");
		dt.Columns.Add("Defect Description");

        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
        }
        //iMES:GridViewExt ID="gd" data bind
        gd.DataSource = dt;
        gd.DataBind();
    }
}


