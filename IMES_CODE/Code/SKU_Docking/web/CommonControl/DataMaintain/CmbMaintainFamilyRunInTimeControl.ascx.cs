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
using System.Text;
using com.inventec.iMESWEB;

public partial class CommonControl_CmbMaintainFamilyRunInTimeControl : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    private IRunInTimeControl iSelectData;
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
            iSelectData = (IRunInTimeControl)ServiceAgent.getInstance().GetMaintainObjectByName<IWarranty>(WebConstant.IRunInTimeControl);

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
            //showCmbErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            //showCmbErrorMessage(ex.Message);
        }
    }

    public void initMaintainFamily()
    {

        if (iSelectData != null)
        {
            IList<string> lstMaintainFamily = null;

            lstMaintainFamily = iSelectData.GetFamilyListFromRunInTimeControl();

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

        //第一项是空
        this.drpMaintainFamily.Items.Add(string.Empty);

        if (lstMaintainFamily != null)
        {
            foreach (string temp in lstMaintainFamily)
            {
                item = new ListItem(temp, temp);
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

        ScriptManager.RegisterStartupScript(this.up, typeof(System.Object), "showCmbErrorMessage", scriptBuilder.ToString(), false);
    }

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpMaintainFamily;
        }

    }
}

