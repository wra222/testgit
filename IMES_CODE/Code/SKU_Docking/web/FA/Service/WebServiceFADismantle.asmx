<%--
/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: DismantleFA page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-04-10  Zhao Meili(DD)        Create                      
 * Known issues:
 */
 --%>

<%@ WebService Language="C#" Class="WebServiceFADismantle" %>

using System;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.inventec.iMESWEB;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Collections.Generic;


[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WebServiceFADismantle : System.Web.Services.WebService
{
    IProduct iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(WebConstant.CommonObject);
    
    [WebMethod]
    public string Dismantle(string snorproid,string sDismantletype,string sKeyparts,string sReturnStation,  string line,string Pcode, string editor, string station, string customer)
    {
        try
        {
            IDismantleFA DismantleFAManager = (IDismantleFA)ServiceAgent.getInstance().GetObjectByName<IDismantleFA>(WebConstant.DismantleFA);
            DismantleFAManager.Dismantle(snorproid, sDismantletype, sKeyparts, sReturnStation,line, Pcode, editor, station, customer);
            return "";
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public ArrayList inputProdId(string snorproid,string line,string Pcode, string editor, string station, string customer)
    {
        IList<string> prodinfoList = new List<string>();

        ArrayList retLst = new ArrayList();
        try
        {
            IDismantleFA DismantleFAManager = (IDismantleFA)ServiceAgent.getInstance().GetObjectByName<IDismantleFA>(WebConstant.DismantleFA);
            prodinfoList = DismantleFAManager.InputProdId(snorproid, line, Pcode, editor, station, customer);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(prodinfoList[0]);
            retLst.Add(prodinfoList[1]);
            retLst.Add(prodinfoList[2]);
            retLst.Add(prodinfoList[3]);
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
        //model = "model";
        //retLst.Add(WebConstant.SUCCESSRET);
        //retLst.Add(prodId);
        //retLst.Add(model);
        //defectList = new List<DefectInfo>();
        //DefectInfo defect = new DefectInfo();
        //defect.id = "aaa";
        //defect.description = "111111111111111222222222222222333333333333333333344444444444444444455555555555555";
        //defectList.Add(defect);
        //defect = new DefectInfo();
        //defect.id = "bbb";
        //defect.description = "bbb";
        //defectList.Add(defect);
        //retLst.Add(defectList);
        //return retLst;
    }

    
}

