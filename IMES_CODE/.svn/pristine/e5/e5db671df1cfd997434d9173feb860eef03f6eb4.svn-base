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

public partial class CommonControl_DataMaintain_CmbMaintainMBCode : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
   private IPartManager iPart;
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
           iPart = (IPartManager)ServiceAgent.getInstance().GetMaintainObjectByName<IPartManager>(WebConstant.IPartManager);
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainMBCode.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainMBCode.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainMBCode.Width = Unit.Parse(width);
                }

                this.drpMaintainMBCode.CssClass = cssClass;
                this.drpMaintainMBCode.Enabled = enabled;

                if (enabled)
                {
                    initMaintainModel();
                }
                else
                {
                    this.drpMaintainMBCode.Items.Add(new ListItem("", ""));
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

    public void initMaintainModel()
    {

        if (iPart != null)
        {
            IList<string> lstMaintainMBCode = null;

            lstMaintainMBCode = iPart.GetInfoValue("MB");

            if (lstMaintainMBCode != null && lstMaintainMBCode.Count != 0)
            {
                initControl(lstMaintainMBCode);
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
        initMaintainModel();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainMBCode.Items.Clear();
        drpMaintainMBCode.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstMaintainModel)
    {
        ListItem item = null;

        this.drpMaintainMBCode.Items.Clear();
        if (lstMaintainModel != null)
        {
            foreach (string temp in lstMaintainModel)
            {
                item = new ListItem(temp,temp);
                this.drpMaintainMBCode.Items.Add(item);
            }

        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainMBCode.SelectedIndex = index;
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
            return this.drpMaintainMBCode;
        }

    }
}


