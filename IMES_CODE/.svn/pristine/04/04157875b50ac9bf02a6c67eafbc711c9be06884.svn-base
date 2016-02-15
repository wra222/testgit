/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbPdLine
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-05   Tong.Zhi-Yong     Create 
 * 2010-01-12   Tong.Zhi-Yong     Modify ITC-1103-0001
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
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.DataModel;

public partial class CommonControl_CmbPdLine : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private IPdLine iPDLine;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string stage;

    public string Width
    {
        get { return width; }
        set { width = value; }
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

    public string Stage
    {
        get { return stage; }
        set { stage = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iPDLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpPDLine.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpPDLine.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpPDLine.Width = Unit.Parse(width);
                }

                this.drpPDLine.CssClass = cssClass;
                this.drpPDLine.Enabled = enabled;

                if (enabled)
                {
                    initPDLine(station, customer, stage);
                }
                else
                {
                    this.drpPDLine.Items.Add(new ListItem("", ""));
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

    public void initPDLine(string paraStation, string customer, string stage)
    {
        if (iPDLine != null)
        {
            IList<PdLineInfo> lstPDLine = null;

            if (!string.IsNullOrEmpty(customer))
            {
                if (!string.IsNullOrEmpty(paraStation))
                {
                    lstPDLine = iPDLine.GetPdLineList(paraStation, customer);
                }
                else if (!string.IsNullOrEmpty(stage))
                {
                    lstPDLine = iPDLine.GetPdLineListByStageAndCustomer(stage, customer);
                }
                else
                {
                    lstPDLine = iPDLine.GetPdLineList(customer);
                }
            }

            if (lstPDLine != null && lstPDLine.Count != 0)
            {
                initControl(lstPDLine);
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
        initPDLine(station, customer,stage);
        up.Update();
    }

    public void refresh(string paraStation, string customer)
    {
        initPDLine(paraStation, customer,stage);
        up.Update();
    }

    public void clearContent()
    {
        this.drpPDLine.Items.Clear();
        drpPDLine.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<PdLineInfo> lstPDLine)
    {
        ListItem item = null;

        this.drpPDLine.Items.Clear();
        this.drpPDLine.Items.Add(string.Empty);

        if (lstPDLine != null)
        {
            foreach (PdLineInfo temp in lstPDLine)
            {
                //ITC-1103-0001 Tong.Zhi-Yong 2010-01-12
                //ADD trim() -- Line取得有空格，过滤  ----如需匹配line的话，需要过滤空格
                item = new ListItem(temp.id.Trim() + " " + temp.friendlyName.Trim(), temp.id.Trim());

                this.drpPDLine.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpPDLine.SelectedIndex = index;
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
            return this.drpPDLine;
        }

    }
}
