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
using System.IO;
using IMES.Entity.Repository.Meta.IMESSKU;

public partial class Query_ECOModelQuery : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;

    // IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);

    //IFA_rpt_sqeweeklyreport FA_rpt_sqeweeklyreport = ServiceAgent.getInstance().GetObjectByName<IFA_rpt_sqeweeklyreport>(WebConstant.IFA_rpt_sqeweeklyreport);
    IFA_ECOModelQuery iFA_ECOModelQuery = ServiceAgent.getInstance().GetObjectByName<IFA_ECOModelQuery>(WebConstant.IFA_ECOModelQuery);
    public string[] GvQueryColumnName = {"ID", "ECRNo","ECONo", "Model", "ValidateFromDate", 
                                            "Plant", "PreStatus", "Status", "Remark", "Editor",
                                            "Cdt","Udt"};
    public int[] GvQueryColumnNameWidth = { 5, 75,75, 75, 90,  
                                            30,50, 50, 70,30,
                                            90,90};

    string customer = "";
    //String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //DBConnection = CmbDBType.ddlGetConnection();
            
            customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
                //InitPage();
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
        ///this.lblDate.Text = this.GetLocalResourceObject("eng_lblDate").ToString();
        //this.lblFrom.Text = this.GetLocalResourceObject("eng_lblFrom").ToString();
        //this.lblTo.Text = this.GetLocalResourceObject("eng_lblTo").ToString();
        //this.btnQuery.InnerText = this.GetLocalResourceObject("eng_btnQuery").ToString();
        //this.btnExport.InnerText = this.GetLocalResourceObject("eng_btnExcel").ToString();

    }

    private void InitCondition()
    {
        txtFromDate.Text = "";// DateTime.Now.ToString("yyyy-MM-dd");
        txtToDate.Text = "";// DateTime.Now.ToString("yyyy-MM-dd");
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
    }
    private void InitGridView()
    {
        for (int i = 0; i < GvQueryColumnNameWidth.Length; i++)
        {
            gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
        }
    }
    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            foreach (string columnname in GvQueryColumnName)
            {
                newRow[columnname] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        foreach (string columnname in GvQueryColumnName)
        {
            retTable.Columns.Add(columnname, Type.GetType("System.String"));
        }

        return retTable;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        try
        {
            beginWaitingCoverDiv(this);
            string Model = this.txtModelTop.Value.Trim();
            string from = this.txtFromDate.Text;
            string to = this.txtToDate.Text;
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;
           
            if(!string.IsNullOrEmpty(from))
            {
                fromDate = DateTime.Parse(from + " 00:00:00");
            }
            if(!string.IsNullOrEmpty(to))
            {
                toDate = DateTime.Parse(to + " 23:59:59");
            }
            if (fromDate > toDate)
            {
                throw new Exception("Error : From Date > To Date!!");
            }

            IList<ECOModelInfo> ecoModellist = iFA_ECOModelQuery.GetECOModelList(Model, fromDate, toDate);
            DataTable dt = ListToDataTable(ecoModellist);
            if (dt.Rows.Count > 0)
            {
                gvResult.DataSource = dt;
                gvResult.DataBind();
                InitGridView();
                //EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                BindNoData();
                showErrorMessage("Not Found Any Information!!");
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData();
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string id = this.hidID.Value;
            string changeStatus = this.cmbChangeStatus.SelectedValue;
            string remark = this.txtRemark.Value.Trim();
            IList<ECOModelInfo> ecoModellist = iFA_ECOModelQuery.GetECOModelList(int.Parse(id));
            ECOModelInfo item = new ECOModelInfo();
            //item = ecoModellist[0];
            //ECOModel item = new ECOModel();
            item.Udt = DateTime.Now;
            item.PreStatus = ecoModellist[0].Status;
            item.Status = changeStatus;
            item.Remark = remark;
            item.Editor = Master.userInfo.UserId;
            item.Cdt = ecoModellist[0].Cdt;
            item.Plant = ecoModellist[0].Plant;
            item.ValidateFromDate = ecoModellist[0].ValidateFromDate;
            item.ECONo = ecoModellist[0].ECONo;
            item.ECRNo = ecoModellist[0].ECRNo;
            item.Model = ecoModellist[0].Model;
            item.ID = ecoModellist[0].ID;
            IList<ECOModelInfo> list = iFA_ECOModelQuery.SaveECOModelChange(item);
            DataTable dt = ListToDataTable(list);
            if (dt.Rows.Count > 0)
            {
                gvResult.DataSource = dt;
                gvResult.DataBind();
                InitGridView();
                //EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                BindNoData();
                showErrorMessage("Not Found Any Information!!");
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData();
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
    }

    private DataTable ListToDataTable(IList<ECOModelInfo> ecoModellist)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        foreach (ECOModelInfo item in ecoModellist)
        {
            newRow = dt.NewRow();
            newRow["ID"]=item.ID;
            newRow["ECRNo"] = item.ECRNo;
            newRow["ECONo"] = item.ECONo;
            newRow["Model"] = item.Model;
            newRow["ValidateFromDate"] = item.ValidateFromDate.ToString("yyyy-MM-dd hh:mm:ss");
            newRow["Plant"] = item.Plant;
            newRow["PreStatus"] = item.PreStatus;
            newRow["Status"] = item.Status;
            newRow["Remark"] = item.Remark;
            newRow["Editor"] = item.Editor;
            newRow["Cdt"] = item.Cdt.ToString("yyyy-MM-dd hh:mm:ss");
            newRow["Udt"] = item.Udt.ToString("yyyy-MM-dd hh:mm:ss");
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private void BindNoData()
    {
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
        //EnableBtnExcel(this, false, btnExport.ClientID);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvResult, Page.Title, Page);
    }
}
