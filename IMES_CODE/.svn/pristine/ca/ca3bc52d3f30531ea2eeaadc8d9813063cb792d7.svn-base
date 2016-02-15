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

public partial class CommonControl_DataMaintain_CmbMaintainFAFloatLocationPdLine : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    //private string station;
    //private string customer;
    private IFAFloatLocation iKitLoc;
    private Boolean isPercentage = false;
    private Boolean enabled = true;

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    //public string Station
    //{
    //    get { return station; }
    //    set { station = value; }
    //}

    //public string Customer
    //{
    //    get { return customer; }
    //    set { customer = value; }
    //}

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
            iKitLoc = ServiceAgent.getInstance().GetMaintainObjectByName<IFAFloatLocation>(WebConstant.FAFloatLocationObjet);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpPdLineList.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpPdLineList.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpPdLineList.Width = Unit.Parse(width);
                }

                this.drpPdLineList.CssClass = cssClass;
                this.drpPdLineList.Enabled = enabled;

                if (enabled)
                {
                    initFamilyList();
                }
                else
                {
                    this.drpPdLineList.Items.Add(new ListItem("", ""));
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

    public void initFamilyList()
    {
        //*********************
        if (iKitLoc != null)
        {
            //IList<string> lstFamily = null;

            IList<LineInfo> lstFamily = iKitLoc.GetAllPdLines();
            List<String> lineStr = new List<String>();
            Console.Write(lstFamily.Count);
            if (lstFamily != null && lstFamily.Count != 0)
            {
               this.drpPdLineList.Items.Clear();
               foreach (LineInfo temp in lstFamily)
                {
                    string LineItem = temp.line.Substring(0, 1);
                    if (lineStr.Contains(LineItem))
                    {
                        continue;
                    }
                    lineStr.Add(LineItem);
                    ListItem item = new ListItem(LineItem, LineItem);
                    this.drpPdLineList.Items.Add(item);
                }
            }
            else
            {
                this.drpPdLineList.Items.Clear();
            }
        }
        else
        {
            this.drpPdLineList.Items.Clear();
        }
    }

    public void refresh()
    {
        initFamilyList();
        up.Update();
    }

    public void clearContent()
    {
        this.drpPdLineList.Items.Clear();
        drpPdLineList.Items.Add(new ListItem("", ""));
        up.Update();
    }

    public void setSelected(int index)
    {
        this.drpPdLineList.SelectedIndex = index;
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
            return this.drpPdLineList;
        }

    }
}
