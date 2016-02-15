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
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;





public partial class SA_MBLabelPrint : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    ISMTMO iSMTMO = ServiceAgent.getInstance().GetObjectByName<ISMTMO>(WebConstant.CommonObject);
    public string userId;
    public string customer;
    


    protected void Page_Load(object sender, EventArgs e)
     {
        try
        {
            //注册MBCode下拉框的选择事件
            //this.cmbMBCode.IsDataFilter = true;
            this.cmbMBCode.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbMBCode_Selected);

            //注册model下拉框的选择事件
            this.cmbModel.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbModel_Selected);

            //注册MO下拉框的选择事件
            this.cmbMO.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbMO_Selected);

            userId = Master.userInfo.UserId;
            customer = Master.userInfo.Customer;
            this.cmbMBCode.Service = "002";
            this.cmbModel.Service = "002";
            this.cmbMO.Service = "006";
            if (!Page.IsPostBack)
            {
                InitLabel();
                this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                this.station.Value = Request["Station"];
                //this.cmbMO.Service = "002";
                
                this.pCode.Value = Request["PCode"];
              //  this.pCode.Value = "OPSA004  ";

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
    /// 选择MBCode下拉框，会刷新111下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbMBCode_Selected(object sender, System.EventArgs e)
    {
       
        try
        {
            this.cmbMBCode.Service = "002";
            this.cmbModel.Service = "002";
            this.cmbMO.Service = "006";
             //如果选择为空
            if (this.cmbMBCode.InnerDropDownList.SelectedValue == "")
            {
                //清空111下拉框内容
                this.cmbModel.clearContent();
                //清空MO下拉框内容
                this.cmbMO.clearContent();
                //清空MO Qty与Remain Qty
                this.lbShowMoQty.Text = "";
                this.lbShowReQty.Text = "";
                this.UpdatePanel1.Update();
                this.UpdatePanel2.Update();
                
            }
            else
            {
                //刷新model下拉框内容
                this.cmbModel.refresh(this.cmbMBCode.InnerDropDownList.SelectedValue);
                if (this.cmbModel.InnerDropDownList.Items.Count > 1)
                {
                    this.cmbModel.InnerDropDownList.SelectedIndex = 1;
                    cmbModel_Selected(sender, e);
                }
                else
                {
                    //清空MO下拉框内容
                    this.cmbMO.clearContent();
                    //清空MO Qty与Remain Qty
                    this.lbShowMoQty.Text = "";
                    this.lbShowReQty.Text = "";
                    this.UpdatePanel1.Update();
                    this.UpdatePanel2.Update();
                }
            }

            
        } catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
       
    }


  
    /// <summary>
    ///选择model下拉框，会刷新MO下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbModel_Selected(object sender, System.EventArgs e)
    {
       
        try
        {
            this.cmbMBCode.Service = "002";
            this.cmbModel.Service = "002";
            this.cmbMO.Service = "006";
             //刷新MO下拉框内容
             if (this.cmbModel.InnerDropDownList.SelectedValue == "")
            {
                //清空MO下拉框内容
                this.cmbMO.clearContent();
                //清空MO Qty与Remain Qty
                this.lbShowMoQty.Text = "";
                this.lbShowReQty.Text = "";
                this.UpdatePanel1.Update();
                this.UpdatePanel2.Update();
               
            }
            else
            {
                this.cmbMO.refreshDropContent(this.cmbModel.InnerDropDownList.SelectedValue);
                if (this.cmbMO.InnerDropDownList.Items.Count > 1)
                {
                    this.cmbMO.InnerDropDownList.SelectedIndex = 1;
                    cmbMO_Selected(sender, e);
                }
                else
                {
                    //清空MO Qty与Remain Qty
                    this.lbShowMoQty.Text = "";
                    this.lbShowReQty.Text = "";
                    this.UpdatePanel1.Update();
                    this.UpdatePanel2.Update();
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
    ///选择111下拉框，会刷新MO下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbModel_Selected1(object sender, System.EventArgs e)
    {

        try
        {
            this.cmbMBCode.Service = "002";
            this.cmbModel.Service = "002";
            this.cmbMO.Service = "006";
            //刷新MO下拉框内容
            if (this.cmbModel.InnerDropDownList.SelectedValue == "")
            {
                //空MO下拉框内容
                this.cmbMO.clearContent();
               

            }
            else
            {
                this.cmbMO.refreshDropContent(this.cmbModel.InnerDropDownList.SelectedValue);
              
            }
            //空MO Qty与Remain Qty
            this.lbShowMoQty.Text = "";
            this.lbShowReQty.Text = "";
            this.UpdatePanel1.Update();
            this.UpdatePanel2.Update();

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
    ///  选择MO下拉框，会更新MO Qty与Remain Qty的内
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbMO_Selected(object sender, System.EventArgs e)
    {
        int remainQty;
     
        if (this.cmbMO.InnerDropDownList.SelectedValue == "")
        {
            this.lbShowMoQty.Text = "";
            this.lbShowReQty.Text = "";
        }
        else
        {
            try
            {
                SMTMOInfo sMTMOInfo = iSMTMO.GetSmtmoInfoList(this.cmbMO.InnerDropDownList.SelectedValue);                
                this.lbShowMoQty.Text = sMTMOInfo.totalMBQty.ToString();
                remainQty = sMTMOInfo.totalMBQty - sMTMOInfo.printedMBQty;
                this.lbShowReQty.Text = remainQty.ToString(); 

            }
            catch (FisException ee)
            {
                writeToAlertMessage(ee.mErrmsg);
            }
            catch (Exception ex)
            {
                writeToAlertMessage(ex.Message);
            }
            //this.lbShowMoQty.Text = "1";
            //this.lbShowReQty.Text = "50";


          
            
        }
        this.UpdatePanel1.Update();
        this.UpdatePanel2.Update();

    }
    /// <summary>
    ///初始化页面的静态label容
    /// </summary>
    private void InitLabel()
    {
        this.lbMBCode.Text = this.GetLocalResourceObject(Pre + "_lblMBCodePCB").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbMO.Text = this.GetLocalResourceObject(Pre + "_lblMO").ToString();
        this.lbMoQty.Text = this.GetLocalResourceObject(Pre + "_lblMOQty").ToString();
        this.lbReQty.Text = this.GetLocalResourceObject(Pre + "_lblRemainQty").ToString();
        this.lbPdline.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbPeriod.Text = this.GetLocalResourceObject(Pre + "_lblPeriod").ToString();
        this.lbThisMonth.Text = this.GetLocalResourceObject(Pre + "_lblTMonth").ToString();
        this.lbNextMonth.Text = this.GetLocalResourceObject(Pre + "_lblNMonth").ToString();
        this.lbQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.lbFactor.Text = this.GetLocalResourceObject(Pre + "_lbFactor").ToString();
        this.btpPrintSet.Value= this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString(); 
        this.btnPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
        this.lbQtyTip.Text = this.GetLocalResourceObject(Pre + "_lblQtyTip").ToString();
        this.lblLargeLabel.Text = this.GetLocalResourceObject(Pre + "_lblLargeLabel").ToString();
        this.lblSmallLabel.Text = this.GetLocalResourceObject(Pre + "_lblSmallLabel").ToString();
        setFocus();
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
    /// 调用web service打印接口成功后需要reset页面信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnHidden_Click(object sender, System.EventArgs e)
    {
        try
        {
            //this.cmbMBCode.InnerDropDownList.SelectedIndex = 0;

            //重置mbCode下拉框,并触发它的选择事件
            // this.cmbMBCode.setSelected(0);
            // cmbMBCode_Selected(sender, e);


            // //重置pdLine下拉框
            // //            <bug>
            // //            BUG NO:ITC-1103-0155 
            // //            REASON:不需要重置
            // //            </bug>
            // //this.cmbPdLine.setSelected(0);

            // //重置文本框
            // this.txtDateCode.Value = "";
            // this.txtQty.Value = "";
            // //重置radio button
            // this.thisMonth.Checked = true;
            // this.nextMonth.Checked = false;
         
           //endWaitingCoverDiv();
          
           // setFocus();

            if (this.txtQty.Value == this.lbShowReQty.Text)
            {
                //MB Label 已经全部列印完毕时，需要将该MO 从MO 下拉列表中删除，当前选择MO 为空，清空MO 数量，剩余数量；其他内容保持不变
                cmbModel_Selected1(sender, e);
            }
            else
            {
                cmbMO_Selected(sender, e);
            }
        } catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            endWaitingCoverDiv();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            endWaitingCoverDiv();
        }
    }

    /// <summary>
    ///reset页面信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
            //this.cmbMBCode.InnerDropDownList.SelectedIndex = 0;

            //重置mbCode下拉框,并触发它的选择事件
            this.cmbMBCode.setSelected(0);
            cmbMBCode_Selected(sender, e);

            //重置pdLine下拉框
            //            <bug>
            //            BUG NO:ITC-1103-0155 
            //            REASON:不需要重置
            //            </bug>
            this.cmbPdLine.setSelected(0);




            //重置文本框
            /* marked by Jiali 2011.03.14
            this.txtDateCode.Value = "";
            */
            this.txtQty.Value = "";
            //重置radio button

            this.thisMonth.Checked = true;
            this.nextMonth.Checked = false;
            this.lbShowMoQty.Text = "";
            this.lbShowReQty.Text = "";
            this.UpdatePanel1.Update();
            this.UpdatePanel2.Update();

           

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
            "window.setTimeout (setMBCodeCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setMBCodeCmbFocus2", script, false);
       
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

}
