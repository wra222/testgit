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
public partial class CommonControl_cmdMaintainSMTDashboardLine : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private List<String[]> selectValues;
    private string connectionString;
    private IQueryCommon iSMTLine=ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    private Boolean isPercentage = false;
    private Boolean enabled = true;


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
                        drpMaintainLine.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainLine.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainLine.Width = Unit.Parse(width);
                }

                this.drpMaintainLine.CssClass = cssClass;
                this.drpMaintainLine.Enabled = enabled;

                if (enabled)
                {
                    initMaintainLine();
                }
                else
                {
                    this.drpMaintainLine.Items.Add(new ListItem("", ""));
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
    public void initMaintainLine()
    {
            if (iSMTLine != null)
            {
                DataTable dt = iSMTLine.GetSMTLine(connectionString);
                IList<PdLineInfo> lstMaintainLine = new List<PdLineInfo>();
                if (dt.Rows.Count == 0)
                { initControl(null); }
                else
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        PdLineInfo pdline = new PdLineInfo();
                        pdline.id = dr["Line"].ToString().Trim();
                        pdline.friendlyName = dr["Descr"].ToString().Trim();
                        lstMaintainLine.Add(pdline);

                    }
                }
                if (lstMaintainLine != null && lstMaintainLine.Count != 0)
                {
                    initControl(lstMaintainLine);
                }
                else
                {
                    initControl(null);
                }
            }
            else
            {
                initControl(null);
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

    public void refresh()
    {
        initMaintainLine();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainLine.Items.Clear();
        up.Update();
    }

    private void initControl(IList<PdLineInfo> lstMaintainLine)
    {
        ListItem item = null;

        this.drpMaintainLine.Items.Clear();

        if (lstMaintainLine != null)
        {
            foreach (PdLineInfo temp in lstMaintainLine)
            {
                item = new ListItem(temp.id + '-' + temp.friendlyName, temp.id);
                this.drpMaintainLine.Items.Add(item);
            }
        }
        else
        {
            this.drpMaintainLine.Items.Add(string.Empty);
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainLine.SelectedIndex = index;
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
            return this.drpMaintainLine;
        }
    }
}
