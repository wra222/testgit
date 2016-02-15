﻿/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbPdLine
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-05   Tong.Zhi-Yong     Create 
 * 2010-01-12   Tong.Zhi-Yong     Modify ITC-1103-0001
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
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;

public partial class CommonControl_CmbFamilyForMaintain : System.Web.UI.UserControl
{
    //combobox宽度
    private String length = "300";

    //combobox样式
    private String css;

    //combobox宽度是否支持百分比设定
    private Boolean isPercentage = false;

    private string _service = string.Empty;
    //combobox是否可用,默认为可用
    private Boolean enabled = true;

    private string customer = string.Empty;

    private Boolean firstNullItem = false;

    //private IFamily iFamily = (IFamily)ServiceAgent.getInstance().GetObjectByName(WebConstant.IFAMILY_SERVICE);

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
                initFamily();
            }
            else
            {
                this.DropDownList1.Items.Add(new ListItem("", ""));
            }


        }

    }

    /// <summary>
    /// 为combobox控件添加Service属性，供使用者直接赋值
    /// </summary>
    public String Service
    {

        set
        {
            _service = value;
        }

    }

    /// <summary>
    /// 为combobox控件添加Customer属性，供使用者直接赋值
    /// </summary>
    public String Customer
    {

        set
        {
            customer = value;
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
    /// 为combobox控件添加Enabled属性，供使用者直接赋值
    /// </summary>
    public Boolean FirstNullItem
    {
        get
        {
            return firstNullItem;
        }
        set
        {
            firstNullItem = value;
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

    public void refresh()
    {
        initFamily();
        up.Update();
    }

    /// <summary>
    /// 初始化combobox的内容
    /// </summary>
    /// <returns></returns>

    protected void initFamily()
    {
        DropDownList1.Items.Add(new ListItem("", ""));

        IFamilyManagerEx iFamily = ServiceAgent.getInstance().GetMaintainObjectByName<IFamilyManagerEx>(WebConstant.IFamilyManagerEx);

        if (iFamily != null)
        {
            /*IList<FamilyInfo> lstFamily = iFamily.FindFamiliesByCustomerOrderByFamily(this.customer);
            if (lstFamily != null)
            {
                foreach (FamilyInfo family in lstFamily)
                {
                    DropDownList1.Items.Add(new ListItem(family.friendlyName, family.friendlyName));
                }
            }*/

            IList<string> lstFamily = iFamily.GetFamilyByCustomer(this.customer);
            if (lstFamily != null)
            {
                foreach (string family in lstFamily)
                {
                    DropDownList1.Items.Add(new ListItem(family, family));
                }
            }
        }


    }

    public void setSelected(int index)
    {
        this.DropDownList1.SelectedIndex = index;
        up.Update();
    }

}
