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
using IMES.Infrastructure;
using System.Collections.Generic;
using com.inventec.RBPC.Net.datamodel.intf;
using com.inventec.RBPC.Net.entity;

public partial class MasterPage : System.Web.UI.MasterPage
{
    private static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private bool displayInfoArea = true;
    protected string languagePre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_";
    public bool NeedPrint = true;
    public UserInfo userInfo = new UserInfo();
    
    //ITC-1103-0121 Tong.Zhi-Yong 2010-01-27
    public bool DisplayInfoArea
    {
        get { return displayInfoArea; }
        set { displayInfoArea = value; }
    }

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
                                                sessionId??"",
                                                userId??"");
           /*
            if (!Page.IsPostBack)
            {
                
                //string userId = Request["UserId"];
                //if (Session["UserId"] == null || Session["UserId"].ToString() == "")
                if (string.IsNullOrEmpty(sessionId))
                {
                    if (string.IsNullOrEmpty(userId) ||
                        Session == null ||
                        Session["UserId"] == null ||
                        Session["UserId"].ToString() != userId)
                    {
                        string strMsg = Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "SessionIsNull");
                        Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
                    }
                }
                else
                {
                    if (Session == null ||
                       Session[sessionId] == null ||
                       Session[sessionId].ToString() != sessionId ||
                       Session[sessionId + "-UserId"] == null ||
                       Session[sessionId+"-UserId"].ToString() != userId)
                    {
                        string strMsg = Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "SessionIsNull");
                        Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
                    }   
                }

                //checkAccount();               
            }
            */
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
        this.hiddenStation.Value = Request["Station"];
        this.hiddenPCode.Value = Request["PCode"];
        this.BtnDownloadBat.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(languagePre + "LabelGoToDownloadBatPage");
        lbtFreshPage.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(languagePre + "ReloadPage");
        
        if (!displayInfoArea)
        {
            this.MessageTextArea.Style.Add("display", "none");
            BtnDownloadBat.Style.Add("display", "none");
            this.lbtFreshPage.Style.Add("display", "none");
        }
    }

    private void setUserInfo()
    {
        //userInfo.UserId = Request["UserId"];
        //userInfo.UserName = HttpUtility.UrlDecode(Request["UserName"]);
        //userInfo.Customer = Request["Customer"];
        //userInfo.AccountId = long.Parse(Request["AccountId"]);
        //userInfo.Login = Request["Login"];
        userInfo.UserId = "dyh";
        userInfo.UserName = "dyh";
        userInfo.Customer = "HP";
        userInfo.AccountId = 1;
        userInfo.Login = "DYH";
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
                AccountInfo account = authorManager.getAccountByToken(token);
            }
            catch (Exception ex)
            {              
                logger.Error(ex);
                string strMsg = Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "SessionIsNull");
                Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
            }
        }
        else
        {
            string strMsg = Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "SessionIsNull");
            Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
        }
    }
}
