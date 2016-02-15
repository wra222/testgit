/*
 INVENTEC corporation (c)2009 all rights reserved. 
 Description: iMES Frame Page
             
 Update: 
 Date         Name                Reason 
 ==========   ==================  =====================================    
 2009-10-28   Li.Ming-Jun(eB1)    Create
 2009-02-04   Li.Ming-Jun(eB1)    Modify: ITC-1103-0150
 Known issues: 
 */
using System;
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
using com.inventec.RBPC.Net.datamodel.intf;
using com.inventec.RBPC.Net.entity;
using com.inventec.iMESWEB;

public partial class iMES_Frame : Page 
{
    string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string Domain = WebCommonMethod.getConfiguration("DocumentDomain");
    UserInfo userInfo = new UserInfo();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //解决跨域嵌套时iFrame子页面的Session无法访问
            Response.AddHeader("P3P", "CP=CAO PSA OUR");

            AjaxPro.Utility.RegisterTypeForAjax(typeof(iMES_Frame));

            //Get customer
            String strCustomer = Request["customer"];
            userInfo.Customer = strCustomer;

            //Get token
            String strToken = Request["Token"];
            IToken token = (IToken)WebCommonMethod.deserialize(strToken);

            //Init logon user information
            AuthorityManager authorManager = new AuthorityManager();
            AccountInfo account = authorManager.getAccountByToken(token);

            //Modify: ITC-1103-0150
            if (account.Login.Contains("\\"))
                userInfo.UserId = (String)account.Login.Split(new Char[] { '\\' })[1]; //itcxxxxxx
            else
                userInfo.UserId = (String)account.Login;

            userInfo.UserName = (String)account.Name;
            userInfo.Department = (String)account.Department;
            userInfo.Domain = (String)account.Domain;
            userInfo.Company = (String)account.Company;
            userInfo.AccountId = (long)account.Id;
            userInfo.Login = (String)account.Login; //itc\itcxxxxxx
            //Vincent add application info
            userInfo.Application = account.Application;

            Session["UserId"] = userInfo.UserId;

            //Init treeview
            TreeViewControl treeVC = new TreeViewControl(userInfo);
            treeVC.TreeNodePopulate(treeFunction.Nodes, token);
            Session["iMES_treeNodes"] = treeVC.getTreeNodes();
        }
    }

	//Get Function name by P Code
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public String getFunctionByPcode(String PCode)
    {
        String retValue = "";
        String strPCode = PCode.Trim();

        if (strPCode != "" && strPCode != null)
        {
            DataTable treeNodes = (DataTable)Session["iMES_treeNodes"];

            DataRow[] arrDr = treeNodes.Select("Pcode = '" + strPCode + "'");

            if (arrDr.Length > 0)
                retValue = arrDr[0]["Name"].ToString() + "|" + arrDr[0]["NavigateUrl"].ToString();
            else
                retValue = "Error|" + Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgNoPcode");
        }
        else
            retValue = "Error|" + Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPcodeIsNull");

        return retValue;
    }
}
