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
using com.inventec.iMESWEB;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;

public partial class CommonControl_DataMaintain_CmbGradeHPFamily : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    //private string station;
    //private string customer;
    private IGrade iGrade;
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
            iGrade = ServiceAgent.getInstance().GetMaintainObjectByName<IGrade>(WebConstant.GradeMAITAIN);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.ddlFamily.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        this.ddlFamily.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    this.ddlFamily.Width = Unit.Parse(width);
                }

                this.ddlFamily.CssClass = cssClass;
                this.ddlFamily.Enabled = enabled;

                if (enabled)
                {
                    initCustomer();
                }
                else
                {
                    this.ddlFamily.Items.Add(new ListItem("", ""));
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

        if (iGrade != null)
        {
            IList<string> lstGrade = null;

            lstGrade = iGrade.GetHPFamilys();

            Console.Write(lstGrade.Count);
            if (lstGrade != null && lstGrade.Count != 0)
            {
                initControl(lstGrade);
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
        this.ddlFamily.Items.Clear();
        ddlFamily.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstCustomer)
    {
        ListItem item = null;

        this.ddlFamily.Items.Clear();
        //this.drpCustomer.Items.Add(string.Empty);


        if (lstCustomer != null)
        {
            //     item = new ListItem("All", "All");
            //     this.ddlFamily.Items.Add(item);
            foreach (string temp in lstCustomer)
            {

                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp, temp);
                this.ddlFamily.Items.Add(item);
            }
        }

        //item = new ListItem("Add New...", "$$$");
        //this.drpCustomer.Items.Add(item);
    }

    public void setSelected(int index)
    {
        this.ddlFamily.SelectedIndex = index;
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
            return this.ddlFamily;
        }

    }
}
