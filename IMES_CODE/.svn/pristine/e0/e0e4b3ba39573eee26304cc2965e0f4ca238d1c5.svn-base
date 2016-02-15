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

public partial class CommonControl_CmbMaintainPartType : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainPartType;
    private IPartCheck iSelectData;
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

    //public string MaintainPartType
    //{
    //    get { return MaintainPartType; }
    //    set { MaintainPartType = value; }
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IPartCheck>(WebConstant.MaintainPartCheckObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainPartType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainPartType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainPartType.Width = Unit.Parse(width);
                }

                this.drpMaintainPartType.CssClass = cssClass;
                this.drpMaintainPartType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainPartType();
                }
                else
                {
                    this.drpMaintainPartType.Items.Add(new ListItem("", ""));
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

    public void initMaintainPartType()
    {

        if (iSelectData != null)
        {
            IList<SelectInfoDef> lstMaintainPartType = null;

            lstMaintainPartType = iSelectData.GetPartTypeList();

            //Console.Write(lstMaintainPartType.Count);
            if (lstMaintainPartType != null && lstMaintainPartType.Count != 0)
            {
                initControl(lstMaintainPartType);
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
        initMaintainPartType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainPartType.Items.Clear();
        drpMaintainPartType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainPartType)
    {
        ListItem item = null;

        this.drpMaintainPartType.Items.Clear();
        //this.drpMaintainPartType.Items.Add(string.Empty);

        if (lstMaintainPartType != null)
        {
            foreach (SelectInfoDef temp in lstMaintainPartType)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainPartType.Items.Add(item);
            }

        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainPartType.SelectedIndex = index;
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
            return this.drpMaintainPartType;
        }

    }
}

