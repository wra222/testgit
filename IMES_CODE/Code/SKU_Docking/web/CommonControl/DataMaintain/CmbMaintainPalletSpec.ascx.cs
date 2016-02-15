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

public partial class CommonControl_CmbMaintainPalletSpec: System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    private IPLTStandard  iSelectData;
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IPLTStandard>(WebConstant.MaintainPLTStandardObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainPalletSpec.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainPalletSpec.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainPalletSpec.Width = Unit.Parse(width);
                }
                
                this.drpMaintainPalletSpec.CssClass = cssClass;
                this.drpMaintainPalletSpec.Enabled = enabled;

                if (enabled)
                {
                    initMaintainPalletSpec();
                }
                else
                {
                    this.drpMaintainPalletSpec.Items.Add(new ListItem("", ""));
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

    public void initMaintainPalletSpec()
    {

        if (iSelectData != null)
        {
            IList<PltspecificationInfo> lstMaintainPalletSpec = null;

            lstMaintainPalletSpec = iSelectData.GetPLTSpecificationList();

            //Console.Write(lstMaintainStation.Count);
            if (lstMaintainPalletSpec != null && lstMaintainPalletSpec.Count != 0)
            {
                initControl(lstMaintainPalletSpec);
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
        initMaintainPalletSpec();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainPalletSpec.Items.Clear();
        drpMaintainPalletSpec.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<PltspecificationInfo> lstMaintainDPltSpec)
    {
        ListItem item = null;

        this.drpMaintainPalletSpec.Items.Clear();
        this.drpMaintainPalletSpec.Items.Add(string.Empty);
        //Value=@Defect Code，Text=@DefectCode +空格+@Descr
        if (lstMaintainDPltSpec != null)
        {
            //item = new ListItem("", "");
            //this.drpMaintainPalletSpec.Items.Add(item);

            foreach (PltspecificationInfo temp in lstMaintainDPltSpec)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.descr, temp.len.ToString()+"|" + temp.wide.ToString() + "|" +temp.high.ToString());
                this.drpMaintainPalletSpec.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainPalletSpec.SelectedIndex = index;
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
            return this.drpMaintainPalletSpec;
        }

    }
}

