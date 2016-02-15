/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for RCTO OQC Output Page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-21  itc202017             Create
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

public partial class FA_OQCOutput : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String StationId;

    IOQCOutput iOQCOutput = ServiceAgent.getInstance().GetObjectByName<IOQCOutput>(WebConstant.OQCOutputObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try          
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {
                StationId = Request["Station"];
                clearGrid();
                InitLabel();
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

    private void InitLabel()
    {
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblProId.Text = this.GetLocalResourceObject(Pre + "_lblProId").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblPassCnt.Text = this.GetLocalResourceObject(Pre + "_lbPassCnt").ToString();
        this.lblFailCnt.Text = this.GetLocalResourceObject(Pre + "_lblFailCnt").ToString();
        this.lblTableTitle.Text = this.GetLocalResourceObject(Pre + "_lblTableTitle").ToString();
        this.lblSupportDefectList.Text = this.GetLocalResourceObject(Pre + "_lblSupportDefectList").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.chk9999.Text = this.GetLocalResourceObject(Pre + "_chk9999").ToString();
        this.txtPassCnt.Text = "0";
        this.txtFailCnt.Text = "0";


        IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
        IList<DefectInfo> defectList = iDefect.GetDefectList("PRD");
        foreach (DefectInfo item in defectList)
        {
            this.lbDefectList.Items.Add(new ListItem(item.id + " " + item.friendlyName, item.id));
        }
    }

    protected void gd_DataBound(object sender, GridViewRowEventArgs e)
    {
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
        retTable.Columns.Add("dCode", Type.GetType("System.String"));
        retTable.Columns.Add("Descr", Type.GetType("System.String"));
        return retTable;
    }

    private void initTableColumnHeader()
    {
        this.gd.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titleDefectCode").ToString();
        this.gd.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleDescr").ToString();
        this.gd.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        this.gd.HeaderRow.Cells[1].Width = Unit.Pixel(300);
    }
        
    private void clearGrid()
    {
        try
        {
            this.gd.DataSource = getNullDataTable();
            this.gd.DataBind();
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

        ScriptManager.RegisterStartupScript(this.txt, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

}
