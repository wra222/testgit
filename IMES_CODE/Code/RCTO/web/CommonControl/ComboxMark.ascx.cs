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
                //��ʼ��combobox������
                refreshDropContent();
            }
            else
            {
                this.DropDownList1.Items.Add(new ListItem("", ""));
            }
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
    /// Ϊcombobox�ؼ����Width���ԣ���ʹ����ֱ�Ӹ�ֵ
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
    /// Ϊcombobox�ؼ����CssClass���ԣ���ʹ����ֱ�Ӹ�ֵ
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
        this.up.Update();

    }

    /// <summary>
    /// ��ʼ��combobox������
    /// </summary>
    /// <returns></returns>

    protected void initDropContent()
    {
        //����combobox��ʼ������Ϊ��
        DropDownList1.Items.Add(new ListItem("", ""));

    }

    /// <summary>
    /// ���¸���combobox������
    /// </summary>
    /// <param name="family"></param>
    public void refreshDropContent()
    {
        //�������combobox����
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
        //ˢ��update panel
        up.Update();


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