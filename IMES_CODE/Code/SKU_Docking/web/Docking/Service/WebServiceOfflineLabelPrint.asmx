<%@ WebService Language="C#" Class="WebServiceOfflineLabelPrint" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Collections;
using IMES.DataModel;
using IMES.Docking.Interface.DockingIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]

public class WebServiceOfflineLabelPrint : System.Web.Services.WebService
{
    IOfflineLabelPrintForDocking iOfflineLabelPrint = ServiceAgent.getInstance().GetObjectByName<IOfflineLabelPrintForDocking>(WebConstant.OfflineLabelPrintForDockingObject);
    bool bIsFirstIn = false;
 
       
    [WebMethod]
    public bool isFirst_In()
    {
        if (bIsFirstIn == false)
        {
            bIsFirstIn = true;
            return false;
        }
        
        return true;
    }
    
    [WebMethod]
    public string HelloWorld() {
        //throw new FieldAccessException("errormess");
        return "Hello World3";
    }

    public IList<OfflineLableSettingDef> get_offline_lable_list()
    {
        IList<OfflineLableSettingDef> retList = null;
        try
        {
            retList = iOfflineLabelPrint.Get_offline_lable_list();
        }
        catch (FisException e)
        {
            //retList.Clear();
            throw new Exception(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return retList;
    }



    [WebMethod]
    public ArrayList inputProdId(string pdLine, string prodId, string editor, string station, string customer)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            retLst = iOfflineLabelPrint.InputProdId(pdLine, prodId, editor, station, customer);
            //retLst.Add(WebConstant.SUCCESSRET);
            //retLst.Add(prodId);
        }
        catch (FisException e)
        {
            retLst.Clear();
            retLst.Add(e.mErrmsg);
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
            iOfflineLabelPrint.InputBoxId(prodId, boxId);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(boxId);
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
    public ArrayList Cancel(string MB, string station)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iOfflineLabelPrint.Cancel(MB);

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

