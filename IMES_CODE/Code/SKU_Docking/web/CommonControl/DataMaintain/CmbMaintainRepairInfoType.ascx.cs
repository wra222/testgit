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
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;

public partial class CommonControl_DataMaintain_CmbMaintainRepairInfoType : System.Web.UI.UserControl
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

    public CommonControl_DataMaintain_CmbMaintainRepairInfoType()
    {
        this.selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeText1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeValue1");
        selectValues.Add(item1);
        
        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeText2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeValue2");
        selectValues.Add(item2);

        string[] item3 = new String[2];
        item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeText3");
        item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeValue3");
        selectValues.Add(item3);

        string[] item4 = new String[2];
        item4[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeText4");
        item4[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeValue4");
        selectValues.Add(item4);

        string[] item5 = new String[2];
        item5[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeText5");
        item5[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeValue5");
        selectValues.Add(item5);

        string[] item6 = new String[2];
        item6[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeText6");
        item6[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaitainRepairInfoTypeValue6");
        selectValues.Add(item6);
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
                        drpMaintainRepairInfoType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainRepairInfoType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainRepairInfoType.Width = Unit.Parse(width);
                }

                this.drpMaintainRepairInfoType.CssClass = cssClass;
                this.drpMaintainRepairInfoType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainTypeObject();
                }
                else
                {
                    this.drpMaintainRepairInfoType.Items.Add(new ListItem("", ""));
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

        this.drpMaintainRepairInfoType.Items.Clear();
        //this.drpMaintainStationObject.Items.Add(string.Empty);

        for (int i = 0; i < this.selectValues.Count; i++)
        {
            item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
            this.drpMaintainRepairInfoType.Items.Add(item);
        }
    }

    public void refresh()
    {
        initMaintainTypeObject();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainRepairInfoType.Items.Clear();
        drpMaintainRepairInfoType.Items.Add(new ListItem("", ""));
        up.Update();
    }

  
    public void setSelected(int index)
    {
        this.drpMaintainRepairInfoType.SelectedIndex = index;
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
            return this.drpMaintainRepairInfoType;
        }

    }
}
