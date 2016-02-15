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

public partial class CommonControl_DataMaintain_CmbMaintainAssetRuleAstType : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainAssetRuleAstType;
    private IAssetRule iAssetRule;
    private Boolean isPercentage = false;
    private Boolean enabled = true;


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

    //public string MaintainAssetRuleAstType
    //{
    //    get { return MaintainAssetRuleAstType; }
    //    set { MaintainAssetRuleAstType = value; }
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iAssetRule = ServiceAgent.getInstance().GetMaintainObjectByName<IAssetRule>(WebConstant.MaintainAssetRuleObject);
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainAssetRuleAstType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainAssetRuleAstType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainAssetRuleAstType.Width = Unit.Parse(width);
                }

                this.drpMaintainAssetRuleAstType.CssClass = cssClass;
                this.drpMaintainAssetRuleAstType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainAssetRuleAstType();
                }
                else
                {
                    this.drpMaintainAssetRuleAstType.Items.Add(new ListItem("", ""));
                }
            }
        }
        catch (FisException ex)
        {
            //showCmbErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            //showCmbErrorMessage(ex.Message);
        }
    }

    public void initMaintainAssetRuleAstType()
    {

        if (iAssetRule != null)
        {
            //IList<SelectInfoDef> lstMaintainAssetRuleAstType = null;

            //lstMaintainAssetRuleAstType = iAssetRule.GetAstTypeList();
            IList<ConstValueInfo> datalist = iAssetRule.GetCheckItemValue("AstRuleCheckItem", "ASTType");
            string types = "";
            if (datalist.Count != 0)
            {
                types = datalist[0].value;
            }
            IList<string> list = types.Split(',').ToList();
            this.drpMaintainAssetRuleAstType.Items.Clear();
            if (list.Count > 0)
            {
                this.drpMaintainAssetRuleAstType.Items.Add(string.Empty);
                for (int i = 0; list.Count > i; i++)
                {
                    this.drpMaintainAssetRuleAstType.Items.Add(list[i].ToString().Trim());
                }
            }
            else
            {
                this.drpMaintainAssetRuleAstType.Items.Add(string.Empty);
            }


            
            //if (lstMaintainAssetRuleAstType != null && lstMaintainAssetRuleAstType.Count != 0)
            //{
            //    initControl(lstMaintainAssetRuleAstType);
            //}
            //else
            //{
            //    this.drpMaintainAssetRuleAstType.Items.Add(string.Empty);
            //    //initControl(null);
            //}
        }
        else
        {
            this.drpMaintainAssetRuleAstType.Items.Add(string.Empty);
            //initControl(null);
        }
    }

    public void refresh()
    {
        initMaintainAssetRuleAstType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainAssetRuleAstType.Items.Clear();
        drpMaintainAssetRuleAstType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainAssetRuleAstType)
    {
        ListItem item = null;

        this.drpMaintainAssetRuleAstType.Items.Clear();
        this.drpMaintainAssetRuleAstType.Items.Add(string.Empty);
        
        //this.drpMaintainAssetRuleAstType.Items.Add(item);

        if (lstMaintainAssetRuleAstType != null)
        {
            foreach (SelectInfoDef temp in lstMaintainAssetRuleAstType)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainAssetRuleAstType.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainAssetRuleAstType.SelectedIndex = index;
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
            return this.drpMaintainAssetRuleAstType;
        }

    }
}

