/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbMBCode
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-12   Tong.Zhi-Yong     Create 
 * 2010-01-13   Tong.Zhi-Yong     Modify ITC-1103-0027
 * 2012-01-09   zhu lei           Modify ITC211026
 * 
 * Known issues:Any restrictions about this file 
 */
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
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;

public partial class CommonControl_CmbMBCode : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IMB_CODE iMB_CODE;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private Boolean isFromEcrVersion = false;
    private Boolean isDataFilter = false;
    //add by zhulei
    private string _service;

    public String Service
    {
        set { _service = value; }
    }

    public Boolean IsDataFilter
    {
        get { return isDataFilter; }
        set { isDataFilter = value; }
    }

    public Boolean IsFromEcrVersion
    {
        get { return isFromEcrVersion; }
        set { isFromEcrVersion = value; }
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iMB_CODE = ServiceAgent.getInstance().GetObjectByName<IMB_CODE>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.drpMBCode.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMBCode.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMBCode.Width = Unit.Parse(width);
                }

                this.drpMBCode.CssClass = cssClass;
                this.drpMBCode.Enabled = enabled;

                if (enabled)
                {
                    if (!isFromEcrVersion)
                    {
                        initMBCode();
                    }
                    else
                    {
                        initMBCodeForEcrVersion();
                    }
                }
                else
                {
                    this.drpMBCode.Items.Add(new ListItem("", ""));
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

    public void initMBCode()
    {
        if (iMB_CODE != null)
        {
            IList<MB_CODEAndMDLInfo> lstMBCode = null;
            //add by zhulei
            if (_service == "002")
            {
                // MB Label Print
                lstMBCode = iMB_CODE.GetMbCodeAndMdlInfoList("MB", "MB", "MDL", "B SIDE");
            }
            else if (_service == "004")
            {
                // VGA Label Print
                lstMBCode = iMB_CODE.GetMbCodeAndMdlInfoList("MB", "MB", "MDL", "B SIDE", "VGA", "SV");
            }
            else if (isDataFilter)
            {
                lstMBCode = iMB_CODE.GetMBCodeAndMdlListExceptPrinted();
            }
            else
            {
                lstMBCode = iMB_CODE.GetMbCodeAndMdlList();
            }
            
            if (lstMBCode != null && lstMBCode.Count != 0)
            {
                initControl(lstMBCode);
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

    public void initMBCodeForEcrVersion()
    {
        if (iMB_CODE != null)
        {
            IList<string> lstMBCode = iMB_CODE.GetMbCodeList();

            if (lstMBCode != null && lstMBCode.Count != 0)
            {
                initControlForEcrVersion(lstMBCode);
            }
            else
            {
                initControlForEcrVersion(null);
            }
        }
        else
        {
            initControlForEcrVersion(null);
        }
    }

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpMBCode;
        }
    }

    public void refresh()
    {
        initMBCode();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMBCode.Items.Clear();
        drpMBCode.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<MB_CODEAndMDLInfo> lstMBCode)
    {
        ListItem item = null;

        this.drpMBCode.Items.Clear();
        this.drpMBCode.Items.Add(string.Empty);

        if (lstMBCode != null)
        {
            foreach (MB_CODEAndMDLInfo temp in lstMBCode)
            {
                //ITC-1103-0027 Tong.Zhi-Yong 2010-01-13
                item = new ListItem(temp.mbCode + " " + temp.mdl, temp.mbCode);

                this.drpMBCode.Items.Add(item);
            }
        }

    }

    private void initControlForEcrVersion(IList<string> lstMBCode)
    {
        ListItem item = null;

        this.drpMBCode.Items.Clear();
        this.drpMBCode.Items.Add(string.Empty);

        if (lstMBCode != null)
        {
            foreach (string temp in lstMBCode)
            {
                item = new ListItem(temp, temp);

                this.drpMBCode.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpMBCode.SelectedIndex = index;
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
}
