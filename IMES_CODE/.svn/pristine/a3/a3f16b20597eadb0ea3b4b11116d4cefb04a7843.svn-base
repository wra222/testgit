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
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;

public partial class CommonControl_DataMaintain_CmbMaintainFamilyForSMT : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private SMTLineSpeed iSMTLineSpeed;
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
            iSMTLineSpeed = ServiceAgent.getInstance().GetMaintainObjectByName<SMTLineSpeed>(WebConstant.SMTLineSpeed);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpLine.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpLine.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpLine.Width = Unit.Parse(width);
                }

                this.drpLine.CssClass = cssClass;
                this.drpLine.Enabled = enabled;

                if (enabled)
                {
                    initFamily();
                }
                else
                {
                    this.drpLine.Items.Add(new ListItem("", ""));
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
        if (iSMTLineSpeed != null)
        {
            IList<string> lstFamily = null;

            lstFamily = iSMTLineSpeed.GetFamily("");

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
        this.drpLine.Items.Clear();
        drpLine.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstFamily)
    {
        ListItem item = null;

        this.drpLine.Items.Clear();

        if (lstFamily != null)
        {
            foreach (string temp in lstFamily)
            {
                item = new ListItem(temp, temp);

                this.drpLine.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpLine.SelectedIndex = index;
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
            return this.drpLine;
        }

    }
}
