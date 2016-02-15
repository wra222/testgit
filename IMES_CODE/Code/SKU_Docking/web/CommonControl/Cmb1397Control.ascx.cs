 //Description: 1397 DropdownList
 //Update: 
 //Date         Name                Reason 
 //==========   ==================  =====================================    
 //2009-10-15   Lucy.Liu(EB1)  create
 //Known issues:Any restrictions about this file
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic ;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IMES.Station.Interface.CommonIntf;
using com.inventec.iMESWEB;
using IMES.DataModel;


public partial class CommonControl_Cmb1397Control : System.Web.UI.UserControl
{
     //combobox���
    private String length = "300";

    //combobox��ʽ
    private String css;

    //combobox����Ƿ�֧�ְٷֱ��趨
    private Boolean isPercentage = false;

    //combobox�Ƿ����,Ĭ��Ϊ����
    private Boolean enabled = true;
    private string _service = string.Empty;

    //private I1397Level i1397Level = (I1397Level)ServiceAgent.getInstance().GetObjectByName(WebConstant.I1397LEVEL_SERVICE);
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
    /// Ϊcombobox�ؼ����service���ԣ���ʹ����ֱ�Ӹ�ֵ
    /// </summary>
    public String Service
    {

        set
        {
            _service = value;
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
    public void refreshDropContent(String family)
    {
        //�������combobox����
        DropDownList1.Items.Clear();
        DropDownList1.Items.Add(new ListItem("", ""));

        I1397Level i1397Level = null;
        if (_service == "008")
        {
            i1397Level = ServiceAgent.getInstance().GetObjectByName<I1397Level>(WebConstant.Print1397LabelObject);
        }
        else
        {
            i1397Level = ServiceAgent.getInstance().GetObjectByName<I1397Level>(WebConstant.CommonObject);

        }

        //��������������ȡ����
        if (i1397Level != null)
        {
            IList<_1397LevelInfo> lst1397 = i1397Level.Get1397LevelList(family);

            foreach (_1397LevelInfo _1397 in lst1397)
            {
                DropDownList1.Items.Add(new ListItem(_1397.friendlyName, _1397.id));
            }
        }
        ////���´����ǲ����õģ�ʹ��ʱ��ɾ��
        //DropDownList1.Items.Add(new ListItem(family + "1", family + "1"));
        //DropDownList1.Items.Add(new ListItem(family + "2", family + "2"));
        //DropDownList1.Items.Add(new ListItem(family + "3", family + "3"));
        
        //ˢ��update panel
        up.Update();


    }

    public void setSelected(int index)
    {
        this.DropDownList1.SelectedIndex = index;
        up.Update();
    }

       
}
