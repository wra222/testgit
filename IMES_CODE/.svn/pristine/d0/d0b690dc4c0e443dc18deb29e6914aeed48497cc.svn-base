<%@ WebService Language="C#" Class="WebServiceAssemblyVC" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Collections;


//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServiceAssemblyVC : System.Web.Services.WebService
{
    //private IModelManager iModelManager = ServiceAgent.getInstance().GetMaintainObjectByName<IModelManager>(WebConstant.IModelManager);
    private IAssemblyVC iAssemblyVC = ServiceAgent.getInstance().GetMaintainObjectByName<IAssemblyVC>(WebConstant.MaintainAssemblyVCObject);

    [WebMethod]
    public ArrayList GetPartNoList(string VC)
    {
        ArrayList ret = new ArrayList();
        try
        {
            IList<string> list =  iAssemblyVC.GetPartNoList(VC);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(list);
            return ret;
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
}

