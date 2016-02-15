using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
using System.Data;
using System.IO;

public partial class Query_PAK_BsamGetByCartonSN: IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "";
    IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
    String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterMPGet(this.Master);
        try
        {
            if (!this.IsPostBack)
            {


                InitCondition();


            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg, this);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message, this);
        }

    }
    private void InitCondition()
    {


        //string customer = Master.userInfo.Customer;

        //InitLabel();
        //InitGridView();
        //InitGrvColumnHeader();

    }
    private void InitLabel()
    {

        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
    private void InitGridView()
    {

        //gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[5].Width = Unit.Percentage(20);



    }

    private void BindGrv()
    {
        string DBConnection = CmbDBType.ddlGetConnection();
        hidModelList.Value= hidModelList.Value.Replace("'", "");
        txtCartonSN.Text = txtCartonSN.Text.Replace("'", "");

      if (string.IsNullOrEmpty(txtCartonSN.Text) && string.IsNullOrEmpty(hidModelList.Value))
      {
          showErrorMessage("Please input Carton SN", this);
          endWaitingCoverDiv(this);
          return;
      }

      string[] cartonArr = hidModelList.Value.Trim().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
       string cartonList =  string.Join(",", cartonArr) ;
       if (!string.IsNullOrEmpty(txtCartonSN.Text))
       {
           cartonList = txtCartonSN.Text.Trim();
     
       }

       DataTable dt = PAK_Common.GetByCartonSN(DBConnection, cartonList);
  
          if (dt == null || dt.Rows.Count == 0)
          {
              EnableBtnExcel(this, false, btnExcel.ClientID);
              BindNoData(dt,gvResult);
          }
          else
          {
              EnableBtnExcel(this, true, btnExcel.ClientID);
          


              gvResult.DataSource = dt;
              gvResult.DataBind();

              string hh = gvResult.HeaderRow.Cells[1].Text;
              gvResult.FooterRow.Cells[1].Text = dt.Rows.Count.ToString();
              gvResult.FooterRow.Cells[1].Font.Bold = true;

              //gvResult.FooterRow.Cells[6].Text = dt.Compute("Sum(UnitWeight)", "").ToString();
              //gvResult.FooterRow.Cells[6].Font.Bold = true;


              gvResult.FooterRow.Cells[1].Font.Size = FontUnit.Parse("14px");
              //gvResult.FooterRow.Cells[6].Font.Size = FontUnit.Parse("14px");
              gvResult.FooterRow.Visible = true;
              gvResult.FooterStyle.BorderColor = System.Drawing.Color.White;
             // InitGrvColumnHeader();
          }
          endWaitingCoverDiv(this);
        }



    public void endWaitingCoverDiv(Control ctr)
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(ctr, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //     beginWaitingCoverDiv(this.UpdatePanel1);     
        try
        {
            BindGrv();
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message, this.UpdatePanel1);
            BindNoData();
        }
        finally
        {
            endWaitingCoverDiv(this);
        }


    }
    private void InitGrvColumnHeader()
    {


    }
    private void BindNoData()
    {
        this.gvResult.DataSource = getNullDataTable(ColumnList);
        this.gvResult.DataBind();
        InitGridView();
        InitGrvColumnHeader();
    }
    private void BindNoData(DataTable dt)
    {

        foreach (DataColumn dc in dt.Columns)
        {
            ColumnList = ColumnList + dc.ColumnName + ",";

        }
        ColumnList = ColumnList.Substring(0, ColumnList.Length - 1);
        this.gvResult.DataSource = getNullDataTable(ColumnList);
        this.gvResult.DataBind();

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        MemoryStream ms = ExcelTool.GridViewToExcel(gvResult, "Data", 3);
        this.Response.ContentType = "application/download";
        this.Response.AddHeader("Content-Disposition", "attachment; filename=file.xls");
        this.Response.Clear();
        this.Response.BinaryWrite(ms.GetBuffer());
        ms.Close();
        ms.Dispose();
    }

    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DateTime d;
        //    if(DateTime.TryParse(e.Row.Cells[11].Text,out d))
        //    {
        //               e.Row.Cells[11].Text=d.ToString("yyyy/MM/dd HH:mm");
        //    }

        //}
    }

}
