 //Description: Part NO DropdownList
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

public partial class CommonControl_PartnoControl : System.Web.UI.UserControl
{
     //combobox���
    private String length = "300";

    //combobox��ʽ
    private String css;

    //combobox����Ƿ�֧�ְٷֱ��趨
    private Boolean isPercentage = false;

    //private IPartNo iPartNo = ServiceAgent.getInstance().GetObjectByName<IPartNo>(WebConstant.CommonObject);

    //combobox�Ƿ����,Ĭ��Ϊ����
    private Boolean enabled = true;

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
    /// Ϊcombobox�ؼ����Service���ԣ���ʹ����ֱ�Ӹ�ֵ
    /// </summary>
    public String Service
    {

        set
        {
            _service = value;
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
    public void refreshDropContent(String type, String description)
    {
        
        //�������combobox����
        DropDownList1.Items.Clear();

        //��������������ȡ����
        DropDownList1.Items.Add(new ListItem("", ""));
        if (_service == "100")
        {
           

            ITPCBCollection iTPCBCollection = ServiceAgent.getInstance().GetObjectByName<ITPCBCollection>(WebConstant.TPCBCollectionObject);

            IList<string> partList = iTPCBCollection.GetPartNoList(type, description);
            if (!(partList == null) && (partList.Count > 0))
            {
                foreach (string part in partList)
                {
                    DropDownList1.Items.Add(new ListItem(part, part));
                }
            }
        }
        else
        {
            IPartNo iPartNo = ServiceAgent.getInstance().GetObjectByName<IPartNo>(WebConstant.CommonObject);
            IList<PartNoInfo> partList = iPartNo.GetPartNoList(type, description);
            if (!(partList == null) && (partList.Count > 0))
            {
                foreach (PartNoInfo part in partList)
                {
                    DropDownList1.Items.Add(new ListItem(part.friendlyName, part.id));
                }
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
