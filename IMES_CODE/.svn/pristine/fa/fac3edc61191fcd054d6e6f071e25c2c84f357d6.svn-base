

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

public partial class MasterPage : System.Web.UI.MasterPage
{
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
        // if (Session["UserId"] == null || Session["UserId"].ToString() == "")
        // {
            // string strMsg = Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "SessionIsNull");
            // Server.Transfer(Request.ApplicationPath + "/RedirectErrMsg.aspx?Message=" + HttpUtility.UrlEncode(strMsg));
        // }

        setUserInfo();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.hiddenStation.Value = Request["Station"];
        this.hiddenPCode.Value = Request["PCode"];
        this.BtnDownloadBat.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(languagePre + "BtnDownloadBat");
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
}
