/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UI SN Check.docx –2011/10/20 
 * UC:CI-MES12-SPEC-PAK-UC SN Check.docx –2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-20   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：判断是否需要检查Asset Tag SN Check：	Select b.Tp,Type,@message=Message,Sno1, @lwc=L_WC, @mwc=M_WC from Special_Det a,Special_Maintain b where a.SnoId=@Productid and b.Type=a.Tp and b.SWC=”68”得到的Tp=”K”的记录时，表示需要检查Asset Tag SN Check是否做过 (目前只考虑得到一条记录的情况??)-数据接口尚未定义（in Activity：CheckAssetTagSN）
* UC 具体业务：当BOM(存在PartType=ALC and BomNodeType=PL的part) 且model<>PC4941AAAAAY时，表示有ALC，这时没有真正的Pizza盒-数据接口尚未定义（in Activity：CheckSNIdentical）
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class PAK_CheckDoorSn : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {    
     
           UserId = UserId ?? Master.userInfo.UserId;
           Customer = Customer ?? Master.userInfo.Customer;
           if (!this.IsPostBack)
            {
                initLabel();
        
                this.cmbPdLine.Station = Request["station"];
                this.cmbPdLine.Customer = Customer;
            
            }
       }
       catch (FisException ex)
       {
           writeToAlertMessage(ex.mErrmsg);
       }
       catch (Exception ex)
       {
           writeToAlertMessage(ex.Message);
       }
    }

    private void initLabel()
    {
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblPdline.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
        this.lblCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lblDoorSn.Text = this.GetLocalResourceObject(Pre + "_lblDoorSn").ToString();
        this.lblFrameSn.Text = this.GetLocalResourceObject(Pre + "_lblFrameSn").ToString();
  
    }


    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
}


