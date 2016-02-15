
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Station.Interface.StationIntf;

public partial class FA_PilotRunMO : System.Web.UI.Page
{
    //public string[] GvQueryColumnName = { "Pilot MO","Stage","Mo Type","Model","Part No",
    //                                        "Vendor","Qty","Plan Start Time","Cause Descr","State",
    //                                        "CombinedQty","Remark","Editor","Cdt","Udt"};
    //public int[] GvQueryColumnNameWidth = { 120,50,70,110,110,
    //                                        110,45,120,100,50,
    //                                        100,100,90,160,160};


    public string[] GvQueryColumnName = { "Pilot MO","Stage","Mo Type","Model","Part No",
                                            "Vendor","Cause Descr","Qty","State","CombinedQty",
                                            "Plan Start Time","Remark","Editor","Cdt","Udt"};
    public int[] GvQueryColumnNameWidth = { 120,50,70,110,330,
                                            330,110,45,50,90,
                                            120,100,90,160,160};
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IReleaseProductIDHold iReleaseProductIDHold = ServiceAgent.getInstance().GetObjectByName<IReleaseProductIDHold>(WebConstant.ReleaseProductIDHoldObject);
    private IPilotRunMO iPilotRunMO = ServiceAgent.getInstance().GetObjectByName<IPilotRunMO>(WebConstant.PilotRunMOObject);
    private const int DEFAULT_ROWS = 6;
    private const int COL_NUM = 15;
    public String UserId;
    public String Customer;
    public String GuidCode;
    public String AccountId;
    public String Login;
    public string HoldStationValue;
    public string[] HoldStationList;
    public string fromday;
    public string today;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
            {
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                AccountId = Master.userInfo.AccountId.ToString();
                Login = Master.userInfo.Login;
            }
            if (!this.IsPostBack)
            {
                HoldStationValue = Request["HoldStation"] ?? "";
                this.btnRelease.Disabled = true;
                this.btnHold.Disabled = true;
                this.btnSave.Disabled = true;
                initLabel();
                initMoType();
                bindTable(null);
            }
            fromday = DateTime.Now.ToString("yyyy-MM-dd");
            today = DateTime.Now.ToString("yyyy-MM-dd");
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
   
    private void initLabel()
    {
        this.Panel3.GroupingText = "Pilot Mo List:";
    }

    private void initMoType()
    {
        try
        {
            this.cmbMOType.Items.Clear();
            //this.cmbMOType.Items.Add(string.Empty);
            IList<ConstValueInfo> lst = iPilotRunMO.GetMOTypeList("PilotMoType");
            foreach (ConstValueInfo item in lst)
            {
                this.cmbMOType.Items.Add(item.name);
            }
            this.UpdatePanel2.Update();
        }
        catch (FisException exp)
        {
            showErrorMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            showErrorMessage(exp.Message.ToString());
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        bindTable(null);
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void bindTable(IList<PilotMoInfo> list)
    {
        DataTable dt = initTable();
        DataRow dr = null;
        if (list != null && list.Count != 0)
        {
            foreach (PilotMoInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.mo;
                dr[1] = temp.stage;
                dr[2] = temp.moType;
                dr[3] = temp.model;
                dr[4] = temp.partNo;
                dr[5] = temp.vendor;
                dr[6] = temp.causeDescr;
                dr[7] = temp.qty;
                dr[8] = temp.state;
                dr[9] = temp.combinedQty;
                dr[10] = temp.planStartTime.ToString("yyyy-MM-dd");
                dr[11] = temp.remark;
                dr[12] = temp.editor;
                dr[13] = temp.cdt.ToString("yyyy-MM-dd-HH:mm:ss");
                dr[14] = temp.udt.ToString("yyyy-MM-dd-HH:mm:ss");
                dt.Rows.Add(dr);
            }
            for (int i = dt.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }
        else
        {
            dt = getNullDataTable(DEFAULT_ROWS);
        }

        gd.DataSource = dt;
        gd.DataBind();
        InitGridView();
        this.UpdatePanel1.Update();
    }

    private void InitGridView()
    {
        for (int i = 0; i < GvQueryColumnNameWidth.Count(); i++)
        {
            gd.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
        }
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            for (int k = 0; k < 11; k++)
            {
                newRow[k] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        for (int i = 0; i < GvQueryColumnName.Count(); i++)
        {
            retTable.Columns.Add(GvQueryColumnName[i], System.Type.GetType("System.String"));
        }
        return retTable;
    }
    
    protected void btnQuery_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string beginday = this.hidfromday.Value;
            string endday = this.hidtoday.Value;
            string motype = this.cmbMOType.SelectedValue.ToString().Trim();
            IList<PilotMoInfo> ret = new List<PilotMoInfo>();
            PilotMoInfo condition = new PilotMoInfo();
            condition.moType = motype;
            ret = iPilotRunMO.GetPilotMoList(condition,beginday,endday);
            bindTable(ret);
            this.UpdatePanel1.Update();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "Query", "setNewItemValue();", true);
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
    }

    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string pilotmo = this.hidpilotmo.Value.ToString();
            //string pilotmo = this.lblPilotMOContent.Text.Trim();
            int qty = Convert.ToInt32(this.txtQty.Text.ToString());
            int combineqty = Convert.ToInt32(this.hidcombineqty.Value.ToString());
            if (qty == 0 && combineqty == 0)
            {
                iPilotRunMO.DeletePilotMO(pilotmo);
            }
            else
            { 
                PilotMoInfo item = new PilotMoInfo();
                item.mo = pilotmo;
                item.qty = qty;
                if (qty == combineqty)
                {
                    item.combinedState = PilotMoCombinedStateEnum.Full.ToString();
                }
                else if (combineqty == 0)
                {
                    item.combinedState = PilotMoCombinedStateEnum.Empty.ToString();
                }
                else if (combineqty > 0)
                {
                    item.combinedState = PilotMoCombinedStateEnum.Partial.ToString();
                }
                iPilotRunMO.UpdatePilotMO(item);
            }
           
            btnQuery_ServerClick(sender, e);
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
    }

    protected void btnHole_ServerClick(object sender, EventArgs e)
    {
        try
        {
            PilotMoInfo item = new PilotMoInfo();
            item.mo = this.hidpilotmo.Value.ToString();
            item.state = "Hold";
            iPilotRunMO.UpdatePilotMO(item);
            btnQuery_ServerClick(sender, e);
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
    }

    protected void btnRelease_ServerClick(object sender, EventArgs e)
    {
        try
        {
            PilotMoInfo item = new PilotMoInfo();
            item.mo = this.hidpilotmo.Value.ToString();
            item.moType = this.cmbMOType.SelectedValue.ToString().Trim();
            item.state = "Release";
            iPilotRunMO.ReleasePilotMO(item);
            btnQuery_ServerClick(sender, e);
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
        //for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        //{
        //    e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }
}
