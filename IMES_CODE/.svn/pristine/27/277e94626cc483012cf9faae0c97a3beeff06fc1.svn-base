using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using IMES.LD;
using log4net;
using System.Reflection;

namespace IMES.LC
{
    /// <summary>
    /// Summary description for GetLocationandCoordinate
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GetLocationandCoordinate : System.Web.Services.WebService
    {
        public static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 获得DB查询的SN、Location、Coordinate信息
        /// Stautus>0 =>success and Status<0 fail
        /// Status=-1 => can't find data
        /// Status=-2 => error
        /// Status=1 => success
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        [WebMethod]
        public LocationResponse GetLocationResponse(string SN)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}(CustSN:{1})", methodName, SN);
            try
            {
                //1.检查传进来的参数
                Execute.ValidateParameter(SN);

                //2.获取DB中的数据
                LocationResponse locationResponse = Execute.GetLocationandCoordinateInfo(SN);
                logger.DebugFormat("Reponse data:{0}", locationResponse.ToString());
                return locationResponse;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                LocationResponse locationResponse = new LocationResponse();

                locationResponse.SN = SN;
                locationResponse.Location = "";
                locationResponse.Coordinate = "";
                locationResponse.Status = -2;
                locationResponse.ErrorMsg = e.Message;
                logger.DebugFormat("Reponse data:{0}", locationResponse.ToString());
                return locationResponse;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
    }
}
