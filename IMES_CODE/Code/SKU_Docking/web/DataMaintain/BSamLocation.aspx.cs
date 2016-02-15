using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;
using System.Collections.Generic;


public partial class DataMaintain_BSamLocation : System.Web.UI.Page
{
    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IBSamLocation iBSamLocation;
    private const int COL_NUM = 11;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage10;
    public string Editor;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            iBSamLocation = ServiceAgent.getInstance().GetMaintainObjectByName<IMES.Maintain.Interface.MaintainIntf.IBSamLocation>(WebConstant.MaintainBSamLocation);

            if (!this.IsPostBack)
            {
                IList<string> lstModel = null;
                lstModel = iBSamLocation.GetAllBSamModel();
                setcmbModel(lstModel);
                userName = Master.userInfo.UserId;
                Editor = userName;
                this.HiddenUserName.Value = userName;
                bindTable(null);
            }
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "InitControl", "", true);
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

    private void setcmbModel(IList<string> lstModel)
    {
        ListItem item = null;
        if (lstModel != null)
        {
            foreach (string temp in lstModel)
            {
                item = new ListItem(temp.ToString().Trim(), temp.ToString().Trim());
                this.cmbModel.Items.Add(item);
            }
        }

    }

    public void btnQuery_serverclick(object sender, System.EventArgs e)
    {
        try
        {
            string Model = "";
            BSamLocationQueryType BSamLocation = BSamLocationQueryType.All;
            if (SelectLocation.Checked)
            {
                if (cmbLocation.SelectedValue.ToString() != null && cmbLocation.SelectedValue.ToString() != "")
                {
                    
                    string SelectValue = cmbLocation.SelectedValue.ToString();
                    switch (SelectValue)
                    {
                        case "All"://全部
                            BSamLocation = BSamLocationQueryType.All;
                            break;
                        case "Empty"://空庫位
                            BSamLocation = BSamLocationQueryType.Empty;
                            break;
                        case "Occupy"://使用中庫位
                            BSamLocation = BSamLocationQueryType.Occupy;
                            break;
                        case "Hold"://禁用庫位
                            BSamLocation = BSamLocationQueryType.Hold;
                            break;
                    }
                    IList<BSamLocaionInfo> BSamList = iBSamLocation.GetBSamLocation(BSamLocation,Model);
                    bindTable(BSamList);
                }
            }
            else if (SelectModel.Checked)
            {
                if (cmbModel.SelectedValue.ToString() != null && cmbModel.SelectedValue.ToString() != "")
                {
                    Model = cmbModel.SelectedValue.ToString();
                    BSamLocation = BSamLocationQueryType.Model;
                    IList<BSamLocaionInfo> BSamList = iBSamLocation.GetBSamLocation(BSamLocation, Model);
                    bindTable(BSamList);
                }
            }

        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    protected void btnSave_serverclick(object sender, System.EventArgs e)
    {
        try
        {
            if (this.hidSelectId.Value != "")
            {
                Editor = Master.userInfo.UserId;
                string SelectID = this.hidSelectId.Value.Substring(0, this.hidSelectId.Value.Length - 1);
                bool isInputOrOutput = (cmb_Y_N.SelectedValue.ToString() == "Y") ? true : false;
                string[] arry = SelectID.Split(',');
                IList<string> Ids = new List<string>();
                foreach (string i in arry)
                {
                    //Ids.Add(Convert.ToInt32(i));
                    Ids.Add(i);
                }
                if (cmbIn_Out.SelectedValue == "in")
                {
                    iBSamLocation.UpdateHoldInLocation(Ids, isInputOrOutput, Editor);
                    btnQuery_serverclick(sender, e);
                }
                else if (cmbIn_Out.SelectedValue == "out")
                {
                    iBSamLocation.UpdateHoldOutLocation(Ids, isInputOrOutput, Editor);
                    btnQuery_serverclick(sender, e);
                }
            }
            else
            {
                showErrorMessage("請至少勾選一項...");
            }
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
        }
       
        this.updatePanel.Update();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelect = e.Row.FindControl("chk") as CheckBox;
            DataRowView drv = e.Row.DataItem as DataRowView;
            string id = drv["LocationId"].ToString();
            chkSelect.Attributes.Add("onclick", string.Format("setSelectVal(this,'{0}');", id));
        }
    }

    private void bindTable(IList<BSamLocaionInfo> list)
    {

        DataTable dt = setColumnName();
        DataRow dr = null;

        if (list != null && list.Count != 0)
        {

            foreach (BSamLocaionInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.LocationId;
                dr[1] = temp.Model;
                dr[2] = temp.Qty;
                dr[3] = temp.RemainQty;
                dr[4] = temp.FullQty;
                dr[5] = temp.FullCartonQty;
                dr[6] = (temp.HoldInput.ToString() == "N") ? "准許" : "禁止";//temp.HoldInput;
                dr[7] = (temp.HoldOutput.ToString() == "N") ? "准許" : "禁止";
                dr[8] = temp.Editor;
                dr[9] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[10] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                dt.Rows.Add(dr);
            }
            //for (int i = list.Count; i < DEFAULT_ROWS; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
        }
        else
        {
            dt = getNullDataTable(1);
        }
        
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true); 
    }

    public DataTable setColumnName()
    {
        DataTable dt = new DataTable();
        
        dt.Columns.Add("LocationId");
        dt.Columns.Add("Model");
        dt.Columns.Add("Qty");
        dt.Columns.Add("RemainQty");
        dt.Columns.Add("FullQty");
        dt.Columns.Add("FullCartonQty");
        dt.Columns.Add("可否入庫");
        dt.Columns.Add("可否出庫");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        return dt;
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(3);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(8);//LocationId
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(9);//Model
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(4);//Qty
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(7);//RemainQty
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(5);//FullQty
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(8);//FullCartonQty
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(5);//可否入庫
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(5);//可否出庫
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(6);//Editor
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(10);//Cdt
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(10);//Udt
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = setColumnName();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            for (int k = 0; i < 10; i++)
            {
                newRow[k] = "";
            }
            
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");

        //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

}
