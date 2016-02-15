/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: webroot_aspx_authorities_accountauthority
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-11-19   itc204011     Create 
 * qa bug no:ITC-1103-0041
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using log4net;
using com.inventec.RBPC.Net.entity;
using com.inventec.system.exception;
using com.inventec.system;
using com.inventec.imes.manager;
using System.Text;
using com.inventec.RBPC.Net.intf;
using com.inventec.portal.dashboard;
using com.inventec.system.util;



public partial class webroot_aspx_authorities_accountauthority : System.Web.UI.Page
{
	private static readonly ILog log = LogManager.GetLogger(typeof(webroot_aspx_authorities_accountauthority));
    //DataTable dtPrimPer;
    TreeViewControl treeVC;
    public int DEFAULT_ROWS_UsersInSubSystem = 14;
    public int DEFAULT_ROWS_GroupsInSubSystem = 7;
    public int DEFAULT_ROWS_UsersInGroup = 7;
    com.inventec.iMESWEB.UserInfo userInfo = new com.inventec.iMESWEB.UserInfo();

    protected void Page_Load(object sender, EventArgs e)
	{
        try
        {
            userInfo = new com.inventec.iMESWEB.UserInfo();
            userInfo.Login = StringUtil.decode_URL(Request.QueryString["logonUser"]);

            IAccountManager securityManager = null;
            securityManager = RBPCAgent.getRBPCManager<IAccountManager>();

            string application = "";

            if (userInfo.Login.Contains("\\"))
            {
                application = Constants.APPLICATION_ALL;
            }
            else
            {
                application = System.Configuration.ConfigurationManager.AppSettings.Get("RBPCApplication").ToString();
            }

            AccountInfo account = securityManager.findAccountByLogin(userInfo.Login, application);

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

            //userInfo = Master.userInfo;
            hidEditorId.Value = userInfo.AccountId.ToString();
                        
            if (!Page.IsPostBack)
            {

                log.Debug("webroot_aspx_authorities_accountauthority");
                AjaxPro.Utility.RegisterTypeForAjax(typeof(webroot_aspx_authorities_accountauthority));
                bindUsersInSubSystemTable();
                bindGroupsTable();
                bindUsersInGroupTable();
            }

            TreeView1.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");

            if (TreeView1.Nodes.Count == 0)
            {
                treeVC = new TreeViewControl();
                TreeView1.Nodes.Clear();
                treeVC.TreeNodePopulate(TreeView1.Nodes);
                TreeView1.PathSeparator = ("|").ToCharArray()[0];
            }
            TreeView1.ExpandAll();
        }
            /*
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }*/
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }

