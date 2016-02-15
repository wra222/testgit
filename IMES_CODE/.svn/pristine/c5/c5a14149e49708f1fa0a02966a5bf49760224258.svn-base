using System;
using System.Data;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.Web.UI;
using IMES.Maintain.Interface.MaintainIntf;
using System.Web.UI.WebControls;
using System.Web;

public partial class CommonFunction_SQLInfoDialog : System.Web.UI.Page
{
    private string MsgInfoTODialog;
    protected void Page_Load(object sender, EventArgs e)
    {
        MsgInfoTODialog = HttpContext.Current.Session["MsgInfoTODialog"].ToString();
        
        string[] arry = MsgInfoTODialog.Split('^');


        string MsgInfoTOPage = "";
        foreach (string items in arry)
        {
            MsgInfoTOPage += items + Environment.NewLine;
        }
        this.SQLInfo.InnerText = MsgInfoTOPage;
    }
}
