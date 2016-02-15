/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: ProdIdRange DropdownList
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-05    Chen Xu (EB1-4)     Create
 * 
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

public partial class CommonControl_ProdIdRangeControl : System.Web.UI.UserControl
{
    private String length = "300";

    private String css;

    private String station;

    private Boolean isPercentage = false;

    private Boolean enabled = true;

    private IProdIdRange iProdIDRange;


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
    /// 
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
    public void refreshDropContent(String MOId)
    {
        //首先清空combobox内容
        DropDownList1.Items.Clear();

        //根据联动参数获取内容
        DropDownList1.Items.Add(new ListItem("", ""));
        iProdIDRange = ServiceAgent.getInstance().GetObjectByName<IProdIdRange>(WebConstant.CommonObject);
        IList<ProdIdRangeInfo> pdirList = iProdIDRange.GetProdIdRangeList(MOId);
        if (iProdIDRange != null)
        {
            if (!(pdirList == null) && (pdirList.Count > 0))
            {

                foreach (ProdIdRangeInfo pdir in pdirList)
                {
                    DropDownList1.Items.Add(new ListItem(pdir.startId + "-" + pdir.endId, pdir.startId + "-" + pdir.endId));
                }
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
}