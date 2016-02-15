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

public partial class CommonControl_DataMaintain_CmbMiantainDesc : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IPartManager iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string nodeType;
    public string NodeType
    {
        get { return nodeType; }
        set { nodeType = value; }
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IPartManager>(WebConstant.IPartManager);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drplstTypeDesc.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drplstTypeDesc.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drplstTypeDesc.Width = Unit.Parse(width);
                }

                this.drplstTypeDesc.CssClass = cssClass;
                this.drplstTypeDesc.Enabled = enabled;

                if (enabled)
                {
                    initMaintainDescType();
                }
                else
                {
                    this.drplstTypeDesc.Items.Add(new ListItem("", ""));
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

    public void initMaintainDescType()
    {
        if (iSelectData != null)
        {
            IList<DescTypeInfo> lstMaintainDescType = null;
            IList<String> lstPartType = new List<String>();
            if (nodeType != null)
            {
                lstMaintainDescType = iSelectData.getDescriptionList(nodeType);


                if (lstMaintainDescType != null && lstMaintainDescType.Count != 0)
                {
                    foreach (DescTypeInfo ptd in lstMaintainDescType)
                    {
                        lstPartType.Add(ptd.description.ToString());
                    }
                    for (int i = 0; i < lstPartType.Count; i++)
                    {
                        for (int j = i + 1; j < lstPartType.Count; j++)
                        {
                            if (lstPartType[i].Equals(lstPartType[j]))
                            {
                                lstPartType.RemoveAt(j);
                            }
                        }
                    }
                    if (lstPartType.Count != 0)
                    {
                        initControl(lstPartType);
                    }


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
    }

    public void refresh()
    {
        initMaintainDescType();
        up.Update();
    }

    public void clearContent()
    {
        this.drplstTypeDesc.Items.Clear();
        drplstTypeDesc.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<String> lstMaintainDescType)
    {
        ListItem item = null;

        this.drplstTypeDesc.Items.Clear();
        item = new ListItem("", "");
        this.drplstTypeDesc.Items.Add(item);
        if (lstMaintainDescType != null)
        {
            foreach (String temp in lstMaintainDescType)
            {
                item = new ListItem(temp, temp);
                this.drplstTypeDesc.Items.Add(item);
            }

        }
    }

    public void setSelected(int index)
    {
        this.drplstTypeDesc.SelectedIndex = index;
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
            return this.drplstTypeDesc;
        }

    }
}
