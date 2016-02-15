/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbSamplea
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
//using IMES.Station.Interface.StationIntf;

public partial class CommonControl_CmbSamplea : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    //private IPdLine iPDLine;
    //private IOfflineLabelPrint iOfflineLabelPrint;
    private ISamplea iSamplea;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private IList<OfflineLableSettingDef> __g_list = new List<OfflineLableSettingDef>();

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
            //iPDLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.CommonObject);
            //IOfflineLabelPrint iOfflineLabelPrint = ServiceAgent.getInstance().GetObjectByName<IOfflineLabelPrint>(WebConstant.OfflineLabelPrintObject);
            iSamplea = ServiceAgent.getInstance().GetObjectByName<ISamplea>(WebConstant.CommonObject);
        


            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpSamplea.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpSamplea.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpSamplea.Width = Unit.Parse(width);
                }

                this.drpSamplea.CssClass = cssClass;
                this.drpSamplea.Enabled = enabled;

                if (enabled)
                {
                    initSamplea();
                }
                else
                {
                    this.drpSamplea.Items.Add(new ListItem("", ""));
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

    public void initSamplea()
    {
        if (iSamplea != null)
        {
            //IList<PdLineInfo> lstPDLine = null;
            IList<OfflineLableSettingDef> lst_Samplea = new List<OfflineLableSettingDef>();

                   lst_Samplea = iSamplea.GetSampleaList();
                   // lst_Samplea.Add("12");
                   // lst_Samplea.Add("34");
                   if (__g_list.Count > 0)
                   {
                       foreach (OfflineLableSettingDef xx in __g_list)
                       {
                           __g_list.Remove(xx);
                       }
                   }
                   foreach (OfflineLableSettingDef temp in lst_Samplea)
                   {
                       __g_list.Add(temp);
                   }

            if (lst_Samplea != null && lst_Samplea.Count != 0)
            {
                initControl(lst_Samplea);
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
        initSamplea();
        up.Update();
    }

    public void clearContent()
    {
        this.drpSamplea.Items.Clear();
        drpSamplea.Items.Add(new ListItem("", ""));
        up.Update();
    }

    //private void initControl(IList<PdLineInfo> lstPDLine)
    //private void initControl(IList<OfflineLableSettingDef> lst_OfflineLabelPrint)
    private void initControl(IList<OfflineLableSettingDef> lst_Samplea)
    {
        this.drpSamplea.Items.Clear();
        this.drpSamplea.Items.Add(string.Empty);

        if (lst_Samplea != null)
        {
            //foreach (PdLineInfo temp in lstPDLine)
            foreach (OfflineLableSettingDef temp in lst_Samplea)
            {
                //ITC-1103-0001 Tong.Zhi-Yong 2010-01-12
                //ADD trim() -- Line取得有空格，过滤  ----如需匹配line的话，需要过滤空格
                //item = new ListItem(temp.id.Trim() + " " + temp.friendlyName.Trim(), temp.id.Trim());

                this.drpSamplea.Items.Add(temp.description);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpSamplea.SelectedIndex = index;
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
            return this.drpSamplea;
        }

    }

    public IList<OfflineLableSettingDef> __Content
    {
        get
        {
            return this.__g_list;
        }
    }
}
