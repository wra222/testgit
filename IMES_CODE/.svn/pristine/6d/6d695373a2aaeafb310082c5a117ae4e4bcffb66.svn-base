/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PACosmetic
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-20   zhu lei            Create 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections;
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
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.DataModel;

using System.Web.Services;

public partial class PAK_PACosmetic : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 6;
    public String UserId;
    public String Customer;
    public string SpeedLimit = "-1";
    public string Type = "";

    [WebMethod]
    public static IList<string> getCommSetting(string hName, string editor)
    {
        IList<string> ret = new List<string>();
        try
        {
            IList<COMSettingInfo> wsiList = ServiceAgent.getInstance().GetObjectByName<ILabelLightGuide>(WebConstant.LabelLightGuideObject).getCommSetting(hName, editor);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(wsiList[0].commPort.ToString());
            ret.Add(wsiList[0].baudRate);
            ret.Add(wsiList[0].rthreshold.ToString());
            ret.Add(wsiList[0].sthreshold.ToString());
            ret.Add(wsiList[0].handshaking.ToString());
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
           //cmbPdLine.InnerDropDownList.AutoPostBack = true;
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
            {
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
            }
            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(DEFAULT_ROWS);
                //UserId = Master.userInfo.UserId;
                //Customer = Master.userInfo.Customer;
                this.cmbPdLine.Station = Request["station"];
                this.cmbPdLine.Customer = Customer;
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }
   
    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        IPACosmetic iPACosmetic = ServiceAgent.getInstance().GetObjectByName<IPACosmetic>(WebConstant.PACosmeticObject);
        string line = cmbPdLine.InnerDropDownList.SelectedValue;
        string Station = Request["Station"].ToString();
        IList<string> ListLineSpeed = iPACosmetic.GetLineSpeed(line, Station);
        if (ListLineSpeed.Count > 0)
        {
            SpeedLimit = ListLineSpeed[2].ToString();
            Type = ListLineSpeed[3].ToString();
            //updHidden.Update();
            this.LimitSpeed.Value = SpeedLimit.ToString();
            this.TypeValue.Value = ListLineSpeed[3].ToString();
            this.SpeedExpression.Value = ListLineSpeed[4].ToString();
            this.HoldStation.Value = ListLineSpeed[5].ToString();
        }
        
    }

    private void initLabel()
    {
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.Panel3.GroupingText = this.GetLocalResourceObject(Pre + "_pnlInputDefectList").ToString();
        this.lblFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lblPdline.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
        this.lblProductId.Text = this.GetLocalResourceObject(Pre + "_lblProductid").ToString();
        this.lblCustomerSn.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSn").ToString();
        this.lblWWAN.Text = this.GetLocalResourceObject(Pre + "_lblWWAN").ToString();
        this.lblPCID.Text = this.GetLocalResourceObject(Pre + "_lblPCID").ToString();
        this.Panel1.GroupingText = this.GetLocalResourceObject(Pre + "_pnlProductInformation").ToString();
        this.Panel2.GroupingText = this.GetLocalResourceObject(Pre + "_pnlCheckItem").ToString();
        this.chk9999.Text = this.GetLocalResourceObject(Pre + "_chk9999").ToString();
        this.lblColor.Text = this.GetLocalResourceObject(Pre + "_lblColor").ToString();
        this.lbSIMCard.Text = this.GetLocalResourceObject(Pre + "_lblSIMCard").ToString();
    }

    private void showErrorMessage(string errorMsg)
    {
        bindTable(DEFAULT_ROWS);
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    } 

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDefectCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
        
        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }

        gd.DataSource = dt;
        gd.DataBind();
    }
}
