/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for ChangeKeyParts Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Change Key Parts.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Change Key Parts.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
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

public partial class FA_ChangeKeyParts : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    IChangeKeyParts iChangeKeyParts = ServiceAgent.getInstance().GetObjectByName<IChangeKeyParts>(WebConstant.ChangeKeyPartsObject);
    ITestStation iTS = ServiceAgent.getInstance().GetObjectByName<ITestStation>(WebConstant.CommonObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            cmbKPT.InnerDropDownList.AutoPostBack = true;
            cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_SelectedIndexChanged);
            cmbKPT.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_SelectedIndexChanged);
            btnReset.ServerClick += new EventHandler(btnReset_ServerClick);
            btnGridFresh.ServerClick += new EventHandler(btnGridFresh_ServerClick);
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {
                InitLabel();
                clearGrid();

                this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                this.cmbPdLine.Stage = "FA";
                this.hidStation.Value = Request["Station"];
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
            if (!String.IsNullOrEmpty(e.Row.Cells[1].Text.Trim()) && !(e.Row.Cells[1].Text.Trim().ToLower() == "&nbsp;"))
            {
                e.Row.Cells[1].ToolTip = e.Row.Cells[1].Text;
            }

            if (!String.IsNullOrEmpty(e.Row.Cells[2].Text.Trim()) && !(e.Row.Cells[2].Text.Trim().ToLower() == "&nbsp;"))
            {
                e.Row.Cells[2].ToolTip = e.Row.Cells[2].Text;
            }
        }
    }

    private void cmbPdLine_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        setProductInfo("", "", "", "");
        hidWantData.Value = "0";
        this.upHidden.Update();
        showInfo("");
        clearGrid();
        callInputRun();
    }

    private void btnReset_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            hidWantData.Value = "0";
            iChangeKeyParts.Save(txtProId.Text);
            showSuccessInfo(txtProId.Text);
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
            setProductInfo("", "", "", "");
            clearGrid();
            endWaitingCoverDiv();
            enableCombox();
            callInputRun();
        }
    }

    private void btnGridFresh_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            ProductInfo pInfo = new ProductInfo();
            string wc = "";
            IList<BomItemInfo> BOMlist = iChangeKeyParts.GetTableData(hidInput.Value, hidKpt.Value, hidLine.Value, UserId, hidStation.Value, Customer, out pInfo, out wc);
            setProductInfo(pInfo.id, pInfo.modelId, pInfo.customSN, wc);

            DataTable dt = initTable();
            DataRow newRow;

            int cnt = 0;
            if (BOMlist != null && BOMlist.Count > 0)
            {
                hidWantData.Value = "1";
                foreach (BomItemInfo ele in BOMlist)
                {
                    /*
                     * Answer to: ITC-1360-1698
                     * Description: Use PartNoItem value instead of get it by each part.
                    */
                    /*
                    string str = "";
                    foreach (PartNoInfo pni in ele.parts)
                    {
                        foreach (NameValueInfo nvi in pni.properties)
                        {
                            if (nvi.Name == "VendorCode")
                            {
                                str += nvi.Value + ",";
                            }
                        }
                    }*/

                    newRow = dt.NewRow();
                    newRow["Type"] += ele.type;
                    newRow["Desc"] += ele.description;
                    newRow["Name"] += ele.PartNoItem;// (str.EndsWith(",") ? str.Remove(str.Length - 1) : str);
                    newRow["Qty"] += ele.qty.ToString();
                    newRow["PQty"] += "0";
                    newRow["Collection"] += "";
                    dt.Rows.Add(newRow);
                    cnt++;
                }
            }
            else
            {
                hidWantData.Value = "0";
                iChangeKeyParts.Save(txtProId.Text);
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
        }
        catch (FisException ee)
        {
            setProductInfo("", "", "", "");
            clearGrid();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            setProductInfo("", "", "", "");
            clearGrid();
            writeToAlertMessage(ex.Message.Replace("\\","\\\\"));
        }
        finally
        {
            this.upHidden.Update();
            endWaitingCoverDiv();
            if (hidWantData.Value == "1") disableCombox();
            callInputRun();
        }
    }

    private void InitLabel()
    {
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblKeyPart.Text = this.GetLocalResourceObject(Pre + "_lblKeyPart").ToString();
        this.lblRetutnWC.Text = this.GetLocalResourceObject(Pre + "_lblRetutnWC").ToString();
        this.lblProId.Text = this.GetLocalResourceObject(Pre + "_lblProId").ToString();
        this.lblCTSN.Text = this.GetLocalResourceObject(Pre + "_lblCTSN").ToString();
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
        retTable.Columns.Add("Type", Type.GetType("System.String"));
        retTable.Columns.Add("Desc", Type.GetType("System.String"));
        retTable.Columns.Add("Name", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        retTable.Columns.Add("PQty", Type.GetType("System.Int32"));
        retTable.Columns.Add("Collection", Type.GetType("System.String"));
        return retTable;
    }

    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titleType").ToString();
        this.GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleDesc").ToString();
        this.GridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titleName").ToString();
        this.GridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titleQty").ToString();
        this.GridViewExt1.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_titlePQty").ToString();
        this.GridViewExt1.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_titleCollection").ToString();
        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(35);
        this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(70);
        this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(15);
        this.GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(20);
        this.GridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(250);
    }

    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            clearGrid();
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

    private void setProductInfo(string id, string model, string cust, string wc)
    {
        try
        {
            this.txtProId.Text = id;
            this.upProId.Update();
            this.txtModel.Text = model;
            this.upModel.Update();
            this.txtCTSN.Text = cust;
            this.upCTSN.Update();
            /*
             * Answer to: ITC-1360-0557
             * Description: display description of test station on UI.
            */
            this.txtRetWC.Text = wc + " " + iTS.GeStationDescr(wc);
            this.upRetWC.Update();
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
            this.gridViewUP.Update();
            initTableColumnHeader();
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

    /*
     * Answer to: ITC-1360-0921
     * Description: Disable pdLine/KP combox after input productID,
     *              and enable them after sample complete or page reset.
     */
    private void enableCombox()
    {
        String script = "<script language='javascript'> getPdLineCmbObj().disabled = false;\n getChangeKPTypeCmbObj().disabled = false; </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "enableCombox", script, false);
    }

    private void disableCombox()
    {
        String script = "<script language='javascript'> getPdLineCmbObj().disabled = true;\n getChangeKPTypeCmbObj().disabled = true; </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "disableCombox", script, false);
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

    private void callInputRun()
    {
        String script = "<script language='javascript'>callNextInput(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callInputRun", script, false);
    }

    private void showInfo(string info)
    {

        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void showSuccessInfo(string id)
    {
        String script = "<script language='javascript'> ShowSuccessfulInfoFormat(true, \"ProductID\", \"" + id + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void setPdLineCombFocus()
    {

        String script = "<script language='javascript'>  setPdLineCmbFocus(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
}
