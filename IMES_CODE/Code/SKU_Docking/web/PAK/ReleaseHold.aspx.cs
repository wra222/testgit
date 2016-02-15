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
using System.IO;
using System.Linq;

[Serializable]
public partial class PAK_ReleaseHold : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    IModelWeight iModelWeight = ServiceAgent.getInstance().GetObjectByName<IModelWeight>(WebConstant.ModelWeightObject);
    public int initRowsCount = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnQueryData.ServerClick += new EventHandler(btnQueryData_ServerClick);
            btnSaveData.ServerClick += new EventHandler(btnSaveData_ServerClick);
            UserId = Master.userInfo.UserId;
            if (!Page.IsPostBack)
            {
                InitLabel();
                clearGrid();
                BindDefect();
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg, false);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message, false);
        }
    }
    private void BindDefect()
    {
        List<string> lst = iModelWeight.GetDefectCodeList("RELEASE");
        droDefect.Items.Add("");
        foreach (string d in lst)
        {
            droDefect.Items.Add(new ListItem(d, d.Split('-')[0]));
        }

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        string id = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelect = e.Row.FindControl("chk") as CheckBox;
            DataRowView drv = e.Row.DataItem as DataRowView;
            id = drv["ProductID"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                chkSelect.Visible = false;
            }
            else
            {
                chkSelect.Attributes.Add("onclick", string.Format("setSelectVal(this,'{0}');", id));
                chkSelect.Attributes.Add("prdID", id);
                e.Row.Cells[0].Attributes.Add("prdID", id);
            }
            e.Row.Cells[9].Style.Add("display", "none");

        }
    }
    private IList<int> GetHoldID()
    {
        IList<int> lst = new List<int>();
        string[] arrID = hidSelectID.Value.Split(',');

        foreach (string id in arrID)
        {
            foreach (GridViewRow dr in GridViewExt1.Rows)
            {
                if (dr.Cells[2].Text.Trim() == id)
                {
                    lst.Add(int.Parse(dr.Cells[9].Text));
                    continue;
                }
            }


        }
        return lst;

    }

    private void btnSaveData_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            IList<int> holdIDlst = GetHoldID();
            IList<string> prdList = hidSelectID.Value.Split(',').ToList();
            iModelWeight.ReleaseHold(prdList, holdIDlst, hidDefect.Value, UserId);
            IList<HoldInfo> lstHoldInfo = iModelWeight.GetHoldProductList(hidModel.Value.Trim());
            BindGrv(lstHoldInfo);
            if (lstHoldInfo.Count > 0)
            {
                CallClientFunction("SaveSuccess();ShowBtn(true);");
            }
            else
            {
                CallClientFunction("SaveSuccess();ShowBtn(false)");
            }

        }

        catch (FisException ee)
        {
            clearGrid();
            writeToAlertMessage(ee.mErrmsg, true);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message, true);
        }

    }
    private void BindGrv(IList<HoldInfo> lstHoldInfo)
    {

        DataTable dt = initTable();
        DataRow newRow;
        int cnt = 0;
        if (lstHoldInfo.Count > 0)
        {
            foreach (HoldInfo holdInfo in lstHoldInfo)
            {
                newRow = dt.NewRow();
                newRow["Custsn"] = holdInfo.CUSTSN;
                newRow["ProductID"] = holdInfo.ProductID;
                newRow["PreLine"] = holdInfo.PreLine;
                newRow["PreStation"] = holdInfo.PreStation;
                newRow["HoldTime"] = holdInfo.HoldTime;
                newRow["HoldUser"] = holdInfo.HoldUser;
                newRow["HoldCode"] = holdInfo.HoldCode;
                newRow["HoldCodeDescr"] = holdInfo.HoldCodeDescr;
                newRow["HoldID"] = holdInfo.HoldID;

                dt.Rows.Add(newRow);
                cnt++;
            }
        }

        for (; cnt < initRowsCount; cnt++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        this.GridViewExt1.DataSource = dt;
        this.GridViewExt1.DataBind();
        initTableColumnHeader();

    }


    private void btnQueryData_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            IList<HoldInfo> lstHoldInfo = iModelWeight.GetHoldProductList(hidModel.Value.Trim());
            BindGrv(lstHoldInfo);
            if (lstHoldInfo.Count > 0)
            {
                CallClientFunction("endWaitingCoverDiv();ShowBtn(true);");
            }
            else
            {
                CallClientFunction("ShowInfo('No Data');endWaitingCoverDiv();ShowBtn(false)");
            }
        }
        catch (FisException ee)
        {
            clearGrid();
            writeToAlertMessage(ee.mErrmsg, true);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message, true);
        }

    }

    private void InitLabel()
    {
        //this.lblFrom.Text = this.GetLocalResourceObject(Pre + "_lblFrom").ToString();
        //this.lblTo.Text = this.GetLocalResourceObject(Pre + "_lblTo").ToString();
        //this.chkAllData.Text = this.GetLocalResourceObject(Pre + "_chkAllData").ToString();    
    }

    private DataTable getNullDataTable()
    {

        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Chk");
        retTable.Columns.Add("Custsn", Type.GetType("System.String"));
        retTable.Columns.Add("ProductID", Type.GetType("System.String"));
        retTable.Columns.Add("PreLine", Type.GetType("System.String"));
        retTable.Columns.Add("PreStation", Type.GetType("System.String"));
        retTable.Columns.Add("HoldTime", Type.GetType("System.String"));
        retTable.Columns.Add("HoldUser", Type.GetType("System.String"));
        retTable.Columns.Add("HoldCode", Type.GetType("System.String"));
        retTable.Columns.Add("HoldCodeDescr", Type.GetType("System.String"));
        retTable.Columns.Add("HoldID", Type.GetType("System.String"));
        return retTable;
    }

    private void initTableColumnHeader()
    {

        this.GridViewExt1.HeaderRow.Cells[1].Text = "Custsn";
        this.GridViewExt1.HeaderRow.Cells[2].Text = "ProductID";
        this.GridViewExt1.HeaderRow.Cells[3].Text = "PreLine";
        this.GridViewExt1.HeaderRow.Cells[4].Text = "PreStation";
        this.GridViewExt1.HeaderRow.Cells[5].Text = "HoldTime";
        this.GridViewExt1.HeaderRow.Cells[6].Text = "HoldUser";
        this.GridViewExt1.HeaderRow.Cells[7].Text = "HoldCode";
        this.GridViewExt1.HeaderRow.Cells[8].Text = "HoldCodeDescr";
        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Percentage(7);
        this.GridViewExt1.HeaderRow.Cells[4].Width = Unit.Percentage(7);
        this.GridViewExt1.HeaderRow.Cells[6].Width = Unit.Percentage(7);
        this.GridViewExt1.HeaderRow.Cells[7].Width = Unit.Percentage(7);

        this.GridViewExt1.HeaderRow.Cells[9].Style.Add("display", "none");
    }

    private void clearGrid()
    {
        try
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            this.gridViewUP.Update();
            initTableColumnHeader();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg, true);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message, false);
        }
    }
    private void CallClientFunction(string funcName)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine(funcName);
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.gridViewUP, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void writeToAlertMessage(string errorMsg, bool isEndWaitingCoverDiv)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        if (isEndWaitingCoverDiv)
        {
            scriptBuilder.AppendLine("endWaitingCoverDiv();");
        }
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.gridViewUP, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.gridViewUP, ClientScript.GetType(), "clearInfo", script, false);
    }




    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

}
