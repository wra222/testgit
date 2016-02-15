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
using IMES.DataModel;
using IMES.Infrastructure;
using com.inventec;
using System.Text;
using IMES.Maintain.Interface;
using com.inventec.iMESWEB;
//using IMES.Maintain.Interface.MaintainIntf;


public partial class CommonControl_DataMaintain_CmbMaintainAllTypeFamily : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IAllKindsOfType iFamily;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public string Width
    {
        get 
        {
            return width;
        }
        set
        {
            width = value;
        }
    }

    public string CssClass
    {
        get
        {
            return cssClass;
        }
        set
        {
            cssClass = value;
        }
    }

    public Boolean Enabled
    {
        get
        {
            return enabled;
        }
        set
        {
            enabled = value;
        }
    }

    public Boolean IsPercentage
    {
        get
        {
            return isPercentage;
        }
        set
        {
            isPercentage = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iFamily = (IAllKindsOfType)ServiceAgent.getInstance().GetMaintainObjectByName<IAllKindsOfType>(WebConstant.MaintainAllKindsOfTypeObject);
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainFamily.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainFamily.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainFamily.Width = Unit.Parse(width);
                }
                this.drpMaintainFamily.CssClass = cssClass;
                this.drpMaintainFamily.Enabled = enabled;

                if (enabled)
                {
                    initMaintainFamily();
                }
                else
                {
                    this.drpMaintainFamily.Items.Add(new ListItem("", ""));
                }
            }
        }
        catch (FisException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void initMaintainFamily()
    {
        if (iFamily != null)
        {
            IList<string> lstMaintainFamily = null;
            lstMaintainFamily = iFamily.GetFamilyList();
            if (lstMaintainFamily != null && lstMaintainFamily.Count != 0)
            {
                initControl(lstMaintainFamily);
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
        initMaintainFamily();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainFamily.Items.Clear();
        drpMaintainFamily.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstMaintainFamily)
    {
        ListItem item = null;
        this.drpMaintainFamily.Items.Clear();
        if (lstMaintainFamily != null)
        {
            this.drpMaintainFamily.Items.Add(new ListItem("", ""));
            foreach (string temp in lstMaintainFamily)
            {
                item = new ListItem(temp.Trim().ToString(),temp.Trim().ToString());
                this.drpMaintainFamily.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainFamily.SelectedIndex = index;
        up.Update();
    }

    private void showCmbErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\0013", string.Empty).Replace("\0010", "\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.up,typeof(System.Object),"showCmbErrorMessage",scriptBuilder.ToString(),false);

    }

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpMaintainFamily;
        }
    }
}
