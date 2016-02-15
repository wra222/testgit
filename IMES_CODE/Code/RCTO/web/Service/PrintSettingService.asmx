<%--
 INVENTEC corporation (c)2009 all rights reserved. 
 Description: Print Service
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-02-01  Lucy Liu      Create 
 
 Known issues:
 --%>
 

<%@ WebService Language="C#" Class="PrintSettingService" %>


using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections;
using com.inventec.iMESWEB;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using log4net;
using com.inventec.template;
using com.inventec.template.structure;
using com.inventec.imes.BLL;
using IMES.DataModel;

//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class PrintSettingService : System.Web.Services.WebService 
{

    IPrintTemplate iPrintTemplate = ServiceAgent.getInstance().GetObjectByName<IPrintTemplate>(WebConstant.CommonObject);
    
   
    [WebMethod]
    public ArrayList GetPrintInfo(string wc)
    
    {
        ArrayList ret = new ArrayList();
        ArrayList tempList = new ArrayList();
        
        try
        {
            IList<string> labelTypeList = iPrintTemplate.GetPrintLabelTypeList(wc);
            if (labelTypeList != null && labelTypeList.Count != 0)
            {


                foreach (string temp in labelTypeList)
                {
                    PrintInfo printInfo = new PrintInfo();
                    printInfo.labelType = temp;
                    printInfo.templateList = iPrintTemplate.GetPrintTemplateList(temp);
                    tempList.Add(printInfo);
                }
            }

            //for (int i = 0; i < 6; i++)
            //{
            //    PrintInfo printInfo = new PrintInfo();
            //    printInfo.labelType = "c" + i;
            //    IList<PrintTemplateInfo> aaa = new List<PrintTemplateInfo>();
            //    PrintTemplateInfo printTemplateInfo = new PrintTemplateInfo();
            //    printTemplateInfo.id = "1" + printInfo.labelType;
            //    printTemplateInfo.friendlyName = "1template" + printInfo.labelType;
            //    aaa.Add(printTemplateInfo);

            //    printTemplateInfo = new PrintTemplateInfo();
            //    printTemplateInfo.id = "2" + printInfo.labelType;
            //    printTemplateInfo.friendlyName = "2template" + printInfo.labelType;
            //    aaa.Add(printTemplateInfo);


            //    printInfo.templateList = aaa;
            //    tempList.Add(printInfo);
            //}
          
            
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(tempList);
            
        }
            
        catch (FisException ex)
        {
            ret.Add(ex.mErrmsg );
        }

        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
            

    }



    public class PrintInfo
    {
        public string labelType;
        public IList<PrintTemplateInfo> templateList;
    }
}


