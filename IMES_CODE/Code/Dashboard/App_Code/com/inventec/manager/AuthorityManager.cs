/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: Account Manager
 *              
 * Update: 
 * Date         Name            Reason
 * ========== ================= =====================================
 * 2008-12-21   itc98079        Create 
 *
 * Known issues:
 * ITC-934-0101 ITC-1330-0073
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using com.inventec.imes.dao;
using com.inventec.RBPC.Net.datamodel.intf;
using com.inventec.RBPC.Net.entity;
using com.inventec.RBPC.Net.entity.Session;
using com.inventec.RBPC.Net.intf;
using com.inventec.system;
using com.inventec.system.exception;
using log4net;
using com.inventec.iMESWEB;
/// <summary>
///AuthorityManager 的摘要说明
/// </summary>
namespace com.inventec.imes.manager
{
    [Serializable]
    public class AuthorityManager
    {
        static IAccountManager accountMgr = null;
        static IPermissionManager permissionMgr = null;
        static IRoleManager roleMgr = null;
        static ISecurityManager securityMgr = null;
        string application;
        long editorID;
       // private UserInfo userInfo = new UserInfo();

        public AuthorityManager()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
            //TcpClientChannel channel = new TcpClientChannel();
            //ChannelServices.RegisterChannel(channel, false);

            //string strRBPCServiceURL = ConfigurationManager.AppSettings["RBPC.Service.url"].ToString();
            //string strRBPCServicePort = ConfigurationManager.AppSettings["RBPC.Service.port"].ToString();
            string strRBPCServiceURL = ConfigurationManager.AppSettings["RBPCServiceAddress"].ToString();
            string strRBPCServicePort = ConfigurationManager.AppSettings["RBPCServicePort"].ToString();
            //string tcpChannel = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/";


            string strAccountMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/IAccountManager";
            log.Debug("*-*-strAccountMgrURL=" + strAccountMgrURL);
            string strPermissionMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/IPermissionManager";
            log.Debug("*-*-strPermissionMgrURL=" + strPermissionMgrURL);
            string strRoleMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/IRoleManager";
            log.Debug("*-*-strRoleMgrURL=" + strRoleMgrURL);
            string strSecurityMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/ISecurityManager";
            log.Debug("*-*-strSecurityMgrURL=" + strSecurityMgrURL);


            accountMgr = (IAccountManager)Activator.GetObject(typeof(IAccountManager), strAccountMgrURL);
            permissionMgr = (IPermissionManager)Activator.GetObject(typeof(IPermissionManager), strPermissionMgrURL);
            roleMgr = (IRoleManager)Activator.GetObject(typeof(IRoleManager), strRoleMgrURL);
            securityMgr = (ISecurityManager)Activator.GetObject(typeof(ISecurityManager), strSecurityMgrURL);
            application = System.Configuration.ConfigurationManager.AppSettings.Get("RBPCApplication").ToString();

        }

        public AuthorityManager(UserInfo ui)
            : this()
        {
            string strRBPCServiceURL = ConfigurationManager.AppSettings["RBPCServiceAddress"].ToString();
            string strRBPCServicePort = ConfigurationManager.AppSettings["RBPCServicePort"].ToString();


            string strAccountMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/IAccountManager";
            string strPermissionMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/IPermissionManager";
            string strRoleMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/IRoleManager";
            string strSecurityMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/ISecurityManager";


            accountMgr = (IAccountManager)Activator.GetObject(typeof(IAccountManager), strAccountMgrURL);
            permissionMgr = (IPermissionManager)Activator.GetObject(typeof(IPermissionManager), strPermissionMgrURL);
            roleMgr = (IRoleManager)Activator.GetObject(typeof(IRoleManager), strRoleMgrURL);
            securityMgr = (ISecurityManager)Activator.GetObject(typeof(ISecurityManager), strSecurityMgrURL);
            application = System.Configuration.ConfigurationManager.AppSettings.Get("RBPCApplication").ToString();
            editorID = ui.AccountId;//31989
        }

        public AuthorityManager(string strEditorID)
        {
            string strRBPCServiceURL = ConfigurationManager.AppSettings["RBPCServiceAddress"].ToString();
            string strRBPCServicePort = ConfigurationManager.AppSettings["RBPCServicePort"].ToString();


            string strAccountMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/IAccountManager";
            string strPermissionMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/IPermissionManager";
            string strRoleMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/IRoleManager";
            string strSecurityMgrURL = "tcp://" + strRBPCServiceURL + ":" + strRBPCServicePort + "/ISecurityManager";


            accountMgr = (IAccountManager)Activator.GetObject(typeof(IAccountManager), strAccountMgrURL);
            permissionMgr = (IPermissionManager)Activator.GetObject(typeof(IPermissionManager), strPermissionMgrURL);
            roleMgr = (IRoleManager)Activator.GetObject(typeof(IRoleManager), strRoleMgrURL);
            securityMgr = (ISecurityManager)Activator.GetObject(typeof(ISecurityManager), strSecurityMgrURL);
            application = System.Configuration.ConfigurationManager.AppSettings.Get("RBPCApplication").ToString();
            // TODO: Complete member initialization
            editorID = Int64.Parse(strEditorID);
        }
        private static readonly ILog log = LogManager.GetLogger(typeof(AuthorityManager));


        /*
         * Method name: 
         *		getPrimaryPermissionsByGroupName
         * 
         * Description: 
         *		获取一个组所具有的primary权限
         * 
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
         *      (DataTable) Primary permissions
         * 
         * Remark: 
         *      1）检查传入的Group ID是否存在，
         *      2）做多7项，最少为0项
         * 
         * Example: 
         * 
         * Author: 
         *      98079
         * 
         * Date:
         *      2011-10
         */
        public DataTable getPrimaryPermissionsByGroupName(string strGroupName)
        {
            log.Debug("getPrimaryPermissionsByGroupID");
            if (null == strGroupName || strGroupName.Trim().Equals(""))
            {
                log.Debug("strGroupID=" + "\"\"");
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }
            /*
            AuthorityDao authDao = new AuthorityDao();
            string strRecordSum = authDao.getRecordSum("[T_Group]", " [id]='" + strGroupID + "'");

            if (strRecordSum.Equals("0"))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }*/

            //DataTable dtPrimaryPermissions = authDao.getPrimaryPermissions(strGroupID);

            DataTable dtPrimaryPermissions = roleMgr.getPermNamesByRoleNameAndPermType(application, strGroupName, application);
            log.Debug("dtPrimaryPermissions.Rows.Count=" + dtPrimaryPermissions.Rows.Count);

            return dtPrimaryPermissions;
        }


        /*Description: getDomainsByApplication
        * note:  应用于selectdepartment.aspx, selectuser.aspx下拉框取数据, 数据包括"local"+所有domain name
        * Return Value: string
         * 2011-8-24
         * itc98079
         */
        [AjaxPro.AjaxMethod()]
        public DataTable getDomainsByApplication()
        {
            log.Debug("getDomainsByApplication()");

            DataTable dt = new DataTable();

            DataColumn domain = new DataColumn(Constants.TABLE_COLUMN_DOMAIN);
            dt.Columns.Add(domain);

            try
            {

                List<string> dtAllDomains = accountMgr.getDomains("", Constants.APPLICATION_ALL);
                foreach (string eachDomain in dtAllDomains)
                {

                    DataRow tmpRow = dt.NewRow();
                    tmpRow[Constants.TABLE_COLUMN_DOMAIN] = eachDomain;
                    dt.Rows.Add(tmpRow);

                }
                
                List<string> dtLocalDomain = accountMgr.getDomains(Constants.DOMAIN_SELECT_ITEM_LOCAL, application);
                foreach (string eachDomain in dtLocalDomain)
                {

                    DataRow tmpRow = dt.NewRow();
                    tmpRow[Constants.TABLE_COLUMN_DOMAIN] = eachDomain;
                    dt.Rows.Add(tmpRow);

                }
            }
            catch (RBPCException e)
            {
                log.Debug("in getDomainsByApplication-->RBPCException2: " + e.Message);
            }
            catch (Exception e)
            {
                log.Debug("in getDomainsByApplication-->Exception2: " + e.Message);
            }
            log.Debug("getDomainsByApplication_end");

            return dt;
        }


