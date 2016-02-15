﻿using System;
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
using com.inventec.iMESWEB;
using System.Text;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;

public partial class CommonControl_cmbMBFamily2 : System.Web.UI.UserControl
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string cssClass;
    private string width;
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

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMBFamily.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMBFamily.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMBFamily.Width = Unit.Parse(width);
                }

                this.drpMBFamily.CssClass = cssClass;
                this.drpMBFamily.Enabled = enabled;

                if (enabled)
                {
                    //initObject();
                }
                else
                {
                    this.drpMBFamily.Items.Add(new ListItem("", ""));
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

    public void initObject(out string value)
    {
        IRCTOMBMaintain mbmaintain;
        mbmaintain = (IRCTOMBMaintain)ServiceAgent.getInstance().GetMaintainObjectByName<IRCTOMBMaintain>(WebConstant.MaintainMBObject);
        ListItem item = null;

        this.drpMBFamily.Items.Clear();
        this.drpMBFamily.Items.Add(string.Empty);

        IList<string> list = new List<string>();
        list = mbmaintain.GetFamilyInfo("MB");
        foreach (string temp in list)
        {
            item = new ListItem(temp, temp);
            this.drpMBFamily.Items.Add(item);
        }
        if (list != null && list.Count > 0)
        {
            this.setSelected(1);
            value = this.drpMBFamily.SelectedValue.Trim();
        }
        else
        {
            value = "";
        }
    }

    public void refresh()
    {
        string value = "";
        initObject(out value);
        up.Update();
    }

    public void clearContent()
    {
        this.drpMBFamily.Items.Clear();
        drpMBFamily.Items.Add(new ListItem("", ""));
        up.Update();
    }


    public void setSelected(int index)
    {
        this.drpMBFamily.SelectedIndex = index;
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
            return this.drpMBFamily;
        }

    }
}
