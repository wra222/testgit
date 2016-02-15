/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Output
* UI:CI-MES12-SPEC-PAK-UC PAQC Sorting.docx –2012/07/18
* UC:CI-MES12-SPEC-PAK-UC PAQC Sorting.docx –2012/07/18            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-07-18   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
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
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;
using System.Text;

public partial class PAK_PAQCSorting : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IPAQCSorting iPAQCSorting = ServiceAgent.getInstance().GetObjectByName<IPAQCSorting>(WebConstant.PAQCSortingObject);

    private const int COL_NUM = 6;
    private const int DEFAULT_ROWS = 12;
    //private IDefect iDefect;
    public String UserId;
    public String Customer;
    public String Station;
    public String Message;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.CmbPdLine1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            this.CmbPdLine1.InnerDropDownList.AutoPostBack = true;

            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
                initLabel();
                Station = Request["station"];
                //ITC-1413-0083
                this.CmbPdLine1.Stage = "PAK"; 
                this.CmbPdLine1.Customer = Master.userInfo.Customer;
                IList<PaqcsortingInfo> stationList = iPAQCSorting.GetStationList("", UserId, out Message);
                bindTable(stationList, DEFAULT_ROWS);

                setFocus();
            }

        }
        catch (FisException ex) 
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    private void initLabel()
    {
        this.lblPDLine.Text = this.GetLocalResourceObject(Pre + "_lblPDLine").ToString();
        this.lblCheckStation.Text = this.GetLocalResourceObject(Pre + "_lblCheckStation").ToString();
        this.btnResetPage.InnerText = this.GetLocalResourceObject(Pre + "_btnResetPage").ToString();

    }

    private void bindTable(IList<PaqcsortingInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(" ");  
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemStation").ToString());  
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDescr").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemTime").ToString());  

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemPassQty").ToString()); 
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemLeastQty").ToString()); 

        dt.Columns.Add("id");      

        if (list != null && list.Count != 0)
        {
            foreach (PaqcsortingInfo temp in list)
            {
                dr = dt.NewRow();

                if (temp.id < 0)
                {
                    dr[0] = "-";
                    if (temp.id == -1)
                    {
                        dr[3] = " ";
                    }
                    else
                    {   //ITC-1413-0088
                        dr[3] = temp.previousFailTime.ToString();
                    }
                }
                else
                {
                    dr[0] = " ";
                    dr[3] = temp.previousFailTime.ToString();
                }
                dr[1] = temp.station;
                dr[2] = temp.Descr;          
                dr[4] = temp.PassQty.ToString();
                dr[5] = temp.LeastPassQty.ToString();
                dr[6] = temp.id;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            //this.hidRecordCount.Value = list.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            //this.hidRecordCount.Value = "";
        }

        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        gridViewUP.Update();
        ScriptManager.RegisterStartupScript(this.gridViewUP, typeof(System.Object), "showStation", "showStation('"+Message+"');", true);
    }

    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        try
        {
            if (this.CmbPdLine1.InnerDropDownList.SelectedValue == "")
            { 
                this.CmbPdLine1.clearContent(); 
            }
            else
            {
                int index = this.CmbPdLine1.InnerDropDownList.SelectedIndex;
                string line = this.CmbPdLine1.InnerDropDownList.SelectedValue;
                IList<PaqcsortingInfo> stationList = iPAQCSorting.GetStationList(line, UserId, out Message);

                bindTable(stationList, DEFAULT_ROWS);
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

    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
            int index = this.CmbPdLine1.InnerDropDownList.SelectedIndex;
            string line = this.CmbPdLine1.InnerDropDownList.SelectedValue;
            IList<PaqcsortingInfo> stationList = iPAQCSorting.GetStationList(line, UserId, out Message);
            bindTable(stationList, DEFAULT_ROWS);

            //ScriptManager.RegisterStartupScript(this.gridViewUP, typeof(System.Object), "reHighLight", "reHighLight();", true);
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
    private void setColumnWidth()
    {
        //gd.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        //Set column width 
        //================================= Add Code ======================================

        gd.HeaderRow.Cells[0].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(1);
        //================================= Add Code End ==================================
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }

    }

    private void setFocus()
    {
        //String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        //ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }
}
