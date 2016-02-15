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

public partial class CommonControl_cmbMaintainDefect : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    private IDefectStation iSelectData;
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IDefectStation>(WebConstant.MaintainDefectStationObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainDefect.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainDefect.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainDefect.Width = Unit.Parse(width);
                }

                this.drpMaintainDefect.CssClass = cssClass;
                this.drpMaintainDefect.Enabled = enabled;

                if (enabled)
                {
                    initMaintainStation();
                }
                else
                {
                    this.drpMaintainDefect.Items.Add(new ListItem("", ""));
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

    public void initMaintainStation()
    {

        if (iSelectData != null)
        {
            IList<DefectInfo> lstMaintainDefect = null;

            lstMaintainDefect = iSelectData.GetDefectCodeList();

            //Console.Write(lstMaintainStation.Count);
            if (lstMaintainDefect != null && lstMaintainDefect.Count != 0)
            {
                initControl(lstMaintainDefect);
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
        initMaintainStation();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainDefect.Items.Clear();
        drpMaintainDefect.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<DefectInfo> lstMaintainDefect)
    {
        ListItem item = null;

        this.drpMaintainDefect.Items.Clear();
        this.drpMaintainDefect.Items.Add(string.Empty);
        //Value=@Defect Code，Text=@DefectCode +空格+@Descr
        if (lstMaintainDefect != null)
        {
            foreach (DefectInfo temp in lstMaintainDefect)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.id+" "+temp.description, temp.id);
                this.drpMaintainDefect.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainDefect.SelectedIndex = index;
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
            return this.drpMaintainDefect;
        }

    }
}

