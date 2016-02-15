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
//using IMES.Station.Interface.StationIntf;
//using IMES.Station.Interface.CommonIntf;
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;

//ITC-1136-0003  2010,2,4
//ITC-1136-0005  2010,2,4
//ITC-1136-0013 2010,2,4

public partial class DataMaintain_Family : System.Web.UI.Page
{

    //!!! need change
    public String userName;
    
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 16;
    private IFamily2 iFamily;
    private const int COL_NUM = 6;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    //public string pmtMessage6;
    //public string pmtMessage7;
    //public string pmtMessage8;
    //public string pmtMessage9;
    //public string pmtMessage10;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            iFamily = (IFamily2)ServiceAgent.getInstance().GetMaintainObjectByName<IFamily2>(WebConstant.MaintainCommonObject);
            
            this.cmbCustomer.InnerDropDownList.Load += new EventHandler(cmbCustomer_Load);
            //this.cmbCustomer.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbCustomer_SelectedChange); 
            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                //pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
                //pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
                //pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
                //pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
                //pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();
                
                userName = Master.userInfo.UserId; //UserInfo.UserName;
                initLabel();
                initcmb();
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
    
    protected void btnRefreshCustomerList_Click(Object sender, EventArgs e)
    {
        this.cmbCustomer.refresh();
        this.cmbCustomer.InnerDropDownList.Text = this.HiddenCustomerName.Value;
        cmbCustomer_SelectedChange(sender, e);
    }


    protected void btnRefreshFamilyList_Click(Object sender, EventArgs e)
    {
        cmbCustomer_SelectedChange(sender, e);
    }

