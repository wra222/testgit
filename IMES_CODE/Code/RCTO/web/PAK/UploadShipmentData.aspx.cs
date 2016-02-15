/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for UploadShipmentData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Upload Shipment Data to SAP.docx –2011/10/26 
 * UC:CI-MES12-SPEC-PAK-UC Upload Shipment Data to SAP.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
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
using System.IO;


[Serializable]
public partial class PAK_UploadShipmentData : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    IUploadShipmentData iUploadShipmentData = ServiceAgent.getInstance().GetObjectByName<IUploadShipmentData>(WebConstant.UploadShipmentDataObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnQueryData.ServerClick += new EventHandler(btnQueryData_ServerClick);
            btnSaveData.ServerClick += new EventHandler(btnSaveData_ServerClick);
            btnComplete.ServerClick += new EventHandler(btnComplete_ServerClick);
            GridViewExt1.DataBound +=new EventHandler(GridViewExt1_DataBound);
            if (!Page.IsPostBack)
            {        
                InitLabel();
                clearGrid();
                hidGUID.Value = Guid.NewGuid().ToString().Replace("-", "");
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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[9].Style.Add("display", "none");  
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelect = e.Row.FindControl("chk") as CheckBox;
            DataRowView drv = e.Row.DataItem as DataRowView;
            string id = drv["DN"].ToString();
            chkSelect.Attributes.Add("onclick", string.Format("setSelectVal(this,'{0}');", id));
        }
    } 

    private void GridViewExt1_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < GridViewExt1.Rows.Count; i++)
        {
            if (GridViewExt1.Rows[i].Cells[1].Text.Trim().Equals("&nbsp;")) //No data
            {
                GridViewExt1.Rows[i].Cells[0].Controls[1].Visible = false;
            }
            /*
            * Answer to: ITC-1360-1781
            * Description: UC updated(see below contents).
            */
            //(5/1)状态不为88 和 98 的记录，也不允许被选择
            //(5/1)当PAQC 显示为”PQ” 时，则该条记录也不能被选择
            /*
            * Answer to: ITC-1360-1841
            * Description: UC updated(2012-06-01).
            */
            //是否Enable Checkbox的逻辑放在服务中实现，此处仅用标志位进行判断
            if (GridViewExt1.Rows[i].Cells[9].Text.Trim() == "0")
            {
                ((CheckBox)GridViewExt1.Rows[i].FindControl("chk")).Enabled = false;
            }
        }
    }

    private void btnComplete_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string dns = hidSelectId.Value; //"id1,id2,id3...idn,"
            if (dns.EndsWith(",")) dns = dns.Remove(dns.Length - 1);   //Remove the last character ','
            IList<string> dnList = dns.Split(',');
            iUploadShipmentData.ChangeDNStatus(dnList);
        }
        catch (FisException ee)
        {
            clearGrid();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message.Replace("\\", "\\\\"));
        }
        finally
        {
            reQuery();
            endWaitingCoverDiv();
        }
    }

    private void btnSaveData_ServerClick(object sender, System.EventArgs e)
    {
        bool bNeedRequery = true;
        try
        {
            string dns = hidSelectId.Value; //"id1,id2,id3...idn,"
            if (dns.EndsWith(",")) dns = dns.Remove(dns.Length - 1);   //Remove the last character ','
            IList<string> dnList = dns.Split(',');
            IList<string> lines = iUploadShipmentData.GetFileData(dnList);
            //写文件
            string dtsFtpPath = ConfigurationManager.AppSettings["DTSFtpPath"].ToString();
            if (!dtsFtpPath.EndsWith("\\"))
            {
                dtsFtpPath = dtsFtpPath + "\\";
            }
            FileStream fs = new FileStream(dtsFtpPath + hidGUID.Value + "-tmp.TXT", FileMode.Create);
            if (fs == null)
            {
                writeToAlertMessage(GetLocalResourceObject(Pre + "_msgCheckAccess").ToString());
                return;
            }

            StreamWriter sw = new StreamWriter(fs);
            foreach (string tmp in lines)
            {
                sw.WriteLine(tmp);
            }
            sw.Flush();
            sw.Close();
            fs.Close();
            bNeedRequery = false;
            transFile();            
        }
        catch (FisException ee)
        {
            clearGrid();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message.Replace("\\", "\\\\"));
        }
        finally
        {
            if (bNeedRequery) reQuery();
            endWaitingCoverDiv();
        }
    }

    private void btnQueryData_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string fromDate = hidFromDate.Value;
            string toDate = hidToDate.Value;

            /*
             * Answer to: ITC-1360-1588
             * Description: Not allow clear date.
             */
            /*
            if ((fromDate == null || fromDate == "") && (toDate != null && toDate != ""))
            {
                fromDate = toDate;
            }

            if ((fromDate != null && fromDate != "") && (toDate == null || toDate == ""))
            {
                toDate = fromDate;
            }
            */

            bool bAllData = chkAllData.Checked;

            DateTime from = Convert.ToDateTime(fromDate);
            DateTime to = Convert.ToDateTime(toDate);

            if (from > to)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgBadDate").ToString());
                clickDateFrom();
                return;
            }
            
            IList<S_RowData_UploadShipmentData> info = iUploadShipmentData.GetTableData(from, to, bAllData);
            DataTable dt = initTable();
            DataRow newRow;

            int cnt = 0;
            if (info != null)
            {
                foreach (S_RowData_UploadShipmentData ele in info)
                {
                    newRow = dt.NewRow();
                    newRow["Date"] += ele.m_date.ToString("yyyy-MM-dd");
                    newRow["DN"] += ele.m_dn;
                    newRow["PN"] += ele.m_pn;
                    newRow["Model"] += ele.m_model;
                    newRow["Qty"] += ele.m_qty.ToString();
                    newRow["Status"] += ele.m_status;
                    newRow["Pack"] += ele.m_pack.ToString();
                    newRow["PAQC"] += ele.m_paqc;
                    newRow["Flag"] += (ele.m_bAllowUpload ? "1" : "0");
                    dt.Rows.Add(newRow);
                    cnt++;
                }
            }

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
            clearGrid();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    private void InitLabel()
    {
        this.lblFrom.Text = this.GetLocalResourceObject(Pre + "_lblFrom").ToString();
        this.lblTo.Text = this.GetLocalResourceObject(Pre + "_lblTo").ToString();
        this.chkAllData.Text = this.GetLocalResourceObject(Pre + "_chkAllData").ToString();    
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
        retTable.Columns.Add("Chk");
        retTable.Columns.Add("Date", Type.GetType("System.String"));
        retTable.Columns.Add("DN", Type.GetType("System.String"));
        retTable.Columns.Add("PN", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("Pack", Type.GetType("System.Int32"));
        retTable.Columns.Add("PAQC", Type.GetType("System.String"));
        retTable.Columns.Add("Flag");
        return retTable;
    }

    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = "";
        this.GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleDate").ToString();
        this.GridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titleDN").ToString();
        this.GridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titlePN").ToString();
        this.GridViewExt1.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_titleModel").ToString();
        this.GridViewExt1.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_titleQty").ToString();
        this.GridViewExt1.HeaderRow.Cells[6].Text = this.GetLocalResourceObject(Pre + "_titleStatus").ToString();
        this.GridViewExt1.HeaderRow.Cells[7].Text = this.GetLocalResourceObject(Pre + "_titlePack").ToString();
        this.GridViewExt1.HeaderRow.Cells[8].Text = this.GetLocalResourceObject(Pre + "_titlePAQC").ToString();
        this.GridViewExt1.HeaderRow.Cells[9].Text = "";
        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(70);
        this.GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(90);
        this.GridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(30);
        this.GridViewExt1.HeaderRow.Cells[6].Width = Unit.Pixel(30);
        this.GridViewExt1.HeaderRow.Cells[7].Width = Unit.Pixel(30);
        this.GridViewExt1.HeaderRow.Cells[8].Width = Unit.Pixel(30);
        this.GridViewExt1.HeaderRow.Cells[9].Width = Unit.Pixel(0);
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

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void transFile()
    {
        String script = "<script language='javascript'> downloadFile(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "transFile", script, false);
    }

    private void clickDateFrom()
    {
        String script = "<script language='javascript'>document.getElementById('btnFrom').click();</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "clickDateFrom", script, false);
    }

    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "jsAlert", script, false);
    }

    private void reQuery()
    {
        String script = "<script language='javascript'>\n"
                        + "ctlSelectedId.value = \"\";\n"
                        + "document.getElementById('" + btnQueryData.ClientID + "').click();\n"
                        + "</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "reQuery", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

}
