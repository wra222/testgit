/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Sevice for Asset Tag Label Reprint Page
 * UI:CI-MES12-SPEC-PAK-UI Asset Tag Label Print.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC Asset Tag Label Print.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：Product.DeliveryNo 在DeliveryInfo分别查找InfoType=‘CustPo’/‘IECSo’-数据接口已提供，需进一步调试（in Activity：GetProductByCustomerSN）
* UC 具体业务：在ProductLog不存在Station='81' and Status=1 and Line= 'ATSN Print'的记录-数据接口已提供，需进一步调试（in Activity：CheckAssetSNReprint）
* UC 具体业务：Product_Part.Value where ProductID=@prdid and PartNo in (bom中BomNodeType=’AT’对应的Pn)-数据接口已提供，需进一步调试（in Activity：GetProductByCustomerSN）
* UC 具体业务：保存product和Asset SN的绑定关系-- Insert Product_Part values(@prdid,@partpn,@astsn,’’,’AT’,@user,getdate(),getdate()) -数据接口已提供，需进一步调试（in Activity：GenerateAssetTagLabel））
* UC 具体业务：得到IE维护的Asset SN可用范围select @descr=rtrim(Descr) from Maintain (nolock) where Tp='AST' and Code=@cust ，其中@cust是Product.Model对应的ModelInfo中Name=’Cust’对应的值-数据接口尚未定义（in Activity：GenerateAssetTagLabel）
* UC 具体业务：查找CUST（其中@cust是Product.Model对应的ModelInfo中Name=’Cust’对应的值）已用过的最大号-数据接口存在协商问题（in Activity：GenerateAssetTagLabel））
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

public partial class PAK_AssetTagLabelReprint : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {    
           //2012-4-16
           UserId = Master.userInfo.UserId;
           Customer = Master.userInfo.Customer;

            if (!this.IsPostBack)
            {
                initLabel();
                
                if (Request["Station"] != null)
                {
                    this.hiddenStation.Value = Request["Station"]; //站号
                }
                else
                {
                    this.hiddenStation.Value = "";
                }
                
                this.pCode.Value = Request["PCode"];
                /*
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                 */
                this.txtDataEntry.Focus();
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
        //this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        //this.lblCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lblReason.Text = this.GetLocalResourceObject(Pre + "_lblReason").ToString();
        //this.btnPrintSetting.Text = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        //this.btnReprint.Text = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();          
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();                 
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();                                   
    }


    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
}


