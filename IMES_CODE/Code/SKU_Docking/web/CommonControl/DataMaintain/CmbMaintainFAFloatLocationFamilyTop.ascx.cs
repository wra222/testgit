/*
 *issue
 * ITC-1361-0037   itc210012  2011-01-16
 * 
 */
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

public partial class CommonControl_DataMaintain_CmbMaintainFAFloatLocationFamilyTop : System.Web.UI.UserControl
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
                        drpFamilyTopList.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpFamilyTopList.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpFamilyTopList.Width = Unit.Parse(width);
                }

                this.drpFamilyTopList.CssClass = cssClass;
                this.drpFamilyTopList.Enabled = enabled;

                if (enabled)
                {
                    initFamilyList();
                }
                else
                {
                    this.drpFamilyTopList.Items.Add(new ListItem("", ""));
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

        if (iKitLoc != null)
        {
            IList<string> lstFamily = null;

            lstFamily = iKitLoc.GetAllFamilys();

            Console.Write(lstFamily.Count);
            this.drpFamilyTopList.Items.Clear();
            this.drpFamilyTopList.Items.Add("All");
            if (lstFamily != null && lstFamily.Count != 0)
            {
               
                foreach (string temp in lstFamily)
                {
                    this.drpFamilyTopList.Items.Add(temp);
                }
            }
            else
            {
                this.drpFamilyTopList.Items.Clear();
            }
        }
        else
        {
            this.drpFamilyTopList.Items.Clear();
        }
    }

    public void refresh()
    {
        initFamilyList();
        up.Update();
    }

    public void clearContent()
    {
        this.drpFamilyTopList.Items.Clear();
        drpFamilyTopList.Items.Add(new ListItem("", ""));
        up.Update();
    }

    public void setSelected(int index)
    {
        this.drpFamilyTopList.SelectedIndex = index;
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
            return this.drpFamilyTopList;
        }

    }
}
