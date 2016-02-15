
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using log4net;
using System.Text;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Station.Interface.BSamIntf;
using IMES.Station.Interface.StationIntf;
//using IMES.FisObject;
//using IMES.FisObject.PAK.Carton;
using System.IO;
public partial class BSam_UnpackDNByCarton : System.Web.UI.Page
{  
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private static String userId;
    public static String customer;
    public static String station;
    protected void Page_Load(object sender, EventArgs e)
    {
       
    
        if (!this.IsPostBack)
        {
            customer = Master.userInfo.Customer;
            userId = Master.userInfo.UserId;
            station = Request["Station"] ?? "";
            InitLabel();
       }
    }
    private void InitLabel()
    {
       this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbDataEntry").ToString();
    }
      

    [System.Web.Services.WebMethod]
    public static  void Unpack(string sn)
    {
        try
        {
            IUnpackDNByCarton iUnpackDNByCarton = ServiceAgent.getInstance().GetObjectByName<IUnpackDNByCarton>(WebConstant.IUnpackDNByCarton);
            iUnpackDNByCarton.Unpack(sn, "", userId, station, customer);
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
    [System.Web.Services.WebMethod]
    public static ArrayList CheckDN(string sn)
    {

        try
        {
            IUnpackDNByCarton iUnpackDNByCarton = ServiceAgent.getInstance().GetObjectByName<IUnpackDNByCarton>(WebConstant.IUnpackDNByCarton);
            ArrayList rArray = iUnpackDNByCarton.GetDNListByInput(sn);
            List<string> dnList = (List<string>)rArray[1];
            string dn = "";
            if (dnList.Count > 0)
            {
                dn = string.Join(",", dnList.ToArray());
            }
            ArrayList rstArr = new ArrayList();
            rstArr.Add(rArray[0].ToString());
            rstArr.Add(dn);
            return rstArr;

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
