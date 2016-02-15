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
public partial class CommonControl_CmdMaintainSMTDashboardRefreshTime : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string smtLine;
    private string connectionString;
    //private IPdLine iPDLine;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
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
    public string SmtLine
    {
        get { return smtLine; }
        set { smtLine = value; }
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
            if (string.IsNullOrEmpty(connectionString))
            {
                SetConnectionString();
            }

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainSMTDashboardRefreshTime.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainSMTDashboardRefreshTime.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainSMTDashboardRefreshTime.Width = Unit.Parse(width);
                }

                this.drpMaintainSMTDashboardRefreshTime.CssClass = cssClass;
                this.drpMaintainSMTDashboardRefreshTime.Enabled = enabled;

                if (enabled)
                {
                    initRefreshTime();
                }
                else
                {
                    this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("5", "5"));
                    this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("10", "10"));
                    this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("15", "15"));
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
    public void initRefreshTime()
    {
        if (QueryCommon != null)
        {

            if (string.IsNullOrEmpty(smtLine))
            { return; }
            //    IList<PdLineInfo> lstPDLine = null;
            IList<SMTDashBoardLineInfo> smtInfoList = new List<SMTDashBoardLineInfo>();
            //Add for Query using...by Benson


            DataTable dt = QueryCommon.GetSMTRefrshTimeAndStationByLine(ConnectionString, smtLine);
            if (dt.Rows.Count == 0)
            { initControl(null); }
            else
            {
                
                    foreach (DataRow dr in dt.Rows)
                    {
                        SMTDashBoardLineInfo smtInfoByLine = new SMTDashBoardLineInfo();
                        smtInfoByLine.id = dr["Line"].ToString().Trim();
                        smtInfoByLine.refreshTime =Convert.ToInt32(dr["RefreshTime"]);
                        smtInfoByLine.station = dr["Station"].ToString().Trim();
                        smtInfoList.Add(smtInfoByLine);
                    }


                    initControl(smtInfoList);
            }
        }
        else
        {
            initControl(null);
        }
    }

    public void refresh()
    {
        initRefreshTime();
        up.Update();
    }
    public void setAutoPostBack(bool isAutoPostBack)
    {
        this.drpMaintainSMTDashboardRefreshTime.AutoPostBack = isAutoPostBack;
    }
    public void clearContent()
    {
        this.drpMaintainSMTDashboardRefreshTime.Items.Clear();

        this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("5", "5"));
        this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("10", "10"));
        this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("15", "15"));
        up.Update();
    }

    private void initControl(IList<SMTDashBoardLineInfo> smtInfoList)
    {
        ListItem item = null;

        this.drpMaintainSMTDashboardRefreshTime.Items.Clear();


        if (smtInfoList != null)
        {
            foreach (SMTDashBoardLineInfo temp in smtInfoList)
            {
                item = new ListItem(temp.refreshTime.ToString(), temp.refreshTime.ToString());

                this.drpMaintainSMTDashboardRefreshTime.Items.Add(item);
                if (temp.refreshTime == 5)
                {
                    this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("10", "10"));
                    this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("15", "15"));
                }
                else if (temp.refreshTime ==10)
                {
                    this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("5", "5"));
                    this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("15", "15"));
                }
                else if (temp.refreshTime == 15)
                {
                    this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("5", "5"));
                    this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("10", "10"));
                }
            }
        }
        else
        {
            this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("5", "5"));
            this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("10", "10"));
            this.drpMaintainSMTDashboardRefreshTime.Items.Add(new ListItem("15", "15"));
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainSMTDashboardRefreshTime.SelectedIndex = index;
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
            return this.drpMaintainSMTDashboardRefreshTime;
        }

    }
}
