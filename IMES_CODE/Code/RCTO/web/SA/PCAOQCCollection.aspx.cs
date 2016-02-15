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

public partial class PCAOQCCollection : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 6;
    public String UserId;
    public String Customer;
    public string SpeedLimit = "-1";
    public string Type = "";

  

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            IDefect idefectService = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
            cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
       
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
            {
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
            }
            if (!this.IsPostBack)
            {
                initLabel();
        
               UserId = Master.userInfo.UserId;
               Customer = Master.userInfo.Customer;
               CmbDefect.refreshDropContentByTypeAndCustomer("SACause", "HP");
                this.cmbPdLine.Station = Request["station"];
                this.cmbPdLine.Customer = Customer;
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
   
    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
      
    }

    private void initLabel()
    {
        this.lblPdline.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
 
    }
     

    private void showErrorMessage(string errorMsg)
    {
     
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updHidden, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    } 
     [System.Web.Services.WebMethod]
    public static ArrayList InputMBNo(string mbNo,string line, string editor, string station, string customer)
    {
         try
         {
             IPCAOQCCollection obj = ServiceAgent.getInstance().GetObjectByName<IPCAOQCCollection>(WebConstant.PCAOQCCollection);
             ArrayList arr =obj.InputMBNo(mbNo,line,editor,station,customer);
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
     public static void Save(string mbNo, string actionName, string result, string cause, string remark)
     {
         try
         {
             IPCAOQCCollection obj = ServiceAgent.getInstance().GetObjectByName<IPCAOQCCollection>(WebConstant.PCAOQCCollection);
             obj.Save(mbNo, actionName, "MB", "1", result, cause, remark);
         
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
     public static void Cancel(string mbNo)
     {
         try
         {
             IPCAOQCCollection obj = ServiceAgent.getInstance().GetObjectByName<IPCAOQCCollection>(WebConstant.PCAOQCCollection);
             obj.Cancel(mbNo);
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
