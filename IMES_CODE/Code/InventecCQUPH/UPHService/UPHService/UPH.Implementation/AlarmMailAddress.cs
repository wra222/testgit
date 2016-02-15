using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPH.Interface;
using log4net;
using System.Reflection;
using UPH.DB;
using UPH.Entity.Infrastructure.Framework;
using UPH.Entity.Infrastructure.Interface;
using UPH.Entity.Repository.Meta.IMESSKU;
using System.Data;

namespace UPH.Implementation
{  

    public class AlarmMailAddress : MarshalByRefObject,IAlarmMailAddress
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<string> GetLine(string Process)
        {
            List<string> list = new List<string>();
            StringBuilder st = new StringBuilder();
            if (Process == "ALL")
            {
                st.AppendLine("SELECT DISTINCT PdLine FROM PdLine");
                DataTable dt = new DataTable();
                string Connection = SQLHelper.ConnectionString_ONLINE(0);
                dt = SQLHelper.ExecuteDataFill(Connection,
                    System.Data.CommandType.Text,
                    st.ToString()
                    );
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString());
                }
                return list;
            }
            else
            {
                st.AppendLine("SELECT DISTINCT PdLine FROM PdLine where Process=@Process");
                DataTable dt = new DataTable();
                string Connection = SQLHelper.ConnectionString_ONLINE(0);
                dt = SQLHelper.ExecuteDataFill(Connection,
                    System.Data.CommandType.Text,
                    st.ToString(),
                    SQLHelper.CreateSqlParameter("@Process", Process.ToString())
                    );
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString());
                }
                return list;
            }
        }
        public DataTable GetLine1()
        {
            string sqlCMD = "SELECT DISTINCT PdLine FROM PdLine ";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD
                                           );
            return dt;
        }

        public List<string> GetMailProcess()
        {
            List<string> list = new List<string>();
            StringBuilder st = new StringBuilder();
            st.AppendLine("SELECT DISTINCT Process FROM AlarmStatus");
            DataTable dt = new DataTable();
            string Connection = SQLHelper.ConnectionString_ONLINE(0);
            dt = SQLHelper.ExecuteDataFill(Connection,
                System.Data.CommandType.Text,
                st.ToString()
                );
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[0].ToString());
            }
            return list;
        }
        public DataTable GetAlarmALL()
        {
            string sqlCMD = "SELECT *FROM dbo.AlarmMailAddress ";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD
                                           );
            return dt;
        }
        public DataTable GetMail(string Dept, string Process, string PdLine)
        {
            string sqlCMD = "SELECT *FROM dbo.AlarmMailAddress WHERE Dept=@Dept AND Process=@Process AND PdLine=@PdLine ";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("Process", Process.Trim()),
                                           SQLHelper.CreateSqlParameter("Dept", Dept.Trim()),
                                           SQLHelper.CreateSqlParameter("PdLine", PdLine.Trim())


                                           );
            return dt;
        }
        public DataTable UpdateMail(string Editor, string Remark, string PdLine, string MailAddress)
        {
            string sqlCMD = "UPDATE dbo.AlarmMailAddress SET MailAddress=@MailAddress,Remark=@Remark,Editor=@Editor,Udt=GETDATE() WHERE PdLine=@PdLine ";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           SQLHelper.CreateSqlParameter("Editor", Editor.Trim()),
                                           SQLHelper.CreateSqlParameter("Remark", Remark.Trim()),
                                           SQLHelper.CreateSqlParameter("PdLine", PdLine.Trim()),
                                           SQLHelper.CreateSqlParameter("MailAddress", MailAddress.Trim())


                                           );
            return dt;
        }
        public DataTable GetMail2(string PdLine, string Process)
        {
            string sqlCMD = "SELECT *FROM dbo.AlarmMailAddress WHERE  PdLine=@PdLine and Process=@Process ";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,
                                           
                                           SQLHelper.CreateSqlParameter("PdLine", PdLine.Trim()),
                                           SQLHelper.CreateSqlParameter("Process", Process.Trim())


                                           );
            return dt;
        }
        public DataTable InsertMail(string Editor, string Remark, string PdLine, string MailAddress, string Process, string Dept)
        {
            string sqlCMD = "INSERT dbo.AlarmMailAddress( Dept ,Process ,PdLine ,MailAddress ,Remark ,Editor ,Cdt ,Udt)VALUES  ( @Dept , @Process , @PdLine , @MailAddress , @Remark , @Editor , GETDATE() , GETDATE()) ";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,

                                           SQLHelper.CreateSqlParameter("PdLine", PdLine.Trim()),
                                           SQLHelper.CreateSqlParameter("Process", Process.Trim()),
                                           SQLHelper.CreateSqlParameter("Editor", Editor.Trim()),
                                           SQLHelper.CreateSqlParameter("Remark", Remark.Trim()),
                                           SQLHelper.CreateSqlParameter("MailAddress", MailAddress.Trim()),
                                           SQLHelper.CreateSqlParameter("Dept", Dept.Trim())


                                           );
            return dt;
        }

        public DataTable DELETEMail(string Editor, string PdLine, string Process)
        {
            string sqlCMD = "DELETE FROM dbo.AlarmMailAddress WHERE PdLine=@PdLine AND Process=@Process ";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,

                                           SQLHelper.CreateSqlParameter("PdLine", PdLine.Trim()),
                                           SQLHelper.CreateSqlParameter("Process", Process.Trim()),
                                           SQLHelper.CreateSqlParameter("Editor", Editor.Trim())
                                           );
            return dt;
        }
        public DataTable GetLineMail(string PdLine)
        {
            string sqlCMD = "SELECT *FROM dbo.AlarmMailAddress WHERE  PdLine=@PdLine";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_ONLINE(0),
                                          System.Data.CommandType.Text,
                                          sqlCMD,

                                           SQLHelper.CreateSqlParameter("PdLine", PdLine.Trim())


                                           );
            return dt;
        }



    }
}
