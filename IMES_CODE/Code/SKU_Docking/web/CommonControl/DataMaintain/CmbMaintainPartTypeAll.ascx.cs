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

public partial class CommonControl_DataMaintain_CmbMaintainPartNodeType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IPartTypeAttribute iSelectData;
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IPartTypeAttribute>(WebConstant.PartTypeAttributeObjet);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainPartNodeType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainPartNodeType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainPartNodeType.Width = Unit.Parse(width);
                }

                this.drpMaintainPartNodeType.CssClass = cssClass;
                this.drpMaintainPartNodeType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainPartType();
                }
                else
                {
                    this.drpMaintainPartNodeType.Items.Add(new ListItem("", ""));
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

    public void initMaintainPartType()
    {

        if (iSelectData != null)
        {
            IList<PartTypeAttributeDef> lstMaintainPartType = null;

            lstMaintainPartType = iSelectData.GetPartTypeList();

            IList<String> lstPartType = new List<String>();

            if (lstMaintainPartType != null && lstMaintainPartType.Count != 0)
            {
                foreach (PartTypeAttributeDef ptd in lstMaintainPartType)
                {
                    lstPartType.Add(ptd.code.ToString());
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
        initMaintainPartType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainPartNodeType.Items.Clear();
        drpMaintainPartNodeType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<String> lstMaintainPartType)
    {
        ListItem item = null;

        this.drpMaintainPartNodeType.Items.Clear();
        this.drpMaintainPartNodeType.Items.Add(new ListItem("", ""));
        if (lstMaintainPartType != null)
        {
            foreach (String temp in lstMaintainPartType)
            {
                item = new ListItem(temp, temp);
                this.drpMaintainPartNodeType.Items.Add(item);
            }

        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainPartNodeType.SelectedIndex = index;
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
            return this.drpMaintainPartNodeType;
        }

    }
}
