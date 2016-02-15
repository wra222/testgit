<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: MB Label Print(SA)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-10-27  207006(EB2)          Create 
 Known issues:
 --%>

<%@ WebService Language="C#" Class="SpecialModelForITCNDService" %>

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
public class SpecialModelForITCNDService : System.Web.Services.WebService
{
    SpecialModelForItcnd iSpecialModelForItcnd = ServiceAgent.getInstance().GetObjectByName<SpecialModelForItcnd>(WebConstant.SpecialModelForItcndObject);
    private const string SUCCESS = "SUCCESSRET";
    
    [WebMethod]
    public ArrayList Query(string family, string model, string type)
    {
        ArrayList ret = new ArrayList();
        IList<string> Maskstrlst = new List<string>();
        IList<string> Modelstrlst = new List<string>();
        try
        {
            var para = iSpecialModelForItcnd.Query(family, model, type);
            ret.Add(SUCCESS);
            //ret.Add( para[0]);
            if (para.Count>0)
            {
                IList<TsModelInfo> tmp2 = (IList<TsModelInfo>)para[0];

                //string dd = tmp[0].editor;

                for (int i = 0; i < tmp2.Count; i++)
                {
                    //IList<TsModelInfo> tmp = new List<TsModelInfo>();
                    Maskstrlst.Add(tmp2[i].mark);
                    Modelstrlst.Add(tmp2[i].model);
                }
                ret.Add(Modelstrlst);
                ret.Add(Maskstrlst);
            }
            return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            return ret;
        }
        catch (Exception e)
        {
            throw e;
        }
        
    }

    [WebMethod]
    public ArrayList Insert(string family, string model, string type, string user)
    {
        ArrayList ret = new ArrayList();
        string customer;
        
        IList<string> Maskstrlst = new List<string>();
        IList<string> Modelstrlst = new List<string>();
        try
        {
            iSpecialModelForItcnd.Insert(family, model, type, user,out customer);
            Maskstrlst.Add(type);
            Modelstrlst.Add(model);
            

            ret.Add(SUCCESS);
            ret.Add(Modelstrlst);
            ret.Add(Maskstrlst);
            ret.Add(customer);
            return ret;
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
            return ret;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    [WebMethod]
    public void Delete(string family, string model, string type)
    {
        ArrayList ret = new ArrayList();
        try
        {
            iSpecialModelForItcnd.Delete(family, model, type);

            ret.Add(SUCCESS);
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
        }
        catch (Exception e)
        {
            throw e;
        }

    }
}

