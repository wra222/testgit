<%@ WebService Language="C#" Class="WebServiceGeneSMTMO" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;
using log4net;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceGeneSMTMO : System.Web.Services.WebService
{
    IGenSMTMO iGenSMTMO = ServiceAgent.getInstance().GetObjectByName<IGenSMTMO>(WebConstant.GenSMTMOObject);
    
    [WebMethod]
    public string HelloWorld() {
        //throw new FieldAccessException("errormess");
        return "Hello World3";
    }

    [WebMethod]
    public ArrayList generate(string model, string qty, string isMassProduc, string remark, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();        

        try
        {
            string NewSMTMO = iGenSMTMO.GenerateSMTMO(model, int.Parse(qty), isMassProduc, remark, editor, station, customer);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(NewSMTMO);
            retLst.Add(isMassProduc);
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }       
        return retLst;      
    }

    [WebMethod]
    public ArrayList deleteMo(List<string> moList, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();
        try
        {

            iGenSMTMO.Delete(moList, editor,station,customer);
            retLst.Add(WebConstant.SUCCESSRET);

        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
      
        return retLst;       
    }     
}
