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

public partial class webroot_aspx_authorities_selectuser : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(webroot_aspx_authorities_selectuser));

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(webroot_aspx_authorities_selectuser));
        AjaxPro.Utility.RegisterTypeForAjax(typeof(com.inventec.imes.manager.AuthorityManager));
        log.Debug("page load");
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
        DataTable dtAllDomains = authMgr.getDomainsByApplication();
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
        DataTable dtCompanies = authMgr.getCompaniesByDomain(strDomain);
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
        DataTable dtDepts = authMgr.getDeptsByCompany(strDomain, strCompany,"");
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
        dtUsers = authMgr.getUsersByDepartment(strDomain, strCompany, strDepartment, strSearch);

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
    public string adduser_onClickOK(string strUserIDs, string strGroupName, string editorID)
    {
        log.Debug("*-*-strUserIDs=" + strUserIDs);
        string[] arrUserIDs = strUserIDs.Split(Constants.CHECKBOX_ITEM_DELIM.ToCharArray());
        log.Debug("*-*-arrUserIDs[0]=" + arrUserIDs[0]);
        //log.Debug("*-*-arrUserIDs[1]=" + arrUserIDs[1]);
        //log.Debug("*-*-arrUserIDs[2]=" + arrUserIDs[2]);

        AuthorityManager authMgr = new AuthorityManager(editorID);
        string result=authMgr.saveAddUsersToGroup(strUserIDs, strGroupName);

        return result;
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
