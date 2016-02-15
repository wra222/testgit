//Description: Decode Type DropdownList
 //Update: 
 //Date         Name                Reason 
 //==========   ==================  =====================================    
 //2009-10-15   Lucy.Liu(EB1)  create
 //Known issues:Any restrictions about this file
using System;
using System.Text;
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
using IMES.Infrastructure;


public partial class CommonControl_DecodeTypeControl : System.Web.UI.UserControl
{
     //combobox宽度
    private String length = "300";

    //combobox样式
    private String css;

    //combobox宽度是否支持百分比设定
    private Boolean isPercentage = false;

    //combobox是否可用,默认为可用
    private Boolean enabled = true;

    private IDCode iDCode = ServiceAgent.getInstance().GetObjectByName<IDCode>(WebConstant.CommonObject);

    private Boolean isFru;
    private Boolean isVGA = false;

    private string customer;

    //Jessica Liu, 2012-5-29
    private Boolean isDK = false;
    public Boolean IsDK
    {
        get { return isDK; }
        set { isDK = value; }
    }

    public Boolean IsVGA
    {
        get { return isVGA; }
        set { isVGA = value; }
    }
    private Boolean isKP = false;
    public Boolean IsKP
    {
        get { return isKP; }
        set { isKP = value; }
    }

    public string Customer
    {
        get { return customer; }
        set { customer = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
        catch (FisException ex)
        {
            showCmbErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showCmbErrorMessage(ex.Message);
        }
     
    }

    private void showCmbErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\0013", string.Empty).Replace("\0010", "\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.up, typeof(System.Object), "showCmbErrorMessage", scriptBuilder.ToString(), false);
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
    /// 为combobox控件添加Customer属性，供使用者直接赋值
    /// </summary>
    public Boolean IsFru
    {

        set
        {
            isFru = value;
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

        if (iDCode != null)
        {
            IList<DCodeInfo> lstDCode = null;//iDCode.GetDCodeRuleListForMB(isFru);

            if (!string.IsNullOrEmpty(customer))
            {
                if (isKP)
                {
                    lstDCode = iDCode.GetDCodeRuleListForKP(customer);
                }
                else
                {

                    if (isVGA)
                    {
                        lstDCode = iDCode.GetDCodeRuleListForVB(customer);
                    }
                    else if (isDK)  //Jessica Liu, 2012-5-29
                    {
                        lstDCode = iDCode.GetDCodeRuleListForDK(customer);
                    }
                    else
                    {
                        // modify by zhulei
                        //lstDCode = iDCode.GetDCodeRuleListForMB(isFru,customer);
                        lstDCode = iDCode.GetDCodeRuleListForMB("Customer");

                    }
                }
            }
            foreach (DCodeInfo dCode in lstDCode)
            {
                DropDownList1.Items.Add(new ListItem(dCode.friendlyName, dCode.id));
            }
        }
        
      
    }

    public void setSelected(int index)
    {
        this.DropDownList1.SelectedIndex = index;
        up.Update();
    }


    

       
}
