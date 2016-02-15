/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbPPIDType
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

public partial class CommonControl_CmbPPIDType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IPPIDType iPPIDType;
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
        if (!this.IsPostBack)
        {
            if (isPercentage)
            {
                if (Convert.ToInt32(width) > 100)
                {
                    drpPPIDType.Width = Unit.Percentage(100);
                }
                else
                {
                    drpPPIDType.Width = Unit.Percentage(Convert.ToDouble(width));
                }
            }
            else
            {
                drpPPIDType.Width = Unit.Parse(width);
            }

            this.drpPPIDType.CssClass = cssClass;
            this.drpPPIDType.Enabled = enabled;

            if (enabled)
            {
                initPPIDType();
            }
            else
            {
                this.drpPPIDType.Items.Add(new ListItem("", ""));
            }
        }
    }

    public void initPPIDType()
    {
        if (iPPIDType != null)
        {
            IList<PPIDTypeInfo> lstPPIDType = iPPIDType.GetPPIDTypeList();

            if (lstPPIDType != null && lstPPIDType.Count != 0)
            {
                initControl(lstPPIDType);
            }
        }
    }

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpPPIDType;
        }
    }

    public void refresh()
    {
        initPPIDType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpPPIDType.Items.Clear();
        drpPPIDType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<PPIDTypeInfo> lstPPIDType)
    {
        ListItem item = null;

        this.drpPPIDType.Items.Clear();

        foreach (PPIDTypeInfo temp in lstPPIDType)
        {
            item = new ListItem(temp.friendlyName, temp.id);

            this.drpPPIDType.Items.Add(item);
        }
    }

    public void setSelected(int index)
    {
        this.drpPPIDType.SelectedIndex = index;
        up.Update();
    }
}
