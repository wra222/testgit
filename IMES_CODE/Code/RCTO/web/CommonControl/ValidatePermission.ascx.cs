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
using com.inventec.RBPC.Net.entity;
using com.inventec.RBPC.Net.intf;
using com.inventec.RBPC.Net.datamodel.intf;
using com.inventec.iMESWEB;
public partial class ValidatePermission : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidatePrivilege();
    }
    private bool ValidatePrivilege()
    {
        IToken MyToken = (IToken)HttpContext.Current.Session["TOKEN"];
        IPermissionManager MyPermissionManager = RBPCAgent.getRBPCManager<IPermissionManager>();

        ISecurityManager MySecurityManager = RBPCAgent.getRBPCManager<ISecurityManager>();
        IAccountManager MyAccountManager = RBPCAgent.getRBPCManager<IAccountManager>();

        if (MyToken == null)
        {
            throw new Exception("no token");
        }

        AccountInfo MyAccout = MySecurityManager.getAccountByToken(MyToken);
        RoleInfo[] MyRole = MyAccountManager.getRoles("IMES", MyAccout.Login);
        if (MyRole == null)
        {
            throw new Exception("no Role");
        }

        PermissionInfo[] permissions = MyPermissionManager.getPermissions(MyRole[0].Id);

        if (permissions == null)
        {
            throw new Exception();
        }

        for (int i = 0; i < permissions.Length; i++)
        {
            if (permissions[i].Target.Symbol == PageSymbol)
            {
                return true;
            }
        }

        return false;
    }

    public string PageSymbol
    {
        get
        {
            return this.HiddenPageSymbol.Value;
        }
        set
        {
            this.HiddenPageSymbol.Value = value;
        }
    }


}
