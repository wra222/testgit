/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: KP Print Impl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-04-22   LuycLiu     Create 

 * 
 * Known issues:Any restrictions about this file     
 */

using System;
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.UnitOfWork;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{

    public class ImportDataToTestBoxDataLog : MarshalByRefObject, IImportDataToTestBoxDataLog
    {
     private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



 


        public DataTable GetPCBData(string mbsnList)
        {

            string strSQL = @" select p.PCBNo, p.CUSTSN,p.MAC,pf.InfoValue from PCB p,PCBInfo pf where p.PCBNo=pf.PCBNo
                                           and pf.InfoType='ShipMode' and p.PCBNo in (" + mbsnList + ")";
            DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, null);
           return dt;
        }
      
        public void InsertAllData(DataTable dt,string user,string station)
        {
            string[] colName = new string[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            { colName[i] = dt.Columns[i].ColumnName; }
            try
            {
                SqlTransactionManager.Begin();
                foreach (DataRow dr in dt.Rows)
                {
                    InsertTextBoxDataLog(dr, colName);
                    InsertPCBLog(user, station, dr["MBSN"].ToString());
                    UpdatePCBStatus(user, station, dr["MBSN"].ToString());
                }

                SqlTransactionManager.Commit();
            }
         
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw new SystemException(e.Message);
            }
            finally
            {
                   SqlTransactionManager.Dispose();
                   SqlTransactionManager.End();
                logger.Debug("ImportDataToTestBoxDataLog");
               }
        
            
          
        }
        private void UpdatePCBStatus(string user, string station,string pcbno)
        {
            string sql = "update PCBStatus set Editor=@user,Station=@station,Udt=getdate(),Status=@status where PCBNo=@pcbno";

            SqlParameter paraUser = new SqlParameter("@user", SqlDbType.VarChar);
            paraUser.Direction = ParameterDirection.Input;
            paraUser.Value = user;
            SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar);
            paraStation.Direction = ParameterDirection.Input;
            paraStation.Value = station;
            SqlParameter paraPCBNo= new SqlParameter("@pcbno", SqlDbType.VarChar);
            paraPCBNo.Direction = ParameterDirection.Input;
            paraPCBNo.Value = pcbno;
            SqlParameter paraStatus= new SqlParameter("@status", SqlDbType.Int);
            paraStatus.Direction = ParameterDirection.Input;
            paraStatus.Value = 1;
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData, CommandType.Text, sql, paraUser, paraStation, paraPCBNo,paraStatus);
    
        }
        private void InsertPCBLog(string user, string station, string pcbno)
        {
            string sql = @"insert PCBLog select @pcbno,p.PCBModelID,@station,1,s.Line,@user,GETDATE()  from PCB p, PCBStatus s 
                                 where p.PCBNo=@pcbno and s.PCBNo=@pcbno ";
            SqlParameter paraName = new SqlParameter("@user", SqlDbType.VarChar);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = user;
            SqlParameter paraName2 = new SqlParameter("@station", SqlDbType.VarChar);
            paraName2.Direction = ParameterDirection.Input;
            paraName2.Value = station;
            SqlParameter paraName3 = new SqlParameter("@pcbno", SqlDbType.VarChar);
            paraName3.Direction = ParameterDirection.Input;
            paraName3.Value = pcbno;
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData, CommandType.Text, sql, paraName, paraName2, paraName3);
        
        }
        private void InsertTextBoxDataLog(DataRow dr,string[] colName)
        {
            string sql1 = @"INSERT INTO TestBoxDataLog([PCBNo],[SerialNumber],[MACAddress],[ProductID],[FixtureID],[WC],[LineNum],[IsPass],[TestCase],[Descr],[ErrorCode],[TestTime]
                                   ,[DateManufactured],[IMEI],[IMSI],[EventType],[DeviceAttribute],[Platform],[ICCID],[ModelNumber],[EAN],[PublicKey],[PrivateKey],[Cdt])  Values";
            string col = "";
            string sql2 = "";
            string sqlAll = "";
            string value="";

            SqlParameter[]  sqlParmArray = new SqlParameter[colName.Length+1]; // +1 for @Cdt
            int iCount=0;
            foreach (string s in colName)
            {
                col = "@" + s;
                sql2 = sql2 + col + ",";
                value= dr[s].ToString();
                sqlParmArray[iCount] = new SqlParameter(col, value);
                          
            
                sqlParmArray[iCount].Direction = ParameterDirection.Input;
           
                iCount++;
            }
            sqlParmArray[iCount] = new SqlParameter("@Cdt", DateTime.Now); ;
            sqlParmArray[iCount].Direction = ParameterDirection.Input;
         
            sql2 = "(" + sql2 + "@Cdt)";
            sqlAll = sql1 + sql2;
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData, CommandType.Text,sqlAll, sqlParmArray);
    
        
        }
     
    }
}
