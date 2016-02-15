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


public partial class webroot_aspx_authorities_accountauthority : System.Web.UI.Page
{
	private static readonly ILog log = LogManager.GetLogger(typeof(webroot_aspx_authorities_accountauthority));
    //DataTable dtPrimPer;
    TreeViewControlEx treeVC;
    public int DEFAULT_ROWS_UsersInSubSystem = 14;
    public int DEFAULT_ROWS_GroupsInSubSystem = 7;
    public int DEFAULT_ROWS_UsersInGroup = 7;
    com.inventec.iMESWEB.UserInfo userInfo = new com.inventec.iMESWEB.UserInfo();
    private string portalName = System.Configuration.ConfigurationManager.AppSettings["RBPCApplication"];

    protected void Page_Load(object sender, EventArgs e)
	{
        try
        {
            userInfo = Master.userInfo;
            hidEditorId.Value = Master.userInfo.AccountId.ToString();
            Session["accountauthoritylogin"] = Master.userInfo.Login;
            Session["accountauthorityid"] = Master.userInfo.AccountId;
            if (!Page.IsPostBack)
            {

                log.Debug("webroot_aspx_authorities_accountauthority");
                AjaxPro.Utility.RegisterTypeForAjax(typeof(webroot_aspx_authorities_accountauthority));
                bindGroupsTable();
                bindUsersInGroupTable();
            }

            TreeView1.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");

            if (TreeView1.Nodes.Count == 0)
            {
                treeVC = new TreeViewControlEx(userInfo);
                TreeView1.Nodes.Clear();
                treeVC.TreeNodePopulate(TreeView1.Nodes);
                TreeView1.PathSeparator = ("|").ToCharArray()[0];
            }
            TreeView1.ExpandAll();
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    public void AllOverTree()   
    {
        for (int i = 0; i < TreeView1.Nodes[0].ChildNodes.Count; i++)
        {
            string aa = "0";
        }
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
 




    protected void addUserInGroup_click(Object sender, EventArgs e)
    {

        bindUsersInGroupTable();
        
        //ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "HighLightUsersInSubSystemTable", "HighLightUsersInGroupTable();", true);
    }

    protected void deleteUserOrDeptInGroup_click(Object sender, EventArgs e)
    {
        try
        {

            AuthorityManager authMgr = new AuthorityManager(userInfo);

            //authMgr.deleteUserInGroup(hidUserLoginInGroup.Value, hidGroupId.Value, hidUserTypeInGroup.Value);
            authMgr.accountRoleEx.DeleteAccountRole(hidUserLoginInGroup.Value, hidGroupName.Value, portalName);

            bindUsersInGroupTable();
			
			//強制 refresh gdGroupsInSubSystem
			bindGroupsTable();

            ScriptManager.RegisterStartupScript(this.updatePanel4, typeof(System.Object), "HighLightUsersInSubSystemTable", "HighLightUsersInGroupTable();", true);
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
            /*if (e.Row.Cells[5].Text.Trim() == Constants.RBPC_ACCOUNT_USER)
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

            }*/
            if ("domain".Equals(e.Row.Cells[5].Text.Trim()))
            {
                e.Row.Cells[0].Text = "<img src='../../images/DomainUser.bmp' />";
                e.Row.Cells[0].ToolTip = "domain";
            }
            else if ("local".Equals(e.Row.Cells[5].Text.Trim()))
            {
                e.Row.Cells[0].Text = "<img src='../../images/LocalUser.gif' />";
                e.Row.Cells[0].ToolTip = "local";
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

    /*protected void bindUsersInGroupTable()
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
    }*/
    public DataTable getEmptyUserOrDeptListTableData()
    {
        DataTable dtUserOrDeptList = new DataTable();
        DataColumn icon = new DataColumn(" ");
        DataColumn userOrDept = new DataColumn("User");
        DataColumn userIDOrDeptPath = new DataColumn("User or Department");
        DataColumn domain = new DataColumn("Domain");
        DataColumn compnay = new DataColumn("Company");
        DataColumn type = new DataColumn("type");

        dtUserOrDeptList.Columns.Add(icon);
        dtUserOrDeptList.Columns.Add(userOrDept);
        dtUserOrDeptList.Columns.Add(userIDOrDeptPath);
        dtUserOrDeptList.Columns.Add(domain);
        dtUserOrDeptList.Columns.Add(compnay);
        dtUserOrDeptList.Columns.Add(type);

        return dtUserOrDeptList;
    }
    protected void bindUsersInGroupTable()
    {
        try
        {
            AuthorityManager authMgr = new AuthorityManager();
            DataTable dtUsersInGroup = getEmptyUserOrDeptListTableData();
            if (!string.IsNullOrEmpty(hidGroupName.Value))
            {
                List<AccountInfo> lst = authMgr.accountRoleEx.GetOwnAccountByRoleApp(hidGroupName.Value, portalName);
                foreach (AccountInfo ai in lst)
                {
                    string icon = "";
                    // 0:icon 1:userOrDept 2:userIDOrDeptPath 3:domain 4:compnay 5:type
                    dtUsersInGroup.Rows.Add(icon, ai.Name, ai.Login, ai.Domain, ai.Company, ai.Type);
                }
            }

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
        gdUsersInGroup.HeaderRow.Cells[0].Width = Unit.Pixel(10);//symbol
        gdUsersInGroup.HeaderRow.Cells[1].Width = Unit.Pixel(120);//user_or_dept_name
        gdUsersInGroup.HeaderRow.Cells[3].Width = Unit.Pixel(95);//domain
        gdUsersInGroup.HeaderRow.Cells[4].Width = Unit.Pixel(95);//company
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
        e.Row.Cells[7].Style.Add("display", "none");//
        e.Row.Cells[8].Style.Add("display", "none");//
        e.Row.Cells[9].Style.Add("display", "none");//


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
            if (e.Row.Cells[8].Text.Trim() == "2")
            {

                e.Row.Cells[0].Text = "<img src='../../images/SYS_cus_pro.gif' />";
                e.Row.Cells[0].ToolTip = "System Group";

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
            //DataTable dtGroups = authMgr.getUserAndGroupList();
            DataTable dtGroups = new DataTable();
            dtGroups.Columns.Add(" ", typeof(string));
            dtGroups.Columns.Add("User Group", typeof(string));
            dtGroups.Columns.Add("Author-login", typeof(string));
            dtGroups.Columns.Add("Author-name", typeof(string));
            dtGroups.Columns.Add("Create Time", typeof(string));
            dtGroups.Columns.Add("Update Time", typeof(string));
            dtGroups.Columns.Add("Comment", typeof(string));
            dtGroups.Columns.Add("id", typeof(string));
            dtGroups.Columns.Add("grouptype", typeof(string));
            dtGroups.Columns.Add("application", typeof(string));
            List<RoleExInfo> lst = authMgr.accountRoleEx.GetOwnRoleByLoginApplication(userInfo.Login, portalName);
            foreach (RoleExInfo ai in lst)
            {
                string icon = "";
                // 0:icon 1:rolename 2:authorlogin 3:authorname 4:cdt 5:udt 6:descr 7:id 8:type 9:application
                dtGroups.Rows.Add(icon, ai.Name, ai.EditorLogin, ai.EditorName, ai.Cdt, ai.Udt, ai.Descr, ai.Id, ai.Type, ai.Application);
            }

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
			
			gdGroupsInSubSystem.SelectedIndex = -1;
			hidGroupName.Value = "";
			//強制清空 gdUsersInGroup
			bindUsersInGroupTable();
			//強制清空 TreeView1
			queryClick(null, null);
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
        gdGroupsInSubSystem.HeaderRow.Cells[0].Width = Unit.Pixel(10);//symbol
        gdGroupsInSubSystem.HeaderRow.Cells[1].Width = Unit.Pixel(100);//groupName
        gdGroupsInSubSystem.HeaderRow.Cells[2].Width = Unit.Pixel(100);//
        gdGroupsInSubSystem.HeaderRow.Cells[3].Width = Unit.Pixel(100);//
        gdGroupsInSubSystem.HeaderRow.Cells[4].Width = Unit.Pixel(90);//
        gdGroupsInSubSystem.HeaderRow.Cells[5].Width = Unit.Pixel(90);//
        gdGroupsInSubSystem.HeaderRow.Cells[6].Width = Unit.Pixel(90);//comment
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


    protected void addGroup_click(Object sender, EventArgs e)
    {

        bindGroupsTable();

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "HighLightGroupTable", "HighLightGroupTable(true);", true);
    }
    protected void editGroup_click(Object sender, EventArgs e)
    {

        bindGroupsTable();

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "HighLightGroupTable", "HighLightGroupTable();", true);
    }


    protected void deleteGroup_click(Object sender, EventArgs e)
    {
        try
        {

            AuthorityManager authMgr = new AuthorityManager(userInfo);

            //authMgr.deleteUserGroupItem(hidGroupName.Value);
            List<string> lst = new List<string>();
            lst.Add(portalName);
            authMgr.accountRoleEx.DeleteRole(hidGroupName.Value, lst);

            bindGroupsTable();
            bindUsersInGroupTable();
            queryClick(sender, e);
            //ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "HighLightGroupTable", "HighLightGroupTable();", true);
            ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "HighLightGroupTable", "HighLightGroupTable(true);", true);
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    //////////////////以上处理右边group部分



    protected void btnRefreshUsersAndPermissionInGroup_Click(Object sender, EventArgs e)
    {
        bindUsersInGroupTable();
        queryClick(sender, e);

        //ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "btnRefreshUsersAndPermissionInGroup_Click_Complete", "btnRefreshUsersAndPermissionInGroup_Click_Complete();", true);

    }

    TreeNode FindNodeByValue(TreeNode tn, string val)
    {
        if (tn.Value.Equals(val))
            return tn;
        TreeNode ans = null;
        foreach (TreeNode sub in tn.ChildNodes)
        {
            TreeNode t = FindNodeByValue(sub, val);
            if (t != null)
            {
                ans = t;
                break;
            }
        }
        return ans;
    }

    //以下处理tree
    public void queryClick(object sender, System.EventArgs e)
    {
        try
        {
            if (TreeView1.Nodes.Count == 0)
                return;
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
            //DataTable dtPrimPer = authMgr.getPrimaryPermissionsByGroupName(hidGroupName.Value);
			DataTable dtPrimPer = new DataTable();
			dtPrimPer.Columns.Add("name", typeof(string));
            dtPrimPer.Columns.Add("type", typeof(string));
            dtPrimPer.Columns.Add("id", typeof(string));
            List<PermissionInfo> lst = authMgr.accountRoleEx.GetOwnPermissionByLoginRole(userInfo.Login, hidGroupName.Value, portalName);
            foreach (PermissionInfo ai in lst)
			{
				dtPrimPer.Rows.Add(ai.Name, ai.Type, ai.Id);
			}


            TreeNode rootNode =  TreeView1.Nodes[0];
            //循环permission，如果有permission name与treeview节点相同，则check该节点
            for (int i = 0; i < dtPrimPer.Rows.Count; i++)
            {
                string permissionId = (string)dtPrimPer.Rows[i]["id"];  // "/iMES/Operation/FA/Image Download Check"
                //permissionType = permissionType.Substring(1);               // "iMES/Operation/FA/Image Download Check"
                //TreeNode tmp = TreeView1.FindNode(permissionId);
                TreeNode tmp = FindNodeByValue(rootNode, permissionId);
                if (tmp == null)
                    continue;
                tmp.Checked = true;

                if (tmp.Parent != null)
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
                        if (tmp.Parent != null)
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


    /*public void saveClick(object sender, System.EventArgs e)
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

    }*/

    private List<long> GetTreeChecked()
    {
        if (TreeView1.CheckedNodes.Count == 0)
			return null;
		
		Hashtable parentNodes = new Hashtable();
        AuthorityManager authMgr = new AuthorityManager(userInfo);
		List<PermissionInfo> lstAll = authMgr.accountRoleEx.GetOwnPermissionByLoginRole(userInfo.Login, hidGroupName.Value, portalName);
        foreach (PermissionInfo ai in lstAll){
            if (string.IsNullOrEmpty(ai.Privilege.Privilege))
                parentNodes.Add(ai.Id.ToString(), null);
		}
		
		List<long> lst = new List<long>();
        for (int i = 0; i < TreeView1.CheckedNodes.Count; i++)
        {
            if (TreeView1.CheckedNodes[i].Value.Equals("Root"))
				continue;
            if (parentNodes.ContainsKey(TreeView1.CheckedNodes[i].Value))
                continue;
            lst.Add( Convert.ToInt32( TreeView1.CheckedNodes[i].Value ) );
        }
        return lst;
    }
    public void saveClick(object sender, System.EventArgs e)
    {
        AuthorityManager authMgr = new AuthorityManager();
        List<long> arrPermissions = GetTreeChecked();
        if (arrPermissions == null || arrPermissions.Count == 0)
        {
            showErrorMessage("請勾選Menu!");
            return;
        }
        string editor = Master.userInfo.Login;
        authMgr.accountRoleEx.CreateAndUpdateRoleAndRolePermission(hidGroupName.Value, portalName, hidGroupType.Value, hidGroupComment.Value, arrPermissions, editor);
        //bool rtn = authMgr.saveAuthority(arrPermissions, txtGroupId.Value);
        
		//強制 refresh gdGroupsInSubSystem
		bindGroupsTable();
		
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
