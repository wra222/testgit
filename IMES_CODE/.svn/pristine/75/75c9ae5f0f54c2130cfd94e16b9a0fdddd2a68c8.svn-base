/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  Treeview control
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-11-29 Li, Ming-Jun(eB1)     Create 
 * 2010-01-13 Li, Ming-Jun(eB1)     Modify: ITC-1103-0046 
 * 2010-02-04 Li, Ming-Jun(eB1)     Modify: ITC-1103-0168 
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using com.inventec.RBPC.Net.datamodel.intf;
using com.inventec.RBPC.Net.entity;
using com.inventec.RBPC.Net.entity.Session;
using com.inventec.RBPC.Net.intf;
using com.inventec.system.exception;
using com.inventec.system;
using log4net;

namespace com.inventec.iMESWEB
{
    /// <summary>
    /// Summary description for AuthorityManager
    /// </summary>
    public class AuthorityManager
    {
        private IPermissionManager permissionManager = null;
        private ISecurityManager securityManager = null;
        private IAccountManager accountManager = null;
        private IRoleManager roleManager = null;
        private string appName = "";
        private UserInfo userInfo = new UserInfo();
        private string appNameAll = Constants.APPLICATION_ALL;
        private bool isDomainUser = false;
        private ILog logger = LogManager.GetLogger("fisLog");

        public AuthorityManager()
        {
            permissionManager = RBPCAgent.getRBPCManager<IPermissionManager>();
            securityManager = RBPCAgent.getRBPCManager<ISecurityManager>();
            roleManager = RBPCAgent.getRBPCManager<IRoleManager>();
            accountManager = RBPCAgent.getRBPCManager<IAccountManager>();
            appName = ConfigurationManager.AppSettings["RBPCApplication"].ToString();
        }

        public AuthorityManager(UserInfo ui)
            : this()
        {
            userInfo = ui;

            if (userInfo.Login.Contains("\\"))
            {
                isDomainUser = true;
            }
        }

        //Get all parent node
        public DataTable getAllPermission()
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
            dtPermission.Columns.Add(new DataColumn("Pcode"));

            PermissionInfo[] permissins = permissionManager.findPermissionsByApplication(appName);
            for (int i = 0; i < permissins.Length; i++)
            {
                if (permissins[i].Privilege.Constraint == "Parent")
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
                    dr["Pcode"] = permissins[i].Descr;

                    dtPermission.Rows.Add(dr);
                }
            }

