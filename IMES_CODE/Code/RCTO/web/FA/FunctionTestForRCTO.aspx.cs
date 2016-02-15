/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Codebehind for  FA Test Station Page
 * UI:CI-MES12-SPEC-FA-UI FA Test Station.docx --2011/10/20 
 * UC:CI-MES12-SPEC-FA-UC FA Test Station.docx --2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-28  Zhang.Kai-sheng       (Reference Ebook SourceCode) Create
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
using System.Text;

public partial class FA_FunctionTestForRCTO : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initRowsCount = 6;
    public String userId;
    public String customer;
    public String station;
    //-----------------------------------
    //To handle Page_Load (Server's control refresh or change) 
    //-----------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            userId = Master.userInfo.UserId;
            customer = Master.userInfo.Customer;
            if (null == station || "" == station)
            {
                station = Request["Station"];
            }
            if (!Page.IsPostBack)
            {
                initTableColumnHeader();
                InitLabel();

                this.gridview.DataSource = getNullDataTable();
                this.gridview.DataBind();
                
                
                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;
                this.useridHidden.Value = Master.userInfo.UserId;
                this.customerHidden.Value = Master.userInfo.Customer;
                //DEBUG : Get Value from Master Page --KSHZH
                //--------------------------------------------
                this.cmbPdLine.Customer = customer;
                if ((Request["Station"]==null)||(Request["Station"] == ""))
                {
                    this.cmbPdLine.Stage = "FA";
                }
                else
                {
                    this.cmbPdLine.Station = Request["Station"];
                }
                //--------------------------------------------
                this.pCode.Value = Request["PCode"];
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
        this.lbPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lbFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lbEPIA.Text = this.GetLocalResourceObject(Pre + "_lblEPIA").ToString();
        this.lbProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdId").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbDefectList.Text = this.GetLocalResourceObject(Pre + "_lblDefectList").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.nineSelect.Text = this.GetLocalResourceObject(Pre + "_nineSelect").ToString();//"Dont Scan 9999";

        setFocus();
    }

    private DataTable getNullDataTable()
    {
       DataTable dt = initTable();
       DataRow newRow;
       for (int i = 0; i < initRowsCount; i++)
       {
            newRow = dt.NewRow();
            newRow["DefectId"] = String.Empty;
            newRow["DefectDescr"] = String.Empty;
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
       DataTable retTable  = new DataTable();
       
       retTable.Columns.Add("DefectId", Type.GetType("System.String"));
       retTable.Columns.Add("DefectDescr", Type.GetType("System.String"));
       return retTable;
    }

    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        
        this.gridview.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_lblDefectCode").ToString();
        this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();
        
        //this.gridview.Columns[0].ItemStyle.Width = Unit.Pixel(160);
        //this.gridview.Columns[1].ItemStyle.Width = Unit.Pixel(160);
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
    private void setFocus()
    {
        /*
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (setTestStationCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setTestStationCmbFocus", script, false);
         */
    }

    /// <summary>
    /// reset页面信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
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

    protected void nineSelect_CheckedChanged(object sender, EventArgs e)
    {
        if (nineSelect.Checked == true)
        {

        }
        else 
        {

        }
        
    }
}
