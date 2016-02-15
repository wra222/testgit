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
//using IMES.Station.Interface.CommonIntf;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using System.Net.Mail;
using System.IO;
public partial class Query_PAK_Query87Product : IMESQueryBasePage
{
  
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string SMTP = WebCommonMethod.getConfiguration("SMTP");

    IPAK_Query87Product PAK_Query87Product = ServiceAgent.getInstance().GetObjectByName<IPAK_Query87Product>(WebConstant.PAK_Query87Product);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);

    protected void Page_Load(object sender, EventArgs e)
    {
   
        RegisterMPGet(this.Master);
        ChxLstProductType1.IsHide = true;
        try
        {
           if (!this.IsPostBack)
            {
                string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
                string dbName = Request["DBName"] ?? configDefaultDB;
                hidDbName.Value = dbName;
                CmbDBType.DefaultSelectDB = dbName;
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
    private void GetLine()
    {
       
        List<string> Process = new List<string>();
        Process.Add("FA");
        Process.Add("PAK");
        string customer = Master.userInfo.Customer;
        string Conn = CmbDBType.ddlGetConnection();
     
        DataTable dtPdLine = QueryCommon.GetLine(Process, customer, true, Conn);
        if (dtPdLine.Rows.Count > 0)
        {
            //foreach (DataRow dr in dtPdLine.Rows)
            //{
            //    lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
            //}
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }
    }
    private void InitCondition()
    {       
        txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd");     
        GetLine();
        InitLabel();
   
    }
    private void InitLabel()
    {

        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
    private void InitGridView()
    {
        
        //gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(22);
 
    }
    private void Mail()
    {
        bool IsDebug = false;
        string body = GridViewToHtml(gvResult);
     
        MailMessage message = new MailMessage();
        MailAddress from = new MailAddress("Zhang.Xiao-lan@inventec.com.cn", "Zhang, Xiao-lan");
        message.From = from;
        //message.To.Add("Chen.BensonPM@inventec.com");
      
        List<string> lst=new  List<string>() { "87ReportMailList"};
        DataTable dt = GetSysSetting(lst, CmbDBType.ddlGetConnection());
        string mailValue=dt.Rows[0]["Value"].ToString();

     //   showErrorMessage(mailValue, this); return;
        if (string.IsNullOrEmpty(mailValue))
        {
            EnableBtnExcelandMail(true); showErrorMessage("Please define mail list", this); return;
        }
        string[] mailList = mailValue.Split(';');
        string addr = "";
        if (IsDebug)
        {
            message.To.Add(new MailAddress("Chen.BensonPM@inventec.com"));
        
        }
        else
        {
            foreach (String s in mailList)
            {
                addr = s;
                if (s.IndexOf("@") == -1)
                {
                    addr = s + "@inventec.com.cn";
                }
                message.To.Add(new MailAddress(addr));
            }
        }
      


        message.IsBodyHtml = true;
        message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼
        message.Body = body;
        message.Subject = "The Combined DN Product List";
        SmtpClient smtpClient = new SmtpClient(SMTP, 25);//設定E-mail Server和port
        //   smtpClient.Credentials
        smtpClient.Send(message);
        EnableBtnExcelandMail(true); showErrorMessage("Mail successfully", this);

    }
    private string GridViewToHtml(GridView gv)
    {
        GridView gvTmp = new GridView();
      
        gvTmp=gv;
        gvTmp.Width = Unit.Parse("900");
        gvTmp.HeaderRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#A9A9F5");
      //  gvTmp.CssClass = "";
        gvTmp.HeaderRow.Cells[5].Attributes.Add("width", "60px");
        gvTmp.HeaderRow.Cells[0].Attributes.Add("width", "60px");
        gvTmp.HeaderRow.ForeColor = System.Drawing.Color.White;
     
        gvTmp.RowStyle.BackColor = System.Drawing.Color.FromArgb(212, 218, 195);
        gvTmp.AlternatingRowStyle.BackColor = System.Drawing.Color.FromArgb(226, 235, 204);
   
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvTmp.RenderControl(hw);
        gv.Width = Unit.Percentage(98);

        return sb.ToString();
    }
   private void BindGrv()
   {
        
         string Connection = CmbDBType.ddlGetConnection();
         DateTime ShipDate = DateTime.Parse(txtShipDate.Text.Trim());
          IList<string> lstPdLine = new List<string>();
          string Line="";
          foreach (ListItem item in lboxPdLine.Items)
            {
                if (item.Selected)
                {
                    lstPdLine.Add(item.Value);
                }
            }

           Line = string.Join(",", lstPdLine.ToArray());

           DataTable dt = PAK_Query87Product.GetQueryResult(Connection, radPallet.SelectedValue, Line, ShipDate,"");
          if (dt == null || dt.Rows.Count == 0)
          {

              EnableBtnExcelandMail(false);
              base.BindNoData(dt, gvResult);
          }
          else
          {
           
              EnableBtnExcelandMail(true);
              gvResult.DataSource = dt;
              gvResult.DataBind();
              InitGrvColumnHeader();
             
             
          }
    
   
   
   }

   private void EnableBtnExcelandMail( bool enable)
   {
       string tmp = "";
       if (enable)
       {
           tmp = "document.getElementById('" + btnExcel.ClientID + "').style.display = 'inline';";
           tmp += "document.getElementById('" + btnMail.ClientID + "').style.display = 'inline';";
       }
       else
       {

           tmp = "document.getElementById('" + btnExcel.ClientID + "').style.display = 'none';";
           tmp += "document.getElementById('" + btnMail.ClientID + "').style.display = 'none';";

       }

       String script = "<script language='javascript'>" + "\r\n" +
         tmp + "\r\n" + "</script>";
       ScriptManager.RegisterStartupScript(this, typeof(System.Object), "EnableExcel", script, false);

   }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
     //   beginWaitingCoverDiv(this.UpdatePanel1);     
        try
       {
         BindGrv();
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message.Replace(@"""", ""),this.UpdatePanel1);
          //  BindNoData();
        }
         finally
        {
             endWaitingCoverDiv(this);
        }
  
      
    }
    private void InitGrvColumnHeader()
    {
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(50);
        gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(70);
        
    }
   
    protected void btnExcel_Click(object sender, EventArgs e)
    {
      
        System.IO.MemoryStream ms =ExcelTool.GridViewToExcel(gvResult, "Data");
        this.Response.ContentType = "application/download";
        this.Response.AddHeader("Content-Disposition", "attachment; filename=file.xls");
        this.Response.Clear();
        this.Response.BinaryWrite(ms.GetBuffer());
        ms.Close();
        ms.Dispose();

    }
    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        { Mail(); InitGrvColumnHeader(); }
        catch (Exception ex2)
        {
            EnableBtnExcelandMail(true);
            showErrorMessage(ex2.Message + "\r\n" + ex2.InnerException.Message, this);
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
    }
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
  
    }
}
