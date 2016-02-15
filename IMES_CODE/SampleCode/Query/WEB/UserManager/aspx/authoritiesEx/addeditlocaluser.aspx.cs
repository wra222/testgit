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
using Microsoft.VisualBasic;

using com.inventec.imes.manager;

public partial class webroot_aspx_authorities_addeditlocaluser : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(webroot_aspx_authorities_addeditlocaluser));

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(webroot_aspx_authorities_addeditlocaluser));
        //log.Debug("page load");
    }


    /*
     * Method Name :
     *		saveLocalUser
     *	
     * Description :
     *		保存(新增\修改)一个local user
     *	
     * Parameters: 
     *      string strLocalUserID: Local User ID。如果是新增操作，则为""
     *      
     *      string strLogin: Local User Login
     *      
     *      string strName: Local User name
     *      
     *      string strPassword: password
     *      
     *      string strCompany: company
     *      
     *      string strDepartment: department
     *      
     *      string strEMail：email
     *      
     * Return Value: 
     *      (string)成功保存了的Local User的ID
     * 
     * Remark: 
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
    public string saveLocalUser(string strLocalUserID, string strLogin, string strName, string strPassword, string strCompany, string strDepartment, string strEMail, string editorID)
    {
        log.Debug("saveLocalUser");

        log.Debug("id=" + strLocalUserID);
        log.Debug("strName=" + strName);
        log.Debug("strLogin=" + strLogin);
        log.Debug("strPassword=" + strPassword);
        log.Debug("strCompany=" + strCompany);
        log.Debug("strDepartment=" + strDepartment);
        log.Debug("strEMail=" + strEMail);

        AuthorityManager authMgr = new AuthorityManager(editorID);

        string strSavedLocalUserID = authMgr.saveLocalUser(strLocalUserID, strLogin, strName, strPassword, strCompany, strDepartment, strEMail);
        log.Debug("*-*-*-*--*-*-strSavedLocalUserID=" + strSavedLocalUserID);

        return strSavedLocalUserID;
        //manager method		
    }


    /*
     * Method Name :
     *		getLocalUser
     *	
     * Description :
     *		取得一个local user的信息
     *	
     * Parameters: 
     *      string strLocalUserID: Local User ID。
     *      
     * Return Value: 
     *      local user infos
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
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public DataTable getLocalUser(string strLocalUserID)
    {
        log.Debug("getLocalUser");
        log.Debug("*-*-strLocalUserID=" + strLocalUserID);

        AuthorityManager authMgr = new AuthorityManager();
        DataTable dtAccountInfo = authMgr.getLocalUserByID(strLocalUserID);

        return dtAccountInfo;
    }
}
