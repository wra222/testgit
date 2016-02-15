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

public partial class CommonControl_DataMaintain_CmbMaintainAssetRuleCheckItem : System.Web.UI.UserControl
{
    const string ASTTYPE_ATSN4 = "ATSN4";
    const string ASTTYPE_ATSN7 = "ATSN7";

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

    private string astType;

    public string AstType
    {
        get { return astType; }
        set { astType = value; }
    }

    public CommonControl_DataMaintain_CmbMaintainAssetRuleCheckItem()
    {

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
                        drpMaintainAssetRuleCheckItem.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainAssetRuleCheckItem.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainAssetRuleCheckItem.Width = Unit.Parse(width);
                }

                this.drpMaintainAssetRuleCheckItem.CssClass = cssClass;
                this.drpMaintainAssetRuleCheckItem.Enabled = enabled;

                if (enabled)
                {
                    initMaintainAssetRuleCheckItem();
                }
                else
                {
                    this.drpMaintainAssetRuleCheckItem.Items.Add(new ListItem("", ""));
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

    public void initMaintainAssetRuleCheckItem()
    {

        ListItem item = null;

        this.drpMaintainAssetRuleCheckItem.Items.Clear();
        this.drpMaintainAssetRuleCheckItem.Items.Add(string.Empty);

        if (this.astType == ASTTYPE_ATSN4)
        {
            this.selectValues = new List<string[]>();
            string[] item1 = new String[2];
            item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText1");
            item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue1");
            selectValues.Add(item1);

            string[] item2 = new String[2];
            item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText2");
            item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue2");
            selectValues.Add(item2);

            string[] item3 = new String[2];
            item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText3");
            item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue3");
            selectValues.Add(item3);

            string[] item4 = new String[2];
            item4[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText4");
            item4[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue4");
            selectValues.Add(item4);

            string[] item5 = new String[2];
            item5[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText5");
            item5[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue5");
            selectValues.Add(item5);

			//Add by Benson For Mantis 1539
            string[] item6 = new String[2];
            item6[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText8");
            item6[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue8");
            selectValues.Add(item6);
            //Add by Benson

			
            for (int i = 0; i < this.selectValues.Count; i++)
            {
                item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
                this.drpMaintainAssetRuleCheckItem.Items.Add(item);
            }

        }
        else if (this.astType == ASTTYPE_ATSN7)
        {
            this.selectValues = new List<string[]>();
            string[] item1 = new String[2];
            item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText1");
            item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue1");
            selectValues.Add(item1);

            string[] item2 = new String[2];
            item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText2");
            item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue2");
            selectValues.Add(item2);

            string[] item3 = new String[2];
            item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText3");
            item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue3");
            selectValues.Add(item3);

            string[] item4 = new String[2];
           // item4[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText6");
           // item4[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue6");
                        //Modify by Benson
            item4[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText8");
            item4[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue8");
          
            //Modify by Benson

			
			selectValues.Add(item4);

            string[] item5 = new String[2];
            item5[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText4");
            item5[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue4");
            selectValues.Add(item5);

            string[] item6 = new String[2];
            item6[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText5");
            item6[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue5");
            selectValues.Add(item6);

            string[] item7 = new String[2];
            item7[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemText7");
            item7[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckItemValue7");
            selectValues.Add(item7);

            for (int i = 0; i < this.selectValues.Count; i++)
            {
                item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
                this.drpMaintainAssetRuleCheckItem.Items.Add(item);
            }
        }
        //else
        //{
            //this.drpMaintainAssetRuleCheckItem.Items.Add(new ListItem("", ""));
        //}

    }

    public void refresh()
    {
        initMaintainAssetRuleCheckItem();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainAssetRuleCheckItem.Items.Clear();
        drpMaintainAssetRuleCheckItem.Items.Add(new ListItem("", ""));
        up.Update();
    }

  
    public void setSelected(int index)
    {
        this.drpMaintainAssetRuleCheckItem.SelectedIndex = index;
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
            return this.drpMaintainAssetRuleCheckItem;
        }

    }
}

