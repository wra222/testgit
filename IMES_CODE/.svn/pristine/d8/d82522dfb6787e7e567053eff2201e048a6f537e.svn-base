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

public partial class CommonControl_CmbWarrantyFormat : System.Web.UI.UserControl
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

    public CommonControl_CmbWarrantyFormat()
    {
        this.selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyFormatItemText1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyFormatItemValue1");
        selectValues.Add(item1);

        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyFormatItemText2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyFormatItemValue2");
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
                        drpWarrantyFormat.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpWarrantyFormat.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpWarrantyFormat.Width = Unit.Parse(width);
                }

                this.drpWarrantyFormat.CssClass = cssClass;
                this.drpWarrantyFormat.Enabled = enabled;

                if (enabled)
                {
                    initWarrantyFormat();
                }
                else
                {
                    this.drpWarrantyFormat.Items.Add(new ListItem("", ""));
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

    public void initWarrantyFormat()
    {

        ListItem item = null;

        this.drpWarrantyFormat.Items.Clear();
        //this.drpWarrantyFormat.Items.Add(string.Empty);

        for (int i = 0; i < this.selectValues.Count; i++)
        {
            item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
            this.drpWarrantyFormat.Items.Add(item);
        }
    }

    public void refresh()
    {
        initWarrantyFormat();
        up.Update();
    }

    public void clearContent()
    {
        this.drpWarrantyFormat.Items.Clear();
        drpWarrantyFormat.Items.Add(new ListItem("", ""));
        up.Update();
    }

  
    public void setSelected(int index)
    {
        this.drpWarrantyFormat.SelectedIndex = index;
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
            return this.drpWarrantyFormat;
        }

    }
}

