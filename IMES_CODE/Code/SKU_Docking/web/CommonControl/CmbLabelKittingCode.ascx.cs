/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbLabelKittingCode
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx –2011/1/6 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx –2011/1/6            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-1-6   Du.Xuan               Create   
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

public partial class CommonControl_CmbLabelKittingCode : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private string codetype;
    private ILabelKittingCode iLabelKittingCode;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public string CodeType
    {
        get { return codetype; }
        set { codetype = value; }
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
            iLabelKittingCode = ServiceAgent.getInstance().GetObjectByName<ILabelKittingCode>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpLabelKittingCode.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpLabelKittingCode.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpLabelKittingCode.Width = Unit.Parse(width);
                }

                this.drpLabelKittingCode.CssClass = cssClass;
                this.drpLabelKittingCode.Enabled = enabled;

                if (enabled)
                {
                    initCodeType(codetype);
                }
                else
                {
                    this.drpLabelKittingCode.Items.Add(new ListItem("", ""));
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

    public void initCodeType(string type)
    {
        if (iLabelKittingCode != null)
        {
            IList<LabelKittingCode> lstLabelKittingCode = null;
 
            if (!string.IsNullOrEmpty(type))
            {
                lstLabelKittingCode = iLabelKittingCode.GetLabelKittingCodeList(type);
            }

            if (lstLabelKittingCode != null && lstLabelKittingCode.Count != 0)
            {
                initControl(lstLabelKittingCode);
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
        initCodeType(codetype);
        up.Update();
    }

    public void refresh(string ctype)
    {
        initCodeType(ctype);
        up.Update();
    }

    public void clearContent()
    {
        this.drpLabelKittingCode.Items.Clear();
        drpLabelKittingCode.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<LabelKittingCode> lstLabelKittingCode)
    {
        ListItem item = null;

        this.drpLabelKittingCode.Items.Clear();
        this.drpLabelKittingCode.Items.Add(string.Empty);

        if (lstLabelKittingCode != null)
        {
            foreach (LabelKittingCode temp in lstLabelKittingCode)
            {
                item = new ListItem(temp.code);
                this.drpLabelKittingCode.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpLabelKittingCode.SelectedIndex = index;
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
            return this.drpLabelKittingCode;
        }

    }
}
