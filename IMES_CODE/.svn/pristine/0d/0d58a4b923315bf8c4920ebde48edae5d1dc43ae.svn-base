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

public partial class CommonControl_CmbMaintainModel : System.Web.UI.UserControl
{

    private string cssClass;
    private string width;
    //private string station;
    //private string MaintainModel;
    private IModelManager iSelectData;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string family;

    public string Family
    {
        get { return family; }
        set { family = value; }
    }

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

    //public string MaintainModel
    //{
    //    get { return MaintainModel; }
    //    set { MaintainModel = value; }
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
            iSelectData = ServiceAgent.getInstance().GetMaintainObjectByName<IModelManager>(WebConstant.IModelManager);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpMaintainModel.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpMaintainModel.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpMaintainModel.Width = Unit.Parse(width);
                }

                this.drpMaintainModel.CssClass = cssClass;
                this.drpMaintainModel.Enabled = enabled;

                if (enabled)
                {
                    initMaintainModel();
                }
                else
                {
                    this.drpMaintainModel.Items.Add(new ListItem("", ""));
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

    public void initMaintainModel()
    {

        if (iSelectData != null && family != null && family != "")
        {
            IList<ModelMaintainInfo> lstMaintainModel = null;

            lstMaintainModel = iSelectData.GetModelList(family);

            //Console.Write(lstMaintainModel.Count);
            if (lstMaintainModel != null && lstMaintainModel.Count != 0)
            {
                initControl(lstMaintainModel);
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
        initMaintainModel();
        up.Update();
    }

    public void clearContent()
    {
        this.drpMaintainModel.Items.Clear();
        drpMaintainModel.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<ModelMaintainInfo> lstMaintainModel)
    {
        ListItem item = null;
        this.drpMaintainModel.Items.Clear();

        if (lstMaintainModel != null && lstMaintainModel.Count > 0)
        {
            item = new ListItem("", "");
            this.drpMaintainModel.Items.Add(item);

            foreach (ModelMaintainInfo temp in lstMaintainModel)
            {
                //item = new ListItem(temp.id + " " + temp.friendlyName, temp.id);
                item = new ListItem(temp.Model, temp.Model);
                this.drpMaintainModel.Items.Add(item);
            }

        }
        else
        {
            this.drpMaintainModel.Items.Add(string.Empty);
        }
    }

    public void setSelected(int index)
    {
        this.drpMaintainModel.SelectedIndex = index;
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
            return this.drpMaintainModel;
        }

    }
}

