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
using System.IO;


public partial class PAK_UpdateConsolidate : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string filename = "";
    public int fullRowCount = 10;
    public String UserId;
    public String Customer;
    public String Station;

    IUpdateConsolidate iUpdateConsolidate = ServiceAgent.getInstance().GetObjectByName<IUpdateConsolidate>(WebConstant.UpdateConsolidateObject);


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                Station = Request["Station"];

                //DataTable retTable = new DataTable();
                //retTable = iUpdateConsolidate.GetAbnormalConsolidate("");
                bindTable(null);
                setColumnWidth();
                InitLabel();
            }

            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
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

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }


    private void bindTable(DataTable dt1)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colShipment").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDelivery").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPallet").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colModel").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colConsolidate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colConQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colActConQty").ToString());
        if (dt1 != null && dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dr = dt.NewRow();
                dr[0] = dt1.Rows[i][0];
                dr[1] = dt1.Rows[i][1];
                dr[2] = dt1.Rows[i][2];
                dr[3] = dt1.Rows[i][3];
                dr[4] = dt1.Rows[i][4];
                dr[5] = dt1.Rows[i][5];
                dr[6] = dt1.Rows[i][6];

                dt.Rows.Add(dr);
            }
            for (int j = dt.Rows.Count; j < fullRowCount; j++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < fullRowCount; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Pixel(60);
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(60);
        gd.HeaderRow.Cells[2].Width = Unit.Pixel(60);
        gd.HeaderRow.Cells[3].Width = Unit.Pixel(60);
        gd.HeaderRow.Cells[4].Width = Unit.Pixel(60);
        gd.HeaderRow.Cells[5].Width = Unit.Pixel(25);
        gd.HeaderRow.Cells[6].Width = Unit.Pixel(25);
    }



    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }

    private void InitLabel()
    {
        this.lblInput.Text = this.GetLocalResourceObject(Pre + "_lblInput").ToString();
        this.lblDelivery.Text = this.GetLocalResourceObject(Pre + "_lblDelivery").ToString();
        this.lblPallet.Text = this.GetLocalResourceObject(Pre + "_lblPallet").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblShipment.Text = this.GetLocalResourceObject(Pre + "_lblShipment").ToString();
        this.lblConQty.Text = this.GetLocalResourceObject(Pre + "_lblConQty").ToString();
        this.lblActConQty.Text = this.GetLocalResourceObject(Pre + "_lblActConQty").ToString();
        this.lblConsolidate.Text = this.GetLocalResourceObject(Pre + "_lblConsolidate").ToString();
        this.lblField.Text = this.GetLocalResourceObject(Pre + "_lblField").ToString();
        this.btnQuery.Value = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnUpdate.Value = this.GetLocalResourceObject(Pre + "_btnUpdate").ToString();
        this.btnClear.Value = this.GetLocalResourceObject(Pre + "_btnClear").ToString();
    }

    
    
    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            bindTable(null);
            setColumnWidth();
            endWaitingCoverDiv();
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

    protected void ConsolidateQuery(object sender, System.EventArgs e)
    {
        try
        {
            DataTable retTable = new DataTable();
            retTable = iUpdateConsolidate.GetAbnormalConsolidate(this.txtInput.Value.Trim());
            bindTable(retTable);
            setColumnWidth();
            endWaitingCoverDiv();
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
}