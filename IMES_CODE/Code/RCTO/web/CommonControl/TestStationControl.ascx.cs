 //Description: Test Station DropdownList
 //Update: 
 //Date         Name                Reason 
 //==========   ==================  =====================================    
 //2009-10-15   Lucy.Liu(EB1)  create
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
using System.Linq;



public partial class CommonControl_TestStationControl : System.Web.UI.UserControl
{
     //combobox宽度
    private String length = "300";

    //combobox样式
    private String css;
        
    //combobox初始化参数定义
    private String type;

    //combobox宽度是否支持百分比设定
    private Boolean isPercentage = false;

    //combobox是否可用,默认为可用
    private Boolean enabled = true;

    ITestStation iTestStation = ServiceAgent.getInstance().GetObjectByName<ITestStation>(WebConstant.CommonObject);

    //指明哪站用(SA,FA)
    private String station = "SA";
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
    /// 供使用者为combobox设定初始化参数
    /// </summary>
    public String Type
    {
       set
        {
            type = value;
        }
        get
        {
            return type;
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
    /// 为combobox控件添加哪站使用
    /// </summary>
    public String Station
    {
        get
        {
            return station;
        }
        set
        {
            station = value;
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

        

        if (iTestStation != null)
        {
            IList<TestStationInfo> lst = null;
            if (String.Equals(station, "SA", System.StringComparison.OrdinalIgnoreCase))
            {
                lst = iTestStation.GetSATestStationList();
            }
            else if (String.Equals(station, "SAForQ", System.StringComparison.OrdinalIgnoreCase))
            {
                lst = iTestStation.GetSATestStationList();
            }
            else
            {
                lst = iTestStation.GetFATestStationList();
            }
           

            if (lst != null && lst.Count != 0)
            {
                initControl(lst);
            }
        }
        //根据初始化参数获取内容()
        
        //this.DropDownList1.Items.Add(new ListItem(type + "1", type + "1"));
        //this.DropDownList1.Items.Add(new ListItem(type + "2", type + "2"));
      
             
    }

    private void initControl(IList<TestStationInfo> lst)
    {
        ListItem item = null;
        string showname = "";
        this.DropDownList1.Items.Clear();
        this.DropDownList1.Items.Add(new ListItem("", ""));

        //order by Station UC Modify 2012/0305
        var tmpstalist = from testitem in lst
                         orderby testitem.id
                         select testitem;

        if (String.Equals(station, "SAForQ", System.StringComparison.OrdinalIgnoreCase))
        {
            foreach (TestStationInfo temp in tmpstalist)
            {
                if (temp.id == "15")
                {
                    showname = temp.id.Trim() + " " + temp.friendlyName;
                    item = new ListItem(showname, temp.id);
                    this.DropDownList1.Items.Add(item);
                }
            }
        }
        else
        {
            foreach (TestStationInfo temp in tmpstalist)
            {
                showname = temp.id.Trim() + " " + temp.friendlyName;
                item = new ListItem(showname, temp.id);
                this.DropDownList1.Items.Add(item);
            }
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

       
}
