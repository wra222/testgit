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

public partial class CommonControl_DataMaintain_CmbMaintainProcess : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainProcess;
    private IRework iRework;
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

    //public string MaintainProcess
    //{
    //    get { return MaintainProcess; }
    //    set { MaintainProcess = value; }
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
            iRework = ServiceAgent.getInstance().GetMaintainObjectByName<IRework>(WebConstant.MaintainReworkObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainProcess.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainProcess.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainProcess.Width = Unit.Parse(width);
                }

                this.drpMaintainProcess.CssClass = cssClass;
                this.drpMaintainProcess.Enabled = enabled;

                if (enabled)
                {
                    initMaintainProcess();
                }
                else
                {
                    this.drpMaintainProcess.Items.Add(new ListItem("", ""));
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

    public void initMaintainProcess()
    {

        if (iRework != null)
        {
            IList<ProcessMaintainInfo> lstMaintainProcess = null;

            lstMaintainProcess = iRework.GetProcessList();

            if (lstMaintainProcess != null && lstMaintainProcess.Count != 0)
            {
                initControl(lstMaintainProcess);
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
        initMaintainProcess();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainProcess.Items.Clear();
        drpMaintainProcess.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<ProcessMaintainInfo> lstMaintainProcess)
    {
        ListItem item = null;

        this.drpMaintainProcess.Items.Clear();
        //this.drpMaintainProcess.Items.Add(string.Empty);

        if (lstMaintainProcess != null)
        {
            foreach (ProcessMaintainInfo temp in lstMaintainProcess)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Process, temp.Process);
                this.drpMaintainProcess.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainProcess.SelectedIndex = index;
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
            return this.drpMaintainProcess;
        }

    }
}

