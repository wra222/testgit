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

public partial class CommonControl_CmbMaintainValueType : System.Web.UI.UserControl
{

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string cssClass;
    private string width;
    private List<String[]> selectValues;
    //private string station;
    //private string MaintainValueType;
    //private IPartCheck iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string partType;



    public string PartType
    {
        get { return partType; }
        set { partType = value; }
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

    //public string MaintainValueType
    //{
    //    get { return MaintainValueType; }
    //    set { MaintainValueType = value; }
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

    public CommonControl_CmbMaintainValueType()
    {
        this.selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText1");
        selectValues.Add(item1);

        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText2");
        selectValues.Add(item2);

        string[] item3 = new String[2];
        item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText3");
        item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText3");
        selectValues.Add(item3);

        //string[] item4 = new String[2];
        //item4[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText4");
        ////item4[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText4");
        //selectValues.Add(item4);

        //string[] item5 = new String[2];
        //item5[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText5");
        //item5[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText5");
        //selectValues.Add(item5);

        //string[] item6 = new String[2];
        //item6[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText6");
        //item6[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainValueTypeText6");
        //selectValues.Add(item6);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IPartCheck>(WebConstant.MaintainPartCheckObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainValueType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainValueType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainValueType.Width = Unit.Parse(width);
                }

                this.drpMaintainValueType.CssClass = cssClass;
                this.drpMaintainValueType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainValueType();
                }
                else
                {
                    this.drpMaintainValueType.Items.Add(new ListItem("", ""));
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

    public void initMaintainValueType()
    {

        //if (iSelectData != null)
        //{
        //    IList<SelectInfoDef> lstMaintainValueType = null;

        //    lstMaintainValueType = iSelectData.GetValueTypeList(partType);

        //    //Console.Write(lstMaintainValueType.Count);
        //    if (lstMaintainValueType != null && lstMaintainValueType.Count != 0)
        //    {
        //        initControl(lstMaintainValueType);
        //    }
        //    else
        //    {
        //        initControl(null);
        //    }
        //}
        //else
        //{
            initControl(null);
        //}
    }

    public void refresh()
    {
        initMaintainValueType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainValueType.Items.Clear();
        drpMaintainValueType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainValueType)
    {
        ListItem item = null;

        this.drpMaintainValueType.Items.Clear();
        //this.drpMaintainValueType.Items.Add(string.Empty);

        for (int i = 0; i < this.selectValues.Count; i++)
        {
            item = new ListItem(this.selectValues[i][0], this.selectValues[i][1]);
            this.drpMaintainValueType.Items.Add(item);
        }

        //if (lstMaintainValueType != null)
        //{
        //    foreach (SelectInfoDef temp in lstMaintainValueType)
        //    {
        //        //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
        //        item = new ListItem(temp.Text, temp.Value);
        //        this.drpMaintainValueType.Items.Add(item);
        //    }
        //}
    }

    public void setSelected(int index)
    {
        this.drpMaintainValueType.SelectedIndex = index;
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
            return this.drpMaintainValueType;
        }
    }
}

