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
using IMES.Docking.Interface.DockingIntf;

public partial class CombinePack : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICombinePack iCombinePack = ServiceAgent.getInstance().GetObjectByName<ICombinePack>(WebConstant.CombinePackObject);

    public int initRowsCount = 6;

    public string userId;
    public string customer;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userId = Master.userInfo.UserId;
            customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {

                InitLabel();
                this.GridViewExt1.DataSource = getNullDataTable();
                this.GridViewExt1.DataBind();
                initTableColumnHeader();


                //this.station.Value = "8C";//TEST
                this.station.Value = Request["Station"];
                //this.pCode.Value = "DKCM014"; //test
                this.pCode.Value = Request["PCode"];


                this.useridHidden.Value = Master.userInfo.UserId;

                //       this.cmbPdLine.InnerDropDownList+= new EventHandler(cmbPdLine_Selected);
                //    GetQty();
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


    private void InitLabel()
    {

        this.lbProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdId").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblCollectionData.Text = this.GetLocalResourceObject(Pre + "_lblCollectionData").ToString();
        this.lbCustSN.Text = this.GetLocalResourceObject(Pre + "_lblCustSN").ToString();
        this.lblProductInfo.Text = this.GetLocalResourceObject(Pre + "_lblProductInfo").ToString();



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

    /// <summary>
    /// ¸ù¾ÝÄ¬ÈÏÐÐÊý£¬³õÊ¼»¯gridviewÄÚÈÝ
    /// </summary>
    /// <returns></returns>
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


    /// <summary>
    /// »ñÈ¡¼ÇÂ¼
    /// </summary>
    /// <param name="pdLine"></param>
    /// <param name="prodId"></param>
    /// <returns></returns>
    /// ITC-1360-0469
    /// ITC-1360-0450
    /// ITC-1360-0309
    private DataTable getBomDataTable(string prodId)
    {
        DataTable dt = initTable();
        DataRow newRow;
        int sumCount = 0;
        int scanQtyHidden = 0;
        string realProdID;
        string strModel;
        string strCustSN;
        bool IsPAQC=false;
        int cnt = 0;
        //  string realCustsn;

        IList<BomItemInfo> list = iCombinePack.InputProdIdorCustsn(prodId, Master.userInfo.UserId, this.station.Value, Master.userInfo.Customer, out realProdID, out strModel, out strCustSN, out IsPAQC);
        setProdId(realProdID);
        setModel(strModel);
        setCustSN(strCustSN);
        setPAQC(IsPAQC);
        //     custsnHidden.Value = realCustsn;
        //ArrayList list = new ArrayList(input);
        if (list.Count == 0)
        {
            String script = "<script language='javascript'> processDataEntry('getbomnull'); </script>";
            ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "processDataEntry", script, false);

        }

        if (list != null && list.Count != 0)
        {

            // list.Sort(new IcpBom());

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

                // string strtmp = "";
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

                newRow["PartType"] = temp.tp;
                newRow["PartDescr"] = temp.parts[0].description;
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
        this.scanQtyHidden.Value = scanQtyHidden.ToString();
        setScanHiddenQty();
        //for (int i = 0; i < initRowsCount; i++)
        //{
        //    newRow = dt.NewRow();
        //    if (i % 2 == 0)
        //    {
        //        newRow["SubstitutePartNo"] = "90A" + i + ";" + "90B" + i;
        //        newRow["SubstituteDescr"] = "00A" + i + ";" + "00B" + i;
        //    }
        //    else
        //    {
        //        newRow["SubstitutePartNo"] = "";
        //        newRow["SubstituteDescr"] = "";
        //    }
        //    newRow["PartNo"] = "1" + i;
        //    newRow["PartType"] = "PartType";
        //    newRow["PartDescr"] = "PartDescr";
        //    newRow["Qty"] = 1;
        //    sumCount = sumCount + 1;
        //    newRow["PQty"] = 0;
        //    newRow["CollectionData"] = String.Empty;
        //    newRow["HfCollectionData"] = String.Empty;
        //    newRow["HfPartNo"] = String.Empty;
        //    dt.Rows.Add(newRow);
        //}
        this.sumCountHidden.Value = sumCount.ToString();

        setFocus();
        return dt;


    }


    public class IcpBom : IComparer<BomItemInfo>
    {
        //°´ÊéÃûÅÅÐò 
        public int Compare(BomItemInfo x, BomItemInfo y)
        {
            return x.parts[0].iecPartNo.CompareTo(y.parts[0].iecPartNo);
        }
    }

    /// <summary>
    /// ³õÊ¼»¯ÁÐÀàÐÍ
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Image", Type.GetType("System.String"));
        retTable.Columns.Add("SubstitutePartNo", Type.GetType("System.String"));
        retTable.Columns.Add("SubstituteDescr", Type.GetType("System.String"));
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        retTable.Columns.Add("PartType", Type.GetType("System.String"));
        retTable.Columns.Add("PartDescr", Type.GetType("System.String"));
        //retTable.Columns.Add("ValueType", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        retTable.Columns.Add("PQty", Type.GetType("System.Int32"));
        retTable.Columns.Add("CollectionData", Type.GetType("System.String"));
        retTable.Columns.Add("HfCollectionData", Type.GetType("System.String"));
        retTable.Columns.Add("HfPartNo", Type.GetType("System.String"));
        return retTable;


    }

    /// <summary>
    /// ÉèÖÃ±í¸ñÁÐÃû³Æ¼°¿í¶È
    /// </summary>
    /// <returns></returns>
    /// ITC-1414-0066 BUG
    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = "";
        this.GridViewExt1.HeaderRow.Cells[1].Text = "";
        this.GridViewExt1.HeaderRow.Cells[2].Text = "";
        this.GridViewExt1.HeaderRow.Cells[3].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColPartNo");
        this.GridViewExt1.HeaderRow.Cells[4].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColPartType");
        this.GridViewExt1.HeaderRow.Cells[5].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColPartDescr");
        //this.GridViewExt1.HeaderRow.Cells[6].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColValueType");

        this.GridViewExt1.HeaderRow.Cells[6].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColQty");
        this.GridViewExt1.HeaderRow.Cells[7].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColPQty");
        this.GridViewExt1.HeaderRow.Cells[8].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblColCollectionData");
        this.GridViewExt1.HeaderRow.Cells[9].Text = "";
        this.GridViewExt1.HeaderRow.Cells[10].Text = "";
        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(0);
        this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(0);
        this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(150);//Part No/Item Name
        this.GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(250);//Part Descr
        //this.GridViewExt1.HeaderRow.Cells[6].Width = Unit.Pixel(80);
        this.GridViewExt1.HeaderRow.Cells[6].Width = Unit.Pixel(40);
        this.GridViewExt1.HeaderRow.Cells[7].Width = Unit.Pixel(40);
        this.GridViewExt1.HeaderRow.Cells[8].Width = Unit.Pixel(150);
        this.GridViewExt1.HeaderRow.Cells[9].Width = Unit.Pixel(0);
        this.GridViewExt1.HeaderRow.Cells[10].Width = Unit.Pixel(0);
    }

    /// <summary>
    /// °ó¶¨ÐÐÊý¾ÝÊ±£¬Òþ²Ø×îºóÒ»ÁÐ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Attributes.Add("style", "display:none;");
        e.Row.Cells[1].Attributes.Add("style", "display:none;");
        e.Row.Cells[2].Attributes.Add("style", "display:none;");
        e.Row.Cells[9].Attributes.Add("style", "display:none;");
        e.Row.Cells[10].Attributes.Add("style", "display:none;");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (!String.IsNullOrEmpty(e.Row.Cells[1].Text) && !(e.Row.Cells[1].Text.Trim().ToLower() == "&nbsp;"))
            {
                string tmp = e.Row.Cells[1].Text;
                if (tmp.IndexOf(";") != -1)
                {
                    e.Row.Cells[0].Text = "<img src=\"../Images/see.png\" onclick=\"showSubstitute('" + e.Row.Cells[3].Text.Trim() + "','" + e.Row.Cells[1].Text.Trim() + "','" + e.Row.Cells[2].Text.Trim() + "')\"  style=\"cursor:hand;\"/>";

                }
            }
            /*

                        if (!String.IsNullOrEmpty(e.Row.Cells[1].Text.Trim()) && !(e.Row.Cells[1].Text.Trim().ToLower() == "&nbsp;"))
                        {

                            e.Row.Cells[0].Text = "<img src=\"../Images/see.png\" onclick=\"showSubstitute('" + e.Row.Cells[3].Text.Trim() + "','" + e.Row.Cells[1].Text.Trim() + "','" + e.Row.Cells[2].Text.Trim()  + "')\"  style=\"cursor:hand;\"/>";

                        }*/

            e.Row.Cells[3].ToolTip = e.Row.Cells[10].Text;
            e.Row.Cells[8].ToolTip = e.Row.Cells[9].Text;

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if ((i == 0) || (i == 3) || (i == 8) || (i == 7))
                {
                    continue;
                }
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }

    /// <summary>
    /// Çå¿Õ±í¸ñ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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


    /// <summary>
    /// É¨ÈëProdIdºó£¬»ñÈ¡part BomÐÅÏ¢
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FreshGrid(object sender, System.EventArgs e)
    {
        try
        {
            //   beginWaitingCoverDiv();
            //CountPassedPCS();
            String prodId = this.prodHidden.Value;

            // if (prodId.Trim().Length == 10)
            // { prodId = prodId.Trim().Substring(0, 9); }
            this.GridViewExt1.DataSource = getBomDataTable(prodId);
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
        //ÖÃ¿ìËÙ¿Ø¼þ½¹µã
        setFocus();
        //¿ìËÙ¿Ø¼þµÈ´ýÔÙ´ÎÊäÈë
        callInputRun();
    }

    /// <summary>
    /// ¿ìËÙ¿Ø¼þµÈ´ýÔÙ´ÎÊäÈë
    /// </summary>
    private void callInputRun()
    {

        String script = "<script language='javascript'> getAvailableData('processDataEntry'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callInputRun", script, false);
    }

    /// <summary>
    /// ÖÃ¿ìËÙ¿Ø¼þ½¹µã
    /// </summary>
    private void setFocus()
    {

        String script = "<script language='javascript'>  getCommonInputObject().focus();; </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setFocus", script, false);
    }


    /// <summary>
    /// É¨ÈëProdId³É¹¦£¬ÏòLabel¿òÖÐ¸³Öµ
    /// </summary>
    private void setProdId(string value)
    {
        try
        {
            this.txtProdId.Text = value;
            this.UpdatePanel2.Update();
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
            this.UpdatePanel3.Update();
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

    private void setCustSN(string value)
    {
        try
        {
            this.txtCustSN.Text = value;
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

    private void setPAQC(bool value)
    {
        try
        {
            if (value)
            {
                this.txtPAQC.Text = "PAQC";
                this.UpdatePanel4.Update();
            }
            else
            {
                this.txtPAQC.Text = "";
                this.UpdatePanel4.Update();
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
    private void setScanHiddenQty()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "setScanHiddenQty();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setScanHiddenQty", script, false);
    }

}
