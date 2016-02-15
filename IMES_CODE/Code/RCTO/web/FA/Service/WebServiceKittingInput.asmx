<%@ WebService Language="C#" Class="WebServiceKittingInput" %>


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

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]

public class WebServiceKittingInput : System.Web.Services.WebService
{
    IKittingInput iKittingInput = ServiceAgent.getInstance().GetObjectByName<IKittingInput>(WebConstant.KittingInputObject);
    
    [WebMethod]
    public string HelloWorld() {
        //throw new FieldAccessException("errormess");
        return "Hello World3";
    }


    [WebMethod]
    public ArrayList inputProdId(string pdLine, string prodId, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            retLst = iKittingInput.InputProdId(pdLine, prodId, editor, station, customer);
            //retLst.Add(WebConstant.SUCCESSRET);
            //retLst.Add(prodId);
        }
        catch (FisException e)
        {
            //retLst.Clear();
            //retLst.Add(e.mErrmsg);
            throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;

    }

    [WebMethod]
    public ArrayList inputBoxId(string prodId, string boxId)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            iKittingInput.InputBoxId(prodId, boxId);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(boxId);
        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
            //retLst.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retLst;
    }
    
    [WebMethod]
    public ArrayList Cancel(string MB)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iKittingInput.Cancel(MB);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(MB);

        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }
  
}

