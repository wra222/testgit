//Description: Light Code DropdownList
//Update: 
//Date         Name                Reason 
//==========   ==================  =====================================    
//2011-12-21   itc202017           create
//Known issues:Any restrictions about this file
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.DataModel;



public partial class CommonControl_CmbLightCodeControl : System.Web.UI.UserControl
{
    //combobox宽度
    private String length = "300";

    //combobox样式
    private String css;


    //combobox宽度是否支持百分比设定
    private Boolean isPercentage = false;

    //combobox是否可用,默认为可用
    private Boolean enabled = true;

    ILightCode iLightCode = ServiceAgent.getInstance().GetObjectByName<ILightCode>(WebConstant.CommonObject);

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //设定高度
            if (isPercentage)
            {
                if (Convert.ToInt32(length) > 100)
                {
                    DropDownList1.Width = Unit.Percentage(100);
                }
                else
                {
                    DropDownList1.Width = Unit.Percentage(Convert.ToDouble(length));
                }
            }
            else
            {
                DropDownList1.Width = Unit.Parse(length);
            }

            DropDownList1.CssClass = css;
            this.DropDownList1.Enabled = enabled;
            if (enabled)
            {
                initDropReason();
            }
            else
            {
                this.DropDownList1.Items.Add(new ListItem("", ""));
            }
        }

    }

    /// <summary>
    /// 为combobox控件添加Width属性，供使用者直接赋值
    /// </summary>
    public String Width
    {
        get
        {
            return length;
        }
        set
        {
            length = value;
        }


    }

    /// <summary>
    /// 为combobox控件添加CssClass属性，供使用者直接赋值
    /// </summary>
    public String CssClass
    {
        get
        {
            return css;
        }
        set
        {
            css = value;
        }


    }

    /// <summary>
    /// 为combobox控件添加Enabled属性，供使用者直接赋值
    /// </summary>
    public Boolean Enabled
    {
        get
        {
            return enabled;
        }
        set
        {
            enabled = value;
        }


    }

  

    /// <summary>
    /// 为combobox控件添加IsPercentage属性，供使用者直接赋值
    /// </summary>
    public Boolean IsPercentage
    {
        get
        {
            return isPercentage;
        }
        set
        {
            isPercentage = value;
        }


    }

    /// <summary>
    /// 返回该用户控件中定义的DropDownList对象
    /// </summary>
    public DropDownList InnerDropDownList
    {
        get
        {
            return this.DropDownList1;
        }

    }


    /// <summary>
    /// 清空combobox内容
    /// </summary>
    public void clearContent()
    {

        //清空combobox内容
        this.DropDownList1.Items.Clear();
        this.DropDownList1.Items.Add(new ListItem("", ""));
        this.up.Update();

    }

    /// <summary>
    /// 初始化combobox的内容
    /// </summary>
    /// <returns></returns>

    protected void initDropReason()
    {

        this.DropDownList1.Items.Clear();
        this.DropDownList1.Items.Add(new ListItem("", ""));

        if (iLightCode != null)
        {
            IList<string> lst = iLightCode.GetLightCodeList();
           
            if (lst != null && lst.Count != 0)
            {
                initControl(lst);
            }
        }
   

    }

    private void initControl(IList<string> lst)
    {


        foreach (string temp in lst)
        {
            this.DropDownList1.Items.Add(new ListItem(temp));
        }
    }

    /// <summary>
    /// 为combobox置高亮项
    /// </summary>
    /// <param name="index"></param>
    public void setSelected(int index)
    {
        this.DropDownList1.SelectedIndex = index;
        up.Update();
    }

    /// <summary>
    /// 重新更新combobox的内容
    /// </summary>
    
    public void refreshDropContent()
    {

        initDropReason();    
        up.Update();
    }


}
