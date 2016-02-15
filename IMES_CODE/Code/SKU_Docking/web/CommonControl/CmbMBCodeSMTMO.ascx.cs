using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.Station.Interface.StationIntf;

public partial class CommonControl_CmbMBCodeSMTMO : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IGenSMTMO iGenSMTMO = null;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

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
            iGenSMTMO = ServiceAgent.getInstance().GetObjectByName<IGenSMTMO>(WebConstant.GenSMTMOObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.drpMBCode.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMBCode.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMBCode.Width = Unit.Parse(width);
                }

                this.drpMBCode.CssClass = cssClass;
                this.drpMBCode.Enabled = enabled;

                if (enabled)
                {
                    initMBCode();                    
                }
                else
                {
                    this.drpMBCode.Items.Add(new ListItem("", ""));
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

    public void initMBCode()
    {
        if (iGenSMTMO != null)
        {
            IList<MbCodeAndMdlInfo> lst = null;

            lst = iGenSMTMO.GetMBCodeAndPCB();            
            if (lst != null && lst.Count != 0)
            {
                initControl(lst);
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

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpMBCode;
        }
    }

    public void refresh()
    {
        initMBCode();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMBCode.Items.Clear();
        drpMBCode.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<MbCodeAndMdlInfo> lstMBCode)
    {
        ListItem item = null;

        this.drpMBCode.Items.Clear();
        this.drpMBCode.Items.Add(string.Empty);
        if (lstMBCode != null)
        {
            foreach (MbCodeAndMdlInfo temp in lstMBCode)
            {
                item = new ListItem(temp.mbCode + " " + temp.mdl, temp.mbCode);
                this.drpMBCode.Items.Add(item);
            }
        }        
    }

    public void setSelected(int index)
    {
        this.drpMBCode.SelectedIndex = index;
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
}
