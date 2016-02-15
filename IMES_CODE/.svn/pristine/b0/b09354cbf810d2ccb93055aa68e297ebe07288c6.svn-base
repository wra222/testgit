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

public partial class CommonControl_DataMaintain_CmbBomNodeType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IPartManager iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    IPartTypeManagerEx iPartTypeManagerEx = ServiceAgent.getInstance().GetMaintainObjectByName<IPartTypeManagerEx>(WebConstant.IPartTypeManagerEx);

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
                        drpBomNodeType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpBomNodeType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpBomNodeType.Width = Unit.Parse(width);
                }

                this.drpBomNodeType.CssClass = cssClass;
                this.drpBomNodeType.Enabled = enabled;

                if (enabled)
                {
                    initMaintainPartType();
                }
                else
                {
                    this.drpBomNodeType.Items.Add(new ListItem("", ""));
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
          //  IList<PartTypeDef> lstMaintainPartType = null;
            IList<String> lstBomNode = null;
           lstBomNode= iPartTypeManagerEx.GetBomNodeTypeList();
         //   lstMaintainPartType = iSelectData.getPartNodeType("PType");
           // IList<String> lstDesc = new List<String>();
           if (lstBomNode != null && lstBomNode.Count > 0)
           {

               initControl(lstBomNode);
           }
           else
           {
               initControl(null);
           }
            //if (lstMaintainPartType != null && lstMaintainPartType.Count != 0)
            //{
            //    foreach (PartTypeDef ptd in lstMaintainPartType)
            //    {
            //        lstDesc.Add(ptd.code.ToString());
            //    }
            //    for (int i = 0; i < lstDesc.Count; i++)
            //    {
            //        for (int j = i + 1; j < lstDesc.Count; j++)
            //        {
            //            if (lstDesc[i].Equals(lstDesc[j]))
            //            {
            //                lstDesc.RemoveAt(j);
            //            }
            //        }
            //    }
            //    if (lstDesc.Count != 0)
            //    {
            //        initControl(lstDesc);
            //    }
            //}
            //else
            //{
            //    initControl(null);
            //}
        }
        else
        {
            initControl(null);
        }
    }

    public void refresh()
    {
        initMaintainPartType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpBomNodeType.Items.Clear();
        drpBomNodeType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<String> lstMaintainPartType)
    {
        ListItem item = null;
      

        this.drpBomNodeType.Items.Clear();
        this.drpBomNodeType.Items.Add(new ListItem("", ""));
        if (lstMaintainPartType != null)
        {
            foreach (String temp in lstMaintainPartType)
            {
                item = new ListItem(temp, temp);
                this.drpBomNodeType.Items.Add(item);
            }

        }
    }

    public void setSelected(int index)
    {
        this.drpBomNodeType.SelectedIndex = index;
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
            return this.drpBomNodeType;
        }

    }
}
