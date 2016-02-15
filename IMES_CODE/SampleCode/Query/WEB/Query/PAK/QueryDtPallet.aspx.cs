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

public partial class Query_PAK_QueryDtPallet : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //public string PAKStation = WebCommonMethod.getConfiguration("PAKStation");

    private string ColumnList = "";
    IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
   
    protected void Page_Load(object sender, EventArgs e)
    {
       // RegisterMPGet(this.Master);
        hidExcelPath.Value = MapPath(@"~\TmpExcel\");
        try
        {
            if (!this.IsPostBack)
            {
                //string dbName = "";
                //if (Request["DBName"] != null)
                //{
                //    dbName = Request["DBName"].ToString().Trim();
                //}
                //CmbDBType.DefaultSelectDB = dbName;
                InitCondition();
              
             
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg,this);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message,this);
        }

    }
    private void InitCondition()
    {

        txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        string customer = Master.userInfo.Customer;
        //customer = "Champ";
      //  this.CmbPdLine.Customer = customer;        
        

        //this.gvResult.DataSource = getNullDataTable(ColumnList);
        //this.gvResult.DataBind();
        InitLabel();
        //InitGridView();
       // InitGrvColumnHeader();
      
    }
    private void InitLabel()
    {

        //lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
     //   lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
    private void InitGridView()
    {
        
        gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(20);
        gvResult.HeaderRow.Cells[2].Width = Unit.Percentage(20);
        gvResult.HeaderRow.Cells[3].Width = Unit.Percentage(20);
        gvResult.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gvResult.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gvResult.HeaderRow.Cells[6].Width = Unit.Percentage(10);

    }
    private string GetStatus()
    {
        string r="";
        List<string> lst = new List<string>();
        foreach (ListItem li in chkState.Items)
        {
            if (li.Selected)
            {
                if (li.Value == "")
                {
                    lst.Add("99");
                    lst.Add("9A");
                    lst.Add("NODA");
                
                }
                else
                { lst.Add(li.Value); }
            
        
            }
        }
       
        r = string.Join(",", lst.ToArray()) ;
        return r;
    }
    private void BindGrv()
    {
        string statusList = GetStatus();
        string Connection = CmbDBType.ddlGetConnection();
        hidPalletList.Value = hidPalletList.Value.Replace("'", "");
        txtPallet.Text = txtPallet.Text.Replace("'", "");
        DateTime FromDate = DateTime.ParseExact(txtFromDate.Text.Trim(), "yyyy-MM-dd", null);
        DateTime ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "yyyy-MM-dd", null);
        string palletList = "";
        if (radPallet.Checked)
        {
            if (string.IsNullOrEmpty(txtPallet.Text) && string.IsNullOrEmpty(hidPalletList.Value))
            {
                showErrorMessage("Please input pallet no", this);
                endWaitingCoverDiv(this);
                return;
            }
            string[] palletArr = hidPalletList.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            palletList = string.Join(",", palletArr);
            if (!string.IsNullOrEmpty(txtPallet.Text))
            {
                palletList = txtPallet.Text.Trim();
            }
        }
        else
        {
            palletList = "";
        }
    
        DataTable dt = PAK_Common.GetDtPallet(Connection,palletList,FromDate,ToDate, statusList);

        if (dt == null || dt.Rows.Count == 0)
        {
        //    EnableBtnExcel(this, false, btnExcel.ClientID);
            EnableBtnExcel(this, false, "btnExcel");

            BindNoData(dt, gvResult);
        }
        else
        {
          //  EnableBtnExcel(this, true, btnExcel.ClientID);
            EnableBtnExcel(this, true, "btnExcel");
            if (Session["QueryDtPallet"] == null)
            { Session.Add("QueryDtPallet", dt); }
            else
            { Session["QueryDtPallet"] = dt; }
            string h = dt.Rows[0][0].ToString();
            gvResult.DataSource = dt;
            gvResult.DataBind();
            // InitGrvColumnHeader();
        }
        gvDetail.Visible = false;

    }
   private void BindGrv2()
    {
        string Connection = CmbDBType.ddlGetConnection();
       string statusList = GetStatus();
       //return;
      hidPalletList.Value= hidPalletList.Value.Replace("'", "");
      txtPallet.Text= txtPallet.Text.Replace("'", "");
      string[] modelArr = hidPalletList.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);


       string modelList = "'" + string.Join("','", modelArr) + "'";
       if (!string.IsNullOrEmpty(txtPallet.Text))
       {
           modelList = txtPallet.Text.Trim();
           modelList = "'" + modelList + "'";
       }

       string[] stationArr = PAKStation.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);

       PAKStation = "'" + string.Join("','", stationArr) + "'";

    //     string Connection = CmbDBType.ddlGetConnection();
         DateTime FromDate = DateTime.ParseExact(txtFromDate.Text.Trim(), "yyyy-MM-dd", null);
         DateTime ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "yyyy-MM-dd", null);
         DataTable dt = PAK_Common.GetDtPallet(Connection,"",FromDate, ToDate, statusList);
   
          if (dt == null || dt.Rows.Count == 0)
          {
              EnableBtnExcel(this, false, "btnExcel");
              BindNoData(dt,gvResult);
        
          }
          else
          {
     
              EnableBtnExcel(this, true, "btnExcel");
              gvResult.DataSource = dt;
              gvResult.DataBind();

              if (Session["QueryDtPallet"] == null)
              { Session.Add("QueryDtPallet", dt); }
              else
              { Session["QueryDtPallet"] = dt; }
             // InitGrvColumnHeader();
          }
          gvDetail.Visible = false;
   
   
   }

  
    protected void btnQuery_Click(object sender, EventArgs e)
    {
     //   beginWaitingCoverDiv(this.UpdatePanel1);     
        try
       {
         // string[] dnArr = hidPalletList.Value.Split('\n');
           BindGrv();
           // if (radPallet.Checked)
           //{ BindGrv2(); }
           //else
           //{ BindGrv(); }
   
        }
         catch (Exception ex)
        {
            showErrorMessage(ex.Message,this.UpdatePanel1);
         //   BindNoData();
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
       // InitGrvColumnHeader();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ToolUtility t = new ToolUtility();
        t.ExportExcel(gvResult, "Excel", Page);
    }


    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string pallet = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            pallet = e.Row.Cells[2].Text.Trim();
            if (e.Row.Cells[5].Text!= "" && e.Row.Cells[6].Text != "&nbsp;")
            {
                e.Row.Cells[2].CssClass = "querycell";
                e.Row.Cells[2].Attributes.Add("onclick", "SelectDetail('" + pallet + "')");
            }
            if (e.Row.Cells[5].Text =="NODA")
            {
                e.Row.Cells[5].Text = "";
            }
            if (e.Row.Cells[5].Text.Trim() == "OT" || e.Row.Cells[5].Text.Trim() == "IN" || e.Row.Cells[5].Text.Trim() == "DT" || e.Row.Cells[5].Text.Trim() == "RD")
            {
                e.Row.Cells[5].BackColor = System.Drawing.ColorTranslator.FromHtml("#06FB48");
            }
            e.Row.Cells[5].Text = GetState(e.Row.Cells[5].Text.Trim());
            if (e.Row.Cells[6].Text != "" && e.Row.Cells[6].Text != "&nbsp;")
            {
                e.Row.Cells[6].Text = DateTime.Parse(e.Row.Cells[6].Text).ToString("yyyy/MM/dd HH:mm");
            }
           
        }
      
    }
    public void QueryDetailClick(object sender, System.EventArgs e)
    {
        string Connection = CmbDBType.ddlGetConnection();
        gvDetail.Visible = true;
        string pallet = hidPalletNo.Value.Trim();
        DataTable dt = PAK_Common.GetPLTLog(Connection,pallet);
        gvDetail.DataSource = dt;
        gvDetail.DataBind();

        endWaitingCoverDiv(this);
    }
    private static string GetState(string state)
    {
        string caseState = state;
        switch (state)
        {
            case "":
                caseState = "未刷過電梯";
                break;
            case "NODA":
                caseState = "未刷過電梯";
                break;
            case "DT":
                caseState = "已刷入DT";
                break;
            case "RW":
                caseState = "退回產線";
                break;
            case "IN":
                caseState = "已刷入W/H";
                break;
            case "OT":
                caseState = "已刷出車管系統";
                break;
            case "RD":
                caseState = "退回產線后再刷DT";
                break;
    
        }
        return caseState;
    }
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string status = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            status = e.Row.Cells[1].Text.Trim();

            e.Row.Cells[1].Text = GetState(status);
            if (e.Row.Cells[2].Text != "" && e.Row.Cells[2].Text != "&nbsp;")
            {
                e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString("yyyy/MM/dd HH:mm");
            }

        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string DownExcel_WebMethod(string path)
    {
      

        if (HttpContext.Current.Session["QueryDtPallet"] == null)
        {
            throw new Exception("請重新查詢");
        }
        DataTable dt = (DataTable)HttpContext.Current.Session["QueryDtPallet"];
       
        foreach (DataRow dr in dt.Rows)
        {
            dr[5] = GetState(dr[5].ToString().Trim());
        }

    //    e.Row.Cells[5].Text = GetState(e.Row.Cells[5].Text.Trim());

        int y = HttpContext.Current.Session.Timeout;
        string fileID = ExcelTool.DataTableToExcelSaveLocal(dt, "QueryDtPallet", path);
        return fileID;


    }
}
