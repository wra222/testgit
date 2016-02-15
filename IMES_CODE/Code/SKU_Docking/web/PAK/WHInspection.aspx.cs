
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class PAK_WHInspection : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    private IWHInspection iWHInspection;
    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {
           iWHInspection = ServiceAgent.getInstance().GetObjectByName<IWHInspection>(WebConstant.WHInspectionObject);
           UserId = UserId ?? Master.userInfo.UserId;
           Customer = Customer ?? Master.userInfo.Customer;
           if (!this.IsPostBack)
            {
                initLabel();
                initMaterialType();
                //this.cmbPdLine.Station = Request["station"];
                //this.cmbPdLine.Customer = Customer;
            
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
        //this.lblPdline.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
        //this.lblCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
  
    }

    private void initMaterialType()
    {
        this.cmbMaterialType.Items.Clear();
        this.cmbMaterialType.Items.Add(string.Empty);
        IList<ConstValueTypeInfo> list = iWHInspection.GetMaterialType("MaterialType");
        foreach (ConstValueTypeInfo items in list)
        {
            if (items.value != "")
            {
                this.cmbMaterialType.Items.Add(new ListItem { Text = items.value, Value = items.value });
            }
        }
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
}


