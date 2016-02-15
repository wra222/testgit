/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbLabelKittingCode
* UI:CI-MES12-SPEC-PAK-UI Pizza Kitting.docx –2011/1/6 
* UC:CI-MES12-SPEC-PAK-UC Pizza Kitting.docx –2011/1/6            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-1-6   Du.Xuan               Create   
* ITC-1360-0931 排序顺序与sql有差异，沿用sql的
* Known issues:
* TODO：
* 
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
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.DataModel;


public partial class CommonControl_CmbStationByType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private string stationtype;
    private IStationByType iStation;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private Boolean bNeedStationInText = false;
    private string sortField = "Descr";

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public string StationType
    {
        get { return stationtype; }
        set { stationtype = value; }
    }

    public Boolean BNeedStationInText
    {
        get { return bNeedStationInText; }
        set { bNeedStationInText = value; }
    }

    public string SortField
    {
        get { return sortField; }
        set { sortField = value; }
    }

    public string Station
    {
        get { return station; }
        set { station = value; }
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

    public Boolean IsPercentage
    {
        get { return isPercentage; }
        set { isPercentage = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iStation = ServiceAgent.getInstance().GetObjectByName<IStationByType>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpStation.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpStation.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpStation.Width = Unit.Parse(width);
                }

                this.drpStation.CssClass = cssClass;
                this.drpStation.Enabled = enabled;

                if (enabled)
                {
                    initStationByType(stationtype);
                }
                else
                {
                    this.drpStation.Items.Add(new ListItem("", ""));
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

    public void initStationByType(string type)
    {
        if (iStation != null)
        {
            IList<StationInfo> lstStation = null;
 
            if (!string.IsNullOrEmpty(type))
            {
                lstStation = iStation.GetStationByTypeList(type);
            }

            if (lstStation != null && lstStation.Count != 0)
            {
                initControl(lstStation);
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

    public void refresh()
    {
        initStationByType(stationtype);
        up.Update();
    }

    public void refresh(string ctype)
    {
        initStationByType(ctype);
        up.Update();
    }

    public void clearContent()
    {
        this.drpStation.Items.Clear();
        drpStation.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<StationInfo> lstStation)
    {
        ListItem item = null;

        this.drpStation.Items.Clear();
        this.drpStation.Items.Add(string.Empty);

        if (lstStation != null)
        {
            IEnumerable<StationInfo> newList;
            if (sortField == "Station")
            {
                newList = lstStation.OrderBy(StationInfo => StationInfo.StationId);
            }
            else
            {
                newList = lstStation;//.OrderBy(StationInfo => StationInfo.Descr);
            }
            foreach (StationInfo temp in newList)
            {
                if (bNeedStationInText)
                {
                    item = new ListItem(temp.StationId + " " + temp.Descr, temp.StationId);
                }
                else
                {
                    item = new ListItem(temp.Descr, temp.StationId);
                }
                this.drpStation.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpStation.SelectedIndex = index;
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
            return this.drpStation;
        }

    }
}
