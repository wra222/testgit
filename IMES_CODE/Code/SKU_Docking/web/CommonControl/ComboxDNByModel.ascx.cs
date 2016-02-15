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
                    //��ʼ��combobox������
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

    protected void initDropContent()
    {
        //����combobox��ʼ������Ϊ��
        DropDownList1.Items.Add(new ListItem("", ""));

    }

    /// <summary>
    /// ���¸���combobox������
    /// </summary>
    /// <param name="family"></param>
    public void refreshDropContent(string Model, string Service)
    {
        //�������combobox����
        DropDownList1.Items.Clear();

        DropDownList1.Items.Add(new ListItem("", ""));


        // modified by 208014
        if (Service == "053")
        {
            combineDNCartonService = ServiceAgent.getInstance().GetObjectByName<ICombinePOInCarton>(WebConstant.CombinePOInCartonObject);    // add by itc-208014

         //   DnList = this.combineDNCartonService.getDNList(Model); // Spec for BN: ������ȥ����DN������
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