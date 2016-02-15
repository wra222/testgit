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
using System.Data;

public partial class Query_FA_ModelBOM : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IFA_ModelBOM ModelBOM = ServiceAgent.getInstance().GetObjectByName<IFA_ModelBOM>(WebConstant.ModelBOM);
    string Connection = "";
    DataTable dtModelBOM = new DataTable();
    public string[] GvModelInfoColumnName = { "Model","Name","Value","Editor", "Udt" };
    public int[] GvModelInfoColumnWidth = { 100, 80, 250, 100, 150 };

    public string[] GvPartInfoInfoColumnName = { "id", "PartNo", "InfoType", "InfoValue", "Editor", "Udt" };
    public int[] GvPartInfoColumnWidth = { 20, 100, 150, 200, 100, 150 };

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Connection = CmbDBType.ddlGetConnection();
            if (!this.IsPostBack)
            {
                InitPage();
                InitCondition();
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }


    private void InitPage()
    {
        this.lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        this.lblPno.Text = this.GetLocalResourceObject(Pre + "_lblPno").ToString();        
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();        
    }

    private void InitCondition()
    {
        try
        {
            tvModelBOM.Nodes.Clear();            
            //TreeNode tr = new TreeNode();
            //GenerateTreeMenu(0, tr);            
            tvModelBOM.Visible = false;
            tvModelBOM.ShowExpandCollapse = true;
            this.gvModelInfo.DataSource = getNullDataTable_Modelnfo(1);
            this.gvModelInfo.DataBind();

            this.gvPartInfo.DataSource = getNullDataTable_PartInfo(1);
            this.gvPartInfo.DataBind();


            InitGridView();
        }
        catch (Exception ex)
        {
            this.Response.Write(ex.Message);
        }
    }

    private void InitGridView()
    {
        for (int i = 0; i < GvModelInfoColumnName.Length; i++)
        {
            gvModelInfo.HeaderRow.Cells[i].Width = Unit.Pixel(GvModelInfoColumnWidth[i]);
        }
        InitGvPartInfoView();
    }
    private void InitGvPartInfoView() {
        for (int i = 0; i < GvPartInfoInfoColumnName.Length; i++)
        {
            gvPartInfo.HeaderRow.Cells[i].Width = Unit.Pixel(GvPartInfoColumnWidth[i]);
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
    
    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {        
        try
        {            
            if (txtModel.Text.Trim() == "")
            {
                showErrorMessage("Please input any Model !!");
                endWaitingCoverDiv();
                return;
            }
            TreeNode tr = new TreeNode();
            tvModelBOM.Nodes.Clear();

            GenerateTreeRoot(txtModel.Text.Trim().ToUpper(),tr);
            tvModelBOM.Visible = true;
            DataTable dtInfo = ModelBOM.GetModelInfo(Connection, txtModel.Text.Trim().ToUpper());
            gvModelInfo.DataSource = dtInfo;
            gvModelInfo.DataBind();
            InitGridView();
        }
        catch (Exception ex)
        {
            clearpage();
            showErrorMessage(ex.Message);
        }

        endWaitingCoverDiv();
    }

    private void GenerateTreeRoot(string ParentName, TreeNode rootNode){
         tvModelBOM.Nodes.Clear();

         dtModelBOM = ModelBOM.GetModelBOM(Connection, txtModel.Text.Trim());
         TreeNode tr = new TreeNode();         
         tr.Text = "[0]" + ParentName;
         tvModelBOM.Nodes.Add(tr);
         tr.Expanded = true;
         tr.SelectAction = TreeNodeSelectAction.None;

         GenerateTreeMenu(ParentName,tr,false);
    }

    private void GenerateTreeMenu(string ParentName, TreeNode rootNode, bool root)
    {

        DataRow[] dr = dtModelBOM.Select(string.Format("Material = '{0}'", ParentName));


        for (int i = 0; i < dr.Length; i++)
        {
            TreeNode tr = new TreeNode();
            object[] rootData = dr[i].ItemArray;

            //tr.Value = rootData[1].ToString().Trim();

            if (root)//根節點
            {
                tr.Text = rootData[1].ToString() ; //
                tvModelBOM.Nodes.Add(tr);
                tr.Expanded = true;
                tr.SelectAction = TreeNodeSelectAction.None;
            }
            else
            {
                tr.Text = " [" + rootData[4].ToString().Trim() + "] " + rootData[1].ToString().Trim() + " ( ";
                if (rootData[2].ToString().Trim() != "")
                {
                    tr.Text += "BomNodeType : " + rootData[2].ToString().Trim();
                }
                if (rootData[3].ToString().Trim() != "")
                {
                    tr.Text += " , PartType : " + rootData[3].ToString().Trim();
                }
                tr.Text += " ) ";
                tr.Value = rootData[1].ToString().Trim();

                
                rootNode.ChildNodes.Add(tr);
                tr.Expanded = true;                
                tr.SelectAction = TreeNodeSelectAction.Select;
                
            }

            GenerateTreeMenu(rootData[1].ToString().Trim(), tr, false);
        }
    }

    protected void tvModelBOM_SelectedNodeChanged(object sender, EventArgs e)
    {

        string PartNo = tvModelBOM.SelectedNode.Value;
        DataTable dt = ModelBOM.GetPartInfo(Connection, PartNo);
        if (dt.Rows.Count > 0) {
            gvPartInfo.DataSource = dt;
            gvPartInfo.DataBind();
        } else {
            gvPartInfo.DataSource = getNullDataTable_PartInfo(1);
            gvPartInfo.DataBind();
        }
        InitGvPartInfoView();        
    }

    private DataTable getNullDataTable_Modelnfo(int j)
    {
        DataTable dt = initModelInfoTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow = dt.NewRow();
            foreach (string columnname in GvModelInfoColumnName)
            {
                newRow[columnname] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }   
    private DataTable getNullDataTable_PartInfo(int j)
    {
        DataTable dt = initPartInfoTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow = dt.NewRow();
            foreach (string columnname in GvPartInfoInfoColumnName)
            {
                newRow[columnname] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initModelInfoTable()
    {
        DataTable retTable = new DataTable();
        foreach (string columnname in GvModelInfoColumnName)
        {
            retTable.Columns.Add(columnname, Type.GetType("System.String"));
        }
        return retTable;
    }
    private DataTable initPartInfoTable()
    {
        DataTable retTable = new DataTable();
        foreach (string columnname in GvPartInfoInfoColumnName)
        {
            retTable.Columns.Add(columnname, Type.GetType("System.String"));
        }
        return retTable;
    }

    private void clearpage() {
        tvModelBOM.Nodes.Clear();
        gvModelInfo.DataSource = getNullDataTable_Modelnfo(1);
        gvModelInfo.DataBind();
        gvPartInfo.DataSource = getNullDataTable_PartInfo(1);
        gvPartInfo.DataBind();
    }
}
