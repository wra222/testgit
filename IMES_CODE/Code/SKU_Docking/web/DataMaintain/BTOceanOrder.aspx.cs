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
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.IO;


public partial class DataMaintain_BTOceanOrder : System.Web.UI.Page
{
    public String userName;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IBTOceanOrder IBTOceanOrder = (IBTOceanOrder)ServiceAgent.getInstance().GetMaintainObjectByName<IBTOceanOrder>(WebConstant.IBTOceanOrderManager);
    private const int COL_NUM = 5;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    protected void Page_Load(object sender, EventArgs e)
    {
        pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
        pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
        pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
        pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
        pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
        pmtMessage6 = this.GetLocalResourceObject("eng_pmtMessage6").ToString() + "\r\n" + this.GetLocalResourceObject("cn_pmtMessage6").ToString();
        pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
        pmtMessage8 = this.GetLocalResourceObject("eng_pmtMessage8").ToString() + "\r\n" + this.GetLocalResourceObject("cn_pmtMessage8").ToString();
        try
        {
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUsername.Value = userName;
                initLabel();
                ShowBTOceanOrderList();
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


    #region 页面的label与gridview控件的初始化，added by ShhWang on 2011-10-11
    /// <summary>
    /// 初始化页面Label文本显示，added by ShhWang on 2011-10-13
    /// </summary>
    private void initLabel()
    {
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblBTOceanOrderText").ToString();
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLineText").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModelText").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnUpload.Value = this.GetLocalResourceObject(Pre + "_lbUploadFileText").ToString();
    }

    /// <summary>
    /// 设置gridview列的宽度百分比 added by ShhWang on 2011-10-13
    /// </summary>
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(15);  //.Pixel(240);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(25);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(25);
    }
    #endregion


    #region 对于数据的增删改查触发的事件
    /// <summary>
    /// 修改数据方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_serverClick(Object sender, EventArgs e)
    {
        BTOceanOrder item = new BTOceanOrder();
        string oldPdLine = this.hidPdLine.Value.ToString().Trim();
        string oldModel = this.hidModel.Value.ToString().Trim();
        string pdLine = this.CmPdLine.InnerDropDownList.SelectedValue;
        string model = this.dModel.Text.Trim().ToUpper();
        item.pdLine = pdLine;
        item.model = model;
        item.editor = this.HiddenUsername.Value;
        item.udt = DateTime.Now;
        try
        {
            IBTOceanOrder.updateBTOceanOrderbyPdlineAndModel(item, oldPdLine, oldModel);

        }
        catch (FisException ex)
        {

            showErrorMessage(ex.mErrmsg);
            return;

        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        ShowBTOceanOrderList();
        this.UpdatePanel2.Update();
        pdLine = replaceSpecialChart(pdLine);
        model = replaceSpecialChart(model);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + pdLine + "','" + model + "');", true);

    }

    ///// <summary>
    ///// 添加数据方法
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void btnAdd_serverClick(Object sender, EventArgs e)
    {
        BTOceanOrder item = new BTOceanOrder();
        string PdLine = this.CmPdLine.InnerDropDownList.SelectedValue.ToString().Trim();
        string Model = this.dModel.Text.ToString().Trim().ToUpper();
        item.pdLine = PdLine;
        item.model = Model;
        item.editor = this.HiddenUsername.Value;
        item.cdt = DateTime.Now;
        item.udt = DateTime.Now;
        try
        {
            IBTOceanOrder.addBTOceanOrder(item);
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
        ShowBTOceanOrderList();
        this.UpdatePanel2.Update();
        PdLine = replaceSpecialChart(PdLine);
        Model = replaceSpecialChart(Model);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + PdLine + "','" + Model + "');", true);

    }

    /// <summary>
    /// 删除一条数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        BTOceanOrder item = new BTOceanOrder();
        string pdLine = this.CmPdLine.InnerDropDownList.SelectedValue.ToString();
        string model = this.dModel.Text;
        string oldPdLine = this.hidPdLine.Value;
        string oldModel = this.hidModel.Value;
        try
        {
            IBTOceanOrder.deleteBTOceanOrderByPdlineAndModel(oldPdLine, oldModel);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        ShowBTOceanOrderList();
        this.UpdatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();", true);

    }

