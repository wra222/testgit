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

public partial class SA_PCATestStationTrans : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initRowsCount = 6;
    public string userId;
    public string customer;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        try {
            //initTableColumnHeader();
            //ע��pdLine�������ѡ���¼�
            //this.cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);

            //ע��test station�������ѡ���¼�
            this.cmbTestStation.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbTestStation_Selected);

            if (!Page.IsPostBack)
            {
                Master.NeedPrint = false;
                InitLabel();
                initTableColumnHeader();
                //�󶨿ձ��
                this.gridview.DataSource = getNullDataTable();
                this.gridview.DataBind();
                //ViewState["ds"] = this.gridview.DataSource;
                initTableColumnHeaderforMBCode();
                this.gridviewMB.DataSource = getNullDataTableForMBCode();
                this.gridviewMB.DataBind();
                ////��pdLine combo box�ĳ�ʼ����������
                //this.cmbPdLine.Station = Request["Station"];
                //this.cmbPdLine.Station = "20";
                this.cmbTestStation.Type = "SATEST";
                //this.station.Value = Request["Station"];
                //this.station.Value = "20";
                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;
                this.useridHidden.Value = userId;
                this.customerHidden.Value = customer;
                //����UC��Ա��Ҫ�󣬳�ʼʱ������Line 2/25
                //this.cmbPdLine.Customer = customer;
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    /// <summary>
    /// ѡ��pdLIne����testStation�����򣬻�reset����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbTestStation_Selected(object sender, System.EventArgs e)
    {
        //try
        //{
          
        //    //ˢ��MO����������
        //    if (this.cmbTestStation.InnerDropDownList.SelectedValue == "")
        //    {
        //        //���MO����������
        //        this.cmbPdLine.clearContent();
             
        //    }
        //    else
        //    {
        //        this.cmbPdLine.refresh(this.cmbTestStation.InnerDropDownList.SelectedValue, UserInfo.Customer);
        //    }
        //}
        //catch (FisException ee)
        //{
        //    writeToAlertMessage(ee.mErrmsg);
        //}
        //catch (Exception ex)
        //{
        //    writeToAlertMessage(ex.Message);
        //}
        try
        {
            if (this.cmbTestStation.InnerDropDownList.SelectedValue == "")
            { this.cmbPdLine.clearContent(); }
            else
            {
                var selecttstation = cmbTestStation.InnerDropDownList.SelectedValue;
                if (selecttstation.ToUpper() == "1A")//��1A����Fru Runin Test
                {
                    String script = "<script language='javascript'>" + "\r\n" +
                         "window.setTimeout (FruRuninTest,100);" + "\r\n" +
                         "</script>";
                    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "FruRuninTest", script, false);
                }
                else
                {
                    var selectLine = cmbPdLine.InnerDropDownList.SelectedItem.Text;
                    this.cmbPdLine.refresh(this.cmbTestStation.InnerDropDownList.SelectedValue, this.customerHidden.Value);
                    int i = 0;
                    foreach (ListItem lst in this.cmbPdLine.InnerDropDownList.Items)
                    {
                        if (selectLine == lst.Text)
                        {
                            cmbPdLine.InnerDropDownList.SelectedIndex = i;
                            return;
                        }
                        i++;
                    }
                }
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }


    }

    /// <summary>
    /// ��ʼ��ҳ��ľ�̬label
    /// </summary>
    private void InitLabel()
    {
        this.lbpdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbtestStation.Text = this.GetLocalResourceObject(Pre + "_lblTestStation").ToString();
        this.lbPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lbFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lbMBSn.Text = this.GetLocalResourceObject(Pre + "_lblMBSn").ToString();
        this.lb111.Text = this.GetLocalResourceObject(Pre + "_lbl111").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lbDefectList.Text = this.GetLocalResourceObject(Pre + "_lblDefectList").ToString();
        this.nineSelect.Text = this.GetLocalResourceObject(Pre + "_nineSelect").ToString();//"Dont Scan 9999";
        this.lblPromptTrans.Text = this.GetLocalResourceObject(Pre + "_PromptTrans").ToString();
        
        setFocus();
      
    }

    private DataTable getNullDataTable()
    {
       DataTable dt = initTable();
       DataRow newRow;
       for (int i = 0; i < initRowsCount; i++)
       {
            newRow = dt.NewRow();
            newRow["DefectId"] = String.Empty;
            newRow["DefectDescr"] = String.Empty;
            dt.Rows.Add(newRow);
       }
       
       return dt;
    }
    private DataTable getNullDataTableForMBCode()
    {
        DataTable dt = initTableForMBCode();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            newRow["MBCode"] = String.Empty;
            newRow["PassQty"] = String.Empty;
            dt.Rows.Add(newRow);
        }

        return dt;
    }
    /// <summary>
    /// ��ʼ��������
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
       DataTable retTable  = new DataTable();
       retTable.Columns.Add("DefectId", Type.GetType("System.String"));
       retTable.Columns.Add("DefectDescr", Type.GetType("System.String"));
      
        //this.retTable.HeaderRow.Cells[0].Width = Unit.Percentage(30); 
       return retTable;
    }

    private DataTable initTableForMBCode()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("MBCode", Type.GetType("System.String"));
        retTable.Columns.Add("PassQty", Type.GetType("System.String"));

        //this.retTable.HeaderRow.Cells[0].Width = Unit.Percentage(30); 
        return retTable;
    }

    /// <summary>
    /// ���ñ�������Ƽ����
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.gridview.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_lblDefectCode").ToString();
        this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();
        this.gridview.Columns[0].ItemStyle.Width = Unit.Percentage(30);

        this.gridview.Columns[1].ItemStyle.Width = Unit.Percentage(70);

        //this.gridview.HeaderRow.Cells[0].Width = Unit.Percentage(30);
        //gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
    }
    private void initTableColumnHeaderforMBCode()
    {
        this.gridviewMB.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_tblMBCode").ToString();
        this.gridviewMB.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_tblpassqty").ToString();
        this.gridviewMB.Columns[0].ItemStyle.Width = Unit.Percentage(50);

        this.gridviewMB.Columns[1].ItemStyle.Width = Unit.Percentage(50);

        //this.gridview.HeaderRow.Cells[0].Width = Unit.Percentage(30);
        //gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
    }


    /// <summary>
    /// ���������Ϣ
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {
       

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

   

    /// <summary>
    /// Ϊ����м�tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }

    }
    /// <summary>
    /// Ϊ����м�tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewExtMB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }

    }
      /// <summary>
    ///�ý���
    /// </summary>  
    private void setFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (setTestStationCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setTestStationCmbFocus", script, false);
    }

    /// <summary>
    /// resetҳ����Ϣ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
            //this.cmbMBCode.InnerDropDownList.SelectedIndex = 0;

            //����mbCode������,����������ѡ���¼�
            this.cmbTestStation.setSelected(0);
            cmbTestStation_Selected(sender, e);
            setFocus();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);

        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);

        }
    }
    /// <summary>
    /// btnFru_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnFru_Click(object sender, System.EventArgs e)
    {
        //DEBUG ITC-1360-0358 ADD Server method refresh Pdline
        try
        {
            var selectLine = cmbPdLine.InnerDropDownList.SelectedItem.Text;
            this.cmbPdLine.refresh(this.cmbTestStation.InnerDropDownList.SelectedValue, this.customerHidden.Value);
            int i = 0;
            foreach (ListItem lst in this.cmbPdLine.InnerDropDownList.Items)
            {
                if (selectLine == lst.Text)
                {
                    cmbPdLine.InnerDropDownList.SelectedIndex = i;
                    return;
                }
                i++;
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }

    }

    /// <summary>
    /// nineSelect_CheckedChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void nineSelect_CheckedChanged(object sender, EventArgs e)
    {
        if (nineSelect.Checked == true)
        {

        }
        else
        {

        }

    }
}
