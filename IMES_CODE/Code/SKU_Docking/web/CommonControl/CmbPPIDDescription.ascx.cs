/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbPPIDDescription
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-14   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
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
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using IMES.DataModel;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;

public partial class CommonControl_CmbPPIDDescription : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IPPIDDescription iPPIDDescription;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string ppidType;

    public string PPIDType
    {
        get { return ppidType; }
        set { ppidType = value; }
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
            iPPIDDescription = ServiceAgent.getInstance().GetObjectByName<IPPIDDescription>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpPPIDDescription.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpPPIDDescription.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpPPIDDescription.Width = Unit.Parse(width);
                }

                this.drpPPIDDescription.CssClass = cssClass;
                this.drpPPIDDescription.Enabled = enabled;

                if (enabled)
                {
                    initPPIDDescription(ppidType);
                }
                else
                {
                    this.drpPPIDDescription.Items.Add(new ListItem("", ""));
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

    public void initPPIDDescription(string ppidType)
    {
        if (iPPIDDescription != null)
        {
            IList<PPIDDescriptionInfo> lstPPIDDescription = iPPIDDescription.GetPPIDDescriptionList(ppidType);

            if (lstPPIDDescription != null && lstPPIDDescription.Count != 0)
            {
                initControl(lstPPIDDescription);
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

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpPPIDDescription;
        }
    }

    public void refresh(string ppidType)
    {
        initPPIDDescription(ppidType);
        up.Update();
    }

    public void clearContent()
    {
        this.drpPPIDDescription.Items.Clear();
        drpPPIDDescription.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<PPIDDescriptionInfo> lstPPIDDescription)
    {
        ListItem item = null;

        this.drpPPIDDescription.Items.Clear();
        this.drpPPIDDescription.Items.Add(string.Empty);

        if (lstPPIDDescription != null)
        {
            foreach (PPIDDescriptionInfo temp in lstPPIDDescription)
            {
                item = new ListItem(temp.friendlyName, temp.id);

                this.drpPPIDDescription.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpPPIDDescription.SelectedIndex = index;
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
}
