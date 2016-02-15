/*
 INVENTEC INVENTEC corporation (c)2011 all rights reserved. 
 Description: IEC Label Print
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2011-11-30  zhu lei              Create
 2012-03-04  Li.Ming-Jun          ITC-1360-0969
 Known issues:
 TODO:
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
//using IMES.Infrastructure.FisObjectRepositoryFramework;
//using IMES.FisObject.Common.Warranty;


public partial class FA_IECLabelPrint : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IIECLabelPrint iIECLabelPrint = ServiceAgent.getInstance().GetObjectByName<IIECLabelPrint>(WebConstant.IECLabelPrintObject);
    public string UserId;
    public string Customer;

     protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //注册Family下拉框的选择事件
            this.cmbFamily.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbFamily_Selected);

            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;

            if (!Page.IsPostBack)
            {
                InitLabel();
                this.cmbFamily.Customer = Customer;
                this.cmbDataCode.Customer = Customer;
            }
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

    /// <summary>
    /// 选择Model下拉框，会刷新Dcode下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
     public void btnHidModel_Click(object sender, System.EventArgs e)
     {
        try {
            //如果选择为空
            if (this.cmbModel.getModelObj().Value.Trim() == "")
            {
                if (this.chkPC.Checked)
                {
                    //清空Dcode下拉框内容
                    this.cmbDataCode.clearContent();
                }
                else
                {
                    this.cmbDataCode.InnerDropDownList.Enabled = true;
                }
            }
            else
            {
                this.cmbModel.checkModelExist();
                this.cmbModel.checkModelBelongtoFamily(this.cmbFamily.InnerDropDownList.SelectedValue);

                //刷新PPID Description下拉框内容
                string value = iIECLabelPrint.SetDataCodeValue(this.cmbModel.getModelContent(), Master.userInfo.Customer);
                for (int i = 0; i < this.cmbDataCode.InnerDropDownList.Items.Count; i++)
                {
                    if (this.cmbDataCode.InnerDropDownList.Items[i].Text.Trim() == value.Trim())
                    {
                        this.cmbDataCode.setSelected(i);
                        this.cmbDataCode.InnerDropDownList.Enabled = false;
                        break;
                    }
                }
            }
        }
        catch (FisException ee)
        {
            this.cmbModel.clearContent();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            this.cmbModel.clearContent();
            writeToAlertMessage(ex.Message);
        }

    }

    /// <summary>
    /// 选择Family下拉框，会刷新Model下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbFamily_Selected(object sender, System.EventArgs e)
    {
        try {
            //刷新Model下拉框内容
            if (this.cmbFamily.InnerDropDownList.SelectedValue == "")
            {
                if (this.chkPC.Checked)
                {
                    //清空Model下拉框内容
                    this.cmbModel.clearContent();
                }               
            }
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

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lblchkPC.Text = this.GetLocalResourceObject(Pre + "_chkPC").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblPartType.Text = this.GetLocalResourceObject(Pre + "_lblPartType").ToString();
        this.lblREV.Text = this.GetLocalResourceObject(Pre + "_lblREV").ToString();
        this.lblDataCode.Text = this.GetLocalResourceObject(Pre + "_lblDateCode").ToString();
        this.lblConfig.Text = this.GetLocalResourceObject(Pre + "_lblConfig").ToString();
        this.lblQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.lblQtyTip.Text = this.GetLocalResourceObject(Pre + "_lblQtyTip").ToString();
        this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
        this.btnPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
        this.chkPC.Checked = true;
    }



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
        try {
            //重置下拉框
            this.cmbFamily.setSelected(0);
            this.cmbDataCode.setSelected(0);
            this.chkPC.Checked = true;
           
            //重置文本框
            this.cmbModel.clearContent();
            this.txtConfig.Value = "";
            this.txtQty.Value = "";
            this.txtREV.Value = "";
            setFocus();
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

    /// <summary>
    ///置焦点
    /// </summary>  
    private void setFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "setFamilyCmbFocus();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setFamilyCmbFocus", script, false);
    }
}