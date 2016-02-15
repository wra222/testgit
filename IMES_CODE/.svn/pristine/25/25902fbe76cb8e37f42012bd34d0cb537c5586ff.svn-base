using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;

public partial class CommonControl_CmbDBType : System.Web.UI.UserControl
{
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    private static DBInfo objDbInfo;
    private string defaultSelectDB ;
    private string defaultSelectDBType;
    private static string _onLinesConnString;
    private static string _hisLinesConnString;
    protected void Page_Load(object sender, EventArgs e)
    
    {
    //  Control o=  base.FindControl("hidDBName");

        if (!IsPostBack)
        {
            string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
            defaultSelectDBType = Request["DBType"] != null ? Request["DBType"].ToString().Trim() : "OnlineDB";
            defaultSelectDB = Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;
            objDbInfo = iConfigDB.GetDBInfo();
            IniDBInfo();
         
        }
    }
    private void IniDBInfo()
    {
        objDbInfo = iConfigDB.GetDBInfo();
        ddlDB.Items.Clear();
        string[] dbList = objDbInfo.OnLineDBList;
        foreach (string OnLineDB in dbList)
        {
            ddlDB.Items.Add(new ListItem(OnLineDB, OnLineDB));
        }
      
        SetddlDBTypeSelect();
        SetddlDBSelect();
        ddlDBType.Items[0].Value = "OnlineDB";
        ddlDBType.Items[1].Value = "History";
        _onLinesConnString= objDbInfo.OnLineConnectionString;
        _hisLinesConnString = objDbInfo.HistoryConnectionString;

        //ddlDB.Text = "HPIMES";
    }
    private void SetdefaultSelectDBType() {
        defaultSelectDBType = Request["DBType"] != null ? Request["DBType"].ToString().Trim() : "OnlineDB";
        
    }

    private void SetdefaultSelectDB()
    {
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        defaultSelectDB = Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;
    
    }

    private void SetddlDBSelect()
    {
        if (defaultSelectDB == null)
        { SetdefaultSelectDB(); }
        
       

        int i = 0;
        foreach (ListItem it in ddlDB.Items)
        {
            if (it.Text.ToUpper() == defaultSelectDB.ToUpper())
            { ddlDB.SelectedIndex = i; break; }
            i++;
        }
      //  ddlDB.Text = defaultSelectDB;\
    }
    private void SetddlDBTypeSelect()
    {
          if (defaultSelectDBType == null)
        { SetdefaultSelectDBType(); }

        foreach (ListItem it in ddlDBType.Items)
        {
            if (it.Text.ToUpper() == defaultSelectDBType.ToUpper())
            {
                it.Selected = true;
                break;
            }
        }
        ddlDBType_SelectedIndexChanged(null, null);
    }
   
    public string DefaultSelectDB
    {
        get
        {
            return defaultSelectDB;
        }
        set
        {
            defaultSelectDB = value;
        }
    }
    protected void ddlDBType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlDB.Items.Clear();
    
        string[] dbList;
        if (ddlDBType.SelectedValue == "OnlineDB")
        {
            dbList = objDbInfo.OnLineDBList;
          
        }
        else
        {
            dbList = objDbInfo.HistoryDBList; 
        }
           foreach (string OnLineDB in dbList)
          {
             ddlDB.Items.Add(new ListItem(OnLineDB, OnLineDB));
           }
        //ddlDB.Items.Clear();
        //if (ddlDBType.SelectedIndex == 0)
        //{
        //  //  string OnLineDB = iConfigDB.GetOnlineDB();
        //   // ddlDB.Items.Add(new ListItem(OnLineDB, OnLineDB));
        //    List<string> OnLineDBLst = iConfigDB.GetOnlineDBList();
           
        //    foreach (string OnLineDB in OnLineDBLst)
        //    {
        //        ddlDB.Items.Add(new ListItem(OnLineDB, OnLineDB));
        //    }
         
        //}
        //else
        //{
        //    List<string> ConfigDB = iConfigDB.GetHistoryDBList();

        //    foreach (string HistoryDB in ConfigDB)
        //    {
        //        ddlDB.Items.Add(new ListItem(HistoryDB, HistoryDB));
        //    }
        //}
    }

    public string ddlGetConnection()
    {
        string connString = "";
        string dbName = "";
        if (ddlDB.SelectedValue == "")
        {
            IniDBInfo();
            dbName = ddlDB.SelectedValue;
        }
         else
         {dbName=ddlDB.SelectedValue;}


        if (ddlDBType.SelectedItem.Value == "OnlineDB")
        {
            connString = string.Format(_onLinesConnString, dbName);
            //connString = string.Format(objDbInfo.OnLineConnectionString, dbName);
        
        }
        else
        {
            connString = string.Format(_hisLinesConnString, dbName);
        
        }
        return connString;
  
       
    }
}
