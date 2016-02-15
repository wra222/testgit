
using System;
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
using System.Text;
using System.Linq;



public partial class CreateProductandCombineLCM : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IMO iMO = ServiceAgent.getInstance().GetObjectByName<IMO>(WebConstant.CommonObject);
    //ITravelCardPrint2012 iTravelCardPrint = ServiceAgent.getInstance().GetObjectByName<ITravelCardPrint2012>(WebConstant.TravelCardPrint2012Object);
    //ICombineKeyParts iCombineKeyParts = ServiceAgent.getInstance().GetObjectByName<ICombineKeyParts>(WebConstant.CombineKeyPartsObject);
    ICreateProductandCombineLCM iCreateProductandCombineLCM = ServiceAgent.getInstance().GetObjectByName<ICreateProductandCombineLCM>(WebConstant.CreateProductandCombineLCM);
    public string userId;
    public string customer;
    public string today;
    public int initRowsCount = 6;
    public IList<MOInfo> moList = new List<MOInfo>();
    public String AccountId;
    public String Login;
    public String UserName;
    public String FriendlyName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Pre =this.GetLocalResourceObject("language").ToString();
            this.cmbFamily.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbFamily_Selected);
            this.cmbModelMO.SelectedIndexChanged += new EventHandler(cmbModelMO_Selected);

            string temp = Request["MOPrefix"];
            if (!string.IsNullOrEmpty(temp))
            {
                FriendlyName = Request["MOPrefix"];
            }
            this.hidMOPrefix.Value = FriendlyName;
            InitLabel();
            if (!Page.IsPostBack)
            {
                this.GridViewExt1.DataSource = getNullDataTable();
                this.GridViewExt1.DataBind();
                initTableColumnHeader();

                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;
                this.pCode.Value = Request["PCode"];
                this.station1.Value = Request["Station"];
                this.editor1.Value = userId;
                this.customer1.Value = customer;
                
                this.cmbPdLine.Customer = customer;
                if (string.IsNullOrEmpty(Request["Stage"]))
                {
                    this.cmbPdLine.Stage = "FA";
                }
                else
                {
                    this.cmbPdLine.Stage = Request["Stage"];
                }
                this.cmbPdLine.Station = Request["Station"];
                //this.cmbFamily.Service = "013";
                this.cmbFamily.Customer = customer;
                AccountId = Master.userInfo.AccountId.ToString();
                Login = Master.userInfo.Login;
                UserName = Master.userInfo.UserName;
            }
            //today = DateTime.Now.ToString("yyyy-MM-dd");
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    /// <summary>
    /// </summary>
    private void cmbFamily_Selected(object sender, System.EventArgs e)
    {
        try
        {
            this.lbShowMoQty.Text = "";
            this.lbShowReQty.Text = "";
            this.UpdatePanel1.Update();
            this.UpdatePanel2.Update();
            if (this.cmbFamily.InnerDropDownList.SelectedValue == "")
            {
                this.cmbFamily.Focus();
                initcmbModelMO(this.cmbFamily.InnerDropDownList.SelectedValue.ToString().Trim());
            }
            else
            {
                initcmbModelMO(this.cmbFamily.InnerDropDownList.SelectedValue.ToString().Trim());
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void initcmbModelMO(string family) 
    {
        ListItem item = null;
        IList<MOInfo> tempList = iCreateProductandCombineLCM.GetMOListByFamily(family);
        moList = (from q in tempList
                  where q.friendlyName.StartsWith(FriendlyName)
                  select q).ToList<MOInfo>();
        this.cmbModelMO.Items.Clear();
        this.cmbModelMO.Items.Add(string.Empty);
        if (moList != null)
        {
            foreach (MOInfo temp in moList)
            {
                item = new ListItem(temp.id.Trim() + "-" + temp.model.Trim(), temp.id.Trim() + "-" + temp.qty + "-" + temp.pqty);
                this.cmbModelMO.Items.Add(item);
            }
        }
        this.cmbModelMO.SelectedIndexChanged += new EventHandler(cmbModelMO_Selected);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbModelMO_Selected(object sender, System.EventArgs e)
    {
        int remainQty;
        if (this.cmbModelMO.SelectedValue == "")
        {
            this.lbShowMoQty.Text = "";
            this.lbShowReQty.Text = "";
        }
        else
        {
            try
            {
                string[] qtyList = this.cmbModelMO.SelectedValue.ToString().Split('-');
                this.lbShowMoQty.Text = qtyList[1];
                remainQty = Convert.ToInt32(qtyList[1]) - Convert.ToInt32(qtyList[2]);
                this.lbShowReQty.Text = remainQty.ToString();                                
            }
            catch (FisException ee)
            {
                writeToAlertMessage(ee.mErrmsg);
            }
            catch (Exception ex)
            {
                writeToAlertMessage(ex.Message);
            }            
        }
        this.UpdatePanel1.Update();
        this.UpdatePanel2.Update();
    }

    /// <summary>
    /// </summary>
    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lbMO.Text = this.GetLocalResourceObject(Pre + "_lblMO").ToString();
        this.lbMoQty.Text = this.GetLocalResourceObject(Pre + "_lblMOQty").ToString();
        this.lbReQty.Text = this.GetLocalResourceObject(Pre + "_lblRemainQty").ToString();
        this.btnPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
        this.btpPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.lblCollectionData.Text = this.GetLocalResourceObject(Pre + "_lblCollectionData").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
    }

   
    /// <summary>
    /// µ÷ÓÃweb service´òÓ¡½Ó¿Ú³É¹¦ºóÐèÒªresetÒ³ÃæÐÅÏ¢
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnHidden_Click(object sender, System.EventArgs e)
    {
        try
        {
            showSuccessInfo1();
            //if (this.txtQty.Value == this.lbShowReQty.Text)
            //{
            //    //MB Label ÒÑ¾­È«²¿ÁÐÓ¡Íê±ÏÊ±£¬ÐèÒª½«¸ÃMO ´ÓMO ÏÂÀ­ÁÐ±íÖÐÉ¾³ý£¬µ±Ç°Ñ¡ÔñMO Îª¿Õ£¬Çå¿ÕMO ÊýÁ¿£¬Ê£ÓàÊýÁ¿£»ÆäËûÄÚÈÝ±£³Ö²»±ä
            //    cmbModel_Selected1(sender, e);
            //}
            //else
            //{
            //    cmbMO_Selected(sender, e);
            //}       
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            endWaitingCoverDiv();            
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            endWaitingCoverDiv();            
        }
    }

    /// <summary>
    /// resetÒ³ÃæÐÅÏ¢
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {            
            //showSuccessInfo2();    
            string moValue = this.cmbModelMO.SelectedValue.ToString().Trim();

            initcmbModelMO(this.cmbFamily.InnerDropDownList.SelectedValue.ToString().Trim());
            cmbModelMO_Selected(sender, e);
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);            
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);            
        }       
    }    
    
    /// <summary>
    /// Êä³ö´íÎóÐÅÏ¢
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {
      
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void showSuccessInfo1()
    {
        String script = "<script language='javascript'>" + "\r\n" +
             "test();" + "\r\n" +
             "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "showSuccessInfo1", script, false);
    }

    private void showSuccessInfo2()
    {
        String script = "<script language='javascript'>" + "\r\n" +
             "test1();" + "\r\n" +
             "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "showSuccessInfo2", script, false);
    }

    private void alertNoQueryCondAndFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (alertNoQueryCondAndFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alertNoQueryCondAndFocus", script, false);
    }

    private void alertNoInputModel()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (alertNoInputModel,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alertNoInputModel", script, false);
    }

    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        e.Row.Cells[1].Attributes.Add("style", "display:none;");
        e.Row.Cells[2].Attributes.Add("style", "display:none;");
        e.Row.Cells[9].Attributes.Add("style", "display:none;");
        e.Row.Cells[10].Attributes.Add("style", "display:none;");
        e.Row.Cells[11].Attributes.Add("style", "display:none;");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //ITC-1360-0827
            if (!String.IsNullOrEmpty(e.Row.Cells[1].Text) && !(e.Row.Cells[1].Text.Trim().ToLower() == "&nbsp;"))
            {
                string tmp = e.Row.Cells[1].Text;
                if (tmp.IndexOf(";") != -1)
                {
                    e.Row.Cells[0].Text = "<img src=\"../Images/see.png\" onclick=\"showSubstitute('" + e.Row.Cells[3].Text.Trim() + "','" + e.Row.Cells[1].Text.Trim() + "','" + e.Row.Cells[2].Text.Trim() + "')\"  style=\"cursor:hand;\"/>";

                }
            }
            
            e.Row.Cells[5].ToolTip = e.Row.Cells[10].Text;
            e.Row.Cells[8].ToolTip = e.Row.Cells[9].Text;

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    if ((i == 0) || (i == 5) || (i == 8) || (i == 7))
                    {
                        continue;
                    }
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;

                }
            }

        }
    }

    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = "";
        this.GridViewExt1.HeaderRow.Cells[1].Text = "";
        this.GridViewExt1.HeaderRow.Cells[2].Text = "";
        this.GridViewExt1.HeaderRow.Cells[3].Text = "PartType";
        this.GridViewExt1.HeaderRow.Cells[4].Text = "PartDescr";
        this.GridViewExt1.HeaderRow.Cells[5].Text = "PartNo";
        this.GridViewExt1.HeaderRow.Cells[6].Text = "Qty";
        this.GridViewExt1.HeaderRow.Cells[7].Text = "PQty";
        this.GridViewExt1.HeaderRow.Cells[8].Text = "CollectionData";
        this.GridViewExt1.HeaderRow.Cells[9].Text = "";
        this.GridViewExt1.HeaderRow.Cells[10].Text = "";
        this.GridViewExt1.HeaderRow.Cells[11].Text = "";
        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(0);
        this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(250);//Part Descr
        this.GridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(150);//Part No/Item Name
        this.GridViewExt1.HeaderRow.Cells[6].Width = Unit.Pixel(40);
        this.GridViewExt1.HeaderRow.Cells[7].Width = Unit.Pixel(40);
        this.GridViewExt1.HeaderRow.Cells[8].Width = Unit.Pixel(150);
        this.GridViewExt1.HeaderRow.Cells[9].Width = Unit.Pixel(0);
        this.GridViewExt1.HeaderRow.Cells[10].Width = Unit.Pixel(0);
        this.GridViewExt1.HeaderRow.Cells[11].Width = Unit.Pixel(0);
    }

    private DataTable getNullDataTable()
    {

        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Image", Type.GetType("System.String"));
        retTable.Columns.Add("SubstitutePartNo", Type.GetType("System.String"));
        retTable.Columns.Add("SubstituteDescr", Type.GetType("System.String"));
        retTable.Columns.Add("PartType", Type.GetType("System.String"));
        retTable.Columns.Add("PartDescr", Type.GetType("System.String"));
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        retTable.Columns.Add("PQty", Type.GetType("System.Int32"));
        retTable.Columns.Add("CollectionData", Type.GetType("System.String"));
        retTable.Columns.Add("HfCollectionData", Type.GetType("System.String"));
        retTable.Columns.Add("HfPartNo", Type.GetType("System.String"));
        retTable.Columns.Add("HfIndex", Type.GetType("System.String"));
        return retTable;
    }

    private void callInputRun()
    {

        String script = "<script language='javascript'> getAvailableData('processDataEntry'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callInputRun", script, false);
    }

    protected void FreshGrid(object sender, System.EventArgs e)
    {
        try
        {
            String pdLine = this.cmbPdLine.InnerDropDownList.SelectedValue;
            String prodId = this.prodHidden.Value;
            String firstInputCT = this.firstInputCT.Value;
            String subStation = this.station1.Value;// this.cmbsubStation.InnerDropDownList.SelectedValue;
            string cnt = "";
            //this.station.Value = subStation;
            if (prodId.Trim().Length == 10)
            { prodId = prodId.Trim().Substring(0, 9); }
            this.GridViewExt1.DataSource = getBomDataTable(pdLine, subStation, prodId, out cnt);
            this.GridViewExt1.DataBind();
            initTableColumnHeader();
            String script = "<script language='javascript'> setStatus(true); InputLCMCT('" + firstInputCT + "','" + cnt + "'); </script>";
            ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setStatus", script, false);
            endWaitingCoverDiv();
        }
        catch (FisException ee)
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            initTableColumnHeader();

            writeToAlertMessage(ee.mErrmsg);
            endWaitingCoverDiv();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            endWaitingCoverDiv();
        }
        //辦厒諷璃脹渾婬棒怀
        callInputRun();
    }

    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            initTableColumnHeader();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void setProdId(string value)
    {
        try
        {
            this.txtProdId.Text = value;
            this.UpdatePanel1.Update();
        }

        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private DataTable getBomDataTable(string pdLine, string subStation, string prodId, out string cnts)
    {
        DataTable dt = initTable();
        DataRow newRow;
        int sumCount = 0;
        int scanQtyHidden = 0;
        string realProdID;
        string strModel;
        int cnt = 0;
        cnts = "";
        IList<BomItemInfo> list = iCreateProductandCombineLCM.InputProdIdorCustsn(pdLine, prodId, Master.userInfo.UserId, subStation, Master.userInfo.Customer, out realProdID, out strModel);
        setProdId(realProdID);
        if (list.Count == 0)
        {
            String script = "<script language='javascript'> processDataEntry('getbomnull'); </script>";
            ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "processDataEntry", script, false);
        }

        if (list != null && list.Count != 0)
        {
            foreach (BomItemInfo temp in list)
            {
                string str = "";
                str = temp.PartNoItem;
                newRow = dt.NewRow();
                if (temp.parts.Count >= 1)
                {
                    for (int i = 0; i < temp.parts.Count; i++)
                    {
                        newRow["SubstitutePartNo"] += temp.parts[i].id;
                        newRow["SubstituteDescr"] += temp.parts[i].description;
                        if (i < temp.parts.Count - 1)
                        {
                            newRow["SubstitutePartNo"] += ";";
                            newRow["SubstituteDescr"] += "|";
                        }
                    }
                }
                else
                {
                    newRow["SubstitutePartNo"] = "";
                    newRow["SubstituteDescr"] = "";
                }
                if (str.Length > 20)
                {
                    newRow["PartNo"] = str.Substring(0, 20) + "...";
                }
                else
                {
                    newRow["PartNo"] = str;
                }
                newRow["HfPartNo"] = str;
                newRow["PartType"] = temp.type;
                newRow["PartDescr"] = temp.description;
                newRow["Qty"] = temp.qty.ToString();
                sumCount = sumCount + temp.qty;  //(temp.qty - temp.scannedQty);
                newRow["PQty"] = temp.scannedQty.ToString();
                string collectionData = String.Empty;
                string collecPn = String.Empty;
                if (temp.collectionData != null)
                {
                    for (int i = 0; i < temp.collectionData.Count; i++)
                    {
                        scanQtyHidden++;
                        collectionData += temp.collectionData[i].sn;
                        collecPn += temp.collectionData[i].pn;
                        if (i < temp.collectionData.Count - 1)
                        {
                            collectionData += ",";
                            collecPn += ",";
                        }
                    }
                }
                newRow["HfCollectionData"] = collectionData;
                if (collectionData.Length > 20)
                {
                    newRow["CollectionData"] = collectionData.Substring(0, 20) + "...";
                }
                else
                {
                    newRow["CollectionData"] = collectionData;
                }
                newRow["HfIndex"] = cnt;
                dt.Rows.Add(newRow);
                cnt++;
            }
            cnts = cnt.ToString();
            if (list.Count < initRowsCount)
            {
                for (int i = list.Count; i < initRowsCount; i++)
                {
                    newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                }
            }
        }
        else
        {
            for (int i = 0; i < initRowsCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
        }
        return dt;
    }

}
