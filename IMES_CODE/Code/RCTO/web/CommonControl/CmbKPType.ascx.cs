/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbKPType
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-09   Tong.Zhi-Yong     Create 
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
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using System.Text;

public partial class CommonControl_CmbKPType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IKPType iKPType;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

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
            iKPType = ServiceAgent.getInstance().GetObjectByName<IKPType>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.drpKPType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpKPType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpKPType.Width = Unit.Parse(width);
                }

                this.drpKPType.CssClass = cssClass;
                this.drpKPType.Enabled = enabled;

                if (enabled)
                {
                    this.initKPType();
                }
                else
                {
                    this.drpKPType.Items.Add(new ListItem("", ""));
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

    public void initKPType()
    {
        if (iKPType != null)
        {
            IList<KPTypeInfo> lstKPType = iKPType.GetKPTypeList();

            if (lstKPType != null && lstKPType.Count != 0)
            {
                initControl(lstKPType);
            }
        }
    }

    public void refresh()
    {
        initKPType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpKPType.Items.Clear();
        drpKPType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<KPTypeInfo> lstKPType)
    {
        ListItem item = null;

        this.drpKPType.Items.Clear();
        drpKPType.Items.Add(new ListItem("", ""));

        foreach (KPTypeInfo temp in lstKPType)
        {
            item = new ListItem(temp.friendlyName, temp.id);

            this.drpKPType.Items.Add(item);
        }
    }

    public void setSelected(int index)
    {
        this.drpKPType.SelectedIndex = index;
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
