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

public partial class CombineCarton_CR : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //ICombineKeyParts iCombineKeyParts = ServiceAgent.getInstance().GetObjectByName<ICombineKeyParts>(WebConstant.CombineKeyPartsObject);
    ICombineCarton_CR iCombineCarton_CR = ServiceAgent.getInstance().GetObjectByName<ICombineCarton_CR>(WebConstant.CombineCarton_CR);
    public int initRowsCount = 50;
    public string userId;
    public string customer;
    public String Customer;
    public String AccountId;
    public String Login;
    public String UserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            userId = Master.userInfo.UserId;
            customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {
                InitLabel();
                InitSelect();
                this.GridViewExt1.DataSource = getNullDataTable();
                this.GridViewExt1.DataBind();
                initTableColumnHeader();
                this.cmbPdLine.Stage = "FA";
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                this.pCode.Value = Request["PCode"];
                this.stationHF.Value = Request["Station"];
                this.useridHidden.Value = Master.userInfo.UserId;
                AccountId = Master.userInfo.AccountId.ToString();
                Login = Master.userInfo.Login;
                UserName = Master.userInfo.UserName;
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
        this.lbpdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lbCollection.Text = this.GetLocalResourceObject(Pre + "_lblCollectionData").ToString();
        this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrtSet").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
        setinitFocus();
    }

    private void InitSelect()
    {
        IList<ConstValueTypeInfo> lstLine = iCombineCarton_CR.GetQtySelect();
        this.cmbqty.Items.Clear();
        //this.cmbqty.Items.Add(string.Empty);
        if (lstLine.Count != 0)
        {
            foreach (ConstValueTypeInfo item in lstLine)
            {
                this.cmbqty.Items.Add(item.value);
            }
            this.cmbqty.SelectedIndex = 0;
        }
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

    /// <summary>
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        return retTable;
    }

    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = "ProductID";
        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(100);
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
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
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

    /// <summary>
    ///置焦点
    /// </summary>  
    private void setinitFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (setPdLineCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }
    
    private void callInputRun()
    {
        String script = "<script language='javascript'> getAvailableData('processDataEntry'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callInputRun", script, false);
    }

    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
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
