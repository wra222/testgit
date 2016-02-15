<%@ WebService Language="C#" Class="WebServiceChangeKeyParts" %>
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for ChangeKeyParts Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Change Key Parts.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Change Key Parts.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

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
public class WebServiceChangeKeyParts : System.Web.Services.WebService
{
    IChangeKeyParts iChangeKeyParts = ServiceAgent.getInstance().GetObjectByName<IChangeKeyParts>(WebConstant.ChangeKeyPartsObject);
   
    [WebMethod]
    public ArrayList InputPartNo(string prodId, string input)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            MatchedPartOrCheckItem item = iChangeKeyParts.TryPartMatchCheck(prodId, input);
            retLst.Add(WebConstant.SUCCESSRET);
            retLst.Add(input);
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
    public ArrayList ClearCT(string prodId)
    {
        ArrayList retLst = new ArrayList();
        try
        {
            iChangeKeyParts.ClearCT(prodId);
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
    
    [WebMethod]
    public void Cancel(string prodId)
    {
        iChangeKeyParts.Cancel(prodId);
    }
}


