/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: selectuser.aspx.cs
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-11-19   itc98079        Create 
 * 按钮"add single user"和“add user”共用selectuser.aspx
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using com.inventec.RBPC.Net.entity;
using com.inventec.system;
using log4net;
using com.inventec.imes.manager;
using com.inventec.RBPC.Net.intf;

public partial class webroot_aspx_authorities_selectuser : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(webroot_aspx_authorities_selectuser));
    private string portalName = System.Configuration.ConfigurationManager.AppSettings["RBPCApplication"];

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(webroot_aspx_authorities_selectuser));
        AjaxPro.Utility.RegisterTypeForAjax(typeof(com.inventec.imes.manager.AuthorityManager));
        log.Debug("page load");
    }

    private DataTable GetTableFromList(ref List<string> lst, string columnname)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add(columnname, typeof(string));
		for (int i = 0; i < lst.Count; i++){
			dt.Rows.Add(lst[i]);
		}
		return dt;
	}

    /*
     * Method name: 
     *		getAllDomains
     * 
     * Description: 
     *		获取所有Domain
     * 
     * Parameters: 
     *      (NULL)
     * 
     * Return Value: 
     *      (DataTable) dtAllDomains
     * 
     * Remark: 
     * 
     * Example: 
     * 
     * Author: 
     *      itc98079
     * 
     * Date:
     *      2011-8-24
     */
    [AjaxPro.AjaxMethod()]
    public DataTable getAllDomainsByApplication()
    {
        AuthorityManager authMgr = new AuthorityManager();
        //DataTable dtAllDomains = authMgr.getDomainsByApplication();
        List<string> lst = authMgr.accountRoleEx.GetAccountFieldValue("domain");
        DataTable dtAllDomains = GetTableFromList(ref lst, "domain");
        return dtAllDomains;
    }

    /*
     * Method name: 
     *		getCompaniesByDomain
     * 
     * Description: 
     *		根据Domain获取所有Company
     * 
     * Parameters: 
     *      (string) strDomain
     * 
     * Return Value: 
     *      (DataTable) dtCompanies
     * 
     * Remark: 
     * 
     * Example: 
     * 
     * Author: 
     *      itc98079
     * 
     * Date:
     *      2011-8-24
     */
    [AjaxPro.AjaxMethod()]
    public DataTable getCompaniesByDomain(string strDomain)
    {
        log.Debug("getCompaniesByDomain");
        AuthorityManager authMgr = new AuthorityManager();
        //DataTable dtCompanies = authMgr.getCompaniesByDomain(strDomain);
        Hashtable h = new Hashtable();
        h.Add("domain", strDomain);
        List<string> lst = authMgr.accountRoleEx.GetAccountFieldValue("company", h);
        DataTable dtCompanies = GetTableFromList(ref lst, "company");

        return dtCompanies;
    }

    /*
     * Method name: 
     *		getDeptsByCompany
     * 
     * Description: 
     *		根据Company获取所有Department
     * 
     * Parameters: 
     *      (string) strCompany
     * 
     * Return Value: 
     *      (DataTable) dtDepts
     * 
     * Remark: 
     * 
     * Example: 
     * 
     * Author: 
     *      itc98079
     * 
     * Date:
     *      2011-8-24
     */
    [AjaxPro.AjaxMethod()]
    public DataTable getDeptsByCompany(string strDomain, string strCompany)
    {
        log.Debug("getDeptsByCompany");
        AuthorityManager authMgr = new AuthorityManager();
        //DataTable dtDepts = authMgr.getDeptsByCompany(strDomain, strCompany,"");
        Hashtable h = new Hashtable();
        h.Add("domain", strDomain);
        h.Add("company", strCompany);
        List<string> lst = authMgr.accountRoleEx.GetAccountFieldValue("department", h);
        DataTable dtDepts = GetTableFromList(ref lst, "department");

        return dtDepts;
    }

    /*
     * Method name: 
     *		getTableData
     * 
     * Description: 
     *		根据UserIDs进行保存
     * 
     * Parameters: 
     *      (string) parName,
     *      
     *		(string) parType
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
     *      2011-8-24
     */
    [AjaxPro.AjaxMethod()]
    public DataTable getTableData(string strDomain, string strCompany, string strDepartment, string strSearch)
    {
        log.Debug("getTableData()--strDomain=" + strDomain);
        log.Debug("getTableData()--strCompany=" + strCompany);
        log.Debug("getTableData()--strDepartment=" + strDepartment);
        log.Debug("getTableData()--strSearch=" + strSearch);
        //get the record set in mainpage
        AuthorityManager authMgr = new AuthorityManager();

        //define datatable for tableedit
        DataTable dtUsers = new DataTable();
        //dtUsers = authMgr.getUsersByDepartment(strDomain, strCompany, strDepartment, strSearch);
		dtUsers.Columns.Add(" ", typeof(string));
        dtUsers.Columns.Add("name", typeof(string));
        dtUsers.Columns.Add("login", typeof(string));
		dtUsers.Columns.Add("descr", typeof(string));
        dtUsers.Columns.Add("accounttype", typeof(string));
        dtUsers.Columns.Add("id", typeof(string));
		List<AccountInfo> lst = authMgr.accountRoleEx.FindAccount(strCompany, strDomain, strDepartment, strSearch, strSearch);
		foreach (AccountInfo ai in lst)
		{
            string icon = "";
            // 0:icon 1:name 2:login 3:descr 4:type 5:id
            dtUsers.Rows.Add(icon, ai.Name, ai.Login, ai.Domain + "/" + ai.Company + "/" + ai.Department, ai.Type, ai.Id);
		}

        //convert List<ConditionSet> to datatable
        //for (int i = 0; i < csRecord.Count; i++)
        //{
        //    //log.Debug("=====");
        //    ConditionSet cc = (ConditionSet)csRecord[i];
        //    DataRow tRow = dt.NewRow();
        //    tRow[0] = cc.Operate;
        //    tRow[1] = cc.Datatype;
        //    tRow[2] = cc.Values;
        //    tRow[3] = cc.IsEmpty;

        //    dt.Rows.Add(tRow);
        //}
        return dtUsers;
    }

    /*
     * Method name:
     *		adduser_onClickOK
     *
     * Description:
     *		根据UserIDs进行保存
     *
     * Parameters:
     *      (string) strUserIDs
     *      
     *      (string) strGroupID
     *
     * Return Value:
     *      (bool) bResult
     *
     * Remark:
     *
     * Example:
     *
     * Author:
     *      itc98079
     *
     * Date:
     *      2011-8-24
     */
    [AjaxPro.AjaxMethod()]
    public bool adduser_onClickOK(string strUserIDs, string strGroupName)
    {
        log.Debug("*-*-strUserIDs=" + strUserIDs);
        string[] arrUserIDs = strUserIDs.Split(Constants.CHECKBOX_ITEM_DELIM.ToCharArray());
        log.Debug("*-*-arrUserIDs[0]=" + arrUserIDs[0]);
        //log.Debug("*-*-arrUserIDs[1]=" + arrUserIDs[1]);
        //log.Debug("*-*-arrUserIDs[2]=" + arrUserIDs[2]);

        AuthorityManager authMgr = new AuthorityManager();
        //string result=authMgr.saveAddUsersToGroup(strUserIDs, strGroupName);
        string strEditor = Session["accountauthoritylogin"] as string;
	authMgr.accountRoleEx.CreateAndUpdateAccountRole(new List<string>(arrUserIDs), strGroupName, portalName, strEditor);

        return true;
    }

    /*
     * Method name: 
     *		singleuser_onClickOK
     * 
     * Description: 
     *		根据UserIDs进行保存
     * 
     * Parameters: 
     *      (string) strUserIDs
     * 
     * Return Value: 
     *      (bool) bResult
     * 
     * Remark: 
     * 
     * Example: 
     * 
     * Author: 
     *      itc98079
     * 
     * Date:
     *      2011-8-24
     */
    [AjaxPro.AjaxMethod()]
    public string singleuser_onClickOK(string strUserIDs, string editorID) //    public bool singleuser_onClickOK(string strUserIDs, string editorID)
    {
        log.Debug("*-*-strUserIDs=" + strUserIDs);
        string[] arrUserIDs = strUserIDs.Split(Constants.CHECKBOX_ITEM_DELIM.ToCharArray());
        log.Debug("*-*-arrUserIDs[0]=" + arrUserIDs[0]);
        //log.Debug("*-*-arrUserIDs[1]=" + arrUserIDs[1]);
        //log.Debug("*-*-arrUserIDs[2]=" + arrUserIDs[2]);

        AuthorityManager authMgr = new AuthorityManager(editorID);
        string bResult = authMgr.saveAssignUsersToRole(strUserIDs);
        //bool bResult = authMgr.saveAssignUsersToRole(strUserIDs);
        log.Debug("*-*-bResult=" + bResult);
        return bResult;
    }

    /*
     * Method name: 
     *		getEmptyTable
     * 
     * Description: 
     *		获取empty table
     * 
     * Parameters: 
     *      (N/A)
     * 
     * Return Value: 
     *      
     * 
     * Remark: 
     * 
     * Example: 
     * 
     * Author: 
     *      itc98079
     * 
     * Date:
     *      2011-8-24
     */
    [AjaxPro.AjaxMethod()]
    public DataTable getEmptyTable()
    {

        DataTable dt = new DataTable();

        DataColumn login = new DataColumn(Constants.TABLE_COLUMN_USER_LOGIN);
        DataColumn userName = new DataColumn(Constants.TABLE_COLUMN_USER_NAME);
        DataColumn userId = new DataColumn(Constants.TABLE_COLUMN_USER_ID);
        DataColumn domain = new DataColumn(Constants.TABLE_COLUMN_DOMAIN);
        DataColumn company = new DataColumn(Constants.TABLE_COLUMN_COMPANY);
        DataColumn department = new DataColumn(Constants.TABLE_COLUMN_USER_DEPARTMENT);
        DataColumn email = new DataColumn(Constants.TABLE_COLUMN_USER_EMAIL);
        dt.Columns.Add(login);
        dt.Columns.Add(userName);
        dt.Columns.Add(userId);
        dt.Columns.Add(domain);
        dt.Columns.Add(company);
        dt.Columns.Add(department);
        dt.Columns.Add(email);



        return dt;
    }

}
