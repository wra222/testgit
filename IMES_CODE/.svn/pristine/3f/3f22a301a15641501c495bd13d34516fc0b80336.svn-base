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

public partial class CommonControl_CmbMaintainItemType : System.Web.UI.UserControl
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

    public CommonControl_CmbMaintainItemType()
    {
        this.selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainItemTypeText1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainItemTypeValue1");
        selectValues.Add(item1);

        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainItemTypeText2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainItemTypeValue2");
        selectValues.Add(item2);


        string[] item3 = new String[2];
        item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainItemTypeText3");
        item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainItemTypeValue3");
        selectValues.Add(item3);


        string[] item4 = new String[2];
        item4[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainItemTypeText4");
        item4[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainItemTypeValue4");
        selectValues.Add(item4);

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
                        drpMaintainItemType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainItemType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainItemType.Width = Unit.Parse(width);
                }

                this.drpMaintainItemType.CssClass = cssClass;
                this.drpMaintainItemType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainItemType();
                }
                else
                {
                    this.drpMaintainItemType.Items.Add(new ListItem("", ""));
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

    public void initMaintainItemType()
    {

        ListItem item = null;

        this.drpMaintainItemType.Items.Clear();
        //this.drpMaintainItemType.Items.Add(string.Empty);

        for (int i = 0; i < this.selectValues.Count; i++)
        {
            item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
            this.drpMaintainItemType.Items.Add(item);
        }
    }

    public void refresh()
    {
        initMaintainItemType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainItemType.Items.Clear();
        drpMaintainItemType.Items.Add(new ListItem("", ""));
        up.Update();
    }

  
    public void setSelected(int index)
    {
        this.drpMaintainItemType.SelectedIndex = index;
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
            return this.drpMaintainItemType;
        }

    }
}

