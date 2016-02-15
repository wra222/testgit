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


public partial class CommonControl_CmbDeliveryListByModel : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private string model;
    private string status;
    private IDeliveryByModel iStation;
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
            iStation = ServiceAgent.getInstance().GetObjectByName<IDeliveryByModel>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpDelivery.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpDelivery.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpDelivery.Width = Unit.Parse(width);
                }

                this.drpDelivery.CssClass = cssClass;
                this.drpDelivery.Enabled = enabled;

                if (enabled)
                {
                    initDeliveryByModel(model,status);
                }
                else
                {
                    this.drpDelivery.Items.Add(new ListItem("", ""));
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

    public void initDeliveryByModel(string dnModel,string dnStatus)
    {
        if (iStation != null)
        {
            IList<DNForUI> lstDn = null;

            if (!string.IsNullOrEmpty(dnModel))
            {
                lstDn = iStation.GetDeliveryListByModel(dnModel,dnStatus);
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
        initDeliveryByModel(model,status);
        up.Update();
    }

    public void refresh(string dnModel,string status)
    {
        initDeliveryByModel(dnModel,status);
        up.Update();
    }

    public void clearContent()
    {
        this.drpDelivery.Items.Clear();
        drpDelivery.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<DNForUI> lstDn)
    {
        ListItem item = null;

        this.drpDelivery.Items.Clear();
        this.drpDelivery.Items.Add(" ");
        deliverynum = 1;

        if (lstDn != null)
        {
            foreach (DNForUI curDev in lstDn)
            {
                string Shiptype = "";
                if (curDev.ShipWay == "T001")
                {
                    Shiptype = "A";
                }
                else if (curDev.ShipWay == "T002")
                {
                    Shiptype = "O";
                }
                else
                {
                    Shiptype = curDev.ShipWay;
                }
                    
                string devStr = curDev.DeliveryNo + "_" + curDev.ShipDate.Year.ToString("d4") + "/"
                    + curDev.ShipDate.Month.ToString("d2") + "/" + curDev.ShipDate.Day.ToString("d2") + "_" + curDev.Qty.ToString() + "_" + Shiptype;

                item = new ListItem(devStr, curDev.DeliveryNo);
                this.drpDelivery.Items.Add(item);
                deliverynum ++;
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpDelivery.SelectedIndex = index;
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
            return this.drpDelivery;
        }

    }

}
