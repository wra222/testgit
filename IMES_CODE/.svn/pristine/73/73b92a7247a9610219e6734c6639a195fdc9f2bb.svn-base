 //Description: Family DropdownList
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
using com.inventec.iMESWEB;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;

public partial class CommonControl_FamilyControl : System.Web.UI.UserControl
{
     //combobox宽度
    private String length = "300";

      //combobox样式
    private String css;

    //combobox宽度是否支持百分比设定
    private Boolean isPercentage = false;

    private string _service = string .Empty ;
    //combobox是否可用,默认为可用
    private Boolean enabled = true;
    
    private string customer = string.Empty;

    private string station = string.Empty;

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
                } else 
                {
                    DropDownList1.Width = Unit.Percentage(Convert.ToDouble(length));
                }
            } else {
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
    /// 为combobox控件添加Service属性，供使用者直接赋值
    /// </summary>
    public String Service
    {

        set
        {
            _service  = value;
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
    /// 为combobox控件添加Station属性，供使用者直接赋值
    /// </summary>
    public String Station
    {

        set
        {
            station = value;
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

        DropDownList1.Items.Add(new ListItem("", ""));

        IFamily iFamily = null ;
        if (_service == "008")
        {
            iFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.Print1397LabelObject);
            if (iFamily != null)
            {
                IList<FamilyInfo> lstFamily = iFamily.GetFamilyList(customer);
                if (lstFamily != null)
                {
                    foreach (FamilyInfo family in lstFamily)
                    {
                        DropDownList1.Items.Add(new ListItem(family.friendlyName, family.id));
                    }
                }

            }
            
        }
         else if (_service =="000")
        {
            IVirtualMo iVirtualMO = ServiceAgent.getInstance().GetObjectByName<IVirtualMo>(WebConstant.VirtualMoObject);

             if (iVirtualMO != null)
            {
                IList<FamilyInfo> lstFamily = iVirtualMO.FindFamiliesByCustomer(customer);
                if (lstFamily != null)
                {
                    foreach (FamilyInfo family in lstFamily)
                    {
                       // DropDownList1.Items.Add(new ListItem(family.friendlyName, family.id));
                        DropDownList1.Items.Add(new ListItem(family.id, family.id)); // UC(LiuCaiBin):客户环境没有配置Descr，要求按id显示，本_service号(000)仅用于VirtualMO页面
                    }
                }

            }
          
 
        }
        else if (_service == "001")
        {
            iFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.CommonObject);
            if (iFamily != null)
            {
                IList<FamilyInfo> lstFamily = iFamily.GetFamilyList(customer);
                if (lstFamily != null)
                {
                    foreach (FamilyInfo family in lstFamily)
                    {
                        DropDownList1.Items.Add(new ListItem(family.id+"   "+family.friendlyName+"   "+customer, family.id));
                    }
                }

            }
          
 
        }
        //else if (_service == "100")
        //{
        //    ITPCBCollection iTPCBCollection = ServiceAgent.getInstance().GetObjectByName<ITPCBCollection>(WebConstant.TPCBCollectionObject);
        //    if (iTPCBCollection != null)
        //    {
        //        IList<FamilyInfo> lstFamily = iTPCBCollection.GetFamilyList();
        //        if (lstFamily != null)
        //        {
        //            foreach (FamilyInfo family in lstFamily)
        //            {
        //                DropDownList1.Items.Add(new ListItem(family.friendlyName, family.id));
        //            }
        //        }

        //    }

        //}
        //else if (_service == "014")
        //{
        //    ITravelCardPrint iTravelCardPrint = ServiceAgent.getInstance().GetObjectByName<ITravelCardPrint>(WebConstant.TravelCardPrintObject);
        //    if (iTravelCardPrint != null)
        //    {
        //        IList<FamilyInfo> lstFamily = iTravelCardPrint.GetFamilyList(customer);
        //        if (lstFamily != null)
        //        {
        //            foreach (FamilyInfo family in lstFamily)
        //            {
        //                DropDownList1.Items.Add(new ListItem(family.friendlyName, family.id));
        //            }
        //        }

        //    }

        //}
        else if (_service == "013")
        {
            //For Travel Cart Print
            //ITC-1360-0353
            ITravelCardPrint2012 iTravelCardPrint = ServiceAgent.getInstance().GetObjectByName<ITravelCardPrint2012>(WebConstant.TravelCardPrint2012Object);
            //iFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.CommonObject);
            if (iTravelCardPrint != null)
            {
                IList<FamilyInfo> lstFamily = iTravelCardPrint.GetFamilyList(customer);
                if (lstFamily != null)
                {
                    foreach (FamilyInfo family in lstFamily)
                    {
                        //ITC-1360-1254
                        DropDownList1.Items.Add(new ListItem(family.id + " " + family.friendlyName + " " + customer, family.id));
                    }
                }

            }
        }
        else if (_service == "015") //ITC-1414-0139, Jessica Liu, 2012-6-13
        {
            iFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.CommonObject);
            if (iFamily != null)
            {
                IList<FamilyInfo> lstFamily = iFamily.FindFamiliesByCustomerOrderByFamily(customer);
                if (lstFamily != null)
                {
                    foreach (FamilyInfo family in lstFamily)
                    {
                        DropDownList1.Items.Add(new ListItem(family.id, family.id));
                    }
                }

            }

        }
        else
        {
            iFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.CommonObject);
            if (iFamily != null)
            {
                IList<FamilyInfo> lstFamily = iFamily.GetFamilyList(customer);
                if (lstFamily != null)
                {
                    foreach (FamilyInfo family in lstFamily)
                    {
                        DropDownList1.Items.Add(new ListItem(family.friendlyName, family.id));
                    }
                }

            }


        }
       


    }


    /// <summary>
    /// 重新更新combobox的内容
    /// </summary>
    /// <param name="family"></param>
    public void refreshDropContent(String customer)
    {

        //首先清空combobox内容
        DropDownList1.Items.Clear();

        //            <bug>
        //                BUG NO:ITC-1122-0282
        //                REASON:clear已经加了空串，这里去掉重复加入
        //            </bug>
        IFamily iFamily = null;
        if (_service == "008")
        {
            iFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.Print1397LabelObject);
            if (iFamily != null)
            {
                IList<FamilyInfo> lstFamily = iFamily.GetFamilyList(customer);
                if (lstFamily != null)
                {
                    foreach (FamilyInfo family in lstFamily)
                    {
                        DropDownList1.Items.Add(new ListItem(family.friendlyName, family.id));
                    }
                }

            }

        }
        //else if (_service == "100")
        //{
        //    ITPCBCollection iTPCBCollection = ServiceAgent.getInstance().GetObjectByName<ITPCBCollection>(WebConstant.TPCBCollectionObject);
        //    if (iTPCBCollection != null)
        //    {
        //        IList<FamilyInfo> lstFamily = iTPCBCollection.GetFamilyList();
        //        if (lstFamily != null)
        //        {
        //            foreach (FamilyInfo family in lstFamily)
        //            {
        //                DropDownList1.Items.Add(new ListItem(family.friendlyName, family.id));
        //            }
        //        }

        //    }

        //}
        //else if (_service == "014")
        //{
        //    ITravelCardPrint iTravelCardPrint = ServiceAgent.getInstance().GetObjectByName<ITravelCardPrint>(WebConstant.TravelCardPrintObject);
        //    if (iTravelCardPrint != null)
        //    {
        //        IList<FamilyInfo> lstFamily = iTravelCardPrint.GetFamilyList(customer);
        //        if (lstFamily != null)
        //        {
        //            foreach (FamilyInfo family in lstFamily)
        //            {
        //                DropDownList1.Items.Add(new ListItem(family.friendlyName, family.id));
        //            }
        //        }

        //    }

        //}
        else
        {
            iFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.CommonObject);
            if (iFamily != null)
            {
                IList<FamilyInfo> lstFamily = iFamily.GetFamilyList(customer);
                if (lstFamily != null)
                {
                    foreach (FamilyInfo family in lstFamily)
                    {
                        DropDownList1.Items.Add(new ListItem(family.friendlyName, family.id));
                    }
                }

            }


        }
        //刷新update panel
        up.Update();


    }

    public void setSelected(int index)
    {
        this.DropDownList1.SelectedIndex = index;
        up.Update();
    }


}
