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

public partial class Query_PAK_CheckWeightStation : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    static IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
    static string DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        CmbDBType.Visible = false;
      //  CmbDBType.DefaultSelectDB = "HPDocking";
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

        //lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        //lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
   
  
   

    [System.Web.Services.WebMethod]

    public static string[] GetQueryResult(string input)
    {
       
        DataTable dt = null;
        dt = PAK_Common.CheckWeightStation(DBConnection, input);
        List<string> lst = new List<string>();
        string[] result;
        if (dt.Rows.Count == 1)
        {
            result = new string[] { dt.Rows[0][0].ToString() };
        }
        else
        {
            //msgWeight MO MOQty  PassQty Model
            foreach(DataRow dr in dt.Rows)
            {
                lst.Add(dr["value"].ToString()  );
            }
            result = lst.ToArray();
        }
       return  result ;
    }


}
