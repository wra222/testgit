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

public partial class CommonControl_cmbMaintainCheckItem : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainCheckItem;
    private ICheckItem iSelectData;
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

    //public string MaintainCheckItem
    //{
    //    get { return MaintainCheckItem; }
    //    set { MaintainCheckItem = value; }
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<ICheckItem>(WebConstant.MaintainCheckItemObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainCheckItem.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainCheckItem.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainCheckItem.Width = Unit.Parse(width);
                }

                this.drpMaintainCheckItem.CssClass = cssClass;
                this.drpMaintainCheckItem.Enabled = enabled;

                if (enabled)
                {
                    initMaintainCheckItem();
                }
                else
                {
                    this.drpMaintainCheckItem.Items.Add(new ListItem("", ""));
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

    public void initMaintainCheckItem()
    {

        if (iSelectData != null && customer != null && customer!="")
        {
            IList<SelectInfoDef> lstMaintainCheckItem = null;

            lstMaintainCheckItem = iSelectData.GetItemNameListByCustomer(customer);

            //Console.Write(lstMaintainCheckItem.Count);
            if (lstMaintainCheckItem != null && lstMaintainCheckItem.Count != 0)
            {
                initControl(lstMaintainCheckItem);
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
        initMaintainCheckItem();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainCheckItem.Items.Clear();
        drpMaintainCheckItem.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainCheckItem)
    {
        ListItem item = null;

        this.drpMaintainCheckItem.Items.Clear();
        this.drpMaintainCheckItem.Items.Add(string.Empty);

        if (lstMaintainCheckItem != null)
        {
            foreach (SelectInfoDef temp in lstMaintainCheckItem)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainCheckItem.Items.Add(item);
            }
        }
        if (customer != null && customer != "")
        {
            item = new ListItem("Add a new Check Item", "AddANewCheckItem");
            this.drpMaintainCheckItem.Items.Add(item);
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainCheckItem.SelectedIndex = index;
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
            return this.drpMaintainCheckItem;
        }

    }
}

