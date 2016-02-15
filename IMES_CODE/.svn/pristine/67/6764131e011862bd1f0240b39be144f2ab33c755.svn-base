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
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using System.Text;


public partial class CommonControl_DataMaintain_CmbMaintainStationType : System.Web.UI.UserControl
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

    public CommonControl_DataMaintain_CmbMaintainStationType()
    {
        this.selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText1");
        selectValues.Add(item1);

        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText2");
        selectValues.Add(item2);

        string[] item3 = new String[2];
        item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText3");
        item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText3");
        selectValues.Add(item3);

        string[] item4 = new String[2];
        item4[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText4");
        item4[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText4");
        selectValues.Add(item4);

        string[] item5 = new String[2];
        item5[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText5");
        item5[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText5");
        selectValues.Add(item5);

        string[] item6 = new String[2];
        item6[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText6");
        item6[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText6");
        selectValues.Add(item6);

        string[] item9 = new String[2];
        item9[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText9");
        item9[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText9");
        selectValues.Add(item9);

        string[] item8 = new String[2];
        item8[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText8");
        item8[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText8");
        selectValues.Add(item8);

        string[] item7 = new String[2];
        item7[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText7");
        item7[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText7");
        selectValues.Add(item7);

        string[] item10 = new String[2];
        item10[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText10");
        item10[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationTypeText10");
        selectValues.Add(item10);
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
                        drpMaintainStationType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainStationType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainStationType.Width = Unit.Parse(width);
                }

                this.drpMaintainStationType.CssClass = cssClass;
                this.drpMaintainStationType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainStationType();
                }
                else
                {
                    this.drpMaintainStationType.Items.Add(new ListItem("", ""));
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

    public void initMaintainStationType()
    {

        ListItem item = null;

        this.drpMaintainStationType.Items.Clear();
        //this.drpMaintainStationType.Items.Add(string.Empty);
        IStation2 iStationManager = ServiceAgent.getInstance().GetMaintainObjectByName<IStation2>(WebConstant.MaintainStationObject);
        DataTable dt = iStationManager.GetStationInfoList();
        //var a = (from DataRow q in dt.Rows
        //         select new { col1 = q["StationType"] }).Distinct();

        IList<string> stationtype = new List<string>();
        foreach (DataRow dr in dt.Rows)
        {
            stationtype.Add(dr["StationType"].ToString());
        }
        stationtype = (from q in stationtype
                       select q).Distinct().ToList<string>();

        foreach (string items in stationtype)
        {
            item = new ListItem(items.Trim(), items.Trim());
            this.drpMaintainStationType.Items.Add(item);
        }


        //for (int i = 0; i < this.selectValues.Count; i++)
        //{
        //    item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
        //    this.drpMaintainStationType.Items.Add(item);
        //}
    }

    public void refresh()
    {
        initMaintainStationType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainStationType.Items.Clear();
        drpMaintainStationType.Items.Add(new ListItem("", ""));
        up.Update();
    }

  
    public void setSelected(int index)
    {
        this.drpMaintainStationType.SelectedIndex = index;
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
            return this.drpMaintainStationType;
        }

    }
}

