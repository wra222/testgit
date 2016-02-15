using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using System.Text;

public partial class CommonControl_cmbMaintainByStationType : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainStation;
    private IStation2 iStation;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    //public string Station
    //{
    //    get { return station; }
    //    set { station = value; }
    //}

    //public string MaintainStation
    //{
    //    get { return MaintainStation; }
    //    set { MaintainStation = value; }
    //}

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
            iStation = ServiceAgent.getInstance().GetMaintainObjectByName<IStation2>(WebConstant.MaintainStationObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainStation.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainStation.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainStation.Width = Unit.Parse(width);
                }

                this.drpMaintainStation.CssClass = cssClass;
                this.drpMaintainStation.Enabled = enabled;

                if (enabled)
                {
                    initMaintainStation();
                }
                else
                {
                    this.drpMaintainStation.Items.Add(new ListItem("", ""));
                }
            }
        }
        catch (FisException ex)
        {
            //showCmbErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            //showCmbErrorMessage(ex.Message);
        }
    }

    public void initMaintainStation()
    {

        if (iStation != null)
        {
            IList<StationDef> lstMaintainStation = null;

            lstMaintainStation = iStation.getStationByStationType("FATest");
            if (lstMaintainStation != null && lstMaintainStation.Count != 0)
            {
                initControl(lstMaintainStation);
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
        initMaintainStation();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainStation.Items.Clear();
        drpMaintainStation.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<StationDef> lstMaintainStation)
    {
        ListItem item = null;

        this.drpMaintainStation.Items.Clear();
        //this.drpMaintainStation.Items.Add(string.Empty);

        if (lstMaintainStation != null)
        {
            foreach (StationDef temp in lstMaintainStation)
            {
                item = new ListItem(temp.station + " " + temp.name, temp.station);
                this.drpMaintainStation.Items.Add(item);
            }

        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainStation.SelectedIndex = index;
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
            return this.drpMaintainStation;
        }

    }
}

