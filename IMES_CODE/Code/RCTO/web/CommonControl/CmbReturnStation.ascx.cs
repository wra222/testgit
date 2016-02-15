/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbReturnStation
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-02   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
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
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using System.Collections.Generic;
using IMES.DataModel;

public partial class CommonControl_CmbReturnStation : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private IDefect iDefect;
    private ITestStation iTestStation;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string customer;
    private string type = "ReturnStation";
    private Boolean isFromStation = false;

    public Boolean IsFromStation
    {
        get { return isFromStation; }
        set { isFromStation = value; }
    }

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public string Customer
    {
        get { return customer; }
        set { customer = value; }
    }

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public string Station
    {
        get { return station; }
        set { station = value; }
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
            iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
            iTestStation = ServiceAgent.getInstance().GetObjectByName<ITestStation>(WebConstant.CommonObject);
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.drpReturnStation.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpReturnStation.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpReturnStation.Width = Unit.Parse(width);
                }

                this.drpReturnStation.CssClass = cssClass;
                this.drpReturnStation.Enabled = enabled;

                if (enabled)
                {
                    initReturnStation();
                }
                else
                {
                    this.drpReturnStation.Items.Add(new ListItem("", ""));
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

    public void initReturnStation()
    {
        if (!isFromStation)
        {
            if (iDefect != null)
            {
                if (!string.IsNullOrEmpty(customer))
                {
                    IList<DefectInfo> lstDefect = iDefect.GetDefectInfoByTypeAndCustomer(type, customer);

                    if (lstDefect != null && lstDefect.Count != 0)
                    {
                        initControl(lstDefect);
                    }
                }
                else
                {
                    initControl(new List<DefectInfo>());
                }
            }
        }
        else
        {
            if (iTestStation != null)
            {
                IList<StationInfo> lstStation = iTestStation.GetStationListByType(type);

                if (lstStation != null && lstStation.Count != 0)
                {
                    initControl(lstStation);
                }
                else
                {
                    initControl(new List<StationInfo>());
                }
            }
        }
    }

    public void initStationByPdline(string pdline, string type)
    {
        if (iTestStation != null)
        {
            IList<StationInfo> lstStation = null;

            if (!string.IsNullOrEmpty(type))
            {
                lstStation = iTestStation.GetStationListByLineAndType(pdline, type);
            }

            if (lstStation != null && lstStation.Count != 0)
            {
                initControl(lstStation);
            }
            else
            {
                initControl(new List<StationInfo>());
            }
        }
        else
        {
            initControl(new List<StationInfo>());
        }
    }

    public void refresh()
    {
        initReturnStation();
        up.Update();
    }

    public void refreshByPdline(string pdline, string type)
    {
        initStationByPdline(pdline, type);
        up.Update();
    }


    public void clearContent()
    {
        this.drpReturnStation.Items.Clear();
        drpReturnStation.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<DefectInfo> lstDefect)
    {
        ListItem item = null;

        this.drpReturnStation.Items.Clear();
        drpReturnStation.Items.Add(new ListItem("", ""));

        foreach (DefectInfo temp in lstDefect)
        {
            item = new ListItem(temp.id + ' ' + temp.friendlyName, temp.id);

            this.drpReturnStation.Items.Add(item);
        }
    }

    private void initControl(IList<StationInfo> lstStation)
    {
        ListItem item = null;
        this.drpReturnStation.Enabled = enabled;
        this.drpReturnStation.Items.Clear();
        drpReturnStation.Items.Add(new ListItem("", ""));

        foreach (StationInfo temp in lstStation)
        {
            item = new ListItem(temp.StationId + ' ' + temp.Descr, temp.StationId);

            this.drpReturnStation.Items.Add(item);
        }
    }

    public void setSelected(int index)
    {
        this.drpReturnStation.SelectedIndex = index;
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
            return this.drpReturnStation;
        }

    }
}
