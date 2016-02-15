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
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;

public partial class DataMaintain_Celdata : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private ICeldata iceldata;
    private const int COL_NUM = 8;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;

    protected void Page_Load(object sender, EventArgs e)
    {
        iceldata = (ICeldata)ServiceAgent.getInstance().GetMaintainObjectByName<ICeldata>(WebConstant.Celdata);
        if (!this.IsPostBack)
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
      //      pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            
            //need change..
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            //load data
            initLabel();
            //find all AC Adaptor...
            //...
            IList<CeldataDef> datalst=null;
            try 
            {
                datalst = iceldata.GetAllCeldatas();
            }
            catch(FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
                return;
            }
            catch(Exception ee)
            {
                showErrorMessage(ee.Message);
                return;
            }
            bindTable(datalst, DEFAULT_ROWS);

        }

    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        
        try
        {
            string zmod = ttZMOD.Text.Trim();
            //调用删除方法.
            iceldata.DeleteCeldataItem(zmod);
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
        //按照ac adaptor list加载表格中的数据.
        //...
        showListByCeldataList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();HideWait();", true);
    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {

        CeldataDef def = new CeldataDef();
        def.platform = this.ttPlatform.Text.Trim();
        def.productSeriesName = this.ttProdSName.Text.Trim();
        def.category = this.ttCategory.Text.Trim();
        def.grade = Convert.ToInt32(this.ttGrade.Text.Trim());
        def.tec = this.ttTEC.Text.Trim();
        def.zmod = this.ttZMOD.Text.Trim();
        def.editor = this.HiddenUserName.Value.Trim();

        System.DateTime Cdt = DateTime.Now;
        string timeStr = Cdt.ToString();
        def.cdt = timeStr;
        string zmod = "";
        try
        {

            //调用添加的方法 相同的key时需要抛出异常...
            zmod = iceldata.AddCeldataItem(def);
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
        //按照ac adaptor list加载表格中的数据
        //...
        showListByCeldataList();
        this.updatePanel2.Update();
        //    string assemblyId = replaceSpecialChart(adaptor.assemb);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + zmod + "');HideWait();", true);
    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //e.Row.Cells[7].Attributes.Add("style", e.Row.Cells[7].Attributes["style"] + "display:none");
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
    private void initLabel()
    {
        this.lblZMODList.Text = this.GetLocalResourceObject(Pre + "_lblZMODList").ToString();
        this.lblPlatform.Text = this.GetLocalResourceObject(Pre + "_lblPlatform").ToString();
        this.lblProdSName.Text = this.GetLocalResourceObject(Pre + "_lblProdSName").ToString();
        this.lblCategory.Text = this.GetLocalResourceObject(Pre + "_llblCategory").ToString();
        this.lblGrade.Text = this.GetLocalResourceObject(Pre + "_lblGrade").ToString();
        this.lblTEC.Text = this.GetLocalResourceObject(Pre + "_lblTEC").ToString();
        this.lblZMOD.Text = this.GetLocalResourceObject(Pre + "_lblZMOD").ToString();

        
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();

    }
    private void bindTable(IList<CeldataDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblPlatform").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblProdSName").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCategory").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblGrade").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblTEC").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblZMOD").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCdt").ToString());
        //dt.Columns.Add("id");
        if (list != null && list.Count != 0)
        {
            foreach (CeldataDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.platform;
                dr[1] = temp.productSeriesName;
                dr[2] = temp.category;
                dr[3] = temp.grade;
                dr[4] = temp.tec;
                
                dr[5] = temp.zmod;
                dr[6] = temp.editor;
                dr[7] = temp.cdt;
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
        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();

        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(12);
    }

   

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return errorMsg;
    }
    private Boolean showListByCeldataList()
    {
    //    string acadaptorlst = this.ttAcAdaptorList.Text.Trim();
        IList<CeldataDef> dataLst = null;
        try
        {
            //if (acadaptorlst == "")
            {
                dataLst = iceldata.GetAllCeldatas();
            }
            //else 
            //{
            //    adaptorLst = iACAdaptor.GetAdaptorByAssembly(acadaptorlst);
            //}

            if (dataLst == null || dataLst.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataLst, DEFAULT_ROWS);
            }
        }
        catch (FisException fex)
        {
           
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(fex.mErrmsg);
            return false;
        }
        catch (System.Exception ex)
        {

            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }

   
}
