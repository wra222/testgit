using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using System.Text;

public partial class CommonControl_cmbMaintainSection : System.Web.UI.UserControl
{

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string dept;
    private string section;
    private string cssClass;
    private string width;
    private IDept iDept;

    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public string Dept
    {
        get { return dept; }
        set { dept = value; }
    }

    public string Section
    {
        get { return section; }
        set { section = value; }
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
            iDept = ServiceAgent.getInstance().GetMaintainObjectByName<IDept>(WebConstant.MaintainDeptObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainSection.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainSection.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainSection.Width = Unit.Parse(width);
                }

                this.drpMaintainSection.CssClass = cssClass;
                this.drpMaintainSection.Enabled = enabled;

                if (enabled)
                {
                    initMaintainSection();
                }
                else
                {
                    this.drpMaintainSection.Items.Add(new ListItem("", ""));
                }
            }
        }
        catch (FisException ex)
        {
            //showCmbErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            //showCmbErrorMessage(ex.Message);
        }
    }

    public void initMaintainSection()
    {

        if (iDept != null)
        {
            IList<string> lstMaintainSection = null;
            DeptInfo deptCondition = new DeptInfo();
            deptCondition.dept = dept;

            lstMaintainSection = iDept.GetSectionList(deptCondition);

            //Console.Write(lstMaintainLineStage.Count);
            if (lstMaintainSection != null && lstMaintainSection.Count != 0)
            {
                initControl(lstMaintainSection);
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
        initMaintainSection();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainSection.Items.Clear();
        drpMaintainSection.Items.Add(new ListItem("", ""));
        up.Update();
    }

    public void setSelected(int index)
    {
        this.drpMaintainSection.SelectedIndex = index;
        up.Update();
    }

    private void initControl(IList<string> lstMaintainLineSection)
    {
        ListItem item = null;

        this.drpMaintainSection.Items.Clear();
        this.drpMaintainSection.Items.Add(string.Empty);

        if (lstMaintainLineSection != null)
        {
            foreach (string temp in lstMaintainLineSection)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp);
                this.drpMaintainSection.Items.Add(item);
            }

        }
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
            return this.drpMaintainSection;
        }
    }
}

