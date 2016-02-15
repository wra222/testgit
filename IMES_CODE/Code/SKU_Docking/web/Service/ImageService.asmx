<%--
 INVENTEC corporation (c)2009 all rights reserved. 
 Description: Image Service
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2009-12-20  Chen Xu (eB1-4)      Create 
 
 Known issues:
 --%>
 
<%@ WebService Language="C#" Class="ImageService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.inventec.template;
using com.inventec.template.structure;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using com.inventec.iMESWEB;
using log4net;
using com.inventec.imes.Model;
using System.Collections;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using com.inventec.imes.BLL;


//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class ImageService: System.Web.Services.WebService
{
    ILog logger = log4net.LogManager.GetLogger("iMesLog");
         
    [WebMethod]
 //   public ImageObject getPrintImgInfoBySinglePara(String templateName, String paramValue, Boolean isPrintRoom)
    public ArrayList getImageListForPrint(printItemEntityForPrint curPrintItem, Boolean isPrintRoom, int i, string curDPI)
    {
       
        string templateName = string.Empty;
        string labeltypeName = string.Empty;

        try
        {
            ArrayList retValue = new ArrayList();

            if ((curDPI == "") || (curDPI == "-1"))
            {
                logger.Error("Printer Information is null or empty! curPrintItem:" + curPrintItem.TemplateName);
            }

            else
            {
                templateName = curPrintItem.TemplateName;
                templateName = "TempalteNameTest1";    //add for testing


                LabelManager imageService = ServiceAgent.getInstance().getPrintTemplateObject(isPrintRoom);

                IList<ImageInfo> printList = new List<ImageInfo>();

                if (curPrintItem.Keys.Count > 0)
                {
                    List<ParamInfo> paramValueLst = new List<ParamInfo>();
                    ParamInfo para;

                    foreach (Dictionary<String, Object> item in curPrintItem.Keys)
                    {
                        para = new ParamInfo();

                        if (item.ContainsKey("ParamName"))
                        {
                            //  para.ParamName = item.this("ParmaName");
                        }

                        if (item.ContainsKey("Values"))
                        {
                            Object objarr = new Object();

                            //if (item.Item("Values") != null)
                            //{
                            //    para.Values = new List<string>();
                            //    // objarr = DirectCast(item.TemplateName("Values"), System.Array);
                            //    foreach (Object temp in objarr)
                            //    {
                            //        para.Values.Add(temp);
                            //    }
                            //}

                        }

                        paramValueLst.Add(para);
                    }

                    printList = imageService.getImageListForPrint(templateName, null, paramValueLst, isPrintRoom, curDPI);

                }

                //else
                //{
                //    string paramValueEmpty = string.Empty;
                //    printList = imageService.getImageListForPrintBySinglePara(templateName, null, paramValueEmpty, isPrintRoom, curDPI);

                //}

                retValue.Add(WebConstant.SUCCESSRET);
                retValue.Add(templateName);

                retValue.Add(printList);
                retValue.Add(i);
                retValue.Add(curPrintItem.Piece);

                return retValue;

            }
        }
        catch (FisException ex)
        {
            logger.Error("getImgInfo exceptin: " + ex.mErrmsg + "  curPrintItem:" + templateName);
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            logger.Error("getImgInfo exceptin: " + ex.Message + "  curPrintItem:" + templateName);
            throw ex;
        }
        finally {
            
        }
return new ArrayList();
    }

    
    
}




  