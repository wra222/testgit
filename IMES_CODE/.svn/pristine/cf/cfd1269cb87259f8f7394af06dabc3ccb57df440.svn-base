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

public partial class CommonControl_CmbMaintainPartTypeByCustomer : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainPartTypeByCustomer;
    private IPartCheckSetting iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string customer;

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

    //public string MaintainPartTypeByCustomer
    //{
    //    get { return MaintainPartTypeByCustomer; }
    //    set { MaintainPartTypeByCustomer = value; }
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
                        drpMaintainPartTypeByCustomer.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainPartTypeByCustomer.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainPartTypeByCustomer.Width = Unit.Parse(width);
                }

                this.drpMaintainPartTypeByCustomer.CssClass = cssClass;
                this.drpMaintainPartTypeByCustomer.Enabled = enabled;

                if (enabled)
                {
                    initMaintainPartTypeByCustomer();
                }
                else
                {
                    this.drpMaintainPartTypeByCustomer.Items.Add(new ListItem("", ""));
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

    public void initMaintainPartTypeByCustomer()
    {

        if (iSelectData != null && customer != null && customer!="")
        {
            IList<SelectInfoDef> lstMaintainPartTypeByCustomer = null;

            lstMaintainPartTypeByCustomer = iSelectData.GetCustomerPartTypeList(customer);

            //Console.Write(lstMaintainPartTypeByCustomer.Count);
            if (lstMaintainPartTypeByCustomer != null && lstMaintainPartTypeByCustomer.Count != 0)
            {
                initControl(lstMaintainPartTypeByCustomer);
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
        initMaintainPartTypeByCustomer();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainPartTypeByCustomer.Items.Clear();
        drpMaintainPartTypeByCustomer.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainPartTypeByCustomer)
    {
        ListItem item = null;

        this.drpMaintainPartTypeByCustomer.Items.Clear();
        //this.drpMaintainPartTypeByCustomer.Items.Add(string.Empty);

        if (lstMaintainPartTypeByCustomer != null)
        {
            foreach (SelectInfoDef temp in lstMaintainPartTypeByCustomer)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainPartTypeByCustomer.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainPartTypeByCustomer.SelectedIndex = index;
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
            return this.drpMaintainPartTypeByCustomer;
        }

    }
}