            return dtPermission;
        }

        //Get all children node by token
        public DataTable getAuthoritiesByToken(IToken token)
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
            dtPermission.Columns.Add(new DataColumn("Pcode"));

            //PermissionInfo[] permissions = securityManager.getAuthorities(token, appName);
            //if (permissions != null)
            //{
            //    for (int i = 0; i < permissions.Length; i++)
            //    {
            //        DataRow dr = dtPermission.NewRow();
            //        dr["Id"] = permissions[i].Id;
            //        dr["Name"] = permissions[i].Name;
            //        dr["Level"] = permissions[i].Target.Type;
            //        dr["ValuePath"] = permissions[i].Type;
            //        dr["Sort"] = permissions[i].Target.Symbol;
            //        dr["NavigateUrl"] = permissions[i].Privilege.Privilege;
            //        dr["ImageUrl"] = permissions[i].Target.DisplayName;
            //        dr["Parent"] = permissions[i].Privilege.Constraint;
            //        dr["Pcode"] = permissions[i].Descr;

            //        dtPermission.Rows.Add(dr);
            //    }
            //}

            List<string> permissionNames = getPrimaryPermissionsByUserID(userInfo.Login);

            foreach (string permissionName in permissionNames)
            {
                PermissionInfo permission = permissionManager.findPermissionByName(appName, permissionName);

                DataRow dr = dtPermission.NewRow();
                dr["Id"] = permission.Id;
                dr["Name"] = permission.Name;
                dr["Level"] = permission.Target.Type;
                dr["ValuePath"] = permission.Type;
                dr["Sort"] = permission.Target.Symbol;
                dr["NavigateUrl"] = permission.Privilege.Privilege;
                dr["ImageUrl"] = permission.Target.DisplayName;
                dr["Parent"] = permission.Privilege.Constraint;
                dr["Pcode"] = permission.Descr;

                dtPermission.Rows.Add(dr);
            }

            return dtPermission;
        }

        //Get user information by token
        public AccountInfo getAccountByToken(IToken token)
        {
            return securityManager.getAccountByToken(token);
        }
        //Modify: ITC-1281-0055,ITC-1281-0048
        public List<string> getPrimaryPermissionsByUserID(string strUserID)
        {
            if (null == strUserID || strUserID.Trim().Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }


            List<string> listGroupIDs = getGroupIDsByUserID(strUserID);
            List<string> listPermissionNames = new List<string>();
            if (listGroupIDs.Count == 0)
            {
                return listPermissionNames;
            }

            foreach (string tmpGroupID in listGroupIDs)
            {
                DataTable dtPrimaryPermissionNames = getPrimaryPermissionsByGroupName(tmpGroupID);
                for (int i = 0; i < dtPrimaryPermissionNames.Rows.Count; i++)
                {
                    if (!listPermissionNames.Contains(dtPrimaryPermissionNames.Rows[i]["name"].ToString()))
                    //防止放入重复的Primary permission name
                    {
                        listPermissionNames.Add(dtPrimaryPermissionNames.Rows[i]["name"].ToString());
                    }
                }
            }

            return listPermissionNames;
        }


        public List<string> getGroupIDsByUserID(string strUserID)
        {
            if (null == strUserID || strUserID.Trim().Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            AccountInfo acctInfo;
            if (isDomainUser) //if domain user, value of application for RBPC is all.
            {
                acctInfo = accountManager.findAccountByLogin(strUserID, appNameAll);
            }
            else
            {
                acctInfo = accountManager.findAccountByLogin(strUserID, appName);
            }

            string strUserDeptLogin = acctInfo.Domain.ToString()
                + Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR
                + acctInfo.Company.ToString()
                + Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR
                + acctInfo.Department.ToString();

            List<string> listGroupIDs = new List<string>();

            try
            {
                RoleInfo[] userRoleInfos;
                if (isDomainUser) //if domain user, value of application for RBPC is all.
                {
                    userRoleInfos = accountManager.getRolesByApp(appNameAll, strUserID, appName);// getRoles(Constants.APPLICATION_ALL, strUserID);
                }
                else
                {
                    userRoleInfos = accountManager.getRolesByApp(appName, strUserID, appName);// getRoles(Constants.APPLICATION_ALL, strUserID);
                }

                //用户所属的Role
                if (null != userRoleInfos)
                {

                    foreach (RoleInfo tmpRoleInfo in userRoleInfos)
                    {
                        listGroupIDs.Add(tmpRoleInfo.Name);
                    }
                }
            }
            catch (RBPCException e)
            {
                ExceptionManager.Throw(e.Message);
            }
            catch (Exception e)
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            try
            {
                RoleInfo[] userDeptRoleInfos;
                if (isDomainUser) //if domain user, value of application for RBPC is all.
                {
                    userDeptRoleInfos = accountManager.getRolesByApp(appNameAll, strUserDeptLogin, appName);
                }
                else
                {
                    userDeptRoleInfos = accountManager.getRolesByApp(appName, strUserDeptLogin, appName);
                }
                //用户所在部门所属的Role
                if (null != userDeptRoleInfos)
                {

                    foreach (RoleInfo tmpRoleInfo in userDeptRoleInfos)
                    {
                        if (!listGroupIDs.Contains(tmpRoleInfo.Name))//防止放入重复的Group ID
                        {
                            listGroupIDs.Add(tmpRoleInfo.Name);
                        }
                    }
                }
            }
            catch (RBPCException e)
            {
            }
            catch (Exception e)
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            return listGroupIDs;
        }

        public DataTable getPrimaryPermissionsByGroupName(string strGroupName)
        {
            if (null == strGroupName || strGroupName.Trim().Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            DataTable dtPrimaryPermissions = roleManager.getPermNamesByRoleNameAndPermType(appName, strGroupName, appName);

            return dtPrimaryPermissions;
        }

    }

    public class UserInfo
    {
        private String l_userId;
        public String UserId
        {
            get
            {
                return l_userId;
            }
            set
            {
                l_userId = value;
            }
        }

        private String l_userName;
        public String UserName
        {
            get
            {
                return l_userName;
            }
            set
            {
                l_userName = value;
            }
        }

        private String l_department;
        public String Department
        {
            get
            {
                return l_department;
            }
            set
            {
                l_department = value;
            }
        }

        private String l_domain;
        public String Domain
        {
            get
            {
                return l_domain;
            }
            set
            {
                l_domain = value;
            }
        }

        private String l_company;
        public String Company
        {
            get
            {
                return l_company;
            }
            set
            {
                l_company = value;
            }
        }

        private String l_customer;
        public String Customer
        {
            get
            {
                return l_customer;
            }
            set
            {
                l_customer = value;
            }
        }

        private long l_accountId;
        public long AccountId
        {
            get
            {
                return l_accountId;
            }
            set
            {
                l_accountId = value;
            }
        }

        private String l_login;
        public String Login
        {
            get
            {
                return l_login;
            }
            set
            {
                l_login = value;
            }
        }
    }
}
