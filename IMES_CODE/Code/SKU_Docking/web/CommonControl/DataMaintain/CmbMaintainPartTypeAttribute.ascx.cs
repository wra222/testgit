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

public partial class CommonControl_DataMaintain_CmbMaintainPartTypeAttribute : System.Web.UI.UserControl
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
                        drpPartTypeAttributeList.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpPartTypeAttributeList.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpPartTypeAttributeList.Width = Unit.Parse(width);
                }

                this.drpPartTypeAttributeList.CssClass = cssClass;
                this.drpPartTypeAttributeList.Enabled = enabled;

                if (enabled)
                {
                    initPartTypeAttributeList();
                }
                else
                {
                    this.drpPartTypeAttributeList.Items.Add(new ListItem("", ""));
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

    public void initPartTypeAttributeList()
    {

        if (iPartTypeAttribute != null)
        {
            IList<string> lstPartTypeAttribute = null;
            //获取到所有TP值
            lstPartTypeAttribute = iPartTypeAttribute.GetTPList();
            Console.Write(lstPartTypeAttribute.Count);
            if (lstPartTypeAttribute != null && lstPartTypeAttribute.Count != 0)
            {
                this.drpPartTypeAttributeList.Items.Clear();
                foreach (string temp in lstPartTypeAttribute)
                {
                    this.drpPartTypeAttributeList.Items.Add(temp);
                }
            }
            else
            {
                this.drpPartTypeAttributeList.Items.Clear();
            }
        }
        else
        {
            this.drpPartTypeAttributeList.Items.Clear();
        }
    }

    public void refresh()
    {
        initPartTypeAttributeList();
        up.Update();
    }

    public void clearContent()
    {
        this.drpPartTypeAttributeList.Items.Clear();
        drpPartTypeAttributeList.Items.Add(new ListItem("", ""));
        up.Update();
    }

    public void setSelected(int index)
    {
        this.drpPartTypeAttributeList.SelectedIndex = index;
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
            return this.drpPartTypeAttributeList;
        }

    }
}
