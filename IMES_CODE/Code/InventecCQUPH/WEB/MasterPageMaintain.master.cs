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
using com.inventec.RBPC.Net.datamodel.intf;

public partial class MasterPageMaintain : System.Web.UI.MasterPage
{
    public UserInfo userInfo = new UserInfo();
    private static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private bool displayInfoArea = true;
   // protected string languagePre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_";
    public bool NeedPrint = true;


    //protected string languagePre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_";

    //protected void Page_Init(object sender, EventArgs e)
    //{
    //    if (Session["UserId"] == null || Session["UserId"].ToString() == "")
    //    {
    //        string strMsg = "Please Login in!!!!";
    //        //Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
    //        Response.Redirect(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
    //    }

    //    setUserInfo();
    //}
    protected void Page_Init(object sender, EventArgs e)
    {
        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        logger.InfoFormat("BEGIN: {0}()", methodName);
        try
        {
            string sessionId = Request["SessionId"];
            string userId = Request["UserId"];
            logger.DebugFormat("Path:{0} IsPostBack:{1} SessionId:{2} UserId:{3}",
                                                Page.AppRelativeVirtualPath,
                                                Page.IsPostBack.ToString(),
                                                sessionId ?? "",
                                                userId ?? "");
            if (!Page.IsPostBack)
            {

                userId = Request["UserId"];
                if (Session["UserId"] == null || Session["UserId"].ToString() == "")
                    if (string.IsNullOrEmpty(sessionId))
                    {
                        if (string.IsNullOrEmpty(userId) ||
                            Session == null ||
                            Session["UserId"] == null ||
                            Session["UserId"].ToString() != userId)
                        {
                            //string strMsg = Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "SessionIsNull");
                            string strMsg = "Please Login in!!!!";
                            Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
                        }
                    }
                    else
                    {
                        if (Session == null ||
                           Session[sessionId] == null ||
                           Session[sessionId].ToString() != sessionId ||
                           Session[sessionId + "-UserId"] == null ||
                           Session[sessionId + "-UserId"].ToString() != userId)
                        {
                            //string strMsg = Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "SessionIsNull");
                            string strMsg = "Please Login in!!!!";
                            Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
                        }
                    }

                checkAccount();               
            }
            setUserInfo();
        }
        catch (Exception ex)
        {
            logger.Error(methodName, ex);
        }
        finally
        {
            logger.InfoFormat("END: {0}()", methodName);
        }
    }



    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void setUserInfo()
    {
        userInfo.UserId = Request["UserId"];
        userInfo.UserName = HttpUtility.UrlDecode(Request["UserName"]);
        userInfo.Customer = Request["Customer"];
        userInfo.AccountId = long.Parse(Request["AccountId"]);
        userInfo.Login = Request["Login"];
        //userInfo.UserId = "0001";
        //userInfo.UserName = "admin";
        //userInfo.Customer = "HP";
        //userInfo.AccountId = 1111;
        //userInfo.Login = "admin";
    }

    private void checkAccount()
    {
        String strToken = Session["Token"].ToString();
        if (!string.IsNullOrEmpty(strToken))
        {
            IToken token = (IToken)WebCommonMethod.deserialize(strToken);
            //Init logon user information
            AuthorityManager authorManager = new AuthorityManager();
            try
            {
                com.inventec.RBPC.Net.entity.AccountInfo account = authorManager.getAccountByToken(token);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                //string strMsg = Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "SessionIsNull");
                string strMsg = "SessionIsNull";
                Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
            }
        }
        else
        {
            //string strMsg = Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "SessionIsNull");
            string strMsg = "SessionIsNull";
            Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
        }
    }
}
