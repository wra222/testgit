/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2009-02-01   207006     ITC-1122-0041
 * 2009-02-01   207006     ITC-1122-0042 
 * 2009-02-01   207006     ITC-1122-0078
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
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class FA_SpecialModelForITCND : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
   // IProIdPrint iProIdPrint = ServiceAgent.getInstance().GetObjectByName<IProIdPrint>(WebConstant.ProIdPrintObject);
    IMO iMO = ServiceAgent.getInstance().GetObjectByName<IMO>(WebConstant.CommonObject);

    public String UserId;
    public String Customer;
    private const int DEFAULT_ROWS = 12;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //2009-02-01   207006     ITC-1122-0041
            //2009-02-01   207006     ITC-1122-0042 
            //2009-02-01   207006     ITC-1122-0078
            //注册MBCode下拉框的选择事件
            this.CmbFamily1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbFamily_Selected);

            //注册111下拉框的选择事件
            this.CmbModel1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbModel_Selected);

            if (!Page.IsPostBack)
            {
                InitLabel();
                this.stationHF.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];


                bindTable(DEFAULT_ROWS);
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.CmbFamily1.Customer =  Customer;
                this.CmbFamily1.Service = "001";
                setFocus();
               // this.CmbModel1.Customer = Master.userInfo.Customer;
               // this.CmbModel1.Service = "014";//"014";
                //this.CmbFamily1.Service = "013";// "014";

             

            }
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }
    /// <summary>
    /// 选择Family下拉框，会刷新Model下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbFamily_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //如果选择为空
            if (this.CmbFamily1.InnerDropDownList.SelectedValue == "")
            {
                //清空111下拉框内容
                this.CmbModel1.clearContent();
                //清空MO下拉框内容
            }
            else
            {
                this.CmbModel1.Service = "102";
                //刷新111下拉框内容
                this.CmbModel1.refreshDropContent(this.CmbFamily1.InnerDropDownList.SelectedValue.ToString());

                if (this.CmbModel1.InnerDropDownList.Items.Count > 1)
                {
                    this.CmbModel1.InnerDropDownList.SelectedIndex = 1;
                   // cmbModel_Selected(sender, e);
                }
                else {
                    this.CmbModel1.clearContent();
                }
            }
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    /// <summary>
    /// 选择111下拉框，会刷新MO下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbModel_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //刷新MO下拉框内容
            if (this.CmbModel1.InnerDropDownList.SelectedValue == "")
            {
                //清空MO下拉框内容
            }
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lbFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbSpecialType.Text = this.GetLocalResourceObject(Pre + "_lblSpecialType").ToString();
        this.lblSpecialModelList.Text = this.GetLocalResourceObject(Pre + "_lblSpecialModelList").ToString();
       
        //this.lbPrintTemplate.Text = this.GetLocalResourceObject(Pre + "_lblPrintTemplate").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnQuery.Value = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnInsert.Value = this.GetLocalResourceObject(Pre + "_btnInsert").ToString();

        this.SpecialType.Items[0].Text = "";
        this.SpecialType.Items[1].Text = this.GetLocalResourceObject(Pre + "_lstItem0").ToString();
        this.SpecialType.Items[2].Text = this.GetLocalResourceObject(Pre + "_lstItem1").ToString();
        this.SpecialType.Items[0].Selected = true;
        this.btnDelete.Disabled = true;
      
    }

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colModel").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colOther").ToString());
        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
        }

        gd.DataSource = dt;
        gd.DataBind();
    }
/*
    protected void SpecialType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (SpecialType.SelectedValue == "Product")
        {

        }
        else
        {

        }

    }
    */
    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(String errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\'", string.Empty).Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// 调用web service打印接口成功后需要reset页面信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnHidden_Click(object sender, System.EventArgs e)
    {

    }

    /// <summary>
    ///置焦点
    /// </summary>  
    private void setFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout(setFamilyCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setFamilyCmbFocus", script, false);
    }

    /// <summary>
    ///置焦点
    /// </summary>  
    private void setFocus_reset()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout(setFamilyCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setFamilyCmbFocus", script, false);
    }
}
