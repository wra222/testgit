
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

public partial class CommonControl_DataMaintain_CmbMaintainAssetRuleCheckStation : System.Web.UI.UserControl
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string cssClass;
    private string width;
    private List<String[]> selectValues;
    private IAssetRule iAssetRule;
    private IDefectStation iDefectStation;

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

    public CommonControl_DataMaintain_CmbMaintainAssetRuleCheckStation()
    {
        //this.selectValues = new List<string[]>();
        //string[] item1 = new String[2];
        //item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckStationText1");
        //item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckStationValue1");
        //selectValues.Add(item1);

        //string[] item2 = new String[2];
        //item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckStationText2");
        //item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckStationValue2");
        //selectValues.Add(item2);

        //string[] item3 = new String[2];
        //item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckStationText3");
        //item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainAssetRuleCheckStationValue3");
        //selectValues.Add(item3);

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
            iAssetRule = ServiceAgent.getInstance().GetMaintainObjectByName<IAssetRule>(WebConstant.MaintainAssetRuleObject);
            iDefectStation = ServiceAgent.getInstance().GetMaintainObjectByName<IDefectStation>(WebConstant.MaintainDefectStationObject);
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainAssetRuleCheckStation.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainAssetRuleCheckStation.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainAssetRuleCheckStation.Width = Unit.Parse(width);
                }

                this.drpMaintainAssetRuleCheckStation.CssClass = cssClass;
                this.drpMaintainAssetRuleCheckStation.Enabled = enabled;

                if (enabled)
                {
                    initMaintainAssetRuleCheckStation();
                }
                else
                {
                    this.drpMaintainAssetRuleCheckStation.Items.Add(new ListItem("", ""));
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

    public void initMaintainAssetRuleCheckStation()
    {
        //ListItem item = new ListItem();
        this.drpMaintainAssetRuleCheckStation.Items.Clear();
        this.drpMaintainAssetRuleCheckStation.Items.Add(string.Empty);
        IList<ConstValueInfo> list = iAssetRule.GetCheckItemValue("AstRuleCheckItem", "Station");
        IList<StationMaintainInfo> stationlist = iDefectStation.GetStationList();
        IList<string> listitem = new List<string>();
        if (list.Count != 0)
        {
            listitem = list[0].value.ToString().Split(',').ToList();
        }

        IList<ListItem> s = (from constvalue in listitem
                             join station in stationlist on constvalue equals station.Station
                             select new ListItem { Text = station.Station + " " + station.Descr, Value = station.Station }).ToList();

        if (s.Count > 0)
        {
            foreach (ListItem items in s)
            {

                drpMaintainAssetRuleCheckStation.Items.Add(items);
            }
        }
        else
        {
            this.drpMaintainAssetRuleCheckStation.Items.Add(string.Empty);
        }
        //for (int i = 0; i < this.selectValues.Count; i++)
        //{
        //    item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
        //    this.drpMaintainAssetRuleCheckStation.Items.Add(item);
        //}
    }

    public void refresh()
    {
        initMaintainAssetRuleCheckStation();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainAssetRuleCheckStation.Items.Clear();
        drpMaintainAssetRuleCheckStation.Items.Add(new ListItem("", ""));
        up.Update();
    }

  
    public void setSelected(int index)
    {
        this.drpMaintainAssetRuleCheckStation.SelectedIndex = index;
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
            return this.drpMaintainAssetRuleCheckStation;
        }

    }
}