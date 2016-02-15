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

public partial class CommonControl_CmbCustomer : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string customer;
    private ICustomer iCustomer;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

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

    //public string Customer
    //{
    //    get { return customer; }
    //    set { customer = value; }
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
            iCustomer = ServiceAgent.getInstance().GetMaintainObjectByName<ICustomer>(WebConstant.MaintainCommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpCustomer.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpCustomer.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpCustomer.Width = Unit.Parse(width);
                }

                this.drpCustomer.CssClass = cssClass;
                this.drpCustomer.Enabled = enabled;

                if (enabled)
                {
                    initCustomer();
                }
                else
                {
                    this.drpCustomer.Items.Add(new ListItem("", ""));
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

    public void initCustomer()
    {

        if (iCustomer != null)
        {
            IList<CustomerInfo> lstCustomer = null;

            lstCustomer = iCustomer.GetCustomerList();

            Console.Write(lstCustomer.Count);
            if (lstCustomer != null && lstCustomer.Count != 0)
            {
                initControl(lstCustomer);
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
        initCustomer();
        up.Update();
    }

    public void clearContent()
    {
        this.drpCustomer.Items.Clear();
        drpCustomer.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<CustomerInfo> lstCustomer)
    {
        ListItem item = null;

        this.drpCustomer.Items.Clear();
        //this.drpCustomer.Items.Add(string.Empty);


        if (lstCustomer != null)
        {
            foreach (CustomerInfo temp in lstCustomer)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.customer, temp.customer);
                this.drpCustomer.Items.Add(item);
            }
        }

        //item = new ListItem("Add New...", "$$$");
        //this.drpCustomer.Items.Add(item);
    }

    public void setSelected(int index)
    {
        this.drpCustomer.SelectedIndex = index;
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
            return this.drpCustomer;
        }

    }
}

