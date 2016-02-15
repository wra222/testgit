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

public partial class CommonControl_CmbMaintainVendor :  System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    private IVendorCode iVendorCode;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public string Width
    {
        get { return width; }
        set { width = value; }
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
            iVendorCode = ServiceAgent.getInstance().GetMaintainObjectByName<IVendorCode>(WebConstant.MaintainVendorCodeObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpVendor.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpVendor.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpVendor.Width = Unit.Parse(width);
                }

                this.drpVendor.CssClass = cssClass;
                this.drpVendor.Enabled = enabled;

                if (enabled)
                {
                    initMaintainVendor();
                }
                else
                {
                    this.drpVendor.Items.Add(new ListItem("", ""));
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

    public void initMaintainVendor()
    {

        if (iVendorCode != null)
        {
            IList<string> lstMaintainVendor = null;

            lstMaintainVendor = iVendorCode.GetVendorFromIqcPnoBom();

            //Console.Write(lstMaintainPartType.Count);
            if (lstMaintainVendor != null && lstMaintainVendor.Count != 0)
            {
                initControl(lstMaintainVendor);
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
        initMaintainVendor();
        up.Update();
    }

    public void clearContent()
    {
        this.drpVendor.Items.Clear();
        drpVendor.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstMaintainVendor)
    {
        ListItem item = null;
        this.drpVendor.Items.Clear();
        this.drpVendor.Items.Add(" ");

        if (lstMaintainVendor != null)
        {
            foreach (string temp in lstMaintainVendor)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp, temp);
                this.drpVendor.Items.Add(item);
            }
        }        
    }

    public void setSelected(int index)
    {
        this.drpVendor.SelectedIndex = index;
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
            return this.drpVendor;
        }

    }
}