
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
using IMES.Station.Interface.StationIntf;

public partial class CommonControl_CmbConstValueType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private IOfflinePizzaKittingForRCTO iOfflinePizzaKittingForRCTO;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string stage;
    private string type;
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

    public string Stage
    {
        get { return stage; }
        set { stage = value; }
    }
    public string Type
    {
        get { return type; }
        set { type = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iOfflinePizzaKittingForRCTO = ServiceAgent.getInstance().GetObjectByName<IOfflinePizzaKittingForRCTO>(WebConstant.OfflinePizzaKittingForRCTO);

            if (!this.IsPostBack)
            {
               // drpConstValueType.Attributes.Add("onChange", "return onModelChange()"); 
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpConstValueType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpConstValueType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpConstValueType.Width = Unit.Parse(width);
                }

                this.drpConstValueType.CssClass = cssClass;
                this.drpConstValueType.Enabled = enabled;

                if (enabled)
                {
                    initConstValueType(station, customer, stage);
                }
                else
                {
                    this.drpConstValueType.Items.Add(new ListItem("", ""));
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

    public void initConstValueType(string paraStation, string customer, string stage)
    {
        if (iOfflinePizzaKittingForRCTO != null)
        {
           
            IList<ConstValueTypeInfo> lstConstValueTypeInfo = null;

            lstConstValueTypeInfo = iOfflinePizzaKittingForRCTO.GetConstValueTypeListByType(type);
           

            if (lstConstValueTypeInfo != null && lstConstValueTypeInfo.Count != 0)
            {
                initControl(lstConstValueTypeInfo);
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
        initConstValueType(station, customer, stage);
        up.Update();
    }

    public void refresh(string paraStation, string customer)
    {
        initConstValueType(paraStation, customer, stage);
      //  up.Update();
    }

    public void clearContent()
    {
        this.drpConstValueType.Items.Clear();
        drpConstValueType.Items.Add(new ListItem("", ""));
        //up.Update();
    }

    private void initControl(IList<ConstValueTypeInfo> lstPDLine)
    {
        ListItem item = null;

        this.drpConstValueType.Items.Clear();
        this.drpConstValueType.Items.Add(string.Empty);

        if (lstPDLine != null)
        {
            foreach (ConstValueTypeInfo temp in lstPDLine)
            {
   
                item = new ListItem(temp.value, temp.value);

                this.drpConstValueType.Items.Add(item);
            }
        }
        this.drpConstValueType.SelectedIndex = 0;
    }

    public void setSelected(int index)
    {
        this.drpConstValueType.SelectedIndex = index;
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
            return this.drpConstValueType;
        }

    }
}
