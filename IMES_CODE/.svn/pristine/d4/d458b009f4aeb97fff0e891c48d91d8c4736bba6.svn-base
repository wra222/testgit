/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Codebehind for  FA Test Station Page
 * UI:CI-MES12-SPEC-FA-UI FA Test StationForDocking.docx
 * UC:CI-MES12-SPEC-FA-UC FA Test StationForDocking.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-23  Zhang.Kai-sheng       (Reference Ebook SourceCode) Create
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
//using IMES.Station.Interface.CommonIntf;
//using IMES.Station.Interface.StationIntf;
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;

public partial class DOCKING_FATestStation : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initRowsCount = 6;
    public string userId;
    public string customer;
    //-----------------------------------
    //To handle Page_Load (Server's control refresh or change) 
    //-----------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            //initTableColumnHeader();
            //Print2DLabelStation.Value = ConfigurationManager.AppSettings["Print2DLabelStation"].ToString();
            this.cmbTestStation.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbTestStation_Selected);
            
            // 不需让user选择Line,所以CmpPdLine不用跟cmbTestStation做连动
            if (!Page.IsPostBack)
            {
                initTableColumnHeader();
                InitLabel();
                //绑定空表格
                this.gridview.DataSource = getNullDataTable();
                this.gridview.DataBind();
                //ViewState["ds"] = this.gridview.DataSource;
                //将pdLine combobox的初始化参数传入
                //此处应标明是获取FA的所有Test Station
                this.cmbTestStation.Station = "FATest";
                
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
    /// 选择pdLIne或者testStation下拉框，会reset界面
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbTestStation_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //刷新下拉框内容
            if (this.cmbTestStation.InnerDropDownList.SelectedValue == "")
            {
                //清空下拉框内容
                this.cmbPdLine.clearContent();
            }
            else
            {
                //this.cmbPdLine.refresh(this.cmbTestStation.InnerDropDownList.SelectedValue, Master.userInfo.Customer);
                var selectLine = this.cmbPdLine.InnerDropDownList.SelectedItem.Text;
                this.cmbPdLine.refresh(this.cmbTestStation.InnerDropDownList.SelectedValue, this.customerHidden.Value);
                int i = 0;
                foreach (ListItem lst in this.cmbPdLine.InnerDropDownList.Items)
                {
                    if (selectLine == lst.Text)
                    {
                        cmbPdLine.InnerDropDownList.SelectedIndex = i;
                        return;
                    }
                    i++;
                }

            }
            
            //String script = "<script language='javascript'>" + "\r\n" + "ResetPage();" + "\r\n" + "</script>";
            //ScriptManager.RegisterStartupScript(this.updatePanel1, ClientScript.GetType(), "ResetPage", script, false);

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
        this.lbtestStation.Text = this.GetLocalResourceObject(Pre + "_lblTestStation").ToString();
        this.lbPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lbFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lbProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdId").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbDefectList.Text = this.GetLocalResourceObject(Pre + "_lblDefectList").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.nineSelect.Text = this.GetLocalResourceObject(Pre + "_nineSelect").ToString();//"Dont Scan 9999";
        this.lbTesttool.Text = this.GetLocalResourceObject(Pre + "_lblTestTool").ToString(); 
        //this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrtSet").ToString();
        //kshzh this.lblDefectStation.Text = this.GetLocalResourceObject(Pre + "_lblDefectStation").ToString();
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
            //this.cmbMBCode.InnerDropDownList.SelectedIndex = 0;
            //重置mbCode下拉框,并触发它的选择事件
            this.cmbTestStation.setSelected(0);
            cmbTestStation_Selected(sender, e);
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
