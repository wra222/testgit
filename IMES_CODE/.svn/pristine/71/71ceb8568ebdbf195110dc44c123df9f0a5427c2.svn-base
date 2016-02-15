/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
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

public partial class FA_AssignModel : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IAssignModel iAssignModel = ServiceAgent.getInstance().GetObjectByName<IAssignModel>(WebConstant.AssignModelObject);

    public String UserId;
    public String Customer;
	
	public String AccountId;
    public String Login;
	public String UserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
 
            if (string.IsNullOrEmpty(Customer))
            { 
				Customer = Master.userInfo.Customer; 
			}
            if (string.IsNullOrEmpty(UserId))
            { 
				UserId = Master.userInfo.UserId; 
			}
			
			this.cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            this.cmbPdLine.InnerDropDownList.AutoPostBack = true;
			
			//注册MBCode下拉框的选择事件
            this.CmbFamily1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbFamily_Selected);

            //注册111下拉框的选择事件
            this.CmbModel1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbModel_Selected);

            //注册MO下拉框的选择事件
//            this.cmbMO.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbMO_Selected);

            if (!Page.IsPostBack)
            {
                InitLabel();
                this.station.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];
                //this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                this.CmbFamily1.Customer = Master.userInfo.Customer;
                setFocus();
//                this.cmbMO.Service = "014";
                this.CmbModel1.Service = "014";
                this.CmbFamily1.Service = "014";

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
				AccountId = Master.userInfo.AccountId.ToString();
                Login = Master.userInfo.Login;
                UserName = Master.userInfo.UserName;

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
	
	private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        try
        {
			cmbModel_Selected(sender, e);
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
//                this.cmbMO.clearContent();
                //清空MO Qty与Remain Qty
                this.lbShowMoQty.Text = "";
                this.lbShowReQty.Text = "";
            }
            else
            {
//                this.cmbMO.Service = "014";
                this.CmbModel1.Service = "002";
                //刷新111下拉框内容
                this.CmbModel1.refreshDropContent(this.CmbFamily1.InnerDropDownList.SelectedValue);

                this.CmbModel1.InnerDropDownList.SelectedIndex = 0;
                cmbModel_Selected(sender, e);
                /*if (this.CmbModel1.InnerDropDownList.Items.Count > 1)
                {
                    this.CmbModel1.InnerDropDownList.SelectedIndex = 0;
//                    cmbModel_Selected(sender, e);
                }
                else
                {
//                    this.cmbMO.clearContent();
                    //清空MO Qty与Remain Qty
                    this.lbShowMoQty.Text = "";
                    this.lbShowReQty.Text = "";
                } */
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
            ClearLabel();
            string line = this.cmbPdLine.InnerDropDownList.SelectedValue;
			string family = this.CmbFamily1.InnerDropDownList.SelectedValue;
            string model = this.CmbModel1.InnerDropDownList.SelectedValue;
            if (line == "" || family == "" || model == "")
                return;
            line = line.Substring(0, 1);
            IList<string> lst = iAssignModel.GetActiveModel(line, model);
            if (lst != null)
            {
                this.lbShowMoQty.Text = lst[0];
                this.lbShowReQty.Text = lst[1];
                this.txtShipDate.Text = lst[2];
				this.pdLine.Value = this.cmbPdLine.InnerDropDownList.SelectedValue;
				this.theLine.Value = line;
				this.theFamily.Value = family;
				this.theModel.Value = model;
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
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
		this.lbPdline.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
		this.lbFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbMoQty.Text = this.GetLocalResourceObject(Pre + "_lblMOQty").ToString();
        this.lbReQty.Text = this.GetLocalResourceObject(Pre + "_lblRemainQty").ToString();
        
        this.lbShipDate.Text = "ShipDate";
		this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
    }

    private void ClearLabel()
    {
        this.lbShowMoQty.Text = "";
        this.lbShowReQty.Text = "";
        this.txtShipDate.Text = "";
		this.pdLine.Value = "";
		this.theLine.Value = "";
		this.theFamily.Value = "";
		this.theModel.Value = "";
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
        try
        {
//                this.cmbMO.Service = "014";
                if(this.CmbModel1.InnerDropDownList.SelectedValue == "")
                {
                    this.CmbModel1.clearContent();
                }
                else
                {

//                 this.cmbMO.refreshDropContent(this.CmbModel1.InnerDropDownList.SelectedValue);
            
                }
//                cmbMO_Selected(sender, e);

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
    ///置焦点
    /// </summary>  
    private void setFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout(setPdLineCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
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
