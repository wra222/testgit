/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Model DropdownList
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-10-15   Chen Xu (EB1-4)     create
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
using IMES.Station.Interface.StationIntf;
using com.inventec.iMESWEB;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.DataModel;
using IMES.Docking.Interface.DockingIntf;


public partial class ModelControl_ForDocking : System.Web.UI.UserControl
{
    private String length = "300";

    private String css;

    private String station;

    private String belongPage;

    private Boolean isPercentage = false;

    private Boolean enabled = true;

    private IModel iModel;

    private string _station = string.Empty;
    
    private string _service = string.Empty;

    private string _customer = string.Empty;
  
 //   IProIdPrint iProIdPrint = (IProIdPrint)ServiceAgent.getInstance().GetObjectByName(WebConstant.IProIdPrint_SERVICE);

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
    /// 为combobox控件添加Customer属性，供使用者直接赋值
    /// </summary>
    public String Customer
    {

        set
        {
            _customer = value;
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
        set
        {
            _station = value;
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

   
    /// <summary>
    /// 重新更新combobox的内容
    /// </summary>
    /// <param name="family"></param>
    //public void refreshDropContent(String familyId)
    //{
    //    //首先清空combobox内容
    //    DropDownList1.Items.Clear();

    //    //根据联动参数获取内容
    //    DropDownList1.Items.Add(new ListItem("", ""));

    //    if (belongPage == "")
    //    {
    //        //仍然用公用的接口获取
    //        iModel = (IModel)ServiceAgent.getInstance().GetObjectByName(WebConstant.IModel_SERVICE);
    //        IList<ModelInfo> mdList = this.iModel.GetModelList(familyId);
            
    //        //if (!(mdList == null) && (mdList.Count > 0))
    //        //{

    //        //    foreach (ModelInfo md in mdList)
    //        //    {
    //        //        DropDownList1.Items.Add(new ListItem(md.friendlyName,md.id));
    //        //    }
    //        //}
    //        //如下代码是测试用的，使用时请删除
    //        if (familyId != "")
    //        {
    //            DropDownList1.Items.Add(new ListItem(familyId + "1", familyId + "1"));
    //            DropDownList1.Items.Add(new ListItem(familyId + "2", familyId + "2"));
    //            DropDownList1.Items.Add(new ListItem(familyId + "3", familyId + "3"));
    //        }
    //    }

    //    else
    //    {

    //        iModel = (IModel)ServiceAgent.getInstance().GetObjectByName(WebConstant.IProIdPrint_SERVICE);

    //        IList<ModelInfo> mdList = this.iModel.GetModelList(familyId);
           
    //        //if (!(mdList == null) && (mdList.Count > 0))
    //        //{

    //        //    foreach (ModelInfo md in mdList)
    //        //    {
    //        //        DropDownList1.Items.Add(new ListItem(md.friendlyName,md.id));
    //        //    }
    //        //}
    //        //如下代码是测试用的，使用时请删除
    //        if (familyId != "")
    //        {
    //            DropDownList1.Items.Add(new ListItem(familyId + "1", familyId + "1"));
    //            DropDownList1.Items.Add(new ListItem(familyId + "2", familyId + "2"));
    //            DropDownList1.Items.Add(new ListItem(familyId + "3", familyId + "3"));
    //        }
    //        }
    //        //刷新update panel
    //        up.Update();

    //}

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

        iModel= null;
        if (_service == "014")  
        {

            iModel = ServiceAgent.getInstance().GetObjectByName<IModel>(WebConstant.ProIdPrintObject);
        }
        else if (_service == "013")
        {

            ITravelCardPrint2012 iTravelCardPrint = ServiceAgent.getInstance().GetObjectByName<ITravelCardPrint2012>(WebConstant.TravelCardPrint2012Object);

            if (iTravelCardPrint != null)
            {
                IList<ModelInfo> lstModel = iTravelCardPrint.GetModelList(family);

                foreach (ModelInfo Model in lstModel)
                {
                    DropDownList1.Items.Add(new ListItem(Model.friendlyName, Model.id));
                }
            }
        }
        else if (_service == "ProdIdPrintForDocking")
        {
            IProdIdPrintForDocking iProdIdPrint = ServiceAgent.getInstance().GetObjectByName<IProdIdPrintForDocking>(WebConstant.ProdIdPrintForDocking);
            if (iProdIdPrint != null)
            {
                IList<string> lstModel = iProdIdPrint.GetModelList(family);
                foreach (string Model in lstModel)
                {
                    DropDownList1.Items.Add(new ListItem(Model, Model));
                }
            }
        }
        else if (_service == "002")
        {
            iModel = ServiceAgent.getInstance().GetObjectByName<IModel>(WebConstant.CommonObject);

        }
        else if (_service == "102")
        {
            iModel = ServiceAgent.getInstance().GetObjectByName<IModel>(WebConstant.VirtualMoForDocking);
        }
        else if (_service == "094")
        {
            iModel = ServiceAgent.getInstance().GetObjectByName<IModel>(WebConstant.AdjustMOObject);
        }

        if (iModel != null)
        {
            IList<ModelInfo> lstModel = iModel.GetModelList(family);

            foreach (ModelInfo Model in lstModel)
            {
                DropDownList1.Items.Add(new ListItem(Model.friendlyName,Model.id)); 
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