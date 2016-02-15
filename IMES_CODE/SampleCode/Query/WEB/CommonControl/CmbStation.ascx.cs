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
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
public partial class CommonControl_CmbStation : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string defaultValue;
    private List<string> stationTypeList;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
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
    public List<string> StationTypeList
    {
        get { return stationTypeList; }
        set { stationTypeList = value; }
    }
    public string DefaultValue
    {
        get { return defaultValue; }
        set { defaultValue = value; }
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
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpStation.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpStation.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpStation.Width = Unit.Parse(width);
                }

                this.drpStation.CssClass = cssClass;
                this.drpStation.Enabled = enabled;

                if (enabled)
                {
                    initPDStation();
                }
                else
                {
                    this.drpStation.Items.Add(new ListItem("", ""));
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

    public void initPDStation()
    {
        if (QueryCommon != null)
        {
    
          
            //DataTable dt = QueryCommon.GetStation(stationTypeList,DBCo);
            DataTable dt = new DataTable();
             if (dt.Rows.Count == 0)
             { initControl(null); ;}
             else
             {

                 initControl(dt);
             }
      
        }
        else
        {
            initControl(null);
        }
    }

    public void refresh()
    {
        initPDStation();
        up.Update();
    }

    
    public void setAutoPostBack(bool isAutoPostBack)
    {
        this.drpStation.AutoPostBack = isAutoPostBack;
    }
    public void clearContent()
    {
        this.drpStation.Items.Clear();
        drpStation.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(DataTable dt)
    {
        ListItem item = null;

        this.drpStation.Items.Clear();
        this.drpStation.Items.Add(string.Empty);
       
        if (dt != null && dt.Rows.Count>0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                item = new ListItem(dr["Station"].ToString().Trim() + " " + dr["Name"].ToString().Trim(), dr["Station"].ToString().Trim());

                this.drpStation.Items.Add(item);
            }
            if (!string.IsNullOrEmpty(defaultValue))
            { this.drpStation.SelectedValue = defaultValue; }
           
        }

    }

    public void setSelected(int index)
    {
        this.drpStation.SelectedIndex = index;
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
            return this.drpStation;
        }

    }
}
