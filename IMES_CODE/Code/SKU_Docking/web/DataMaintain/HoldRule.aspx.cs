﻿
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
using IMES.DataModel;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;

public partial class DataMaintain_HoldRule : System.Web.UI.Page
{
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 13;
    public String UserId;
    public String Customer;
    private IHoldRule iHoldRule = null;
    private string gFamily = string.Empty;
    private string gVc = string.Empty;
    
    private bool gHasHightRow = false;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            this.HiddenUserName.Value = UserId;
            iHoldRule = ServiceAgent.getInstance().GetMaintainObjectByName<IHoldRule>(WebConstant.HoldRuleMaintainObject);
            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_msgConfirmDelete").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_msgInput").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                initLabel();
                initlineTop();
                initfamilyTop();
                initmodelTop();
                initline();
                initfamily();
                initHoldStation();
                initHoldCode(); 
                initCheckInStation();
                
                bindTable(null, DEFAULT_ROWS);
            }
            this.cmbFamilyTop.SelectedIndexChanged += new EventHandler(cmbFamilyTop_Selected);
            this.cmbFamilyTop.AutoPostBack = true;
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
    }

    private void initLabel()
    {
        this.lblFamily.Text=this.GetLocalResourceObject(Pre+"_lblFamily").ToString();
        this.btnQuery.Value=this.GetLocalResourceObject(Pre+"_btnQuery").ToString();
        this.lblMasterLabelList.Text=this.GetLocalResourceObject(Pre+"_lblMasterLabelList").ToString();
        this.btnDelete.Value=this.GetLocalResourceObject(Pre+"_btnDelete").ToString();
        this.btnSave.Value=this.GetLocalResourceObject(Pre+"_btnSave").ToString();
    }

    private void initlineTop()
    {
        IList<string> list = iHoldRule.GetLineTop();
        this.cmbLineTop.Items.Clear();
        this.cmbLineTop.Items.Add(new ListItem("ALL",""));
        foreach (string items in list)
        {
            if (items == "")
            {
                continue;
            }
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbLineTop.Items.Add(selectListItem);
        }
    }

    private void initfamilyTop()
    {
        IList<string> list = iHoldRule.GetFamilyTop();
        this.cmbFamilyTop.Items.Clear();
        this.cmbFamilyTop.Items.Add(new ListItem("ALL", ""));
        foreach (string items in list)
        {
            if (items == "")
            {
                continue;
            }
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbFamilyTop.Items.Add(selectListItem);
        }
    }

    private void initmodelTop()
    {
        this.cmbModelTop.Items.Clear();
        this.cmbModelTop.Items.Add(new ListItem("ALL", ""));
    }

    private void initline()
    {
        IList<string> list = iHoldRule.GetLine();
        this.cmbLine.Items.Clear();
        this.cmbLine.Items.Add(new ListItem("", ""));
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbLine.Items.Add(selectListItem);
        }
    }

    private void initfamily()
    {
        IList<string> list = iHoldRule.GetFamily();
        this.cmbFamily.Items.Clear();
        this.cmbFamily.Items.Add(new ListItem("", ""));
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbFamily.Items.Add(selectListItem);
        }
    }

    private void initCheckInStation()
    {
        IList<string> list = iHoldRule.GetCheckInStation();
        this.cmbCheckINStation.Items.Clear();
        this.cmbCheckINStation.Items.Add(new ListItem("", ""));
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbCheckINStation.Items.Add(selectListItem);
        }
    }

    private void initHoldStation()
    {
        IList<string> list = iHoldRule.GetHoldStation();
        this.cmbHoldStation.Items.Clear();
        this.cmbHoldStation.Items.Add(new ListItem("", ""));
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbHoldStation.Items.Add(selectListItem);
        }
    }

    private void initHoldCode()
    {
        IList<string> list = iHoldRule.GetHoldCode();
        this.cmbHoldCode.Items.Clear();
        this.cmbHoldCode.Items.Add(new ListItem("", ""));
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbHoldCode.Items.Add(selectListItem);
        }
    }

    private void cmbFamilyTop_Selected(object sender, System.EventArgs e)
    {
        string family = this.cmbFamilyTop.SelectedItem.Text;
        IList<string> list = iHoldRule.GetModelTop(family);
        this.cmbModelTop.Items.Clear();
        this.cmbModelTop.Items.Add(new ListItem("ALL", ""));
        foreach (string items in list)
        {
            if (items == "")
            {
                continue;
            }
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbModelTop.Items.Add(selectListItem);
        }
    }

    private void bindTable(IList<HoldRuleInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("ID");//0
        dt.Columns.Add("Line");
        dt.Columns.Add("Family");
        dt.Columns.Add("Model");
        dt.Columns.Add("CUSTSN");
        dt.Columns.Add("CheckINStation");
        dt.Columns.Add("HoldStation");
        dt.Columns.Add("HoldCode");
        dt.Columns.Add("HoldDescr");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");//11
        dt.Columns.Add("Chk");

        if (list != null && list.Count != 0)
        {
            foreach (HoldRuleInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ID;
                dr[1] = temp.Line;
                dr[2] = temp.Family;
                dr[3] = temp.Model;
                dr[4] = temp.CUSTSN;
                dr[5] = temp.CheckInStaion;
                dr[6] = temp.HoldStaion;
                dr[7] = temp.HoldCode;
                dr[8] = temp.HoldDescr;
                dr[9] = temp.Editor;
                dr[10] = temp.Cdt;
                dr[11] = temp.Udt;
                
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }
        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        //setColumnWidth();
        initTableColumnHeader();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
    }


    protected void gd_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < gd.Rows.Count; i++)
        {
          
            if (gd.Rows[i].Cells[0].Text.Trim().Equals("&nbsp;")) //ID
            {
                gd.Rows[i].Cells[12].Controls[1].Visible = false;
            }
            else
            {
                gd.Rows[i].Cells[12].Controls[1].Visible = true;
            }
            
        }
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Style.Add("display", "none");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                   
                }
                CheckBox chkSelect = e.Row.FindControl("chk") as CheckBox;
                DataRowView drv = e.Row.DataItem as DataRowView;
                string id = drv["ID"].ToString();
                chkSelect.Attributes.Add("onclick", string.Format("setSelectVal(this,'{0}');", id));
                
            }

        }
    }

    protected void Query_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string line = this.cmbLineTop.SelectedItem.Value.ToString();
            string family = this.cmbFamilyTop.SelectedItem.Value.ToString();
            string model = this.cmbModelTop.SelectedItem.Value.ToString();
            string custsn = this.txtCUSTSNTop.Text.Trim();
            string custsblistquery = this.HiddenSNQuery.Value.Trim().ToUpper();
           // string custsblistquery = "5CG4525MGQ,5CG5434H94,5CG5434HC9";
            IList<string> inputCUSTSNList = new List<string>();
            string[] strList = custsblistquery.Split(',');
            foreach (string item in strList)
            {
                if (item == "")
                {
                    continue;
                }
                inputCUSTSNList.Add(item.ToUpper());
            }


            if (inputCUSTSNList.Count > 0 && string.IsNullOrEmpty(custsn))
            {
                IList<HoldRuleInfo> holdRuleList = new List<HoldRuleInfo>();
                foreach (string sn in inputCUSTSNList)
                {
                    HoldRuleInfo condition = new HoldRuleInfo();
                    condition.CUSTSN = sn;
                    IList<HoldRuleInfo> holdlist=iHoldRule.GetHoldRule(condition);
                    foreach (HoldRuleInfo info in holdlist)
                    {
                        holdRuleList.Add(info);
                    }
                   
                }
                bindTable(holdRuleList, DEFAULT_ROWS);  

            }
            else
            {
                HoldRuleInfo condition = new HoldRuleInfo();
                if (line != "")
                {
                    condition.Line = line;
                }
                if (family != "")
                {
                    condition.Family = family;
                }
                if (model != "")
                {
                    condition.Model = model;
                }
                if (custsn != "")
                {
                    condition.CUSTSN = custsn;
                }
                IList<HoldRuleInfo> holdRuleList = iHoldRule.GetHoldRule(condition);
                bindTable(holdRuleList, DEFAULT_ROWS);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
            //this.hidRecordCount.Value = "0";
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
            //this.hidRecordCount.Value = "0";
        }
        finally
        {
            hideWait();
        }
    }

    protected void Save_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            IList<HoldRuleInfo> holdRuleList = null;
            string line = this.cmbLine.SelectedItem.Value.ToString();
            string family = this.cmbFamily.SelectedItem.Value.ToString();
            string model = this.txtModel.Text.ToString().Trim().ToUpper();
            string custsn = this.hfCUSTSN.Value.ToString().Trim();
            //string custsnList = this.hfCUSTSN.Value.ToString();
            string checkinstation = this.cmbCheckINStation.SelectedItem.Value.ToString();
            string holdstation = this.cmbHoldStation.SelectedItem.Value.ToString();
            string holdcode = this.cmbHoldCode.SelectedItem.Value.ToString();
            string holdDescr = this.txtHoldDescr.Text.ToString().Trim().ToUpper();
            string id = this.hidDeleteID.Value;
            IList<string> inputCUSTSNList = new List<string>();
            string[] strList = custsn.Split(',');
            foreach (string item in strList)
            {
                if (item == "")
                {
                    continue;
                }
                inputCUSTSNList.Add(item.ToUpper());
            }
            
            HoldRuleInfo holdInfo = new HoldRuleInfo();
            holdInfo.Line = line;
            holdInfo.Family = family;
            holdInfo.Model = model;
            holdInfo.CUSTSN = custsn;
            holdInfo.CheckInStaion = checkinstation;
            holdInfo.HoldStaion = holdstation;
            holdInfo.HoldCode = holdcode;
            holdInfo.HoldDescr = holdDescr;
            holdInfo.Editor = UserId;
            holdInfo.Cdt = DateTime.Now;
            holdInfo.Udt = DateTime.Now;
            if (inputCUSTSNList.Count == 0)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    holdInfo.ID = int.Parse(id);
                    iHoldRule.UpdateHoldRule(holdInfo);
                }
                else
                {
                    iHoldRule.InsertHoldRule(holdInfo);
                }
                holdRuleList = iHoldRule.GetHoldRule(new HoldRuleInfo { Line = line,
                                                                        Family = family,
                                                                        Model = model,
                                                                        CUSTSN = custsn });
            }
            else
            {
                if (!string.IsNullOrEmpty(id))
                {
                    holdInfo.ID = int.Parse(id);
                    iHoldRule.UpdateHoldRule(holdInfo);
                }
                else
                {
                    iHoldRule.InsertMultiCustSNHoldRule(inputCUSTSNList, holdInfo);
                }
                holdRuleList = iHoldRule.GetHoldRule(new HoldRuleInfo { Line = line,
                                                                        Family = family,
                                                                        Model = model });
            }
            
            int ID = holdRuleList[0].ID;
            bindTable(holdRuleList, DEFAULT_ROWS);
            initlineTop();
            initfamilyTop();
            initmodelTop();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + ID + "');", true);//
        
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
  //          showAlertErrorMessage(msgHasExistRecord);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
            //this.hidRecordCount.Value = "0";
        }
        finally
        {
           
            hideWait();
        }

    }

    protected void Delete_ServerClick(Object sender, EventArgs e)
    {
        try
        {

            IList<HoldRuleInfo> holdRuleList = null;
            string id = this.hidDeleteID.Value.ToString();
            if (string.IsNullOrEmpty(id))
            {
                throw new FisException("This ID is not Exists");
            }

            IList<string> inputCUSTSNList = new List<string>();
            string[] strList = id.Split(',');
            foreach (string item in strList)
            {
                if (item == "")
                {
                    continue;
                }
                inputCUSTSNList.Add(item.ToUpper());
            }
           
            if (inputCUSTSNList.Count > 0)
            {
                foreach (string deleteid in inputCUSTSNList)
                {
                    int delID = int.Parse(deleteid);
                    iHoldRule.DeleteHoldRule(delID);
                }
            }
            else
            {
                throw new FisException("This ID is not Exists");
            }
            holdRuleList = iHoldRule.GetHoldRule(new HoldRuleInfo
            {
                Line = this.cmbLineTop.SelectedItem.Value.ToString(),
                Family = this.cmbFamilyTop.SelectedItem.Value.ToString(),
                Model = this.cmbModelTop.SelectedItem.Value.ToString(),
                CUSTSN = this.txtCUSTSN.Text.Trim()
            });
            bindTable(holdRuleList, DEFAULT_ROWS);
            initlineTop();
            initfamilyTop();
            initmodelTop();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "Delete", "DeleteComplete();", true);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
            //this.hidRecordCount.Value = "0";
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
            //this.hidRecordCount.Value = "0";
        }
        finally
        {
            hideWait();
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void showAlertErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("alert(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "showAlertErrorMessage", scriptBuilder.ToString(), false);
    }

    private void changeSelectedIndex(string index, string family, string vc,string code)
    {
        //StringBuilder scriptBuilder = new StringBuilder();

        //scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("selectedRowIndex=" + index + ";");
        //this.cmbFamily2.refresh();
        //this.cmbFamily2.InnerDropDownList.SelectedValue = family;
        //scriptBuilder.AppendLine("document.getElementById('" + this.ttVcBottom.ClientID + "').value='" + vc + "';");
        //scriptBuilder.AppendLine("document.getElementById('" + this.ttCode.ClientID + "').value='" + code + "';");
        
        //if (string.Compare(index, "-1", true) == 0)
        //{
        //    scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=true;");
        //}
        //else
        //{
        //    scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=false;");
        //}

        //scriptBuilder.AppendLine("</script>");

        //ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "changeSelectedIndex", scriptBuilder.ToString(), false);
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        
        scriptBuilder.AppendLine("HideWait();");
        
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(4);
        gd.HeaderRow.Cells[2].Width = Unit.Pixel(5);
        gd.HeaderRow.Cells[3].Width = Unit.Pixel(9);
        gd.HeaderRow.Cells[4].Width = Unit.Pixel(9);
        gd.HeaderRow.Cells[5].Width = Unit.Pixel(9);
        gd.HeaderRow.Cells[6].Width = Unit.Pixel(8);

        gd.HeaderRow.Cells[7].Width = Unit.Pixel(7);
        gd.HeaderRow.Cells[8].Width = Unit.Pixel(7);
        gd.HeaderRow.Cells[9].Width = Unit.Pixel(5);
        gd.HeaderRow.Cells[10].Width = Unit.Pixel(13);
        gd.HeaderRow.Cells[11].Width = Unit.Pixel(13);
       // gd.HeaderRow.Cells[12].Width = Unit.Pixel(4);
    }


    private void initTableColumnHeader()
    {
       // this.gd.HeaderRow.Cells[0].Text = "ID";
       // this.gd.HeaderRow.Cells[0].Text = "ID";
        this.gd.HeaderRow.Cells[1].Text = "Line";
        this.gd.HeaderRow.Cells[2].Text = "Family";
        this.gd.HeaderRow.Cells[3].Text = "Model";
        this.gd.HeaderRow.Cells[4].Text ="CUSTSN";
        this.gd.HeaderRow.Cells[5].Text = "CheckINStation";
        this.gd.HeaderRow.Cells[6].Text = "HoldStation";
        this.gd.HeaderRow.Cells[7].Text = "HoldCode";
        this.gd.HeaderRow.Cells[8].Text = "HoldDescr";
        this.gd.HeaderRow.Cells[9].Text = "Editor";
        this.gd.HeaderRow.Cells[10].Text = "Cdt";
       this.gd.HeaderRow.Cells[11].Text = "Udt";
       this.gd.HeaderRow.Cells[12].Text = "Select";
        this.gd.HeaderRow.Cells[0].Width = Unit.Pixel(0);
        this.gd.HeaderRow.Cells[1].Width = Unit.Pixel(40);
        this.gd.HeaderRow.Cells[2].Width = Unit.Pixel(50);
        this.gd.HeaderRow.Cells[3].Width = Unit.Pixel(90);
        this.gd.HeaderRow.Cells[4].Width = Unit.Pixel(90);
        this.gd.HeaderRow.Cells[5].Width = Unit.Pixel(70);
        this.gd.HeaderRow.Cells[6].Width = Unit.Pixel(70);
        this.gd.HeaderRow.Cells[7].Width = Unit.Pixel(70);
        this.gd.HeaderRow.Cells[8].Width = Unit.Pixel(100);
        this.gd.HeaderRow.Cells[9].Width = Unit.Pixel(50);
        this.gd.HeaderRow.Cells[10].Width = Unit.Pixel(90);
        this.gd.HeaderRow.Cells[11].Width = Unit.Pixel(90);
        this.gd.HeaderRow.Cells[12].Width = Unit.Pixel(30);

        this.gd.HeaderRow.Cells[12].Width = Unit.Pixel(30);

        //this.gvDN.HeaderRow.Cells[9].Visible = false;
        //this.gvDN.HeaderRow.Cells[10].Visible = false;
        //   this.gvDN.HeaderRow.Cells[9].Style.Add("display", "none");
        //   this.gvDN.HeaderRow.Cells[10].Style.Add("display", "none");
    }
    
}