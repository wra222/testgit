
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;


public partial class FA_FARepair : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 6;
    private IFARepair iFARepair;
    private IRepair iRepairLog;
    private Object commServiceObj;
    private Object repairServiceObj;
    private IProduct iProduct;
    private ITestStation iTestStation;
    public String UserId;
    public String Customer;
    private int hidCol = 8;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(null, DEFAULT_ROWS, null);
                setColumnWidth();
                //CmbPdLine.Station = Request["Station"];
                CmbPdLine.Station = Request["Station"];
                //CmbPdLine.Stage = "FA";
                CmbPdLine.Customer = Master.userInfo.Customer;              
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                refreshCmbReturnStation(null, "0");
            }

            repairServiceObj = ServiceAgent.getInstance().GetObjectByName<IFARepair>(WebConstant.FARepairObject);
            commServiceObj = ServiceAgent.getInstance().GetObjectByName<IRepair>(WebConstant.CommonObject);

            iFARepair = (IFARepair)repairServiceObj;
            iRepairLog = (IRepair)commServiceObj;
            iProduct = (IProduct)commServiceObj;
            iTestStation = (ITestStation)commServiceObj;
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS, null);
            this.hidRecordCount.Value = "0";
            refreshCmbReturnStation(null, "0");
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS, null);
            this.hidRecordCount.Value = "0";
            refreshCmbReturnStation(null, "0");
        }
    }

    private void initLabel()
    {
        this.lblPdLine.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
        //this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnEdit.InnerText = this.GetLocalResourceObject(Pre + "_btnEdit").ToString();
        //this.btnDelete.InnerText = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.lblProdid.Text = this.GetLocalResourceObject(Pre + "_lblProdid").ToString();
        this.lblRepairLog.Text = this.GetLocalResourceObject(Pre + "_lblRepairLog").ToString();
        this.lblTestStation.Text = this.GetLocalResourceObject(Pre + "_lblTestStation").ToString();
        this.lblReturnStation.Text = this.GetLocalResourceObject(Pre + "_lblReturnStation").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[2].Width = Unit.Pixel(40);
        gd.HeaderRow.Cells[7].Width = Unit.Pixel(60);
    }

    protected void hidbtn_ServerClick(Object sender, EventArgs e)
    {
        string prodId = this.hidProdId.Value.Trim();
        string PdLine = this.hidPdLine.Value.Trim();
        ProductStatusInfo statusInfo;
        IList<RepairInfo> logList;
        logList = null;
        ProductInfo info;
        int ret;

        try
        {
            string editor = Master.userInfo.UserId;
            string station = Request["Station"];
            //string station = "45";
            string customer = Master.userInfo.Customer;

            info = iProduct.GetProductInfo(prodId);

            // mantis 1578
           // if (info.modelId.IndexOf("173") == 0)
           // {
           //     throw new Exception(this.GetLocalResourceObject(Pre + "_err173Model").ToString());
           // }

            statusInfo = iProduct.GetProductStatusInfo(prodId);
            iFARepair.InputProdId(statusInfo.pdLine, prodId, editor, station, customer);
            
            this.lblTestStationContent.Text = statusInfo.station + " " + iTestStation.GeStationDescr(statusInfo.station);
            this.lblModelContent.Text = info.modelId;
            this.hidMac.Value = iFARepair.GetProductMac(prodId);
            logList = iRepairLog.GetProdRepairList(prodId);            
            if (logList == null || logList.Count == 0)
            {                
                bindTable(null, DEFAULT_ROWS, statusInfo.station);
                refreshCmbReturnStation(null, "0");
            }
            else
            {
                ret = bindTable(logList, DEFAULT_ROWS, statusInfo.station);
                if (ret == 1)
                {
                    refreshCmbReturnStation(prodId, "0");
                }
            }
            inputFinish();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS, null);
            clearLabel();
            this.hidRecordCount.Value = "0";
            refreshCmbReturnStation(null, "0");
            hideWait();
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS, null);
            clearLabel();
            this.hidRecordCount.Value = "0";
            refreshCmbReturnStation(null, "0");
            hideWait();
        }
    }

    protected void hidRefresh_ServerClick(Object sender, EventArgs e)
    {
        IList<RepairInfo> logList;
        ProductInfo info;
        ProductStatusInfo statusInfo;
        string prodId = this.hidProdId.Value.Trim();
        info = iProduct.GetProductInfo(prodId);
        statusInfo = iProduct.GetProductStatusInfo(prodId);
        this.hidMac.Value = iFARepair.GetProductMac(prodId);
        int ret;

        try
        {
            logList = iRepairLog.GetProdRepairList(prodId);

            if (logList == null || logList.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS, statusInfo.station);
                refreshCmbReturnStation(null, "0");
            }
            else
            {
                ret = bindTable(logList, DEFAULT_ROWS, statusInfo.station);
                if (ret == 1)
                {
                    refreshCmbReturnStation(prodId, "0");
                }
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS, null);
            this.hidRecordCount.Value = "0";
            refreshCmbReturnStation(null, "0");
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS, null);
            this.hidRecordCount.Value = "0";
            refreshCmbReturnStation(null, "0");
        }
    }

    private void refreshCmbReturnStation(string prodid, string status)
    {
        if (!string.IsNullOrEmpty(prodid))
        {
            IList<StationInfo> returnList = null;
            returnList = iFARepair.GetReturnStationList(prodid, Convert.ToInt32(status));
            if (returnList != null && returnList.Count > 0)
            {
                this.CmbReturnStation.clearContent();
                foreach (StationInfo temp in returnList)
                {
                    ListItem item = null;
                    item = new ListItem(temp.Descr, temp.StationId);
                    this.CmbReturnStation.InnerDropDownList.Items.Add(item);                   
                }

                if (returnList.Count == 1)
                {
                    this.CmbReturnStation.setSelected(1);
                }
            }
            else
            {
                this.CmbReturnStation.clearContent();
            }
        }
        else
        {
            this.CmbReturnStation.clearContent();
        }
    }

    private int bindTable(IList<RepairInfo> list, int defaultRow, string station)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        int ret = 0;

        dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPdLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colTestStn").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDefect").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCause").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCreateDate").ToString());        
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUpdateDate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colReturnStation").ToString());
        dt.Columns.Add("hideCol");

        if (list != null && list.Count != 0 && !string.IsNullOrEmpty(station))
        {
            foreach (RepairInfo temp in list)
            {                
                dr = dt.NewRow();
                dr[0] = temp.isManual;
                dr[1] = temp.pdLine;
                dr[2] = temp.testStation;
                dr[3] = temp.defectCodeID + " " + temp.defectCodeDesc;
                // mantis 1578
                //dr[4] = temp.cause + " " + temp.causeDesc;
                dr[4] = temp.majorPart;
                dr[5] = temp.cdt;
                dr[6] = temp.udt;
                if (temp.returnStation == null || temp.returnStation == "")
                {
                    dr[7] = "";
                    //IList<DefectCodeStationInfo> returnList = new List<DefectCodeStationInfo>();
                    //returnList = iFARepair.GetReturnStation(temp.Identity, temp.defectCodeID, station, temp.cause);
                    //dr[7] = returnList[0].nxt_stn;
                    //temp.returnStation = returnList[0].nxt_stn;
                }
                else
                {
                    dr[7] = temp.returnStation;
                }
                dr[8] = getHideColumn(temp);

                dt.Rows.Add(dr);
            }
            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            this.hidRecordCount.Value = list.Count.ToString();
            ret = 1;
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            this.hidRecordCount.Value = "";

            this.CmbReturnStation.clearContent();
            ret = 0;
        }

        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        return ret;
    }

    private string getHideColumn(RepairInfo temp)
    {
        StringBuilder builder = new StringBuilder();
        string seperator = "\u0003";

        builder.Append(temp.id);
        builder.Append(seperator);
        builder.Append(temp.repairID);
        builder.Append(seperator);
        builder.Append(temp.type);
        builder.Append(seperator);
        builder.Append(temp.obligation);
        builder.Append(seperator);
        builder.Append(temp.component);
        builder.Append(seperator);
        builder.Append(temp.site);//5
        builder.Append(seperator);
        builder.Append(temp.majorPart);
        builder.Append(seperator);
        builder.Append(temp.remark);
        builder.Append(seperator);
        builder.Append(temp.vendorCT);
        builder.Append(seperator);
        builder.Append(temp.partType);//9
        builder.Append(seperator);
        builder.Append(temp.oldPart);
        builder.Append(seperator);
        builder.Append(temp.oldPartSno);
        builder.Append(seperator);
        builder.Append(temp.newPart);
        builder.Append(seperator);
        builder.Append(temp.newPartSno);
        builder.Append(seperator);
        builder.Append(temp.manufacture);
        builder.Append(seperator);
        builder.Append(temp.versionA);
        builder.Append(seperator);
        builder.Append(temp.versionB);
        builder.Append(seperator);
        builder.Append(temp.returnSign);
        builder.Append(seperator);
        builder.Append(temp.mark);
        builder.Append(seperator);
        builder.Append(temp.subDefect);
        builder.Append(seperator);
        builder.Append(temp.piaStation);
        builder.Append(seperator);
        builder.Append(temp.distribution);
        builder.Append(seperator);
        builder.Append(temp._4M);
        builder.Append(seperator);
        builder.Append(temp.responsibility);
        builder.Append(seperator);
        builder.Append(temp.action);
        builder.Append(seperator);
        builder.Append(temp.cover);
        builder.Append(seperator);
        builder.Append(temp.uncover);
        builder.Append(seperator);
        builder.Append(temp.trackingStatus);
        builder.Append(seperator);
        builder.Append(temp.isManual);
        builder.Append(seperator);
        builder.Append(temp.editor);
        builder.Append(seperator);
        builder.Append(temp.newPartDateCode);
        builder.Append(seperator);
        builder.Append(temp.defectCodeID);
        builder.Append(seperator);
        builder.Append(temp.cause); //32
        //add new value
        builder.Append(seperator);
        builder.Append(temp.returnStation);
        //add product mac
        builder.Append(seperator);
        builder.Append(this.hidMac.Value);
        builder.Append(seperator);
        builder.Append(temp.location);
        builder.Append(seperator);
        builder.Append(temp.mtaID);
        //builder.Append("11:22:33:44");



        return builder.ToString();
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[hidCol].Style.Add("display", "none");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Font.Size = FontUnit.Point(16);
            e.Row.Cells[hidCol].Text = e.Row.Cells[hidCol].Text.Replace(Environment.NewLine, "<br>");
            if (!e.Row.Cells[0].Text.Equals("&nbsp;"))
            {
                if (e.Row.Cells[0].Text.Equals("0"))
                {
                    e.Row.Cells[0].Text = "×";
                }
                else
                {
                    e.Row.Cells[0].Text = "√";
                }
            }
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("getAvailableData(\"processData\"); inputFlag = false;");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("setCommonFocus();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }

    private void inputFinish()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("inputFinish();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "inputFinish", scriptBuilder.ToString(), false);
    }

    private void clearLabel()
    {
        this.lblTestStationContent.Text = string.Empty;
    }

    private void checkResult(string par)
    {

        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("checkFinish(\"" + par + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "checkResult", scriptBuilder.ToString(), false);

    }

    protected void hidCheck_ServerClick(Object sender, EventArgs e)
    {
        string prodId = this.hidProdId.Value.Trim();
        int ret = 0;
        //Vincent mark don't check NC code by configure file
        //ret = iFARepair.CheckReturnProduct(prodId);

        checkResult(ret.ToString());
    }
}
