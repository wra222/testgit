using System;
using System.Collections;
using System.Configuration;
using System.Data;
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

public partial class CommonControl_DataMaintain_CmbMaintainMBTestCode : System.Web.UI.UserControl
{
    private string cssClass;

    private string width;
    private IPartManager ipart;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string family;
    private string systemType;
    public string Family
    {
        get { return family; }
        set { family = value; }
    }
    public Boolean Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    public string CssClass
    {
        get { return cssClass; }
        set { cssClass = value; }
    }
    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public Boolean IsPercentage
    {
        get { return isPercentage; }
        set { isPercentage = value; }
    }

    public string SystemType
    {
        get { return systemType; }
        set { systemType = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
         

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMBTestCode.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMBTestCode.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMBTestCode.Width = Unit.Parse(width);
                }

                this.drpMBTestCode.CssClass = cssClass;
                this.drpMBTestCode.Enabled = enabled;

                if (enabled)
                {
                    initCode();
                }
                else
                {
                    this.drpMBTestCode.Items.Add(new ListItem("", ""));
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

    public void initCode()
    {
        ipart = ServiceAgent.getInstance().GetMaintainObjectByName<IPartManager>(WebConstant.IPartManager);
        this.drpMBTestCode.Items.Clear();

        IList<string> lstCode = new List<string>();
        if (systemType != null && systemType == "MTDK004")
        {
            lstCode = ipart.GetPartInfoValueByPartDescr(family, "MB", "", "");
        }
        else
        {
            lstCode = ipart.GetPartInfoValueByPartDescr(family, "MB", "MAC", "T");
        }
            if (lstCode != null && lstCode.Count > 0)
            {
               initControl(lstCode);
            }
       
    }

    public void refresh()
    {
        initCode();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMBTestCode.Items.Clear();
        this.drpMBTestCode.Items.Add(new ListItem("", ""));
        up.Update();
    }


    public void setSelected(int index)
    {
        this.drpMBTestCode.SelectedIndex = index;
        up.Update();
    }
    public void initControl(IList<string> lstCode)
    {
        this.drpMBTestCode.Items.Clear();
        if (lstCode != null)
        {
            foreach (string tmp in lstCode)
            {
                this.drpMBTestCode.Items.Add(new ListItem(tmp, tmp));
            }
        }

    }

    public void showCmbErrorMessage(string errorMsg)
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
            return this.drpMBTestCode;
        }

    }
}
