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

public partial class FA_FACombineKeyParts : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICombineKeyParts iCombineKeyParts = ServiceAgent.getInstance().GetObjectByName<ICombineKeyParts>(WebConstant.CombineKeyPartsObject);

    public int initRowsCount = 6;
    public string userId;
    public string customer;
    public String Customer;
    public String AccountId;
    public String Login;
    public String UserName;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            cmbsubStation.IsFromStation = true;

            cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbsubStation_Selected);
            cmbsubStation.InnerDropDownList.AutoPostBack = true;
            userId = Master.userInfo.UserId;
            customer = Master.userInfo.Customer;

            if (!Page.IsPostBack)
            {
                InitLabel();
                this.GridViewExt1.DataSource = getNullDataTable();
                this.GridViewExt1.DataBind();
                initTableColumnHeader();

                //this.cmbPdLine.Station = "37";//for Test
                //this.cmbPdLine.Station = Request["Station"];
                if (string.IsNullOrEmpty(Request["Stage"]))
                {
                    this.cmbPdLine.Stage = "FA";
                }
                else
                {
                    this.cmbPdLine.Stage = Request["Stage"];
                }
                this.cmbPdLine.Customer = Master.userInfo.Customer;

                //this.station.Value = Request["Station"];
                //this.station.Value = "37";//for Test
                this.pCode.Value = Request["PCode"];



                //this.cmbsubStation.Station = "FA";
                this.cmbsubStation.Type = "FACombine";
                this.cmbsubStation.Enabled = false;

                this.useridHidden.Value = Master.userInfo.UserId;

                AccountId = Master.userInfo.AccountId.ToString();
                Login = Master.userInfo.Login;
                UserName = Master.userInfo.UserName;



            }
            else
            {
                this.cmbsubStation.Enabled = true;
            }
            /* else
             {
                this.cmbsubStation.Type = "FACombine";
                this.cmbsubStation.Enabled = true;
               
                 cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbsubStation_Selected);
                 cmbsubStation.InnerDropDownList.AutoPostBack = true;


              }*/
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

    private void cmbsubStation_Selected(object sender, System.EventArgs e)
    {
        //string line = cmbsubStation.InnerDropDownList.SelectedValue;
        this.cmbsubStation.refreshByPdline(this.cmbPdLine.InnerDropDownList.SelectedValue, "FACombine");


    }

    /// <summary>
    /// 选择pdLIne或者testStation下拉框，会reset界面
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbTestStation_Selected(object sender, System.EventArgs e)
    {
        try
        {

            //刷新MO下拉框内容
            if (this.cmbsubStation.InnerDropDownList.SelectedValue == "")
            {
                //清空MO下拉框内容
                this.cmbPdLine.clearContent();

            }
            else
            {
                this.cmbPdLine.refresh(this.cmbsubStation.InnerDropDownList.SelectedValue, Master.userInfo.Customer);
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


    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lbpdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbsubStation.Text = this.GetLocalResourceObject(Pre + "_lblTestStation").ToString();
        //this.lbPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        // this.lbFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lbProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdId").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        //this.lbDefectList.Text = this.GetLocalResourceObject(Pre + "_lblDefectList").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        // this.lblDefectStation.Text = this.GetLocalResourceObject(Pre + "_lblDefectStation").ToString();
        this.lbCollection.Text = this.GetLocalResourceObject(Pre + "_lblCollectionData").ToString();
        this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrtSet").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
        setinitFocus();

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

    public class IcpBom : IComparer<BomItemInfo>
    {
        //按书名排序 
        public int Compare(BomItemInfo x, BomItemInfo y)
        {
            return x.parts[0].iecPartNo.CompareTo(y.parts[0].iecPartNo);
        }
    }

    /// <summary>
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = "";
        this.GridViewExt1.HeaderRow.Cells[1].Text = "";
        this.GridViewExt1.HeaderRow.Cells[2].Text = "";
        this.GridViewExt1.HeaderRow.Cells[3].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColPartType");
        this.GridViewExt1.HeaderRow.Cells[4].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColPartDescr");
        this.GridViewExt1.HeaderRow.Cells[5].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColPartNo");
        //this.GridViewExt1.HeaderRow.Cells[6].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColValueType");

        this.GridViewExt1.HeaderRow.Cells[6].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColQty");
        this.GridViewExt1.HeaderRow.Cells[7].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColPQty");
        this.GridViewExt1.HeaderRow.Cells[8].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColCollectionData");
        this.GridViewExt1.HeaderRow.Cells[9].Text = "";
        this.GridViewExt1.HeaderRow.Cells[10].Text = "";
        this.GridViewExt1.HeaderRow.Cells[11].Text = "";
        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(0);
        this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(250);//Part Descr
        this.GridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(150);//Part No/Item Name
        //this.GridViewExt1.HeaderRow.Cells[6].Width = Unit.Pixel(80);
        this.GridViewExt1.HeaderRow.Cells[6].Width = Unit.Pixel(40);
        this.GridViewExt1.HeaderRow.Cells[7].Width = Unit.Pixel(40);
        this.GridViewExt1.HeaderRow.Cells[8].Width = Unit.Pixel(150);
        this.GridViewExt1.HeaderRow.Cells[9].Width = Unit.Pixel(0);
        this.GridViewExt1.HeaderRow.Cells[10].Width = Unit.Pixel(0);
        this.GridViewExt1.HeaderRow.Cells[11].Width = Unit.Pixel(0);

    }



    /// <summary>
    /// 输出错误信息
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


    /// <summary>
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
            /*
            if (!String.IsNullOrEmpty(e.Row.Cells[1].Text) && !(e.Row.Cells[1].Text.Trim().ToLower() == "&nbsp;"))
            {

                e.Row.Cells[0].Text = "<img src=\"../Images/see.png\" onclick=\"showSubstitute('" + e.Row.Cells[3].Text.Trim() + "','" + e.Row.Cells[1].Text.Trim() + "','" + e.Row.Cells[2].Text.Trim() + "')\"  style=\"cursor:hand;\"/>";

            }*/
            e.Row.Cells[5].ToolTip = e.Row.Cells[10].Text;
            e.Row.Cells[8].ToolTip = e.Row.Cells[9].Text;

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                //if ((i == 0) || (i == 1) || (i == 2) || (i == 9) || (i == 10))
                //{
                //    continue;
                //}
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

    /// <summary>
    ///置焦点
    /// </summary>  
    private void setinitFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (setPdLineCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
   
      
    }

    /// <summary>
    ///置焦点
    /// </summary>  
    private void setFocus()
    {/*
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (setPdLineCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);

        */
    }

    protected void FreshGrid(object sender, System.EventArgs e)
    {
        try
        {
            //   beginWaitingCoverDiv();
            String pdLine = this.cmbPdLine.InnerDropDownList.SelectedValue;
            String prodId = this.prodHidden.Value;

            String subStation = this.cmbsubStation.InnerDropDownList.SelectedValue;
            this.station.Value = subStation;
            if (prodId.Trim().Length == 10)
            { prodId = prodId.Trim().Substring(0, 9); }
            this.GridViewExt1.DataSource = getBomDataTable(pdLine, subStation, prodId);
            this.GridViewExt1.DataBind();
            initTableColumnHeader();
            // setProdId(prodId);

            String script = "<script language='javascript'> setStatus(true); </script>";
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
        //置快速控件焦点
        setFocus();
        //快速控件等待再次输入
        callInputRun();
    }

    private DataTable getBomDataTable(string pdLine, string subStation, string prodId)
    {
        DataTable dt = initTable();
        DataRow newRow;
        int sumCount = 0;
        int scanQtyHidden = 0;
        string realProdID;
        string strModel;
        int cnt = 0;
        //  string realCustsn;
        //need to add some codes .......... , 2011-10-24
        IList<BomItemInfo> list = iCombineKeyParts.InputProdIdorCustsn(pdLine, prodId, Master.userInfo.UserId, subStation, Master.userInfo.Customer, out realProdID, out strModel);
        setProdId(realProdID);
        setModel(strModel);
        //     custsnHidden.Value = realCustsn;
        //List<BomItemInfo> list = new List<BomItemInfo>(input);
        if (list.Count == 0)
        {
            String script = "<script language='javascript'> processDataEntry('getbomnull'); </script>";
            ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "processDataEntry", script, false);

        }

        if (list != null && list.Count != 0)
        {

            //list.Sort(new IcpBom());

            foreach (BomItemInfo temp in list)
            {
                
                string str = "";
                /*
                foreach (PartNoInfo pni in temp.parts)
                {
                    foreach (NameValueInfo nvi in pni.properties)
                    {
                        if (nvi.Name == "VendorCode")
                        {
                            str += nvi.Value + ",";
                        }
                    }
                }
                */
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
                //string strtmp = "";
               // strtmp += str.EndsWith(",") ? str.Remove(str.Length - 1) : str;
                if (str.Length > 20)
                {
                    newRow["PartNo"] = str.Substring(0, 20) + "...";
                }
                else
                {
                    newRow["PartNo"] = str;
                }
                //newRow["SubstitutePartNo"] += str.EndsWith(",") ? str.Remove(str.Length - 1) : str;
                newRow["HfPartNo"] = str;
                newRow["PartType"] = temp.type;
                newRow["PartDescr"] = temp.description;
                //newRow["ValueType"] = temp.parts[0].valueType;
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
                //newRow["HfPartNo"] = collecPn;
                dt.Rows.Add(newRow);
                cnt++;


            }
            hidRowCnt.Value = cnt.ToString();
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
        setFocus();
        return dt;


    }

    private void callInputRun()
    {

        String script = "<script language='javascript'> getAvailableData('processDataEntry'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callInputRun", script, false);
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

    private void setModel(string value)
    {
        try
        {
            this.txtModel.Text = value;
            this.UpdatePanel4.Update();
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
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }


}
