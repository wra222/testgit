using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.RBPC.Net.entity;
using com.inventec.RBPC.Net.intf;
using com.inventec.RBPC.Net.datamodel.intf;
using System.Text.RegularExpressions;

namespace com.inventec.iMESWEB
{

    /// <summary>
    /// Summary description for IMESBasePage
    /// </summary>
    public class IMESBasePage : System.Web.UI.Page
    {
        public IMESBasePage()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public bool ValidatePrivilege(string pageSymbol)
        {
            //IToken MyToken = (IToken)HttpContext.Current.Session["TOKEN"];
            //IPermissionManager MyPermissionManager = RBPCAgent.getRBPCManager<IPermissionManager>();

            //ISecurityManager MySecurityManager = RBPCAgent.getRBPCManager<ISecurityManager>();
            //IAccountManager MyAccountManager = RBPCAgent.getRBPCManager<IAccountManager>();
            //AccountInfo MyAccout = MySecurityManager.getAccountByToken(MyToken);
            //RoleInfo[] MyRole = MyAccountManager.getRoles("IMES", MyAccout.Login);
            //if (MyRole == null)
            //{
            //    throw new Exception();
            //}

            //PermissionInfo[] permissions = MyPermissionManager.getPermissions(MyRole[0].Id);

            //if (permissions == null)
            //{
            //    throw new Exception();
            //}

            //for (int i = 0; i < permissions.Length; i++)
            //{
            //    if (permissions[i].Target.Symbol == "")
            //    {
            //        return true;
            //    }
            //}

            //System.Web.HttpContext.Current.Response.Redirect("NoPrivilegePage");
            return false;
        }

        private static Regex regxCQ = new Regex(@"^5CG[0-9]{3}[A-Z0-9]{4}$");
        private static Regex regxCQ2 = new Regex(@"^A5CG[0-9]{3}[A-Z0-9]{4}$");
        private static Regex regxSH = new Regex(@"^CNU[A-Z0-9]{7}$");
        private static Regex regxSH7cd = new Regex(@"^7CD[A-Z0-9]{7}$");
        public static bool CheckCustomerSN(string sn)
        {
            if (sn.Length == 11 && sn.Substring(0, 1) == "S")
                sn = sn.Substring(1, 10);
            return (regxSH.Match(sn).Success || regxSH7cd.Match(sn).Success || regxCQ.Match(sn).Success || regxCQ2.Match(sn).Success);
        }
    }
}