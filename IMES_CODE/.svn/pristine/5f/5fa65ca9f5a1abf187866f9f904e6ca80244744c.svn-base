using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;


public partial class FA_KittingInput : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    public string kiFloor = WebCommonMethod.getConfiguration("KIFloor");
    public Boolean floorEnabled = false;
    public string userId;
    public string customer;
    

     protected void Page_Load(object sender, EventArgs e)
    {
        try {
             if (!Page.IsPostBack)
            {
                InitLabel();
                //将pdLine combobox的初始化参数传入
                this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                this.cmbPdLine.Stage = "FA";

                this.station.Value = Request["Station"]; 
                //设置floor是否被enabled
                string[] customerArray = kiFloor.Split(',');
              
                foreach (string customer1 in customerArray)
                {
                    if (String.Equals(customer1, Master.userInfo.Customer, System.StringComparison.OrdinalIgnoreCase))
                    {
                        floorEnabled = true;
                    }
                }

                //this.cmbFloor.setSelected(1);

                if (floorEnabled)
                {
                    setFloorCmbFocus();
                }
                else
                {
                    //-- this.cmbFloor.Enabled = false;
                    //setPdLineCombFocus();
                }
                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;
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
        //this.lbFloor.Text = this.GetLocalResourceObject(Pre + "_lblFloor").ToString();
        this.lbInputAmt.Text = this.GetLocalResourceObject(Pre + "_lblInputAmt").ToString();
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdId").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbBoxId.Text = this.GetLocalResourceObject(Pre + "_lblBoxId").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.txtInputAmt.Text = "0";
    }



    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {
       
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    /// <summary>
    /// 置快速控件焦点
    /// </summary>
    //private void setPdLineCombFocus()
    //{
    //
    //    String script = "<script language='javascript'>  window.setTimeout (setPdLineCmbFocus,100); </script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    //}

    /// <summary>
    /// 置快速控件焦点
    /// </summary>
    private void setFloorCmbFocus()
    {

        String script = "<script language='javascript'>  window.setTimeout (setFloorCmbFocus,100); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setFloorCmbFocus", script, false);
    }
}