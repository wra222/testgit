 //Description: Pallet DropdownList
 //Update: 
 //Date         Name                Reason 
 //==========   ==================  =====================================    
 //2009-10-15   Lucy.Liu(EB1)       create
 //2010-05-18   Tong.Zhi-Yong       Modify ITC-1155-0079
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

public partial class CommonControl_PalletControl : System.Web.UI.UserControl
{
     //combobox���
    private String length = "300";

    //combobox��ʽ
    private String css;

    //combobox����Ƿ�֧�ְٷֱ��趨
    private Boolean isPercentage = false;

    private IPallet iPallet = ServiceAgent.getInstance().GetObjectByName<IPallet>(WebConstant.CommonObject);

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
                //��ʼ��combobox������
                initDropReason();
            }
            else
            {
                this.DropDownList1.Items.Add(new ListItem("", ""));
            }
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
       //����combobox��ʼ������Ϊ��
        DropDownList1.Items.Add(new ListItem("", ""));
      
    }
   
    /// <summary>
    /// ���¸���combobox������
    /// </summary>
    /// <param name="family"></param>
    public void refreshDropContent(String dn)
    {
        //�������combobox����
        DropDownList1.Items.Clear();

        //ITC-1155-0079 Tong.Zhi-Yong 2010-05-18
        //��������������ȡ����
        DropDownList1.Items.Add(new ListItem("", ""));
        IList<PalletInfo> lst = this.iPallet.GetPalletList(dn);
        if (!(lst == null) && (lst.Count > 0))
        {
            foreach (PalletInfo pallet in lst)
            {
                DropDownList1.Items.Add(new ListItem(pallet.friendlyName, pallet.id));
            }
        }
       
        //���´����ǲ����õģ�ʹ��ʱ��ɾ��
        //DropDownList1.Items.Add(new ListItem(dn + "1", dn + "1"));
        //DropDownList1.Items.Add(new ListItem(dn + "2", dn + "2"));
        //DropDownList1.Items.Add(new ListItem(dn + "3", dn + "3"));
        
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
