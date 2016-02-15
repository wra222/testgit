using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;
using System.Web.UI;
public partial class PAK_InputSnListForPAKSorting : System.Web.UI.Page
{
    private string station;
    private string userId;
    private string customer;
    
    protected void Page_Load(object sender, EventArgs e)
   {

       station = Request["Station"] ?? "";
       userId = Master.userInfo.UserId;
        customer = Master.userInfo.Customer;
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            IPizzaKitting pizza = ServiceAgent.getInstance().GetObjectByName<IPizzaKitting>(WebConstant.IPizzaKitting);
            string ss = txtSnList.Text;
            string[] snArr = txtSnList.Text.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<string> snList = new List<string>(snArr.Length);
            snList.AddRange(snArr);
            //string station,string editor,string line,string customer)
            List<string> lst = pizza.UploadSnForSorting(snList, station, userId, "", customer);
            if (lst.Count > 0)
            {
                string log = string.Join("\r\n", lst.ToArray());
                txtLog.Text = log;
            }
            else
            {
                txtLog.Text = "Success!";
            }
            txtSnList.Text = "";
            txtSnList.Focus();
            endWaitingCoverDiv();
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
        }
     
    }


    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();Clear();" + "\r\n" +
            "</script>";
       ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
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
