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
public partial class CommonControl_CmdMaintainSMTDashboardStation : System.Web.UI.UserControl
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
                        drpMaintainSMTDashboardStation.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainSMTDashboardStation.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainSMTDashboardStation.Width = Unit.Parse(width);
                }

                this.drpMaintainSMTDashboardStation.CssClass = cssClass;
                this.drpMaintainSMTDashboardStation.Enabled = enabled;

                if (enabled)
                {
                    initSMTStation();
                }
                else
                {
                    this.drpMaintainSMTDashboardStation.Items.Add(new ListItem("0A", "0A"));
                    this.drpMaintainSMTDashboardStation.Items.Add(new ListItem("0B", "0B"));
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
    public void initSMTStation()
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
                    smtInfoByLine.refreshTime = Convert.ToInt32(dr["RefreshTime"]);
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
        initSMTStation();
        up.Update();
    }
    public void setAutoPostBack(bool isAutoPostBack)
    {
        this.drpMaintainSMTDashboardStation.AutoPostBack = isAutoPostBack;
    }
    public void clearContent()
    {
        this.drpMaintainSMTDashboardStation.Items.Clear();

        this.drpMaintainSMTDashboardStation.Items.Add(new ListItem("0A", "0A"));
        this.drpMaintainSMTDashboardStation.Items.Add(new ListItem("0B", "0B"));
        up.Update();
    }

    private void initControl(IList<SMTDashBoardLineInfo> smtInfoList)
    {
        ListItem item = null;

        this.drpMaintainSMTDashboardStation.Items.Clear();


        if (smtInfoList != null)
        {
            foreach (SMTDashBoardLineInfo temp in smtInfoList)
            {
                item = new ListItem(temp.station, temp.station);

                this.drpMaintainSMTDashboardStation.Items.Add(item);
                if (temp.station == "0A")
                {
                    this.drpMaintainSMTDashboardStation.Items.Add(new ListItem("0B", "0B"));
                }
                else if (temp.station == "0B")
                {
                    this.drpMaintainSMTDashboardStation.Items.Add(new ListItem("0A", "0A"));
                }
            }
        }
        else
        {
            this.drpMaintainSMTDashboardStation.Items.Add(new ListItem("0A", "0A"));
            this.drpMaintainSMTDashboardStation.Items.Add(new ListItem("0B", "0B"));
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainSMTDashboardStation.SelectedIndex = index;
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
            return this.drpMaintainSMTDashboardStation;
        }

    }
}
