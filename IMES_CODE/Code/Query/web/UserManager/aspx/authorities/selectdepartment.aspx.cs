/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: selectuser.aspx.cs
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-12-19   itc204011     Create 
 * Known issues:Any restrictions about this file 
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

public partial class webroot_aspx_authorities_selectdepartment : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(webroot_aspx_authorities_selectdepartment));

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(webroot_aspx_authorities_selectdepartment));
        AjaxPro.Utility.RegisterTypeForAjax(typeof(com.inventec.imes.manager.AuthorityManager));
        log.Debug("webroot_aspx_authorities_selectdepartment : page load");
    }


    /*
         * 新增若干个部门
         * Parameters: 
         *      string strGroupID: Group ID。如果是新增操作，则为""
         *      
         *      string strGroupName: Group name
         *      
         *      string strComment: Comment
         *      
         *      string strEditorID：操作者的ID
         *      
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
         *      2011-8-24
         */
    [AjaxPro.AjaxMethod()]
    public bool onClickOK(string strDeptPaths, string strGroupID, string strDomain, string editorID)
    {
        log.Debug("*-*-strDeptPaths=" + strDeptPaths);
        string[] arrDeptPaths = strDeptPaths.Split(Constants.COL_DELIM.ToCharArray());
        log.Debug("*-*-arrDeptPaths[0]=" + arrDeptPaths[0]);

        AuthorityManager authMgr = new AuthorityManager(editorID);
        bool bResult = authMgr.saveAddDepartment(arrDeptPaths, strGroupID, strDomain);
        return true;
    }





    /*
    * Description: get All Domains
    * Parameters: (none)
    * Return Value: DataTable dtAllDomains
    */
    [AjaxPro.AjaxMethod()]
    public DataTable getAllDomains()
    {
        AuthorityManager authMgr = new AuthorityManager();
        DataTable dtAllDomains = authMgr.getDomainsByApplication();
        return dtAllDomains;
    }


    /*
    * Description: get Companies By Domain
    * Parameters: strDomain
    * Return Value: DataTable dtCompanies
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
    * Description: Get Table Data
    * Parameters: none
    * Return Value: DataTable
    */
    
    [AjaxPro.AjaxMethod()]
    public DataTable getTableData(string strDomain, string strCompany, string strSearch)
    {
        log.Debug("getTableData()--strDomain=" + strDomain);
        log.Debug("getTableData()--strCompany=" + strCompany);
        log.Debug("getTableData()--strSearch=" + strSearch);
        //get the record set in mainpage

        AuthorityManager authMgr = new AuthorityManager();

        //define datatable for tableedit
        DataTable dtDepts = new DataTable();
        DataColumn aDept = new DataColumn("aDept");
        DataColumn Department = new DataColumn("Department");
        DataColumn dept_path = new DataColumn("dept_path");

        dtDepts = authMgr.getDeptsByCompany(strDomain, strCompany, strSearch);

        dtDepts.Columns.Add(aDept);
        dtDepts.Columns.Add(Department);
        dtDepts.Columns.Add(dept_path);

        int dtDeptsCount = dtDepts.Rows.Count - 1;
        for (int i = dtDeptsCount; i >= 0; i--)
        {
            DataRow tmpRow = dtDepts.Rows[i];

            //Filters out any empty department's name from dtDepts
            if (tmpRow[0].ToString().Trim().Equals(""))
            {
                dtDepts.Rows.RemoveAt(i);
            }
            else
            {
                tmpRow[1] = tmpRow[0];
                tmpRow[2] = tmpRow[0];
                tmpRow[3] = strDomain + ";" + strCompany + ";" + tmpRow[0];
            }
        }
        dtDepts.Columns.Remove(Constants.TABLE_COLUMN_DEPARTMENT);
      
        return dtDepts;
    }
    /*
    [AjaxPro.AjaxMethod()]
    public DataTable getTableHeadData()
    {
        DataTable dtHeader = new DataTable();
        DataColumn operate = new DataColumn("c");
        DataColumn datatype = new DataColumn("User Name");
        DataColumn values = new DataColumn("userid");
        DataColumn isEmpty = new DataColumn("dept");

        dtHeader.Columns.Add(operate);
        dtHeader.Columns.Add(datatype);
        dtHeader.Columns.Add(values);
        dtHeader.Columns.Add(isEmpty);
        return dtHeader;
    }*/
}
