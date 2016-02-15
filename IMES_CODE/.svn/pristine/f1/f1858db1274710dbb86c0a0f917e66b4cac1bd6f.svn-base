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

public partial class Query_PAK_SetMailList : IMESQueryBasePage
{
    public string userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 20;
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
  
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);

    private static string dbName;
    private static string reportName;
    private static string DBConnection = "";

   
    private string partNo;

    private const int COL_NUM = 6;
 
 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
            
              
         
                dbName = Request.QueryString["dbName"];
                reportName = Request.QueryString["reportName"];
                GetConnection();
            
                bindTable();
         
              //  initLabel();
             //   bindTable();
      
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
    private void BindTable()
    { 
    
    
    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(txtInfoValue.Text.Trim()))
        {
           showErrorMessage("Please input mail user name!");
            return;
        }

        string mailLst ;

        if (hidMailList.Value == "")
        { mailLst = txtInfoValue.Text.Trim().Replace(";",""); }
        else
        { mailLst = hidMailList.Value + ";" + txtInfoValue.Text.Trim().Replace(";", ""); }

   
        QueryCommon.UpdateSysSetting(reportName, mailLst, DBConnection);
        bindTable();
  
        this.UpdatePanel1.Update();
        Reset();
    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        string[] arr= hidMailList.Value.Split(';');
        List<string> lstMail = new List<string>();
        foreach (string s in arr)
        {
            if (s == hidSelectMail.Value)
            { continue; }
            else
            { lstMail.Add(s); }
        }

        string[] arr2 = lstMail.ToArray();
        string mailLst = string.Join(";", arr2);
    
        QueryCommon.UpdateSysSetting(reportName, mailLst, DBConnection);
        bindTable();

        this.UpdatePanel1.Update();
        Reset();
    
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {

        try
        {
            string[] arr = hidMailList.Value.Split(';');
            List<string> lstMail = new List<string>();
            foreach (string s in arr)
            {
                if (s == hidSelectMail.Value)
                {
                    lstMail.Add(txtInfoValue.Text.Trim().Replace(";", ""));
                    continue;
                }
                else
                { lstMail.Add(s); }
            }


            string[] arr2 = lstMail.ToArray();
            string mailLst = string.Join(";", arr2);

            QueryCommon.UpdateSysSetting(reportName, mailLst, DBConnection);
            bindTable();

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
        //e.Row.Cells[5].Style["display"] = "none";
        //for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        //{
        //    e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        //}

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    for (int i = 0; i < COL_NUM; i++)
        //    {
        //        if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
        //        {
        //            e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
        //        }
        //    }
         
        //}

    }
    private void Reset()
    {
        CallCluentFunc("setNewItemValue", "setNewItemValue()");
        //setNewItemValue
    }

    private void bindTable()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("Mail To");
        List<string> list = new List<string> { "87ReportMailList" };
        DataTable dt2= GetSysSetting2(list, dbName);
        List<string> listResult = new List<string>();
        string[] arrResult = null;
        if (dt2.Rows.Count > 0)
        {
            arrResult = dt2.Rows[0]["Value"].ToString().Split(';');
            hidMailList.Value = dt2.Rows[0]["Value"].ToString();
        }
    

        if (arrResult!=null && arrResult.Length > 0)
        {
            
            foreach (string t in arrResult)
            {
                dr = dt.NewRow();
                dr[0] = t;
                dt.Rows.Add(t);
            }

            for (int i = arrResult.Length; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

     
        }
        else
        {
            for (int i = 0; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            
        }
        //gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        //setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;", true);
    }
    private void CallCluentFunc(string key, string funcion)
    {
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), key, funcion, true);

    }
    

    private void initLabel()
    {
        //this.lblPartNo.Text = this.GetLocalResourceObject(Pre + "_lblPartNoText").ToString();
        //this.lblNodeType.Text = this.GetLocalResourceObject(Pre + "_lblNodeTypeText").ToString();
        //this.lblAttributeList.Text = this.GetLocalResourceObject(Pre + "_lblAttributeListText").ToString();
        //this.lblItemName.Text = this.GetLocalResourceObject(Pre + "_lblItemNameText").ToString();
        //this.lblNodeType.Text = this.GetLocalResourceObject(Pre + "_lblPartNodeTypeText").ToString();
        //this.btnClose.Value = this.GetLocalResourceObject(Pre + "_btnClose").ToString();
        //this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(22);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(22);
       // gd.HeaderRow.Cells[5].Width = Unit.Percentage(22);

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
