/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  dropdown list
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * 2010-10-30  Chen Xu (eB1-4)       Add: ICombinePOInCarton 
 * 2011-03-22  Chen Xu (eB1-4)       Add: IPrintShiptoCartonLabel
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
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.DataModel;
using com.inventec.iMESWEB;


public partial class comboxControl_ComboxDNByModel : System.Web.UI.UserControl
{
    //combobox width
    private String length = "300";

    //combobox style
    private String css;
    //combobox with percentage setting
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private ICombinePOInCarton combineDNCartonService;
    private IPalletDataCollectionTRO palletCollectionService;
    private IPrintShiptoCartonLabel printShiptoCartonLabelService;
    private IList<string> DnList;
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
                    initDropContent();
                }
                else
                {
                    this.DropDownList1.Items.Add(new ListItem("", ""));
                }
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

    protected void initDropContent()
    {
        //联动combobox初始化内容为空
        DropDownList1.Items.Add(new ListItem("", ""));

    }

    /// <summary>
    /// 重新更新combobox的内容
    /// </summary>
    /// <param name="family"></param>
    public void refreshDropContent(string Model, string Service)
    {
        //首先清空combobox内容
        DropDownList1.Items.Clear();

        DropDownList1.Items.Add(new ListItem("", ""));


        // modified by 208014
        if (Service == "053")
        {
            combineDNCartonService = ServiceAgent.getInstance().GetObjectByName<ICombinePOInCarton>(WebConstant.CombinePOInCartonObject);    // add by itc-208014

         //   DnList = this.combineDNCartonService.getDNList(Model); // Spec for BN: 新需求去掉了DN下拉框
        }
        else if (Service == "071") // add by itc-208014
        {
            printShiptoCartonLabelService = ServiceAgent.getInstance().GetObjectByName<IPrintShiptoCartonLabel>(WebConstant.PrintShiptoCartonLabelObject);
           
            DnList = this.printShiptoCartonLabelService.getDNList(Model);
        }
        else
        {
            palletCollectionService = ServiceAgent.getInstance().GetObjectByName<IPalletDataCollectionTRO>(WebConstant.PalletDataCollectionTROObject);

            DnList = this.palletCollectionService.getDNList();

        }
        if ((DnList!= null) && (DnList.Count>0))
        {
            foreach (string dnItem in DnList)
            {

                DropDownList1.Items.Add(new ListItem(dnItem, dnItem));
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