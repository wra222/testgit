﻿<%@ WebService Language="C#" Class="BOMNodeDataService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure;
using IMES.DataModel;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
[System.Web.Script.Services.ScriptService]
public class BOMNodeDataService : System.Web.Services.WebService
{
    IBOMNodeData iModelBOM = (IBOMNodeData)ServiceAgent.getInstance().GetMaintainObjectByName<IBOMNodeData>(WebConstant.IBOMNodeData);

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    public string SaveModelBOMAs(string code, string newCode, string editor, Boolean isNeedCheck)
    {
        //com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
        try
        {
            return iModelBOM.SaveModelBOMAs(code, newCode, editor, isNeedCheck);
        }
        catch (FisException ex)
        {
            //!!! need change
            if (ex.mErrcode == "DMT012")
            {
                return "";
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        
    }

    [WebMethod]
    public string SaveAs(string code, string newCode, string editor, Boolean isNeedCheck)
    {
        try
        {
            return iModelBOM.SaveAs(code, newCode, editor, isNeedCheck);
        }
        catch (FisException ex)
        {
            if (ex.mErrcode == "DMT012")
            {
                return "";
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public string SaveAsDummy(string code, string editor, string Customer, Boolean isNeedCheck)
    {
        try
        {
            return iModelBOM.SaveAsDummy(code, editor, Customer, isNeedCheck);
        }
        catch (FisException ex)
        {
            if (ex.mErrcode == "DMT012")
            {
                return "";
            }
            else
            {
                throw new Exception(ex.mErrmsg);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

