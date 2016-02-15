<%@ WebService Language="C#" Class="WebServiceCommonLabelPrint" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using System.Collections;
using IMES.Infrastructure;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceCommonLabelPrint  : System.Web.Services.WebService
{
    ICommonLabelPrint iCommonLabelPrint = ServiceAgent.getInstance().GetObjectByName<ICommonLabelPrint>(WebConstant.CommonLabelPrint);

	public struct S_Result_CommonLabelPrint_GetOfflineLabelSetting
	{
		public int PrintMode;
		public string labelSpec;
		public string fileName;
		public string SPName;
		public ArrayList ParamList;
	}
	
    [WebMethod]
    public ArrayList GetOfflineLabelSettingList(string editor, string customer)
    {
        ArrayList retLst = new ArrayList();
        ArrayList retService = new ArrayList();
        IList<string> res = new List<string>();
        try
        {
            retService = iCommonLabelPrint.GetOfflineLabelSettingList(editor, customer);
			retLst.Add(WebConstant.SUCCESSRET);
			retLst.Add(retService);            
        }
        catch (FisException e)
        {
            throw e;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;
    }
	
    [WebMethod]
    public ArrayList GetOfflineLabelSetting(string labelDescr, string editor, string customer)
    {
        ArrayList ret = new ArrayList();
        ArrayList result = new ArrayList();
        try
        {
            ret = iCommonLabelPrint.GetOfflineLabelSetting(labelDescr, editor, customer);
			
			S_Result_CommonLabelPrint_GetOfflineLabelSetting s = new S_Result_CommonLabelPrint_GetOfflineLabelSetting
			{
				PrintMode = (int)ret[0],
				labelSpec = (string)ret[1],
				fileName = (string)ret[2],
				SPName = (string)ret[3],
				ParamList = (ArrayList)ret[4]
			};
			
            result.Add(WebConstant.SUCCESSRET);
            result.Add(s);

            return result;
        }
        catch (FisException e)
        {
            throw e;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

}
