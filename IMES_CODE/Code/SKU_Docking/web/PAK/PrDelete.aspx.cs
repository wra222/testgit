/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/PAK/PrDelete Page
 * UI:CI-MES12-SPEC-PAK-UI PrDelete.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PrDelete.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-16   zhanghe               (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO：
 * 
 * * XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
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
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;


public partial class PAK_PrDelete : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Pre = this.GetLocalResourceObject("language").ToString();
            this.lbPrsn.Text = this.GetLocalResourceObject(Pre + "_lblprsn").ToString();
            this.lbProId.Text = this.GetLocalResourceObject(Pre + "_lblproid").ToString();
            this.lbProSn.Text = this.GetLocalResourceObject(Pre + "_lblprosn").ToString();
            this.btdel.Value = this.GetLocalResourceObject(Pre + "_btndel").ToString();
            this.lblProdSN.Text = this.GetLocalResourceObject(Pre + "_lblProdSN").ToString();


            string station = Request["Station"];

            //ForTest
            //station = "90";
            //FotTest
            this.station1.Value = station;
            this.editor1.Value = Master.userInfo.UserId;
            this.customer1.Value = Master.userInfo.Customer;                
        }
    }
}
