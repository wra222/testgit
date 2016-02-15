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

public partial class CommonControl_CmbMaintainStationByItemCheck : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainStationByItemCheck;
    private ICheckItem iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string checkItemId;

    public string CheckItemId
    {
        get { return checkItemId; }
        set { checkItemId = value; }
    }



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

    //public string MaintainStationByItemCheck
    //{
    //    get { return MaintainStationByItemCheck; }
    //    set { MaintainStationByItemCheck = value; }
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<ICheckItem>(WebConstant.MaintainCheckItemObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainStationByItemCheck.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainStationByItemCheck.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainStationByItemCheck.Width = Unit.Parse(width);
                }

                this.drpMaintainStationByItemCheck.CssClass = cssClass;
                this.drpMaintainStationByItemCheck.Enabled = enabled;

                if (enabled)
                {
                    initMaintainStationByItemCheck();
                }
                else
                {
                    this.drpMaintainStationByItemCheck.Items.Add(new ListItem("", ""));
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

    public void initMaintainStationByItemCheck()
    {

        if (iSelectData != null && checkItemId != null && checkItemId != "")
        {
            IList<SelectInfoDef> lstMaintainStationByItemCheck = null;

            lstMaintainStationByItemCheck = iSelectData.GetStationListByCheckedItemID(CheckItemId);

            //Console.Write(lstMaintainStationByItemCheck.Count);
            if (lstMaintainStationByItemCheck != null && lstMaintainStationByItemCheck.Count != 0)
            {
                initControl(lstMaintainStationByItemCheck);
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
        initMaintainStationByItemCheck();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainStationByItemCheck.Items.Clear();
        drpMaintainStationByItemCheck.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainStationByItemCheck)
    {
        ListItem item = null;

        this.drpMaintainStationByItemCheck.Items.Clear();
        //this.drpMaintainStationByItemCheck.Items.Add(string.Empty);

        if (lstMaintainStationByItemCheck != null)
        {
            foreach (SelectInfoDef temp in lstMaintainStationByItemCheck)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainStationByItemCheck.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainStationByItemCheck.SelectedIndex = index;
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
            return this.drpMaintainStationByItemCheck;
        }

    }
}

