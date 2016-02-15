<%--
/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-03   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1. 检查Pallet 上的所有SKU
 *                2. 列印Ship to Pallet Label；
 *                3. 内销要额外列印一张Pallet CPMO Label
 */
--%>
<%@ WebService Language="C#" Class="WebServicePalletVerify" %>


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
using System.Data;
using System.Linq;
using IMES.Docking.Interface.DockingIntf;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebServicePalletVerify  : System.Web.Services.WebService 
{
   // private IPalletVerifyDataForDocking iPalletVerify = 
    //        ServiceAgent.getInstance().GetObjectByName<IPalletVerifyDataForDocking>(WebConstant.PalletVerifyDataForDockingObject);

    private IPalletVerifyDataForDocking iPalletVerify;
    private Object PalletVerify;
    private string PalletVerifyObjectBllName = WebConstant.PalletVerifyDataForDockingObject;

    public class DNModel
    {
        public string DN { get; set; }
        public string Model { get; set; }
    }
    
    [WebMethod]
    public ArrayList inputFirstCustSN(string firstCustSn, string line, string editor, string station, string customer)
    {
        ArrayList ret = new ArrayList();
        int index = 0;
        string labeltypeBranch =string.Empty;
        
        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyDataForDocking>(PalletVerifyObjectBllName);
            iPalletVerify = (IPalletVerifyDataForDocking)PalletVerify;
            ArrayList retLst = new ArrayList();
            retLst =iPalletVerify.InputFirstCustSn(firstCustSn, line, editor, station, customer,out index, out labeltypeBranch);
            ret.Add(retLst);
            ret.Add(index); 
            ret.Add(labeltypeBranch);
            // for edits
            //internalID distinct
            //-----------------------------------------------------------------------
            IList<string> distinctDNlist = new List<string>();
            IList<string> distinctModellist = new List<string>();
            string strdistinctID = "";
            IList<DNModel> DeliveryModelList = new List<DNModel>();
            DNModel deliveryModelItem = new DNModel();
            IList<string> DeliveryPerPalletList = (IList<string>)retLst[10];
            IList<string> ModelPerpalletList = (IList<string>)retLst[13];
            for (var i = 0; i < DeliveryPerPalletList.Count; i++)
            {
                deliveryModelItem = new DNModel();
                deliveryModelItem.DN = DeliveryPerPalletList[i];
                deliveryModelItem.Model = ModelPerpalletList[i];
                DeliveryModelList.Add(deliveryModelItem);
            }
            IList<DNModel> orderbyDN = (from u in DeliveryModelList orderby u.DN select u).ToList();

            foreach (DNModel temp in orderbyDN)
            {
                if (temp.DN.Substring(0, 10) != strdistinctID)
                {
                    strdistinctID = temp.DN.Substring(0, 10);
                    distinctDNlist.Add(temp.DN);
                    distinctModellist.Add(temp.Model);
                }
            }
            ret.Add(distinctDNlist);
            ret.Add(distinctModellist);
            //-------------------------------------------------------------------------
            
            return ret;
        }
        catch (FisException ex)
        {
            if (ex.mErrcode == "CHK021") ///tmp
            {
                ret.Add(ex.mErrcode);
                ret.Add(ex.mErrmsg);
                return ret;
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
    public ArrayList inputCustSN(string firstCustSn, string custSn)
    {
        ArrayList ret = new ArrayList();

        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyDataForDocking>(PalletVerifyObjectBllName);
            iPalletVerify = (IPalletVerifyDataForDocking)PalletVerify;

            ArrayList retLst = new ArrayList();
            retLst = iPalletVerify.InputCustSn(firstCustSn, custSn);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            return ret;
        }
        catch (FisException ex)
        {
          //  throw new Exception(ex.mErrmsg);

            if (ex.mErrcode == "PAK144" || ex.mErrcode == "CHK880" || ex.mErrcode == "CHK881" || ex.mErrcode == "SFC002")
            {
                ret.Add(ex.mErrcode);
                ret.Add(ex.mErrmsg);
                return ret;
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
    public ArrayList save(string firstCustSn, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
       // ArrayList printparam1 = new ArrayList();
       // ArrayList printparam2 = new ArrayList();
        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyDataForDocking>(PalletVerifyObjectBllName);
            iPalletVerify = (IPalletVerifyDataForDocking)PalletVerify;
            ArrayList retLst = new ArrayList();
            retLst=iPalletVerify.Save(firstCustSn, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
           
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

    [WebMethod]
    public string Cancel(string uutSn)
    {
        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyDataForDocking>(PalletVerifyObjectBllName);
            iPalletVerify = (IPalletVerifyDataForDocking)PalletVerify;

            iPalletVerify.Cancel(uutSn);
            return (WebConstant.SUCCESSRET);
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
    public ArrayList callTemplateCheckLaNew(string dn, string docType)
    {
        ArrayList ret = new ArrayList();
        DataTable dt = new DataTable();
        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyDataForDocking>(PalletVerifyObjectBllName);
            iPalletVerify = (IPalletVerifyDataForDocking)PalletVerify;

            dt = iPalletVerify.call_Op_TemplateCheckLaNew(dn, docType);
            ret.Add(WebConstant.SUCCESSRET);
            int a = 0;
            int rowsCount = dt.Rows.Count;
            int colsCount = dt.Columns.Count;
            string[,] arrTmp = new string[rowsCount, colsCount];
            foreach (System.Data.DataRow row in dt.Rows)
            {
                int b = 0;
                foreach (System.Data.DataColumn column in dt.Columns)
                {
                    arrTmp[a, b] = row[column.ColumnName].ToString();
                    b = b + 1;
                }
                a = a + 1;
            }
            ret.Add(arrTmp);
            ret.Add(rowsCount);
            ret.Add(colsCount);
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
    
    
    [WebMethod]
    public ArrayList reprint(string palletNo, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();
	    ArrayList printparam1 = new ArrayList();
        ArrayList printparam2 = new ArrayList();
        string labeltypeBranch =string.Empty;
        IList<string> PDFLst = new List<string>();
        IList<string> ModelLst = new List<string>();
        string strdistinctID = "";
        try
        {
            PalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerifyDataForDocking>(PalletVerifyObjectBllName);
            iPalletVerify = (IPalletVerifyDataForDocking)PalletVerify;
            ArrayList retLst = new ArrayList();
            retLst = iPalletVerify.rePrint(palletNo, reason, line, editor, station, customer, printItems, out printparam1, out labeltypeBranch, out PDFLst, out ModelLst);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
	       
            //-----------------------------------------------------------------
            //internalID distinct
            IList<DNModel> DeliveryModelList = new List<DNModel>();
            DNModel deliveryModelItem = new DNModel();
            IList<string> ModelPerpalletList = (IList<string>)ModelLst;
            //IList<string> DeliveryPerPalletList = (IList<string>)printparam1;
            IList<string> distinctDNlist = new List<string>();
            IList<string> distinctModellist = new List<string>();
            for (var i = 0; i < printparam1.Count; i++)
            {
                deliveryModelItem = new DNModel();
                //deliveryModelItem.DN = DeliveryPerPalletList[i];
                deliveryModelItem.DN = printparam1[i].ToString();
                deliveryModelItem.Model = ModelPerpalletList[i];
                DeliveryModelList.Add(deliveryModelItem);
            }
            IList<DNModel> orderbyDN = (from u in DeliveryModelList orderby u.DN select u).ToList();
            foreach (DNModel temp in orderbyDN)
            {
                if (temp.DN.Substring(0, 10) != strdistinctID)
                {
                    strdistinctID = temp.DN.Substring(0, 10);
                    distinctDNlist.Add(temp.DN);
                    distinctModellist.Add(temp.Model);
                }
            }
            ret.Add(distinctDNlist);
            //-----------------------------------------------------------------
            ret.Add(labeltypeBranch);
            ret.Add(PDFLst);
            //ret.Add(ModelLst);
            ret.Add(distinctModellist);

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

