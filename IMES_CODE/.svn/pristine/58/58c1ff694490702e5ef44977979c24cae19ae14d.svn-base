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

public partial class CommonControl_cmbMaintainWC : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    private IDefectStation iSelectData;
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IDefectStation>(WebConstant.MaintainDefectStationObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainWC.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainWC.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainWC.Width = Unit.Parse(width);
                }

                this.drpMaintainWC.CssClass = cssClass;
                this.drpMaintainWC.Enabled = enabled;

                if (enabled)
                {
                    initMaintainStation();
                }
                else
                {
                    this.drpMaintainWC.Items.Add(new ListItem("", ""));
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

        if (iSelectData != null)
        {
            IList<StationMaintainInfo> lstMaintainStation = null;

            lstMaintainStation = iSelectData.GetStationList();

            //Console.Write(lstMaintainStation.Count);
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
        this.drpMaintainWC.Items.Clear();
        drpMaintainWC.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<StationMaintainInfo> lstMaintainStation)
    {
        ListItem item = null;

        this.drpMaintainWC.Items.Clear();
        this.drpMaintainWC.Items.Add(string.Empty);
        //Value=@Station，Text=@Station+空格+@Name
        if (lstMaintainStation != null)
        {
            foreach (StationMaintainInfo temp in lstMaintainStation)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Station+" "+temp.Descr, temp.Station);
                this.drpMaintainWC.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainWC.SelectedIndex = index;
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
            return this.drpMaintainWC;
        }

    }
}

