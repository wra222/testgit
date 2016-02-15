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
                printResponse.ErrorMsg = "Cant't find the information for SN: '" + sn + "' in DB!";

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
                modelResponse.Message = "Cant't find the information for SN: '" + sn + "' in DB!";

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
            string sqlStr = @"select c.PartNo,ltrim(rtrim(c.LightNo)) as Location,ltrim(rtrim(d.Descr)) as Coordinate
                              from   Product a(nolock), ModelBOM b(nolock), WipBuffer c(nolock), 
                                     FamilyInfo d(nolock) ,Model e(nolock) 
                              where  c.Code='Automatic' and a.CUSTSN=@SN
                                     and a.Model=b.Material and b.Component=c.PartNo 
                                     and a.Model=e.Model and d.Family=e.Family 
                                     and c.Code=d.Name and c.PartNo=d.Value
                              order by c.PartNo";

            DataTable dt = SqlHelper.ExecuteDataTable(HistoryDBName,System.Data.CommandType.Text, sqlStr,
                new SqlParameter("@SN", sn));

            if (dt == null || dt.Rows.Count == 0)
            {
                LocationResponse locationResponse = new LocationResponse();
                locationResponse.SN = sn;
                locationResponse.Location = "";
                locationResponse.Coordinate = "";
                locationResponse.Status = -1;
                locationResponse.ErrorMsg = "Cant't find the information for SN: '" + sn + "' in DB!";

                return locationResponse;
            }
            else
            {
                DataRowCollection rows = dt.Rows;
                LocationResponse locationResponse = new LocationResponse();
                string locations = "";
                string coordinates = "";

                //将多个Location整理到一个栏位,多个Coordinate整理到一个栏位
                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i];
                    string location = (string)row["Location"];
                    string coordinate = (string)row["Coordinate"];

                    if (i == rows.Count - 1)
                    {
                        locations = locations + location;
                        coordinates = coordinates + "(" + coordinate + ")";
                    }
                    else
                    {
                        locations = locations + location + ",";
                        coordinates = coordinates + "(" + coordinate + "),";
                    }
                }

                locationResponse.SN = sn;
                locationResponse.Location = locations;
                locationResponse.Coordinate = coordinates;
                locationResponse.ErrorMsg = "";
                locationResponse.Status = 1;

                return locationResponse;
            }
        }
        #endregion
    }
}