    /// <summary>
    /// 上传文件并写数据库
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpload_serverClick(Object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        string strUploadedFileName = this.hidFileName.Value;
        try
        {
            if (File.Exists(strUploadedFileName))
            {
                dt = GetExcelTable(strUploadedFileName);
                if (dt.Rows.Count == 0)
                {
                    showErrorMessage(pmtMessage7);
                    return;
                }
                else
                {
                    if (checkStringLength(dt))
                    {
                        showErrorMessage(pmtMessage8);
                        return;
                    }
                    if (checkIsInDB(dt))
                    {
                        saveDataTabel(dt);
                    }
                    else
                    {
                        showErrorMessage(pmtMessage6);
                        return;
                    }
                }
            }
            ShowBTOceanOrderList();
            this.UpdatePanel2.Update();
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "setNewItemValue();DealHideWait();", true);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
            TryDeleteTempFile(strUploadedFileName);
        }
    }
    private static void TryDeleteTempFile(string fullFileName)
    {
        try
        {
            File.Delete(fullFileName);
        }
        catch
        {
            //忽略
        }
    }
    /// <summary>
    /// 保存提交的excel到数据库
    /// </summary>
    /// <param name="dt"></param>
    protected void saveDataTabel(DataTable dt)
    {
        BTOceanOrder btd = new BTOceanOrder();
        btd.pdLine = this.CmPdLine.InnerDropDownList.SelectedValue.ToString();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i][0].ToString() != "")
            {
                btd.model = dt.Rows[i][0].ToString();
                btd.editor = this.HiddenUsername.Value;
                btd.cdt = DateTime.Now;
                btd.udt = DateTime.Now;
                IBTOceanOrder.addBTOceanOrder(btd);
            }
        }

    }

    /// <summary>
    /// 判断字符串长度是否过长
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    protected bool checkStringLength(DataTable dt)
    {
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i][0].ToString().Length > 20)
            {
                return true;
            }
        }
        return false;

    }

    /// <summary>
    /// 判断数据是否存在于数据库中
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    protected Boolean checkIsInDB(DataTable dt)
    {

        string pdline = this.CmPdLine.InnerDropDownList.SelectedValue.ToString();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string model = dt.Rows[i][0].ToString();
            IList<BTOceanOrder> lstBTOcean = new List<BTOceanOrder>();
            lstBTOcean = IBTOceanOrder.getListByPdLineAndModel(pdline, model);
            if (lstBTOcean.Count > 0)
            {
                return false;
            }

        }
        return true;

    }
    #endregion


    #region gridview事件
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
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

        }

    }

    /// <summary>
    /// gridview的数据绑定 added by ShhWang on 2011-10-13
    /// </summary>
    /// <param name="list"></param>
    /// <param name="defaultRow"></param>
    private void bindTable(IList<BTOceanOrder> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstPdLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstModel").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (BTOceanOrder temp in list)
            {
                dr = dt.NewRow();

                dr[0] = Null2String(temp.pdLine);
                dr[1] = Null2String(temp.model);
                dr[2] = Null2String(temp.editor);
                dr[3] = ((System.DateTime)temp.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                dr[4] = ((System.DateTime)temp.udt).ToString("yyyy-MM-dd HH:mm:ss");
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
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

        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        gd.GvExtHeight = dTableHeight.Value;
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "setTableHeight", "resetTableHeight();iSelectedRowIndex = null;", true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected Boolean ShowBTOceanOrderList()
    {

        try
        {
            IList<BTOceanOrder> dataList;
            dataList = IBTOceanOrder.getAllBTOceanOrder();
            if (dataList == null || dataList.Count == 0)
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
    #endregion


    #region 一些系统方法
    /// <summary>
    /// 
    /// </summary>
    /// <param name="errorMsg"></param>
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceData"></param>
    /// <returns></returns>
    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    /// <summary>
    /// 判断是否为空
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    /// <summary>
    /// 读取excel文件
    /// </summary>
    /// <param name="uploadPath"></param>
    /// <returns></returns>
    private DataTable GetExcelTable(string uploadPath)
    {
        DataSet ds;
        //string Xls_ConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploadPath + ";Extended Properties='Excel 12.0;HDR=NO;IMEX=0';";
        string Xls_ConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploadPath + ";Extended Properties='Excel 12.0;HDR=NO;IMEX=1';";

        OleDbConnection Conn = new OleDbConnection(Xls_ConnStr);
        try
        {
            Conn.Open();
            string sql_str = "select * from [Sheet1$]";
            OleDbDataAdapter da = new OleDbDataAdapter(sql_str, Conn);
            ds = new DataSet();
            da.Fill(ds, "excel_data");
            Conn.Close();
        }
        catch
        {
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
            return null;
        }
        finally
        {
            Conn.Dispose();
        }
        if (ds == null)
        {
            return null;
        }
        if (ds.Tables.Count < 1)
        {
            return null;
        }
        return ds.Tables[0];
    }


    #endregion


}
