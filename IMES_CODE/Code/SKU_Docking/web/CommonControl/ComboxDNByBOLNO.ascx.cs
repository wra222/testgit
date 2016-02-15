/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  dropdown list
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * Known issues:
 */
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using IMES.DataModel;
using com.inventec.iMESWEB;
using IMES.Station.Interface.StationIntf;

public partial class comboxControl_ComboxDNByBOLNO:System.Web.UI.UserControl

{
    //combobox width
    private string length = "300";

    //combobox style
    private string css;
    //combobox with percentage setting
    private Boolean isPercentage = false;

    private IPalletDataCollection currentService = ServiceAgent.getInstance().GetObjectByName<IPalletDataCollection>(WebConstant.PalletDataCollectionObject);
    private IPalletDataCollectionTRO currentServiceTRO = ServiceAgent.getInstance().GetObjectByName<IPalletDataCollectionTRO>(WebConstant.PalletDataCollectionTROObject);
    private Boolean enabled = true;
    private Boolean isPalletCollectionTRO = false;

    public Boolean IsPalletCollectionTRO
    {
        get { return isPalletCollectionTRO; }
        set { isPalletCollectionTRO = value; }
    }

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

            DropDownList1.CssClass = css;

            this.DropDownList1.Enabled = enabled;
            if (enabled)
            {
                //��ʼ��combobox������
                initDropContent();
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
        if (!IsPalletCollectionTRO)
        {
            //����combobox��ʼ������Ϊ��
            DropDownList1.Items.Add(new ListItem("", ""));
        }
        else
        {
            DropDownList1.Items.Clear();
            DropDownList1.Items.Add(new ListItem("", ""));

            IList<string> DnList = currentServiceTRO.getDNList();
            if (!(DnList == null) && (DnList.Count > 0))
            {
                foreach (string dnItem in DnList)
                {
                    DropDownList1.Items.Add(new ListItem(dnItem, dnItem));
                }
            }
        }
    }

    /// <summary>
    /// ���¸���combobox������
    /// </summary>
    /// <param name="family"></param>
    public void refreshDropContent(string BOLNO)
    {
        //�������combobox����
        DropDownList1.Items.Clear();
        DropDownList1.Items.Add(new ListItem("", ""));
        IList<string> DnList = null;

        if (!IsPalletCollectionTRO)
        {
            //DnList = currentService.getDNListByBol(BOLNO);
        }
        else
        {
            DnList = currentServiceTRO.getDNList();
        }
         
        if (!(DnList == null) && (DnList.Count > 0))
        {
            foreach (string dnItem in DnList)
            {
                DropDownList1.Items.Add(new ListItem(dnItem, dnItem));
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


}