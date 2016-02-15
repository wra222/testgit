/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbLabelKittingCode
* UI:CI-MES12-SPEC-PAK-UI Pizza Kitting.docx –2011/1/6 
* UC:CI-MES12-SPEC-PAK-UC Pizza Kitting.docx –2011/1/6            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-1-6   Du.Xuan               Create   
* ITC-1360-0931 排序顺序与sql有差异，沿用sql的
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
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
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.DataModel;


public partial class CommonControl_CmbCollectStage : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private string valueType;
    private IConstValue iConstValue;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private Boolean bNeedStationInText = false;
    private Boolean orderByDesc = false;

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public string ValueType
    {
        get { return valueType; }
        set { valueType = value; }
    }

    public Boolean BNeedStationInText
    {
        get { return bNeedStationInText; }
        set { bNeedStationInText = value; }
    }

    public bool OrderByDesc
    {
        get { return orderByDesc; }
        set { orderByDesc = value; }
    }

    public string Station
    {
        get { return station; }
        set { station = value; }
    }

    public string Customer
    {
        get { return customer; }
        set { customer = value; }
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
         //   iConstValue = ServiceAgent.getInstance().GetObjectByName<IConstValue>(WebConstant.CommonObject);
            iConstValue = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValue>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpStage.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpStage.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpStage.Width = Unit.Parse(width);
                }

                this.drpStage.CssClass = cssClass;
                this.drpStage.Enabled = enabled;

                if (enabled)
                {
                    initDataByType(valueType);
                }
                else
                {
                    this.drpStage.Items.Add(new ListItem("", ""));
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

    

    public void initDataByType(string type)
    {
        if (iConstValue != null)
        {
            IList<ConstValueTypeInfo> lstValue = null;
 
            if (!string.IsNullOrEmpty(type))
            {
                lstValue = iConstValue.GetConstValueTypeListByType(type);
            }

            if (lstValue != null && lstValue.Count != 0)
            {
                initControl(lstValue);
            }
            else
            {
                initControl(null);
            }
        }
        else
        {
            initControl(null);
        }
    }

    public void refresh()
    {
        initDataByType(valueType);
        up.Update();
    }

    public void refresh(string ctype)
    {
        initDataByType(valueType);
        up.Update();
    }

    public void clearContent()
    {
        this.drpStage.Items.Clear();
        drpStage.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<ConstValueTypeInfo> lstValue)
    {
        ListItem item = null;

        this.drpStage.Items.Clear();
        this.drpStage.Items.Add(string.Empty);

        if (lstValue != null)
        {
            IEnumerable<ConstValueTypeInfo> newList;
            if (orderByDesc)
            {
                newList = lstValue.OrderByDescending(ConstValueTypeInfo => ConstValueTypeInfo.value);
            }
            else
            {
                newList = lstValue.OrderBy(ConstValueTypeInfo => ConstValueTypeInfo.value);
            }


            foreach (ConstValueTypeInfo temp in newList)
            {
                //if (bNeedStationInText)
                //{
                //    item = new ListItem(temp.StationId + " " + temp.Descr, temp.StationId);
                //}
                //else
                //{
                //    item = new ListItem(temp.Descr, temp.StationId);
                //}
                item = new ListItem(temp.value, temp.value);
                this.drpStage.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpStage.SelectedIndex = index;
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
            return this.drpStage;
        }

    }
}
