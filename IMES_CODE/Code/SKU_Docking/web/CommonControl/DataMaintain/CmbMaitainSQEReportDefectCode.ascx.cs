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

public partial class CommonControl_DataMaintain_CmbMaitainSQEReport : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private ISQEDefectReport isqeReport;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string tp;

    public string Tp
    {
        get { return tp; }
        set { tp = value; }
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
            isqeReport = (ISQEDefectReport)ServiceAgent.getInstance().GetMaintainObjectByName<IPartManager>(WebConstant.SQEDefectReportManager);
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        SQAReportUserControl1.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        SQAReportUserControl1.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    SQAReportUserControl1.Width = Unit.Parse(width);
                }

                this.SQAReportUserControl1.CssClass = cssClass;
                this.SQAReportUserControl1.Enabled = enabled;

                if (enabled)
                {
                    initSQEReport();
                }
                else
                {
                    this.SQAReportUserControl1.Items.Add(new ListItem("", ""));
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

    public void initSQEReport()
    {
        if (isqeReport != null && this.Tp != null)
        {
            IList<DefectCodeInfo> lstDefectInfo = null;

            lstDefectInfo = isqeReport.GetDefecSQEDefectInfo(this.Tp);

            if (lstDefectInfo != null && lstDefectInfo.Count != 0)
            {
                initControl(lstDefectInfo);
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
        initSQEReport();
        up.Update();
    }

    public void clearContent()
    {
        this.SQAReportUserControl1.Items.Clear();
        SQAReportUserControl1.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<DefectCodeInfo> lstDefectInfo)
    {
        ListItem item = null;

        this.SQAReportUserControl1.Items.Clear();
        this.SQAReportUserControl1.Items.Add(new ListItem("",""));
        if (lstDefectInfo != null)
        {
            foreach (DefectCodeInfo temp in lstDefectInfo)
            {
                item = new ListItem(temp.Defect.Trim() + " " + temp.Descr.Trim(),temp.Defect.Trim());
                this.SQAReportUserControl1.Items.Add(item);
            }

        }

    }

    public void setSelected(int index)
    {
        this.SQAReportUserControl1.SelectedIndex = index;
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
            return this.SQAReportUserControl1;
        }

    }
}


