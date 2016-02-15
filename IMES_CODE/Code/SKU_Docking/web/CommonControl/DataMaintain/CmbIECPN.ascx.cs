/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbIECPN
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012/09/04   Kaisheng          Create
 * 
 * Known issues:Any restrictions about this file 
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
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.DataModel;

public partial class CommonControl_DataMaintain__CmbIECPN : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string ctno;
    private string station;
    private string customer;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string stage;
    private ISQEDefectReport _iIecpn = (ISQEDefectReport)ServiceAgent.getInstance().GetMaintainObjectByName<ISQEDefectReport>(WebConstant.SQEDefectReportManager);
    //ISQEDefectReport SQEDefectReport = (ISQEDefectReport)ServiceAgent.getInstance().GetMaintainObjectByName<ITestMB>(WebConstant.SQEDefectReportManager);

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public string CtNo
    {
        get { return ctno; }
        set { ctno = value; }
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
            // _iIecpn = ServiceAgent.getInstance().GetObjectByName<ITestMB>(WebConstant.SQEDefectReportManager);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpIECPN.Width = Unit.Percentage(100);
                        
                    }
                    else
                    {
                        drpIECPN.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpIECPN.Width = Unit.Parse(width);
                }

                this.drpIECPN.CssClass = cssClass;
                this.drpIECPN.Enabled = enabled;

                if (enabled)
                {
                    InitIecpn(CtNo);
                }
                else
                {
                    this.drpIECPN.Items.Add(new ListItem("", ""));
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

    public void InitIecpn(string strctNo)
    {
        if (_iIecpn != null)
        {
            IList<String> lstIecpn = null;

            if (!string.IsNullOrEmpty(strctNo))
            {
                lstIecpn = _iIecpn.GetIecpnList(strctNo);
            }

            if (lstIecpn != null && lstIecpn.Count != 0)
            {
                InitControl(lstIecpn);
            }
            else
            {
                InitControl(null);
            }
        }
        else
        {
            InitControl(null);
        }
    }

    public void refresh()
    {
        InitIecpn(CtNo);
        up.Update();
    }

    public void refresh( string strctNo)
    {
        InitIecpn(strctNo);
        up.Update();
    }

    public void ClearContent()
    {
        this.drpIECPN.Items.Clear();
        drpIECPN.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void InitControl(IList<string> lstIecpn)
    {
        ListItem item = null;

        this.drpIECPN.Items.Clear();


        if (lstIecpn != null && lstIecpn.Count!=0)
        {
            foreach (string temp in lstIecpn)
            {
                //ITC-1103-0001 Tong.Zhi-Yong 2010-01-12
                //ADD trim() -- Line取得有空格，过滤  ----如需匹配line的话，需要过滤空格
                item = new ListItem(temp, temp);
                this.drpIECPN.Items.Add(item);
            }
        }
        else
        {
            this.drpIECPN.Items.Add(string.Empty);
        }

    }

    public void setSelected(int index)
    {
        this.drpIECPN.SelectedIndex = index;
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
            return this.drpIECPN;
        }

    }
}