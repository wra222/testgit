 //Description: MO DropdownList
 //Update: 
 //Date         Name                Reason 
 //==========   ==================  =====================================    
 //2009-10-15   Lucy.Liu(EB1)       create
 //2012-01-09   zhu lei             Modify ITC211026
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


public partial class CommonControl_MOControl : System.Web.UI.UserControl
{
     //combobox���
    private String length = "300";

    //combobox��ʽ
    private String css;

    //combobox����Ƿ�֧�ְٷֱ��趨
    private Boolean isPercentage = false;

    private IMO imoService = null;

    //combobox�Ƿ����,Ĭ��Ϊ����
    private Boolean enabled = true;

    //��������ؼ����ĸ�ҳ������
    private String belongPage = "";

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
    /// Ϊcombobox�ؼ����BelongPage���ԣ���ʹ����ֱ�Ӹ�ֵ
    /// </summary>
    public String BelongPage
    {
        get
        {
            return belongPage;
        }
        set
        {
            belongPage = value;
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
    public void refreshDropContent(String model)
    {
        //�������combobox����
        DropDownList1.Items.Clear();

        //��������������ȡ����
        DropDownList1.Items.Add(new ListItem("", ""));
        
        
        //if (belongPage == "")
        //{
        //    //��Ȼ�ù��õĽӿڻ�ȡ
        //    //imoService = (IMO)ServiceAgent.getInstance().GetObjectByName(WebConstant.IMO_SERVICE);

        //    //List<MOInfo> moList = this.imoService.getMOList(model);
        //    //if (!(moList == null) && (moList.Count > 0))
        //    //{
        //    //    foreach (MOInfo mo in moList)
        //    //    {
        //    //        DropDownList1.Items.Add(new ListItem(mo.friendlyName, mo.id));
        //    //    }
        //    //}
        //    //���´����ǲ����õģ�ʹ��ʱ��ɾ��
        //    DropDownList1.Items.Add(new ListItem(model + "1", model + "1"));
        //    DropDownList1.Items.Add(new ListItem(model + "2", model + "2"));
        //    DropDownList1.Items.Add(new ListItem(model + "3", model + "3"));
        //}
        //else
        //{
        //    //imoService = (IMO)ServiceAgent.getInstance().GetObjectByName(WebConstant.IProIdPrint_SERVICE);

        //    //List<MOInfo> moList = this.imoService.getMOList(model);
        //    //if (!(moList == null) && (moList.Count > 0))
        //    //{
        //    //    foreach (MOInfo mo in moList)
        //    //    {
        //    //        DropDownList1.Items.Add(new ListItem(mo.friendlyName, mo.id));
        //    //    }
        //    //}
        //    //���´����ǲ����õģ�ʹ��ʱ��ɾ��
        //    DropDownList1.Items.Add(new ListItem("belong" + "1", "belong" + "1"));
        //    DropDownList1.Items.Add(new ListItem("belong" + "2", "belong" + "2"));
        //    DropDownList1.Items.Add(new ListItem("belong" + "3", "belong" + "3"));
        //}
       
        if (_service == "014")
        {
            imoService = ServiceAgent.getInstance().GetObjectByName<IMO>(WebConstant.ProIdPrintObject);
        } else if (_service == "013")
        {            
            imoService = ServiceAgent.getInstance().GetObjectByName<IMO>(WebConstant.TravelCardPrint2012Object);
        }
        else if (_service == "002")
        {
            ISMTMO iSMTMO = ServiceAgent.getInstance().GetObjectByName<ISMTMO>(WebConstant.MBLabelPrintObject);
            IList<SMTMOInfo> smtmoList = iSMTMO.GetSMTMOList(model);
            if (!(smtmoList == null) && (smtmoList.Count > 0))
            {
                foreach (SMTMOInfo smtmo in smtmoList)
                {
                    DropDownList1.Items.Add(new ListItem(smtmo.friendlyName, smtmo.id));
                }
            }

        }
        //add by zhulei
        else if (_service == "006")
        {
            ISMTMO iSMTMO = ServiceAgent.getInstance().GetObjectByName<ISMTMO>(WebConstant.CommonObject);
            IList<SMTMOInfo> smtmoList = iSMTMO.GetSmtMoListByPno(model);
            if (!(smtmoList == null) && (smtmoList.Count > 0))
            {
                foreach (SMTMOInfo smtmo in smtmoList)
                {
                    DropDownList1.Items.Add(new ListItem(smtmo.friendlyName, smtmo.id));
                }
            }

        }
        else
        {
            imoService = ServiceAgent.getInstance().GetObjectByName<IMO>(WebConstant.CommonObject);

        }

        if (imoService != null &&  _service != "002" && _service != "006")
        {
           
           IList<MOInfo> moList = imoService.GetMOList(model);
     
            if (!(moList == null) && (moList.Count > 0))
            {
                foreach (MOInfo mo in moList)
                {
                    DropDownList1.Items.Add(new ListItem(mo.friendlyName, mo.id));
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
