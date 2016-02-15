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
using System.Collections.Generic;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Maintain.Interface;
using IMES.Infrastructure;
using com.inventec.iMESWEB;


public partial class CommonControl_DataMaintain_CmbStagePDLine : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string stage;
    private ILine iPDLine;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public string Stage
    {
        get { return stage; }
        set { stage = value; }
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
    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpMaintainPdLine;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iPDLine = (ILine)ServiceAgent.getInstance().GetMaintainObjectByName<ILine>(WebConstant.MaintainLineObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(Width) > 100)
                    {
                        drpMaintainPdLine.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainPdLine.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainPdLine.Width = Unit.Parse(width);
                }

                this.drpMaintainPdLine.CssClass = cssClass;
                this.drpMaintainPdLine.Enabled = enabled;

                if (enabled)
                {
                    initMaintainPdLine();
                }
                else
                {
                    this.drpMaintainPdLine.Items.Add(new ListItem("", ""));
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

    public void initMaintainPdLine()
    {
        if (iPDLine != null)
        {
            DataTable lstMaintainPdLine = null;
            lstMaintainPdLine = iPDLine.GetLineInfoList("HP", "PAK");

            if (lstMaintainPdLine != null && lstMaintainPdLine.Rows.Count > 0)
            {
                initControl(lstMaintainPdLine);
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
        initMaintainPdLine();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainPdLine.Items.Clear();
        drpMaintainPdLine.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(DataTable lstMaintainPdLine)
    {
        ListItem item = null;
        this.drpMaintainPdLine.Items.Clear();

        this.drpMaintainPdLine.Items.Add(new ListItem("ALL", "ALL"));
        List<string> lineStr = new List<string>();
        if (lstMaintainPdLine != null)
        {
            for (int i = 0; i < lstMaintainPdLine.Rows.Count; i++)
            {
                string lineFirstChar = lstMaintainPdLine.Rows[i]["Line"].ToString().Trim().Substring(0,1);
                if (lineStr.Contains(lineFirstChar))
                {
                    continue;
                }
                lineStr.Add(lineFirstChar);
                item = new ListItem(lineFirstChar, lineFirstChar);
                this.drpMaintainPdLine.Items.Add(item);
            }
        }

    }

    public void setSelected(int index)
    {
        this.drpMaintainPdLine.SelectedIndex = index;
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
