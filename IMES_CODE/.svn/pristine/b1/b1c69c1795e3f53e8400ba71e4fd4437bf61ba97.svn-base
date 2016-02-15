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
using System.Text;
using IMES.Infrastructure;

public partial class PAQCCosmetic : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initRowsCount = 6;

    public String UserId;
    public String Customer;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            this.cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);

            if (!Page.IsPostBack)
            {
                string station = Request["Station"];
                InitLabel();
                //绑定空表格
                this.GridView1.DataSource = getNullDataTable();
                this.GridView1.DataBind();
                initTableColumnHeader();

                //将pdLine combobox的初始化参数传入
                string Stage = Request["Stage"];

                if (string.IsNullOrEmpty(Stage))
                {
                    this.cmbPdLine.Stage = "PAK";
                }
                else
                {
                    this.cmbPdLine.Stage = Stage;
                }

                this.cmbPdLine.Customer = Master.userInfo.Customer;

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.station1.Value = station;
                this.editor1.Value = UserId;
                this.customer1.Value = Customer;
            }
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message);
        }
    }

    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lblFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lblCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblProdInfo.Text = this.GetLocalResourceObject(Pre + "_lblProdInfo").ToString();
        this.lblDefectList.Text = this.GetLocalResourceObject(Pre + "_lblDefectList").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
    }

    private DataTable getNullDataTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            newRow["Defect Code"] = String.Empty;
            newRow["Description"] = String.Empty;
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
        retTable.Columns.Add("Defect Code", Type.GetType("System.String"));
        retTable.Columns.Add("Description", Type.GetType("System.String"));
        return retTable;
    }

    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.GridView1.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_colDefect").ToString();
        this.GridView1.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_colDescription").ToString();

        GridView1.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_colDefect").ToString();
        GridView1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colDescription").ToString();
        GridView1.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        GridView1.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        //this.GridView1.Columns[0].ItemStyle.Width = Unit.Pixel(20);
        //this.GridView1.Columns[1].ItemStyle.Width = Unit.Pixel(160);
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
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
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
}
