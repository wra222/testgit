<%--
/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: ModelMaintainService
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2010-2-05   liu xiaoling          Create   
qu bug no:ITC-1136-0111, ITC-1136-0112           
 * Known issues:
 */
 --%>
<%@ WebService Language="C#" Class="ModelMaintainService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ModelMaintainService : System.Web.Services.WebService
{
    private IModelManager iModelManager = ServiceAgent.getInstance().GetMaintainObjectByName<IModelManager>(WebConstant.IModelManager);


    [WebMethod]
    public string GetFamilyInfoByModel(string queryFirstModelName)
    {
        try
        {

            return iModelManager.GetFamilyNameByModel(queryFirstModelName);

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
    public string GetFirstMatchedModelNameByModel(string strFamilyName, string querySecondModelName)
    {
        string strRtn = "";
        try
        {
            System.Collections.Generic.IList<ModelMaintainInfo> tmpModelList = new System.Collections.Generic.List<ModelMaintainInfo>();

            tmpModelList = iModelManager.GetModelListByModel(strFamilyName, querySecondModelName);

            foreach (ModelMaintainInfo model in tmpModelList) 
            {
                strRtn = model.Model;
                break;
            }
            return strRtn;
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

    public void addOrSaveModelInfo(string name, string value, string desc, string id, string modelname)
    {
        //try
        //{
        //    ModelInfoMaintainInfo modelinfo = new ModelInfoMaintainInfo();

        //    modelinfo.Name = name;
        //    modelinfo.Value = value;
        //    modelinfo.Descr = desc;
        //    modelinfo.Id = Int32.Parse(id);
        //    modelinfo.Editor = UserInfo.UserId;

        //    if (id.Equals(""))
        //    {
        //        iModelManager.AddModelInfo(modelname, modelinfo);
                
        //    } else {

        //        iModelManager.SaveModelInfo(modelname, modelinfo);
                
        //    }

        //}
        //catch (FisException ex)
        //{
        //    throw new Exception(ex.mErrmsg);
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }

}

