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

public partial class Query_PAK_SetLineQty : IMESQueryBasePage
{
    public string userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 20;
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);

    private static string dbName;
    private static string DBConnection = "";

   

 
 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
               dbName = Request.QueryString["dbName"];
               GetConnection(); 
                BindTable();
             }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }

    }
    private void GetConnection()
    {
        DBInfo obj = iConfigDB.GetDBInfo();
        DBConnection = string.Format(obj.OnLineConnectionString, dbName);
    }
    #region 数据的增删改查
   
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
       
    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        
    
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {

        try
        {
            PAK_Common.UpdateLineQty(DBConnection, hidLine.Value.ToString(), int.Parse(hidQty.Value.ToString()));
            BindTable();

            this.UpdatePanel1.Update();

            Reset();
        }
        catch (Exception exp)
        {
            showErrorMessage(exp.Message);
        }

     


    }
    #endregion


    #region gridview相关
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
   

    }
    private void Reset()
    {
        CallCluentFunc("setNewItemValue", "setNewItemValue()");
        //setNewItemValue
    }

    private void BindTable()
    {
      
       
        gd.DataSource = PAK_Common.GetLineQty(DBConnection);
        gd.DataBind();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;", true);
    }
    private void CallCluentFunc(string key, string funcion)
    {
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), key, funcion, true);

    }
    

    #endregion


    #region 系统相关
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    #endregion

    
}
