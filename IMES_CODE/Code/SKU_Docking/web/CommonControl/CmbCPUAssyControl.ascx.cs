 //Description: Family DropdownList
 //Update: 
 //Date         Name                Reason 
 //==========   ==================  =====================================    
 //2009-10-15   207006(EB1)  create
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
using System.Collections.Generic;


public partial class CommonControl_CPUAssyControl : System.Web.UI.UserControl
{
    //combobox���
    private String length = "300";

    //combobox��ʽ
    private String css;

    //combobox����Ƿ�֧�ְٷֱ��趨
    private Boolean isPercentage = false;
        
    //combobox�Ƿ����,Ĭ��Ϊ����
    private Boolean enabled = true;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //�趨�߶�
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

            //��ʼ��combobox������
            initDropReason();
        }

    }
    /// <summary>
    /// Ϊcombobox�ؼ����Enabled���ԣ���ʹ����ֱ�Ӹ�ֵ
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
    /// Ϊcombobox�ؼ����Width���ԣ���ʹ����ֱ�Ӹ�ֵ
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
    /// Ϊcombobox�ؼ����CssClass���ԣ���ʹ����ֱ�Ӹ�ֵ
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
    /// Ϊcombobox�ؼ����IsPercentage���ԣ���ʹ����ֱ�Ӹ�ֵ
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
    /// ���ظ��û��ؼ��ж����DropDownList����
    /// </summary>
    public DropDownList InnerDropDownList
    {
        get
        {
            return this.DropDownList1;
        }

    }

    /// <summary>
    /// ���combobox����
    /// </summary>
    public void clearContent()
    {

        //���combobox����
        this.DropDownList1.Items.Clear();
        DropDownList1.Items.Add(new ListItem("", ""));
        this.up.Update();

    }

    /// <summary>
    /// ��ʼ��combobox������
    /// </summary>
    /// <returns></returns>

    protected void initDropReason()
    {
        DropDownList1.Items.Add(new ListItem("", ""));
    }


    /// <summary>
    /// ����indexΪcombobox�ø���ѡ��
    /// </summary>
    /// <param name="index"></param>
    public void setSelected(int index)
    {
        this.DropDownList1.SelectedIndex = index;
        up.Update();
    }


    /// <summary>
    /// ���¸���combobox������
    /// </summary>
    /// <param name="family"></param>
    public void refreshDropContent(IList<String> list)
    {
        //�������combobox����
        DropDownList1.Items.Clear();
        DropDownList1.Items.Add(new ListItem("", ""));
        foreach (string item in list)
        {
            DropDownList1.Items.Add(new ListItem(item, item));
        }
        up.Update();
    }
}
