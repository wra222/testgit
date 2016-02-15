/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Cmb111Level
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-12   Tong.Zhi-Yong     Create 
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

public partial class CommonControl_Cmb111Level : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private I111Level i111Level;
    private string mb_code;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
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

    public string CssClass
    {
        get { return cssClass; }
        set { cssClass = value; }
    }

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public string Mb_code
    {
        get { return mb_code; }
        set { mb_code = value; }
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
            i111Level = ServiceAgent.getInstance().GetObjectByName<I111Level>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.drp111Level.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drp111Level.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drp111Level.Width = Unit.Parse(width);
                }

                this.drp111Level.CssClass = cssClass;
                this.drp111Level.Enabled = enabled;

                if (enabled)
                {
                    this.init111Level(mb_code);
                }
                else
                {
                    this.drp111Level.Items.Add(new ListItem("", ""));
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

    public void init111Level(string mbCode)
    {
        if (i111Level != null)
        {
            IList<_111LevelInfo> lst111Level = null;
            //add by zhulei
            if (_service == "002")
            {
                lst111Level = i111Level.GetPartNoListByInfo("MB", "MB", mbCode);
            }
            else if (isDataFilter)
            {
                lst111Level = i111Level.Get111LevelListExceptPrinted(mbCode);
            }
            else
            {
                lst111Level = i111Level.Get111LevelList(mbCode);
            }

            if (lst111Level != null && lst111Level.Count != 0)
            {
                initControl(lst111Level);
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

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drp111Level;
        }
    }

    public void refresh()
    {
        init111Level(mb_code);
        up.Update();
    }

    public void refresh(string paraMbCode)
    {
        init111Level(paraMbCode);
        up.Update();
    }

    public void clearContent()
    {
        this.drp111Level.Items.Clear();
        drp111Level.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<_111LevelInfo> lst111Level)
    {
        ListItem item = null;

        this.drp111Level.Items.Clear();
        this.drp111Level.Items.Add(string.Empty);

        if (lst111Level != null)
        {
            foreach (_111LevelInfo temp in lst111Level)
            {
                item = new ListItem(temp.friendlyName, temp.id);

                this.drp111Level.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drp111Level.SelectedIndex = index;
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
