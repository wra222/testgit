/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbFloor
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-12   Tong.Zhi-Yong     Create 
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
using System.Text;
using IMES.Infrastructure;

public partial class CommonControl_CmbLocFloor : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private ILocFloor iFloor;
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
            iFloor = ServiceAgent.getInstance().GetObjectByName<ILocFloor>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.drpFloor.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpFloor.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpFloor.Width = Unit.Parse(width);
                }

                this.drpFloor.CssClass = cssClass;
                this.drpFloor.Enabled = enabled;

                if (enabled)
                {
                    initFloor();
                }
                else
                {
                    this.drpFloor.Items.Add(new ListItem("", ""));
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

    public void initFloor()
    {
        if (iFloor != null)
        {
            IList<string> lstFloor = iFloor.GetLocFloorList();

            if (lstFloor != null && lstFloor.Count != 0)
            {
                initControl(lstFloor);
            }
        }
    }

    public void refresh()
    {
        initFloor();
        up.Update();
    }

    public void clearContent()
    {
        this.drpFloor.Items.Clear();
        drpFloor.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstFloor)
    {
        ListItem item = null;

        this.drpFloor.Items.Clear();
        this.drpFloor.Items.Add(string.Empty);

        foreach (string temp in lstFloor)
        {
            item = new ListItem(temp, temp);

            this.drpFloor.Items.Add(item);
        }
    }

    public void setSelected(int index)
    {
        this.drpFloor.SelectedIndex = index;
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
