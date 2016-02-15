using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using com.inventec.iMESWEB;

public partial class MasterPageMaintain : System.Web.UI.MasterPage
{
    public UserInfo userInfo = new UserInfo();
    protected string languagePre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_";

    protected void Page_Init(object sender, EventArgs e)
    {
        //if (Session["UserId"] == null || Session["UserId"].ToString() == "")
        //{
        //    string strMsg = Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "SessionIsNull");
        //    //Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
        //    Response.Redirect(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
        //}

        setUserInfo();
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void setUserInfo()
    {
        //userInfo.UserId = Request["UserId"];
        //userInfo.UserName = HttpUtility.UrlDecode(Request["UserName"]);
        //userInfo.Customer = Request["Customer"];
        //userInfo.AccountId = long.Parse(Request["AccountId"]);
        //userInfo.Login = Request["Login"];
        userInfo.UserId = "0001";
        userInfo.UserName = "admin";
        userInfo.Customer = "HP";
        userInfo.AccountId = 1111;
        userInfo.Login = "admin";
    }
}
