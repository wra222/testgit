/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbPdLine
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-05   Tong.Zhi-Yong     Create 
 * 2010-01-12   Tong.Zhi-Yong     Modify ITC-1103-0001
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections;
using System.Collections.Generic;
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
//using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
//using IMES.DataModel;
using IMES.Query.Interface.QueryIntf;
public partial class CommonControl_CmbPdLine : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private string stage;
    private string connectionString;
    //private IPdLine iPDLine;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private Boolean isWithoutShift = false;
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
   
    public string Width
    {
        get { return width; }
        set { width = value; }
    }
    public string ConnectionString
    {
        get { return connectionString; }
        set { connectionString = value; }
    }
    public string Station
    {
        get { return station; }
        set { station = value; }
    }
    public string Stage
    {
        get { return stage; }
        set { stage = value; }
    }
    public string Customer
    {
        get { return customer; }
        set { customer = value; }
    }

    public string CssClass
    {
        get { return cssClass; }
        set { cssClass = value; }
    }

    public Boolean Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }
    public Boolean IsWithoutShift
    {
        get { return isWithoutShift; }
        set { isWithoutShift = value; }
    }
    public Boolean IsPercentage
    {
        get { return isPercentage; }
        set { isPercentage = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //iPDLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.CommonObject);
            if(string.IsNullOrEmpty(connectionString))
            {
            SetConnectionString();
            }

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpPDLine.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpPDLine.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpPDLine.Width = Unit.Parse(width);
                }
               
                this.drpPDLine.CssClass = cssClass;
                this.drpPDLine.Enabled = enabled;

                if (enabled)
                {
                    initPDLine(station, customer);
                }
                else
                {
                    this.drpPDLine.Items.Add(new ListItem("", ""));
                }
            }
        }
        catch (FisException ex)
        {
            showCmbErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showCmbErrorMessage(ex.Message);
        }
    }
    private void SetConnectionString()
    {
        DBInfo objDbInfo = iConfigDB.GetDBInfo();
      //  string[] dbList = objDbInfo.OnLineDBList;
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        string defaultSelectDB = Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;
        connectionString = string.Format(objDbInfo.OnLineConnectionString, defaultSelectDB);
     
    
    }
    public void initPDLine(string paraStation, string customer)
    {
        if (QueryCommon != null)
        {

            if (string.IsNullOrEmpty(stage))
            { return; }
        //    IList<PdLineInfo> lstPDLine = null;
            IList<PdLineInfo> lstPDLine = new List<PdLineInfo>();
            //Add for Query using...by Benson
            string[] listLine = stage.Split(',');
            List<string> Process = new List<string>();
            foreach (string s in listLine)
            {
                Process.Add(s);
            }

            DataTable dt = QueryCommon.GetLine(Process, customer, isWithoutShift, connectionString);
             if (dt.Rows.Count == 0)
             { initControl(null); ;}
             else
             {
                 if (isWithoutShift)
                 {
                     foreach (DataRow dr in dt.Rows)
                     {
                         PdLineInfo pdline = new PdLineInfo();
                         pdline.id = dr["Line"].ToString().Trim();
                    //     pdline.friendlyName = dr["Line"].ToString().Trim();
                         lstPDLine.Add(pdline);

                     }
                 }
                 else
                 {
                     foreach (DataRow dr in dt.Rows)
                     {
                         PdLineInfo pdline = new PdLineInfo();
                         pdline.id = dr["Line"].ToString().Trim();
                         pdline.friendlyName = dr["Descr"].ToString().Trim();
                         lstPDLine.Add(pdline);

                     }
                 }
              
                 initControl(lstPDLine);
             }
             //return;
            //Add for Query using...by Benson



        //    if (!string.IsNullOrEmpty(customer))
        //    {
        //        if (!string.IsNullOrEmpty(paraStation))
        //        {
        //            lstPDLine = iPDLine.GetPdLineList(paraStation, customer);
        //        }
        //        else
        //        {
        //            lstPDLine = iPDLine.GetPdLineList(customer);
        //        }
        //    }

        //    if (lstPDLine != null && lstPDLine.Count != 0)
        //    {
        //        initControl(lstPDLine);
        //    }
        //    else
        //    {
        //        initControl(null);
        //    }
        }
        else
        {
            initControl(null);
        }
    }

    public void refresh()
    {
        initPDLine(station, customer);
        up.Update();
    }

    public void refresh(string paraStation, string customer)
    {
        initPDLine(paraStation, customer);
        up.Update();
    }
    public void setAutoPostBack(bool isAutoPostBack)
    {
        this.drpPDLine.AutoPostBack = isAutoPostBack;
    }
    public void clearContent()
    {
        this.drpPDLine.Items.Clear();
        drpPDLine.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<PdLineInfo> lstPDLine)
    {
        ListItem item = null;

        this.drpPDLine.Items.Clear();
        this.drpPDLine.Items.Add(string.Empty);
       
        if (lstPDLine != null)
        {
            foreach (PdLineInfo temp in lstPDLine)
            {
                //ITC-1103-0001 Tong.Zhi-Yong 2010-01-12
                item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);

                this.drpPDLine.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpPDLine.SelectedIndex = index;
        up.Update();
    }

    private void showCmbErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\0013", string.Empty).Replace("\0010", "\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.up, typeof(System.Object), "showCmbErrorMessage", scriptBuilder.ToString(), false);
    }

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpPDLine;
        }

    }
}
