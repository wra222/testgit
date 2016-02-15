/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbDismantleType
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-09   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using System.Text;

public partial class CommonControl_CmbDismantleType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private IDismantleType iDismantleType;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private string dismantletype;

    public string Width
    {
        get { return width; }
        set { width = value; }
    }
     public string DismantleType
    {
        get { return dismantletype; }
        set { dismantletype = value; }
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
    /// <summary>
    /// 返回该用户控件中定义的DropDownList对象
    /// </summary>
    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpDismantelType;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iDismantleType = ServiceAgent.getInstance().GetObjectByName<IDismantleType>(WebConstant.CommonObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        this.drpDismantelType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpDismantelType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpDismantelType.Width = Unit.Parse(width);
                }

                this.drpDismantelType.CssClass = cssClass;
                this.drpDismantelType.Enabled = enabled;

                if (enabled)
                {
                    this.initDismantleType();
                }
                else
                {
                    this.drpDismantelType.Items.Add(new ListItem("", ""));
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
    
    public void initDismantleType()
    {
        //this.drpDismantelType.Items.Clear();
        if (iDismantleType != null)
        {
            IList<ConstValueInfo> lstDismantleType = iDismantleType.GetDismantleTypeList(dismantletype);

            if ((lstDismantleType != null) && (lstDismantleType.Count != 0))
            {
                initControl(lstDismantleType);
            }
        }
         
        //initControl(null);
    }

    public void refresh()
    {
        initDismantleType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpDismantelType.Items.Clear();
        drpDismantelType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<ConstValueInfo> lstDismantleType)
    {
        ListItem item = null;
        int i;
        int selindex=0;
        this.drpDismantelType.Items.Clear();
        
        
        //drpDismantelType.Items.Add(new ListItem("", ""));
          foreach (ConstValueInfo temp in lstDismantleType)
        {
            item = new ListItem(temp.name, temp.value);
             
            //this.drpDismantelType.Items.Add(item);
        }
         
       
        for (i = lstDismantleType.Count-1; i >=0 ; i--)
        {
            item = new ListItem(lstDismantleType[i].name, lstDismantleType[i].value);
            this.drpDismantelType.Items.Add(item);
            
            if (((lstDismantleType[i].name.ToUpper() == "PRODUCT") || (lstDismantleType[i].value.ToUpper() == "PRODUCT")) && (dismantletype == "DismantleType"))
            {
                this.drpDismantelType.SelectedIndex = selindex;
            }
            selindex = selindex + 1;
        }
    }

    public void setSelected(int index)
    {
        this.drpDismantelType.SelectedIndex = index;
        up.Update();
    }
    public ListItem getSelectedItem()
    {
        return this.drpDismantelType.SelectedItem;
    }

    private void showCmbErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\0013", string.Empty).Replace("\0010", "\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.up, typeof(System.Object), "showCmbErrorMessage", scriptBuilder.ToString(), false);
    }
    protected void drpDismantelType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
