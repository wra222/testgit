/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for SMT Objective Time Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx –2012/7/11 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx –2012/7/11            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-7-11   Jessica Liu            Create
* Known issues:
* TODO：
* ITC-1361-0188, Jessica Liu, 2012-9-5
* ITC-1361-0218, Jessica Liu, 2012-9-13
*/

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
using System;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;


public partial class DataMaintain_SMTObjectiveTime : IMESBasePage
{
    public String userName;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string today;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private const int COL_NUM = 8;
    private ISMTObjectiveTime iSMTObjectiveTime;   
  
   
    protected void Page_Load(object sender, EventArgs e)
    {
        iSMTObjectiveTime = (ISMTObjectiveTime)ServiceAgent.getInstance().GetMaintainObjectByName<ISMTObjectiveTime>(WebConstant.SMTObjectiveTimeObject);
        if (!this.IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre+"_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre+"_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            today = iSMTObjectiveTime.GetCurDate().ToString("yyyy-MM-dd");

            //need change..
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
 
            //load data
            initLabel();

            initLine();

            //find all SMTLine
            IList<IMES.DataModel.SMTLineDef> datalst = null;
            try 
            {
                datalst = iSMTObjectiveTime.GetAllSMTLineInfo();
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
        string oldLine = this.dOldLine.Value.Trim();

        try 
        {
            SMTLineDef smtline = new SMTLineDef();
            smtline.Line = oldLine;
            //调用删除方法.
            iSMTObjectiveTime.DeleteOneSMTLine(smtline);
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

        //按照SMTLine list加载表格中的数据
        showListBySMTLineList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();HideWait();", true);
    }


    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        SMTLineDef smtline = new SMTLineDef();
        smtline.Line = this.ttLineValue.Value.Trim();
        smtline.ObjectiveTime = Convert.ToDecimal(this.ttObTime.Text.Trim());
        //ITC-1361-0191, Jessica Liu, 2012-9-6
        smtline.StartTime = Convert.ToDateTime(this.dStartTime.Value.Trim());
        smtline.EndTime = Convert.ToDateTime(this.dEndTime.Value.Trim());
        //ITC-1361-0218, Jessica Liu, 2012-9-13
        smtline.Remark = this.ttRemark.Text.ToUpper().Trim();
        smtline.Editor = this.HiddenUserName.Value; 
        
        System.DateTime cdt = DateTime.Now;;
        //string timeStr = cdt.ToString();
        smtline.Cdt = cdt;
        smtline.Udt = cdt;

        //string id = "";
        try 
        {
            //调用添加的方法,可能抛出异常...
            //id=iSMTObjectiveTime.AddOneSMTLine(smtline);
            iSMTObjectiveTime.AddOneSMTLine(smtline);
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

        //按照SMTLine list加载表格中的数据
        showListBySMTLineList();
        this.updatePanel2.Update();
        //string assemblyId = replaceSpecialChart(adaptor.assemb);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + this.ttLineValue.Value.Trim() + "');HideWait();", true);
    }


    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        SMTLineDef smtline = new SMTLineDef();
        smtline.Line = this.ttLineValue.Value.Trim();
        smtline.ObjectiveTime = Convert.ToDecimal(this.ttObTime.Text.Trim());
        //ITC-1361-0191, Jessica Liu, 2012-9-6
        smtline.StartTime = Convert.ToDateTime(this.dStartTime.Value.Trim());
        smtline.EndTime = Convert.ToDateTime(this.dEndTime.Value.Trim());
        //ITC-1361-0218, Jessica Liu, 2012-9-13
        smtline.Remark = this.ttRemark.Text.ToUpper().Trim();
        smtline.Editor = this.HiddenUserName.Value; ;
        string oldLine = this.dOldLine.Value.Trim();

        try 
        {
            //调用更新方法,可能抛出异常
            iSMTObjectiveTime.UpdateOneSMTLine(smtline, oldLine);

        }
        catch(FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(System.Exception ex)
        {
            //IList<IMES.DataModel.SMTLineDef> datalst = iSMTObjectiveTime.GetAllSMTLineInfo();
            //bindTable(datalst, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return;
        }

        //按照SMTLine list加载表格中的数据
        showListBySMTLineList();
        this.updatePanel2.Update();
        //string currentAssmebly = replaceSpecialChart(assembly);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + this.ttLineValue.Value.Trim() + "');HideWait();", true);
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
        this.lblObTimeList.Text = this.GetLocalResourceObject(Pre + "_lblObTimeList").ToString();
        this.lblLine.Text = this.GetLocalResourceObject(Pre + "_lblLine").ToString();
        this.lblObTime.Text = this.GetLocalResourceObject(Pre + "_lblObTime").ToString();
        this.lblHour.Text = this.GetLocalResourceObject(Pre + "_lblHour").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.lblStartTime.Text = this.GetLocalResourceObject(Pre + "_lblStartTime").ToString();
        this.lblEndTime.Text = this.GetLocalResourceObject(Pre + "_lblEndTime").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre +"_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();
        
    }


    private void bindTable(IList<SMTLineDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblObTime").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblStartTime").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEndTime").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblRemark").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblUdt").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (SMTLineDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.Line;
                dr[1] = temp.ObjectiveTime.ToString();
                dr[2] = temp.StartTime.ToString("yyyy-MM-dd");
                dr[3] = temp.EndTime.ToString("yyyy-MM-dd");
                dr[4] = temp.Remark;
                dr[5] = temp.Editor;
                dr[6] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[7] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");

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
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(10);
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


    private Boolean showListBySMTLineList()
    {
        //string smtobtimelst = this.ttSMTObjectiveTimeList.Text.Trim();
        IList<SMTLineDef> smtlineLst = null;

        try
        {
            /*
            //if (smtobtimelst == "")
            {
                smtlineLst = iSMTObjectiveTime.GetAllAdaptorInfo();
            }
            //else 
            //{
            //    smtlineLst = iSMTObjectiveTime.GetAdaptorByAssembly(smtobtimelst);
            //}
            */
            smtlineLst = iSMTObjectiveTime.GetAllSMTLineInfo();
            
            if(smtlineLst == null || smtlineLst.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(smtlineLst, DEFAULT_ROWS);
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


    private void initLine()
    {
        this.ttLine.Items.Add(new ListItem("", ""));
        this.dLineCount.Value = "0";

        try
        {
            DataTable dt = new DataTable();
            dt = iSMTObjectiveTime.GetLineList();

            int a = 0;
            int rowsCount = dt.Rows.Count;
            int colsCount = dt.Columns.Count;
            string[,] arrTmp = new string[rowsCount, colsCount];
            foreach (System.Data.DataRow row in dt.Rows)
            {
                int b = 0;
                foreach (System.Data.DataColumn column in dt.Columns)
                {
                    arrTmp[a, b] = row[column.ColumnName].ToString();
                    b = b + 1;
                }

                a = a + 1;
            }

            int i = 0;
            for (i = 0; i < a; i++)
            {
                //ITC-1361-0188, Jessica Liu, 2012-9-5
                this.ttLine.Items.Add(new ListItem(arrTmp[i, 1] + " " + arrTmp[i, 0], arrTmp[i, 1]));
            }

            this.dLineCount.Value = a.ToString();
            
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }
}
