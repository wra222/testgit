/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbLabelKittingCode
* UI:CI-MES12-SPEC-PAK-UI-Combine Carton in DN
* UC:CI-MES12-SPEC-PAK-UC-Combine Carton in DN          
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/09/07    ITC000052             Create   
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


public partial class CommonControl_CmbDeliveryListByCarton : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private string model;
    private string status;
    private IDeliveryByCarton iStation;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private Boolean bNeedStationInText = false;
    private string sortField = "Descr";
    private int deliverynum;

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public string Model
    {
        get { return model; }
        set { model = value; }
    }

    public string Status
    {
        get { return status; }
        set { model = value; }
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

    public int deliveryNum
    {
        get { return deliverynum; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iStation = ServiceAgent.getInstance().GetObjectByName<IDeliveryByCarton>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpDeliverybyCarton.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpDeliverybyCarton.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpDeliverybyCarton.Width = Unit.Parse(width);
                }

                this.drpDeliverybyCarton.CssClass = cssClass;
                this.drpDeliverybyCarton.Enabled = enabled;

                if (enabled)
                {
                    initDeliveryByCarton(model, status);
                }
                else
                {
                    this.drpDeliverybyCarton.Items.Add(new ListItem("", ""));
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

    public void initDeliveryByCarton(string dnModel, string dnStatus)
    {
        if (iStation != null)
        {
            IList<DNForUI> lstDn = null;

            if (!string.IsNullOrEmpty(dnModel))
            {
                lstDn = iStation.GetDeliveryListByCarton(dnModel, dnStatus);
            }

            if (lstDn != null && lstDn.Count != 0)
            {
                initControl(lstDn);
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
        initDeliveryByCarton(model, status);
        up.Update();
    }

    public void refresh(string dnModel, string status)
    {
        initDeliveryByCarton(dnModel, status);
        up.Update();
    }

    public void clearContent()
    {
        this.drpDeliverybyCarton.Items.Clear();
        drpDeliverybyCarton.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<DNForUI> lstDn)
    {
        ListItem item = null;

        this.drpDeliverybyCarton.Items.Clear();
        this.drpDeliverybyCarton.Items.Add(string.Empty);
        deliverynum = 1;

        if (lstDn != null)
        {
            foreach (DNForUI curDev in lstDn)
            {
                string devStr = curDev.DeliveryNo + "_" + curDev.ShipDate.Year.ToString("d4") + "/"
                    + curDev.ShipDate.Month.ToString("d2") + "/" + curDev.ShipDate.Day.ToString("d2") + "_" + curDev.Qty.ToString();

                item = new ListItem(devStr, curDev.DeliveryNo);
                this.drpDeliverybyCarton.Items.Add(item);
                deliverynum++;
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpDeliverybyCarton.SelectedIndex = index;
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
            return this.drpDeliverybyCarton;
        }

    }

}
