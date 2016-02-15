<%--
/*
 * INVENTEC corporation ?2011 all rights reserved. 
 
*/
--%>
 
<%@ WebService Language="C#" Class="WebServiceModelWeight" %>


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
public class WebServiceModelWeight : System.Web.Services.WebService
{
    IModelWeight iModelWeight = ServiceAgent.getInstance().GetObjectByName<IModelWeight>(WebConstant.ModelWeightObject);

    [WebMethod]
    public IList<string> GetPrIDListInHoldByModel(string model)
    {
            
        try
        {
            IList<string> lst= iModelWeight.GetPrIDListInHoldByModel(model);
            return lst;
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
    public void  SaveModelWeightItemAndHold(ModelWeightDef item, IList<string> prdIdList,string defect)
    {
        ArrayList ret = new ArrayList();

        try
        {
            iModelWeight.SaveModelWeightItemAndHold(item, prdIdList, "Hold", defect);
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
