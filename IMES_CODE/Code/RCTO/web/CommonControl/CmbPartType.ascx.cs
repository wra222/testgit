/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbPartType
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
using System.Text;
using com.inventec.iMESWEB;

public partial class CommonControl_CmbPartType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IDefect iPartType;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string customer;

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

    public string Customer
    {
        get { return customer; }
        set { customer = value; }
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
            //iPartType = ServiceAgent.getInstance().GetMaintainObjectByName<IDefect>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.drpPartType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpPartType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpPartType.Width = Unit.Parse(width);
                }

                this.drpPartType.CssClass = cssClass;
                this.drpPartType.Enabled = enabled;

                if (enabled)
                {
                    this.initPartType();
                }
                else
                {
                    this.drpPartType.Items.Add(new ListItem("", ""));
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

    public void initPartType()
    {
        //if (iPartType != null)
        //{
        //    IList<DefectInfo> lstPartType = getPartType();

        //    if (lstPartType != null && lstPartType.Count != 0)
        //    {
        //        initControl(lstPartType);
        //    }
        //}

        initControl(null);
    }

    public void refresh()
    {
        initPartType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpPartType.Items.Clear();
        drpPartType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<DefectInfo> lstPartType)
    {
        //ListItem item = null;

        this.drpPartType.Items.Clear();
        this.drpPartType.Items.Add(new ListItem("", ""));

        //foreach (DefectInfo temp in lstPartType)
        //{
        //    item = new ListItem(temp.friendlyName, temp.id);

        //    this.drpPartType.Items.Add(item);
        //}
        //Add By itc202007
        this.drpPartType.Items.Add(new ListItem("KP/ME", "KP/ME"));
        this.drpPartType.Items.Add(new ListItem("MB", "MB"));

        //this.drpPartType.Items.Add(new ListItem("KP", "KP"));
        //this.drpPartType.Items.Add(new ListItem("ME", "ME"));
        //this.drpPartType.Items.Add(new ListItem("MB", "MB"));
        //this.drpPartType.Items.Add(new ListItem("Other Type", "Other Type"));
    }

    public void setSelected(int index)
    {
        this.drpPartType.SelectedIndex = index;
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
