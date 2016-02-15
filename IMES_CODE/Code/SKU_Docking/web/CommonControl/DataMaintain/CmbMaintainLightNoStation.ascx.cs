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

public partial class CommonControl_DataMaintain_CmbMaintainLightNoStation : System.Web.UI.UserControl
{
    public const string TYPE_PAKKITTING = "PAKKitting";

    private string cssClass;
    private string width;

    private ILightNo iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string type = TYPE_PAKKITTING;

    public string Type
    {
        get { return type; }
        set { type = value; }
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
                        drpMaintainLightNoStation.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainLightNoStation.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainLightNoStation.Width = Unit.Parse(width);
                }

                this.drpMaintainLightNoStation.CssClass = cssClass;
                this.drpMaintainLightNoStation.Enabled = enabled;

                if (enabled)
                {
                    initMaintainLightNoStation();
                }
                else
                {
                    this.drpMaintainLightNoStation.Items.Add(new ListItem("", ""));
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

    public void initMaintainLightNoStation()
    {

        if (iSelectData != null && type != null && type != "")
        {
            IList<SelectInfoDef> lstMaintainLightNoStation = null;

            lstMaintainLightNoStation = iSelectData.GetLightNoStationList(type);

            //Console.Write(lstMaintainLightNoStation.Count);
            if (lstMaintainLightNoStation != null && lstMaintainLightNoStation.Count != 0)
            {
                initControl(lstMaintainLightNoStation);
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
        initMaintainLightNoStation();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainLightNoStation.Items.Clear();
        drpMaintainLightNoStation.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainLightNoStation)
    {
        ListItem item = null;

        this.drpMaintainLightNoStation.Items.Clear();
        //this.drpMaintainLightNoStation.Items.Add(string.Empty);
        drpMaintainLightNoStation.Items.Add(new ListItem("", ""));

        if (lstMaintainLightNoStation != null)
        {
            foreach (SelectInfoDef temp in lstMaintainLightNoStation)
            {
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainLightNoStation.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainLightNoStation.SelectedIndex = index;
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
            return this.drpMaintainLightNoStation;
        }

    }
}

