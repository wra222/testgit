/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PACosmetic
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-20   zhu lei            Create 
 * 
 * Known issues:Any restrictions about this file 
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
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.DataModel;

using System.Web.Services;
using IMES.Station.Interface.CommonIntf;

public partial class DismantlePilotRun : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;


    IDismantlePilotRun iDismantlePilotRun = ServiceAgent.getInstance().GetObjectByName<IDismantlePilotRun>(WebConstant.DismantlePilotRun);

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
                initLabel();
                SetStage();
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

    private void SetStage()
    {
        IList<string> lst = iDismantlePilotRun.GetStage();
        ListItem item = null;
        dropStage.Items.Clear();
        dropStage.Items.Add(string.Empty);
        foreach (string stage in lst)
        {
            item = new ListItem(stage, stage);
            this.dropStage.Items.Add(item);
        }
    
    }

    private void initLabel()
    {
      //  this.lblPdline.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
 
    }
     

    private void showErrorMessage(string errorMsg)
    {
     
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    } 
     [System.Web.Services.WebMethod]
    public static ArrayList GetPilotMoInfo(string sn, string stage, string customer, string station, string user)
    {
         try
         {
             IDismantlePilotRun iDismantlePilotRun = ServiceAgent.getInstance().GetObjectByName<IDismantlePilotRun>(WebConstant.DismantlePilotRun);
             ArrayList arr =iDismantlePilotRun.GetPilotMoInfo(sn,stage, customer, station, user);
           
             return arr;
         }
         catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
        catch (Exception e)
        {
            throw e;
        }
    
    }
     [System.Web.Services.WebMethod]
     public static void Dismantle(string sn)
     {
         try
         {
             IDismantlePilotRun iDismantlePilotRun = ServiceAgent.getInstance().GetObjectByName<IDismantlePilotRun>(WebConstant.DismantlePilotRun);
            iDismantlePilotRun.Dismantle(sn);
          
         }
         catch (FisException e)
         {
             throw new Exception(e.mErrmsg);
         }
         catch (Exception e)
         {
             throw e;
         }

     }
     [System.Web.Services.WebMethod]
     public static void Cancel(string sn )
     {
         try
         {
             IDismantlePilotRun iDismantlePilotRun = ServiceAgent.getInstance().GetObjectByName<IDismantlePilotRun>(WebConstant.DismantlePilotRun);
             iDismantlePilotRun.Cancel(sn);
         }
         catch (FisException e)
         {
             throw new Exception(e.mErrmsg);
         }
         catch (Exception e)
         {
             throw e;
         }

     }
}
