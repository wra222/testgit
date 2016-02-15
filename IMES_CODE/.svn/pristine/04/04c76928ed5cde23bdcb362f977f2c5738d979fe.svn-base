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

public partial class CommonControl_DataMaintain_CmbMaintainFaKittingFamily : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainFaKittingFamily;
    private IFaKittingUpload iSelectData;
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

    //public string MaintainFaKittingFamily
    //{
    //    get { return MaintainFaKittingFamily; }
    //    set { MaintainFaKittingFamily = value; }
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IFaKittingUpload>(WebConstant.MaintainFaKittingUploadObject);
            
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainFaKittingFamily.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainFaKittingFamily.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainFaKittingFamily.Width = Unit.Parse(width);
                }

                this.drpMaintainFaKittingFamily.CssClass = cssClass;
                this.drpMaintainFaKittingFamily.Enabled = enabled;

                if (enabled)
                {
                    initMaintainFaKittingFamily();
                }
                else
                {
                    this.drpMaintainFaKittingFamily.Items.Add(new ListItem("", ""));
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

    public void initMaintainFaKittingFamily()
    {

        if (iSelectData != null)
        {
            IList<SelectInfoDef> lstMaintainFaKittingFamily = null;

            lstMaintainFaKittingFamily = iSelectData.GetAllFamilyList();

            //Console.Write(lstMaintainFaKittingFamily.Count);
            if (lstMaintainFaKittingFamily != null && lstMaintainFaKittingFamily.Count != 0)
            {
                initControl(lstMaintainFaKittingFamily);
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
        initMaintainFaKittingFamily();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainFaKittingFamily.Items.Clear();
        drpMaintainFaKittingFamily.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainFaKittingFamily)
    {
        ListItem item = null;

        this.drpMaintainFaKittingFamily.Items.Clear();
        //this.drpMaintainFaKittingFamily.Items.Add(string.Empty);

        if (lstMaintainFaKittingFamily != null)
        {
            foreach (SelectInfoDef temp in lstMaintainFaKittingFamily)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainFaKittingFamily.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainFaKittingFamily.SelectedIndex = index;
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
            return this.drpMaintainFaKittingFamily;
        }

    }
}


