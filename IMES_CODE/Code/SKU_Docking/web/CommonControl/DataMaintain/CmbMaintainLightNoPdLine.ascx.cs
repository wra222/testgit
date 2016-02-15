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

public partial class CommonControl_DataMaintain_CmbMaintainLightNoPdLine : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;

    private ILightNo iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string stage;

    private Boolean isAddEmpty = false;

    public Boolean IsAddEmpty
    {
        get { return isAddEmpty; }
        set { isAddEmpty = value; }
    }

    public string Stage
    {
        get { return stage; }
        set { stage = value; }
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<ILightNo>(WebConstant.MaintainLightNoObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainLightNoPdLine.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainLightNoPdLine.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainLightNoPdLine.Width = Unit.Parse(width);
                }

                this.drpMaintainLightNoPdLine.CssClass = cssClass;
                this.drpMaintainLightNoPdLine.Enabled = enabled;

                if (enabled)
                {
                    initMaintainLightNoPdLine();
                }
                else
                {
                    this.drpMaintainLightNoPdLine.Items.Add(new ListItem("", ""));
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

    public void initMaintainLightNoPdLine()
    {

        if (iSelectData != null && stage != null && stage != "")
        {
            IList<SelectInfoDef> lstMaintainLightNoPdLine = null;

            lstMaintainLightNoPdLine = iSelectData.GetPdLineList(stage);

            //Console.Write(lstMaintainLightNoPdLine.Count);
            if (lstMaintainLightNoPdLine != null && lstMaintainLightNoPdLine.Count != 0)
            {
                initControl(lstMaintainLightNoPdLine);
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
        initMaintainLightNoPdLine();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainLightNoPdLine.Items.Clear();
        drpMaintainLightNoPdLine.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainLightNoPdLine)
    {
        ListItem item = null;

        this.drpMaintainLightNoPdLine.Items.Clear();
        //this.drpMaintainLightNoPdLine.Items.Add(string.Empty);
        if (isAddEmpty == true)
        {
            drpMaintainLightNoPdLine.Items.Add(new ListItem("", ""));
        }

        if (lstMaintainLightNoPdLine != null)
        {
            foreach (SelectInfoDef temp in lstMaintainLightNoPdLine)
            {
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainLightNoPdLine.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainLightNoPdLine.SelectedIndex = index;
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
            return this.drpMaintainLightNoPdLine;
        }

    }
}