        /*Description: getCompaniesByDomain
        * 
        * function : for selectdepartment.aspx, selectuser.aspx
        * 
        * date: 2011-8-24
        * 
        * author: itc98079
        */
        [AjaxPro.AjaxMethod()]
        public DataTable getCompaniesByDomain(string strDomain)
        {
            log.Debug("getCompaniesByDomain()");
            DataTable dt = new DataTable();

            DataColumn company = new DataColumn(Constants.TABLE_COLUMN_COMPANY);
            dt.Columns.Add(company);

            try
            {

                string tmpDomain, tmpApplication;
                if (strDomain.Equals(Constants.DOMAIN_SELECT_ITEM_LOCAL))
                {
                    tmpDomain = Constants.DOMAIN_SELECT_ITEM_LOCAL;
                    tmpApplication = application;
                }
                else
                {
                    tmpDomain = strDomain;
                    tmpApplication = Constants.APPLICATION_ALL;
                } 


                List<string> dtCompanies = accountMgr.getCompanies(tmpDomain, "", tmpApplication);
                foreach (string eachCompany in dtCompanies)
                {

                    DataRow tmpRow = dt.NewRow();
                    tmpRow[Constants.TABLE_COLUMN_COMPANY] = eachCompany;
                    dt.Rows.Add(tmpRow);

                }
            }
            catch (RBPCException e)
            {
                log.Debug("RBPCException message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }
            catch (Exception e)
            {
                log.Debug("Exception message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }
            return dt;
        }

        /*Description: get Depts By Company
         * Parameters: strCompany
         * note: 应用于selectuser.aspx下拉框取数据
         * Return Value: DataTable dtDepts
         * 2011-8-24
         * itc98079
        */
        [AjaxPro.AjaxMethod()]
        public DataTable getDeptsByCompany(string strDomain, string strCompany, string strSearch)
        {
            log.Debug("getDeptsByCompany()");
            DataTable dt = new DataTable();

            DataColumn department = new DataColumn(Constants.TABLE_COLUMN_DEPARTMENT);
            dt.Columns.Add(department);

            try
            {
                string tmpDomain, tmpApplication;
                if (strDomain.Equals(Constants.DOMAIN_SELECT_ITEM_LOCAL))
                {
                    tmpDomain = Constants.DOMAIN_SELECT_ITEM_LOCAL;
                    tmpApplication = application;
                }
                else
                {
                    tmpDomain = strDomain;
                    tmpApplication = Constants.APPLICATION_ALL;
                }

                List<string> Depts = accountMgr.getDepartments(tmpDomain, strCompany, strSearch, tmpApplication);
                foreach (string eachDepartment in Depts)
                {

                    DataRow tmpRow = dt.NewRow();
                    tmpRow[Constants.TABLE_COLUMN_DEPARTMENT] = eachDepartment;
                    dt.Rows.Add(tmpRow);

                }
            }
            catch (RBPCException e)
            {
                log.Debug("in getDeptsByCompany-->RBPCException2: " + e.Message);
            }
            catch (Exception e)
            {
                log.Debug("in getDeptsByCompany-->Exception2: " + e.Message);
            }
            return dt;
        }

        /*Description: get users in sub system
        * Parameters: strSearch
        * note: 应用于accountauthority.aspx左边用户列表表格取数据。
        * Return Value: DataTable dtUsers
         * 2011-8-24
         * itc98079
        */
        [AjaxPro.AjaxMethod()]
        public DataTable getUsersInSubSystem(string strSearch)
        {
            DataTable dt = new DataTable();


            DataColumn img = new DataColumn(" ");
            DataColumn userName = new DataColumn(Constants.TABLE_COLUMN_USER_NAME);
            DataColumn login = new DataColumn(Constants.TABLE_COLUMN_USER_LOGIN);
            DataColumn description = new DataColumn(Constants.TABLE_COLUMN_USER_DESCRIPTION);
            DataColumn group = new DataColumn(Constants.TABLE_COLUMN_USER_GROUP);
            DataColumn userType = new DataColumn(Constants.RBPC_ACCOUNT_USER_TYPE);
            DataColumn userId = new DataColumn(Constants.TABLE_COLUMN_USER_ID);
            
            dt.Columns.Add(img);
            dt.Columns.Add(userName);
            dt.Columns.Add(login);
            dt.Columns.Add(description);
            dt.Columns.Add(group);
            dt.Columns.Add(userType);
            dt.Columns.Add(userId);

            try
            {
                //acctApplication=all的部门
                DataTable dtUserIDs = roleMgr.getAcctLoginsByDoubleAppAndAccountType(application, Constants.APPLICATION_ALL, Constants.RBPC_ACCOUNT_DEPARTMENT, strSearch);

                for (int i = 0; i < dtUserIDs.Rows.Count; i++)
                {

                    AccountInfo acct = accountMgr.findAccountByLogin(dtUserIDs.Rows[i][0].ToString(), Constants.APPLICATION_ALL);
                    
                    RoleInfo[] roleInfos = accountMgr.getRolesByApp(Constants.APPLICATION_ALL, acct.Login, application);
                    string strGroup = roleInfos[0].Name;
                    for (int k = 1; k < roleInfos.Length; k++)
                    {
                        strGroup = strGroup + "/" + roleInfos[k].Name;
                    }
                    
                    DataRow tmpRow = dt.NewRow();

                    tmpRow[" "] = "";

                    tmpRow[Constants.TABLE_COLUMN_USER_NAME] = acct.Name;


                    tmpRow[Constants.TABLE_COLUMN_USER_LOGIN] = "";// acct.Login;

                    String[] strAcct = acct.Login.Split((";").ToCharArray());

                    tmpRow[Constants.TABLE_COLUMN_USER_DESCRIPTION] = strAcct[0] + "/" + strAcct[1];

                    tmpRow[Constants.TABLE_COLUMN_USER_GROUP] = strGroup;

                    tmpRow[Constants.RBPC_ACCOUNT_USER_TYPE] = Constants.RBPC_ACCOUNT_DEPARTMENT;

                    tmpRow[Constants.TABLE_COLUMN_USER_ID] = acct.Id;
                    dt.Rows.Add(tmpRow);
                }

                //acctApplication=application的部门
                dtUserIDs = roleMgr.getAcctLoginsByDoubleAppAndAccountType(application, application, Constants.RBPC_ACCOUNT_DEPARTMENT, strSearch);

                for (int i = 0; i < dtUserIDs.Rows.Count; i++)
                {

                    AccountInfo acct = accountMgr.findAccountByLogin(dtUserIDs.Rows[i][0].ToString(), application);

                    RoleInfo[] roleInfos = accountMgr.getRolesByApp(application, acct.Login, application);
                    string strGroup = roleInfos[0].Name;
                    for (int k = 1; k < roleInfos.Length; k++)
                    {
                        strGroup = strGroup + "/" + roleInfos[k].Name;
                    }

                    DataRow tmpRow = dt.NewRow();

                    tmpRow[" "] = "";

                    tmpRow[Constants.TABLE_COLUMN_USER_NAME] = acct.Name;


                    tmpRow[Constants.TABLE_COLUMN_USER_LOGIN] = "";// acct.Login;

                    String[] strAcct = acct.Login.Split((";").ToCharArray());

                    tmpRow[Constants.TABLE_COLUMN_USER_DESCRIPTION] = strAcct[0] + "/" + strAcct[1];

                    tmpRow[Constants.TABLE_COLUMN_USER_GROUP] = strGroup;

                    tmpRow[Constants.RBPC_ACCOUNT_USER_TYPE] = Constants.RBPC_ACCOUNT_DEPARTMENT;

                    tmpRow[Constants.TABLE_COLUMN_USER_ID] = acct.Id;
                    dt.Rows.Add(tmpRow);
                }


                //acctApplication=all的用户
                dtUserIDs = roleMgr.getAcctLoginsByDoubleAppAndAccountType(application, Constants.APPLICATION_ALL, Constants.RBPC_ACCOUNT_USER, strSearch);

                for (int i = 0; i < dtUserIDs.Rows.Count; i++)
                {

                    AccountInfo acct = accountMgr.findAccountByLogin(dtUserIDs.Rows[i][0].ToString(), Constants.APPLICATION_ALL);

                    RoleInfo[] roleInfos = accountMgr.getRolesByApp(Constants.APPLICATION_ALL, acct.Login, application);
                    string strGroup = roleInfos[0].Name;
                    for (int k = 1; k < roleInfos.Length; k++)
                    {
                        strGroup = strGroup + "/" + roleInfos[k].Name;
                    }



                    DataRow tmpRow = dt.NewRow();

                    tmpRow[" "] = "";

                    tmpRow[Constants.TABLE_COLUMN_USER_NAME] = acct.Name;


                    tmpRow[Constants.TABLE_COLUMN_USER_LOGIN] = acct.Login;

                    tmpRow[Constants.TABLE_COLUMN_USER_DESCRIPTION] = acct.Domain + "/" + acct.Company + "/" + acct.Department;

                    tmpRow[Constants.TABLE_COLUMN_USER_GROUP] = strGroup;

                    tmpRow[Constants.RBPC_ACCOUNT_USER_TYPE] = Constants.RBPC_ACCOUNT_USER;

                    tmpRow[Constants.TABLE_COLUMN_USER_ID] = acct.Id;
                    dt.Rows.Add(tmpRow);
                }

                //acctApplication=application的用户
                //dtUserIDs = roleMgr.getAcctLoginsByDoubleAppAndAccountType(application, application, Constants.RBPC_ACCOUNT_USER);
                AccountInfo[] acctInfos = accountMgr.getAccountsByApplication(application, strSearch);

                for (int i = 0; i < acctInfos.Length; i++)
                {
                    
                    AccountInfo acct = acctInfos[i];
                    //过滤掉local部门的记录
                    if (acct.AccountType.Equals(Constants.RBPC_ACCOUNT_DEPARTMENT))
                    {
                        continue;
                    }
                    RoleInfo[] roleInfos = accountMgr.getRolesByApp(application, acct.Login, application);
                    string strGroup = "";
                    if (roleInfos != null)
                    {
                        strGroup = roleInfos[0].Name;
                        for (int k = 1; k < roleInfos.Length; k++)
                        {
                            strGroup = strGroup + "/" + roleInfos[k].Name;
                        }
                    }

                    DataRow tmpRow = dt.NewRow();

                    tmpRow[" "] = "";

                    tmpRow[Constants.TABLE_COLUMN_USER_NAME] = acct.Name;


                    tmpRow[Constants.TABLE_COLUMN_USER_LOGIN] = acct.Login;

                    tmpRow[Constants.TABLE_COLUMN_USER_DESCRIPTION] = acct.Domain + "/" + acct.Company + "/" + acct.Department;

                    tmpRow[Constants.TABLE_COLUMN_USER_GROUP] = strGroup;

                    tmpRow[Constants.RBPC_ACCOUNT_USER_TYPE] = Constants.DOMAIN_SELECT_ITEM_LOCAL;

                    tmpRow[Constants.TABLE_COLUMN_USER_ID] = acct.Id;
                    dt.Rows.Add(tmpRow);
                }
            
            }
            catch (RBPCException e)
            {
                log.Debug("in getUsersInSubSystem-->RBPCException2: " + e.Message);
            }
            catch (Exception e)
            {
                log.Debug("in getUsersInSubSystem-->Exception2: " + e.Message);
            }

            return dt;
        }

        /*Description: get users By Department
        * Parameters: strDepartment
        * note: 应用于selectuser.aspx表格取数据。域用户从application=all取得，本地用户从具体application&domain=local取得
        * Return Value: DataTable dtUsers
         * 2011-8-24
         * itc98079
        */
        [AjaxPro.AjaxMethod()]
        public DataTable getUsersByDepartment(string strDomain, string strCompany, string strDepartment, string strSearch)
        {
            log.Debug("getUsersByDepartment()");
            if (null == strDomain || strDomain.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            if (null == strCompany)
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            if (null == strDepartment)
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

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
            try
            {
                string tmpDomain, tmpApplication, tmpDepartment;
                List<AccountInfo> dtUsers;

                if (strDomain.Equals(Constants.DOMAIN_SELECT_ITEM_LOCAL))
                {
                    tmpDomain = Constants.DOMAIN_SELECT_ITEM_LOCAL;
                    tmpApplication = application;
                }
                else
                {
                    tmpDomain = strDomain;
                    tmpApplication = Constants.APPLICATION_ALL;
                }

                if (strDepartment.Equals(Constants.ALL_OPTION))
                {
                    tmpDepartment = null;
                }
                else
                {
                    tmpDepartment = strDepartment;
                }

                dtUsers = accountMgr.getAccountsByApplication(tmpDomain, strCompany, tmpDepartment, strSearch, Constants.RBPC_ACCOUNT_USER, tmpApplication);

                foreach (AccountInfo eachUser in dtUsers)
                {

                    DataRow tmpRow = dt.NewRow();
                    string tmpLogin = (string)eachUser.Login;
                    int n = tmpLogin.IndexOf("\\");
                    if (n > 0)
                    {
                        tmpLogin = tmpLogin.Substring(n + 1, tmpLogin.Length-n-1).ToLower();
                    }

                    tmpRow[Constants.TABLE_COLUMN_USER_LOGIN] = tmpLogin;
                    tmpRow[Constants.TABLE_COLUMN_USER_NAME] = (string)eachUser.Name;
                    tmpRow[Constants.TABLE_COLUMN_USER_ID] = eachUser.Id.ToString();
                    tmpRow[Constants.TABLE_COLUMN_DOMAIN] = (string)eachUser.Domain;
                    tmpRow[Constants.TABLE_COLUMN_COMPANY] = (string)eachUser.Company;
                    tmpRow[Constants.TABLE_COLUMN_USER_DEPARTMENT] = (string)eachUser.Department;
                    tmpRow[Constants.TABLE_COLUMN_USER_EMAIL] = (string)eachUser.Email;
                    dt.Rows.Add(tmpRow);

                }

            }
            catch (RBPCException e)
            {
                log.Debug("in getUsersByDepartment-->RBPCException2: " + e.Message);
            }
            catch (Exception e)
            {
                log.Debug("in getUsersByDepartment-->Exception2: " + e.Message);
            }
            log.Debug("getUsersByDepartment--dt=" + dt);

            return dt;
        }

        /*Description: deleteDomainOrLocalUser
        * Parameters: strLogin
        * note: 应用于authorityManager.aspx左边区域表格删除数据
        * Return Value: bool
         * 2011-7-3
         * itc98079
        */
        [AjaxPro.AjaxMethod()]
        public bool deleteDomainOrLocalUser(string strAcctId)
        {
            log.Debug("deleteDomainOrLocalUser");
            log.Debug("*-*-strAcctId=" + strAcctId);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    AccountInfo acctInfo = accountMgr.findAccountById(Int64.Parse(strAcctId));

                    if (acctInfo.Type.Equals(Constants.DOMAIN_SELECT_ITEM_LOCAL))//如果是本地用户，删除该用户，同时如果是singleuser,从role表删除，解除accountRole关系
                    {
                        accountMgr.deleteAccount(Int64.Parse(strAcctId));
                        RoleInfo roleInfo = roleMgr.findRoleByName(application, acctInfo.Name);
                        if (roleInfo != null && roleInfo.Type.Equals(Constants.RBPC_ACCOUNT_TYPE_SINGLE_USER_GROUP))
                        {
                            roleMgr.deleteRole(roleInfo.Id);
                        }

                    }
                    else//域用户，解除accountRole关系
                    {
                        RoleInfo[] roleInfo = accountMgr.getRolesByApp(Constants.APPLICATION_ALL, acctInfo.Login, application);
                        for (int i = 0; i < roleInfo.Length; i++)
                        {
                            //如果是singleuser，删除role，同时也就解除了关系
                            if (roleInfo[i].Type.Equals(Constants.RBPC_ACCOUNT_TYPE_SINGLE_USER_GROUP))
                            {
                                roleMgr.deleteRole(roleInfo[i].Id);
                            }
                            else//否则只解除关系
                            {
                                accountMgr.removeRole(acctInfo.Id, roleInfo[i].Id, editorID);
                            }
                        }


                    }
                    scope.Complete();
                }
                catch (RBPCException e)
                {
                    log.Debug("RBPCException message=" + e.Message);
                    ExceptionManager.Throw(e.Message);
                }
                catch (Exception e)
                {
                    log.Debug("Exception message=" + e.Message);
                    ExceptionManager.Throw(e.Message);
                }
            }
            return true;

        }
    


        /*Description: getAllDomainsExceptLocal
        * 
        * function : for AddDomainUser.aspx, application="all"
        * 
        * date: 2011-7-8
        * 
        * author: itc98079
        */
        [AjaxPro.AjaxMethod()]
        public DataTable getAllDomainsExceptLocal()
        {
            log.Debug("getAllDomainsExceptLocal()");

            DataTable dt = new DataTable();

            DataColumn domain = new DataColumn(Constants.TABLE_COLUMN_DOMAIN);
            dt.Columns.Add(domain);

            try
            {
            
                List<string> dtAllDomains = accountMgr.getDomains("", Constants.APPLICATION_ALL);
                foreach (string eachDomain in dtAllDomains)
                {
                    if (!eachDomain.Trim().Equals(Constants.TABLE_COLUMN_LOCAL))
                    {
                        DataRow tmpRow = dt.NewRow();
                        tmpRow[Constants.TABLE_COLUMN_DOMAIN] = eachDomain;
                        dt.Rows.Add(tmpRow);
                    }

                }
            }
            catch (RBPCException e)
            {
                log.Debug("RBPCException message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }
            catch (Exception e)
            {
                log.Debug("Exception message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }

            log.Debug("getAllDomainsExceptLocal_end");

            return dt;
        }





        /*Description: getDeptsByCompanyInAll
        * 
        * function : for AddDomainUser.aspx, application="all"
        * 
        * date: 2011-7-8
        * 
        * author: itc98079
        */
        [AjaxPro.AjaxMethod()]
        public DataTable getDeptsByCompanyInAll(string strDomain, string strCompany, string strSearch)
        {
            log.Debug("getDeptsByCompanyInAll()");
            DataTable dt = new DataTable();

            DataColumn department = new DataColumn(Constants.TABLE_COLUMN_DEPARTMENT);
            dt.Columns.Add(department);
            try
            {
                List<string> Depts = accountMgr.getDepartments(strDomain, strCompany, strSearch, Constants.APPLICATION_ALL);
                foreach (string eachDepartment in Depts)
                {

                    DataRow tmpRow = dt.NewRow();
                    tmpRow[Constants.TABLE_COLUMN_DEPARTMENT] = eachDepartment;
                    dt.Rows.Add(tmpRow);

                }
            }
            catch (RBPCException e)
            {
                log.Debug("RBPCException message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }
            catch (Exception e)
            {
                log.Debug("Exception message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }
            return dt;
        }

        /*Description: getUsersByDepartmentInAll
        * 
        * function : for AddDomainUser.aspx, application="all"
        * 
        * date: 2011-7-8
        * 
        * author: itc98079
        */
        [AjaxPro.AjaxMethod()]
        public DataTable getUsersByDepartmentInAll(string strDomain, string strCompany, string strDepartment, string strSearch)
        {
            log.Debug("getUsersByDepartmentInAll()");
            if (null == strDomain || strDomain.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            if (null == strCompany)
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            if (null == strDepartment)
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

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

            try
            {
                
                List<AccountInfo> dtUsers;
                if (strDepartment.Equals(Constants.ALL_OPTION))
                {
                    dtUsers = accountMgr.getAccountsByApplication(strDomain, strCompany, null, strSearch, Constants.RBPC_ACCOUNT_USER, Constants.APPLICATION_ALL);
                }
                else
                {
                    dtUsers = accountMgr.getAccountsByApplication(strDomain, strCompany, strDepartment, strSearch, Constants.RBPC_ACCOUNT_USER, Constants.APPLICATION_ALL);
                }
                foreach (AccountInfo eachUser in dtUsers)
                {

                    DataRow tmpRow = dt.NewRow();
                    string tmpLogin = (string)eachUser.Login;
                    int n = tmpLogin.IndexOf("\\");
                    if (n > 0)
                    {
                        tmpLogin = tmpLogin.Substring(n + 1, tmpLogin.Length - n - 1).ToLower();
                    }

                    tmpRow[Constants.TABLE_COLUMN_USER_LOGIN] = tmpLogin;
                    tmpRow[Constants.TABLE_COLUMN_USER_NAME] = (string)eachUser.Name;
                    tmpRow[Constants.TABLE_COLUMN_USER_ID] = eachUser.Id.ToString();
                    tmpRow[Constants.TABLE_COLUMN_DOMAIN] = (string)eachUser.Domain;
                    tmpRow[Constants.TABLE_COLUMN_COMPANY] = (string)eachUser.Company;
                    tmpRow[Constants.TABLE_COLUMN_USER_DEPARTMENT] = (string)eachUser.Department;
                    tmpRow[Constants.TABLE_COLUMN_USER_EMAIL] = (string)eachUser.Email;
                    dt.Rows.Add(tmpRow);

                }
            }
            catch (RBPCException e)
            {
                log.Debug("RBPCException message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }
            catch (Exception e)
            {
                log.Debug("Exception message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }

            log.Debug("getUsersByDepartmentInAll--dt=" + dt);

            return dt;
        }

        /*
         * Method name: 
         *		getUserAndGroupList
         * 
         * Description: 
         *		取得user group（含单个user的group）显示列表
         * 
         * Parameters: 
         *      string application: portal应用的名称
         *      
         *		string type: 组的类型。（单用户组-1，Group-0）
         * 
         * Return Value: 
         *      DataTable对象(fileds{id，type，name，createTime，UpdateTime，editor，comment})
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc98079
         * 
         * Date:
         *      2011-8-28
         */
        public DataTable getUserAndGroupList()
        {
            RoleInfo[] dtSingleUserAndGroupList = roleMgr.getAllRoles(application);
            //log.Debug("dtSingleUserAndGroupList.Length" + dtSingleUserAndGroupList.Length);

            DataTable dt = new DataTable();

            DataColumn symbol = new DataColumn(Constants.TABLE_COLUMN_GROUP_SYMBOL);
            DataColumn groupName = new DataColumn(Constants.TABLE_COLUMN_GROUP_NAME);
            DataColumn cdt = new DataColumn(Constants.TABLE_COLUMN_GROUP_CDT);
            DataColumn udt = new DataColumn(Constants.TABLE_COLUMN_GROUP_UDT);
            DataColumn author = new DataColumn(Constants.TABLE_COLUMN_GROUP_AUTHOR);
            DataColumn authorId = new DataColumn(Constants.TABLE_COLUMN_GROUP_AUTHOR_ID);
            DataColumn comment = new DataColumn(Constants.TABLE_COLUMN_GROUP_COMMENT);
            DataColumn id = new DataColumn(Constants.TABLE_COLUMN_GROUP_ID);
            DataColumn type = new DataColumn(Constants.TABLE_COLUMN_GROUP_TYPE);
            dt.Columns.Add(symbol);
            dt.Columns.Add(groupName);
            dt.Columns.Add(id);
            dt.Columns.Add(cdt);
            dt.Columns.Add(udt);
            dt.Columns.Add(author);
            dt.Columns.Add(authorId);
            dt.Columns.Add(comment);
            dt.Columns.Add(type);


            if (null != dtSingleUserAndGroupList && dtSingleUserAndGroupList.Length > 0)
            {
                for (int i = 0; i < dtSingleUserAndGroupList.Length; i++)
                {

                    DataRow tmpRow = dt.NewRow();

                    tmpRow[Constants.TABLE_COLUMN_GROUP_SYMBOL] = "";

                    //EDITOR
                    AccountInfo editor = accountMgr.findAccountById(dtSingleUserAndGroupList[i].EditorId);
                    if (editor == null)
                    {
                        tmpRow[Constants.TABLE_COLUMN_GROUP_AUTHOR] = "";
                    }
                    else
                    {
                        tmpRow[Constants.TABLE_COLUMN_GROUP_AUTHOR] = editor.Name;
                    }

                    tmpRow[Constants.TABLE_COLUMN_GROUP_NAME] = dtSingleUserAndGroupList[i].Name;//HttpContext.Current.Server.HtmlEncode(dtSingleUserAndGroupList[i].Name);
                    tmpRow[Constants.TABLE_COLUMN_GROUP_CDT] = dtSingleUserAndGroupList[i].Cdt.ToShortDateString();
                    if (dtSingleUserAndGroupList[i].Udt.Equals(DateTime.MinValue)){
                        tmpRow[Constants.TABLE_COLUMN_GROUP_UDT] = "";
                    }else{
                        tmpRow[Constants.TABLE_COLUMN_GROUP_UDT] = dtSingleUserAndGroupList[i].Udt.ToShortDateString();
                    }
                    tmpRow[Constants.TABLE_COLUMN_GROUP_AUTHOR_ID] = dtSingleUserAndGroupList[i].EditorId.ToString();
                    tmpRow[Constants.TABLE_COLUMN_GROUP_COMMENT] = dtSingleUserAndGroupList[i].Descr;//HttpContext.Current.Server.HtmlEncode(dtSingleUserAndGroupList[i].Descr);
                    tmpRow[Constants.TABLE_COLUMN_GROUP_ID] = dtSingleUserAndGroupList[i].Id.ToString();
                    tmpRow[Constants.TABLE_COLUMN_GROUP_TYPE] = dtSingleUserAndGroupList[i].Type.ToString();

                    dt.Rows.Add(tmpRow);

                }
            }

            return dt;
        }




        /*
         * Method name: 
         *		getUsersListByGroup
         * 
         * Description: 
         *		取得对应group下的user list显示列表
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
         *      2011-08-24
         */
        public DataTable getUsersAndDeptsListByGroup(string strGroupName)
        {
            DataTable dtRtnTable = new DataTable();

            log.Debug("*-*-*-initiate dtRtnTable");
            DataColumn icon = new DataColumn(" ");
            DataColumn userOrDept = new DataColumn("User or Department");
            DataColumn userIDOrDeptPath = new DataColumn("User_ID_or_Dept_Path");
            DataColumn domain = new DataColumn("Domain");
            DataColumn compnay = new DataColumn("Company");
            DataColumn type = new DataColumn("type");

            dtRtnTable.Columns.Add(icon);
            dtRtnTable.Columns.Add(userOrDept);
            dtRtnTable.Columns.Add(userIDOrDeptPath);
            dtRtnTable.Columns.Add(domain);
            dtRtnTable.Columns.Add(compnay);
            dtRtnTable.Columns.Add(type);

            if (strGroupName.Length == 0)
            {
                return dtRtnTable;
            }

            //domain user
            DataTable dtUserIDs = roleMgr.getAcctLoginsByRoleNameAndAccountType(application, strGroupName, Constants.APPLICATION_ALL, Constants.RBPC_ACCOUNT_USER);
            log.Debug("*-*-*-in getUserAndDeptList: dtUserIDs=" + dtUserIDs.Rows.Count);

            for (int i = 0; i < dtUserIDs.Rows.Count; i++)
            {
                log.Debug("entered loop1");
                AccountInfo acct = accountMgr.findAccountByLogin(dtUserIDs.Rows[i][0].ToString(), Constants.APPLICATION_ALL);

                DataRow tmpRow = dtRtnTable.NewRow();

                tmpRow[" "] = "";

                tmpRow["User or Department"] = acct.Name;


                tmpRow["User_ID_or_Dept_Path"] = acct.Id;

                tmpRow["Domain"] = acct.Domain;

                tmpRow["Company"] = acct.Company;

                tmpRow["type"] = Constants.RBPC_ACCOUNT_USER;

                dtRtnTable.Rows.Add(tmpRow);
            }


            //local user
            dtUserIDs = roleMgr.getAcctLoginsByRoleNameAndAccountType(application, strGroupName, application, Constants.RBPC_ACCOUNT_USER);

            for (int i = 0; i < dtUserIDs.Rows.Count; i++)
            {
                log.Debug("entered loop1");
                AccountInfo acct = accountMgr.findAccountByLogin(dtUserIDs.Rows[i][0].ToString(), application);

                DataRow tmpRow = dtRtnTable.NewRow();

                tmpRow[" "] = "";

                tmpRow["User or Department"] = acct.Name;


                tmpRow["User_ID_or_Dept_Path"] = acct.Id;

                tmpRow["Domain"] = acct.Domain;

                tmpRow["Company"] = acct.Company;

                tmpRow["type"] = Constants.DOMAIN_SELECT_ITEM_LOCAL;//Constants.RBPC_ACCOUNT_USER;

                dtRtnTable.Rows.Add(tmpRow);
            }


            
            DataTable dtDepts = roleMgr.getAcctLoginsByRoleNameAndAccountType(application, strGroupName, Constants.APPLICATION_ALL, Constants.RBPC_ACCOUNT_DEPARTMENT);
            log.Debug("dtDepts.Rows.Count=" + dtDepts.Rows.Count);

            for (int i = 0; i < dtDepts.Rows.Count; i++)
            {
                log.Debug("entered loop2");

                DataRow tmpRow = dtRtnTable.NewRow();
                tmpRow[" "] = "";


                tmpRow["User or Department"] = dtDepts.Rows[i]["login"].ToString().Split(Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR.ToCharArray())[2];

                AccountInfo tmpInfo = accountMgr.findAccountByLogin(dtDepts.Rows[i]["login"].ToString(), Constants.APPLICATION_ALL);
                tmpRow["User_ID_or_Dept_Path"] = tmpInfo.Id;//dtDepts.Rows[i]["login"].ToString();


                tmpRow["Domain"] = dtDepts.Rows[i]["login"].ToString().Split(Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR.ToCharArray())[0];


                tmpRow["Company"] = dtDepts.Rows[i]["login"].ToString().Split(Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR.ToCharArray())[1];

                tmpRow["type"] = Constants.RBPC_ACCOUNT_DEPARTMENT;

                dtRtnTable.Rows.Add(tmpRow);
            }


            dtDepts = roleMgr.getAcctLoginsByRoleNameAndAccountType(application, strGroupName, application, Constants.RBPC_ACCOUNT_DEPARTMENT);
            log.Debug("dtDepts.Rows.Count=" + dtDepts.Rows.Count);

            for (int i = 0; i < dtDepts.Rows.Count; i++)
            {
                log.Debug("entered loop2");

                DataRow tmpRow = dtRtnTable.NewRow();
                tmpRow[" "] = "";


                tmpRow["User or Department"] = dtDepts.Rows[i]["login"].ToString().Split(Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR.ToCharArray())[2];

                AccountInfo tmpInfo = accountMgr.findAccountByLogin(dtDepts.Rows[i]["login"].ToString(), application);
                tmpRow["User_ID_or_Dept_Path"] = tmpInfo.Id;//dtDepts.Rows[i]["login"].ToString();


                tmpRow["Domain"] = dtDepts.Rows[i]["login"].ToString().Split(Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR.ToCharArray())[0];


                tmpRow["Company"] = dtDepts.Rows[i]["login"].ToString().Split(Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR.ToCharArray())[1];

                tmpRow["type"] = Constants.RBPC_ACCOUNT_DEPARTMENT;

                dtRtnTable.Rows.Add(tmpRow);
            }

            return dtRtnTable;

        }


        /*
         * 取得所有permission根目录列表
         * Parameters: 
         * 
         * Return Value: 
         *      DataTable列表包含以下field:{
         *          String name,         \\权限名称
         *      }
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc98079
         * Date:
         *      2011-07-09
         */
        
        [AjaxPro.AjaxMethod]
        public DataTable GetAllPermissionList()
        {
            DataTable dtPermission = new DataTable();
            dtPermission.Columns.Add(new DataColumn("Id"));
            dtPermission.Columns.Add(new DataColumn("Name"));
            dtPermission.Columns.Add(new DataColumn("Level"));
            dtPermission.Columns.Add(new DataColumn("ValuePath"));
            dtPermission.Columns.Add(new DataColumn("Sort"));
            dtPermission.Columns.Add(new DataColumn("NavigateUrl"));
            dtPermission.Columns.Add(new DataColumn("ImageUrl"));
            dtPermission.Columns.Add(new DataColumn("Parent"));

            PermissionInfo[] permissins = permissionMgr.findPermissionsByApplication(application);
            for (int i = 0; i < permissins.Length; i++)
            {
                DataRow dr = dtPermission.NewRow();
                dr["Id"] = permissins[i].Id;
                dr["Name"] = permissins[i].Name;
                dr["Level"] = permissins[i].Target.Type;
                dr["ValuePath"] = permissins[i].Type;
                dr["Sort"] = permissins[i].Target.Symbol;
                dr["NavigateUrl"] = permissins[i].Privilege.Privilege;
                dr["ImageUrl"] = permissins[i].Target.DisplayName;
                dr["Parent"] = permissins[i].Privilege.Constraint;

                dtPermission.Rows.Add(dr);
            }

            return dtPermission;
        }

        /*
         * Method name: 
         *		saveAssignUsersToRole
         * 
         * Description: 
         *		创建Accounts。保存(新增)Add Single User的内容。用于selectuser.aspx。
         * 
         * Parameters: 
         *      DataTable dtPar
         *      
         *		string type: 组的类型。（单用户组-1，Group-0）
         * 
         * Return Value: 
         *      bool
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
        public string saveAssignUsersToRole(string strUserIDs) //public bool saveAssignUsersToRole(string strUserIDs)
        {
            string result = "";

            if (null == strUserIDs || strUserIDs.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }
            //strUserIDs = reconnectStrings(strUserIDs, ",", ",");
            //log.Debug("reconnected strUserIDs=" + strUserIDs);
            string[] arrAccountIDs = strUserIDs.Split(Constants.CHECKBOX_ITEM_DELIM.ToCharArray());
            //long[] arrAccountIDs = (long)strUserIDs.Split(Constants.CHECKBOX_ITEM_DELIM.ToCharArray());

//            DataTable dtUserInfos = getUserListInfo(strUserIDs);

//            AccountInfo[] arrAccountInfo = makeAccountInfoListByUserIDs(dtUserInfos);
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < arrAccountIDs.Length; i++)
                    {
                        AccountInfo acctInfo = accountMgr.findAccountById(Int64.Parse(arrAccountIDs[i]));

                        RoleInfo roleInfo = roleMgr.findRoleByName(application, acctInfo.Name);
                        if (roleInfo != null)
                        {
                            ExceptionManager.Throw("There is the same group existing, fail to add.");
                        }

                        RoleInfo newRoleInfo = new RoleInfo();
                        newRoleInfo.Application = application;
                        newRoleInfo.Type = Constants.RBPC_ACCOUNT_TYPE_SINGLE_USER_GROUP;
                        newRoleInfo.Name = acctInfo.Name;
                        newRoleInfo.Cdt = System.DateTime.Now;
                        //newRoleInfo.Udt = System.DateTime.Now;
                        //newRoleInfo.EditorId = Int64.Parse((string)HttpContext.Current.Session[AttributeNames.USER_ID]);

                        //accountMgr.addRole(Int64.Parse(arrAccountIDs[i]), newRoleInfo, Int64.Parse((string)HttpContext.Current.Session[AttributeNames.USER_ID]));
                        RoleInfo tmpRoleInfo=accountMgr.addRole(Int64.Parse(arrAccountIDs[i]), newRoleInfo, editorID);
                        result = tmpRoleInfo.Id.ToString();
                        
                    }

                    scope.Complete();
                    log.Debug("scope.Complete()");
                }
                catch (RBPCException e)
                {
                    log.Debug("RBPCException Message:" + e.Message);
                    scope.Dispose();
                    log.Debug("dispose -- RBPCException in saveAssignUsersToRole");
                    ExceptionManager.Throw(e.Message);
                }
                catch (Exception e)
                {
                    log.Debug(e.Message);
                    scope.Dispose();
                    log.Debug("dispose -- Exception in saveAssignUsersToRole");
                    ExceptionManager.Throw(e.Message);
                }
            }

            return result;

        }


        /*
         * Method name: 
         *		saveAddDomainUsersToSubSystem
         * 
         * Description: 
         *		向组中添加用户
         * 
         * Parameters: 
         *      strUserIDs
         *      
         *      strGroupID
         * 
         * Return Value: 
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc98079
         * 
         * Date:
         *      2011-7-8
         */
        public void saveAddDomainUsersToSubSystem(string strUserIDs)
        {
            if (null == strUserIDs || strUserIDs.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            string[] arrAccountIDs = strUserIDs.Split(Constants.CHECKBOX_ITEM_DELIM.ToCharArray());
            AccountInfo[] arrAccountInfos = new AccountInfo[arrAccountIDs.Length];
            int i = 0;
            

            try
            {
                foreach(string acctId in arrAccountIDs)
                {
                    AccountInfo acctInfo = accountMgr.findAccountById(Int64.Parse(acctId));
                    acctInfo.Application = application;
                    arrAccountInfos[i] = acctInfo;
                    i++;
                }
                accountMgr.createAccounts(arrAccountInfos, true);
            }
            catch (RBPCException e)
            {
                log.Debug("RBPCException message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }
            catch (Exception e)
            {
                log.Debug("Exception message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }
        }


        /*
         * Method name: 
         *		saveAddDepartment
         * 
         * Description: 
         *		向一个Group中保存若干个新增的Department, 用于selectdepartment.aspx
         * 
         * Parameters: 
         *      string[] arrDeptPaths , string strGroupID
         * 
         * Return Value: 
         *      (bool) true
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc98079
         * 
         * Date:
         *      2011-08-24
         */
        public bool saveAddDepartment(string[] arrDeptPaths, string strGroupID, string strDomain)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    AccountInfo[] acctInfos = makeAccountInfoListByDeptPaths(arrDeptPaths, strDomain);
                    long[] acctIDs = createDeptAccountsInRBPC(acctInfos);
                    relateAccountsAndRoleInRBPC(acctIDs, strGroupID);

                    scope.Complete();
                    log.Debug("scope.Complete()");
                }
                catch (RBPCException e)
                {
                    log.Debug("RBPCException Message:" + e.Message);
                    scope.Dispose();
                    log.Debug("dispose -- RBPCException in saveAddDepartment");
                    ExceptionManager.Throw(e.Message);
                }
                catch (Exception e)
                {
                    log.Debug(e.Message);
                    scope.Dispose();
                    log.Debug("dispose -- Exception in saveAddDepartment");
                    ExceptionManager.Throw(e.Message);
                }
            }

            return true;
        }

        /*
         * Method name: 
         *		saveAddUsersToGroup
         * 
         * Description: 
         *		向组中添加用户，用于selectuser.aspx
         * 
         * Parameters: 
         *      strUserIDs
         *      
         *      strGroupID
         * 
         * Return Value: 
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
        public string saveAddUsersToGroup(string strUserIDs, string strGroupName)
        {
            string result = "";
            if (null == strUserIDs || strUserIDs.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            string[] arrAccountIDs = strUserIDs.Split(Constants.CHECKBOX_ITEM_DELIM.ToCharArray());

            //DataTable dtUserInfos = getUserListInfo(strUserIDs);

            try
            {
                //AccountInfo[] arrAccountInfo = makeAccountInfoListByUserIDs(dtUserInfos);
                //long[] savedAccountIDs = saveSingleUser(arrAccountInfo);

                RoleInfo roleInfo = getRoleInfoByNameInRBPC(strGroupName);
                for (int i = 0; i < arrAccountIDs.Length; i++)
                {
                    //accountMgr.addRole(Int64.Parse(arrAccountIDs[i]), roleInfo, Int64.Parse((string)HttpContext.Current.Session[AttributeNames.USER_ID]));
                    RoleInfo newRoleInfo=accountMgr.addRole(Int64.Parse(arrAccountIDs[i]), roleInfo, editorID);
                    result = newRoleInfo.Id.ToString();
                    
                }

            }
            catch (RBPCException e)
            {
                log.Debug("RBPCException message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }
            catch (Exception e)
            {
                log.Debug("Exception message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }
            return result;
        }


        /*
         * Method name: 
         *		deleteUserInGroup
         * 
         * Description: 
         *		删除Group（也包括删除single user）
         * 
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
         *      2011-07
         */
        public bool deleteUserInGroup(string strItemLogin, string strGroupID, string strItemType)
        {

            try
            {
                /*
                AccountInfo acctInfo;
                if (strItemType.Equals(Constants.DOMAIN_SELECT_ITEM_LOCAL))
                {
                    acctInfo = accountMgr.findAccountByLogin(strItemLogin, application);

                }
                else
                {
                    acctInfo = accountMgr.findAccountByLogin(strItemLogin, Constants.APPLICATION_ALL);
                }*/
                //RoleInfo roleInfo = roleMgr.findRoleById(Int64.Parse(strGroupID));
                //accountMgr.removeRole(acctInfo.Id, roleInfo.Id);
                accountMgr.removeRole(Int64.Parse(strItemLogin), Int64.Parse(strGroupID));
            }
            catch (RBPCException e)
            {
                log.Debug(e.Message);
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            return true;
        }



        /*
         * Method name: 
         *		saveUserGroupItem
         * 
         * Description: 
         *		保存(新增\修改)一个人员组
         * 
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
         *      2011-9-1
         */
        public string saveUserGroupItem(string strGroupID, string strGroupName, string strComment)
        {
            log.Debug("saveUserGroupItem(string strGroupID, string strGroupName, string strComment)");
            if (null == strGroupName || strGroupName.Trim().Equals(""))
            {
                log.Debug("strGroupName=" + "\"\"");
                ExceptionManager.Throw(ExceptionMsg.ERROR_PLEASE_ENTER_A_VALID_GROUP_NAME);
            }

            RoleInfo tmpRoleInfo = new RoleInfo();
            tmpRoleInfo.Application = application;
            tmpRoleInfo.Type = Constants.RBPC_ACCOUNT_TYPE_GROUP;
            tmpRoleInfo.Name = strGroupName;
            tmpRoleInfo.Descr = strComment;
            tmpRoleInfo.EditorId = editorID;


            //add
            if (null == strGroupID || strGroupID.Equals(""))
            {
                string newId = "";
                try
                {
                    RoleInfo roleInfo = roleMgr.findRoleByName(application, strGroupName);
                    if (roleInfo != null)
                    {
                        log.Debug("saveUserGroupItem----There is the same group existing, fail to add.");
                        ExceptionManager.Throw("There is the same group existing, fail to add.");
                    }

                    RoleInfo newRoleInfo = roleMgr.createRole(tmpRoleInfo, editorID);
                    newId = newRoleInfo.Id.ToString();
                }
                catch (RBPCException e)
                {
                    log.Debug("RBPCException message=" + e.Message);
                    ExceptionManager.Throw(e.Message);
                }
                catch (Exception e)
                {
                    log.Debug("Exception message=" + e.Message);
                    ExceptionManager.Throw(e.Message);
                }
                return newId;
            }
            else //edit
            {
                try
                {
                    roleMgr.setRole(Int64.Parse(strGroupID), tmpRoleInfo, editorID);
                }
                catch (RBPCException e)
                {
                    log.Debug("RBPCException message=" + e.Message);
                    ExceptionManager.Throw(e.Message);
                }
                return strGroupID;
            }
        }

        /*
         * Method name: 
         *		saveLocalUser
         * 
         * Description: 
         *		保存(新增\修改)一个人员组
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
         * 
         * Return Value: 
         *      (string)成功保存了的Local User的ID
         * 
         * Remark: 
         *      1）检查传入的Login是否已经存在，
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
        public string saveLocalUser(string strLocalUserID, string strLogin, string strName, string strPassword, string strCompany, string strDepartment, string strEMail)
        {
            log.Debug("saveUserGroupItem(string strLogin, string strName, string strPassword, string strCompany, string strDepartment, string strEMail)");
            if (null == strLogin || strLogin.Trim().Equals(""))
            {
                log.Debug("strLogin=" + "\"\"");
                ExceptionManager.Throw(ExceptionMsg.ERROR_SAME_ACCOUNT_EXISTED);
            }

            AccountInfo tmpAccountInfo = new AccountInfo();
            tmpAccountInfo.Application = application;
            tmpAccountInfo.Domain = Constants.TABLE_COLUMN_LOCAL;
            tmpAccountInfo.Type = Constants.TABLE_COLUMN_LOCAL;
            tmpAccountInfo.AccountType = Constants.RBPC_ACCOUNT_USER;
            tmpAccountInfo.Name = strName;
            tmpAccountInfo.Login = strLogin;
            tmpAccountInfo.Password = strPassword;
            tmpAccountInfo.Company = strCompany;
            tmpAccountInfo.Department = strDepartment;
            tmpAccountInfo.Email = strEMail;
            tmpAccountInfo.Cdt = System.DateTime.Now;
            //tmpAccountInfo.EditorId = Int64.Parse((string)HttpContext.Current.Session[AttributeNames.USER_ID]);
            tmpAccountInfo.EditorId = editorID;


            //add
            if (null == strLocalUserID || strLocalUserID.Equals(""))
            {
                log.Debug("*-*-*-add a new user");
                string newId = "";
                try
                {
                    log.Debug("newAccountInfo.Application=" + tmpAccountInfo.Application);
                    log.Debug("tmpAccountInfo.AccountType=" + tmpAccountInfo.AccountType);
                    log.Debug("tmpAccountInfo.Name=" + tmpAccountInfo.Name);
                    log.Debug("tmpAccountInfo.AccountType=" + tmpAccountInfo.AccountType);
                    log.Debug("tmpAccountInfo.Login=" + tmpAccountInfo.Login);
                    log.Debug("Password=" + strPassword);
                    log.Debug("tmpAccountInfo.Company=" + tmpAccountInfo.Company);
                    log.Debug("tmpAccountInfo.Department=" + tmpAccountInfo.Department);
                    log.Debug("tmpAccountInfo.Email=" + tmpAccountInfo.Email);
                    log.Debug("tmpAccountInfo.Cdt=" + tmpAccountInfo.Cdt);
                    //long newAccountId = accountMgr.createAccount(tmpAccountInfo, strPassword, Int64.Parse((string)HttpContext.Current.Session[AttributeNames.USER_ID]));
                    long newAccountId = accountMgr.createAccount(tmpAccountInfo, strPassword, editorID);
                    newId = newAccountId.ToString();
                    log.Debug("*-*-newId=" + newId);
                }
                catch (RBPCException e)
                {
                    log.Debug("RBPCException message=" + e.Message);
                    ExceptionManager.Throw(e.Message);
                }
                catch (Exception e)
                {
                    log.Debug("Exception message=" + e.Message);
                    ExceptionManager.Throw(e.Message);
                }
                return newId;
            }
            else //edit
            {
                try
                {
                    accountMgr.setAccountInfo(Int64.Parse(strLocalUserID), tmpAccountInfo, editorID);
                    if (strPassword.Length != 0) {
                        accountMgr.encryptPassword(Int64.Parse(strLocalUserID),strPassword);
                    }
                }
                catch (RBPCException e)
                {
                    log.Debug("RBPCException message=" + e.Message);
                    ExceptionManager.Throw(e.Message);
                }
                catch (Exception e)
                {
                    log.Debug("Exception message=" + e.Message);
                    ExceptionManager.Throw(e.Message);
                }
                return strLocalUserID;
            }
        }

        /*
        * Description: 从RBPC数据库中根据1个user的id获取local user info
         * Parameters: string strLocalUserID
         * Return Value: 
         *		(DataTable)
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         * 98079
         * 2009-11
        */
        public DataTable getLocalUserByID(string strLocalUserID)
        {
            log.Debug("getLocalUserByID");


            DataTable dt = new DataTable();

            DataColumn login = new DataColumn(Constants.TABLE_COLUMN_USER_LOGIN);
            DataColumn userName = new DataColumn(Constants.TABLE_COLUMN_USER_NAME);
            DataColumn userId = new DataColumn(Constants.TABLE_COLUMN_USER_ID);
            DataColumn email = new DataColumn(Constants.TABLE_COLUMN_USER_EMAIL);
            DataColumn department = new DataColumn(Constants.TABLE_COLUMN_USER_DEPARTMENT);
            DataColumn company = new DataColumn(Constants.TABLE_COLUMN_USER_COMPANY);
            dt.Columns.Add(login);
            dt.Columns.Add(userName);
            dt.Columns.Add(userId);
            dt.Columns.Add(email);
            dt.Columns.Add(department);
            dt.Columns.Add(company);


            AccountInfo acctInfo = new AccountInfo();
            try
            {
                acctInfo = accountMgr.findAccountById(Int64.Parse(strLocalUserID));
            }
            catch (RBPCException e)
            {
                log.Debug("RBPCException message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }

            DataRow tmpRow = dt.NewRow();
            string tmpLogin = (string)acctInfo.Login;
            int n = tmpLogin.IndexOf("\\");
            if (n > 0)
            {
                tmpLogin = tmpLogin.Substring(n + 1, tmpLogin.Length - n - 1).ToLower();
            }

            tmpRow[Constants.TABLE_COLUMN_USER_LOGIN] = tmpLogin;
            tmpRow[Constants.TABLE_COLUMN_USER_NAME] = (string)acctInfo.Name;
            tmpRow[Constants.TABLE_COLUMN_USER_ID] = acctInfo.Id.ToString();
            tmpRow[Constants.TABLE_COLUMN_USER_EMAIL] = (string)acctInfo.Email;
            tmpRow[Constants.TABLE_COLUMN_USER_DEPARTMENT] = (string)acctInfo.Department;
            tmpRow[Constants.TABLE_COLUMN_USER_COMPANY] = (string)acctInfo.Company;
            dt.Rows.Add(tmpRow); 

            return dt;
        }


        /*
        * Description: 根据1个Group的ID获取Group的Name和Comment
         * Parameters: string strGroupID
         * Return Value: 
         *		(DataTable)
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         * itc98079
         * 2009-11
        */
        public DataTable getGroupInfoByID(string strGroupID)
        {
            log.Debug("getGroupInfoByID");
            log.Debug("*-*-strGroupID=" + strGroupID);

            if (null == strGroupID || strGroupID.Equals(""))
            {
                log.Debug("*-*-*-Empty strGroupID !");
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            
            DataTable dtGroupInfo = new DataTable();
            DataColumn id = new DataColumn("id");
            DataColumn name = new DataColumn("name");
            DataColumn comment = new DataColumn("comment");
            dtGroupInfo.Columns.Add(id);
            dtGroupInfo.Columns.Add(name);
            dtGroupInfo.Columns.Add(comment);

            RoleInfo role = roleMgr.findRoleById(Int64.Parse(strGroupID));

            DataRow tmpRow = dtGroupInfo.NewRow();
            tmpRow[0] = role.Id;
            tmpRow[1] = role.Name;
            tmpRow[2] = role.Descr;

            dtGroupInfo.Rows.Add(tmpRow);

            if (null == dtGroupInfo || dtGroupInfo.Rows.Count == 0)
            {
                log.Debug("group with this id does not exist " + strGroupID);
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            log.Debug(dtGroupInfo.Rows[0][0].ToString());
            log.Debug(dtGroupInfo.Rows[0][1].ToString());

            return dtGroupInfo;
        }


        /*
         * Method name: 
         *		deleteUserGroupItem
         * 
         * Description: 
         *		删除Group（也包括删除single user）
         * 
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
         *      2009-11
         */
        public bool deleteUserGroupItem(String _groupName)
        {
            log.Debug("deleteUserGroupItem");
            if (null == _groupName || _groupName.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            try
            {
                RoleInfo roleInfo = getRoleInfoByNameInRBPC(_groupName);
                roleMgr.revokeAllPermissions(roleInfo.Id);
                roleMgr.deleteRole(roleInfo.Id);
            }
            catch (RBPCException e)
            {
                log.Debug(e.Message);
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            return true;
        }


        /*
         * Method name: 
         *		saveAuthority
         * 
         * Description: 
         *		为一个组保存所有权限
         * 
         * Parameters: 
         *      string[] arrAllPermissions: 1 primary permissions
         *												2 report authorities
         *												3 chart authorities
         *      
         *		string strGroupID
         * 
         * Return Value: 
         *      (DataTable) Primary permissions
         * 
         * Remark: 
         *      1）检查传入的Group ID是否存在，
         *      2）做多7项，最少为0项
         * 
         * Example: 
         * 
         * Author: 
         *      itc98079
         * 
         * Date:
         *      2009-11
         */
        public bool saveAuthority(string[] arrPermissionNames, string strGroupID)
        {
            log.Debug("saveAuthority");
            log.Debug("strGroupID=" + strGroupID);
            //log.Debug("arrPermissions=" + arrPermissions);

            if (null == arrPermissionNames || null == strGroupID || strGroupID.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }
            
            long[] arrPermissions = new long[arrPermissionNames.Length];
            for (int i = 0; i < arrPermissionNames.Length; i++) 
            {
                PermissionInfo perminfo = permissionMgr.findPermissionByName(application, arrPermissionNames[i]);
                arrPermissions[i] = perminfo.Id;
            }
            
            try
            {
                log.Debug("set authorities");
                //roleMgr.revokeAllPermissions(Int64.Parse(strGroupID), Int64.Parse((string)HttpContext.Current.Session[AttributeNames.USER_ID]));
                roleMgr.revokeAllPermissions(Int64.Parse(strGroupID), editorID);
                //由于RBPC中没有全新增加（只有增量式增加）Permissions的接口，所以若要全新给一个Role赋权限，要先revoke该role的所有Permissions，再grantPermissions
                if (arrPermissions.Length > 0)
                {
                    //roleMgr.grantPermissions(Int64.Parse(strGroupID), arrPermissions, Int64.Parse((string)HttpContext.Current.Session[AttributeNames.USER_ID]));
                    roleMgr.grantPermissions(Int64.Parse(strGroupID), arrPermissions, editorID);
                }
            }
            catch (RBPCException e)
            {
                log.Debug(e.Message);
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }


            /*
            bool bResult1 = savePrimaryPermissions(arrAllPermissions[0], strGroupID);
            bool bResult2 = saveReportPermissions(arrAllPermissions[1], strGroupID);
            bool bResult3 = saveReportPermissions(arrAllPermissions[2], strGroupID);
            */

            return true;
        }

        /*
         * Method name: 
         *		reformatDataTableToString
         * 
         * Description: 
         *		将所传入的DataTable的每行第一个栏位的值组成类似"'value1','value2','value3','value'"的字符串
         * 
         * Parameters: 
         *      DataTable dtPar
         *      
         *		string type: 组的类型。（单用户组-1，Group-0）
         * 
         * Return Value: 
         *      string rtnString
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc204011
         * 
         * Date:
         *      2008-12
         */
        private string reformatDataTableToString(DataTable dtPar)
        {
            if (dtPar.Rows.Count == 0)
            {
                return "''";
            }

            string rtnString = "'";
            for (int i = 0; i < dtPar.Rows.Count; i++)
            {
                if (i != 0)
                {
                    rtnString += ",'";
                }
                rtnString = rtnString + dtPar.Rows[i][0].ToString() + "'";
            }
            return rtnString;
        }





        /*
         * Method name: 
         *		saveSingleUserInFISGroup
         * 
         * Description: 
         *		save a single user group in FIS Report
         * 
         * Parameters: 
         *      string strGroupName(User name)
         *      
         *		string type: 组的类型。（单用户组-1，Group-0）
         * 
         * Return Value: 
         *      new Group ID
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
        private string saveSingleUserInFISGroup(string strGroupName)
        {
            //string strEditorID = (string)HttpContext.Current.Session[AttributeNames.USER_ID];
            string strEditorID = editorID.ToString();
            log.Debug("Current UserID(group editor ID) = " + strEditorID);
            AuthorityDao authDao = new AuthorityDao();
            string rtnSavedGroupID = authDao.saveSingleUserRoleInFIS(strGroupName, strEditorID);
            if (rtnSavedGroupID.Equals(""))
            {
                ExceptionManager.Throw(strGroupName + ExceptionMsg.ERROR_GROUP_NAMES_HAVE_BEEN_USED);
            }
            return rtnSavedGroupID;
        }


        /*
         * Method name: 
         *		saveNewUserGroupInFISGroup
         * 
         * Description: 
         *		save a new multi user group in FIS Report
         * 
         * Parameters: 
         *      string strGroupName(用户所输入的Group Name。不可与已有Group重名，不可为空)
         *      
         *		string type: 组的类型。（单用户组-1，Group-0）
         * 
         * Return Value: 
         *      new Group ID
         * 
         * Remark: 
         *		deserted
         * Example: 
         * 
         * Author: 
         *      itc204011
         * 
         * Date:
         *      2008-12
         */
        private string saveNewUserGroupInFISGroup(string strGroupName)
        {
            log.Debug("saveNewUserGroupInFISGroup");
            //string strEditorID = (string)HttpContext.Current.Session[AttributeNames.USER_ID];
            string strEditorID = (string)editorID.ToString();
            log.Debug("Current UserID(group editor ID) = " + strEditorID);
            AuthorityDao authDao = new AuthorityDao();
            string rtnSavedGroupID = authDao.saveNewUserGroupInFIS(strGroupName, strEditorID);
            if (rtnSavedGroupID.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_SAME_GROUP_EXISTED + " -- " + strGroupName);
            }
            return rtnSavedGroupID;
        }


        /*
         * Method name: 
         *		saveSingleUser
         * 
         * Description: 
         *		创建Accounts。保存(新增)Add Single User的内容
         * 
         * Parameters: 
         *      List<Account> accountInfoList (界面选择的多个域用户的信息)
         *      
         *		输入参数account对象中包含以下fields{application，login，type，editor，department }
         * 
         * Return Value: 
         *      new Group ID
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
        public long[] saveSingleUser(AccountInfo[] accountInfoList)
        {
            long[] arrAccountIDs = new long[accountInfoList.Length];
            log.Debug("*-*-*-saveSingleUser");
            //application type name login createdate必填
            try
            {
                //using (TransactionScope scope = new TransactionScope())
                //{
                arrAccountIDs = accountMgr.createAccounts(accountInfoList, false);
                log.Debug("*-*-*-accountID[0]=" + arrAccountIDs[0]);

                //}
            }
            catch (RBPCException e)
            {
                log.Debug("RBPCException message:" + e.Message);
                return arrAccountIDs;
                //ExceptionManager.Throw(ExceptionMsg.ERROR_SAME_ACCOUNT_EXISTED);
            }
            catch (Exception e)
            {
                log.Debug("Exception message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }

            return arrAccountIDs;
        }





        /*
         * Method name: 
         *		makeAccountInfoListByUserIDs
         * 
         * Description: 
         *		向组中添加用户
         * 
         * Parameters: 
         *      (AccountInfo[]) acctInfoDepts
         * 
         * Return Value: 
         *      List<Account> accountInfoList (界面选择的多个域用户的信息)
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
        private AccountInfo[] makeAccountInfoListByUserIDs(DataTable dtUserInfos)
        {
            AccountInfo[] arrAccountInfo = new AccountInfo[dtUserInfos.Rows.Count];
            log.Debug("arrAccountInfo.Length=" + arrAccountInfo.Length);
            if (dtUserInfos.Rows.Count == 1)
            {
                arrAccountInfo[0] = new AccountInfo();
                arrAccountInfo[0].Application = Constants.ALL_OPTION;

                arrAccountInfo[0].Type = Constants.TABLE_COLUMN_DOMAIN;//domain(目前全部是domain，没有local，待考虑)

                arrAccountInfo[0].AccountType = Constants.RBPC_ACCOUNT_USER;//另有Constants.RBPC_ACCOUNT_DEPARTMENT

                arrAccountInfo[0].Name = dtUserInfos.Rows[0]["name"].ToString();
                log.Debug("arrAccountInfo[" + 0 + "].Name=" + arrAccountInfo[0].Name);

                arrAccountInfo[0].Login = dtUserInfos.Rows[0]["login"].ToString();
                log.Debug("arrAccountInfo[" + 0 + "].Login=" + arrAccountInfo[0].Login);

                arrAccountInfo[0].Cdt = System.DateTime.Now;
                log.Debug("arrAccountInfo[" + 0 + "].Cdt=" + arrAccountInfo[0].Cdt);

                return arrAccountInfo;
            }

            for (int i = 0; i < dtUserInfos.Rows.Count; i++)
            {
                arrAccountInfo[i] = new AccountInfo();
                log.Debug("entered loop");

                arrAccountInfo[i].Application = Constants.ALL_OPTION;

                arrAccountInfo[0].Type = Constants.TABLE_COLUMN_DOMAIN;//domain(目前全部是domain，没有local，待考虑)

                arrAccountInfo[i].AccountType = Constants.RBPC_ACCOUNT_USER;//另有Constants.RBPC_ACCOUNT_DEPARTMENT

                arrAccountInfo[i].Name = dtUserInfos.Rows[i]["name"].ToString();
                log.Debug("arrAccountInfo[" + i + "].Name=" + arrAccountInfo[i].Name);

                arrAccountInfo[i].Login = dtUserInfos.Rows[i]["login"].ToString();
                log.Debug("arrAccountInfo[" + i + "].Login=" + arrAccountInfo[i].Login);

                arrAccountInfo[i].Cdt = System.DateTime.Now;
                log.Debug("arrAccountInfo[" + i + "].Cdt=" + arrAccountInfo[i].Cdt);
            }
            return arrAccountInfo;
        }





        /*
         * Method name: 
         *		createDeptAccountsInRBPC
         * 
         * Description: 
         *		生成由department path(domain+company+department)作为login、
         *		department name作为name的Accounts
         * 
         * Parameters: 
         *      (AccountInfo[]) acctInfoDepts
         * 
         * Return Value: 
         *      (long[]) savedAcctIDs
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
        private long[] createDeptAccountsInRBPC(AccountInfo[] acctInfoDepts)
        {
            long[] savedAcctIDs = new long[acctInfoDepts.Length];
            try
            {
                savedAcctIDs = accountMgr.createAccounts(acctInfoDepts, false);
            }
            catch (RBPCException e)
            {
                log.Debug("RBPCException Message:" + e.Message);
            }
            return savedAcctIDs;
        }


        /*
         * Method name: 
         *		relateAccountsAndRoleInRBPC
         *
         * Description: 
         *		生成由department path(domain+company+department)作为login、
         *		department name作为name的Accounts
         * 
         * Parameters: 
         *      long[] acctIDs , string strGroupID
         * 
         * Return Value: 
         *      (void)
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
        public void relateAccountsAndRoleInRBPC(long[] acctIDs, string strGroupID)
        {
            for (int i = 0; i < acctIDs.Length; i++)
            {
                try
                {
                    RoleInfo roleInfo = roleMgr.findRoleById(Int64.Parse(strGroupID));
                    accountMgr.addRole(acctIDs[i], roleInfo);
                }
                catch (RBPCException e)
                {
                    log.Debug("RBPCException Message:" + e.Message);
                }
            }
        }


        /*
         * Method name: 
         *		makeAccountInfoListByDeptPaths
         * 
         * Description: 
         *		根据由domain+company+department组成的department path，
         *		生成一个由department path作为login、department name作为name的AccountInfo[]
         * 
         * Parameters: 
         *      string[] arrDeptPaths
         * 
         * Return Value: 
         *      (AccountInfo[]) arrAccountInfo
         * 
         * Remark: 
         * 
         * Example: 
         * 
         * Author: 
         *      itc98079
         * 
         * Date:
         *      2011-9-8
         */
        private AccountInfo[] makeAccountInfoListByDeptPaths(string[] arrDeptPaths, string strDomain)
        {
            if (null == arrDeptPaths || 0 == arrDeptPaths.Length)
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_SELECT_DEPARTMENT);
            }

            AccountInfo[] arrAccountInfo = new AccountInfo[arrDeptPaths.Length];
            log.Debug("arrAccountInfo.Length=" + arrAccountInfo.Length);

            for (int i = 0; i < arrDeptPaths.Length; i++)
            {
                arrAccountInfo[i] = new AccountInfo();
                log.Debug("*-*-*-arrDeptPaths[" + i + "]=" + arrDeptPaths[i]);


                if (strDomain.Equals(Constants.DOMAIN_SELECT_ITEM_LOCAL))
                {
                    arrAccountInfo[i].Application = application;
                    arrAccountInfo[i].Type = Constants.DOMAIN_SELECT_ITEM_LOCAL;//local
                }
                else 
                {
                    arrAccountInfo[i].Application = Constants.ALL_OPTION;
                    arrAccountInfo[i].Type = Constants.TABLE_COLUMN_DOMAIN;//domain
                }

                arrAccountInfo[i].AccountType = Constants.RBPC_ACCOUNT_DEPARTMENT;//另有Constants.RBPC_ACCOUNT_USER

                arrAccountInfo[i].Name = arrDeptPaths[i].Split(Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR.ToCharArray())[2];
                log.Debug("arrAccountInfo[" + i + "].Name=" + arrAccountInfo[i].Name);

                arrAccountInfo[i].Login = arrDeptPaths[i];
                log.Debug("arrAccountInfo[" + i + "].Login=" + arrAccountInfo[i].Login);

                arrAccountInfo[i].Cdt = System.DateTime.Now;
                log.Debug("arrAccountInfo[" + i + "].Cdt=" + arrAccountInfo[i].Cdt);
            }

            return arrAccountInfo;
        }







        /*
         * Description: 在RBPC4Net数据库中初始化7个基本权限：
         *		Constants.RBPC_PERMISSION_SYS_ACCOUNT_AUTHORITY
                Constants.RBPC_PERMISSION_SYS_DATA_SOURCE_SETTING
                Constants.RBPC_PERMISSION_SYS_PUBLISH
                Constants.RBPC_PERMISSION_REPORT_CREATE_EDIT
                Constants.RBPC_PERMISSION_REPORT_DELETE
                Constants.RBPC_PERMISSION_CHART_CREATE_EDIT
                Constants.RBPC_PERMISSION_CHART_DELETE
         * Parameters: 用户的工号
         * Return Value: (DataTable) dtUserOrDeptList
         */
        private DataTable initPrimaryPermissions()
        {
            AuthorityDao authDao = new AuthorityDao();
            return new DataTable();
        }


        /*
        * Description: 从RBPC4Net数据库中根据1个role的name获取RoleInfo
         * Parameters: string strRoleName
         * Return Value: 
         *		(RoleInfo)
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         * 98079
         * 2009-11
        */
        public RoleInfo getRoleInfoByNameInRBPC(string strRoleName)
        {
            RoleInfo rtnRoleInfo = new RoleInfo();
            try
            {
                rtnRoleInfo = roleMgr.findRoleByName(application, strRoleName);
            }
            catch (RBPCException e)
            {
                log.Debug("RBPCException message=" + e.Message);
                ExceptionManager.Throw(e.Message);
            }

            log.Debug(rtnRoleInfo.Id);
            log.Debug(rtnRoleInfo.Name);
            return rtnRoleInfo;
        }

    }
}