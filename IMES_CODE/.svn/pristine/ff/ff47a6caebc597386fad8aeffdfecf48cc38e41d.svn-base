/*
 *issue
 *
 * ITC-1361-0035  itc210012 2011-01-17
 * 
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
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Collections.Generic;
using System.Text;

public partial class CommonControl_CmbMaintainMasterLabelFamily : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private IECRVersion iECRVersion;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

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
            iECRVersion = ServiceAgent.getInstance().GetMaintainObjectByName<IECRVersion>(WebConstant.IECRVersion);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpFamily.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpFamily.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpFamily.Width = Unit.Parse(width);
                }

                this.drpFamily.CssClass = cssClass;
                this.drpFamily.Enabled = enabled;

                if (enabled)
                {
                    initFamily();
                }
                else
                {
                    this.drpFamily.Items.Add(new ListItem("", ""));
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

    public void initFamily()
    {
        if (iECRVersion != null)
        {
            IList<FamilyInfo> lstFamily = null;

            lstFamily = iECRVersion.GetFamilyInfoListForECRVersion();

            if (lstFamily != null && lstFamily.Count != 0)
            {
                initControl(lstFamily);
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
        initFamily();
        up.Update();
    }

    public void clearContent()
    {
        this.drpFamily.Items.Clear();
        drpFamily.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<FamilyInfo> lstFamily)
    {
        ListItem item = null;

        this.drpFamily.Items.Clear();
        this.drpFamily.Items.Add(string.Empty);

        if (lstFamily != null)
        {
            foreach (FamilyInfo temp in lstFamily)
            {
                item = new ListItem(temp.friendlyName, temp.id);
                item.Attributes.Add("title", temp.friendlyName);
                this.drpFamily.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpFamily.SelectedIndex = index;
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
            return this.drpFamily;
        }

    }

}
