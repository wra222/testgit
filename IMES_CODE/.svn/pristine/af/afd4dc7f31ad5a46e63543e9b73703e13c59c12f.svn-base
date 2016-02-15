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
using Microsoft.Practices.EnterpriseLibrary.Data;
using log4net;
using com.inventec.system;
using System.Reflection;


/// <summary>
///AuthorityDao 的摘要说明
/// </summary>
namespace com.inventec.imes.dao
{
    public class AuthorityDao :AbstractBaseDao
    {
        #region singlton 
        private AuthorityDao()
        {
        }
        private static readonly AuthorityDao _instance = new AuthorityDao();
        /// <summary>
        /// Instance used for singleton
        /// </summary>
        public static AuthorityDao Instance
        {
            get
            {
                //if (_instance == null)
                //    _instance =  new AuthorityDao();
                return _instance;
            }
        }

        #endregion

      

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

        public DataTable GetUPH(string line)
        {
            string sqlCMD = "SELECT*FROM ProductUPH where Line='" + line + "'ORDER BY TimeRange";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];
        
        }
        public DataTable GetUPH()
        {
            string sqlCMD = "SELECT*FROM ProductUPH ORDER BY TimeRange";
            log.Debug(sqlCMD);
            return ExecuteDataSet(sqlCMD).Tables[0];

        }

        public List<string > GetUPHLine()
        {
            List<string> ret = new List<string>();
            string sqlCMD = "SELECT distinct Line FROM ProductUPH order by Line ";
            log.Debug(sqlCMD);
            DataTable dt=  ExecuteDataSet(sqlCMD).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string line=dr["Line"].ToString();
                ret.Add(line);
            }
            return ret;
            


        }
        public  IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public  IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public  T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {  //You can log something here     
                        //throw;    
                    }
                }
            }

            return obj;
        }


     

       

    }
}