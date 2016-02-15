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

public partial class DataMaintain_ITCNDDefectCheck : System.Web.UI.Page
{
    
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IITCNDDefectCheck itcndDefectCheck;
    private const int COL_NUM = 5;

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
   
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CmbMaintainFamilyForECRVersion.InnerDropDownList.Load += new EventHandler(ddlDescr_Load);
        itcndDefectCheck = (IITCNDDefectCheck)ServiceAgent.getInstance().GetMaintainObjectByName<IITCNDDefectCheck>(WebConstant.ITCNDDEFECTCHECK);
        if (!this.IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            //pmtMessage2 = this.GetLocalResourceObject(Pre+"_pmtMessage2").ToString();
              pmtMessage3 = this.GetLocalResourceObject(Pre+"_pmtMessage3").ToString();
            //pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            //pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            //pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            //pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            //pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            //pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            //need change..
              pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            //load data
            initLabel();
            //find all AC Adaptor...
            //...
            IList<IMES.DataModel.ITCNDDefectCheckDef> datalst=null;
            try 
            {
                datalst = itcndDefectCheck.GetAllITCNDDefectCheckItems();
            }
            catch(FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
            }
            catch(Exception ee)
            {
                showErrorMessage(ee.Message);
            }
            bindTable(datalst, DEFAULT_ROWS);
        }
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "InitControl", "initContorls();", true);
    }

    private void ddlDescr_Load(object sender, System.EventArgs e)
    {
        if (!this.CmbMaintainFamilyForECRVersion.IsPostBack)
        {
            string descr = this.CmbMaintainFamilyForECRVersion.InnerDropDownList.SelectedValue.Trim();
            this.CmbMaintainITCNDDefectCheckWithCode.refresh(descr);
            showList();
        }

    }
    protected void btnRefreshPartNoList_ServerClick(object sender, EventArgs e)
    {
        string descr = this.CmbMaintainFamilyForECRVersion.InnerDropDownList.SelectedValue.Trim();
        this.CmbMaintainITCNDDefectCheckWithCode.refresh(descr);
        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "displaySelectedRow", "HideWait();", true);
    }
    //protected void btnDisplaySelectedInfo_ServerClick(object sender, EventArgs e)
    //{
    //    string descrSeled = this.HiddenTypeDescr.Value.Trim();
    //    string partno = this.HiddenPartNo.Value.Trim();
    //    string station = this.HiddenStation.Value.Trim();
    //    string location = this.HiddenLocation.Value.Trim();

    //    if (partno != "")
    //    {
    //        this.ddlPartNo.refresh(descrSeled);
    //        showListByGradeList();
    //        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "display", "displaySelectedItem('" + descrSeled + "','" + partno + "','" + station + "','" + location + "');HideWait();", true);
    //    }
    //}


    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        string selectedFamily = this.dOldId.Value.Trim();
        try 
        {
            ITCNDDefectCheckDef def = new ITCNDDefectCheckDef();
            def.family = this.dOldId.Value.Trim();
            def.code = this.dOldCode.Value.Trim();
            //调用删除方法.
            itcndDefectCheck.RemoveITCNDDefectCheck(def);
        }
        catch(FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        //按照ac adaptor list加载表格中的数据.
        //...
        showList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();HideWait();", true);
    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        ITCNDDefectCheckDef itcnd = new ITCNDDefectCheckDef();
        itcnd.family = this.CmbMaintainFamilyForECRVersion.InnerDropDownList.SelectedValue.Trim();
      // itcnd.type
        itcnd.type = this.CmbMaintainType.InnerDropDownList.SelectedValue.Trim();
        itcnd.editor = this.HiddenUserName.Value.Trim();
        itcnd.code = this.CmbMaintainITCNDDefectCheckWithCode.InnerDropDownList.SelectedValue.Trim();
        System.DateTime cdt = DateTime.Now;

        itcnd.cdt = cdt;
        
        try 
        {
            
            //调用添加的方法 相同的key时需要抛出异常...
            itcndDefectCheck.AddITCNDDefectCheck(itcnd);
        }
        catch(FisException fex)
        {
           
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(System.Exception ex)
        {
            
            showErrorMessage(ex.Message);
            return;
        }
        //按照ac adaptor list加载表格中的数据
        //...
        showList();
        this.updatePanel2.Update();
    //    string assemblyId = replaceSpecialChart(adaptor.assemb);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + itcnd.family + "');HideWait();", true);
    }
    
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
    private void initLabel()
    {
        this.lblTestMB.Text = this.GetLocalResourceObject(Pre + "_lblTestMBList").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.lblCode.Text = this.GetLocalResourceObject(Pre+"_lblCode").ToString();
    }
    private void bindTable(IList<ITCNDDefectCheckDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colType").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        
        
        if (list != null && list.Count != 0)
        {
            foreach (ITCNDDefectCheckDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.family;
                dr[1] = temp.code;
                dr[2] = temp.type;
                dr[3] = temp.editor;
                dr[4] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
               
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
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
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
    private Boolean showList()
    {
        
        IList<ITCNDDefectCheckDef> dataLst = null;
        try
        {
            //if (acadaptorlst == "")
            {
                dataLst = itcndDefectCheck.GetAllITCNDDefectCheckItems();
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
        catch(FisException fex)
        {
           
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(fex.mErrmsg);
            return false;
        }
        catch(System.Exception ex)
        {

            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }
    
}
