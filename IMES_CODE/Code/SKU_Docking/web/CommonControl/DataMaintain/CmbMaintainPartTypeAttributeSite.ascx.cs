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

public partial class CommonControl_DataMaintain_CmbMaintainPartTypeAttributeSite : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    //private string station;
    //private string customer;
    private IPartTypeAttribute iPartTypeAttribute;
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

    //public string Customer
    //{
    //    get { return customer; }
    //    set { customer = value; }
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
            iPartTypeAttribute = ServiceAgent.getInstance().GetMaintainObjectByName<IPartTypeAttribute>(WebConstant.PartTypeAttributeObjet);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpSiteList.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpSiteList.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpSiteList.Width = Unit.Parse(width);
                }

                this.drpSiteList.CssClass = cssClass;
                this.drpSiteList.Enabled = enabled;

                if (enabled)
                {
                    initSiteList();
                }
                else
                {
                    this.drpSiteList.Items.Add(new ListItem("", ""));
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
    //初始化
    public void initSiteList()
    {

        if (iPartTypeAttribute != null)
        {
            IList<string> lstCode = null;

            lstCode = iPartTypeAttribute.GetCodeListByTp("Site");

            Console.Write(lstCode.Count);
            if (lstCode != null && lstCode.Count != 0)
            {
                this.drpSiteList.Items.Clear();
                foreach (string temp in lstCode)
                {
                    this.drpSiteList.Items.Add(temp);
                    //this.drpSiteList.Items(i).Selected = true;
                    //var a = this.drpSiteList.Items(i).Value；
                }
                this.checkItem.Value = lstCode.Count.ToString();
            }
            else
            {
                this.drpSiteList.Items.Clear();
                this.checkItem.Value = null;
            }
        }
        else
        {
            this.drpSiteList.Items.Clear();
            this.checkItem.Value = null;
        }
    }

    public void refresh()
    {
        initSiteList();
        up.Update();
    }

    public void clearContent()
    {
        this.drpSiteList.Items.Clear();
        drpSiteList.Items.Add(new ListItem("", ""));
        up.Update();
    }

    //对被选项打勾
    public void setSelected(IList<string> lst)
    {
        foreach (string site in lst)
        {
            for (int i = 0; i < this.drpSiteList.Items.Count; i++)
            {
                if (site == this.drpSiteList.Items[i].Value)
                {
                    this.drpSiteList.Items[i].Selected = true;
                }
            }
        }
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

    public CheckBoxList InnerDropDownList
    {
        get
        {
            return this.drpSiteList;
        }

    }
}
