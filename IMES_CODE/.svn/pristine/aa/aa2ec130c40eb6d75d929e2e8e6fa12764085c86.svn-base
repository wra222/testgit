using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace IMES.LD
{
    public class SqlStatment
    {
        public static string HistoryDBName = "HistoryDBServer";

        #region 雷雕：回传信息及写ProductLog
        //GetPrintContent
        public static PrintResponse GetPrintContentBySN(string sn)
        {
            string sqlStr = @"select distinct a.CUSTSN as SN,b.Value as PN,c.Value as Warranty,d.Value as MN
                              from   Product a,ModelInfo b,ModelInfo c,ModelInfo d
                              where  a.CUSTSN=@SN 
                                     and a.Model=b.Model 
                                     and a.Model=c.Model
                                     and a.Model=d.Model
                                     and b.Name='PN'
                                     and c.Name='WARRANTY'
                                     and d.Name='MN1'";

            DataTable dt = SqlHelper.ExecuteDataTable(System.Data.CommandType.Text,
                sqlStr, new SqlParameter("@SN", sn));

            if (dt == null || dt.Rows.Count == 0)
            {
                PrintResponse printResponse = new PrintResponse();
                printResponse.SN = sn;
                printResponse.PN = "";
                printResponse.Warranty = "";
                printResponse.MN = "";
                printResponse.Status=-1;
                printResponse.ErrorMsg = "The SN: " + sn + " is not exist in DB!";

                return printResponse;
            }
            else 
            {
                return ToPrintResponse(dt.Rows[0]);
            }
        }

        private static PrintResponse ToPrintResponse(DataRow row)
        {
            PrintResponse printResponse = new PrintResponse();
            printResponse.SN = (string)row["SN"];
            printResponse.PN = (string)row["PN"];
            printResponse.Warranty = (string)row["Warranty"];
            printResponse.MN = (string)row["MN"];
            printResponse.ErrorMsg = "";
            printResponse.Status = 1;

            return printResponse;
        }

        //insert ProductLog
        public static void InsertProductLog(string sn, int status)
        {
            string sqlStr = @"insert ProductLog
                                    (ProductID, Model, Station, Status, Line, Editor, Cdt)
                              select ProductID,Model,'LD',@Status,'E','LD',GETDATE()
                              from   Product 
                              where  CUSTSN=@SN";
             SqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, sqlStr,
                new SqlParameter("@SN", sn),
                new SqlParameter("@Status", status));
        }
        #endregion


        #region EoLA：QC无纸化专案回传信息
        public static ModelResponse GetModelByCustSN(string sn)
        {
            string sqlStr = @"select top 1 a.ProductID,a.CUSTSN,a.Model,b.Family,c.CustomerID
                              from   Product a, Model b ,Family c
                              where  a.Model = b.Model  and c.Family=b.Family
                                     and (a.CUSTSN =@SN or a.ProductID =@SN)
                              order by a.Cdt";

            DataTable dtcust = SqlHelper.ExecuteDataTable(HistoryDBName, System.Data.CommandType.Text, sqlStr,
                new SqlParameter("@SN", sn));

            if (dtcust == null || dtcust.Rows.Count == 0)
            {
                ModelResponse modelResponse = new ModelResponse();

                modelResponse.ProductID = "";
                modelResponse.CustSN = sn;
                modelResponse.Model = "";
                modelResponse.Family = "";
                modelResponse.Customer = "";
                modelResponse.ErrorCode = -1;
                modelResponse.Message = "The CustSN: " + sn + " is not exist in DB!";

                return modelResponse;
            }
            else
            {
                return ToModelResponse(dtcust.Rows[0]);
            }
        }

        private static ModelResponse ToModelResponse(DataRow row)
        {
            ModelResponse modelResponse = new ModelResponse();

            modelResponse.CustSN = (string)row["CustSN"];
            modelResponse.ProductID = (string)row["ProductID"];
            modelResponse.Model = (string)row["Model"];
            modelResponse.Family = (string)row["Family"];
            modelResponse.Customer = (string)row["CustomerID"];
            modelResponse.ErrorCode = 0;
            modelResponse.Message = "";

            return modelResponse;
        }
        #endregion


        #region 自动贴标签：回传信息
        public static LocationResponse GetLocationandCoordinateBySN(string sn)
        {
            string sqlStr = @"select CUSTSN as SN,@Location as Location,@Coordinate as Coordinate
                              from   Product where CUSTSN=@SN";

            DataTable dt = SqlHelper.ExecuteDataTable(HistoryDBName,System.Data.CommandType.Text, sqlStr,
                new SqlParameter("@SN", sn),
                new SqlParameter("@Location", "Location"),
                new SqlParameter("@Coordinate", "Coordinate"));

            if (dt == null || dt.Rows.Count == 0)
            {
                LocationResponse locationResponse = new LocationResponse();
                locationResponse.SN = sn;
                locationResponse.Location = "";
                locationResponse.Coordinate = "";
                locationResponse.Status = -1;
                locationResponse.ErrorMsg = "The SN: " + sn + " is not exist in DB!";

                return locationResponse;
            }
            else
            {
                return ToLocationResponse(dt.Rows[0]);
            }
        }

        private static LocationResponse ToLocationResponse(DataRow row)
        {
            LocationResponse locationResponse = new LocationResponse();
            locationResponse.SN = (string)row["SN"];
            locationResponse.Location = (string)row["Location"];
            locationResponse.Coordinate = (string)row["Coordinate"];
            locationResponse.ErrorMsg = "";
            locationResponse.Status = 1;

            return locationResponse;
        }
        #endregion
        #region IMES 收集Client端信息
        public static void InsertData(string mac, string ip, string hostname, string UserName,string Domain)
        {
             
            string IMESRPBC = "IMESRPBC";
            string sqlStr = @" insert  ClientCollection  Values(@Hostname,@mac,@ip,@userName,@Domain,getdate()) ";
            int dt = SqlHelper.ExecuteNonQuery(IMESRPBC, System.Data.CommandType.Text, sqlStr,
               new SqlParameter("@Hostname", hostname),
               new SqlParameter("@mac", mac),
               new SqlParameter("@ip", ip),
              new SqlParameter("@userName", UserName),
              new SqlParameter("@Domain", Domain));
        }
        #endregion
        #region  IMES Check Client Ligin
        public static DataTable CheckLogin(string Hostname, string Mac,string Domain)
        {
            DataTable Result = null;
             string IMESRPBC = "IMESRPBC";
             string sqlStr = @"exec dbo.IMES_CheckLogin  @Hostname,@Mac,@Domain";
            Result = SqlHelper.ExecuteDataTable(IMESRPBC, CommandType.Text, sqlStr,
                 new SqlParameter("@Hostname", Hostname),
                      new SqlParameter("@Mac", Mac),
                     new SqlParameter("@Domain", Domain));

            return Result;
        }
	   #endregion
        #region  获取IMES值班表
        public static DataTable GetFisSupport()
        {
            DataTable Result = null;
            string IMESRPBC = "IMESRPBC";
            string sqlStr = @"exec dbo.IMES_GetFisTeam";
            Result = SqlHelper.ExecuteDataTable(IMESRPBC, CommandType.Text, sqlStr);

            return Result;
        }

        #endregion
        #region UPS 工具login 检查
        public static DataTable UPSCheckUsername(string user,string password)
        {
            DataTable Result = null;
            string IMESRPBC = "IMESRPBC";
            string sqlStr = @"exec dbo.IMES_UPS_CheckLogin  @user,@password";
            Result = SqlHelper.ExecuteDataTable(IMESRPBC, CommandType.Text, sqlStr,
                        new SqlParameter("@user", user),
                      new SqlParameter("@password", password)
                );

            return Result;
        }
        #endregion

        #region SA RFID GetMBinfo
        /// <summary>
        /// RFID 获取MB信息
        /// </summary>
        /// <param name="PCBNO"></param>
        /// <returns></returns>
        public static MBInfoResponse GetMBInfoRFID(string PCBNO)
        {
         
            string sqlStr = @"SELECT a.PCBNo,a.PCBModelID,c.InfoValue AS MBCT,a.MAC,a.ECR,a.IECVER,b.Line
                    FROM PCB a INNER JOIN dbo.PCBStatus b ON a.PCBNo=b.PCBNo AND a.PCBNo=@PCBNO
                    LEFT JOIN PCBInfo c ON  a.PCBNo=c.PCBNo AND c.InfoType='MBCT' ";

            DataTable dt = SqlHelper.ExecuteDataTable(System.Data.CommandType.Text,
                sqlStr, new SqlParameter("@PCBNO", PCBNO));

            if (dt == null || dt.Rows.Count == 0)
            {
                MBInfoResponse mbinfo = new MBInfoResponse();
                mbinfo.MBSN = PCBNO;
                mbinfo.MBPartNo = "";
                mbinfo.MBCT = "";
                mbinfo.MAC = "";
                mbinfo.ECR = "";
                mbinfo.Ver = "";
                mbinfo.Pdline = "";
                mbinfo.Status = "F";
                mbinfo.ErrorText = "The PCBNO: " + PCBNO + " is not exist in DB!";
                mbinfo.Remark = "The PCBNO: " + PCBNO + " is not exist in DB!";
                return mbinfo;
            }
            else
            {
                MBInfoResponse mbinfo = new MBInfoResponse();
                mbinfo.MBSN = PCBNO;
                mbinfo.MBPartNo =(string) dt.Rows[0]["PCBModelID"];
                mbinfo.MBCT = (string)dt.Rows[0]["MBCT"];
                mbinfo.MAC = (string)dt.Rows[0]["MAC"];
                mbinfo.ECR = (string)dt.Rows[0]["ECR"];
                mbinfo.Ver = (string)dt.Rows[0]["IECVER"];
                mbinfo.Pdline = (string)dt.Rows[0]["Line"];
                mbinfo.Status = "T";
                mbinfo.ErrorText = "";
                mbinfo.Remark = "";
                return mbinfo;
            }
        }
        #endregion 
    }
}
