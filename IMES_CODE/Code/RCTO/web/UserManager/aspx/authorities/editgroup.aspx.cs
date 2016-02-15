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

public partial class webroot_aspx_authorities_editgroup : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(webroot_aspx_authorities_editgroup));

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(webroot_aspx_authorities_editgroup));
        //log.Debug("page load");
    }


    /*
     * Method Name :
     *		saveGroup
     *	
     * Description :
     *		保存(新增\修改)一个人员组
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
     *      2009-12
     */
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string saveGroup(string strGroupID, string strGroupName, string strComment, string editorID)
    {
        log.Debug("saveGroup");
        log.Debug("*-*-strGroupID=" + strGroupID);
        log.Debug("*-*-strGroupName=" + strGroupName);
        log.Debug("*-*-strComment=" + strComment);

        AuthorityManager authMgr = new AuthorityManager(editorID);

        string strSavedGroupID = authMgr.saveUserGroupItem(strGroupID, strGroupName, strComment);
        log.Debug("*-*-*-*--*-*-strSavedGroupID=" + strSavedGroupID);

        return strSavedGroupID;
        //manager method		
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
    public DataTable getGroupInfo(string strGroupID)
    {
        log.Debug("getGroupInfo");
        log.Debug("*-*-strGroupID=" + strGroupID);

        AuthorityManager authMgr = new AuthorityManager();
        DataTable dtGroupInfo = authMgr.getGroupInfoByID(strGroupID);

        return dtGroupInfo;
    }
}
