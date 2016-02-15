// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-16   Yuan XiaoWei                 create
// Known issues:
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
//using Microsoft.SqlServer.Dts.Runtime;
using IMES.Station.Interface.StationIntf;
using com.inventec.iMESWEB;
//<summary>
//Summary description for PODataUpload
//</summary>
public class PODataUpload
{
    public PODataUpload()
    {

    }

    public static bool CallDTS(string uploadId)
    {
        //string dtsFtpPath = ConfigurationManager.AppSettings["DTSFtpPath"].ToString(); ;
        //if (!dtsFtpPath.EndsWith("\\"))
        //{
        //    dtsFtpPath = dtsFtpPath + "\\";
        //}
        //string dnDtsPath = dtsFtpPath + "IMES_PAK_POData_UploadDelivery.dtsx";
        //string palletDtsPath = dtsFtpPath + "IMES_PAK_POData_UploadPallet.dtsx";
        //Application uploadDTSApp = new Application();
        //Package dnPackage = uploadDTSApp.LoadPackage(dnDtsPath, null, true);
        //Package palletPackage = uploadDTSApp.LoadPackage(palletDtsPath, null, true);
        ////set Variables


        //IPOData currentPODataService = ServiceAgent.getInstance().GetObjectByName<IPOData>(WebConstant.PODataObject);
        //string pakConnectStr = currentPODataService.GetPAKConnectionString() + ";Provider=SQLNCLI10.1;Auto Translate=False;";
        ////set datasource Connections
        //dnPackage.Variables["UploadId"].Value = uploadId;
        //dnPackage.Connections["IMES_PAK"].ConnectionString = pakConnectStr;
        //dnPackage.Connections["DNFile"].ConnectionString = dtsFtpPath + uploadId + "COMN.TXT";

        //palletPackage.Variables["UploadId"].Value = uploadId;
        //palletPackage.Connections["IMES_PAK"].ConnectionString = pakConnectStr;
        //palletPackage.Connections["PalletFile"].ConnectionString = dtsFtpPath + uploadId + "PALTNO.TXT";

        ////Execute
        //DTSExecResult dnResult = dnPackage.Execute();
        //if (dnResult.Equals(DTSExecResult.Success))
        //{
        //    DTSExecResult palletResult = palletPackage.Execute();
        //    if (palletResult.Equals(DTSExecResult.Success))
        //    {
        //        System.IO.File.Delete(dtsFtpPath + uploadId + "COMN.TXT");
        //        System.IO.File.Delete(dtsFtpPath + uploadId + "PALTNO.TXT");
        //        return true;
        //    }
        //    else
        //    {
        //        System.IO.File.Delete(dtsFtpPath + uploadId + "COMN.TXT");
        //        System.IO.File.Delete(dtsFtpPath + uploadId + "PALTNO.TXT");
        //        throw new Exception(palletPackage.Errors[0].Description);
        //    }
        //}
        //else
        //{
        //    System.IO.File.Delete(dtsFtpPath + uploadId + "COMN.TXT");
        //    System.IO.File.Delete(dtsFtpPath + uploadId + "PALTNO.TXT");
        //    throw new Exception(dnPackage.Errors[0].Description);
        //}
        return false;
    }
}
