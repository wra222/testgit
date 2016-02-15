using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;


public partial class DataMaintain_QCRatio : System.Web.UI.Page
{
    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IQCRatio iQCRatio;
    private IFamily2 iFamily;
    Boolean isCustomerLoad;
    Boolean isFamilyLoad;

    private const int COL_NUM = 9;//8

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
    public string pmtMessage13;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            isCustomerLoad = false;
            isFamilyLoad = false;
            this.cmbCustomer.InnerDropDownList.Load += new EventHandler(cmbCustomer_Load);
            this.cmbMaintainFamily.InnerDropDownList.Load += new EventHandler(cmbFamily_Load);
            //this.cmbCustomer.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbCustomer_SelectedChange);
            iQCRatio = ServiceAgent.getInstance().GetMaintainObjectByName<IQCRatio>(WebConstant.MaintainQCRatioObject);
            iFamily = ServiceAgent.getInstance().GetMaintainObjectByName<IFamily2>(WebConstant.MaintainCommonObject);

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
            pmtMessage13 = this.GetLocalResourceObject(Pre + "_pmtMessage13").ToString();

            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId; //UserInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                bindTable(null, DEFAULT_ROWS);
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

    protected void btnFamilyChange_ServerClick(Object sender, EventArgs e)
    {
        string family = this.dFamilyTop.Text.Trim().ToUpper();
        string cusomer = iFamily.GetCustomerByFamily(family);
        if (cusomer != "")
        {
            this.cmbCustomer.InnerDropDownList.SelectedIndex = this.cmbCustomer.InnerDropDownList.Items.IndexOf(this.cmbCustomer.InnerDropDownList.Items.FindByValue(cusomer));
            string familyId = replaceSpecialChart(family);
            ShowFamilyByCustomer();
            ShowListByCustom();
            this.updatePanel1.Update();
            this.updatePanel2.Update();
            this.updatePanel3.Update();
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "findFamily('" + familyId + "');DealHideWait();getMaintainModelByFamilyCmbObj().disabled = true;", true);
       
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "NoMatchFamily();DealHideWait();", true);
        }
    }

    protected void btnComBoxFamilyChange_ServerClick(Object sender, EventArgs e)
    {
        int selectedIndex = this.cmbMaintainFamily.InnerDropDownList.SelectedIndex;
        if (selectedIndex == 0)
        {
            this.cmbMaintainModelByFamily.Family = "";
            this.cmbMaintainModelByFamily.initMaintainModelByFamily();
            this.updatePanel4.Update();
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ComBoxFamilyChange", "DealHideWait();getMaintainModelByFamilyCmbObj().disabled = true;", true);            
        }
        else
        {
            string family = this.cmbMaintainFamily.InnerDropDownList.SelectedValue;
            this.cmbMaintainModelByFamily.Family = family;
            this.cmbMaintainModelByFamily.initMaintainModelByFamily();
            if (this.dModelValue.Value != "")
            {
                ListItem selectedValue = this.cmbMaintainModelByFamily.InnerDropDownList.Items.FindByValue(this.dModelValue.Value);
                if (selectedValue != null)
                {
                    selectedValue.Selected = true;
                }
            }
            this.updatePanel4.Update();
            if (this.cmbMaintainModelByFamily.InnerDropDownList.Items.Count ==1)
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ComBoxFamilyChange", "DealHideWait();getMaintainModelByFamilyCmbObj().disabled = true;", true);
            else
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ComBoxFamilyChange", "DealHideWait();getMaintainModelByFamilyCmbObj().disabled = false;", true);            
        }
    }
    

    protected void btnCustomerChange_ServerClick(object sender, System.EventArgs e)
    {

        ShowFamilyByCustomer();
        ShowListByCustom();
        //此时model列表应该为空，且不能选
        this.cmbMaintainModelByFamily.Family = "";
        this.cmbMaintainModelByFamily.initMaintainModelByFamily();

        this.updatePanel2.Update();
        this.updatePanel3.Update();
        this.updatePanel4.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();getMaintainModelByFamilyCmbObj().disabled = true;", true);
        
    }

    private void cmbCustomer_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbCustomer.IsPostBack)
        {
            isCustomerLoad = true;
            CheckAndStart();

        }
    }

    private void cmbFamily_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbMaintainFamily.IsPostBack)
        {
            isFamilyLoad = true;
            CheckAndStart();
        }
    }

    private void CheckAndStart()
    {
        if (isCustomerLoad == true && isFamilyLoad == true)
        {
            ShowFamilyByCustomer();
            ShowListByCustom();
        }
    }


    private void ShowFamilyByCustomer()
    {
        String customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
        this.cmbMaintainFamily.Customer = customer;
        this.cmbMaintainFamily.initMaintainFamily();
    }

    private Boolean ShowListByCustom()
    {
        String customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
        try
        {
            DataTable dataList;
            dataList = iQCRatio.GetQCRatioList(customer);
            if (dataList == null || dataList.Rows.Count == 0)
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


    //private void ShowMessage(string p)
    //{
    //    throw new NotImplementedException(p);
    //}

    private void initLabel()
    {
        this.lblCustomer.Text = this.GetLocalResourceObject(Pre + "_lblCustomer").ToString();
        this.lblFamilyTop.Text=this.GetLocalResourceObject(Pre + "_lblFamily").ToString();


        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblQCRatio.Text = this.GetLocalResourceObject(Pre + "_lblQCRatio").ToString();
        this.lblEQQCRatio.Text = this.GetLocalResourceObject(Pre + "_lblEQQCRatio").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblPAQCRatio.Text = this.GetLocalResourceObject(Pre + "_lblPAQCRatio").ToString();
        this.lblRPAQCRatio.Text = this.GetLocalResourceObject(Pre + "_lblRPAQCRatio").ToString();

        this.lblList.Text=this.GetLocalResourceObject(Pre + "_lblQCRatioList").ToString();

        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        //gd.HeaderRow.Cells[0].Width = Unit.Percentage(18);
        //gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        //gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[5].Width = Unit.Percentage(12);
        //gd.HeaderRow.Cells[6].Width = Unit.Percentage(14);
        //gd.HeaderRow.Cells[7].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(11);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(11);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(13);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(13);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        QCRatioDef item = new QCRatioDef();

        String model = this.cmbMaintainModelByFamily.InnerDropDownList.SelectedValue;
        if (model != "")
        {
            item.Family = model;
        }
        else
        {
            item.Family = this.cmbMaintainFamily.InnerDropDownList.SelectedValue;
        }

        item.QCRatio = this.dQCRatio.Text.Trim();
        item.EOQCRatio = this.dEQQCRatio.Text.Trim();
        item.PAQCRatio = this.dPAQCRatio.Text.Trim();
        item.RPAQCRatio = this.dRPAQCRatio.Text.Trim();
        item.Customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
        item.Editor = this.HiddenUserName.Value; 

        string oldItemId = this.dOldId.Value.Trim();
        try
        {
            iQCRatio.SaveQCRatio(item, oldItemId);
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
        ShowListByCustom();
               
        String itemId = replaceSpecialChart(item.Family);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        QCRatioDef item = new QCRatioDef();
        String itemId;

        String model = this.cmbMaintainModelByFamily.InnerDropDownList.SelectedValue;
        if (model != "")
        {
            item.Family = model;
        }
        else
        {
            item.Family = this.cmbMaintainFamily.InnerDropDownList.SelectedValue;
        }

        item.QCRatio = this.dQCRatio.Text.Trim();
        item.EOQCRatio = this.dEQQCRatio.Text.Trim();
        item.PAQCRatio = this.dPAQCRatio.Text.Trim();
        item.RPAQCRatio = this.dRPAQCRatio.Text.Trim();
        item.Customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
        item.Editor = this.HiddenUserName.Value; 

        try
        {
            itemId=iQCRatio.AddQCRatio(item);
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
        ShowListByCustom();
        itemId = replaceSpecialChart(itemId);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);

    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        //string familyId = this.dFamily.Text.Trim();
        string oldId = this.dOldId.Value.Trim();
        try
        {
            QCRatioDef item = new QCRatioDef();
            item.Family = oldId;
            iQCRatio.DeleteQCRatio(item);
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
        ShowListByCustom();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);


    }

    private void bindTable(DataTable  list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        //SELECT C.NewFamily,A.[QCRatio],A.[EOQCRatio],A.[PAQCRatio],A.Editor,C.Model,A.Cdt,A.Udt, C.FamilyKey ,C.Flag FROM [QCRatio] AS A INNER JOIN
        //(
        //SELECT Customer AS FamilyKey,0 AS Flag, Customer AS NewFamily, '' AS Model FROM Customer WHERE Customer=@ Customer
        //UNION  
        //SELECT [Model] AS FamilyKey,2 AS Flag, [Family] AS NewFamily, [Model] AS Model FROM [Model] WHERE Family in (SELECT Family FROM [Family] WHERE [CustomerID]=@ Customer)
        //UNION
        //SELECT Family AS FamilyKey,1 AS Flag, Family AS NewFamily, '' AS Model FROM [Family] WHERE [CustomerID]= @ Customer 
        //) AS C ON A.[Family]=C.FamilyKey ORDER BY NewFamily, Model 
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemFamily").ToString());  //0
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemModel").ToString());   
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemQCRatio").ToString());  //1
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEQQCRatio").ToString()); //2
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemPAQCRatio").ToString()); //3
        
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemRPAQCRatio").ToString()); //3

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString()); //4
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemModel").ToString()); //5
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString()); //6
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString()); //7
        dt.Columns.Add("FamilyKey"); //8
        dt.Columns.Add("Flag"); //9

        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                dr = dt.NewRow();
                // for (int j = 0; j < list.Rows[i].ItemArray.Count(); j++)
                //{
                //    //dr[j] = Null2String(list.Rows[i][j]);
                //    if (list.Rows[i][j].GetType() == typeof(DateTime))
                //    {
                //        dr[j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                //    }
                //    else
                //    {
                //        dr[j] = Null2String(list.Rows[i][j]);
                //    }
                //}

                dr[0] = Null2String(list.Rows[i][0]);
                dr[1] = Null2String(list.Rows[i][5]);
                dr[2] = Null2String(list.Rows[i][1]);
                dr[3] = Null2String(list.Rows[i][2]);
                dr[4] = Null2String(list.Rows[i][3]);

                dr[5] = Null2String(list.Rows[i][10]);

                dr[6] = Null2String(list.Rows[i][4]);
                dr[7] = ((System.DateTime)list.Rows[i][6]).ToString("yyyy-MM-dd HH:mm:ss");
                dr[8] = ((System.DateTime)list.Rows[i][7]).ToString("yyyy-MM-dd HH:mm:ss");

                dr[9] = Null2String(list.Rows[i][8]);
                dr[10] = Null2String(list.Rows[i][9]);
                dt.Rows.Add(dr);
            }
            
            for (int i = list.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Rows.Count.ToString();
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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true); 
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
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
