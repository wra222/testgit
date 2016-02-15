 //Description: Family DropdownList
 //Update: 
 //Date         Name                Reason 
 //==========   ==================  =====================================    
 //2009-10-15   207006              create
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

public partial class CommonControl_causeControl : System.Web.UI.UserControl
{
     //combobox宽度
    private String length = "300";

    //combobox样式
    private String css;
    private String _customer = "";
    private String _stage = "";

    //combobox宽度是否支持百分比设定
    private Boolean isPercentage = false;
    private ICause icauseService = ServiceAgent.getInstance().GetObjectByName<ICause>(WebConstant.CommonObject);

    //combobox是否可用,默认为可用
    private Boolean enabled = true;
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
            
            //初始化combobox的内容
            initDropReason();
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

    public String Customer
    {
        get
        {
            return _customer ;
        }
        set
        {
            _customer  = value;
        }
    }

    public String Stage
    {
        get
        {
            return _stage;
        }
        set
        {
            _stage = value;
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
        DropDownList1.Items.Add(new ListItem("", ""));
        this.up.Update();
       
    }
    
    /// <summary>
    /// 初始化combobox的内容
    /// </summary>
    /// <returns></returns>

    protected void initDropReason()
    {
        if (String.IsNullOrEmpty(Stage))
            Stage = "FA";

        DropDownList1.Items.Add(new ListItem("", ""));
        IList<CauseInfo> causeList = this.icauseService.GetCauseList (Customer, Stage);
        if (!(causeList == null) && (causeList.Count > 0))
        {
            foreach (CauseInfo obliItem in causeList)
            {
                DropDownList1.Items.Add(new ListItem(obliItem.id+" "+obliItem.friendlyName, obliItem.id));
            }
        }
      
    }

    /// <summary>
    /// 根据index为combobox置高亮选项
    /// </summary>
    /// <param name="index"></param>
    public void setSelected(int index)
    {
        this.DropDownList1.SelectedIndex = index;
        up.Update();
    }      
}