    protected void btnFamilyChange_ServerClick(Object sender, EventArgs e)
    {
        string family=this.dFamilyTop.Text.Trim().ToUpper();
        string cusomer=iFamily.GetCustomerByFamily(family);
        if (cusomer != "")
        {
            this.cmbCustomer.InnerDropDownList.SelectedIndex = this.cmbCustomer.InnerDropDownList.Items.IndexOf(this.cmbCustomer.InnerDropDownList.Items.FindByValue(cusomer));
            DealFamilyChange(family);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "saveUpdate", "NoMatchFamily();", true);
        }
    }

    private void DealFamilyChange(string family)
    {
        ShowFamilyListByCustom();
        string familyId = replaceSpecialChart(family);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "findFamily('" + familyId + "');", true);
        this.updatePanel.Update();
    }

    private void cmbCustomer_SelectedChange(object sender, System.EventArgs e)
    {
        ShowFamilyListByCustom();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "ShowRowEditInfo();", true);
        this.updatePanel.Update();
    }

    private void cmbCustomer_Load(object sender, System.EventArgs e)
    {
        ShowFamilyListByCustom();
    }

    private Boolean ShowFamilyListByCustom()
    {
        String costomer = this.cmbCustomer.InnerDropDownList.SelectedValue;
        try
        {
            //IList<FamilyDef> dataList;
            //DataTable dt;
            //dt = iFamily.GetFamily(costomer);
            //if (dt==null|| dt.Rows.Count == 0)
            //{
            //    bindTable(null, DEFAULT_ROWS);
            //}
            //else
            //{
            //    bindTable(dt);
            //}

            IList<FamilyDef> dataList;
            dataList = iFamily.GetFamilyInfoList(costomer);
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


    private void ShowMessage(string p)
    {
        throw new NotImplementedException(p);
    }

    private void initcmb()
    {
        this.cmbcustom.Items.Clear();
        ICustomer iCustomer = ServiceAgent.getInstance().GetMaintainObjectByName<ICustomer>(WebConstant.MaintainCommonObject);
        IList<CustomerInfo> lstCustomer = iCustomer.GetCustomerList();
        this.cmbcustom.Items.Add(string.Empty);
        foreach (CustomerInfo item in lstCustomer)
        {
            ListItem value = new ListItem();
            value.Text = item.customer;
            value.Value = item.customer;
            this.cmbcustom.Items.Add(value);
        }
    }

    private void initLabel()
    {
        this.lblCustomer.Text = this.GetLocalResourceObject(Pre + "_lblCustomerText").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblListText").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamilyText").ToString();
        this.lblDescription.Text = this.GetLocalResourceObject(Pre + "_lblDescriptionText").ToString();
        this.lblFamilyTop.Text = this.GetLocalResourceObject(Pre + "_lblFamilyText").ToString();

        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.InnerText = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnInfo.Value = this.GetLocalResourceObject(Pre + "_btnInfo").ToString();
        this.btnInfoName.Value = this.GetLocalResourceObject(Pre + "_btnInfoName").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(20);  //.Pixel(240);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(15);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        string familyId = this.dFamily.Text.Trim().ToUpper();
        string description = this.dDescription.Text.Trim();
        string oldFamilyId = this.dOldFamilyId.Value.Trim();
        string customer = this.cmbcustom.SelectedValue.ToString();
        if (!oldFamilyId.Equals(familyId))
        {
            showErrorMessage(this.GetLocalResourceObject(Pre + "_ErrSaveNewFamily").ToString());
            return;
        }

        FamilyDef item = new FamilyDef();
        item.Family = familyId;
        item.Descr = description;
        item.CustomerID = customer;
        item.Editor = this.HiddenUserName.Value; 
        //item.CustomerID = this.cmbCustomer.InnerDropDownList.SelectedValue;
        
        try
        {
            iFamily.UpdateFamily(item, oldFamilyId);
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
        ShowFamilyListByCustom();

        familyId = replaceSpecialChart(familyId);

        //ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + familyId + "');", true);

    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        FamilyDef item = new FamilyDef();

        string familyId = this.dFamily.Text.Trim().ToUpper();
        string description = this.dDescription.Text.Trim();
        string customer = this.cmbcustom.SelectedValue.ToString();
        item.Family = familyId;
        item.Descr = description;
        item.CustomerID = customer;
        item.Editor = this.HiddenUserName.Value;
        //item.CustomerID = this.cmbCustomer.InnerDropDownList.SelectedValue;
        
        try
        {
            iFamily.AddFamily(item);
          
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
        ShowFamilyListByCustom();
        familyId = replaceSpecialChart(familyId);

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + familyId + "');", true);
    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        //string familyId = this.dFamily.Text.Trim();
        string oldFamilyId = this.dOldFamilyId.Value.Trim();
        try
        {
            /*IFamilyInfoEx iFamilyInfo = ServiceAgent.getInstance().GetMaintainObjectByName<IFamilyInfoEx>(WebConstant.IFamilyInofoObjectEx);
            IList<FamilyInfoDef> lst = iFamilyInfo.GetFamilyInfoList(oldFamilyId);
            if (lst != null && lst.Count > 0)
            {
                bool Found = false;
                for (int i = 0; i < lst.Count; i++)
                {
                    if (!"".Equals(lst[i].value))
                    {
                        Found = true;
                        break;
                    }
                }
                if (Found)
                {
                    showErrorMessage(this.GetLocalResourceObject(Pre + "_ErrExistFamilyInfo").ToString());
                    return;
                }
            }
            */

            IFamilyManagerEx iFamilyEx = ServiceAgent.getInstance().GetMaintainObjectByName<IFamilyManagerEx>(WebConstant.IFamilyManagerEx);
            IList<IMES.DataModel.ModelInfo> models = iFamilyEx.GetModelByFamily(oldFamilyId);
            if (models != null && models.Count > 0)
            {
                showErrorMessage(this.GetLocalResourceObject(Pre + "_ErrExistModel").ToString());
                return;
            }
            iFamilyEx.DeleteFamily(oldFamilyId, this.cmbCustomer.InnerDropDownList.SelectedValue, this.HiddenUserName.Value);

            /*FamilyDef item = new FamilyDef();
            item.Family = oldFamilyId;
            item.Descr = "";
            item.Descr = "";
            item.Editor = "";
            item.CustomerID = this.cmbCustomer.InnerDropDownList.SelectedValue;
            iFamily.DeleteFamily(item);*/
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
        ShowFamilyListByCustom();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "DeleteComplete();", true);
        //dOldFamilyId.Value = "aaaa";

    }
    private void bindTable(DataTable dt)
    {
        gd.DataSource = dt;
        this.hidRecordCount.Value = dt.Rows.Count.ToString();
        for (int i = dt.Rows.Count; i < DEFAULT_ROWS; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gd.DataBind();
        setColumnWidth();
        //DataTable dt = new DataTable();
        //DataRow dr = null;
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_familyText").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_descriptionText").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_Process").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

        //foreach (DataRow dr in dtFamily.Rows)
        //{
        //    dr = dt.NewRow();
        
        //}

        //if (list != null && list.Count != 0)
        //{
        //    foreach (FamilyDef temp in list)
        //    {
        //        dr = dt.NewRow();

        //        dr[0] = temp.Family;
        //        dr[1] = temp.Descr;
        //        // dr[2] = findProcessByProcess(temp.Family.ToString());
        //        dr[3] = temp.Editor;
        //        dr[4] = temp.cdt;
        //        dr[5] = temp.udt;

        //        dt.Rows.Add(dr);
        //    }

        //    for (int i = list.Count; i < DEFAULT_ROWS; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }

        //    this.hidRecordCount.Value = list.Count.ToString();
        //}
        //else
        //{
        //    for (int i = 0; i < defaultRow; i++)
        //    {
        //        dr = dt.NewRow();
        //        dt.Rows.Add(dr);
        //    }

        //    this.hidRecordCount.Value = "";
        //}

        //gd.DataSource = dt;
        //gd.DataBind();
        //setColumnWidth();
    
    }
    private void bindFamilyType(string customer)
    { 
    
    }
    private void bindTable(IList<FamilyDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_familyText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_descriptionText").ToString());
        dt.Columns.Add("CustomerID");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());
   
        if (list != null && list.Count != 0)
        {
            foreach (FamilyDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.Family;
                dr[1] = temp.Descr;
                dr[2] = temp.CustomerID;
                dr[3] = temp.Editor;
                dr[4] = temp.cdt;
                dr[5] = temp.udt;
        
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
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }
   
    //private void hideWait()
    //{
    //    StringBuilder scriptBuilder = new StringBuilder();

    //    scriptBuilder.AppendLine("<script language='javascript'>");
    //    scriptBuilder.AppendLine("setCommonFocus();");
    //    scriptBuilder.AppendLine("</script>");

    //    ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    //}
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
}
