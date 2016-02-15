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

public partial class CommonControl_CmbWarrantyCode : System.Web.UI.UserControl
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

    public CommonControl_CmbWarrantyCode()
    {
        this.selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemText1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemValue1");
        selectValues.Add(item1);

        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemText2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemValue2");
        selectValues.Add(item2);

        string[] item3 = new String[2];
        item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemText3");
        item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemValue3");
        selectValues.Add(item3);

        string[] item4 = new String[2];
        item4[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemText4");
        item4[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemValue4");
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
                        drpWarrantyCode.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpWarrantyCode.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpWarrantyCode.Width = Unit.Parse(width);
                }

                this.drpWarrantyCode.CssClass = cssClass;
                this.drpWarrantyCode.Enabled = enabled;

                if (enabled)
                {
                    initWarrantyCode();
                }
                else
                {
                    this.drpWarrantyCode.Items.Add(new ListItem("", ""));
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

    public void initWarrantyCode()
    {

        ListItem item = null;

        this.drpWarrantyCode.Items.Clear();
        this.drpWarrantyCode.Items.Add(string.Empty);

        for (int i = 0; i < this.selectValues.Count; i++)
        {
            item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
            this.drpWarrantyCode.Items.Add(item);
        }
    }

    public void refresh()
    {
        initWarrantyCode();
        up.Update();
    }

    public void clearContent()
    {
        this.drpWarrantyCode.Items.Clear();
        drpWarrantyCode.Items.Add(new ListItem("", ""));
        up.Update();
    }

  
    public void setSelected(int index)
    {
        this.drpWarrantyCode.SelectedIndex = index;
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
            return this.drpWarrantyCode;
        }

    }
}

