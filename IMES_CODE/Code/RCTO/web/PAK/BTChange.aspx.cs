/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for BT Change Page
 * UI:CI-MES12-SPEC-PAK-BT_CHANGE.docx –2011/10/28 
 * UC:CI-MES12-SPEC-PAK-BT_CHANGE.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-28   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：确认不是一个已存在的Model[数据表ProductBT：栏位：ProductID（char（10）），BT（varchar（50）），Editor，Cdt,Udt]-接口待测试（in Activity：CheckModelValid）
* UC 具体业务：查找符合条件的Product：1） ProductStatus.Line的Stage=“FA” 2） ProductStatus.Line的Stage=“PAK”并且ProductStatus.Station=“69”-接口待测试（in Activity：GetProductByProductStatus）
* UC 具体业务：BT 转 非BT：删除ProductBT表中符合条件的Product-接口待测试（in Activity：BTChangeToUnBT）
* UC 具体业务：非BT 转 BT：将符合条件的Product添加到ProductBT表中ProductBT.BT ='BT'+convert(nvarchar(20),getdate(),112)-接口待测试（in Activity：unBTChangeToBT）
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

public partial class PAK_BTChange : System.Web.UI.Page
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
                this.hiddenStation.Value = Request["Station"];
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
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();   
        this.lblBTToUnBT.Text = this.GetLocalResourceObject(Pre + "_lblBTToUnBT").ToString();
        this.lblUnBTToBT.Text = this.GetLocalResourceObject(Pre + "_lblUnBTToBT").ToString();                        
    }


    public void btnHidden_Click(object sender, System.EventArgs e)
    {
        try
        {
            this.lblDataEntry.Text = "";
            this.lblModel.Text = "";
            this.txtDataEntry.Focus();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
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


