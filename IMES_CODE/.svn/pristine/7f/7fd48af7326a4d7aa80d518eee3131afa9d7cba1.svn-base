/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PIAOutput
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-11-09   CHENPENG     Create 
 * 
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;

public partial class FA_PIAOutput : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 12;
    private IDefect iDefect;
    private Object commServiceObj;
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
            {
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
            }
            if (!this.IsPostBack)
            {
                initLabel();
                initDefectList();
                bindTable(DEFAULT_ROWS);
                //UserId = Master.userInfo.UserId;
                //Customer = Master.userInfo.Customer;
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

    private void initDefectList()
    {
        commServiceObj = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
        iDefect = (IDefect)commServiceObj;

        if (iDefect != null)
        {
            IList<DefectInfo> defectList = iDefect.GetDefectList("PRD");
        
            if (defectList != null && defectList.Count != 0)
            {
                foreach (DefectInfo item in defectList)
                {
                    this.lbDefectList.Items.Add(new ListItem(item.id + " " + item.friendlyName, item.id));
                }
            }
        }
    }

    private void initLabel()
    {
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.lblInputDefectList.Text = this.GetLocalResourceObject(Pre + "_lblInputDefectList").ToString();
        this.lblFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lblPdLine.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
        this.lblProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdid").ToString();
        this.lblSupportDefectList.Text = this.GetLocalResourceObject(Pre + "_lblSupportDefectList").ToString();
        //////////
        this.lblFailQty1.Text = this.GetLocalResourceObject(Pre + "_lblFailQty1").ToString();
        this.lblPassQty1.Text = this.GetLocalResourceObject(Pre + "_lblPassQty1").ToString();
        this.gdCheckBox.Text = this.GetLocalResourceObject(Pre + "_gdCheckBox").ToString();
        this.btnOK.Value = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnMVunpack.Value = this.GetLocalResourceObject(Pre + "_btnMVunpack").ToString();

        this.btnOK.Disabled = true;

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

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDefectCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());

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
        
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }
}
