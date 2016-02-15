using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;

public partial class CommonControl_CmbConstValueTypeForType : System.Web.UI.UserControl
{
    private string cssClass;
    private string width;
    private string station;
    private string customer;
    private string codetype;
    private IConstValueType iConstValueType;//IConstValueMaintain iConstValueMaintain;
    private Boolean isPercentage = false;
    private Boolean enabled = true;
    private int firstNoNullIndex = 0;
    private Boolean isSelectFirstNotNull = false;
    private string BATTCTType;
    public string Width
    {
        get { return width; }
        set { width = value; }
    }

    public string Station
    {
        get { return station; }
        set { station = value; }
    }

    public string Customer
    {
        get { return customer; }
        set { customer = value; }
    }

    public string CssClass
    {
        get { return cssClass; }
        set { cssClass = value; }
    }

    public Boolean Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    public Boolean IsPercentage
    {
        get { return isPercentage; }
        set { isPercentage = value; }
    }

    public Boolean IsSelectFirstNotNull
    {
        get { return isSelectFirstNotNull; }
        set { isSelectFirstNotNull = value; }
    }
    public string BATTCT
    {
        get { return BATTCTType; }
        set { BATTCTType = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iConstValueType = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueType>(WebConstant.ConstValueTypeObject);

            if (!this.IsPostBack)
            {
                if (isPercentage)
                {
                    if (Convert.ToInt32(width) > 100)
                    {
                        drpConstValueTypeForType.Width = Unit.Percentage(100);
                    }
                    else
                    {
                        drpConstValueTypeForType.Width = Unit.Percentage(Convert.ToDouble(width));
                    }
                }
                else
                {
                    drpConstValueTypeForType.Width = Unit.Parse(width);
                }

                this.drpConstValueTypeForType.CssClass = cssClass;
                this.drpConstValueTypeForType.Enabled = enabled;

                if (enabled)
                {
                    initCodeType();
                }
                else
                {
                    this.drpConstValueTypeForType.Items.Add(new ListItem("", ""));
                }

                firstNoNullIndex = 0;
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

    public void initCodeType()
    {
        if (iConstValueType != null)
        {
            IList<ConstValueTypeInfo> lstConstValueType = null;
            IList<ConstValueTypeInfo> query = null;
            //lstConstValueType = iConstValueType.GetConstValueTypeList();
            if (string.IsNullOrEmpty(BATTCTType))
            {
                lstConstValueType = iConstValueType.GetConstValueTypeList();
                query = (from q in lstConstValueType
                         orderby q.value
                         select q).ToList();
            }
            else
            {
                lstConstValueType = iConstValueType.GetConstValueTypeList();
                string[] arry = BATTCTType.Split(',');
                query = (from q in lstConstValueType
                         where arry.Contains(q.value)
                         orderby q.value
                         select q).ToList();
            }
            if (query != null && query.Count != 0)
            {
                initControl(query);
            }
            else
            {
                initControl(null);
            }

            //if (lstConstValueType != null && lstConstValueType.Count != 0)
            //{
            //    initControl(lstConstValueType);
            //}
            //else
            //{
            //    initControl(null);
            //}
        }
        else
        {
            initControl(null);
        }
    }

    public void refresh()
    {
        initCodeType();
        up.Update();
    }

    public void clearContent()
    {
        this.drpConstValueTypeForType.Items.Clear();
        drpConstValueTypeForType.Items.Add(new ListItem("", ""));
        up.Update();
    }

    private void initControl(IList<ConstValueTypeInfo> lstConstValueType)
    {
        ListItem item = null;

        this.drpConstValueTypeForType.Items.Clear();
        this.drpConstValueTypeForType.Items.Add(string.Empty);

        if (lstConstValueType != null)
        {
            bool firstIndexGet = false;
            int i = 1;  //去掉空行
            foreach (ConstValueTypeInfo temp in lstConstValueType)
            {
                item = new ListItem();
                item.Text = temp.value;
                item.Value = temp.value;
                this.drpConstValueTypeForType.Items.Add(item);
                if ((firstIndexGet == false))
                //if ((firstIndexGet == false) && (!string.IsNullOrEmpty(temp)))
                {
                    firstNoNullIndex = i;
                    firstIndexGet = true;
                }

                i++;
            }

            if (isSelectFirstNotNull == true)
            {
                setSelected(firstNoNullIndex);
            }
        }

    }

    public void setSelected(int index)
    {
        //this.drpConstValueTypeForType.SelectedIndex = index;
        this.drpConstValueTypeForType.SelectedIndex = 0;
        up.Update();
    }

    /*
    public int getFirstNoNullIndex()
    {
        return firstNoNullIndex;
    }
    */

    private void showCmbErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\0013", string.Empty).Replace("\0010", "\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.up, typeof(System.Object), "showCmbErrorMessage", scriptBuilder.ToString(), false);
    }

    public DropDownList InnerDropDownList
    {
        get
        {
            return this.drpConstValueTypeForType;
        }

    }
}
