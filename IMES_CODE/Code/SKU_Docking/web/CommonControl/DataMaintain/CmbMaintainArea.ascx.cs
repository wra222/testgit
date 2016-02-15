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
using IMES.DataModel;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Maintain.Interface;

public partial class CommonControl_DataMaintain_CmbMaintainArea : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IAllKindsOfType iArea;
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
            iArea = (IAllKindsOfType)ServiceAgent.getInstance().GetMaintainObjectByName<IAllKindsOfType>(WebConstant.MaintainAllKindsOfTypeObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainArea.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainArea.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainArea.Width = Unit.Parse(width);
                }

                this.drpMaintainArea.CssClass = cssClass;
                this.drpMaintainArea.Enabled = enabled;

                if (enabled)
                {
                    initMaintainArea();
                }
                else
                {
                    this.drpMaintainArea.Items.Add(new ListItem("", ""));
                }
            }
        }
        catch (FisException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public void initMaintainArea()
    {
        if (iArea != null)
        {
            IList<AreaDef> lstMaintainArea = null;

            lstMaintainArea = iArea.GetAreaList();

            if (lstMaintainArea != null && lstMaintainArea.Count != 0)
            {
                initControl(lstMaintainArea);
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
        initMaintainArea();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainArea.Items.Clear();
        drpMaintainArea.Items.Add(new ListItem("",""));
        up.Update();
    }

    private void initControl(IList<AreaDef> lstMaintainArea)
    {
        ListItem item = null;

        this.drpMaintainArea.Items.Clear();

        this.drpMaintainArea.Items.Add(new ListItem("",""));

        if (lstMaintainArea != null)
        {
            foreach (AreaDef temp in lstMaintainArea)
            {
                item = new ListItem(temp.Area,temp.Area);
                this.drpMaintainArea.Items.Add(item);
            }
        }
        //item = new ListItem("","");
        //this.drpMaintainArea.Items.Add(item);

        //if (lstMaintainArea != null)
        //{
        //    foreach (SelectInfoDef temp in lstMaintainArea)
        //    {
        //        item = new ListItem(temp.Text);
        //        this.drpMaintainArea.Items.Add(item);
        //    }
        //}
    }

    public void setSelected(int index)
    {
        this.drpMaintainArea.SelectedIndex = index;
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
            return this.drpMaintainArea;
        }
    }

}
