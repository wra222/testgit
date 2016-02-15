/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbConstValueType
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1              
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-8-6     Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System;
using System.Collections;
using System.Collections.Generic;
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
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;

public partial class CommonControl_CmbConstValueType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private string codetype;
    private IConstValueMaintain iConstValueMaintain;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private int firstNoNullIndex = 0;
    private Boolean isSelectFirstNotNull = false;
    private string PAQCStdRatioType;

    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public string Station
    {
        get { return station; }
        set { station = value; }
    }

    public string Customer
    {
        get { return customer; }
        set { customer = value; }
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

    public Boolean IsSelectFirstNotNull
    {
        get { return isSelectFirstNotNull; }
        set { isSelectFirstNotNull = value; }
    }

    public string PAQCStdRatio
    {
        get { return PAQCStdRatioType; }
        set { PAQCStdRatioType = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iConstValueMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueMaintain>(WebConstant.ConstValueMaintainObject);
            
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpConstValueType.Width = Unit.Percentage(100);
                       
                    }
                    else
                    {
                        drpConstValueType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpConstValueType.Width = Unit.Parse(width);
                }

                this.drpConstValueType.CssClass = cssClass;
                this.drpConstValueType.Enabled = enabled;
                
                if (enabled)
                {
                    initCodeType();
                }
                else
                {
                    this.drpConstValueType.Items.Add(new ListItem("", ""));
                }

                firstNoNullIndex = 0;
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

    public void initCodeType()
    {
        if (iConstValueMaintain != null)
        {
            IList<ConstValueInfo> lstConstValueType = null;
            IList<ConstValueInfo> query = null;
            if (string.IsNullOrEmpty(PAQCStdRatioType))
            {
                lstConstValueType = iConstValueMaintain.GetConstValueTypeList();
                query = (from q in lstConstValueType
                                               orderby q.value
                                               select q).ToList();
            }
            else
            {
                lstConstValueType = iConstValueMaintain.GetConstValueTypeList();
                string[] arry = PAQCStdRatioType.Split(',');
                query = (from q in lstConstValueType
                                               where arry.Contains(q.value)
                                               orderby q.value
                                               select q).ToList();
            }
            if (query != null && query.Count != 0)
            {
                initControl(query);
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
        initCodeType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpConstValueType.Items.Clear();
        drpConstValueType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<ConstValueInfo> lstConstValueType)
    {
        ListItem item = null;
        
        this.drpConstValueType.Items.Clear();
        this.drpConstValueType.Items.Add(string.Empty);

        if (lstConstValueType != null)
        {
            bool firstIndexGet = false;
            int i = 1;  //去掉空行
            foreach (ConstValueInfo temp in lstConstValueType)
            {
                item = new ListItem();
                item.Value = temp.name;
                item.Text = temp.value;
                this.drpConstValueType.Items.Add(item);
                if ((firstIndexGet == false))
                //if ((firstIndexGet == false) && (!string.IsNullOrEmpty(temp)))
                {
                    firstNoNullIndex = i;
                    firstIndexGet = true;
                }

                i++;
            }

            if (isSelectFirstNotNull == true)
            {
                setSelected(firstNoNullIndex);
            }
        }

    }

    //private void initControl2(string[] arrylist)
    //{
    //    ListItem item = null;

    //    this.drpConstValueType.Items.Clear();
    //    this.drpConstValueType.Items.Add(string.Empty);

    //    if (arrylist != null)
    //    {
    //        bool firstIndexGet = false;
    //        int i = 1;  //去掉空行
    //        foreach (string temp in arrylist)
    //        {
    //            item = new ListItem();
    //            item.Value = temp;
    //            item.Text = temp;
    //            this.drpConstValueType.Items.Add(item);
    //            if ((firstIndexGet == false))
    //            //if ((firstIndexGet == false) && (!string.IsNullOrEmpty(temp)))
    //            {
    //                firstNoNullIndex = i;
    //                firstIndexGet = true;
    //            }

    //            i++;
    //        }

    //        if (isSelectFirstNotNull == true)
    //        {
    //            setSelected(firstNoNullIndex);
    //        }
    //    }

    //}

    public void setSelected(int index)
    {
        this.drpConstValueType.SelectedIndex = 0;
        up.Update();
    }

    /*
    public int getFirstNoNullIndex()
    {
        return firstNoNullIndex;
    }
    */

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
            return this.drpConstValueType;
        }

    }
}
