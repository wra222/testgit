﻿using System;
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
using IMES.Maintain.Interface.MaintainIntf;
using System.Text;

//qa bug no:ITC-1136-0017,ITC-1136-0041,ITC-1136-0059，ITC-1136-0108


public partial class ModelMaintain : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 14;

    IModelManager iModelManager = ServiceAgent.getInstance().GetMaintainObjectByName<IModelManager>(WebConstant.IModelManager);
    IModelManagerEx iModelManagerEx = ServiceAgent.getInstance().GetMaintainObjectByName<IModelManagerEx>(WebConstant.IModelManagerEx);
    IConstValue iConstValue = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValue>(WebConstant.MaintainCommonObject);
    ICustomer iCustomer = ServiceAgent.getInstance().GetMaintainObjectByName<ICustomer>(WebConstant.MaintainCommonObject);
    IFamilyManagerEx iFamily = ServiceAgent.getInstance().GetMaintainObjectByName<IFamilyManagerEx>(WebConstant.IFamilyManagerEx);
    private string editor;

    public static Hashtable indexField = new Hashtable();

    static ModelMaintain()
    {
        indexField.Add("CHK", 0);
        indexField.Add("Model", 1);
        indexField.Add("CustPN", 2);
        indexField.Add("Region", 3);
        indexField.Add("ShipType", 4);
        indexField.Add("Status", 5);
        indexField.Add("OsCode", 6);
        indexField.Add("BomApproveDate", 7);
        indexField.Add("Editor", 8);
        indexField.Add("Cdt", 9);
        indexField.Add("Udt", 10);
        indexField.Add("statusId", 11);
        indexField.Add("OSDesc", 12);
        indexField.Add("Family", 13);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            editor = Master.userInfo.UserId;

            if (!Page.IsPostBack)
            {

                InitLabel();
                queryFirstModelName.Attributes.Add("onkeydown", "queryFirstModelName_OnKeyPress()");
                querySecondModelName.Attributes.Add("onkeydown", "querySecondModelName_OnKeyPress()");
                initCoustomerTop();
                initCoustomer();
                initRegion();
                InitShipTypeSelect();
                InitOSCodeSelect();
                InitStatusSelect();
            }

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
    }

    protected void initCoustomerTop()
    {
        try
        {
            this.cmbCustomerTop.Items.Clear();
            this.cmbCustomerTop.Items.Add(string.Empty);
            IList<CustomerInfo> lstValue = iCustomer.GetCustomerList();
            foreach (CustomerInfo item in lstValue)
            {
                ListItem value = new ListItem();
                value.Text = item.customer;
                value.Value = item.customer;
                this.cmbCustomerTop.Items.Add(value);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void initCoustomer()
    {
        try
        {
            this.cmbCustomer.Items.Clear();
            this.cmbCustomer.Items.Add(string.Empty);
            IList<CustomerInfo> lstValue = iCustomer.GetCustomerList();
            foreach (CustomerInfo item in lstValue)
            {
                ListItem value = new ListItem();
                value.Text = item.customer;
                value.Value = item.customer;
                this.cmbCustomer.Items.Add(value);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnGetFamilyTopList_Click(Object sender, EventArgs e)
    {
        try
        {
            string customer = this.cmbCustomerTop.SelectedValue;
            this.cmbFamilyTop.Items.Clear();
            this.cmbFamilyTop.Items.Add(string.Empty);
            IList<string> lstValue = iFamily.GetFamilyByCustomer(customer);
            foreach (string item in lstValue)
            {
                ListItem value = new ListItem();
                value.Text = item;
                value.Value = item;
                this.cmbFamilyTop.Items.Add(value);
            }
            this.updatePanel1.Update();

            ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "RefreshModelListComplete", "setFamilyTop();", true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    protected void btnGetCustomer_Click(Object sender, EventArgs e)
    {
        try
        {
            string family = this.hidFamily.Value;
            if (!string.IsNullOrEmpty(family))
            {
                IList<string> list = iModelManagerEx.GetCustomerByFamily(family);
                string customer = list[0];
                changeFamily(customer);
                ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "RefreshModelListComplete", "changeselect(\"" + customer + "\");", true);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void changeFamily(string customer)
    {
        try
        {
            this.cmbFamily.Items.Clear();
            this.cmbFamily.Items.Add(string.Empty);
            IList<string> lstValue = iFamily.GetFamilyByCustomer(customer);
            foreach (string item in lstValue)
            {
                ListItem value = new ListItem();
                value.Text = item;
                value.Value = item;
                this.cmbFamily.Items.Add(value);
            }
            this.updatePanel2.Update();
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnGetFamilyList_Click(Object sender, EventArgs e)
    {
        try
        {
            string customer = this.cmbCustomer.SelectedValue;
            this.cmbFamily.Items.Clear();
            this.cmbFamily.Items.Add(string.Empty);
            IList<string> lstValue = iFamily.GetFamilyByCustomer(customer);
            foreach (string item in lstValue)
            {
                ListItem value = new ListItem();
                value.Text = item;
                value.Value = item;
                this.cmbFamily.Items.Add(value);
            }
            this.updatePanel2.Update();
        }
        catch (Exception)
        {

            throw;
        }
    }


    protected void initRegion()
    {
        try
        {
            this.CmbRegion.Items.Clear();
            this.CmbRegion.Items.Add(string.Empty);
            IList<RegionInfo> lstValue = iModelManager.GetRegionList();
            foreach (RegionInfo item in lstValue)
            {
                ListItem value = new ListItem();
                value.Text = item.Region;
                value.Value = item.Region;
                this.CmbRegion.Items.Add(value);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void InitShipTypeSelect()
    {
        try
        {
            this.CmbShipType.Items.Clear();
            this.CmbShipType.Items.Add(string.Empty);
            IList<ConstValueInfo> lstValue = iModelManager.GetConstValueListByType("Model.ShipType");
            foreach (ConstValueInfo item in lstValue)
            {
                ListItem value = new ListItem();
                value.Text = item.value;
                value.Value = item.value;
                this.CmbShipType.Items.Add(value);
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void InitOSCodeSelect()
    {
        try
        {
            this.cmbOSCode.Items.Clear();
            this.cmbOSCode.Items.Add(string.Empty);
            IList<ConstValueInfo> lstValue = iModelManager.GetConstValueListByType("Model.OSCode");
            foreach (ConstValueInfo item in lstValue)
            {
                ListItem value = new ListItem();
                value.Text = item.value;
                value.Value = item.description;
                this.cmbOSCode.Items.Add(value);
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    
    protected void InitStatusSelect()
    {
        string value1 = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbModelStatusItemValue1").ToString();
        string value2 = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbModelStatusItemValue2").ToString();

        selStatus.Items.Add(new ListItem(value1, "1"));
        selStatus.Items.Add(new ListItem(value2, "0"));
    }

    private void mkTable(IList<ModelMaintainInfo> modellist)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("Model");//0
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colprocess").ToString());//1
        dt.Columns.Add("CustPN");//2
        dt.Columns.Add("Region");//3
        dt.Columns.Add("ShipType");//4
        dt.Columns.Add("Status");//5
        dt.Columns.Add("OsCode");//6
        dt.Columns.Add("BomApproveDate");//7
        dt.Columns.Add("Editor");//8
        dt.Columns.Add("Cdt");//9
        dt.Columns.Add("Udt");//10
        dt.Columns.Add("statusId");//hidden//11
        dt.Columns.Add("OSDesc");//hidden//12
        dt.Columns.Add("Family");//hidden//13
        

        if (modellist != null && modellist.Count != 0)
        {
            foreach (ModelMaintainInfo temp in modellist)
            {
                dr = dt.NewRow();

                dr[0] = temp.Model;
                dr[1] = temp.CustPN;
                dr[2] = temp.Region;
                dr[3] = temp.ShipType;

                if (temp.Status == "1")//normal:1
                {
                    dr[4] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbModelStatusItemValue1").ToString();
                }
                else
                {//hold:0
                    dr[4] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbModelStatusItemValue2").ToString();
                }

                dr[5] = temp.OsCode;
                if (temp.BomApproveDate == DateTime.MinValue)
                {
                    dr[6] = "";
                }
                else
                {
                    dr[6] = temp.BomApproveDate.Year + "/" + temp.BomApproveDate.Month + "/" + temp.BomApproveDate.Day;
                }
                dr[7] = temp.Editor;

                if (temp.Cdt == DateTime.MinValue)
                {
                    dr[8] = "";
                }
                else
                {
                    dr[8] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (temp.Udt == DateTime.MinValue)
                {
                    dr[9] = "";
                }
                else
                {
                    dr[9] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                }



                dr[10] = temp.Status;

                dr[11] = temp.OSDesc;

                dr[12] = temp.Family;

                dt.Rows.Add(dr);
            }

            for (int i = modellist.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();

                dt.Rows.Add(dr);
            }
        }

        gdModelList.DataSource = dt;
        gdModelList.DataBind();
        clearInputs();
    }

    protected void bindTable()
    {
        try
        {
            string familyId = this.cmbFamilyTop.SelectedValue;
            IList<ModelMaintainInfo> modellist = iModelManager.GetModelList(familyId);

            mkTable(modellist);
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
    }

    protected void btnRefreshModelList_Click(Object sender, EventArgs e)
    {
        bindTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "RefreshModelListComplete", "RefreshModelListComplete();", true);
    }

    protected void bindTableForModelNameChange()
    {
        try
        {
            IModelManagerEx iModelManagerEx = ServiceAgent.getInstance().GetMaintainObjectByName<IModelManagerEx>(WebConstant.IModelManagerEx);
            string modelno = queryFirstModelName.Text.Trim().ToUpper();
            IList<ModelMaintainInfo> modellist = iModelManagerEx.GetModelListByPartialModelNo(modelno, 100);

            mkTable(modellist);
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
    }

    protected void btnModelNameChange_Click(Object sender, EventArgs e)
    {
        bindTableForModelNameChange();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "RefreshModelListForModelNameChangeComplete", "RefreshModelListForModelNameChangeComplete();", true);
    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lblFamilyTop.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblModelList.Text = this.GetLocalResourceObject(Pre + "_lblModelList").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblCustPN.Text = this.GetLocalResourceObject(Pre + "_lblCustPN").ToString();
        this.lblRegion.Text = this.GetLocalResourceObject(Pre + "_lblRegion").ToString();
        this.lblQueryFirstModelName.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblShipType.Text = this.GetLocalResourceObject(Pre + "_lblShipType").ToString();
        this.lblStatus.Text = this.GetLocalResourceObject(Pre + "_lblStatus").ToString();
        this.lblOSCode.Text = this.GetLocalResourceObject(Pre + "_lblOSCode").ToString();
        this.lblOSDesc.Text = this.GetLocalResourceObject(Pre + "_lblOSDesc").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnInfo.Value = this.GetLocalResourceObject(Pre + "_btnInfo").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnInfoName.Value = this.GetLocalResourceObject(Pre + "_btnInfoName").ToString();
        //this.lblProcess.Text = this.GetLocalResourceObject(Pre + "_lblProcess").ToString();
        //setFocus();


    }

    private void setColumnWidth()
    {
        //gdModelList.HeaderRow.Cells[0].Width = Unit.Pixel(80);
        //gdModelList.HeaderRow.Cells[Convert.ToInt16(indexField["Model"])].Width = Unit.Pixel(150);
        //gdModelList.HeaderRow.Cells[Convert.ToInt16(indexField["CustPN"])].Width = Unit.Pixel(100);
        //gdModelList.HeaderRow.Cells[Convert.ToInt16(indexField["Region"])].Width = Unit.Pixel(150);
        //gdModelList.HeaderRow.Cells[3].Width = Unit.Pixel(150);
    }


    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[Convert.ToInt16(indexField["Udt"])].Style.Add("display", "none");//Udt
        e.Row.Cells[Convert.ToInt16(indexField["statusId"])].Style.Add("display", "none");//OSDesc
        e.Row.Cells[Convert.ToInt16(indexField["OSDesc"])].Style.Add("display", "none");//status
        e.Row.Cells[Convert.ToInt16(indexField["Family"])].Style.Add("display", "none");//Family

        e.Row.Cells[Convert.ToInt16(indexField["CHK"])].Style.Add("width", "40px");
        e.Row.Cells[Convert.ToInt16(indexField["Model"])].Style.Add("width", "150px");//
        e.Row.Cells[Convert.ToInt16(indexField["CustPN"])].Style.Add("width", "100px");//
        e.Row.Cells[Convert.ToInt16(indexField["Region"])].Style.Add("width", "70px");//
        e.Row.Cells[Convert.ToInt16(indexField["ShipType"])].Style.Add("width", "100px");//
        
        
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

    protected void btnAdd_Click(Object sender, EventArgs e)
    {
        string modelName;
        try
        {
            string family = this.cmbFamily.SelectedValue;
            modelName = txtModel.Text.ToUpper();

            if (iModelManagerEx.ModelExist(modelName))
            {
                showErrorMessage(this.GetLocalResourceObject(Pre + "_ErrExistModel").ToString());
                return;
            }

            string custPN = txtCustPN.Text;
            string region = CmbRegion.SelectedValue;
            string shiptype = CmbShipType.SelectedValue;
            string status = selStatus.SelectedValue;
            string osCode = cmbOSCode.SelectedItem.Text;
            string osDesc = this.hidDescr.Value;
            string oldModelName = hidModelName.Value;
            ModelMaintainInfo model = new ModelMaintainInfo();

            model.Model = modelName;
            model.CustPN = custPN;
            model.Region = region;
            model.ShipType = shiptype;
            model.Status = status;
            model.OsCode = osCode;
            model.OSDesc = osDesc;
            model.Family = family;
            model.Editor = editor;


            iModelManager.AddModel(model);
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

        bindTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "SaveComplete", "SaveComplete(\"" + modelName + "\");", true);
    }

    protected void btnSave_Click(Object sender, EventArgs e)
    {
        string modelName;
        try
        {
            string family = this.cmbFamily.SelectedValue;
            modelName = txtModel.Text.ToUpper();

            if (!hidModelName.Value.Equals(modelName)) // && iModelManagerEx.ModelExist(modelName))
            {
                showErrorMessage(this.GetLocalResourceObject(Pre + "_ErrSaveNewModel").ToString());
                return;
            }

            string custPN = txtCustPN.Text;
            string region = CmbRegion.SelectedValue;
            string shiptype = CmbShipType.SelectedValue;
            string status = selStatus.SelectedValue;
            string osCode = cmbOSCode.SelectedItem.Text;
            string osDesc = this.hidDescr.Value;
            string oldModelName = hidModelName.Value;

            ModelMaintainInfo model = new ModelMaintainInfo();

            model.Model = modelName;
            model.CustPN = custPN;
            model.Region = region;
            model.ShipType = shiptype;
            model.Status = status;
            model.OsCode = osCode;
            model.OSDesc = osDesc;
            model.Family = family;
            model.Editor = editor;


            iModelManager.SaveModel(model, oldModelName);
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

        bindTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "SaveComplete", "SaveComplete(\"" + modelName + "\");", true);
    }


    
    protected void btnDelete_Click(Object sender, EventArgs e)
    {
        string strModelName = hidModelName.Value;
        
        try
        {
            /*IList<ModelInfoNameAndModelInfoValueMaintainInfo> lst = iModelManager.GetModelInfoList(strModelName);
            if (lst != null && lst.Count > 0)
            {
                bool Found = false;
                for (int i = 0; i < lst.Count; i++)
                {
                    if (!"".Equals(lst[i].Value))
                    {
                        Found = true;
                        break;
                    }
                }
                if (Found)
                {
                    showErrorMessage(this.GetLocalResourceObject(Pre + "_ErrExistModelInfo").ToString());
                    return;
                }
            }

            iModelManager.DeleteModel(strModelName);
            */

            IList<string> lst = iModelManagerEx.GetModelsFromProduct(strModelName, 100);
            if (lst != null && lst.Count > 0)
            {
                showErrorMessage(this.GetLocalResourceObject(Pre + "_ErrExistProduct").ToString());
                return;
            }

            IBOMNodeData iModelBOM = (IBOMNodeData)ServiceAgent.getInstance().GetMaintainObjectByName<IBOMNodeData>(WebConstant.IBOMNodeData);
            IList<string> lstModel = iModelBOM.GetModelsFromModelBOM(strModelName, 100);
            if (lstModel != null && lstModel.Count > 0)
            {
                showErrorMessage(this.GetLocalResourceObject(Pre + "_ErrExistModelBOM").ToString());
                return;
            }

            iModelManagerEx.DeleteModel(strModelName, editor);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
        }

        bindTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "SaveComplete", "SaveComplete(\"" + strModelName + "\");", true);
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");");
        scriptBuilder.AppendLine("clearInputs();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void clearInputs() {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("clearInputs();");
        scriptBuilder.AppendLine("</script>");
        
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "clearInputs", scriptBuilder.ToString(), false);
    }
    

}
