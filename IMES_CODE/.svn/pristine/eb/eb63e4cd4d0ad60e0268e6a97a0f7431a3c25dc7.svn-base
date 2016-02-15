/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  dropdown list
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * 2010-01-13  Tong.Zhi-Yong         Modify ITC-1103-0090
 * Known issues:
 */
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
using IMES.DataModel;
using com.inventec.iMESWEB;

public partial class CommonControl_ComboxMark : System.Web.UI.UserControl
{
    //combobox width
    private string length = "300";

    //combobox style
    private string css;
    //combobox with percentage setting
    private Boolean isPercentage = false;
    private IMark iMarkService = ServiceAgent.getInstance().GetObjectByName<IMark>(WebConstant.CommonObject);
    private Boolean enabled = true;

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

            DropDownList1.CssClass = css;

            this.DropDownList1.Enabled = enabled;
            if (enabled)
            {
                //初始化combobox的内容
                refreshDropContent();
            }
            else
            {
                this.DropDownList1.Items.Add(new ListItem("", ""));
            }
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
    /// 为combobox控件添加Width属性，供使用者直接赋值
    /// </summary>
    public string Width
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
    public string CssClass
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
        this.up.Update();

    }

    /// <summary>
    /// 初始化combobox的内容
    /// </summary>
    /// <returns></returns>

    protected void initDropContent()
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
        DropDownList1.Items.Add(new ListItem("", ""));
        //IList<MarkInfo> markList = this.iMarkService.GetMarkList();

        IList<MarkInfo> markList = getMarkList();

        if (!(markList == null) && (markList.Count > 0))
        {
            foreach (MarkInfo markItem in markList)
            {
                DropDownList1.Items.Add(new ListItem(markItem.friendlyName, markItem.id));
            }
        }
        //刷新update panel
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
    //ITC-1103-0090 Tong.Zhi-Yong 2010-01-13
    private IList<MarkInfo> getMarkList()
    {
        IList<MarkInfo> ret = new List<MarkInfo>();
        MarkInfo m1 = new MarkInfo("0", "0");
        MarkInfo m2 = new MarkInfo("1", "1");

        ret.Add(m1);
        ret.Add(m2);

        return ret;
    }

}