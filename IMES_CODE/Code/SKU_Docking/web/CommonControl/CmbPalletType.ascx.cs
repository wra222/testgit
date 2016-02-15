/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbPalletType
* UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx –2011/12/16 
* UC:CI-MES12-SPEC-PAK-UC Pallet Weight –2011/12/16            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-16   Du.Xuan               Create   
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

public partial class CommonControl_CmbPalletType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private string pallettype;
    private string orderby;
    private IConstValue iConstValue;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public string PalletType
    {
        get { return pallettype; }
        set { pallettype = value; }
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

    public string OrderBy
    {
        get { return orderby; }
        set { orderby = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iConstValue = ServiceAgent.getInstance().GetObjectByName<IConstValue>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpPalletType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpPalletType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpPalletType.Width = Unit.Parse(width);
                }

                this.drpPalletType.CssClass = cssClass;
                this.drpPalletType.Enabled = enabled;

                if (enabled)
                {
                    initPalletType(pallettype);
                }
                else
                {
                    this.drpPalletType.Items.Add(new ListItem("", ""));
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

    public void initPalletType(string type)
    {
        if (iConstValue != null)
        {
            IList<ConstValueInfo> lstPalletType = null;
 
            if (!string.IsNullOrEmpty(type))
            {
                lstPalletType = iConstValue.GetConstValueListByType(type,orderby);
            }
                
            if (lstPalletType != null && lstPalletType.Count != 0)
            {
                initControl(lstPalletType);
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
        initPalletType(pallettype);
        up.Update();
    }

    public void refresh(string ptype)
    {
        initPalletType(ptype);
        up.Update();
    }

    public void clearContent()
    {
        this.drpPalletType.Items.Clear();
        drpPalletType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<ConstValueInfo> lstPalletType)
    {
        ListItem item = null;

        this.drpPalletType.Items.Clear();
        this.drpPalletType.Items.Add(string.Empty);

        if (lstPalletType != null)
        {
            foreach (ConstValueInfo temp in lstPalletType)
            {
                item = new ListItem(temp.name);
                this.drpPalletType.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpPalletType.SelectedIndex = index;
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
            return this.drpPalletType;
        }

    }
}
