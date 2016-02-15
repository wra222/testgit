using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;

public partial class FA_ChangeAST : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    public String UserId;
    public String Customer;

    int gd1_defaultrow = 4;
    int gd2_defaultrow = 3;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                this.hidStation.Value = Request["Station"];
                this.hidPCode.Value = Request["PCode"];

                CmbPdLine.Station = Request["Station"];
                CmbPdLine.Customer = Master.userInfo.Customer;

                bindTable1(null, gd1_defaultrow);
                bindTable2(null, gd2_defaultrow);
                
                InitLabel();                
            }
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    protected void btnClear_ServerClick(Object sender, EventArgs e)
    {
        bindTable1(null, gd1_defaultrow);
        bindTable2(null, gd2_defaultrow);
        this.lblProdId1Content.Text = "";
        updatePanel1.Update();
        this.lblModel1Content.Text = "";
        updatePanel2.Update();
        UpdatePanel3.Update();
        this.lblProdId2Content.Text = "";
        updatePanel4.Update();
        this.lblModel2Content.Text = "";
        updatePanel5.Update();
        updatePanel6.Update();
        hideWait();
    }

    protected void btnCheck_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            IChangeAST iChangeAST = ServiceAgent.getInstance().GetObjectByName<IChangeAST>(WebConstant.ChangeASTObject);

            ArrayList ret = iChangeAST.CheckProduct(this.hidProdId.Value, this.hidLine.Value, UserId, this.hidStation.Value, Customer);

            IList<ASTInfo> list = (IList<ASTInfo>)ret[0];

            //若不存在AT记录，且ProdId2为空，则Product相关信息，显示在ProdId2、Model2、ASTList2
            if ((list == null || list.Count == 0) && this.lblProdId2Content.Text == "")
            {
                lblProdId2Content.Text = hidProdId.Value;
                lblModel2Content.Text = (string)ret[1];
                bindTable2(null, gd2_defaultrow);
                updatePanel4.Update();
                updatePanel5.Update();
                updatePanel6.Update();
                inputFinish();
                return;
            }
            //若不存在AT记录，且ProdId2不为空，则报错：“刷入的ProductID：XXX错误”
            if ((list == null || list.Count == 0) && this.lblProdId2Content.Text != "")
            {
                iChangeAST.returnException2(hidProdId.Value);
            }

            //若存在AT记录，且ProdId1为空，则Product相关信息，显示在ProdId1、Model1、ASTList1
            if ((list != null && list.Count > 0) && this.lblProdId1Content.Text == "")
            {
                bindTable1(list, gd1_defaultrow);
                lblProdId1Content.Text = hidProdId.Value;
                lblModel1Content.Text = (string)ret[1];
                updatePanel1.Update();
                updatePanel2.Update();
                UpdatePanel3.Update();
                inputFinish();
                return;
            }

            //若存在AT记录，且ProdId1不为空，ProdId2为空，则Product相关信息，显示在ProdId2、Model2、ASTList2
            if ((list != null && list.Count > 0) && this.lblProdId1Content.Text != ""
                && this.lblProdId2Content.Text == "")
            {
                bindTable2(list, gd2_defaultrow);
                lblProdId2Content.Text = hidProdId.Value;
                lblModel2Content.Text = (string)ret[1];
                updatePanel4.Update();
                updatePanel5.Update();
                updatePanel6.Update();
                inputFinish();
                return;
            }

            //若存在AT记录，且ProdId1不为空，ProdId2不为空，则报错：“错误的代码”
            if ((list != null && list.Count > 0) && this.lblProdId1Content.Text != ""
                && this.lblProdId2Content.Text != "")
            {
                iChangeAST.returnException5();
            }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
            hideWait();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message.ToString());
            hideWait();
        }
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("setCommonFocus();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }

    private void inputFinish()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("inputFinish();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "inputFinish", scriptBuilder.ToString(), false);
    }

    private void InitLabel()
    {
        this.lblPdline.Text = this.GetLocalResourceObject(Pre + "_lblPdline").ToString();
        this.lblProdId1.Text = this.GetLocalResourceObject(Pre + "_lblProdId1").ToString();
        this.lblModel1.Text = this.GetLocalResourceObject(Pre + "_lblModel1").ToString();
        this.lblProdId2.Text = this.GetLocalResourceObject(Pre + "_lblProdId2").ToString();
        this.lblModel2.Text = this.GetLocalResourceObject(Pre + "_lblModel2").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.btnChange.Value = this.GetLocalResourceObject(Pre + "_btnChange").ToString();
        this.btnSetting.Value = this.GetLocalResourceObject(Pre + "_btnSetting").ToString();   
    }

    private void bindTable1(IList<ASTInfo> list, int defaultRow)
    {

        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("ASTType", Type.GetType("System.String"));
        dt.Columns.Add("PartNo", Type.GetType("System.String"));
        dt.Columns.Add("PartSn", Type.GetType("System.String"));
        
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colASTType").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartNo").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartSn").ToString());

        if (list != null && list.Count > 0)
        {
            foreach (ASTInfo temp in list)
            {
                dr = dt.NewRow();
                dr["ASTType"] = temp.ASTType;
                dr["PartNo"] = temp.PartNo;
                dr["PartSn"] = temp.PartSn;

                dt.Rows.Add(dr);
            }
            for (int i = list.Count; i < defaultRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            this.hidRecordCount.Value = list.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            this.hidRecordCount.Value = "";
        }

        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();
        InitGridView();
    }
    private void InitGridView()
    {
        //GridView1.HeaderRow.Cells[0].Text = "ALL";
        GridView1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colASTType").ToString();
        GridView1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_colPartNo").ToString();
        GridView1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_colPartSn").ToString();
        
        GridView1.HeaderRow.Cells[0].Width = Unit.Pixel(5);
        GridView1.HeaderRow.Cells[1].Width = Unit.Pixel(10);
        GridView1.HeaderRow.Cells[2].Width = Unit.Pixel(20);
        GridView1.HeaderRow.Cells[3].Width = Unit.Pixel(40);
    }

    protected void gd1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (String.IsNullOrEmpty(e.Row.Cells[1].Text.Trim()) || (e.Row.Cells[1].Text.Trim().ToLower() == "&nbsp;"))
            {
                CheckBox check = (CheckBox)e.Row.FindControl("RowChk");
                check.Style.Add("display", "none");
            }
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                    e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace(Environment.NewLine, "<br>");
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Header)
        {

        }
    }



    private void bindTable2(IList<ASTInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colASTType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartSn").ToString());

        if (list != null && list.Count > 0)
        {
            foreach (ASTInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ASTType;
                dr[1] = temp.PartNo;
                dr[2] = temp.PartSn;
                
                dt.Rows.Add(dr);
            }
            for (int i = list.Count; i < defaultRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            this.hidRecordCount.Value = list.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            this.hidRecordCount.Value = "";
        }

        this.GridView2.DataSource = dt;
        this.GridView2.DataBind();
        setColumnWidth();
    }

    private void setColumnWidth()
    {
        this.GridView2.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        this.GridView2.HeaderRow.Cells[1].Width = Unit.Pixel(20);
        this.GridView2.HeaderRow.Cells[2].Width = Unit.Pixel(40);
    }

    protected void gd2_RowDataBound(object sender, GridViewRowEventArgs e)
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
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(String errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\'", string.Empty).Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

}
