/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CmbCheckItemTypeList
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

public partial class CommonControl_CmbCheckItemTypeList : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private string codetype;
    //private IConstValueMaintain iConstValueMaintain;
    //private IBomDescrMaintain iBomDescrMaintain;
    private ICheckItemTypeListMaintain iCheckItemTypeListMaintain;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private int firstNoNullIndex = 0;
    private Boolean isSelectFirstNotNull = false;

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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //iConstValueMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueMaintain>(WebConstant.ConstValueMaintainObject);
            iCheckItemTypeListMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<ICheckItemTypeListMaintain>(WebConstant.CheckItemTypeListObject);
            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpCheckItemTypeList.Width = Unit.Percentage(100);
                       
                    }
                    else
                    {
                        drpCheckItemTypeList.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpCheckItemTypeList.Width = Unit.Parse(width);
                }

                this.drpCheckItemTypeList.CssClass = cssClass;
                this.drpCheckItemTypeList.Enabled = enabled;
                
                if (enabled)
                {
                    initCodeType();
                }
                else
                {
                    this.drpCheckItemTypeList.Items.Add(new ListItem("", ""));
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
        if (iCheckItemTypeListMaintain != null)
        {
            IList<string> lstCheckItemTypeList = null;
            IList<string> query = null;

            lstCheckItemTypeList = iCheckItemTypeListMaintain.GetChechItemTypeList();
            //if (string.IsNullOrEmpty(PAQCStdRatioType))
            //{
            //    lstCheckItemTypeList = iBomDescrMaintain.GetChechItemTypeList();

                
            //    query = (from q in lstCheckItemTypeList
            //                                   orderby q.value
            //                                   select q).ToList();
            //}
            //else
            //{
            //    lstCheckItemTypeList = iConstValueMaintain.GetCheckItemTypeListList();
            //    string[] arry = PAQCStdRatioType.Split(',');
            //    query = (from q in lstCheckItemTypeList
            //                                   where arry.Contains(q.value)
            //                                   orderby q.value
            //                                   select q).ToList();
            //}
            if (lstCheckItemTypeList != null && lstCheckItemTypeList.Count != 0)
            {
                initControl(lstCheckItemTypeList);
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
        this.drpCheckItemTypeList.Items.Clear();
        drpCheckItemTypeList.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<string> lstCheckItemTypeList)
    {
        ListItem item = null;
        
        this.drpCheckItemTypeList.Items.Clear();
        

        if (lstCheckItemTypeList != null)
        {
            bool firstIndexGet = false;
            int i = 0;  //去掉空行
            foreach (string temp in lstCheckItemTypeList)
            {
                item = new ListItem();
                item.Value = temp;
                item.Text = temp;
                this.drpCheckItemTypeList.Items.Add(item);
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
        this.drpCheckItemTypeList.SelectedIndex = 0;
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
            return this.drpCheckItemTypeList;
        }

    }
}