        this.Title = "Dashboard Authority";
    }
    /*
    public void PopulateNode(Object source, TreeNodeEventArgs e)
    {
        TreeNode node = e.Node;

        char[] param = { '&' };
        String[] arrNodeValue = node.Split(param);

        treeVC.CreateTreeNode(e.Node, arrNodeValue[0], int.Parse(arrNodeValue[1]));

        return;
    }*/

    public void AllOverTree()   
    {
        for (int i = 0; i < TreeView1.Nodes[0].ChildNodes.Count; i++)
        {
            string aa = "0";
        }
        /*
        foreach (TreeNode node in TreeView1)   
        {
            if (node.ChildNodes.Count != 0)
            {
                //OperNodeByID(node.Nodes);
            }
            else
            {

            }   
        }  */ 
    }





    /*
     * Method name: 
     *		deleteDomainOrLocalUser
     * 
     * Description: 
     *		删除User表格（左半边）中的记录信息
     * 
     * Parameters: 
     *      (string) strLogin,
     *      
     * Return Value: 
     *      (DataTable) dtUsers
     * 
     * Remark: 
     * 
     * Example: 
     * 
     * Author: 
     *      itc98079
     * 
     * Date:
     *      2011-7
     */
    [AjaxPro.AjaxMethod()]
    public bool deleteDomainOrLocalUser(string strAcctId)
    {
        AuthorityManager authMgr = new AuthorityManager(userInfo);

        authMgr.deleteDomainOrLocalUser(strAcctId);
        return true;
    }
    

    /*
     * Method name: 
     *		getNodeData
     * 
     * Description: 
     *		获取permission tree的全部节点信息
     * 
     * Parameters: 
     *      (string) type,
     *      
     *		(string) uuid
     * 
     * Return Value: 
     *      (DataTable) node infos
     * 
     * Remark: 
     * 
     * Example: 
     * 
     * Author: 
     *      itc98079
     * 
     * Date:
     *      2009-11
     */

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public DataTable getNodeData()
    {
        AuthorityManager authMgr = new AuthorityManager(userInfo);

        //DataTable dt = new DataTable();


        //DataTable tmpDt = authMgr.GetAllPermissionList();

        /*dt = tmpDt.Clone();
        foreach (DataRow dr in tmpDt.Rows)
        {
            DataRow tmpDr = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; ++i)
            {
                tmpDr[i] = dr[i];
            }

            dt.Rows.Add(tmpDr);
        }*/

        return authMgr.GetAllPermissionList();
    }










    /*
     * Method name: 
     *		getSystemTimeOut
     * 
     * Description: 
     *		打开小对话框add user/add deparment/edit group/add group/add single user时，作为是否超时的依据
     * 
     * Parameters: 
     * 
     * Return Value: 
     *      (DataTable) dtUserAndGroup
     * 
     * Remark: 
     * 
     * Example: 
     * 
     * Author: 
     *      itc98079
     * 
     * Date:
     *      2011-9-5
     */
    [AjaxPro.AjaxMethod()]
    public DataTable getSystemTimeOut()
    {

        AuthorityManager authMgr = new AuthorityManager(userInfo);
        DataTable dtDomains = authMgr.getDomainsByApplication();

        return dtDomains;
    }
 



    //////////////////以下处理右边usersInGroup部分

    protected void addDeptInGroup_click(Object sender, EventArgs e)
    {

        bindUsersInSubSystemTable();
        //bindGroupsTable();
        bindUsersInGroupTable();

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "HighLightUsersInSubSystemTable", "HighLightUsersInSubSystemTable();HighLightUsersInGroupTable();", true);
    }

    protected void addUserInGroup_click(Object sender, EventArgs e)
    {

        bindUsersInSubSystemTable();
        bindUsersInGroupTable();

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "HighLightUsersInSubSystemTable", "HighLightUsersInSubSystemTable();HighLightUsersInGroupTable();", true);
    }

    protected void deleteUserOrDeptInGroup_click(Object sender, EventArgs e)
    {
        try
        {

            AuthorityManager authMgr = new AuthorityManager(userInfo);

            authMgr.deleteUserInGroup(hidUserLoginInGroup.Value, hidGroupId.Value, hidUserTypeInGroup.Value);

            bindUsersInSubSystemTable();
            bindUsersInGroupTable();

            ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "HighLightUsersInSubSystemTable", "HighLightUsersInSubSystemTable();HighLightUsersInGroupTable();", true);
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    protected void gdUsersInGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[2].Style.Add("display", "none");//login
        e.Row.Cells[5].Style.Add("display", "none");//type


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[5].Text.Trim() == Constants.RBPC_ACCOUNT_USER)
            {

                e.Row.Cells[0].Text = "<img src='../../images/DomainUser.bmp' />";
                e.Row.Cells[0].ToolTip = Constants.DOMAIN_USER_NAME;

            }
            /////////////local
            if (e.Row.Cells[5].Text.Trim() == Constants.DOMAIN_SELECT_ITEM_LOCAL)
            {

                e.Row.Cells[0].Text = "<img src='../../images/LocalUser.gif' />";
                e.Row.Cells[0].ToolTip = Constants.LOCAL_USER_NAME;

            }
            if (e.Row.Cells[5].Text.Trim() == Constants.RBPC_ACCOUNT_DEPARTMENT)
            {

                e.Row.Cells[0].Text = "<img src='../../images/department.bmp' />";
                e.Row.Cells[0].ToolTip = Constants.DEPARTMENT_NAME;

            }
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }

    protected void bindUsersInGroupTable()
    {
        try
        {
            AuthorityManager authMgr = new AuthorityManager(userInfo);
            DataTable dtUsersInGroup = authMgr.getUsersAndDeptsListByGroup(hidGroupName.Value);
            if (dtUsersInGroup != null && dtUsersInGroup.Rows.Count != 0)
            {

                for (int i = dtUsersInGroup.Rows.Count; i < DEFAULT_ROWS_UsersInGroup; i++)
                {
                    dtUsersInGroup.Rows.Add(dtUsersInGroup.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS_UsersInGroup; i++)
                {
                    dtUsersInGroup.Rows.Add(dtUsersInGroup.NewRow());
                }
            }

            gdUsersInGroup.DataSource = dtUsersInGroup;
            gdUsersInGroup.DataBind();
            setUsersInGroupColumnWidth();
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    private void setUsersInGroupColumnWidth()
    {
        gdUsersInGroup.HeaderRow.Cells[0].Width = Unit.Pixel(20);//symbol
        gdUsersInGroup.HeaderRow.Cells[1].Width = Unit.Pixel(140);//user_or_dept_name
        gdUsersInGroup.HeaderRow.Cells[3].Width = Unit.Pixel(80);//domain
        gdUsersInGroup.HeaderRow.Cells[4].Width = Unit.Pixel(80);//company
    }


    protected void btnRefreshUsersInGroup_Click(Object sender, EventArgs e)
    {
        DataTable dtUsersInGroup = new DataTable();

        try
        {
            bindUsersInGroupTable();
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }

        bindUsersInGroupTable();
        //ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "AddSave1Complete", "AddSave1Complete(\"" + strPartType + "\");", true);
    }
    //////////////////以上处理右边usersInGroup部分


    
    //////////////////以下处理右边group部分
    protected void gdGroupsInSubSystem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[2].Style.Add("display", "none");//id
        e.Row.Cells[6].Style.Add("display", "none");//authorId
        e.Row.Cells[8].Style.Add("display", "none");//type


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[8].Text.Trim() == Constants.RBPC_ACCOUNT_TYPE_GROUP)
            {

                e.Row.Cells[0].Text = "<img src='../../images/UserGroup.GIF' />";
                e.Row.Cells[0].ToolTip = Constants.GROUP_NAME;

            }
            if (e.Row.Cells[8].Text.Trim() == Constants.RBPC_ACCOUNT_TYPE_SINGLE_USER_GROUP)
            {

                e.Row.Cells[0].Text = "<img src='../../images/user.gif' />";
                e.Row.Cells[0].ToolTip = Constants.SINGLE_USER_NAME;

            }
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }

    protected void bindGroupsTable()
    {
        try
        {

            AuthorityManager authMgr = new AuthorityManager(userInfo);
            DataTable dtGroups = authMgr.getUserAndGroupList();
            if (dtGroups != null && dtGroups.Rows.Count != 0)
            {

                for (int i = dtGroups.Rows.Count; i < DEFAULT_ROWS_GroupsInSubSystem; i++)
                {
                    dtGroups.Rows.Add(dtGroups.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS_UsersInSubSystem; i++)
                {
                    dtGroups.Rows.Add(dtGroups.NewRow());
                }
            }

            gdGroupsInSubSystem.DataSource = dtGroups;
            gdGroupsInSubSystem.DataBind();
            setGroupsInSubSystemColumnWidth();
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    private void setGroupsInSubSystemColumnWidth()
    {
        gdGroupsInSubSystem.HeaderRow.Cells[0].Width = Unit.Pixel(20);//symbol
        gdGroupsInSubSystem.HeaderRow.Cells[1].Width = Unit.Pixel(140);//groupName
        gdGroupsInSubSystem.HeaderRow.Cells[3].Width = Unit.Pixel(85);//cdt
        gdGroupsInSubSystem.HeaderRow.Cells[4].Width = Unit.Pixel(85);//udt
        gdGroupsInSubSystem.HeaderRow.Cells[5].Width = Unit.Pixel(80);//author
        //gdGroupsInSubSystem.HeaderRow.Cells[7].Width = Unit.Pixel(100);//comment
    }


    protected void btnRefreshGroupInSubSystem_Click(Object sender, EventArgs e)
    {
        DataTable dtGroups = new DataTable();

        try
        {
            bindGroupsTable();
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }

        bindGroupsTable();
        //ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "AddSave1Complete", "AddSave1Complete(\"" + strPartType + "\");", true);
    }

    protected void addSingleUser_click(Object sender, EventArgs e)
    {

        bindUsersInSubSystemTable();
        bindGroupsTable();

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "HighLightGroupTable", "HighLightUsersInSubSystemTable();HighLightGroupTable(true);", true);
    }

    protected void addGroup_click(Object sender, EventArgs e)
    {

        //bindUsersInSubSystemTable();
        bindGroupsTable();

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "HighLightGroupTable", "HighLightGroupTable(true);", true);
    }
    protected void editGroup_click(Object sender, EventArgs e)
    {

        //bindUsersInSubSystemTable();
        bindGroupsTable();

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "HighLightGroupTable", "HighLightGroupTable();", true);
    }


    protected void deleteGroup_click(Object sender, EventArgs e)
    {
        try
        {

            AuthorityManager authMgr = new AuthorityManager(userInfo);

            authMgr.deleteUserGroupItem(hidGroupName.Value);

            bindUsersInSubSystemTable();
            bindGroupsTable();
            bindUsersInGroupTable();
            queryClick(sender, e);
            ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "HighLightGroupTable", "HighLightGroupTable();HighLightUsersInSubSystemTable();", true);
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    //////////////////以上处理右边group部分



    //////////////////以下处理左边用户部分
    protected void refreshUserInSubSystem_click(Object sender, EventArgs e)
    {
        bindUsersInSubSystemTable();
        hidUserId.Value = "";
        updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "HighLightUsersInSubSystemTable", "HighLightUsersInSubSystemTable();", true);
    }

    protected void gdUsersInSubSystem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[3].Style.Add("display", "none");//description
        e.Row.Cells[4].Style.Add("display", "none");//group
        e.Row.Cells[5].Style.Add("display", "none");//userType
        e.Row.Cells[6].Style.Add("display", "none");//userId


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[5].Text.Trim() == Constants.RBPC_ACCOUNT_USER)
            {

                e.Row.Cells[0].Text = "<img src='../../images/DomainUser.bmp' />";
                e.Row.Cells[0].ToolTip = Constants.DOMAIN_USER_NAME;

            }
            /////////////local
            if (e.Row.Cells[5].Text.Trim() == Constants.DOMAIN_SELECT_ITEM_LOCAL)
            {

                e.Row.Cells[0].Text = "<img src='../../images/LocalUser.gif' />";
                e.Row.Cells[0].ToolTip = Constants.LOCAL_USER_NAME;

            }
            if (e.Row.Cells[5].Text.Trim() == Constants.RBPC_ACCOUNT_DEPARTMENT)
            {

                e.Row.Cells[0].Text = "<img src='../../images/department.bmp' />";
                e.Row.Cells[0].ToolTip = Constants.DEPARTMENT_NAME;

            }
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }

    protected void bindUsersInSubSystemTable()
    {
        try
        {

            AuthorityManager authMgr = new AuthorityManager(userInfo);
            DataTable dtUsers = authMgr.getUsersInSubSystem(idSearchTxt.Text);
            if (dtUsers != null && dtUsers.Rows.Count != 0)
            {

                for (int i = dtUsers.Rows.Count; i < DEFAULT_ROWS_UsersInSubSystem; i++)
                {
                    dtUsers.Rows.Add(dtUsers.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS_UsersInSubSystem; i++)
                {
                    dtUsers.Rows.Add(dtUsers.NewRow());
                }
            }

            gdUsersInSubSystem.DataSource = dtUsers;
            gdUsersInSubSystem.DataBind();
            setUsersInSubSystemColumnWidth();
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    private void setUsersInSubSystemColumnWidth()
    {
        gdUsersInSubSystem.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        gdUsersInSubSystem.HeaderRow.Cells[1].Width = Unit.Pixel(110);
    }

    protected void addLocalUser_click(Object sender, EventArgs e)
    {

        bindUsersInSubSystemTable();

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "HighLightUsersInSubSystemTable", "HighLightUsersInSubSystemTable();", true);
    }

    protected void editLocalUser_click(Object sender, EventArgs e)
    {

        bindUsersInSubSystemTable();
        bindUsersInGroupTable();

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "HighLightUsersInSubSystemTable", "HighLightUsersInSubSystemTable();", true);
    }

    protected void deleteUserInSubSystem_click(Object sender, EventArgs e)
    {

        try
        {

            AuthorityManager authMgr = new AuthorityManager(userInfo);

            authMgr.deleteDomainOrLocalUser(hidUserId.Value);

            bindUsersInSubSystemTable();
            bindGroupsTable();
            bindUsersInGroupTable();
            queryClick(sender, e);
            ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "HighLightUsersInSubSystemTable", "HighLightUsersInSubSystemTable();HighLightGroupTable();", true);
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }
    //////////////////以上处理左边用户部分


    protected void btnRefreshUsersAndPermissionInGroup_Click(Object sender, EventArgs e)
    {
        bindUsersInGroupTable();
        queryClick(sender, e);

        //ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "btnRefreshUsersAndPermissionInGroup_Click_Complete", "btnRefreshUsersAndPermissionInGroup_Click_Complete();", true);

    }



    //以下处理tree
    public void queryClick(object sender, System.EventArgs e)
    {
        try
        {
            AuthorityManager authMgr = new AuthorityManager(userInfo);
            //TreeView1.ExpandAll();
            //清空所有checkbox
            for (int i = 0; i < TreeView1.CheckedNodes.Count; i++)
            {
                TreeView1.CheckedNodes[i].Checked = false;
                i--;
            }
            TreeView1.Nodes[0].Checked = false;

            if (hidGroupName.Value.Equals(""))
            {
                return;
            }
            //取得高亮group对应的所有的permission
            DataTable dtPrimPer = authMgr.getPrimaryPermissionsByGroupName(hidGroupName.Value);



            //循环permission，如果有permission name与treeview节点相同，则check该节点
            for (int i = 0; i < dtPrimPer.Rows.Count; i++)
            {
                string permissionType = (string)dtPrimPer.Rows[i]["type"];  // "/iMES/Operation/FA/Image Download Check"
                //permissionType = permissionType.Substring(1);               // "iMES/Operation/FA/Image Download Check"
                TreeNode tmp = TreeView1.FindNode(permissionType);
                tmp.Checked = true;

                if (tmp.Parent!=null)
                {
                    tmp.Parent.Checked = true;
                    for (int j = 0; j < tmp.Parent.ChildNodes.Count; j++)
                    {
                        if (!tmp.Parent.ChildNodes[j].Checked)
                        {
                            tmp.Parent.Checked = false;
                            break;
                        }
                    }

                    if (tmp.Parent.Checked == true)
                    {
                        tmp = tmp.Parent;
                        if (tmp.Parent!=null)
                        {
                            tmp.Parent.Checked = true;
                            for (int j = 0; j < tmp.Parent.ChildNodes.Count; j++)
                            {
                                if (!tmp.Parent.ChildNodes[j].Checked)
                                {
                                    tmp.Parent.Checked = false;
                                    break;
                                }
                            }
                            /*
                            if (tmp.Parent.Checked == true)
                            {
                                tmp = tmp.Parent;
                                CheckParents(tmp.Parent);
                            }*/
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }

        /*
        //如果所有的子节点都被check，则check父节点
        if (count == TreeView1.Nodes[0].ChildNodes.Count)
        {
            TreeView1.Nodes[0].Checked = true;
        }*/
    }


    public void saveClick(object sender, System.EventArgs e)
    {
        try
        {
            AuthorityManager authMgr = new AuthorityManager(userInfo);

            //DataTable dtPrimPer = authMgr.getPrimaryPermissionsByGroupName(txtGroupName.Value);

            for (int i = 0; i < TreeView1.CheckedNodes.Count; i++)
            {
                if (TreeView1.CheckedNodes[i].ChildNodes.Count > 0)
                {
                    TreeView1.CheckedNodes.RemoveAt(i);
                    i--;
                }

            }

            string[] arrPermissions = new string[TreeView1.CheckedNodes.Count];
            for (int i = 0; i < TreeView1.CheckedNodes.Count; i++)
            {
                arrPermissions[i] = TreeView1.CheckedNodes[i].Value;
            }

            //strPermissions:"name1,name2,name3,name4"
            bool rtn = authMgr.saveAuthority(arrPermissions, hidGroupId.Value);

        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "saveClick_Complete", "saveClick_Complete();", true);

    }
    //以上处理tree







    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        //scriptBuilder.AppendLine("clearAttributeList();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }



}
