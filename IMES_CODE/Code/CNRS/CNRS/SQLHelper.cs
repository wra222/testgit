using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace UTL.SQL
{
   
    class SQLHelper
    {
        #region create input SQL parameter
        static public  void createInputSqlParameter(SqlCommand cmd,
                                                                                string name,
                                                                                 int size,
                                                                                    string value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.VarChar, size);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }
        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,                                                             
                                                              DateTime value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.DateTime);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }

        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                              int size,      
                                                              DateTime value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.SmallDateTime);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }

        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                              int size,
                                                              int value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.Int);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }
        static public void createInputSqlParameter(SqlCommand cmd,
                                                              string name,
                                                               int value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.Int);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }
        static public  void createInputSqlParameter(SqlCommand cmd,
                                                                    string name,
                                                                     int size,
                                                                     float value)
        {
            SqlParameter param = cmd.Parameters.Add(name, SqlDbType.Float);
            param.Direction = ParameterDirection.Input;
            param.Value = value;
        }
        #endregion

        
    }
}
