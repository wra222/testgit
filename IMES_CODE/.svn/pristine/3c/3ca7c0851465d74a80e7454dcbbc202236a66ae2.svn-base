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
using IMES.Query.Interface.QueryIntf;
using IMES.Infrastructure;
using System.Text;
public partial class Query_SA_DetialShow : System.Web.UI.Page
{
     public string pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
     ISMT_TestDataReport intfSMT_TestDataReport = ServiceAgent.getInstance().GetObjectByName<ISMT_TestDataReport>(WebConstant.ISMT_TestDataReport);
     string[] msg = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        //if (!Page.IsPostBack)
        //{
        string message = this.Request.QueryString["msg"];

  
            if (!string.IsNullOrEmpty(message))
            {
                 msg = message.Split('~');
                 /*
                 string DBConnection; string Model; string PdLine;string Station; DateTime From; DateTime To; string Defect;string Tp;
                 DBConnection=msg[0];
                 Model=msg[1];
                 PdLine=msg[2];
                 Station=msg[3];
                 From = DateTime.Parse(msg[4]);
                 To = DateTime.Parse(msg[5]);
                 Defect = msg[6];
                 Tp = msg[7];
                 if (Tp == "Cause")
                 { Title.Text = "Cause Detial"; }
                 else
                 { Title.Text = "Location Detial"; }
                  DataTable Result =intfSMT_TestDataReport.Get_Detial(DBConnection, Model, PdLine, Station, From, To, Defect, Tp);
                  DataView ds = new DataView();
                  ds.Table = Result;
                  gvQuery.AllowPaging = true;
                  gvQuery.PagerSettings.Mode = PagerButtons.NextPreviousFirstLast;
                  gvQuery.PageSize = 20;
                  gvQuery.DataSource = ds;
                  gvQuery.DataBind();
                 */
                 Bind();
                 //errMes = (errMes.Replace( Chr(13).ToString(), "<br/>")).Replace(Chr(10).ToString(), "<br/>");
                //string str = " window.document.getElementById('form1').OK.blur() ; alert("+"'"+"" + message +""+"'"+");";
                string str = " window.document.getElementById('form1').OK.blur() ;  ";
                string script = "<script type='text/javascript' language='javascript'>" + str + "</script>";
                ClientScript.RegisterStartupScript(GetType(), "", script);

            //}
        }
    }
    public void Bind()
    {
        string DBConnection; string Model; string PdLine; string Station; DateTime From; DateTime To; string Defect; string Tp;
        DBConnection = msg[0];
        Model = msg[1];
        PdLine = msg[2];
        Station = msg[3];
        From = DateTime.Parse(msg[4]);
        To = DateTime.Parse(msg[5]);
        Defect = msg[6];
        Tp = msg[7];
        if (Tp == "DefectCode")
        { Title.Text = "DefectCode Detial"; }
        else
        { Title.Text = "Location Detial"; }
        DataTable Result = intfSMT_TestDataReport.Get_Detial(DBConnection, Model, PdLine, Station, From, To, Defect, Tp);
        DataView ds = new DataView();
        ds.Table = Result;
        gvQuery.AllowPaging = true;
        gvQuery.PagerSettings.Mode = PagerButtons.NextPreviousFirstLast;
        gvQuery.PageSize = 20;
       //gvQuery.PagerSettings.FirstPageText="第一頁";

       // gvQuery.PagerSettings.LastPageText="最後頁";

       //  gvQuery.PagerSettings.Mode=NextPreviousFirstLast ;  //显示的模式，自定义     

       // gvQuery.PagerSettings.NextPageText="下一頁"

       //  gvQuery.PagerSettings.Position="TopAndBottom" ;//         分页选择在控件上部和下部均显示；

       //  PreviousPageText="上一页"
        gvQuery.DataSource = ds;
        gvQuery.DataBind();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();

        if (gvQuery.HeaderRow != null && gvQuery.HeaderRow.Cells.Count > 0)
            tu.ExportExcel(gvQuery, "SMT_TestDataReport", Page);
        else
        {
            string str = "   alert( 'Please select one record! ' );";
            string script = "<script type='text/javascript' language='javascript'>" + str + "</script>";
            ClientScript.RegisterStartupScript(GetType(), "", script);
        
        }
    }
    protected void OK_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.close();</script>");// 会弹出询问是否关闭
        Response.Write("<script>window.opener=null;window.close();</script>");// 不会弹出询问
    }
    protected void gvQuery_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvQuery.PageIndex = e.NewPageIndex;
        this.Bind();
    }  
}
