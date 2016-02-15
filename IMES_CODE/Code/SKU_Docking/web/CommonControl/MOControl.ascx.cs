 //Description: MO DropdownList
 //Update: 
 //Date         Name                Reason 
 //==========   ==================  =====================================    
 //2009-10-15   Lucy.Liu(EB1)       create
 //2012-01-09   zhu lei             Modify ITC211026
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
using com.inventec.iMESWEB;
using System.Collections.Generic;
using IMES.DataModel;


public partial class CommonControl_MOControl : System.Web.UI.UserControl
{
     //combobox宽度
    private String length = "300";

    //combobox样式
    private String css;

    //combobox宽度是否支持百分比设定
    private Boolean isPercentage = false;

    private IMO imoService = null;

    //combobox是否可用,默认为可用
    private Boolean enabled = true;

    //表明这个控件被哪个页面引用
    private String belongPage = "";

    private string _service = string.Empty;

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
    /// 为combobox控件添加BelongPage属性，供使用者直接赋值
    /// </summary>
    public String BelongPage
    {
        get
        {
            return belongPage;
        }
        set
        {
            belongPage = value;
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
    public void refreshDropContent(String model)
    {
        //首先清空combobox内容
        DropDownList1.Items.Clear();

        //根据联动参数获取内容
        DropDownList1.Items.Add(new ListItem("", ""));
        
        
        //if (belongPage == "")
        //{
        //    //仍然用公用的接口获取
        //    //imoService = (IMO)ServiceAgent.getInstance().GetObjectByName(WebConstant.IMO_SERVICE);

        //    //List<MOInfo> moList = this.imoService.getMOList(model);
        //    //if (!(moList == null) && (moList.Count > 0))
        //    //{
        //    //    foreach (MOInfo mo in moList)
        //    //    {
        //    //        DropDownList1.Items.Add(new ListItem(mo.friendlyName, mo.id));
        //    //    }
        //    //}
        //    //如下代码是测试用的，使用时请删除
        //    DropDownList1.Items.Add(new ListItem(model + "1", model + "1"));
        //    DropDownList1.Items.Add(new ListItem(model + "2", model + "2"));
        //    DropDownList1.Items.Add(new ListItem(model + "3", model + "3"));
        //}
        //else
        //{
        //    //imoService = (IMO)ServiceAgent.getInstance().GetObjectByName(WebConstant.IProIdPrint_SERVICE);

        //    //List<MOInfo> moList = this.imoService.getMOList(model);
        //    //if (!(moList == null) && (moList.Count > 0))
        //    //{
        //    //    foreach (MOInfo mo in moList)
        //    //    {
        //    //        DropDownList1.Items.Add(new ListItem(mo.friendlyName, mo.id));
        //    //    }
        //    //}
        //    //如下代码是测试用的，使用时请删除
        //    DropDownList1.Items.Add(new ListItem("belong" + "1", "belong" + "1"));
        //    DropDownList1.Items.Add(new ListItem("belong" + "2", "belong" + "2"));
        //    DropDownList1.Items.Add(new ListItem("belong" + "3", "belong" + "3"));
        //}
       
        if (_service == "014")
        {
            imoService = ServiceAgent.getInstance().GetObjectByName<IMO>(WebConstant.ProIdPrintObject);
        } else if (_service == "013")
        {            
            imoService = ServiceAgent.getInstance().GetObjectByName<IMO>(WebConstant.TravelCardPrint2012Object);
        }
        else if (_service == "002")
        {
            ISMTMO iSMTMO = ServiceAgent.getInstance().GetObjectByName<ISMTMO>(WebConstant.MBLabelPrintObject);
            IList<SMTMOInfo> smtmoList = iSMTMO.GetSMTMOList(model);
            if (!(smtmoList == null) && (smtmoList.Count > 0))
            {
                foreach (SMTMOInfo smtmo in smtmoList)
                {
                    DropDownList1.Items.Add(new ListItem(smtmo.friendlyName, smtmo.id));
                }
            }

        }
        //add by zhulei
        else if (_service == "006")
        {
            ISMTMO iSMTMO = ServiceAgent.getInstance().GetObjectByName<ISMTMO>(WebConstant.CommonObject);
            IList<SMTMOInfo> smtmoList = iSMTMO.GetSmtMoListByPno(model);
            if (!(smtmoList == null) && (smtmoList.Count > 0))
            {
                foreach (SMTMOInfo smtmo in smtmoList)
                {
                    DropDownList1.Items.Add(new ListItem(smtmo.friendlyName, smtmo.id));
                }
            }

        }
        else
        {
            imoService = ServiceAgent.getInstance().GetObjectByName<IMO>(WebConstant.CommonObject);

        }

        if (imoService != null &&  _service != "002" && _service != "006")
        {
           
           IList<MOInfo> moList = imoService.GetMOList(model);
     
            if (!(moList == null) && (moList.Count > 0))
            {
                foreach (MOInfo mo in moList)
                {
                    DropDownList1.Items.Add(new ListItem(mo.friendlyName, mo.id));
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
