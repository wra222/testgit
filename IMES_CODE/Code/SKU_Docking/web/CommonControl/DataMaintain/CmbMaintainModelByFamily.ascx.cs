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

public partial class CommonControl_DataMaintain_CmbMaintainModelByFamily : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainModelByFamily;
    private IQCRatio iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string family;

    public string Family
    {
        get { return family; }
        set { family = value; }
    }

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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IQCRatio>(WebConstant.MaintainQCRatioObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainModelByFamily.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainModelByFamily.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainModelByFamily.Width = Unit.Parse(width);
                }

                this.drpMaintainModelByFamily.CssClass = cssClass;
                this.drpMaintainModelByFamily.Enabled = enabled;

                if (enabled)
                {
                    initMaintainModelByFamily();
                }
                else
                {
                    this.drpMaintainModelByFamily.Items.Add(new ListItem("", ""));
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

    public void initMaintainModelByFamily()
    {

        if (iSelectData != null && family != null && family != "")
        {
            IList<SelectInfoDef> lstMaintainModelByFamily = null;

            lstMaintainModelByFamily = iSelectData.GetModelListByFamily(family);

            //Console.Write(lstMaintainModelByFamily.Count);
            if (lstMaintainModelByFamily != null && lstMaintainModelByFamily.Count != 0)
            {
                initControl(lstMaintainModelByFamily);
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
        initMaintainModelByFamily();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainModelByFamily.Items.Clear();
        drpMaintainModelByFamily.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainModelByFamily)
    {
        ListItem item = null;

        this.drpMaintainModelByFamily.Items.Clear();
        this.drpMaintainModelByFamily.Items.Add(string.Empty);

        if (lstMaintainModelByFamily != null)
        {
            foreach (SelectInfoDef temp in lstMaintainModelByFamily)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainModelByFamily.Items.Add(item);
            }

        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainModelByFamily.SelectedIndex = index;
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
            return this.drpMaintainModelByFamily;
        }

    }
}
