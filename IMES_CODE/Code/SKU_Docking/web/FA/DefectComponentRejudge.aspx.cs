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

public partial class DefectComponentRejudge : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;


    //IDismantlePilotRun iDismantlePilotRun = ServiceAgent.getInstance().GetObjectByName<IDismantlePilotRun>(WebConstant.DismantlePilotRun);
    //IDefectComponentRejudge iDefectComponentRejudge = ServiceAgent.getInstance().GetObjectByName<IDefectComponentRejudge>(WebConstant.DefectComponentRejudgeObject);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
                initLabel();
                //SetStage();
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
    public static ArrayList GetDefectComponentInfo(string sn, string customer, string station, string user)
    {
        try
        {
            IDefectComponentRejudge iDefectComponentRejudge = ServiceAgent.getInstance().GetObjectByName<IDefectComponentRejudge>(WebConstant.DefectComponentRejudgeObject);
            ArrayList arr = iDefectComponentRejudge.GetDefectComponentInfo(sn, customer, station, user);
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
    public static void Save(string sn, string status, string comment)
    {
        try
        {
            IDefectComponentRejudge iDefectComponentRejudge = ServiceAgent.getInstance().GetObjectByName<IDefectComponentRejudge>(WebConstant.DefectComponentRejudgeObject);
            iDefectComponentRejudge.Save(sn, status, comment);
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
            IDefectComponentRejudge iDefectComponentRejudge = ServiceAgent.getInstance().GetObjectByName<IDefectComponentRejudge>(WebConstant.DefectComponentRejudgeObject);
            iDefectComponentRejudge.Cancel(sn);
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
