using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using System.Text;
using IMES.Infrastructure;


public partial class CommonControl_DataMaintain_CmbMaintainLightDescr : System.Web.UI.UserControl
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string cssClass;
    private string width;
    //private string station;
    //private string customer;
    private List<String[]> selectValues; 
    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public List<String[]> SelectValues
    {
        get { return selectValues; }
        set { selectValues = value; }
    }
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
    public CommonControl_DataMaintain_CmbMaintainLightDescr()
    {
        this.selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightDescrItemValue1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightDescrItemValue1");
        selectValues.Add(item1);

        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightDescrItemValue2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightDescrItemValue2");
        selectValues.Add(item2);
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
                        this.ddlLightDescr.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        this.ddlLightDescr.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    this.ddlLightDescr.Width = Unit.Parse(width);
                }

                this.ddlLightDescr.CssClass = cssClass;
                this.ddlLightDescr.Enabled = enabled;

                if (enabled)
                {
                    initCustomer();
                }
                else
                {
                    this.ddlLightDescr.Items.Add(new ListItem("", ""));
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

    public void initCustomer()
    {
       initControl();
    }

    public void refresh()
    {
        initCustomer();
        up.Update();
    }

    public void clearContent()
    {
        this.ddlLightDescr.Items.Clear();
        ddlLightDescr.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl()
    {
        ListItem item = null;

        this.ddlLightDescr.Items.Clear();
        //this.drpCustomer.Items.Add(string.Empty);
        for (int i = 0; i < this.selectValues.Count;i++ )
        {
            item = new ListItem(selectValues[i][0],selectValues[i][1]);
            this.ddlLightDescr.Items.Add(item);
        }

        //item = new ListItem("China label", "China label");
        //this.ddlGrade.Items.Add(item);
        //item = new ListItem("TaiWan label", "TaiWan label");
        //this.ddlGrade.Items.Add(item);

    }

    public void setSelected(int index)
    {
        this.ddlLightDescr.SelectedIndex = index;
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
            return this.ddlLightDescr;
        }

    }
}
