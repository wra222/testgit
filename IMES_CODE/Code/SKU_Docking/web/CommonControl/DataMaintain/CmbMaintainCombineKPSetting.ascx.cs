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
using System.Text;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
public partial class CommonControl_cmbMaintainCombineKPSetting : System.Web.UI.UserControl
{
    
       
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string cssClass;
    private string width;
    private List<String[]> selectValues;
    private ICombineKPSetting icombineKPSetting;
    public List<String[]> SelectValues
    {
        get { return selectValues; }
        set { selectValues = value; }
    }

    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string stateofdrp = "";
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
    public string StateOfDrp
    {
        get { return stateofdrp; }
        set { stateofdrp = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!this.IsPostBack)
            {
                icombineKPSetting = (ICombineKPSetting)ServiceAgent.getInstance().GetMaintainObjectByName<ICombineKPSetting>(WebConstant.ICOMBINEKPSETTING);
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainCombineKPSetting.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainCombineKPSetting.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainCombineKPSetting.Width = Unit.Parse(width);
                }

                this.drpMaintainCombineKPSetting.CssClass = cssClass;
                this.drpMaintainCombineKPSetting.Enabled = enabled;
                if (enabled)
                {
                    initMaintainObject();
                }
                else
                {
                    this.drpMaintainCombineKPSetting.Items.Add(new ListItem("", ""));
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

    public void initMaintainObject()
    {
        this.selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = "";
        item1[1] = "";
        this.selectValues.Add(item1);

        

        this.drpMaintainCombineKPSetting.Items.Clear();
        for (int i = 0; i < this.selectValues.Count; i++)
        {
            ListItem item = null;
            item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
            this.drpMaintainCombineKPSetting.Items.Add(item);
        }
        if (stateofdrp == "station")
        {
            IList<StationInfo> relist = icombineKPSetting.GetStation();
            foreach (StationInfo temp in relist)
            {
                ListItem item = null;
                item = new ListItem(temp.StationId + " " + temp.Descr, temp.StationId);
                this.drpMaintainCombineKPSetting.Items.Add(item);
            }
        }
        else if (stateofdrp == "line")
        {
           IList<string> relist = icombineKPSetting.GetLine();
           foreach (string temp in relist)
           {
               ListItem item = null;
               item = new ListItem(temp, temp);
               this.drpMaintainCombineKPSetting.Items.Add(item);
           }

        }
        else if ((stateofdrp == "checktype"))
        {
            IList<string> relist = icombineKPSetting.GetCheckType();
            foreach (string temp in relist)
            {
                ListItem item = null;
                item = new ListItem(temp, temp);
                this.drpMaintainCombineKPSetting.Items.Add(item);
            }
        }
        

       
    }

    public void refresh()
    {
        initMaintainObject();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainCombineKPSetting.Items.Clear();
        drpMaintainCombineKPSetting.Items.Add(new ListItem("", ""));
        up.Update();
    }


    public void setSelected(int index)
    {
        this.drpMaintainCombineKPSetting.SelectedIndex = index;
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
            return this.drpMaintainCombineKPSetting;
        }

    }
}
