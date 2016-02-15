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

public partial class CommonControl_CmbMaintainFamily : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainFamily;
    private IQCRatio iSelectData;
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

    //public string MaintainFamily
    //{
    //    get { return MaintainFamily; }
    //    set { MaintainFamily = value; }
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
            iSelectData = (IQCRatio)ServiceAgent.getInstance().GetMaintainObjectByName<IWarranty>(WebConstant.MaintainQCRatioObject);

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
            showCmbErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showCmbErrorMessage(ex.Message);
        }
    }

    public void initMaintainFamily()
    {

        if (iSelectData != null && customer!=null)
        {
            IList<SelectInfoDef> lstMaintainFamily = null;

            lstMaintainFamily = iSelectData.GetCustomerFamilyList(customer);

            //Console.Write(lstMaintainFamily.Count);
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

    private void initControl(IList<SelectInfoDef> lstMaintainFamily)
    {
        ListItem item = null;

        this.drpMaintainFamily.Items.Clear();

        //第一项是空
        item = new ListItem("", "");
        this.drpMaintainFamily.Items.Add(item);

        if (lstMaintainFamily != null)
        {
            foreach (SelectInfoDef temp in lstMaintainFamily)
            {
                item = new ListItem(temp.Text, temp.Value);
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

