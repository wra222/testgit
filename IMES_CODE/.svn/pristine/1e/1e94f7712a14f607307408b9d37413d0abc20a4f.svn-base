using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using com.inventec.iMESWEB;
using com.inventec.system.util;
//ITC-1136-0124 fix
//ITC-1136-0131 fix
public partial class DataMaintain_MaintainSaveAs : System.Web.UI.Page
{
    public String userName="";
    private IBOMNodeData iModelBOM;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    string OldPartNo = null;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;

    protected void Page_Load(object sender, EventArgs e)
    {
        iModelBOM = (IBOMNodeData)ServiceAgent.getInstance().GetMaintainObjectByName<IBOMNodeData>(WebConstant.IBOMNodeData);
        if (!Page.IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();

            OldPartNo = Request.QueryString["OldPartNo"];
            OldPartNo = StringUtil.decode_URL(OldPartNo);
            this.dOldPartNo.Value = OldPartNo;
            initLabel();
            userName = Request.QueryString["userName"]; //UserInfo.UserId;
            userName = StringUtil.decode_URL(userName);
            this.HiddenUserName.Value = userName;
        }
    }
    private void initLabel()
    {
        this.lblNewCode.Text = this.GetLocalResourceObject(Pre + "_lblNewCode").ToString();
        this.btnOK.Value = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnCancel.Value = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();
    }
}
