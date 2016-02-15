/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CmbPdLine
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-05   liu xiaoling     Create 
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

public partial class CommonControl_CblPalletProcessForMaintain : System.Web.UI.UserControl
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

    private string partNo = string.Empty;

    private string model = string.Empty;

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
                    cblPalletProcess.Width = Unit.Percentage(100);
                }
                else
                {
                    cblPalletProcess.Width = Unit.Percentage(Convert.ToDouble(length));
                }
            }
            else
            {
                cblPalletProcess.Width = Unit.Parse(length);
            }

            cblPalletProcess.CssClass = css;
            this.cblPalletProcess.Enabled = enabled;
            if (enabled)
            {
                initFamily();
            }
            else
            {
                this.cblPalletProcess.Items.Add(new ListItem("", ""));
                this.cblPalletProcess.Items.Add(new ListItem("", ""));
                this.cblPalletProcess.Items.Add(new ListItem("", ""));
                this.cblPalletProcess.Items.Add(new ListItem("", ""));
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
    public String Model
    {

        set
        {
            model = value;
        }

    }

    
    /// <summary>
    /// 为combobox控件添加Customer属性，供使用者直接赋值
    /// </summary>
    public String PartNo
    {

        set
        {
            partNo = value;
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
    /// 返回该用户控件中定义的CheckBoxList对象
    /// </summary>
    public CheckBoxList InnerCheckBoxList
    {
        get
        {
            return this.cblPalletProcess;
        }

    }


    public void refresh()
    {
        initFamily();
        up.Update();
    }

    /// <summary>
    /// 清空combobox内容
    /// </summary>
    public void clearContent()
    {

        //清空combobox内容
        this.cblPalletProcess.Items.Clear();
        cblPalletProcess.Items.Add(new ListItem("", ""));
        this.up.Update();

    }

    /// <summary>
    /// 初始化combobox的内容
    /// </summary>
    /// <returns></returns>

    protected void initFamily()
    {

        //DropDownList_PartTypeDesc.Items.Add(new ListItem("", ""));

        this.cblPalletProcess.Items.Clear();

        IProcessManager iFamily = null;

        iFamily = ServiceAgent.getInstance().GetMaintainObjectByName<IProcessManager>(WebConstant.IProcessManager);

        if (iFamily != null)
        {
            IList<CustomerMaintainInfo> lstFamily = iFamily.GetMyCustomerList();
            if (lstFamily != null && lstFamily.Count > 0)
            {
                foreach (CustomerMaintainInfo family in lstFamily)
                {
                    cblPalletProcess.Items.Add(new ListItem(family.Customer, family.Customer));
                }
            }
            else
            {
                this.cblPalletProcess.Items.Add(new ListItem("", ""));
                this.cblPalletProcess.Items.Add(new ListItem("", ""));
                this.cblPalletProcess.Items.Add(new ListItem("", ""));
                this.cblPalletProcess.Items.Add(new ListItem("", ""));
            }

        }

    }

    public void setSelected(int index)
    {
        this.cblPalletProcess.SelectedIndex = index;
        up.Update();
    }

}
