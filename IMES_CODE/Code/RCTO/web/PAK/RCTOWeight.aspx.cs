/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for RCTOWeight Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI RCTO Weight
 * UC:CI-MES12-SPEC-PAK-UC RCTO Weight
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-09-08  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
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

public partial class PAK_RCTOWeight : System.Web.UI.Page
{
    
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String Station;
    public String ByPassCheckModel;
    protected string Pcode;
    protected int isTestWeight = WebCommonMethod.isTestWeight();
    public string RCTOWeightFilePath = ConfigurationManager.AppSettings["RCTOWeightPath"].ToString(); 
    protected void Page_Load(object sender, EventArgs e)
    {
        UserId = Master.userInfo.UserId;
        Customer = Master.userInfo.Customer;
        ByPassCheckModel = Request["ByPassCheckModel"];
    }  
}