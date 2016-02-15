
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
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;

public partial class CommonControl_CmbException : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    ITravelCardPrint2012 iTravelCardPrint;
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
            iTravelCardPrint = ServiceAgent.getInstance().GetObjectByName<ITravelCardPrint2012>(WebConstant.TravelCardPrint2012Object);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpException.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpException.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpException.Width = Unit.Parse(width);
                }

                this.drpException.CssClass = cssClass;
                this.drpException.Enabled = enabled;

                if (enabled)
                {
                    initException();
                }
                else
                {
                    this.drpException.Items.Add(new ListItem("", ""));
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

    public void initException()
    {
        if (iTravelCardPrint != null)
        {
            IList<ConstValueInfo> list = null;

            list = iTravelCardPrint.GetExList();
            
            if (list != null && list.Count != 0)
            {
                initControl(list);
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
        initException();
        up.Update();
    }

    public void refresh(string paraStation, string customer)
    {
        initException();
        up.Update();
    }

    public void clearContent()
    {
        this.drpException.Items.Clear();
        drpException.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<ConstValueInfo> lst)
    {
        ListItem item = null;

        this.drpException.Items.Clear();
        this.drpException.Items.Add(string.Empty);

        if (lst != null)
        {
            foreach (ConstValueInfo temp in lst)
            {
               
                //item = new ListItem(temp.id.ToString() + " " + temp.value, temp.id.ToString());
                item = new ListItem(temp.value);
                //ITC-1360-1255
                this.drpException.Items.Add(new ListItem(temp.name + " " + temp.value, temp.value));
            }
        }
        //MYTEST
        //this.drpException.Items.Add(new ListItem("exp1"));
        //this.drpException.Items.Add(new ListItem("exp2"));
        //this.drpException.Items.Add(new ListItem("exp3"));
    }

    public void setSelected(int index)
    {
        this.drpException.SelectedIndex = index;
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
            return this.drpException;
        }
    }
}
