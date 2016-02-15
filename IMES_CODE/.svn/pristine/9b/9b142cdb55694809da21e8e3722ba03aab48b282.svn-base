using System;
//using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using System.Text;

public partial class CommonControl_DataMaintain_CmbMaintainProcessType : System.Web.UI.UserControl
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

    public CommonControl_DataMaintain_CmbMaintainProcessType()
    {
        this.selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeText1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeValue1");
        selectValues.Add(item1);

        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeText2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeValue2");
        selectValues.Add(item2);

        string[] item3 = new String[2];
        item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeText3");
        item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeValue3");
        selectValues.Add(item3);

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
                        drpMaintainProcessType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainProcessType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainProcessType.Width = Unit.Parse(width);
                }

                this.drpMaintainProcessType.CssClass = cssClass;
                this.drpMaintainProcessType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainProcessType();
                }
                else
                {
                    this.drpMaintainProcessType.Items.Add(new ListItem("", ""));
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

    public void initMaintainProcessType()
    {

        ListItem item = null;

        this.drpMaintainProcessType.Items.Clear();
        //this.drpMaintainProcessType.Items.Add(string.Empty);

        for (int i = 0; i < this.selectValues.Count; i++)
        {
            item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
            this.drpMaintainProcessType.Items.Add(item);
        }
    }

    public void refresh()
    {
        initMaintainProcessType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainProcessType.Items.Clear();
        drpMaintainProcessType.Items.Add(new ListItem("", ""));
        up.Update();
    }

  
    public void setSelected(int index)
    {
        this.drpMaintainProcessType.SelectedIndex = index;
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
            return this.drpMaintainProcessType;
        }

    }

    public string GetSelectValue
    {
        get
        {
            return this.drpMaintainProcessType.SelectedValue.ToString();
        }
    }
}

