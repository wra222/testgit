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

public partial class CommonControl_cmbMaintainCause : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    private IRepairInfoMaintain iSelectData;
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

    //public string MaintainStation
    //{
    //    get { return MaintainStation; }
    //    set { MaintainStation = value; }
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
            iSelectData = (IRepairInfoMaintain)ServiceAgent.getInstance().GetMaintainObjectByName<IRepairInfoMaintain>(WebConstant.REPAIRINFOMAINTAIN);
            //ServiceAgent.getInstance().GetMaintainObjectByName<IDefectStation>(WebConstant.MaintainCauseStationObject);
            string Causetype = "FACause";

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainCause.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainCause.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainCause.Width = Unit.Parse(width);
                }

                this.drpMaintainCause.CssClass = cssClass;
                this.drpMaintainCause.Enabled = enabled;

                if (enabled)
                {
                    initMaintainDefectCause(Causetype);
                }
                else
                {
                    this.drpMaintainCause.Items.Add(new ListItem("", ""));
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

    public void initMaintainDefectCause(string Causetype)
    {

        if (iSelectData != null)
        {
            IList<RepairInfoMaintainDef> lstMaintainCause = null;

            lstMaintainCause = iSelectData.GetRepairInfoByCondition(Causetype);

            //Console.Write(lstMaintainStation.Count);
            if (lstMaintainCause != null && lstMaintainCause.Count != 0)
            {
                initControl(lstMaintainCause);
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
        initMaintainDefectCause("FACause");
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainCause.Items.Clear();
        drpMaintainCause.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<RepairInfoMaintainDef> lstMaintainCause)
    {
        ListItem item = null;

        this.drpMaintainCause.Items.Clear();
        this.drpMaintainCause.Items.Add(string.Empty);
        //Value=@Defect Code，Text=@DefectCode +空格+@Descr
        if (lstMaintainCause != null)
        {
            foreach (RepairInfoMaintainDef temp in lstMaintainCause)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.code+" "+temp.description, temp.code);
                this.drpMaintainCause.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainCause.SelectedIndex = index;
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
            return this.drpMaintainCause;
        }

    }
}

