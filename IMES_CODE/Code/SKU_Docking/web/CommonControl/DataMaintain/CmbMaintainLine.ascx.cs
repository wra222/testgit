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

public partial class CommonControl_CmbMaintainLine : System.Web.UI.UserControl
{

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string cssClass;
    private string width;
    private List<String[]> selectValues;
    private IPdLineStation iLineStation;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string customer="";
    private string stage="";

    public string Customer
    {
        get { return customer; }
        set { customer = value; }
    }

    public string Stage
    {
        get { return stage; }
        set { stage = value; }
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
            iLineStation = (IPdLineStation)ServiceAgent.getInstance().GetMaintainObjectByName<IWarranty>(WebConstant.IPdLineStation);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainLine.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainLine.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainLine.Width = Unit.Parse(width);
                }

                this.drpMaintainLine.CssClass = cssClass;
                this.drpMaintainLine.Enabled = enabled;

                if (enabled)
                {
                    initMaintainLine();
                }
                else
                {
                    this.drpMaintainLine.Items.Add(new ListItem("", ""));
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

    public void initMaintainLine()
    {
        IList<string> lstMaintainLine = iLineStation.GetLineByCustAndStage(customer, stage);

        if (lstMaintainLine != null && lstMaintainLine.Count != 0)
        {
            initControl(lstMaintainLine);
        }
        else
        {
            initControl(null);
        }
    }

    public void refresh()
    {
        initMaintainLine();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainLine.Items.Clear();
        up.Update();
    }

    private void initControl(IList<string> lstMaintainLine)
    {
        ListItem item = null;

        this.drpMaintainLine.Items.Clear();

        if (lstMaintainLine != null)
        {
            foreach (string temp in lstMaintainLine)
            {
                item = new ListItem(temp, temp);
                this.drpMaintainLine.Items.Add(item);
            }
        }
        else
        {
            this.drpMaintainLine.Items.Add(string.Empty);
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainLine.SelectedIndex = index;
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
            return this.drpMaintainLine;
        }
    }
}

