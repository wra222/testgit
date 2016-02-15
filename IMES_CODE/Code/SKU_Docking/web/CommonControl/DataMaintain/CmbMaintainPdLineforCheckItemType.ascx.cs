/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2010-6-11   itc210001        create
 * 
 * Known issues:Any restrictions about this file 
 *          
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
using System.Collections.Generic;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Maintain.Interface;
using IMES.Infrastructure;
using com.inventec.iMESWEB;

public partial class CommonControl_DataMaintain_CmbMaintainPdLineforCheckItemType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private ICheckItemTypeListMaintain iCheckItemTypeListMaintain;
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

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpMaintainPdLine;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iCheckItemTypeListMaintain  = ServiceAgent.getInstance().GetMaintainObjectByName<ICheckItemTypeListMaintain>(WebConstant.CheckItemTypeListObject);
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(Width) > 100)
                    {
                        drpMaintainPdLine.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainPdLine.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainPdLine.Width = Unit.Parse(width);
                }

                this.drpMaintainPdLine.CssClass = cssClass;
                this.drpMaintainPdLine.Enabled = enabled;

                if (enabled)
                {
                    initMaintainPdLine();
                }
                else
                {
                    this.drpMaintainPdLine.Items.Add(new ListItem("", ""));
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

    public void initMaintainPdLine()
    {
        if (iCheckItemTypeListMaintain != null)
        {
            IList<string> lstMaintainPdLine = null;
            lstMaintainPdLine = iCheckItemTypeListMaintain.GetAllAliasLine();

            if (lstMaintainPdLine != null && lstMaintainPdLine.Count != 0)
            {
                initControl(lstMaintainPdLine);
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
        initMaintainPdLine();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainPdLine.Items.Clear();
        drpMaintainPdLine.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstMaintainPdLine)
    {
        ListItem item = null;
        this.drpMaintainPdLine.Items.Clear();

        this.drpMaintainPdLine.Items.Add(new ListItem("", ""));

        if (lstMaintainPdLine != null)
        {
            foreach (string temp in lstMaintainPdLine)
            {
                item = new ListItem(temp, temp);
                this.drpMaintainPdLine.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainPdLine.SelectedIndex = index;
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
