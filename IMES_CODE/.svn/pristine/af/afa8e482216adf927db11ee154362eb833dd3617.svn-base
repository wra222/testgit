/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for FRU Carton Label Print (for Docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI FRU Carton Label for Docking
 * UC:CI-MES12-SPEC-PAK-UC FRU Carton Label for Docking      
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-25  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class PAK_FRUCartonLabel : System.Web.UI.Page
{

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String Customer;

    [WebMethod]
    public static ArrayList GetPrintTemplate(string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();

        try
        {
            IList<PrintItem> retLst = ServiceAgent.getInstance().GetObjectByName<IFRUCartonLabel>(WebConstant.FRUCartonLabelForDockingObject).GetPrintTemplate(customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            return ret;
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

    [WebMethod]
    public static ArrayList CheckModelExist(string customer, string model)
    {
        ArrayList ret = new ArrayList();

        try
        {
            bool retFlag = ServiceAgent.getInstance().GetObjectByName<IFRUCartonLabel>(WebConstant.FRUCartonLabelForDockingObject).CheckModelExist(customer, model);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retFlag);
            return ret;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
                initLabel();           
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


    /// <summary>
    /// ????????label
    /// </summary>
    /// <returns></returns>
     private void initLabel()
    {
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblPCS.Text = this.GetLocalResourceObject(Pre + "_lblPCS").ToString();
        this.lblQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
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

  
}