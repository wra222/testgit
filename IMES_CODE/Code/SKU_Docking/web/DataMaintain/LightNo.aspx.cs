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
using System.Web.Configuration;
using System.Web;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

public partial class DataMaintain_LightNo : System.Web.UI.Page
{

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); 

    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //!!!here
    private const int DEFAULT_ROWS1 = 7;
    private ILightNo iLightNo;
    private const int COL_NUM1 = 3;

    private const int DEFAULT_ROWS2 = 14;
    private const int COL_NUM2 = 13;

    private const int EXCEL_DATA_START_ROW = 2;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;
    public string pmtMessage10;
    public string pmtMessage11;
    public string pmtMessage12;

    public string  KittingTypeFAKitting;
    public string  KittingTypePAKKitting;
    public string  KittingTypeFALabel;
    public string  KittingTypePAKLabel;

    public const string FA_STAGE = "FA";
    public const string PAK_STAGE = "PAK";
    public const string FA_LABEL = "FA Label";
    public const string PAK_LABEL = "PAK Label";

    public const string TMPKIT_FA = "FA";
    public const string TMPKIT_PAK = "PAK";    

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();
            pmtMessage11 = this.GetLocalResourceObject(Pre + "_pmtMessage11").ToString();
            pmtMessage12 = this.GetLocalResourceObject(Pre + "_pmtMessage12").ToString();

            KittingTypeFAKitting = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightNoKittingTypeValue1");
            KittingTypePAKKitting = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightNoKittingTypeValue2");
            KittingTypeFALabel = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightNoKittingTypeValue3");
            KittingTypePAKLabel = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightNoKittingTypeValue4");

            iLightNo = (ILightNo)ServiceAgent.getInstance().GetMaintainObjectByName<ILightNo>(WebConstant.MaintainLightNoObject);

            this.cmbMaintainLightNoKittingType.InnerDropDownList.Load += new EventHandler(cmbMaintainLightNoKittingType_Load);

            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                bindTable1(null, DEFAULT_ROWS1);
                bindTable2(null, DEFAULT_ROWS2);
            }



            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "InitControl", "initControls();", true);

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

    private void cmbMaintainLightNoKittingType_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbMaintainLightNoKittingType.IsPostBack)
        {
            ShowList1();
            //DealComboxChange();
        }
    }

    private void initLabel()
    {

        this.lblList2.Text = this.GetLocalResourceObject(Pre + "_lblList2").ToString();

        this.lblKittingType.Text = this.GetLocalResourceObject(Pre + "_lblKittingType").ToString();
        this.lblPartNo.Text = this.GetLocalResourceObject(Pre + "_lblPartNo").ToString();
        this.lblSubstitution.Text = this.GetLocalResourceObject(Pre + "_lblSubstitution").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        
        this.lblLightNo.Text = this.GetLocalResourceObject(Pre + "_lblLightNo").ToString();
        this.lblSafeStock.Text = this.GetLocalResourceObject(Pre + "_lblSafeStock").ToString();
        this.lblQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();

        this.lblMaxStock.Text = this.GetLocalResourceObject(Pre + "_lblMaxStock").ToString();
        //this.lblInsertFile.Text = this.GetLocalResourceObject(Pre + "_lblInsertFile").ToString();
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        //this.lblPartType.Text = this.GetLocalResourceObject(Pre + "_lblPartType").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();

        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        
        

    }

    protected void btnComBoxKittingTypeChange_ServerClick(Object sender, EventArgs e)
    {
        ShowList1();
        //DealComboxChange();
        this.dOldCode.Value="";
        ShowList2();
        this.updatePanel1.Update();
        this.updatePanel2.Update();
        //this.updatePanel3.Update();
        //this.updatePanel4.Update();

        //String kittingType = this.cmbMaintainLightNoKittingType.InnerDropDownList.SelectedValue;
        //if (kittingType == KittingTypeFAKitting)
        //{
        //    ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();DealKitTypeFA();", true);
        //}
        //else if (kittingType == KittingTypePAKKitting)
        //{
        //    ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();DealKitTypePAK();", true);

        //}
        //else if (kittingType == KittingTypeFALabel)
        //{
        //    ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();DealKitTypeFALabel();", true);
        //}
        //else//if (kittingType == KittingTypePAKLabel)
        //{
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "KittingTypeChange", "ShowRowEditInfo2();DealHideWait();checkSetEnableCombox();", true);
        //}
        

    }

    //private void DealComboxChange()
    //{
    //    String kittingType = this.cmbMaintainLightNoKittingType.InnerDropDownList.SelectedValue;
    //    if (kittingType == KittingTypeFAKitting)
    //    {
    //        this.cmbMaintainLightNoPdLine.Stage = FA_STAGE;
    //        this.cmbMaintainLightNoPdLine.initMaintainLightNoPdLine();
    //        this.cmbMaintainLightNoPartType.initMaintainLightNoPartType();
    //    }
    //    else if (kittingType == KittingTypePAKKitting)
    //    {
    //        this.cmbMaintainLightNoPdLine.Stage = PAK_STAGE;
    //        this.cmbMaintainLightNoPdLine.initMaintainLightNoPdLine();
    //        this.cmbMaintainLightNoPartType.clearContent();
    //    }
    //    else if (kittingType == KittingTypeFALabel)
    //    {
    //        this.cmbMaintainLightNoPdLine.clearContent();
    //        this.cmbMaintainLightNoPartType.clearContent();
    //    }
    //    else//if (kittingType == KittingTypePAKLabel)
    //    {
    //        this.cmbMaintainLightNoPdLine.clearContent();
    //        this.cmbMaintainLightNoPartType.clearContent();
    //    }

    //}


    //因为重刷新也只能要 2个日期参数，日期参数只在query时放入
    private Boolean ShowList1()
    {
        try
        {
            String kittingType = this.cmbMaintainLightNoKittingType.InnerDropDownList.SelectedValue;

            DataTable dataList;
            if (kittingType == KittingTypeFAKitting)
            {
                dataList = iLightNo.GetKittingCodeListFromLine(FA_STAGE);
            }
            else if (kittingType == KittingTypePAKKitting)
            {
                dataList = iLightNo.GetKittingCodeListFromLine(PAK_STAGE);
            }
            else if (kittingType == KittingTypeFALabel)
            {
                dataList = iLightNo.GetKittingCodeList(FA_LABEL);
            }
            else//if (kittingType == KittingTypePAKLabel)
            {
                dataList = iLightNo.GetKittingCodeList(PAK_LABEL);
            }
            
            if (dataList == null || dataList.Rows.Count == 0)
            {
                bindTable1(null, DEFAULT_ROWS1);
            }
            else
            {
                bindTable1(dataList, DEFAULT_ROWS1);
            }
        }
        catch (FisException ex)
        {
            bindTable1(null, DEFAULT_ROWS1);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            //show error
            bindTable1(null, DEFAULT_ROWS1);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;

    }

    private Boolean ShowList2()
    {
        string currentCode = this.dOldCode.Value.Trim();
        try
        {
            if (currentCode == "")
            {
                bindTable2(null, DEFAULT_ROWS2);
                return false;
            }

            DataTable dataList=new DataTable();

            string isLine =this.dOldIsLine.Value.Trim();
            String kittingType = this.cmbMaintainLightNoKittingType.InnerDropDownList.SelectedValue;

            //if (isLine == "Yes" && kittingType == KittingTypePAKKitting)
            //{    
            //     dataList = iLightNo.GetLightNoListPAK(kittingType, currentCode);
            //}
            //else if (isLine == "Yes" && kittingType != KittingTypePAKKitting)
            //{
            //    dataList = iLightNo.GetLightNoList(kittingType, currentCode);
            //}
            //else if (isLine != "Yes" && kittingType == KittingTypePAKKitting)
            //{                
            //    dataList = iLightNo.GetLightNoListFamiPAK(kittingType, currentCode);
            //}
            //else if (isLine != "Yes" && kittingType != KittingTypePAKKitting)
            //{
            //    dataList = iLightNo.GetLightNoListFami(kittingType, currentCode);
            //}

            dataList = iLightNo.GetLightNoFromSp(currentCode, kittingType, isLine);

            if (dataList == null || dataList.Rows.Count == 0)
            {
                bindTable2(null, DEFAULT_ROWS2);
            }
            else
            {
                bindTable2(dataList, DEFAULT_ROWS2);
            }
        }
        catch (FisException ex)
        {
            bindTable2(null, DEFAULT_ROWS2);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            //show error
            bindTable2(null, DEFAULT_ROWS2);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;

    }

    private void setColumnWidth1()
    {
        gd1.HeaderRow.Cells[0].Width = Unit.Percentage(30);
        gd1.HeaderRow.Cells[1].Width = Unit.Percentage(40);
        gd1.HeaderRow.Cells[2].Width = Unit.Percentage(30);

    }

    private void setColumnWidth2()
    {
        gd2.HeaderRow.Cells[0].Width = Unit.Percentage(12);
        gd2.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd2.HeaderRow.Cells[2].Width = Unit.Percentage(9);
        gd2.HeaderRow.Cells[3].Width = Unit.Percentage(6);
        gd2.HeaderRow.Cells[4].Width = Unit.Percentage(6);
        gd2.HeaderRow.Cells[5].Width = Unit.Percentage(6);
        gd2.HeaderRow.Cells[6].Width = Unit.Percentage(7);
        gd2.HeaderRow.Cells[7].Width = Unit.Percentage(6);
        gd2.HeaderRow.Cells[8].Width = Unit.Percentage(6);
        gd2.HeaderRow.Cells[9].Width = Unit.Percentage(6);
        gd2.HeaderRow.Cells[10].Width = Unit.Percentage(6);
        gd2.HeaderRow.Cells[11].Width = Unit.Percentage(10);
        gd2.HeaderRow.Cells[12].Width = Unit.Percentage(10);

    }

    private void bindTable1(DataTable list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem1KittingCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem1Description").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem1IsLine").ToString());

        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < list.Rows[i].ItemArray.Length; j++)
                {

                    if (list.Rows[i][j].GetType() == typeof(DateTime))
                    {
                        dr[j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        dr[j] = Null2String(list.Rows[i][j]);
                    }

                }
                dt.Rows.Add(dr);
            }

            for (int i = list.Rows.Count; i < DEFAULT_ROWS1; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount1.Value = list.Rows.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            this.hidRecordCount1.Value = "";
        }

        gd1.DataSource = dt;
        gd1.DataBind();
        setColumnWidth1();

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "iSelectedRowIndex1 = null;", true);

    }

    private void bindTable2(DataTable list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2Code").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2PartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2Descr").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2FaLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2Station").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2LightNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2Qty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2Sub").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2SafeStock").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2MaxStock").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2Remark").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2Editor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2Cdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItem2Udt").ToString());
        dt.Columns.Add("ID");


        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < list.Rows[i].ItemArray.Length; j++)
                {

                    if (list.Rows[i][j].GetType() == typeof(DateTime))
                    {
                        dr[j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        dr[j] = Null2String(list.Rows[i][j]);
                    }

                }
                dt.Rows.Add(dr);
            }

            for (int i = list.Rows.Count; i < DEFAULT_ROWS2; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount2.Value = list.Rows.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            this.hidRecordCount2.Value = "";
        }
        gd2.GvExtHeight = dTableHeight.Value;
        gd2.DataSource = dt;
        gd2.DataBind();
        setColumnWidth2();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex2", "iSelectedRowIndex2 = null;resetTableHeight();", true);


    }

    protected void btnList1Change_ServerClick(Object sender, EventArgs e)
    {
        ShowList2();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "ShowRowEditInfo2();DealHideWait();", true);
        //this.updatePanel.Update();

    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {

        string currentCode = this.dOldCode.Value.Trim();
        string oldId = this.dOldId.Value.Trim();
        string isLine = this.dOldIsLine.Value.Trim();

        string editor = this.HiddenUserName.Value.Trim();
        string resultId = "";
        try
        {
            WipBuffer item = new WipBuffer();
            item.Code = currentCode;
            item.Editor = editor;
            item.ID = Int32.Parse(oldId);

            item.Line = this.cmbMaintainLightNoPdLine.InnerDropDownList.SelectedValue.Trim();

            item.LightNo = Int32.Parse(this.dLightNo.Text).ToString();
            item.Max_Stock = Int32.Parse(this.dMaxStock.Text.Trim());
            item.PartNo = this.dPartNo.Text.Trim();//this.dPartNo.Text.ToUpper().Trim();
            item.Qty = Int32.Parse(this.dQty.Text.Trim());
            item.Remark = this.dRemark.Text.Trim();
            item.Safety_Stock =Int32.Parse(this.dSafeStock.Text.Trim());
            item.Sub = this.dSubstitution.Text.ToUpper().Trim();

            item.KittingType = this.cmbMaintainLightNoKittingType.InnerDropDownList.SelectedValue;
            item.Station = this.cmbMaintainLightNoStation.InnerDropDownList.SelectedValue;

            if (isLine == "Yes")
            {
                iLightNo.UpdateLightNo(item,true);
            }
            else
            {
                iLightNo.UpdateLightNo(item, false);
            }

            resultId = item.ID.ToString();
       
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);

            //if (ex.mErrcode == "DMT030" || ex.mErrcode == "DMT053")
            //{
            //    ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "InfoErrorPartNo();", true);
            //}

            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }

        ShowList2();
        this.updatePanel2.Update();
        resultId = replaceSpecialChart(resultId);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();AddUpdateComplete('" + resultId + "');", true);

    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        string idLightNo = this.dOldId.Value.Trim();
        try
        {
            int id=Int32.Parse(idLightNo);
            iLightNo.DeleteLightNo(id);
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
        ShowList2();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);

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
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    protected void gd_RowDataBound1(object sender, GridViewRowEventArgs e)
    {

        for (int i = COL_NUM1; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            for (int i = 0; i < COL_NUM1; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }

    }

    protected void gd_RowDataBound2(object sender, GridViewRowEventArgs e)
    {

        for (int i = COL_NUM2; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            for (int i = 0; i < COL_NUM2; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }

    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {

        string currentCode = this.dOldCode.Value.Trim();
        //string oldId = this.dOldId.Value.Trim();

        string editor = this.HiddenUserName.Value.Trim();

        string isLine = this.dOldIsLine.Value.Trim();

        string resultId = "";
        try
        {
            WipBuffer item = new WipBuffer();
            item.Code = currentCode;
            item.Editor = editor;
            //item.ID = oldId;

            item.Line = this.cmbMaintainLightNoPdLine.InnerDropDownList.SelectedValue.Trim();

            item.LightNo = Int32.Parse(this.dLightNo.Text).ToString();
            item.Max_Stock = Int32.Parse(this.dMaxStock.Text.Trim());
            item.PartNo = this.dPartNo.Text.Trim(); //this.dPartNo.Text.ToUpper().Trim();
            item.Qty = Int32.Parse(this.dQty.Text.Trim());
            item.Remark = this.dRemark.Text.Trim();
            item.Safety_Stock = Int32.Parse(this.dSafeStock.Text.Trim());
            item.Sub = this.dSubstitution.Text.ToUpper().Trim();

            item.KittingType = this.cmbMaintainLightNoKittingType.InnerDropDownList.SelectedValue;
            item.Station = this.cmbMaintainLightNoStation.InnerDropDownList.SelectedValue;

            if (isLine.Trim() == "Yes")
            {
                resultId = iLightNo.AddLightNo(item,true);
            }
            else
            {
                resultId = iLightNo.AddLightNo(item,false);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);

            //if (ex.mErrcode == "DMT030" || ex.mErrcode == "DMT053")
            //{
            //    ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "InfoErrorPartNo();", true);
            //}

            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }

        ShowList2();
        this.updatePanel2.Update();
        resultId = replaceSpecialChart(resultId);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();AddUpdateComplete('" + resultId + "');", true);

    }
    
}

