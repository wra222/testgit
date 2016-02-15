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

public partial class Query_FA_QueryByPartSN : IMESQueryBasePage
{
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IFA_QueryByPartSN FA_QueryByPartSN = ServiceAgent.getInstance().GetObjectByName<IFA_QueryByPartSN>(WebConstant.FA_QueryByPartSN);
    static string DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {


        try
        {
            DataTable dt = FA_QueryByPartSN.GetInfo(DBConnection, hidInput.Value.Trim());
            DataTable dt2 = null;
            gvResult.DataSource = dt;
            if (dt.Rows.Count > 0)
            {
                string id = dt.Rows[0]["ProductID"].ToString();
                gvResult.DataBind();
                 dt2 = FA_QueryByPartSN.GetProduct(DBConnection, id);
                Gr2.DataSource = dt2;
                Gr2.DataBind();
                gvResult.Visible = true;

                Gr2.Visible = true;
            }
            else
            {
                gvResult.Visible = false;
              
                Gr2.Visible = false;
                // BindNoData(dt2, Gr2);
                showErrorMessage("No Data!");
                // TxtPartSN.Text = "";
            }
        }
        catch(Exception ex)
        {
            showErrorMessage(ex.Message);
        
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
       
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    protected void gvResult0_SelectedIndexChanged(object sender, EventArgs e)
    {


    }
}
