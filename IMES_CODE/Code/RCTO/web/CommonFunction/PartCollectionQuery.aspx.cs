using System;
using System.Data;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.Web.UI;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
public partial class CommonFunction_PartCollectionQuery : System.Web.UI.Page
{
    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Customer;
    protected string AlertSelectLine = "";
    protected string AlertNoData = "";
    protected string AlertSuccess = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitPage();
            InitBlankGV();
        }

    }

    protected void BtnRefreshGV_Click(object sender, System.EventArgs e)
    {
        string model = HiddenModel.Value;
        try
        {
            LabelQuery.Text ="Model "+ model + " 查询完毕!";
            Customer = Request.QueryString["Customer"];
            IBOMQuery CurrentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IBOMQuery>(com.inventec.iMESWEB.WebConstant.CommonObject);
            IDictionary<string, IList<IMES.DataModel.BomItemInfo>> resultParts = CurrentService.GetBOM(Customer, model, "");
            if (resultParts == null)
            {
                InitBlankGV();
                AlertNoData = GetLocalResourceObject(Pre + "AlertNoData").ToString();
                throw new Exception(AlertNoData);
            }
            this.GV37.DataSource = GetTable("37", resultParts);
            this.GV37.DataBind();
            this.GV39.DataSource = GetTable("39", resultParts);
            this.GV39.DataBind();
            this.GV3A.DataSource = GetTable("3A", resultParts);
            this.GV3A.DataBind();
            this.GV3B.DataSource = GetTable("3B", resultParts);
            this.GV3B.DataBind();
            this.GV3C.DataSource = GetTable("3C", resultParts);
            this.GV3C.DataBind();
            this.GV3D.DataSource = GetTable("3D", resultParts);
            this.GV3D.DataBind();
            this.GV3E.DataSource = GetTable("3E", resultParts);
            this.GV3E.DataBind();
            this.GV40.DataSource = GetTable("40", resultParts);
            this.GV40.DataBind();
            this.GV8C.DataSource = GetTable("8C", resultParts);
            this.GV8C.DataBind();
            this.GVMP.DataSource = GetTable("MP", resultParts);
            this.GVMP.DataBind();
            this.GVPKCK.DataSource = GetTable("PKCK", resultParts);
            this.GVPKCK.DataBind();
            this.GVPKOK.DataSource = GetTable("PKOK", resultParts);
            this.GVPKOK.DataBind();
            this.GV3F.DataSource = GetTable("3F", resultParts);
            this.GV3F.DataBind();
            this.GV3L.DataSource = GetTable("3L", resultParts);
            this.GV3L.DataBind();
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

    private void InitBlankGV() {
        this.GV37.DataSource = GetBlankTable();
        this.GV37.DataBind();
        this.GV39.DataSource = GetBlankTable();
        this.GV39.DataBind();
        this.GV3A.DataSource = GetBlankTable();
        this.GV3A.DataBind();
        this.GV3B.DataSource = GetBlankTable();
        this.GV3B.DataBind();
        this.GV3C.DataSource = GetBlankTable();
        this.GV3C.DataBind();
        this.GV3D.DataSource = GetBlankTable();
        this.GV3D.DataBind();
        this.GV3E.DataSource = GetBlankTable();
        this.GV3E.DataBind();
        this.GV40.DataSource = GetBlankTable();
        this.GV40.DataBind();
        this.GV8C.DataSource = GetBlankTable();
        this.GV8C.DataBind();
        this.GVMP.DataSource = GetBlankTable();
        this.GVMP.DataBind();
        this.GVPKCK.DataSource = GetBlankTable();
        this.GVPKCK.DataBind();
        this.GVPKOK.DataSource = GetBlankTable();
        this.GVPKOK.DataBind();
        this.GV3F.DataSource = GetBlankTable();
        this.GV3F.DataBind();
        this.GV3L.DataSource = GetBlankTable();
        this.GV3L.DataBind();
    }
    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void InitPage()
    {
        AlertSelectLine = GetLocalResourceObject(Pre + "AlertSelectLine").ToString();
        AlertNoData = GetLocalResourceObject(Pre + "AlertNoData").ToString();
        AlertSuccess = GetLocalResourceObject(Pre + "AlertSuccess").ToString();
        this.LabelModel.Text = this.GetLocalResourceObject(Pre + "LabelModel").ToString();
    }

    private DataTable InitTable()
    {
        DataTable result = new DataTable();
        result.Columns.Add("PartNo", Type.GetType("System.String"));
        result.Columns.Add("PartType", Type.GetType("System.String"));
        result.Columns.Add("Description", Type.GetType("System.String"));
        result.Columns.Add("Qty", Type.GetType("System.String"));
        return result;
    }

    private DataTable GetBlankTable()
    {
        DataTable result = InitTable();
        for (int i = 0; i < DefaultRowsCount; i++)
        {
            DataRow newRow = result.NewRow();

            newRow[0] = string.Empty;
            newRow[1] = string.Empty;
            newRow[2] = string.Empty;
            newRow[3] = string.Empty;
            result.Rows.Add(newRow);
        }
        return result;
    }

    private DataTable GetTable(string station, IDictionary<string, IList<IMES.DataModel.BomItemInfo>> resultParts)
    {

        if (resultParts.ContainsKey(station))
        {
            IList<IMES.DataModel.BomItemInfo> currentStationParts = resultParts[station];
            if (currentStationParts != null)
            {
                DataTable result = InitTable();
                int partsCount = currentStationParts.Count;
                for (int i = 0; i < partsCount; i++)
                {
                    DataRow newRow = result.NewRow();

                    newRow[0] = currentStationParts[i].PartNoItem;
                    newRow[1] = currentStationParts[i].type;
                    newRow[2] = currentStationParts[i].description;
                    newRow[3] = currentStationParts[i].qty;
                    result.Rows.Add(newRow);
                }
                if (partsCount < DefaultRowsCount)
                {
                    for (int i = partsCount; i < DefaultRowsCount; i++)
                    {
                        DataRow newRow = result.NewRow();

                        newRow[0] = string.Empty;
                        newRow[1] = string.Empty;
                        newRow[2] = string.Empty;
                        newRow[3] = string.Empty;
                        result.Rows.Add(newRow);
                    }
                }
                return result;
            }
        }

        return GetBlankTable();

    }

    private int DefaultRowsCount = 6;
}
