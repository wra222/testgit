/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbPdLine
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-05   liu xiao ling     Create 
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

public partial class CommonControl_CmbAttributeForMaintain : System.Web.UI.UserControl
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

    private string mode = string.Empty;

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
                    DropDownList_MyAttribute.Width = Unit.Percentage(100);
                }
                else
                {
                    DropDownList_MyAttribute.Width = Unit.Percentage(Convert.ToDouble(length));
                }
            }
            else
            {
                DropDownList_MyAttribute.Width = Unit.Parse(length);
            }

            DropDownList_MyAttribute.CssClass = css;
            this.DropDownList_MyAttribute.Enabled = enabled;
            if (enabled)
            {
                initFamily();
            }
            else
            {
                this.DropDownList_MyAttribute.Items.Add(new ListItem("", ""));
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
    public String Mode
    {

        set
        {
            mode = value;
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
            return this.DropDownList_MyAttribute;
        }

    }

    /// <summary>
    /// 清空combobox内容
    /// </summary>
    public void clearContent()
    {

        //清空combobox内容
        this.DropDownList_MyAttribute.Items.Clear();
        //DropDownList_MyAttribute.Items.Add(new ListItem("", ""));
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
        /*
        if (firstNullItem)
        {
            DropDownList_MyAttribute.Items.Add(new ListItem("", ""));
        }
        */
        ILabelSettingManager iFamily = null;

        iFamily = ServiceAgent.getInstance().GetMaintainObjectByName<ILabelSettingManager>(WebConstant.ILabelSettingManager);

        IList<string> lstFamily = new List<string>();
        if (iFamily != null)
        {
            switch(mode)
            {
                case "Model":
                     lstFamily = iFamily.GetModelList();
                    break;
                case "ModelInfo":
                     lstFamily = iFamily.GetModelInfoNameList();
                    break;
                case "Delivery":
                     lstFamily = iFamily.GetDeliveryNoList();
                    break;
                case "DeliveryInfo":
                     lstFamily = iFamily.GetDeliveryInfoTypeList();
                    break;
                case "Part":
                     lstFamily = iFamily.GetPartList();
                    break;
                case "PartInfo":
                     lstFamily = iFamily.GetPartInfoTypeList();
                    break;
                case "Customer":
                     lstFamily.Add("Customer");
                    break;
            }
            if (lstFamily != null)
            {
                foreach (string family in lstFamily)
                {
                    DropDownList_MyAttribute.Items.Add(new ListItem(family, family));
                }
            }

        }


    }

    public void setSelected(int index)
    {
        this.DropDownList_MyAttribute.SelectedIndex = index;
        up.Update();
    }

}
