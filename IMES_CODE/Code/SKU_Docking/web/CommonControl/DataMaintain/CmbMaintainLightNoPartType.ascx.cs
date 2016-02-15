using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
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

public partial class CommonControl_DataMaintain_CmbMaintainLightNoPartType : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    private ILightNo iSelectData;
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<ILightNo>(WebConstant.MaintainLightNoObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainLightNoPartType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainLightNoPartType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainLightNoPartType.Width = Unit.Parse(width);
                }

                this.drpMaintainLightNoPartType.CssClass = cssClass;
                this.drpMaintainLightNoPartType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainLightNoPartType();
                }
                else
                {
                    this.drpMaintainLightNoPartType.Items.Add(new ListItem("", ""));
                }
            }
        }
        catch (FisException ex)
        {
            //showCmbErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            //showCmbErrorMessage(ex.Message);
        }
    }

    public void initMaintainLightNoPartType()
    {

        if (iSelectData != null )
        {
            IList<SelectInfoDef> lstMaintainLightNoPartType = null;

            lstMaintainLightNoPartType = iSelectData.GetLightNoPartType();

            //Console.Write(lstMaintainLightNoPartType.Count);
            if (lstMaintainLightNoPartType != null && lstMaintainLightNoPartType.Count != 0)
            {
                initControl(lstMaintainLightNoPartType);
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
        initMaintainLightNoPartType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainLightNoPartType.Items.Clear();
        drpMaintainLightNoPartType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainLightNoPartType)
    {
        ListItem item = null;

        this.drpMaintainLightNoPartType.Items.Clear();
        //this.drpMaintainLightNoPartType.Items.Add(string.Empty);

        //drpMaintainLightNoPartType.Items.Add(new ListItem("", ""));
        if (lstMaintainLightNoPartType != null)
        {
            foreach (SelectInfoDef temp in lstMaintainLightNoPartType)
            {
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainLightNoPartType.Items.Add(item);
            }

        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainLightNoPartType.SelectedIndex = index;
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
            return this.drpMaintainLightNoPartType;
        }

    }
}