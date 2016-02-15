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

public partial class CommonControl_cmbMaintainDept : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    private IDept iDept;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

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
                        drpMaintainDept.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainDept.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainDept.Width = Unit.Parse(width);
                }

                this.drpMaintainDept.CssClass = cssClass;
                this.drpMaintainDept.Enabled = enabled;

                if (enabled)
                {
                    initMaintainDept();
                }
                else
                {
                    this.drpMaintainDept.Items.Add(new ListItem("", ""));
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

    public void initMaintainDept()
    {

        if (iDept != null)
        {
            IList<string> lstMaintainDept = null;

            lstMaintainDept = iDept.GetDeptList();

            if (lstMaintainDept != null && lstMaintainDept.Count != 0)
            {
                initControl(lstMaintainDept);
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
        initMaintainDept();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainDept.Items.Clear();
        drpMaintainDept.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstMaintainDept)
    {
        ListItem item = null;

        this.drpMaintainDept.Items.Clear();
        //this.drpMaintainDept.Items.Add(string.Empty);
        //Value=@Defect Code，Text=@DefectCode +空格+@Descr
        if (lstMaintainDept != null)
        {
            foreach (string temp in lstMaintainDept)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp);
                this.drpMaintainDept.Items.Add(item);
            }
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainDept.SelectedIndex = index;
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
            return this.drpMaintainDept;
        }

    }
}

