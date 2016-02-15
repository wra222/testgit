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
public partial class FA_CheckAst : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String picLoc = System.Configuration.ConfigurationManager.AppSettings["RDS_MVS_ESOP"];

    ICheckAst Icheckast = ServiceAgent.getInstance().GetObjectByName<ICheckAst>(WebConstant.CheckAST);

    private const int DEFAULT_ROWS = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
           // btnUpdateUI.ServerClick += new EventHandler(btnUpdateUI_ServerClick);
            btnExit.ServerClick += new EventHandler(btnExit_ServerClick);
            btnComplete.ServerClick += new EventHandler(btnComplete_ServerClick);
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {
                InitLabel();
                clearGrid();
                setColumnWidth();

                this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                this.hidStation.Value = Request["Station"];
                //this.cmbPdLine.Stage = "FA";
                this.upHidden.Update();
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

    protected void gd_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!String.IsNullOrEmpty(e.Row.Cells[2].Text.Trim()) && e.Row.Cells[2].Text.Trim().Length > 15)
            {
                e.Row.Cells[2].ToolTip = e.Row.Cells[2].Text;
            }
        }
    }

    /// <summary>
    /// Update UI while PdLine changed.
    /// </summary>
    /// <param name="er"></param>
    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        setProductInfo("", "");
        hidWantData.Value = "0";
        hidProdId.Value = "";
        hidInput.Value = "";
        this.upHidden.Update();

        string line = cmbPdLine.InnerDropDownList.SelectedValue;
        if (line == "")
        {
            showInfo("");
            //setSumCounts("", "", "");
            setSumCounts("", "");
            clearGrid();
        }
        else
        {
            showInfo("");
            beginWaitingCoverDiv();
            clearGrid();
            showQCStatics("");
            //showQCStatics(line);
            endWaitingCoverDiv();
        }

        callNextInput();
    }

    private void btnExit_ServerClick(object sender, System.EventArgs e)
    {
        Icheckast.Cancel(hidProdId.Value);
    }

    private void btnComplete_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string wc = "";
            string setMsg = "";
            Icheckast.Save(hidProdId.Value);
            hidWantData.Value = "0";
            //showQCMethod(method);
            setProductInfo("", "");
            clearGrid();
            clearESOP();
           // showQCStatics(wc);
            //showQCStatics(cmbPdLine.InnerDropDownList.SelectedValue);
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            this.upHidden.Update();
            endWaitingCoverDiv();
            enableCombox();
            callNextInput();
        }
    }

    //private void btnUpdateUI_ServerClick(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        showESOP(iAFTMVS.GetESOPList(hidModel.Value));
    //        showSuccessInfo(this.GetLocalResourceObject(Pre + "_msgInputCSNorAst").ToString());
    //    }
    //    catch (FisException ee)
    //    {
    //        showInfo(ee.mErrmsg);
    //        if (ee.mErrcode == "CHK517")
    //        {
    //            writeToAlertMessage(ee.mErrmsg);
    //            hidWantData.Value = "0";
    //            setProductInfo("", "");
    //            clearGrid();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        writeToAlertMessage(ex.Message);
    //    }
    //    finally
    //    {
    //        this.upHidden.Update();
    //        endWaitingCoverDiv();
    //        if (hidWantData.Value == "1") disableCombox();
    //        callNextInput();
    //    }
    //}

    private void clearESOP()
    {
        this.Img1.Src = "";
        this.Img1.Alt = "(No ESOP)";
        this.Img2.Src = "";
        this.Img2.Alt = "(No ESOP)";
        this.upESOP.Update();
    }

    private void showESOP(IList<string> pics)
    {
        if (!picLoc.EndsWith("/") && !picLoc.EndsWith("\\"))
        {
            picLoc += "/";
        }

        if (pics.Count > 0)
        {
            this.Img1.Src = picLoc + pics[0];
            this.Img1.Alt = pics[0];
        }
        else
        {
            this.Img1.Src = "";
            this.Img1.Alt = "(No ESOP)";
        }

        if (pics.Count > 1)
        {
            this.Img2.Src = picLoc + pics[1];
            this.Img2.Alt = pics[1];
        }
        else
        {
            this.Img2.Src = "";
            this.Img2.Alt = "(No ESOP)";
        }

        //if (pics.Count > 2)
        //{
        //    this.Img3.Src = picLoc + pics[2];
        //    this.Img3.Alt = pics[2];
        //}
        //else
        //{
        //    this.Img3.Src = "";
        //    this.Img3.Alt = "(No ESOP)";
        //}

        //if (pics.Count > 3)
        //{
        //    this.Img4.Src = picLoc + pics[3];
        //    this.Img4.Alt = pics[3];
        //}
        //else
        //{
        //    this.Img4.Src = "";
        //    this.Img4.Alt = "(No ESOP)";
        //}

        this.upESOP.Update();
    }

    /*
     * Answer to: ITC-1360-1127
     * Description: Show QC statics for this time.
     */
    private void showQCStatics(string wc)
    {
        string str = txtEpiaCnt.Text.Trim();
        int cntEPIA = (str == "") ? 0 : int.Parse(str);
        str = txtPassCnt.Text.Trim();
        int cntPass = (str == "") ? 0 : int.Parse(str);

        if (wc == "73" || wc == "6A")
        {
            cntEPIA++;
        }
        else if (wc == "79")
        {
            cntPass++;
        }

        setSumCounts(cntPass.ToString(), cntEPIA.ToString());
    }

    /*
    private void showQCStatics(string line)
    {
        try
        {
            int passCnt = 0;
            int piaCnt = 0;
            int epiaCnt = 0;

            iAFTMVS.GetQCStatics(line, out piaCnt, out epiaCnt, out passCnt);

            //setSumCounts(passCnt.ToString(), piaCnt.ToString(), epiaCnt.ToString());
            setSumCounts(passCnt.ToString(), epiaCnt.ToString());
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
    */

    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbPassCnt.Text = this.GetLocalResourceObject(Pre + "_lbPassCnt").ToString();
        //this.lblPiaCnt.Text = this.GetLocalResourceObject(Pre + "_lblPiaCnt").ToString();
        this.lblEpiaCnt.Text = this.GetLocalResourceObject(Pre + "_lblEpiaCnt").ToString();
        this.lblProId.Text = this.GetLocalResourceObject(Pre + "_lblProId").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblTableTitle.Text = this.GetLocalResourceObject(Pre + "_lblTableTitle").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();

        setPdLineCombFocus();
    }

    private void setProductInfo(string id, string model)
    {
        try
        {
            this.txtProId.Text = id;
            this.UpdatePanel4.Update();
            this.txtModel.Text = model;
            this.UpdatePanel5.Update();
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

    //private void setSumCounts(string passCnt, string piaCnt, string epiaCnt)
    private void setSumCounts(string passCnt, string epiaCnt)
    {
        try
        {
            this.txtPassCnt.Text = passCnt;
            //this.txtPiaCnt.Text = piaCnt;
            this.txtEpiaCnt.Text = epiaCnt;
            UpdatePanel1.Update();
            //UpdatePanel2.Update();
            UpdatePanel3.Update();
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

    private void clearGrid()
    {
        try
        {
            bindTable(DEFAULT_ROWS);
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

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void enableCombox()
    {
        String script = "<script language='javascript'> getPdLineCmbObj().disabled = false; </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "enableCombox", script, false);
    }

    private void disableCombox()
    {
        String script = "<script language='javascript'> getPdLineCmbObj().disabled = true; </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "disableCombox", script, false);
    }

    private void callNextInput()
    {
        String script = "<script language='javascript'>callNextInput();</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callNextInput", script, false);
    }

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void showSuccessInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\", \"green\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void showQCMethod(string method)
    {
        String script = "<script language='javascript'> showQCMethod(\"" + method.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "showQCMethodAgent", script, false);
    }

    private void setPdLineCombFocus()
    {

        String script = "<script language='javascript'> setPdLineCmbFocus(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCollection").ToString());

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
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(38);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(26);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(16);
    }
}

