/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: editgroup.aspx.cs
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-11-19   itc98079     Create 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.system;
using log4net;
using com.inventec.system.exception;
using com.inventec.imes.manager;
using com.inventec.RBPC.Net.intf;
using com.inventec.RBPC.Net.entity;
using System.Text;

public partial class webroot_aspx_authorities_editgroup : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(webroot_aspx_authorities_editgroup));
    TreeViewControlEx treeVC;
    private string portalName = System.Configuration.ConfigurationManager.AppSettings["RBPCApplication"];
    com.inventec.iMESWEB.UserInfo userInfo = new com.inventec.iMESWEB.UserInfo();

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(webroot_aspx_authorities_editgroup));
        //log.Debug("page load");
        TreeView1.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");

        userInfo.Login = Session["accountauthoritylogin"] as string;
        userInfo.AccountId = (long) Session["accountauthorityid"];

        if (TreeView1.Nodes.Count == 0)
        {
            treeVC = new TreeViewControlEx(userInfo);
            TreeView1.Nodes.Clear();
            treeVC.TreeNodePopulate(TreeView1.Nodes);
            TreeView1.PathSeparator = ("|").ToCharArray()[0];
        }
        TreeView1.ExpandAll();

        if (!IsPostBack)
        {
            //TreeView1.Nodes[0].Text = portalName;
        }
    }

    private List<long> GetTreeChecked()
    {
        if (TreeView1.CheckedNodes.Count == 0)
            return null;

        Hashtable parentNodes = new Hashtable();
        AuthorityManager authMgr = new AuthorityManager(userInfo);
        List<PermissionInfo> lstAll = authMgr.accountRoleEx.GetOwnPermissionByLoginApplication(userInfo.Login, portalName);
        foreach (PermissionInfo ai in lstAll)
        {
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

    public void AddSaveGroupClick(object sender, System.EventArgs e)
    {
        string strGroupName = txtGroupName.Value;
        string strComment = txtComment.Value;
        log.Debug("AddsaveGroup");
        //log.Debug("*-*-strGroupID=" + strGroupID);
        log.Debug("*-*-strGroupName=" + strGroupName);
        log.Debug("*-*-strComment=" + strComment);

        //string strEditorID = (string)HttpContext.Current.Session[AttributeNames.USER_ID];

        AuthorityManager authMgr = new AuthorityManager(userInfo);

        /*string strSavedGroupID = authMgr.saveUserGroupItem(strGroupID, strGroupName, strComment, strEditorID);
        log.Debug("*-*-*-*--*-*-strSavedGroupID=" + strSavedGroupID);
        return strSavedGroupID;
        */

        string roleType = "0";
        //string editor = Session[AttributeNames.USER_CODE] as string;
        List<long> applicationList = GetTreeChecked();

        if (applicationList == null || applicationList.Count == 0)
        {
            ErrMsg("請勾選Menu!");
            return;
        }

        try
        {
            string accountauthoritylogin = Session["accountauthoritylogin"] as string;
            authMgr.accountRoleEx.CreateAndUpdateRoleAndRolePermission(strGroupName, portalName, roleType, strComment, applicationList, accountauthoritylogin);
        }
        catch (Exception ex)
        {
            ErrMsg("Err! " + ex.Message);
			return;
        }

        //return "";
        //manager method		
        Finish("Success !");
    }

    public void EditSaveGroupClick(object sender, System.EventArgs e)
    {
        string strGroupName = txtGroupName.Value;
        string strComment = txtComment.Value;
        string strOldGroupName = txtOldGroupName.Value;
        log.Debug("EditsaveGroup");
        //log.Debug("*-*-strGroupID=" + strGroupID);
        log.Debug("*-*-strGroupName=" + strGroupName);
        log.Debug("*-*-strOldGroupName=" + strOldGroupName);
        log.Debug("*-*-strComment=" + strComment);

        /*AuthorityManager authMgr = new AuthorityManager(editorID);
        string strSavedGroupID = authMgr.saveUserGroupItem(strGroupID, strGroupName, strComment);
        log.Debug("*-*-*-*--*-*-strSavedGroupID=" + strSavedGroupID);
        return strSavedGroupID;
		*/

        AuthorityManager authMgr = new AuthorityManager(userInfo);
		
		try
        {
            authMgr.accountRoleEx.RenameRole(strOldGroupName, strGroupName, portalName, strComment);
        }
        catch (Exception ex)
        {
            ErrMsg("Err! " + ex.Message);
			return;
        }

        Finish("Success !");
    }

    private void ErrMsg(string msg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("alert(\"" + msg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "ErrMsgAgent", scriptBuilder.ToString(), false);
    }

    private void Finish(string msg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("alert(\"" + msg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("Finish();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "FinishAgent", scriptBuilder.ToString(), false);
    }

    /*
     * Method Name :
     *		getGroupInfo
     *	
     * Description :
     *		取得一个组的信息
     *	
     * Parameters: 
     *      string strGroupID: Group ID。如果是新增操作，则为""
     *      
     *      string strGroupName: Group name
     *      
     *      string strComment: Comment
     *      
     * Return Value: 
     *      (string)成功保存了的Group的ID
     * 
     * Remark: 
     *      1）检查传入的Group name是否已经存在，
     *           如果不存在，则继续保存，否则取消保存动作
     * 
     * Example: 
     * 
     * Author: 
     *      itc98079
     * 
     * Date:
     *      2009-11
     */
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public DataTable getGroupInfo(string strGroupName)
    {
        log.Debug("getGroupInfo");
        log.Debug("*-*-strGroupName=" + strGroupName);

        AuthorityManager authMgr = new AuthorityManager(userInfo);

        //DataTable dtGroupInfo = authMgr.getGroupInfoByID(strGroupID);
		Hashtable h = new Hashtable();
		h.Add("name", strGroupName);
        DataTable dtGroupInfo = new DataTable();
        dtGroupInfo.Columns.Add("name", typeof(string));
        dtGroupInfo.Columns.Add("comment", typeof(string));
        List<RoleExInfo> lst = authMgr.accountRoleEx.FindRole(h);
        if (lst != null && lst.Count > 0)
        {
            RoleExInfo r = lst[0];
            dtGroupInfo.Rows.Add(r.Name, r.Descr);
        }

        return dtGroupInfo;
    }

    public void QueryTreeViewClick(object sender, System.EventArgs e)
    {
        AuthorityManager authMgr = new AuthorityManager(userInfo);

        //清空所有checkbox
        for (int i = 0; i < TreeView1.CheckedNodes.Count; i++)
        {
            TreeView1.CheckedNodes[i].Checked = false;
            i--;
        }
        TreeView1.Nodes[0].Checked = false;

        //取得高亮group对应的所有的permission
        //DataTable dtPrimPer = authMgr.getPrimaryPermissionsByGroupName(txtGroupName.Value);
        DataTable dtPrimPer = new DataTable();
        dtPrimPer.Columns.Add("name", typeof(string));
        /*List<PermissionInfo> lst = authMgr.accountRoleEx.GetPermissionByRole(txtGroupName.Value); ;
        foreach (PermissionInfo ai in lst)
        {
            dtPrimPer.Rows.Add(ai.Name);
        }
        */
        List<RoleExInfo> lst = authMgr.accountRoleEx.GetApplicationByRole(txtGroupName.Value); ;
        foreach (RoleExInfo ai in lst)
        {
            dtPrimPer.Rows.Add(ai.Application);
        }

        int count = 0;
        //循环permission，如果有permission name与treeview节点相同，则check该节点
        for (int i = 0; i < dtPrimPer.Rows.Count; i++)
        {
            string permissionName = (string)dtPrimPer.Rows[i]["name"];
            //循环treeview节点，找到与permission name相同的节点，跳出treeview循环，继续大循环permission
            for (int j = 0; j < TreeView1.Nodes[0].ChildNodes.Count; j++)
            {
                string tmp = TreeView1.Nodes[0].ChildNodes[j].Text;//FindNode(valuePath);
                if (tmp.Equals(permissionName))
                {
                    TreeView1.Nodes[0].ChildNodes[j].Checked = true;
                    count = count + 1;
                    break;
                }
                //TreeView1.Nodes[0].ChildNodes[i].Value
                //TreeView1.Nodes[0].ChildNodes.Count
            }
        }

        //如果所有的子节点都被check，则check父节点
        if (count == TreeView1.Nodes[0].ChildNodes.Count)
        {
            TreeView1.Nodes[0].Checked = true;
        }


    }

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

        //return authMgr.GetAllPermissionList();
        //string editor = Session[AttributeNames.USER_CODE] as string;
        string accountauthoritylogin = Session["accountauthoritylogin"] as string;
        DataTable dt = new DataTable();
        dt.Columns.Add("name", typeof(string));
        dt.Columns.Add("id", typeof(string));
		List<PermissionInfo> lst = authMgr.accountRoleEx.GetOwnPermissionByLoginApplication(accountauthoritylogin, portalName);
        //List<PermissionInfo> lst = authMgr.accountRoleEx.GetOwnPermissionByLoginRole(accountauthoritylogin, "", portalName);
        foreach (PermissionInfo ai in lst)
        {
            dt.Rows.Add(ai.Name, ai.Id);
        }
        return dt;
    }
}
