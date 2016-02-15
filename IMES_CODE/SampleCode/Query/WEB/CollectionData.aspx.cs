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

    
public partial class _CollectionData : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string urlparam = String.Empty;
    public int initRowsCount = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        urlparam = Request.QueryString["page"];
        InitLabel();
        AddClientAction();
        this.GridViewExt1.DataSource = createDataSourcePallet();
        this.GridViewExt1.DataBind();
        InitGridView();
    }

    private void InitLabel()
    {
        
        this.Close.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnOK");
        this.Title = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColTitle");


    }
    private void AddClientAction()
    {
        this.Close.Attributes.Add("onclick", "OnCloseClick()");
    }

    private void InitGridView()
    {
      
        this.GridViewExt1.HeaderRow.Cells[0].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_colNoLow");
        this.GridViewExt1.HeaderRow.Cells[1].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColPartNo");
        this.GridViewExt1.HeaderRow.Cells[2].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColCollectionData");
    }

    /// <summary>
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("No", Type.GetType("System.String"));
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        retTable.Columns.Add("Data", Type.GetType("System.String"));
        return retTable;


    }

    private DataTable createDataSourcePallet()
    {

        DataTable dt = initTable();
        DataRow row;
        DataColumn col = new DataColumn();

        for (int i = 0; i < initRowsCount; i++)
        {
            row = dt.NewRow();
            row["No"] = String.Empty;
            row["PartNo"] = String.Empty;
            row["Data"] = String.Empty;

            dt.Rows.Add(row);
        }

        return dt;
    }
        

      
 

}
