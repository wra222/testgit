
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
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.DataModel;

public partial class CommonControl_CmbDocType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private IPackingListForControl iPackingListControl;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string _service = string.Empty;

    public String Service
    {
        set{_service = value;}
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
            iPackingListControl = 
                ServiceAgent.getInstance().GetObjectByName<IPackingListForControl>(WebConstant.PackingListObject);
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpDocType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpDocType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpDocType.Width = Unit.Parse(width);
                }

                this.drpDocType.CssClass = cssClass;
                this.drpDocType.Enabled = enabled;

                if (enabled)
                {
                    initDocType();
                }
                else
                {
                    this.drpDocType.Items.Add(new ListItem("", ""));
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

    public void initDocType()
    {
        if (iPackingListControl != null)
        {
            IList<string> lstDocType = null;

            lstDocType = iPackingListControl.GetDocTypeList(_service);
            if (lstDocType != null && lstDocType.Count != 0)
            {
                initControl(lstDocType);
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
        initDocType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpDocType.Items.Clear();
        drpDocType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstDocType)
    {
        ListItem item = null;

        if (_service == "PackingListForPL")
        {
            if (lstDocType != null && lstDocType.Count > 0)
            {
                this.drpDocType.Items.Clear();
                this.drpDocType.Items.Add("ALL");
                foreach (string temp in lstDocType)
                {
                    item = new ListItem(temp);
                    this.drpDocType.Items.Add(item);
                }
            }
            else
            {
                this.drpDocType.Items.Clear();
                this.drpDocType.Items.Add(string.Empty);
            }
        }
        else
        {
            this.drpDocType.Items.Clear();
            this.drpDocType.Items.Add(string.Empty);

            if (lstDocType != null)
            {
                foreach (string temp in lstDocType)
                {
                    item = new ListItem(temp);
                    this.drpDocType.Items.Add(item);
                }
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpDocType.SelectedIndex = index;
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
            return this.drpDocType;
        }
    }
}
