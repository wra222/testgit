
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
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;

public partial class CleanRoom_TouchGlassTimeCheck : System.Web.UI.Page
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initProductTableRowsCount = 10;
    public String UserId;
    public String Customer;
    public String Station;
    public String Pcode;
   

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
                initLabel();
                cmbConstValueType1.Type = "RCTOMaterialType";
                cmbConstValueType1.Attributes.Add("disabled", "");
                cmbPdLine1.Attributes.Add("disabled", "");
                Station = Request.QueryString["Station"];
                 this.cmbPdLine1.Stage = "PAK";
                this.cmbPdLine1.Station = Station;
                this.cmbPdLine1.Customer = Master.userInfo.Customer;

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
        this.lblPDLine.Text = this.GetLocalResourceObject(Pre + "_lblPDLine").ToString();
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrintSet").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
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

    static ITouchGlassCheckTime touchglass = ServiceAgent.getInstance().GetObjectByName<ITouchGlassCheckTime>(WebConstant.TouchGlassChecktime);

  
    [System.Web.Services.WebMethod]
    public static void CheckSn(string sn, string line, string editor, string customer,string type,string station)
    {
        try
        {
            touchglass.CheckMaSn(sn, station); 
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }  
    }
    [System.Web.Services.WebMethod]
    public static ArrayList Save(string snList, string line, string editor, string station, string customer,string type,IList<PrintItem> printItemLst)
    {
        try
        {
            ArrayList arr = touchglass.Save(snList, line, editor, station, customer, type, printItemLst);
            return arr;
        }
         catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }  
    }

   

   
}
