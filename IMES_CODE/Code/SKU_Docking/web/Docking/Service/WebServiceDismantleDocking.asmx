<%--
/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: DismantleForDocking page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2012-06-08  Kaisheng             Create                      
 * Known issues:
 */
 --%>

<%@ WebService Language="C#" Class="WebServiceDismantleForDocking" %>

using System;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.inventec.iMESWEB;
using IMES.Docking.Interface.DockingIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Collections.Generic;


[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WebServiceDismantleForDocking : System.Web.Services.WebService
{
    IProduct iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(WebConstant.CommonObject);
    
    [WebMethod]
    public string Dismantle(string snorproid,string sDismantletype,string sKeyparts,string sReturnStation,  string line,string Pcode, string editor, string station, string customer)
    {
        try
        {
            IDismantleForDocking DismantleForDockingManager = (IDismantleForDocking)ServiceAgent.getInstance().GetObjectByName<IDismantleForDocking>(WebConstant.DismantleForDocking);
            DismantleForDockingManager.Dismantle(snorproid, sDismantletype, sKeyparts, sReturnStation,line, Pcode, editor, station, customer);
            return "";

        }
        catch (FisException ex)
        {
            return ex.mErrmsg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [WebMethod]
    public ArrayList inputProdId(string snorproid,string line,string Pcode, string editor, string station, string customer)
    {
        IList<string> prodinfoList = new List<string>();

        ArrayList retLst = new ArrayList();
        try
        {
            //IDismantleForDocking DismantleForDockingManager = (IDismantleForDocking)ServiceAgent.getInstance().GetObjectByName<IDismantleForDocking>(WebConstant.DismantleForDocking);
            //prodinfoList = DismantleForDockingManager.InputProdId(snorproid, line, Pcode, editor, station, customer);
            //retLst.Add(WebConstant.SUCCESSRET);
            //retLst.Add(prodinfoList[0]);
            //retLst.Add(prodinfoList[1]);
            //retLst.Add(prodinfoList[2]);
            //retLst.Add(prodinfoList[3]);
            return retLst;
        }
        catch (FisException e)
        {
            retLst.Add(e.mErrmsg);
            return retLst;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //return retLst;
    }

    
}

