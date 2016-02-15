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

public partial class CommonControl_DataMaintain_CmbMaintainPrintInfoBuild : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainPrintInfoBuild;
    private IPilotRunPrintInfo iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    //public string Station
    //{
    //    get { return station; }
    //    set { station = value; }
    //}

    //public string MaintainPrintInfoBuild
    //{
    //    get { return MaintainPrintInfoBuild; }
    //    set { MaintainPrintInfoBuild = value; }
    //}

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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IPilotRunPrintInfo>(WebConstant.MaintainPilotRunPrintInfoObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainPrintInfoBuild.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainPrintInfoBuild.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainPrintInfoBuild.Width = Unit.Parse(width);
                }

                this.drpMaintainPrintInfoBuild.CssClass = cssClass;
                this.drpMaintainPrintInfoBuild.Enabled = enabled;

                if (enabled)
                {
                    initMaintainPrintInfoBuild();
                }
                else
                {
                    this.drpMaintainPrintInfoBuild.Items.Add(new ListItem("", ""));
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

    public void initMaintainPrintInfoBuild()
    {

        if (iSelectData != null )
        {
            IList<SelectInfoDef> lstMaintainPrintInfoBuild = null;

            lstMaintainPrintInfoBuild = iSelectData.GetBuildList();

            //Console.Write(lstMaintainPrintInfoBuild.Count);
            if (lstMaintainPrintInfoBuild != null && lstMaintainPrintInfoBuild.Count != 0)
            {
                initControl(lstMaintainPrintInfoBuild);
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
        initMaintainPrintInfoBuild();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainPrintInfoBuild.Items.Clear();
        drpMaintainPrintInfoBuild.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainPrintInfoBuild)
    {
        ListItem item = null;

        this.drpMaintainPrintInfoBuild.Items.Clear();
        this.drpMaintainPrintInfoBuild.Items.Add(string.Empty);

        if (lstMaintainPrintInfoBuild != null)
        {
            foreach (SelectInfoDef temp in lstMaintainPrintInfoBuild)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainPrintInfoBuild.Items.Add(item);
            }

        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainPrintInfoBuild.SelectedIndex = index;
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
            return this.drpMaintainPrintInfoBuild;
        }

    }
}

