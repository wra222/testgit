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
using com.inventec.iMESWEB;
using System.Text;
using System.Collections.Generic;
using IMES.Infrastructure;

public partial class CommonControl_cmbMaintainITCNDCheckSettingCheckType : System.Web.UI.UserControl
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string cssClass;
    private string width;
    private List<String[]> selectValues;

    public List<String[]> SelectValues
    {
        get { return selectValues; }
        set { selectValues = value; }
    }

    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public CommonControl_cmbMaintainITCNDCheckSettingCheckType()
    {
        this.selectValues = new List<string[]>();

        //string[] item0 = new String[2];
        //item0[0] = "";
        //item0[1] = "";
        //selectValues.Add(item0);

        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainITCNDCheckSettingText1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainITCNDCheckSettingValue1");
        selectValues.Add(item1);

        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainITCNDCheckSettingText2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainITCNDCheckSettingValue2");
        selectValues.Add(item2);
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

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainITCNDCheckSettingCheckType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainITCNDCheckSettingCheckType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainITCNDCheckSettingCheckType.Width = Unit.Parse(width);
                }

                this.drpMaintainITCNDCheckSettingCheckType.CssClass = cssClass;
                this.drpMaintainITCNDCheckSettingCheckType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainTypeObject();
                }
                else
                {
                    this.drpMaintainITCNDCheckSettingCheckType.Items.Add(new ListItem("", ""));
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

    public void initMaintainTypeObject()
    {

        ListItem item = null;

        this.drpMaintainITCNDCheckSettingCheckType.Items.Clear();
        //this.drpMaintainStationObject.Items.Add(string.Empty);

        for (int i = 0; i < this.selectValues.Count; i++)
        {
            item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
            this.drpMaintainITCNDCheckSettingCheckType.Items.Add(item);
        }
    }

    public void refresh()
    {
        initMaintainTypeObject();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainITCNDCheckSettingCheckType.Items.Clear();
        drpMaintainITCNDCheckSettingCheckType.Items.Add(new ListItem("", ""));
        up.Update();
    }


    public void setSelected(int index)
    {
        this.drpMaintainITCNDCheckSettingCheckType.SelectedIndex = index;
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
            return this.drpMaintainITCNDCheckSettingCheckType;
        }

    }
}
