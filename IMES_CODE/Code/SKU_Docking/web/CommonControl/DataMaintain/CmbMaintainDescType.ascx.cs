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

public partial class CommonControl_DataMaintain_CmbMaintainDescType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;

    private IBomDescrMaintain iBomDescrMaintain;
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
            iBomDescrMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<IBomDescrMaintain>(WebConstant.MaintainBomDescrObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpDescTypeList.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpDescTypeList.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpDescTypeList.Width = Unit.Parse(width);
                }

                this.drpDescTypeList.CssClass = cssClass;
                this.drpDescTypeList.Enabled = enabled;

                if (enabled)
                {
                    initPartTypeAttributeList();
                }
                else
                {
                    this.drpDescTypeList.Items.Add(new ListItem("", ""));
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

        if (iBomDescrMaintain != null)
        {
            IList<string> lstDescType = null;
            //获取到所有TP值
            lstDescType = iBomDescrMaintain.GetAllDescrType();
            if (lstDescType != null && lstDescType.Count != 0)
            {
                this.drpDescTypeList.Items.Clear();
                foreach (string temp in lstDescType)
                {
                    this.drpDescTypeList.Items.Add(temp);
                }              
            }
            else
            {
                this.drpDescTypeList.Items.Clear();
            }
        }
        else
        {
            this.drpDescTypeList.Items.Clear();
        }
    }

    public void refresh()
    {
        initPartTypeAttributeList();
        up.Update();
    }

    public void clearContent()
    {
        this.drpDescTypeList.Items.Clear();
        drpDescTypeList.Items.Add(new ListItem("", ""));
        up.Update();
    }

    public void setSelected(int index)
    {
        this.drpDescTypeList.SelectedIndex = index;
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
            return this.drpDescTypeList;
        }

    }
}
