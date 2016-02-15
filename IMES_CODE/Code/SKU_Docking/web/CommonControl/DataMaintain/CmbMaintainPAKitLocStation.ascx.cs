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
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;

public partial class CommonControl_DataMaintain_CmbMaintainPAKitLocStation : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    //private string station;
    //private string customer;
    private IPAKitLoc ipakitloc;
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

    //public string Customer
    //{
    //    get { return customer; }
    //    set { customer = value; }
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
            ipakitloc = ServiceAgent.getInstance().GetMaintainObjectByName<IPAKitLoc>(WebConstant.PAKITLOCMAITAIN);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.ddlStation.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        this.ddlStation.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    this.ddlStation.Width = Unit.Parse(width);
                }

                this.ddlStation.CssClass = cssClass;
                this.ddlStation.Enabled = enabled;

                if (enabled)
                {
                    initCustomer();
                }
                else
                {
                    this.ddlStation.Items.Add(new ListItem("", ""));
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

    public void initCustomer()
    {
        if (ipakitloc != null)
        {
            IList<string> station = null;

            station = ipakitloc.GetAllPAKikittingStationName();

            //Console.Write(lstMaintainStation.Count);
            if (station != null && station.Count != 0)
            {
                initControl(station);
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
        initCustomer();
        up.Update();
    }

    public void clearContent()
    {
        this.ddlStation.Items.Clear();
        ddlStation.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstCustomer)
    {
        ListItem item = null;

        this.ddlStation.Items.Clear();
        //this.drpCustomer.Items.Add(string.Empty);


        if (lstCustomer != null)
        {
            //     item = new ListItem("All", "All");
            //     this.ddlFamily.Items.Add(item);
            foreach (string temp in lstCustomer)
            {

                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp, temp);
                this.ddlStation.Items.Add(item);
            }
        }

        //item = new ListItem("Add New...", "$$$");
        //this.drpCustomer.Items.Add(item);
    }

    public void setSelected(int index)
    {
        this.ddlStation.SelectedIndex = index;
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
            return this.ddlStation;
        }

    }
}
