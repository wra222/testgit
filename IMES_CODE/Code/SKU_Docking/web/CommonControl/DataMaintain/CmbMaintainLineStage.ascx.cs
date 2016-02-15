﻿using System;
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

public partial class CommonControl_DataMaintain_CmbMaintainLineStage : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainLineStage;
    private ILine iSelectData;
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

    //public string MaintainLineStage
    //{
    //    get { return MaintainLineStage; }
    //    set { MaintainLineStage = value; }
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<ILine>(WebConstant.MaintainLineObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainLineStage.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainLineStage.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainLineStage.Width = Unit.Parse(width);
                }

                this.drpMaintainLineStage.CssClass = cssClass;
                this.drpMaintainLineStage.Enabled = enabled;

                if (enabled)
                {
                    initMaintainLineStage();
                }
                else
                {
                    this.drpMaintainLineStage.Items.Add(new ListItem("", ""));
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

    public void initMaintainLineStage()
    {

        if (iSelectData != null )
        {
            IList<SelectInfoDef> lstMaintainLineStage = null;

            lstMaintainLineStage = iSelectData.GetStageList();

            //Console.Write(lstMaintainLineStage.Count);
            if (lstMaintainLineStage != null && lstMaintainLineStage.Count != 0)
            {
                initControl(lstMaintainLineStage);
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
        initMaintainLineStage();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainLineStage.Items.Clear();
        drpMaintainLineStage.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<SelectInfoDef> lstMaintainLineStage)
    {
        ListItem item = null;

        this.drpMaintainLineStage.Items.Clear();
        //this.drpMaintainLineStage.Items.Add(string.Empty);

        if (lstMaintainLineStage != null)
        {
            foreach (SelectInfoDef temp in lstMaintainLineStage)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Text, temp.Value);
                this.drpMaintainLineStage.Items.Add(item);
            }

        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainLineStage.SelectedIndex = index;
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
            return this.drpMaintainLineStage;
        }

    }
}

