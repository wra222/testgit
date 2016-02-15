using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;

public partial class CommonControl_DataMaintain_CmbMaintainGrade : System.Web.UI.UserControl
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string cssClass;
    private string width;
    //private string station;
    //private string customer;
    private List<String[]> selectValues; 
    private IGrade iGrade;
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
    public CommonControl_DataMaintain_CmbMaintainGrade()
    {
        this.selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainGradeGradeItemValue1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainGradeGradeItemValue1");
        selectValues.Add(item1);

        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainGradeGradeItemValue2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainGradeGradeItemValue2");
        selectValues.Add(item2);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iGrade = ServiceAgent.getInstance().GetMaintainObjectByName<IGrade>(WebConstant.GradeMAITAIN);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.ddlGrade.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        this.ddlGrade.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    this.ddlGrade.Width = Unit.Parse(width);
                }

                this.ddlGrade.CssClass = cssClass;
                this.ddlGrade.Enabled = enabled;

                if (enabled)
                {
                    initCustomer();
                }
                else
                {
                    this.ddlGrade.Items.Add(new ListItem("", ""));
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
        this.ddlGrade.Items.Clear();
        ddlGrade.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl()
    {
        ListItem item = null;

        this.ddlGrade.Items.Clear();
        //this.drpCustomer.Items.Add(string.Empty);
        for (int i = 0; i < this.selectValues.Count;i++ )
        {
            item = new ListItem(selectValues[i][0],selectValues[i][1]);
            this.ddlGrade.Items.Add(item);
        }

        //item = new ListItem("Commercial", "Commercial");
        //this.ddlGrade.Items.Add(item);
        //item = new ListItem("Consumer", "Consumer");
        //this.ddlGrade.Items.Add(item);

        //item = new ListItem("Add New...", "$$$");
        //this.drpCustomer.Items.Add(item);
    }

    public void setSelected(int index)
    {
        this.ddlGrade.SelectedIndex = index;
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
            return this.ddlGrade;
        }

    }
}
