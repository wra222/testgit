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

namespace IMES.LD
{
    public class Execute
    {

        //check input parameter
        public static void ValidateParameter(string sn)
        {
            try
            {
                if (string.IsNullOrEmpty(sn))
                {
                    throw new Exception("The SN input is null or no data!");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void ValidateParameter(string sn, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(sn))
                {
                    throw new Exception("The SN input is null or no data!");
                }

                if ((status != 1) && (status != 0))
                {
                    throw new Exception("The Status is not 0 or 1");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #region 雷雕：回传信息及写ProductLog
        //1.response print message
        public static PrintResponse GetPrintContentInfo(string sn)
        {
            PrintResponse printResponse;
            try 
            {
                printResponse = SqlStatment.GetPrintContentBySN(sn);
                return printResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //2.insert DB
        public static void WriteProductLogBySN(string sn, int status)
        {
            try
            {
                SqlStatment.InsertProductLog(sn, status);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region EoLA：QC无纸化专案回传信息
        //response model message
        public static ModelResponse modelResponseMsg(string sn)
        {
            ModelResponse response;
            try
            {
                response = SqlStatment.GetModelByCustSN(sn);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 自动贴标签：回传信息
        public static LocationResponse GetLocationandCoordinateInfo(string sn)
        {
            LocationResponse locationResponse;
            try
            {
                locationResponse = SqlStatment.GetLocationandCoordinateBySN(sn);
                return locationResponse;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #region   SA RFID 获取MB信息
        public static MBInfoResponse GetMBInfoRFID(string PCBNO)
        {
            MBInfoResponse mbinfo;
            try
            {
                mbinfo = SqlStatment.GetMBInfoRFID(PCBNO);
                return mbinfo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
        
}
