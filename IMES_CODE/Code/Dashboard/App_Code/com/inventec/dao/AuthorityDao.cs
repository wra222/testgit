/*

 * INVENTEC corporation (c)2008 all rights reserved. 

 * Description: Deal with all the authority operations

 * Update: 

 * Date								Name							Reason 

 * ========== ================= =====================================

 * 2008-11-21                  itc204011                        Create 

 * Known issues:
 * bug:ITC-934-0101 read rbpc database throught rbpc interfaces

 */
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.system.dao;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using com.inventec.system.exception;
using Microsoft.Practices.EnterpriseLibrary.Data;
using log4net;
using com.inventec.system;
using com.inventec.system.util;

/// <summary>
///AuthorityDao 的摘要说明
/// </summary>
namespace com.inventec.imes.dao
{
    public class AuthorityDao : AbstractBaseDao
    {
        public AuthorityDao()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(AuthorityDao));
        private string strRBPCDataBaseServer = "";
        private string strRBPCDataBaseName = "";
        private string strRBPCDataBaseUser = "";
        private string strRBPCDataBasePwd = "";

        public DataTable getUsersInfo(string strUserIDs)
        {
            //listUserIDs.ToString();
            //log.Debug(listUserIDs.ToString());

            string sqlCMD = "SELECT * " +
                "FROM [T_User] " +
                "WHERE [T_User].[id] IN (" + strUserIDs + ") AND [T_User].[lifeCycleStatus]=1";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Method name: 
         *		getUsersInfoForUserAndDeptList
         * 
         * Description: 
         *		根据1个或多个User ID从FIS Report数据库中取得所有符合条件的User的信息
         * 
         * Parameters: 
         *      (string) User IDs
         * 
         * Return Value: 
         *      (DataTable)
         *      {
         *			UserID，
         *			UserName,
         *			Domain,
         *			Company
         *      }
         * 
         * Remark: 
         *      N/A
         * 
         * Example: 
         * 
         * Author: 
         *      itc204011
         * 
         * Date:
         *      2008-12
         */
        public DataTable getUsersInfoForUserAndDeptList(string strUserIDs)
        {
            log.Debug("strUserIDs=" + strUserIDs);
            string sqlCMD = "SELECT " +
                "[lifeCycleStatus] as [#], [userName] as [User_or_Dept.], [id] as [User_ID_or_Dept_Path], [domain] as [Domain], [company] as [Company] " +
                "FROM [T_User] " +
                "WHERE [T_User].[id] IN (" + strUserIDs + ") AND [T_User].[lifeCycleStatus]=1";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: Get all the domains
         * Parameters: none
         * Return Value: 
         *		(DataTable)All the domains
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public DataTable getAllDomains()
        {
            string sqlCMD = "SELECT distinct domain FROM T_User order by domain asc";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: get Companies By Domain
         * Parameters: strDomain
         * Return Value: 
         *		(DataTable)Companies
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public DataTable getCompaniesByDomain(string strDomain)
        {
            string sqlCMD = "SELECT distinct company FROM T_User WHERE domain='" + strDomain + "' order by company asc";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: get Depts By Company
         * Parameters: strCompany
         * Return Value: 
         *		(DataTable)Depts
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public DataTable getDeptsByCompany(string strDomain, string strCompany)
        {
            string sqlCMD = "SELECT distinct department "
                + "FROM T_User "
                + "WHERE domain='" + strDomain + "' "
                + "AND company='" + strCompany + "' order by department asc";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: get Depts By Company For Select Dept page
         * Parameters: strCompany
         * Return Value: 
         *		(DataTable)Depts (3 columns)
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public DataTable getDeptsByCompanyForSelectDept(string strDomain, string strCompany)
        {
            log.Debug("strDomain=" + strDomain);
            log.Debug("strCompany=" + strCompany);

            if (null == strDomain || strDomain.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            if (null == strCompany)
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            string sqlCMD = "SELECT " +
                "distinct " +
                "department as aDept, " +
                "department as Department, " +
                "domain+';'+company+';'+department as [dept_path] " +
                "FROM T_User " +
                "WHERE domain='" + strDomain + "' " +
                "AND company='" + strCompany + "' " +
                "AND department<>'' order by Department asc";

            log.Debug(sqlCMD);

            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: Get users who belong to no company By Domain
         * Parameters: none
         * Return Value: 
         *		(DataTable)All the users who belong to no company(3columns)
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public DataTable getDeptsBelongToNoCompanyByDomain(string strDomain)
        {
            string sqlCMD = "SELECT " +
                "distinct " +
                "department as aDept, " +
                "department as Department, " +
                "domain+';'+company+';'+department as [dept_path]  " +
                "FROM T_User WHERE company='' and domain='" + strDomain + "' and department<>'' order by Department asc";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
             * Method name: 
             *		searchDeptForSelectDept
             * 
             * Description: 
             *		根据一个用户的ID获得用户所属的各个Group的ID
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
             *      itc204011
             * 
             * Date:
             *      2009-1
             */
        public DataTable searchDeptForSelectDept(string strDomain, string strCompany, string strKeyWord)
        {
            strKeyWord = StringUtil.Null2String(strKeyWord);
            strKeyWord = StringUtil.convertCharForSql(strKeyWord);
            string sqlCMD = "SELECT DISTINCT ";

            //if (strKeyWord.Equals(""))
            //{
            //sqlCMD += "TOP 3 ";//???
            //}

            sqlCMD = sqlCMD + "[department] as [aDept], "
                + "[department] as [Department], "
                + "[domain]+'"
                + Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR
                + "'+[company]+'"
                + Constants.RBPC_ACCOUNT_DEPARTMENT_LOGIN_SEPARATOR
                + "'+[department] as [dept_path] "
                + "FROM T_User "
                + "WHERE [domain]='" + strDomain + "' AND [company]='" + strCompany + "' "
                + "AND [department] COLLATE Chinese_PRC_CI_AS like '%" + strKeyWord + "%' order by [Department] asc";
            //COLLATE Chinese_PRC_CI_AS 忽略大小写

            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }



        /*
         * Method name: 
         *		searchDeptForSelectDept
         * 
         * Description: 
         *		根据一个用户的ID获得用户所属的各个Group的ID
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
         *      itc204011
         * 
         * Date:
         *      2009-1
         */
        public DataTable searchUserForSelectUser(string strDomain, string strCompany, string strDept, string strKeyWord)
        {
            strKeyWord = StringUtil.Null2String(strKeyWord);
            strKeyWord = StringUtil.convertCharForSql(strKeyWord);
            string sqlCMD = "SELECT DISTINCT ";

            //if (strKeyWord.Equals(""))
            //{
            //    sqlCMD += "TOP 3 ";
            //}

            sqlCMD = sqlCMD + "[code], [userName] as [User_Name], [id], "
                + "[department] as [Department] "
                + "FROM T_User "
                + "WHERE [domain]='" + strDomain
                + "' AND [company]='" + strCompany;

            if (!strDept.Equals(Constants.ALL_OPTION))
            {
                sqlCMD = sqlCMD + "' AND [department]='" + strDept;
            }

            sqlCMD = sqlCMD + "' AND [userName] COLLATE Chinese_PRC_CI_AS like '%" + strKeyWord + "%' order by [userName] asc";
            //COLLATE Chinese_PRC_CI_AS 忽略大小写

            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: Get users who belong to no company By Domain
         * Parameters: none
         * Return Value: 
         *		(DataTable)All the users who belong to no company
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public DataTable getUsersBelongToNoCompanyByDomain(string strDomain)
        {
            string sqlCMD = "SELECT code, userName as [User_Name], id, department as [Department] FROM T_User WHERE company='' and domain='" + strDomain + "' and lifeCycleStatus='1' order by userName asc";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: Get users who belong to no company
         * Parameters: none
         * Return Value: 
         *		(DataTable)All the users who belong to no company
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public DataTable getUsersByCompany(string strDomain, string strCompany)
        {
            log.Debug("strDomain=" + strDomain);
            log.Debug("strCompany=" + strCompany);

            if (null == strDomain || strDomain.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            if (null == strCompany)
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            string sqlCMD = "SELECT code, userName as [User_Name], id, department as [Department] "
                + "FROM T_User WHERE domain='" + strDomain + "' "
                + "AND company='" + strCompany + "' "
                + "AND lifeCycleStatus='1' order by userName asc";

            log.Debug(sqlCMD);

            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: Get users who belong to no company
         * Parameters: none
         * Return Value: 
         *		(DataTable)All the users who belong to no company
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public DataTable getAllUserGroups()
        {
            string sqlCMD = "SELECT T_Group.name as [#], T_Group.name as [User_Group], T_Group.id as [groupID]," +
            " T_Group.createTime as [Create_Time], T_Group.updateTime as [Update_Time], T_User.userName as [Author], " +
            "T_Group.editorId as [authorID], T_Group.comment as [Comment] ,T_Group.type as [type] " +
            "FROM T_User INNER JOIN T_Group " +
            "on T_User.id = T_Group.editorId and T_Group.lifeCycleStatus='1' order by T_Group.type, [User_Group] asc";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: Get users who belong to no company
         * Parameters: none
         * Return Value: 
         *		(DataTable)All the users who belong to no company
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public DataTable getUsersByDepartment(string strDomain, string strCompany, string strDepartment)
        {
            log.Debug("strDomain=" + strDomain);
            log.Debug("strCompany=" + strCompany);
            log.Debug("strDepartment=" + strDepartment);
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

            string sqlCMD = "SELECT code, userName as [User_Name], id, department as [Department] "
                + "FROM T_User "
                + "WHERE domain='" + strDomain + "' "
                + "AND company='" + strCompany + "' ";

            if (strDepartment.Equals(Constants.ALL_OPTION))
            {
                log.Debug("Department: " + Constants.ALL_OPTION);
            }
            else
            {
                sqlCMD = sqlCMD + "AND department='" + strDepartment + "' ";
            }

            sqlCMD = sqlCMD + "AND lifeCycleStatus='1' order by userName asc";

            log.Debug(sqlCMD);

            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: save a single user role in FIS
         * Parameters: string strName(工号), string strEditorID
         * Return Value: 
         *		(void)
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public string saveSingleUserRoleInFIS(string strName, string strEditorID)
        {
            if (!getRecordSum("T_Group", "[name]='" + strName + "'").Equals("0"))
            {
                log.Debug("A same record has existed~~.");
                return "";
            }
            string strUUID = getUUID();
            string sqlCMD =
                "INSERT INTO T_Group(id, type, name, editorId, comment, createTime, updateTime, lifeCycleStatus) "
                + "VALUES('" + strUUID + "', " +
                "'" + Constants.RBPC_ACCOUNT_TYPE_SINGLE_USER_GROUP + "', " +
                "'" + strName + "', " +
                "'" + strEditorID + "', " +
                "'', " +
                "getdate(), " +
                "getdate(), " +
                "1)";
            log.Debug(sqlCMD);
            ExecuteDataSet(sqlCMD);
            return strUUID;
        }


        /*
         * Description: save a multi user role in FIS
         * Parameters: string strName(工号), string strEditorID
         * Return Value: 
         *		(void)
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public string saveNewUserGroupInFIS(string strName, string strEditorID)
        {
            if (!getRecordSum("T_Group", "[name]='" + strName + "'").Equals("0"))
            {
                log.Debug("A same record has existed~~.");
                return "";
            }
            string strUUID = getUUID();
            string sqlCMD =
                "INSERT INTO T_Group(id, type, name, editorId, comment, createTime, updateTime, lifeCycleStatus) "
                + "VALUES('" + strUUID + "', " +
                "'" + Constants.RBPC_ACCOUNT_TYPE_GROUP + "', " +
                "'" + strName + "', " +
                "'" + strEditorID + "', " +
                "'', " +
                "getdate(), " +
                "getdate(), " +
                "1)";
            log.Debug(sqlCMD);
            ExecuteDataSet(sqlCMD);
            return strUUID;
        }


        /*
         * Description: Get users who belong to no company
         * Parameters: none
         * Return Value: 
         *		(DataTable)All the users who belong to no company
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public string getRecordSum(string strTableName, string strCondition)
        {
            string sqlCMD =
                "SELECT COUNT(*) as [" + Constants.RECORD_SUM + "] " +
                "FROM " + strTableName + " WHERE " + strCondition;
            log.Debug(sqlCMD);

            DataTable dtSum = ExecuteDataSet(sqlCMD).Tables[0];
            log.Debug("*-*-*-sum is " + dtSum.Rows[0][0].ToString());

            return dtSum.Rows[0][0].ToString();
        }


        /*
         * Description: Get account logins of which the type is Constants.RBPC_ACCOUNT_USER
         * Parameters: none
         * Return Value: 
         *		(DataTable)
         * Remark: 
         *      XXX
         * Example: N/A
         * Output : N/A
         */
        public DataTable getSingleUserAccountLogin(string roleName)
        {
            log.Debug("strRBPCDataBaseServer=" + strRBPCDataBaseServer);
            log.Debug("strRBPCDataBaseName=" + strRBPCDataBaseName);
            log.Debug("strRBPCDataBaseUser=" + strRBPCDataBaseUser);
            log.Debug("strRBPCDataBasePwd=" + strRBPCDataBasePwd);

            Database dbRBPC4Net = getDbObject(strRBPCDataBaseServer, strRBPCDataBaseName, strRBPCDataBaseUser, strRBPCDataBasePwd);
            string sqlCMD = "SELECT [login] "
                + "FROM [Account] "
                + "inner join [AccountRole] "
                + "on ([acct_id]=[Account].[id]) "
                + "inner join [Role] "
                + "on (role_id=Role.id) "
                + "where [Role].[name]=N'" + roleName + "' "
                + "and [Account].[type]='" + Constants.RBPC_ACCOUNT_USER + "'";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD, dbRBPC4Net).Tables[0];
        }


        /*
         * Description: get a user's code by his ID
         * Parameters: string strUserCode
         * Return Value: 
         *		(DataTable)
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
         */
        public DataTable getCodeByUserID(string strUserID)
        {
            string sqlCMD = "SELECT [code] "
                + "FROM T_User "
                + "where [id]='" + strUserID + "' ";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Description: Get account logins of which the type is Constants.RBPC_ACCOUNT_USER
         * Parameters: none
         * Return Value: 
         *		(DataTable)
         * Remark: 
         *      XXX
         * Example: N/A
         * Output : N/A
         */
        public DataTable getRoleByUserID(string strUserID)
        {
            log.Debug("strRBPCDataBaseServer=" + strRBPCDataBaseServer);
            log.Debug("strRBPCDataBaseName=" + strRBPCDataBaseName);
            log.Debug("strRBPCDataBaseUser=" + strRBPCDataBaseUser);
            log.Debug("strRBPCDataBasePwd=" + strRBPCDataBasePwd);

            Database dbRBPC4Net = getDbObject(strRBPCDataBaseServer, strRBPCDataBaseName, strRBPCDataBaseUser, strRBPCDataBasePwd);
            string sqlCMD = "SELECT [Role].[name] " +
                "FROM [Account] " +
                "INNER JOIN [AccountRole] " +
                "ON ([acct_id]=[Account].[id]) " +
                "INNER JOIN [Role] " +
                "ON ([AccountRole].[role_id]=[Role].[id]) " +
                "WHERE [Account].[login]='" + strUserID + "'";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD, dbRBPC4Net).Tables[0];
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
        */
        public DataTable getGroupInfoByID(string strGroupID)
        {
            string sqlCMD = "SELECT [id],[name],[comment] " +
                "FROM [T_Group] " +
                "WHERE [lifeCycleStatus]='1' and [id]='" + strGroupID + "'";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        }


        /*
         * Method name: 
         *		getSingleUserAccountLoginsByGroupIDFromRBPC
         * 
         * Description: 
         *		根据Group ID从RBPC4Net数据库中取得该Group中所有Single User（相对于Department而言）的ID
         * 
         * Parameters: 
         *      (string) Group ID
         * 
         * Return Value: 
         *      (DataTable) : {string: UserID}
         * 
         * Remark: 
         *      N/A
         * 
         * Example: 
         * 
         * Author: 
         *      itc204011
         * 
         * Date:
         *      2008-12
         */
        public DataTable getSingleUserAccountLoginsByGroupIDFromRBPC(string strGroupID)
        {
            log.Debug("strRBPCDataBaseServer=" + strRBPCDataBaseServer);
            log.Debug("strRBPCDataBaseName=" + strRBPCDataBaseName);
            log.Debug("strRBPCDataBaseUser=" + strRBPCDataBaseUser);
            log.Debug("strRBPCDataBasePwd=" + strRBPCDataBasePwd);

            Database dbRBPC4Net = getDbObject(strRBPCDataBaseServer, strRBPCDataBaseName, strRBPCDataBaseUser, strRBPCDataBasePwd);
            string sqlCMD = "SELECT [Account].[login] " +
                "FROM [Account] " +
                "INNER JOIN [AccountRole] " +
                "ON ([acct_id]=[Account].[id]) " +
                "INNER JOIN [Role] " +
                "ON ([AccountRole].[role_id]=[Role].[id]) " +
                "WHERE [Role].[name]='" + strGroupID + "' " +
                "AND [Account].[type]='" + Constants.RBPC_ACCOUNT_USER + "'";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD, dbRBPC4Net).Tables[0];
        }


        /*
         * Method name: 
         *		getDepartmentAccountLoginsByGroupIDFromRBPC
         * 
         * Description: 
         *		根据Group ID从RBPC4Net数据库中取得该Group中
         *		所有Department（相对于single user而言）的唯一字符串（domain name + company name + department name）
         * 
         * Parameters: 
         *      (string) Group ID
         * 
         * Return Value: 
         *      (DataTable) : {string: UserID}
         * 
         * Remark: 
         *      N/A
         * 
         * Example: 
         * 
         * Author: 
         *      itc204011
         * 
         * Date:
         *      2008-12
         */
        public DataTable getDepartmentAccountLoginsByGroupIDFromRBPC(string strGroupID)
        {
            log.Debug("strRBPCDataBaseServer=" + strRBPCDataBaseServer);
            log.Debug("strRBPCDataBaseName=" + strRBPCDataBaseName);
            log.Debug("strRBPCDataBaseUser=" + strRBPCDataBaseUser);
            log.Debug("strRBPCDataBasePwd=" + strRBPCDataBasePwd);

            Database dbRBPC4Net = getDbObject(strRBPCDataBaseServer, strRBPCDataBaseName, strRBPCDataBaseUser, strRBPCDataBasePwd);
            string sqlCMD = "SELECT [Account].[login] " +
                "FROM [Account] " +
                "INNER JOIN [AccountRole] " +
                "ON ([acct_id]=[Account].[id]) " +
                "INNER JOIN [Role] " +
                "ON ([AccountRole].[role_id]=[Role].[id]) " +
                "WHERE [Role].[name]='" + strGroupID + "' " +
                "AND [Account].[type]='" + Constants.RBPC_ACCOUNT_DEPARTMENT + "'";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD, dbRBPC4Net).Tables[0];
        }


        /*
        * Description: 在fisreport数据库中新增1个Group
         * Parameters: string strGroupName, string strComment, string strEditorID
         * Return Value: 
         *		(string) strUUID
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
        */
        public string saveAddGroup(string strGroupName, string strComment, string strEditorID)
        {
            log.Debug("saveAddGroup");
            //如果存在同名group就停止操作并返回空字符串
            if (!getRecordSum("T_Group", "[name]='" + strGroupName + "'").Equals("0"))
            {
                log.Debug("A same record has existed~~.");
                ExceptionManager.Throw(ExceptionMsg.ERROR_SAME_GROUP_EXISTED);
                return "";
            }

            string strUUID = getUUID();
            log.Debug("new UUID=" + strUUID);
            string sqlCMD = "INSERT INTO [T_Group] " +
                "([id]" +
                ",[name]" +
                ",[type]" +
                ",[editorId]" +
                ",[comment]" +
                ",[createTime]" +
                ",[updateTime]" +
                ",[lifeCycleStatus]) " +
                "VALUES " +
                "('" + strUUID + "'" +
                ",'" + strGroupName + "'" +
                ",'" + Constants.RBPC_ACCOUNT_TYPE_GROUP + "'" +
                ",'" + strEditorID + "'" +
                ",'" + strComment + "'" +
                ",getDate()" +
                ",getDate()" +
                ",'1')";//RBPC   FISREPORT
            log.Debug(sqlCMD);
            ExecuteDataSet(sqlCMD);
            return strUUID;
        }


        /*
        * Description: 在fisreport数据库中编辑并保存1个Group
         * Parameters: string strGroupName, string strComment, string strEditorID
         * Return Value: 
         *		(string) strUUID
         * Remark: 
         *      NULL
         * Example: N/A
         * Output : N/A
        */
        public string saveEditGroup(string strGroupID, string strGroupName, string strComment, string strEditorID)
        {
            //如果存在同名group就停止操作并返回空字符串
            if (!getRecordSum("T_Group", "[id]<>'" + strGroupID + "' AND [name]='" + strGroupName + "'").Equals("0"))
            {
                log.Debug("A same record has existed~~.");
                return "";
            }

            string sqlCMD = "UPDATE [T_Group] " +
                "SET [name] = '" + strGroupName + "'" +
                ",[editorId] = '" + strEditorID + "'" +
                ",[comment] = '" + strComment + "'" +
                ",[updateTime] = getDate() " + "WHERE [id]='" + strGroupID + "'";//RBPC   FISREPORT
            log.Debug(sqlCMD);
            ExecuteDataSet(sqlCMD);
            return strGroupID;
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
        public string initPrimaryPermissions()
        {
            string sqlCMD = "";
            log.Debug(sqlCMD);
            ExecuteDataSet(sqlCMD);
            return Constants.FLAG_SUCCESS;
        }


        /*
         * Method name: 
         *		getPrimaryPermissions
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
         *      (string)成功保存了的Group的ID
         * 
         * Remark: 
         *      1）检查传入的Group name是否已经存在，
         *           如果不存在，则继续保存，否则取消保存动作
         * 
         * Example: 
         * 
         * Author: 
         *      itc204011
         * 
         * Date:
         *      2009-1
         */
        public DataTable getPrimaryPermissions(string strGroupID)
        {
            log.Debug("getPrimaryPermissions");
            log.Debug("strRBPCDataBaseServer=" + strRBPCDataBaseServer);
            log.Debug("strRBPCDataBaseName=" + strRBPCDataBaseName);
            log.Debug("strRBPCDataBaseUser=" + strRBPCDataBaseUser);
            log.Debug("strRBPCDataBasePwd=" + strRBPCDataBasePwd);

            Database dbRBPC4Net = getDbObject(strRBPCDataBaseServer, strRBPCDataBaseName, strRBPCDataBaseUser, strRBPCDataBasePwd);

            string sqlCMD = "SELECT [Permission].[name] " +
                "FROM [Permission] INNER JOIN [RolePermission] " +
                "ON [Permission].[id]=[RolePermission].[perm_id] " +
                "INNER JOIN [Role] " +
                "ON [Role].[id]=[RolePermission].[role_id] " +
                "WHERE [Role].[name]='" + strGroupID + "' " +
                "AND [Permission].[type]='" + Constants.RBPC_PERMISSION_TYPE_PRIMARY + "'";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD, dbRBPC4Net).Tables[0];
        }


        /*
         * Method name: 
         *		deleteGroup
         * 
         * Description: 
         *		删除一个组
         * 
         * Parameters: 
         *      string strGroupID: Group ID
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
         *      itc204011
         * 
         * Date:
         *      2009-1
         */
        public void deleteGroup(string strGroupID)
        {
            if (null == strGroupID || strGroupID.Equals(""))
            {
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            string sqlCMD = "DELETE FROM [T_Group] WHERE [id]='" + strGroupID + "'";
            log.Debug(sqlCMD);
            ExecuteDataSet(sqlCMD);
        }


        /*
         * Method name: 
         *		set Group Update Time
         * 
         * Description: 
         *		设定group的update time
         * 
         * Parameters: 
         *      List<Dictionary<string, string>>： 
         *      listTreeInfos<Dictionary<treeID, treeType>>
         *      Key: AttributeNames.TREE_NODE_ID, AttributeNames.TREE_NODE_TYPE
         *      
         * Return Value: 
         *      (void)
         * 
         * Remark: 
         *      1）检查传入的Group ID是否存在，
         *      2）做多7项，最少为0项
         * 
         * Example: 
         * 
         * Author: 
         *      itc204011
         * 
         * Date:
         *      2009-1
         */
        public void setGroupUpdateTime(string strGroupID)
        {
            log.Debug("setGroupUpdateTime");
            if (null == strGroupID || strGroupID.Equals(""))
            {
                log.Debug("in setGroupUpdateTime empty Group ID");
                ExceptionManager.Throw(ExceptionMsg.ERROR_AUTHORITY_NULL_PARAM);
            }

            log.Debug("strGroupID = " + strGroupID);

            string sqlCMD = "UPDATE [fisreport].[dbo].[T_Group] "
                + "SET [updateTime]=getDate() "
                + "WHERE [id] in ('" + strGroupID + "')";

            log.Debug(sqlCMD);
            ExecuteDataSet(sqlCMD);
        }
    }
}