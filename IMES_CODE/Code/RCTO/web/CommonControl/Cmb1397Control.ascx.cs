 //Description: 1397 DropdownList
 //Update: 
 //Date         Name                Reason 
 //==========   ==================  =====================================    
 //2009-10-15   Lucy.Liu(EB1)  create
 //Known issues:Any restrictions about this file
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic ;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IMES.Station.Interface.CommonIntf;
using com.inventec.iMESWEB;
using IMES.DataModel;


public partial class CommonControl_Cmb1397Control : System.Web.UI.UserControl
{
     //combobox宽度
    private String length = "300";

    //combobox样式
    private String css;

    //combobox宽度是否支持百分比设定
    private Boolean isPercentage = false;

    //combobox是否可用,默认为可用
    private Boolean enabled = true;
    private string _service = string.Empty;

    //private I1397Level i1397Level = (I1397Level)ServiceAgent.getInstance().GetObjectByName(WebConstant.I1397LEVEL_SERVICE);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

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
            this.DropDownList1.CssClass = css;
            this.DropDownList1.Enabled = enabled;
            if (enabled)
            {
                //初始化combobox的内容
                initDropReason();
            }
            else
            {
                this.DropDownList1.Items.Add(new ListItem("", ""));
            }

         

           
        }
     
    }

    /// <summary>
    /// 为combobox控件添加service属性，供使用者直接赋值
    /// </summary>
    public String Service
    {

        set
        {
            _service = value;
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
       //联动combobox初始化内容为空
        DropDownList1.Items.Add(new ListItem("", ""));
      
    }
   
    /// <summary>
    /// 重新更新combobox的内容
    /// </summary>
    /// <param name="family"></param>
    public void refreshDropContent(String family)
    {
        //首先清空combobox内容
        DropDownList1.Items.Clear();
        DropDownList1.Items.Add(new ListItem("", ""));

        I1397Level i1397Level = null;
        if (_service == "008")
        {
            i1397Level = ServiceAgent.getInstance().GetObjectByName<I1397Level>(WebConstant.Print1397LabelObject);
        }
        else
        {
            i1397Level = ServiceAgent.getInstance().GetObjectByName<I1397Level>(WebConstant.CommonObject);

        }

        //根据联动参数获取内容
        if (i1397Level != null)
        {
            IList<_1397LevelInfo> lst1397 = i1397Level.Get1397LevelList(family);

            foreach (_1397LevelInfo _1397 in lst1397)
            {
                DropDownList1.Items.Add(new ListItem(_1397.friendlyName, _1397.id));
            }
        }
        ////如下代码是测试用的，使用时请删除
        //DropDownList1.Items.Add(new ListItem(family + "1", family + "1"));
        //DropDownList1.Items.Add(new ListItem(family + "2", family + "2"));
        //DropDownList1.Items.Add(new ListItem(family + "3", family + "3"));
        
        //刷新update panel
        up.Update();


    }

    public void setSelected(int index)
    {
        this.DropDownList1.SelectedIndex = index;
        up.Update();
    }

       
}
