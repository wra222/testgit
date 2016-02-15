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

public partial class CommonControl_DataMaintain_CmbMaintainPartCheckSettingValueType : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainPartCheckSettingValueType;
    private IPartCheckSetting iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string customer;

    private string partType;

    public string PartType
    {
        get { return partType; }
        set { partType = value; }
    }

    public string Customer
    {
      get { return customer; }
      set { customer = value; }
    }

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    //public string Station
    //{
    //    get { return station; }
    //    set { station = value; }
    //}

    //public string MaintainPartCheckSettingValueType
    //{
    //    get { return MaintainPartCheckSettingValueType; }
    //    set { MaintainPartCheckSettingValueType = value; }
    //}

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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IPartCheckSetting>(WebConstant.MaintainPartCheckSettingObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainPartCheckSettingValueType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainPartCheckSettingValueType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainPartCheckSettingValueType.Width = Unit.Parse(width);
                }

                this.drpMaintainPartCheckSettingValueType.CssClass = cssClass;
                this.drpMaintainPartCheckSettingValueType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainPartCheckSettingValueType();
                }
                else
                {
                    this.drpMaintainPartCheckSettingValueType.Items.Add(new ListItem("", ""));
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

    public void initMaintainPartCheckSettingValueType()
    {

        if (iSelectData != null && customer != null && customer != "" && partType != null && partType != "")
        {
            IList<SelectInfoDef> lstMaintainPartCheckSettingValueType = null;

            lstMaintainPartCheckSettingValueType = iSelectData.GetValueTypeListByCustomerAndPartType(customer,partType);

            //Console.Write(lstMaintainPartCheckSettingValueType.Count);
            if (lstMaintainPartCheckSettingValueType != null && lstMaintainPartCheckSettingValueType.Count != 0)
            {
                initControl(lstMaintainPartCheckSettingValueType);
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
        initMaintainPartCheckSettingValueType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainPartCheckSettingValueType.Items.Clear();
        drpMaintainPartCheckSettingValueType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainPartCheckSettingValueType)
    {
        ListItem item = null;

        this.drpMaintainPartCheckSettingValueType.Items.Clear();
        //this.drpMaintainPartCheckSettingValueType.Items.Add(string.Empty);

        if (lstMaintainPartCheckSettingValueType != null)
        {
            foreach (SelectInfoDef temp in lstMaintainPartCheckSettingValueType)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainPartCheckSettingValueType.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainPartCheckSettingValueType.SelectedIndex = index;
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
            return this.drpMaintainPartCheckSettingValueType;
        }

    }
}

