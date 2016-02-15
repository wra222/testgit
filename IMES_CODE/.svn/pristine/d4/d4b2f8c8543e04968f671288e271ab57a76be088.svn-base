using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
//using IMES.Station.Interface.CommonIntf;
using System.Data;

public partial class Query_FA_HPIMESStationQuery : System.Web.UI.Page
{
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();

    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        DataTable dt = QueryCommon.GetStationDescr(txtStation.Text.Trim(), DBConnection);
      gvResult.DataSource = dt;
      gvResult.DataBind();
    }

    //private void InitGridView()
    //{
    //    int i = 100;
    //    int j = 80;
    //    gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(120);
    //    gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(i);
      
    //}
}
