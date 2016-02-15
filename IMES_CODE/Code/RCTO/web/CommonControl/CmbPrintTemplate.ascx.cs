/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbPrintTemplate
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-06   Tong.Zhi-Yong     Create 
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

public partial class CommonControl_CmbPrintTemplate : System.Web.UI.UserControl
{
    private string station;
    private string cssClass;
    private string width;
    private string labelType;
    private IPrintTemplate iPrintTemplate;
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

    public string CssClass
    {
        get { return cssClass; }
        set { cssClass = value; }
    }

    public string LabelType
    {
        get { return labelType; }
        set { labelType = value; }
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
                    this.drpPrintTemplate.Width = Unit.Percentage(100);
                }
                else
                {
                    drpPrintTemplate.Width = Unit.Percentage(Convert.ToDouble(width));
                }
            }
            else
            {
                drpPrintTemplate.Width = Unit.Parse(width);
            }

            this.drpPrintTemplate.CssClass = cssClass;
            this.drpPrintTemplate.Enabled = enabled;

            if (enabled)
            {
                this.initPrintTemplate(labelType);
            }
            else
            {
                this.drpPrintTemplate.Items.Add(new ListItem("", ""));
            }
        }
    }

    public void initPrintTemplate(string paraLabelType)
    {
        if (iPrintTemplate != null)
        {
            IList<PrintTemplateInfo> lstPrintTemplate = iPrintTemplate.GetPrintTemplateList(paraLabelType);

            if (lstPrintTemplate != null && lstPrintTemplate.Count != 0)
            {
                initControl(lstPrintTemplate);
            }
        }
    }

    public void refresh()
    {
        initPrintTemplate(labelType);
        up.Update();
    }

    public void refresh(string paraStation, string paraLabelType)
    {
        initPrintTemplate(paraLabelType);
        up.Update();
    }

    public void clearContent()
    {
        this.drpPrintTemplate.Items.Clear();
        drpPrintTemplate.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<PrintTemplateInfo> lstPrintTemplate)
    {
        ListItem item = null;

        this.drpPrintTemplate.Items.Clear();

        foreach (PrintTemplateInfo temp in lstPrintTemplate)
        {
            item = new ListItem(temp.friendlyName, temp.id);

            this.drpPrintTemplate.Items.Add(item);
        }
    }

    public void setSelected(int index)
    {
        this.drpPrintTemplate.SelectedIndex = index;
        up.Update();
    }
}
