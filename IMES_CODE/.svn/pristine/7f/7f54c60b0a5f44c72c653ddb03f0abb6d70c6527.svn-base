using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;

using System.Data;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;

public partial class DataMaintain_Grade : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 31;
    private IDefectStation iDefectStation;
    private IRepairInfoMaintain iSelectData;
            
            
    private const int COL_NUM = 11;//7;

    
    public  string MsgSelectOne = "";
    public  string MsgDelConfirm = "";
    public  string MsgPreWC = "";
    public  string MsgDefectSel = "";
    public  string MsgCurWC = "";
    public  string MsgNextWC = "";
    public  string MsgCauseSel = "";
    public string MsgNotFound = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        iSelectData = (IRepairInfoMaintain)ServiceAgent.getInstance().GetMaintainObjectByName<IRepairInfoMaintain>(WebConstant.REPAIRINFOMAINTAIN);
        iDefectStation = (IDefectStation)ServiceAgent.getInstance().GetMaintainObjectByName<IACAdaptor>(WebConstant.MaintainDefectStationObject);
        MsgSelectOne = this.GetLocalResourceObject(Pre + "_MsgSelectOne").ToString();
        MsgDelConfirm = this.GetLocalResourceObject(Pre + "_MsgDelConfirm").ToString();
        MsgPreWC = this.GetLocalResourceObject(Pre + "_MsgPreWC").ToString();
        MsgCurWC = this.GetLocalResourceObject(Pre + "_MsgCurWC").ToString();
        MsgNextWC = this.GetLocalResourceObject(Pre + "_MsgNextWC").ToString();
        MsgDefectSel = this.GetLocalResourceObject(Pre + "_MsgDefectSel").ToString();
        MsgCauseSel = this.GetLocalResourceObject(Pre + "_MsgCauseSel").ToString();
        MsgNotFound = this.GetLocalResourceObject(Pre + "_MsgNotFound").ToString();
        if(!IsPostBack)
        {
            
            
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            List<DefectCodeStationInfo> defectLst = null;
            initLabel();
            try
            {
                initSelect();
                //defectLst = (List<DefectCodeStationInfo>)iDefectStation.GetDefectList();
            }
            catch (FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
                return;
            }
            catch(Exception ee)
            {
                showErrorMessage(ee.Message);
                return;
            }

            //bindTable(defectLst, DEFAULT_ROWS);
            bindTable(null, DEFAULT_ROWS);
        }

    }

    private void initSelect()
    {
        initPreWCTopSelect();
        initMajorPartSelect();
    }

    private void initPreWCTopSelect()
    {
        IList<string> lstPreWCTop = iDefectStation.GetPreStationFromDefectStation();
        this.cmbPreWCTop.Items.Clear();
        if (lstPreWCTop.Count != 0)
        {
            foreach (string item in lstPreWCTop)
            {
                this.cmbPreWCTop.Items.Add(item.Trim());
            }
            this.cmbPreWCTop.SelectedIndex = 0;
        }
        this.updatePanel1.Update();
    }

    private void initMajorPartSelect()
    {

        IList<List<string>> lst = iDefectStation.GetMajorPartList("MajorPart", Request["Customer"]);
        this.cmbMajorPart.Items.Clear();
        this.cmbMajorPart.Items.Add(string.Empty);
        if (lst != null)
        {
            foreach (List<string> temp in lst)
            {
                ListItem item = new ListItem(temp[0] + " " + temp[1], temp[0]);
                this.cmbMajorPart.Items.Add(item);
            }
            this.cmbMajorPart.SelectedIndex = 0;
        }
    }

    protected void btnQuery_ServerClick(object sender, EventArgs e)
    {
        string type = this.hidPerWCTopSelect.Value.Trim();
        try
        {
            IList<DefectCodeStationInfo> lstDefcet = new List<DefectCodeStationInfo>();
            lstDefcet = iDefectStation.GetDefectStationByPreStation(type);
            bindTable(lstDefcet, DEFAULT_ROWS);
            this.cmbPreWCTop.SelectedValue = this.hidPerWCTopSelect.Value;
            this.updatePanel1.Update();
        }
        catch (FisException fex)
        {

            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        finally
        {
            hideWait();
            this.updatePanel2.Update();
        }
        //ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "Query", "HideWait();", true);
    }

    protected void btnAdd_ServerClick(object sender,EventArgs e)
    {
        string familyAndModel = this.txtFamilyAndModel.Text.ToUpper().Trim();
        DefectCodeStationInfo defect = new DefectCodeStationInfo();
        defect.pre_stn = this.ddlPreWC.InnerDropDownList.SelectedValue;
        defect.crt_stn =  this.ddlCurWC.InnerDropDownList.SelectedValue;
        defect.nxt_stn = this.ddlNextWC.InnerDropDownList.SelectedValue;
        defect.defect = this.ddlDefect.InnerDropDownList.SelectedValue;
        defect.cause = this.ddlCause.InnerDropDownList.SelectedValue;
        defect.family = familyAndModel;
        defect.editor = Master.userInfo.UserId;
        defect.majorPart = this.hidMajorPart.Value.Trim();
        string itemId="";
        try 
        {
            if (!string.IsNullOrEmpty(familyAndModel))
            {
                bool check = iDefectStation.CheckFamily(familyAndModel);
                if (check == false)
                {
                    throw new FisException("此Family/Model不存在");
                }
            }
            //if(!iDefectStation.CheckDefectStationUnique(defect.pre_stn,defect.crt_stn,defect.majorPart,defect.cause,defect.defect))
            //{
            //    throw new Exception("");
            //}
            itemId = iDefectStation.AddDefectStation(defect);
            initPreWCTopSelect();
            showDefectList();
            itemId = replaceSpecialChart(itemId);

        }
        catch(FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        finally
        {
            hideWait();
            this.updatePanel2.Update();
            AddUpdateComplete(itemId);
        }
        
        
        //ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');", true);
    }
   
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        string id = this.HiddenSelectedId.Value.Trim();
        string type = this.hidPerWCTopSelect.Value.Trim();
        try
        {
            iDefectStation.DeleteDefectStation(Convert.ToInt32(id));
            initPreWCTopSelect();
            IList<DefectCodeStationInfo> lstDefcet = new List<DefectCodeStationInfo>();
            lstDefcet = iDefectStation.GetDefectStationByPreStation(type);
            
            if (lstDefcet.Count == 0)
            {
                this.cmbPreWCTop.SelectedIndex = 0;
                lstDefcet = iDefectStation.GetDefectStationByPreStation(this.cmbPreWCTop.SelectedValue);
            }
            else
            {
                this.cmbPreWCTop.SelectedValue = this.hidPerWCTopSelect.Value;
            }
            this.updatePanel1.Update();
            bindTable(lstDefcet, DEFAULT_ROWS);
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        finally
        {
            hideWait();
            this.updatePanel2.Update();
            DeleteComplete();
        }
            //ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "Delete", "DeleteComplete();", true);


    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        string itemId = this.HiddenSelectedId.Value.Trim();
        string familyAndModel = this.txtFamilyAndModel.Text.ToUpper().Trim();
        DefectCodeStationInfo newDefect = new DefectCodeStationInfo();
        newDefect.pre_stn = this.ddlPreWC.InnerDropDownList.SelectedValue.Trim();
        newDefect.crt_stn = this.ddlCurWC.InnerDropDownList.SelectedValue.Trim();
        newDefect.nxt_stn = this.ddlNextWC.InnerDropDownList.SelectedValue.Trim();
        newDefect.defect = this.ddlDefect.InnerDropDownList.SelectedValue.Trim();
        newDefect.cause = this.ddlCause.InnerDropDownList.SelectedValue.Trim();
        newDefect.family = familyAndModel;
        newDefect.majorPart = this.hidMajorPart.Value.Trim();
        newDefect.editor = Master.userInfo.UserId;

        newDefect.id = Convert.ToInt32(itemId);
        
        try
        {
            if (!string.IsNullOrEmpty(familyAndModel))
            {
                bool check = iDefectStation.CheckFamily(familyAndModel);
                if (check == false)
                {
                    throw new FisException("此Family/Model不存在");
                }
            }
            iDefectStation.UpdateDefectStation(newDefect);
            initPreWCTopSelect();
            showDefectList();
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
        finally
        {
            hideWait();
            this.updatePanel2.Update();
            AddUpdateComplete(itemId);
        }
        
        
        //ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');HideWait();", true);

    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //e.Row.Cells[COL_NUM - 1].Attributes.Add("style", e.Row.Cells[COL_NUM - 1].Attributes["style"] + "display:none");
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
        this.lblDefectList.Text = this.GetLocalResourceObject(Pre + "_lblDefectList").ToString();
        this.lblCause.Text = this.GetLocalResourceObject(Pre + "_lblCause").ToString();
        this.lblPreWC.Text = this.GetLocalResourceObject(Pre + "_lblPreWC").ToString();
        this.lblCurWC.Text = this.GetLocalResourceObject(Pre + "_lblCurWC").ToString();
        this.lblNextWC.Text = this.GetLocalResourceObject(Pre + "_lblNextWC").ToString();
        this.lblDefect.Text = this.GetLocalResourceObject(Pre + "_lblDefect").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();

    }
    private void bindTable(IList<DefectCodeStationInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblPreWC").ToString());//2>0
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCurWC").ToString());//3>1
        dt.Columns.Add("Family");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblDefect").ToString());//0>2
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCause").ToString());//*>3
        dt.Columns.Add("MajorPart");//1>4
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblNextWC").ToString());//4>5
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEditor").ToString());//5>6
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCdt").ToString());//6>7
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblUdt").ToString());//7>8
        dt.Columns.Add("ID");//8>9

        if (list != null && list.Count != 0)
        {
            foreach (DefectCodeStationInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.pre_stn + " " + temp.preName;
                dr[1] = temp.crt_stn + " " + temp.curName;
                dr[2] = temp.family;
                dr[3] = temp.defect + " " + temp.dfDescr;
                dr[4] = temp.cause;
                dr[5] = temp.majorPart;
                dr[6] = temp.nxt_stn + " " + temp.nxtName;
                dr[7] = temp.editor;
                dr[8] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[9] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[10] = temp.id;
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

    //private Boolean showDefectList()
    //{
    //    IList<DefectCodeStationInfo> lstDefcet = new List<DefectCodeStationInfo>();
    //    try
    //    {
    //        string type = this.hidPerWCTopSelect.Value;
            
    //        lstDefcet = iDefectStation.GetDefectStationByPreStation(type);
    //        if (lstDefcet == null || lstDefcet.Count == 0)
    //        {
    //            bindTable(null, DEFAULT_ROWS);
    //        }
    //        else
    //        {
    //            bindTable(lstDefcet, DEFAULT_ROWS);
    //        }
    //    }
    //    catch (FisException fex)
    //    {
    //        bindTable(null, DEFAULT_ROWS);
    //        showErrorMessage(fex.mErrmsg);
    //        return false;
    //    }
    //    catch (System.Exception ex)
    //    {
    //        bindTable(null, DEFAULT_ROWS);
    //        showErrorMessage(ex.Message);
    //        return false;
    //    }
    //    return true;
    //}

    private Boolean showDefectList()
    {
        IList<DefectCodeStationInfo> lstDefcet = new List<DefectCodeStationInfo>();
        try
        {
            string type = this.hidPerWCSelect.Value;

            lstDefcet = iDefectStation.GetDefectStationByPreStation(type);
            if (lstDefcet == null || lstDefcet.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(lstDefcet, DEFAULT_ROWS);
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
    
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return errorMsg;
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("setCommonFocus();");
        scriptBuilder.AppendLine("HideWait();");
        //scriptBuilder.AppendLine("window.setTimeout('function(){getCommonInputObject().focus();getCommonInputObject().select();}',0);");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }

    private void DeleteComplete()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("DeleteComplete();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "DeleteComplete", scriptBuilder.ToString(), false);
    }

    private void AddUpdateComplete(string itemId)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("AddUpdateComplete('" + itemId + "')");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "AddUpdateComplete", scriptBuilder.ToString(), false);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(11);//(17); //pre wc 
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(11);//(17); //cur wc
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(15);//(15); //defect
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(15);//(17); //next wc
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(8);//(15); //editor
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(12);//(19); //cdt
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(4);
        //gd.HeaderRow.Cells[6].Width = Unit.Percentage(10);
    }
}
