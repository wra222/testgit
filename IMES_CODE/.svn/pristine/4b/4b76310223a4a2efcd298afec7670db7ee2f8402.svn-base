using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using com.inventec.RBPC.Net.datamodel.intf;
using com.inventec.RBPC.Net.entity;
using com.inventec.RBPC.Net.entity.Session;
using com.inventec.RBPC.Net.intf;
using com.inventec.RBPC.Net.datamodel.Session;

//using System;
//using System.Data;
//using System.Configuration;
//using System.Linq;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
//using System.Collections.Generic;
//using System.Web.Services.Protocols;
//using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using com.inventec.iMESWEB;
using System.Collections.Generic;

//ITC-1370-0004
/// <summary>
///AccountUser 的摘要说明
/// </summary>
/// 

namespace com.inventec.portal.dashboard
{

    [Serializable]
    public class UserAcountInfoDef
    {
        private String logonUser;

        public String LogonUser
        {
            get { return logonUser; }
            set { logonUser = value; }
        }

        private String isAuthorityUsermanager;

        public String IsAuthorityUsermanager
        {
            get { return isAuthorityUsermanager; }
            set { isAuthorityUsermanager = value; }
        }

        private String isAuthorityDashboard;

        public String IsAuthorityDashboard
        {
            get { return isAuthorityDashboard; }
            set { isAuthorityDashboard = value; }
        }

        public UserAcountInfoDef()
        {
            this.isAuthorityDashboard = "False";
            this.isAuthorityUsermanager = "False";
            this.logonUser = "";
        }

    }

    public class AccountUserInfo
    {
        public AccountUserInfo()
        {

        }

        [AjaxPro.AjaxMethod]
        public static UserAcountInfoDef GetLogonUser(String tokenString)
        {
            UserAcountInfoDef result = new UserAcountInfoDef();

            ISecurityManager securityManager = null;
            securityManager = RBPCAgent.getRBPCManager<ISecurityManager>();

            Token tokenObject = new Token();
            tokenObject = (Token)WebCommonMethod.deserialize(tokenString);
            AccountInfo accountInfo = securityManager.getAccountByToken(tokenObject);

            result.LogonUser = Null2String(accountInfo.Login); //itc\itcxxxxxx

            //string application = System.Configuration.ConfigurationManager.AppSettings.Get("RBPCApplication").ToString();
            //根据登入人取得权限列表
            //PermissionInfo[] accountInfoList = securityManager.getAuthorities(tokenObject,application);

            UserInfo userInfo = new UserInfo();
            userInfo.Login = accountInfo.Login;

            if (accountInfo.Login.Contains("\\"))
                userInfo.UserId = (String)accountInfo.Login.Split(new Char[] { '\\' })[1]; //itcxxxxxx
            else
                userInfo.UserId = (String)accountInfo.Login;

            userInfo.UserName = (String)accountInfo.Name;
            userInfo.Department = (String)accountInfo.Department;
            userInfo.Domain = (String)accountInfo.Domain;
            userInfo.Company = (String)accountInfo.Company;
            userInfo.AccountId = (long)accountInfo.Id;
            userInfo.Login = (String)accountInfo.Login; //itc\itcxxxxxx
            ///////////////////////

            AuthorityManager manager = new AuthorityManager(userInfo);
            List<string> accountInfoList = manager.getPrimaryPermissionsByUserID(accountInfo.Login);

            for (int i = 0; i < accountInfoList.Count; i++)
            {
                if (accountInfoList[i] == "Authority Manager")
                {
                    result.IsAuthorityUsermanager = "True";
                }

                if (accountInfoList[i] == "Maintain")
                {
                    result.IsAuthorityDashboard = "True";
                }

            }

            //Session["UserId"] = userInfo.UserId;
            return result;
        }

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }

    }

}