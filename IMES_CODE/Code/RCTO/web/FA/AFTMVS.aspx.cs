/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for AFTMVS Page
 *             
 * UI:CI-MES12-SPEC-FA-UI AFT MVS.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC AFT MVS.docx –2011/10/25            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-28  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

/*
 * Answer to: ITC-1360-0026
 * Description: Use data struct defined in datamodel instead of fisObject.
 */
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

public partial class FA_AFTMVS : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String picLoc = System.Configuration.ConfigurationManager.AppSettings["RDS_MVS_ESOP"];

    IAFTMVS iAFTMVS = ServiceAgent.getInstance().GetObjectByName<IAFTMVS>(WebConstant.AFTMVSObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
          
        {
            cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            btnProcess.ServerClick += new EventHandler(btnProcess_ServerClick);
            btnExit.ServerClick += new EventHandler(btnExit_ServerClick);
            btnComplete.ServerClick += new EventHandler(btnComplete_ServerClick);
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {
        
                InitLabel();
                clearGrid();

                this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                this.hidStation.Value = Request["Station"];
                this.cmbPdLine.Stage = "FA";
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
        iAFTMVS.Cancel(hidProdId.Value);
    }

    private void btnComplete_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string wc = "";
            string method = iAFTMVS.GetQCMethod(hidProdId.Value, out wc);
            hidWantData.Value = "0";
            showQCMethod(method);
            setProductInfo("", "");
            clearGrid();
            clearESOP();
            showQCStatics(wc);
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

    private void btnProcess_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            if (hidWantData.Value == "0")   //Input product ID
            {
                string id;
                string model;
                IList<BomItemInfo> bom = iAFTMVS.InputProductIDorCustSN(hidInput.Value, cmbPdLine.InnerDropDownList.SelectedValue, UserId, hidStation.Value, Customer, out id, out model);
                setProductInfo(id, model);
                hidProdId.Value = id;
                hidInput.Value = "";
                showTableData(bom);
                showESOP(iAFTMVS.GetESOPList(model));
                hidWantData.Value = "1";
                showSuccessInfo(this.GetLocalResourceObject(Pre + "_msgInputCSNorAst").ToString());
                /*
                if (bom.Count != 0)
                {
                    showTableData(bom);
                    hidWantData.Value = "1";
                    showInfo(this.GetLocalResourceObject(Pre + "_msgInputAst").ToString());
                }
                else
                {
                    string wc = "";
                    string method = iAFTMVS.GetQCMethod(out wc);
                    showQCMethod(method);
                    /*
                     * Answer to: ITC-1360-0942
                     * Description: Clear product info on complete.
                     /
                    setProductInfo("", "");
                    clearGrid();
                    showQCStatics(wc);
                    //showQCStatics(cmbPdLine.InnerDropDownList.SelectedValue);
                }
                */
            }
            else    //Input AST label
            {
                string pn = iAFTMVS.InputASTLabel(hidProdId.Value, hidInput.Value);
                updateTable(pn, hidInput.Value);
                hidInput.Value = "";
            }
        }
        catch (FisException ee)
        {
            showInfo(ee.mErrmsg);
            if (ee.mErrcode == "CHK517")
            {
                writeToAlertMessage(ee.mErrmsg);
                hidWantData.Value = "0";
                setProductInfo("", "");
                clearGrid();
            }
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            this.upHidden.Update();
            endWaitingCoverDiv();
            if (hidWantData.Value == "1") disableCombox();
            callNextInput();
        }
    }

    private void clearESOP()
    {
        this.Img1.Src = "";
        this.Img1.Alt = "(No ESOP)";
        this.Img2.Src = "";
        this.Img2.Alt = "(No ESOP)";
        this.Img3.Src = "";
        this.Img3.Alt = "(No ESOP)";
        this.Img4.Src = "";
        this.Img4.Alt = "(No ESOP)";
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

        if (pics.Count > 2)
        {
            this.Img3.Src = picLoc + pics[2];
            this.Img3.Alt = pics[2];
        }
        else
        {
            this.Img3.Src = "";
            this.Img3.Alt = "(No ESOP)";
        }

        if (pics.Count > 3)
        {
            this.Img4.Src = picLoc + pics[3];
            this.Img4.Alt = pics[3];
        }
        else
        {
            this.Img4.Src = "";
            this.Img4.Alt = "(No ESOP)";
        }

        this.upESOP.Update();
    }

    private void showTableData(IList<BomItemInfo> info)
    {
        DataTable dt = initTable();
        DataRow newRow;

        int cnt = 0;
        if (info != null)
        {
            foreach (BomItemInfo ele in info)
            {
                /*
                 * Answer to: ITC-1360-0545
                 * Description: Wrong column name input, fixed.
                 */
                newRow = dt.NewRow();
                newRow["PartType"] += ele.type;
                newRow["Descr"] += ele.description;
                if (ele.tp == "PP" || ele.tp == "ATSN2")
                {
                    string thisPN = "";
                    for (int idx = 0; idx < ele.PartNoItem.Length; idx++)
                    {
                        if (idx <= 4) thisPN += ele.PartNoItem[idx];
                        else thisPN += "*";
                    }
                    newRow["PartNo"] += thisPN;
                }
                else
                {
                    newRow["PartNo"] += ele.PartNoItem;
                }
                newRow["Qty"] += ele.qty.ToString();
                newRow["PQty"] += "0";
                newRow["ColData"] += "";
                dt.Rows.Add(newRow);
                cnt++;
            }
        }
        hidRowCnt.Value = cnt.ToString();

        for (; cnt < initRowsCount; cnt++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        this.GridViewExt1.DataSource = dt;
        this.GridViewExt1.DataBind();
        initTableColumnHeader();
        gridViewUP.Update();
        return;
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

    private DataTable getNullDataTable()
    {
      
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("PartType", Type.GetType("System.String"));
        retTable.Columns.Add("Descr", Type.GetType("System.String"));
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        retTable.Columns.Add("PQty", Type.GetType("System.Int32"));
        retTable.Columns.Add("ColData", Type.GetType("System.String"));
        return retTable;
    }

    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titlePartType").ToString();
        this.GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleDescr").ToString();
        this.GridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titlePartNo").ToString();
        this.GridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titleQty").ToString();
        this.GridViewExt1.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_titlePQty").ToString();
        this.GridViewExt1.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_titleColData").ToString();
        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(65);
        this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(65);
        this.GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(80);
        this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(20);
        this.GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(20);
        this.GridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(150);
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
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            initTableColumnHeader();
            gridViewUP.Update();
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

    /*
     * Answer to: ITC-1360-1063
     * Description: Missing quoters when call js function.
     */
    private void updateTable(string pn, string ast)
    {
        String script = "<script language='javascript'> processTableUpdate(\"" + pn + "\", \"" + ast + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "updateTable", script, false);
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
}
