using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
//using IMES.Station.Interface.CommonIntf;
using System.Data;

public partial class Query_PAK_QueryIndiaMPRLabel : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    static IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
    static string DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        RegisterMPGet(this.Master);
        try
        {
            if (!this.IsPostBack)
            {
                string dbName = "";
                if (Request["DBName"] != null)
                {
                    dbName = Request["DBName"].ToString().Trim();
                }
                CmbDBType.DefaultSelectDB = dbName;
                InitCondition();
         
             
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg,this);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message,this);
        }

    }
  
    private void InitCondition()
    {
    
        string customer = Master.userInfo.Customer;
         InitLabel();
       
      
    }
    private void InitLabel()
    {

        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
   
  
   

    [System.Web.Services.WebMethod]

    public static string GetQueryResult(string input)
    {
       
        DataTable dt = null;
        string result = "";
        dt = PAK_Common.QueryIndiaMPRLabel(DBConnection, input);

        if (dt == null || dt.Rows.Count == 0)
        {
            result = "No Data!";
        }
        else
        {
            //需要貼MPR label Price= 
            result = "需要貼MPR label Price= " + dt.Rows[0][0].ToString();
        }
       return  result ;
    }


}
