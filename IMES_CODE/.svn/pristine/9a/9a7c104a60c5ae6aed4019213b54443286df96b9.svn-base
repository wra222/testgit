/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Component DropdownList
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-10-15   Chen Xu (EB1-4)     Create
 * 2010-01-12   Chen Xu (EB1-4)     Modify: ITC-1103-0044 
 * 
 * Known issues:Any restrictions about this file 
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
using com.inventec.iMESWEB;
using IMES.DataModel;

public partial class CommonControl_ComponentControl : System.Web.UI.UserControl
{
    private String length = "300";

    private String css;

    private String station;

    private Boolean isPercentage = false;

    private Boolean enabled = true;

    private IComponent iComponet;

    private string customer;    //   ITC-1103-0044 

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
                //initDropReason();
                refreshDropContent();
            }
            else
            {
                this.DropDownList1.Items.Add(new ListItem("", ""));
            }
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
    /// 为combobox控件添加Station属性，供使用者直接赋值
    /// </summary>
    public string Station
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
    /// 为combobox控件添加Customer属性，供使用者直接赋值    ITC-1103-0044 
    /// </summary>
    /// 
    public string Customer
    {
        get { return customer; }
        set { customer = value; }
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
    public void refreshDropContent()
    {
        //首先清空combobox内容
        DropDownList1.Items.Clear();

        //根据联动参数获取内容
        DropDownList1.Items.Add(new ListItem("", ""));
        iComponet = ServiceAgent.getInstance().GetObjectByName<IComponent>(WebConstant.CommonObject);
        if (iComponet != null)
        {
            IList<ComponentInfo> ctList = iComponet.GetComponentList(customer);
            if (!(ctList == null) && (ctList.Count > 0))
            {

                foreach (ComponentInfo component in ctList)
                {
                    //DropDownList1.Items.Add(new ListItem(component.friendlyName, component.id));
                    DropDownList1.Items.Add(new ListItem(component.id+" "+component.friendlyName, component.id));

                }
            }
        }
        up.Update();

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
