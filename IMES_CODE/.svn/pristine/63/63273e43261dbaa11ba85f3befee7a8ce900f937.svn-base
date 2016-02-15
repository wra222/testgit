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
    }
        
}
