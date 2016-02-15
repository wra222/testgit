using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//using log4net;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;
using MaintainControl;
using System.Collections;
using System.Collections.Generic;


public partial class DataMaintain_AssignModelMgr : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 15;
    private IAssignModelMgr iAssignModelMgr;
    private const int COL_NUM = 8;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;

    public string today;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();

			this.cmbCustomer.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbCustomer_Selected);
            this.cmbFamily.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbFamily_Selected);

            iAssignModelMgr = (IAssignModelMgr)ServiceAgent.getInstance().GetMaintainObjectByName<IAssignModelMgr>(WebConstant.MaintainAssignModelMgrObject);
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                today ="";
                bindTable(null, DEFAULT_ROWS);
                setColumnWidth();

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

    private void initLabel()
    {

        this.lblQueryCondition.Text = this.GetLocalResourceObject(Pre + "_lblQueryCondition").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();

        this.lblStartDate.Text = this.GetLocalResourceObject(Pre + "_lblStartDate").ToString();
        
		this.lblCustomer.Text = "Customer";
		this.lblFamily.Text = "Family";
		this.lblModelInput.Text = "Model";
		
        this.lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        this.lblLine.Text = this.GetLocalResourceObject(Pre + "_lblLine").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        //this.lblTruckID.Text = this.GetLocalResourceObject(Pre + "_lblTruckID").ToString();

        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnUpdate.InnerText = this.GetLocalResourceObject(Pre + "_btnUpdate").ToString();
        this.btnDelete.InnerText = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnUpload.InnerText = this.GetLocalResourceObject(Pre + "_btnUpload").ToString();

    }

    protected void btnQuery_ServerClick(Object sender, EventArgs e)
    {
        ShowList();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "QueryComplete", "ShowRowEditInfo(null);DealHideWait();", true);
    }

    //鍥犱负閲嶅埛鏂颁篃鍙兘瑕?2涓棩鏈熷弬鏁帮紝鏃ユ湡鍙傛暟鍙湪query鏃舵斁鍏?
    private Boolean ShowList()
    {
        if (string.IsNullOrEmpty(this.dCalValue1.Value.Trim()))
        {
            bindTable(null, DEFAULT_ROWS);
            return true;
        }

        DateTime shipDate = Convert.ToDateTime(this.dCalValue1.Value.Trim());
        string model = this.cmbModel.InnerDropDownList.SelectedValue;
        try
        {
            DataTable dataList = iAssignModelMgr.GetByModelShipDate(model, shipDate);
            if (dataList == null || dataList.Rows.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataList, DEFAULT_ROWS);
            }
        }
        catch (FisException ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            //show error
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;

    }


    private void setColumnWidth()
    {
        if (gd.HeaderRow != null && gd.HeaderRow.Cells.Count > 0)
        {
            gd.HeaderRow.Cells[0].Width = Unit.Pixel(0); // ID
            gd.HeaderRow.Cells[1].Width = Unit.Pixel(30); // Line
            gd.HeaderRow.Cells[2].Width = Unit.Pixel(90); // Model
            gd.HeaderRow.Cells[3].Width = Unit.Pixel(30); // Qty
            gd.HeaderRow.Cells[4].Width = Unit.Pixel(150); // date
            gd.HeaderRow.Cells[5].Width = Unit.Pixel(90); //
            gd.HeaderRow.Cells[6].Width = Unit.Pixel(70);
            gd.HeaderRow.Cells[7].Width = Unit.Pixel(90);
            gd.HeaderRow.Cells[8].Width = Unit.Pixel(0);
            gd.HeaderRow.Cells[9].Width = Unit.Pixel(0);
        }
    }

    private void bindTable(DataTable list, int defaultRow)
    {
        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                for (int j = 0; j < list.Rows[i].ItemArray.Length; j++)
                {
                    if (list.Rows[i][j].GetType() == typeof(DateTime))
                    {
                        list.Rows[i][j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    }
                    else
                    {
                        list.Rows[i][j] = Null2String(list.Rows[i][j]);
                    }
                }
            }

            int nowCnt = list.Rows.Count;
            for (int i = nowCnt; i < DEFAULT_ROWS; i++)
            {
                list.Rows.Add(list.NewRow());
            }

            //this.hidRecordCount.Value = list.Rows.Count.ToString();
        }

        gd.DataSource = list;
        gd.DataBind();

        setColumnWidth();

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;", true);
    }

    protected void btnUpdate_ServerClick(Object sender, EventArgs e)
    {
        
    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        string oldId = this.dOldId.Value.Trim();
        try
        {
            IList<int> lst = new List<int>();
            lst.Add(Convert.ToInt16(oldId));
            iAssignModelMgr.Delete(lst);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        ShowList();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);


    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("alert('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /*for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }*/

    }
	
	
	/// <summary>
    /// 选择Customer下拉框，会刷新Family下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void cmbCustomer_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //如果选择为空
            if (this.cmbCustomer.InnerDropDownList.SelectedValue == "")
            {
                //清空Family下拉框内容
                this.cmbFamily.clearContent();

            }
            else
            {
                //刷新Family下拉框内容
                this.cmbFamily.Customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
                this.cmbFamily.refresh();
                //选择第一个选项
                this.cmbFamily.InnerDropDownList.SelectedIndex = 0;
                this.cmbModel.Family = this.cmbFamily.InnerDropDownList.SelectedValue;
                this.cmbModel.refresh();
            }

            cmbFamily_Selected(sender, e);
			//ShowModelToleranceList();
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    /// <summary>
    /// 选择Family下拉框，会刷新Model下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void cmbFamily_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //如果选择为空
            if (this.cmbFamily.InnerDropDownList.SelectedValue == "")
            {
                //清空Model下拉框内容
                this.cmbModel.clearContent();

            }
            else
            {
                //刷新Model下拉框内容
                this.cmbModel.Family = this.cmbFamily.InnerDropDownList.SelectedValue;
                this.cmbModel.refresh();
                //选择第一个选项
                this.cmbModel.InnerDropDownList.SelectedIndex = 0;

            }

            //ShowModelToleranceList();
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }


}
