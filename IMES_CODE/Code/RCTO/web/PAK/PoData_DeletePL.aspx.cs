/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PoData(Delete for PL user) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PO Data.docx –2011/11/08 
 * UC:CI-MES12-SPEC-PAK-UC PO Data.docx –2011/11/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-09  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
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
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;



public partial class PAK_PoData_DeletePL : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    IPoData iPoData = ServiceAgent.getInstance().GetObjectByName<IPoData>(WebConstant.PoDataObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnQueryData.ServerClick += new EventHandler(btnQueryData_ServerClick);
            btnShowDetail.ServerClick += new EventHandler(btnShowDetail_ServerClick);
            btnDeleteDN.ServerClick += new EventHandler(btnDeleteDN_Click);
            btnDeleteShip.ServerClick += new EventHandler(btnDeleteShip_Click);

            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {        
                InitLabel();
                clearGrid();
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

    private void resetPage()
    {
        try
        {
            hidDateFrom.Value = "";
            hidDateTo.Value = "";
            hidSelectedDN.Value = "";
            hidSelectedShipment.Value = "";
            upHidden.Update();
            clearGrid();
            String script = "<script language='javascript'>\n"
                + "document.getElementById(\"" + txtShipment.ClientID + "\").value = \"\";\n"
                + "document.getElementById(\"" + txtDN.ClientID + "\").value = \"\";\n"
                + "document.getElementById(\"" + txtPO.ClientID + "\").value = \"\";\n"
                + "document.getElementById(\"" + txtModel.ClientID + "\").value = \"\";\n"
                + "document.getElementById(\"txtDateFrom\").value = \"\";\n"
                + "document.getElementById(\"txtDateTo\").value = \"\";\n"
                + "</script>";
            ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "clearConditions", script, false);
            showInfo("");
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            txtShipment.Focus();
        }

    }

    private void btnDeleteDN_Click(object sender, System.EventArgs e)
    {
        /*
         * Answer to: ITC-1360-1351
         */
        try
        {
            int res = iPoData.DeletePLDN(hidSelectedDN.Value, UserId);
            switch (res)
            {
                case -1:
                    jsAlert(GetLocalResourceObject(Pre + "_msgBoundDN").ToString());
                    break;
                case -2:
                    jsAlert(GetLocalResourceObject(Pre + "_msgSAPDN").ToString());
                    break;
                default:
                    jsAlert(GetLocalResourceObject(Pre + "_msgDeleteSuccess").ToString());
                    break;
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
        finally
        {
            reQuery();
            endWaitingCoverDiv();
        }
    }

    private void btnDeleteShip_Click(object sender, System.EventArgs e)
    {
        bool bSuccess = false;
        try
        {
            int res = iPoData.DeleteShipment(hidSelectedShipment.Value, UserId);
            if (res == -1)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgBoundDN").ToString());
            }
            else
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgDeleteSuccess").ToString());
                bSuccess = true;
                /*
                 * Answer to: ITC-1360-1354
                 * Description: Reset page after delete shipment.
                 */
                resetPage();
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
        finally
        {
            if (!bSuccess) reQuery();
            endWaitingCoverDiv();
        }
    }

    private void btnQueryData_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            /*
             * Answer to: ITC-1360-1137
             * Description: Clear grid before query.
             */
            clearGrid();

            string ship = txtShipment.Text.Trim();
            string dn = txtDN.Text.Trim();
            string pn = txtPO.Text.Trim();
            string model = txtModel.Text.Trim();
            string from = hidDateFrom.Value;
            string to = hidDateTo.Value;
            if ((ship == null || ship == "")
                && (dn == null || dn == "")
                && (pn == null || pn == "")
                && (model == null || model == "")
                && (from == null || from == "")
                && (to == null || to == ""))
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgNoInput").ToString());
                SetFocus(txtShipment.ClientID);
                return;
            }

            if ((from == null || from == "") && (to != null && to != ""))
            {
                from = to;
            }

            if ((from != null && from != "") && (to == null || to == ""))
            {
                to = from;
            }

            DNQueryCondition cond = new DNQueryCondition();
            cond.ShipmentNo = ship.ToUpper();
            cond.DeliveryNo = dn.ToUpper();
            cond.PONo = pn.ToUpper();
            cond.Model = model.ToUpper();
            if ((from != null && from != "") && (to != null && to != ""))
            {
                DateTime t1 = Convert.ToDateTime(from);// new DateTime(int.Parse(from.Substring(0, 4)), int.Parse(from.Substring(5, 2)), int.Parse(from.Substring(8, 2)));
                DateTime t2 = Convert.ToDateTime(to);// new DateTime(int.Parse(to.Substring(0, 4)), int.Parse(to.Substring(5, 2)), int.Parse(to.Substring(8, 2)));
                if (t1 > t2)
                {
                    jsAlert(GetLocalResourceObject(Pre + "_msgBadDate").ToString());
                    clickDateFrom();
                    return;
                }
                cond.ShipDateFrom = t1;
                cond.ShipDateTo = t2;
            }

            int recordCount = 0;

            IList<DNForUI> dnList = iPoData.QueryData("PL", cond, out recordCount);

            if (dnList.Count == 0)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgNoRecord").ToString());
                return;
            }

            if (recordCount > 1000)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgTooManyRecords").ToString());
            }

            DataTable dt = initTable1();
            DataRow newRow;
            int cnt = 0;
            if (dnList != null)
            {
                foreach (DNForUI ele in dnList)
                {
                    newRow = dt.NewRow();
                    newRow["DN"] += ele.DeliveryNo;
                    newRow["ShipNo"] += ele.ShipmentID;
                    newRow["PoNo"] += ele.PoNo;
                    newRow["Model"] += ele.ModelName;
                    newRow["ShipDate"] += ele.ShipDate.ToString("yyyy-MM-dd");
                    newRow["Qty"] += ele.Qty.ToString();
                    newRow["Status"] += ele.Status;
                    newRow["CDate"] += ele.Cdt.ToString("yyyy-MM-dd");
                    dt.Rows.Add(newRow);
                    cnt++;
                }
            }

            for (; cnt < initRowsCount; cnt++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            this.gve1.DataSource = dt;
            this.gve1.DataBind();
            initTableColumnHeader1();
            up1.Update();
        }
        catch (FisException ee)
        {
            clearGrid();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            /*
             * Answer to: ITC-1360-1343
             * Description: Set focus after query.
             */
            txtShipment.Focus();
            endWaitingCoverDiv();
        }
    }

    private void btnShowDetail_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            clearGrid2();
            clearGrid3();
            string dn = hidSelectedDN.Value;
            string ship = hidSelectedShipment.Value;
            if (dn == "" || ship == "")
            {
                return;
            }
            IList<DNInfoForUI> infoList = iPoData.GetDNInfoList("PL", dn);

            DataTable dt = initTable2();
            DataRow newRow;
            int cnt = 0;
            if (infoList != null)
            {
                foreach (DNInfoForUI ele in infoList)
                {
                    newRow = dt.NewRow();
                    newRow["AttrName"] += ele.InfoType;
                    newRow["AttrValue"] += ele.InfoValue;
                    dt.Rows.Add(newRow);
                    cnt++;
                }
            }

            for (; cnt < initRowsCount; cnt++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            this.gve2.DataSource = dt;
            this.gve2.DataBind();
            initTableColumnHeader2();
            up2.Update();

            IList<PalletCapacityInfo> palletList = iPoData.GetDNPalletCapacityList("PL", ship);

            dt = initTable3();
            cnt = 0;
            if (palletList != null)
            {
                foreach (PalletCapacityInfo ele in palletList)
                {
                    newRow = dt.NewRow();
                    newRow["PNo"] += ele.PalletNo;
                    newRow["TotalQty"] += ele.TotalQty.ToString();
                    newRow["OKQty"] += ele.OKQty.ToString();
                    newRow["DiffQty"] += ele.DiffQty.ToString();
                    dt.Rows.Add(newRow);
                    cnt++;
                }
            }

            for (; cnt < initRowsCount; cnt++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            this.gve3.DataSource = dt;
            this.gve3.DataBind();
            initTableColumnHeader3();
            up3.Update();
        }
        catch (FisException ee)
        {
            clearGrid2();
            clearGrid3();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    private void InitLabel()
    {
        lblShipment.Text = GetLocalResourceObject(Pre + "_lblShipment").ToString();
        lblDN.Text = GetLocalResourceObject(Pre + "_lblDN").ToString();
        lblFrom.Text = GetLocalResourceObject(Pre + "_lblFrom").ToString();
        lblTo.Text = GetLocalResourceObject(Pre + "_lblTo").ToString();
        lblPO.Text = GetLocalResourceObject(Pre + "_lblPO").ToString();
        lblModel.Text = GetLocalResourceObject(Pre + "_lblModel").ToString();

        lblDNList.Text = GetLocalResourceObject(Pre + "_lblDNList").ToString();
        lblDNInfoList.Text = GetLocalResourceObject(Pre + "_lblDNInfoList").ToString();
        lblPalletList.Text = GetLocalResourceObject(Pre + "_lblPalletList").ToString();
    }

    /*
     * Answer to: ITC-1360-0874
     * Description: Change ["] to [\"] to avoid javascript error.
     */
    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private DataTable getNullDataTable1()
    {
        DataTable dt = initTable1();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable2()
    {
        DataTable dt = initTable2();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable3()
    {
        DataTable dt = initTable3();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable1()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("DN", Type.GetType("System.String"));
        retTable.Columns.Add("ShipNo", Type.GetType("System.String"));
        retTable.Columns.Add("PoNo", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("ShipDate", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("CDate", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable2()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("AttrName", Type.GetType("System.String"));
        retTable.Columns.Add("AttrValue", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable3()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("PNo", Type.GetType("System.String"));
        retTable.Columns.Add("TotalQty", Type.GetType("System.Int32"));
        retTable.Columns.Add("OKQty", Type.GetType("System.Int32"));
        retTable.Columns.Add("DiffQty", Type.GetType("System.Int32"));
        return retTable;
    }

    private void initTableColumnHeader1()
    {
        gve1.HeaderRow.Cells[0].Text = GetLocalResourceObject(Pre + "_titleDN").ToString();
        gve1.HeaderRow.Cells[1].Text = GetLocalResourceObject(Pre + "_titleShipNo").ToString();
        gve1.HeaderRow.Cells[2].Text = GetLocalResourceObject(Pre + "_titlePoNo").ToString();
        gve1.HeaderRow.Cells[3].Text = GetLocalResourceObject(Pre + "_titleModel").ToString();
        gve1.HeaderRow.Cells[4].Text = GetLocalResourceObject(Pre + "_titleSDate").ToString();
        gve1.HeaderRow.Cells[5].Text = GetLocalResourceObject(Pre + "_titleQty").ToString();
        gve1.HeaderRow.Cells[6].Text = GetLocalResourceObject(Pre + "_titleStatus").ToString();
        gve1.HeaderRow.Cells[7].Text = GetLocalResourceObject(Pre + "_titleCDate").ToString();
        gve1.HeaderRow.Cells[0].Width = Unit.Pixel(80);
        gve1.HeaderRow.Cells[1].Width = Unit.Pixel(90);
        gve1.HeaderRow.Cells[2].Width = Unit.Pixel(90);
        gve1.HeaderRow.Cells[3].Width = Unit.Pixel(80);
        gve1.HeaderRow.Cells[4].Width = Unit.Pixel(50);
        gve1.HeaderRow.Cells[5].Width = Unit.Pixel(30);
        gve1.HeaderRow.Cells[6].Width = Unit.Pixel(30);
        gve1.HeaderRow.Cells[7].Width = Unit.Pixel(50);
    }

    private void initTableColumnHeader2()
    {
        gve2.HeaderRow.Cells[0].Text = GetLocalResourceObject(Pre + "_titleAttrName").ToString();
        gve2.HeaderRow.Cells[1].Text = GetLocalResourceObject(Pre + "_titleAttrValue").ToString();
        gve2.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        gve2.HeaderRow.Cells[1].Width = Unit.Pixel(100);
    }

    private void initTableColumnHeader3()
    {
        gve3.HeaderRow.Cells[0].Text = GetLocalResourceObject(Pre + "_titlePNo").ToString();
        gve3.HeaderRow.Cells[1].Text = GetLocalResourceObject(Pre + "_titleTotalQty").ToString();
        gve3.HeaderRow.Cells[2].Text = GetLocalResourceObject(Pre + "_titleOKQty").ToString();
        gve3.HeaderRow.Cells[3].Text = GetLocalResourceObject(Pre + "_titleDiffQty").ToString();
        gve3.HeaderRow.Cells[0].Width = Unit.Pixel(80);
        gve3.HeaderRow.Cells[1].Width = Unit.Pixel(40);
        gve3.HeaderRow.Cells[2].Width = Unit.Pixel(40);
        gve3.HeaderRow.Cells[3].Width = Unit.Pixel(40);
    }

    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            clearGrid();
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

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void clickDateFrom()
    {
        String script = "<script language='javascript'>document.getElementById('btnFrom').click();</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "clickDateFrom", script, false);
    }

    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "jsAlert", script, false);
    }

    private void reQuery()
    {
        String script = "<script language='javascript'>\n"
                        + "document.getElementById('" + hidSelectedDN.ClientID + "').value = \"\";\n"
                        + "document.getElementById('" + hidSelectedShipment.ClientID + "').value = \"\";\n"
                        + "document.getElementById('" + btnQueryData.ClientID + "').click();\n"
                        + "</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "reQuery", script, false);
    }
   
    private void clearGrid()
    {
        try
        {
            clearGrid1();
            clearGrid2();
            clearGrid3();
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

    private void clearGrid1()
    {
        try
        {
            gve1.DataSource = getNullDataTable1();
            gve1.DataBind();
            up1.Update();
            initTableColumnHeader1();
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

    private void clearGrid2()
    {
        try
        {
            gve2.DataSource = getNullDataTable2();
            gve2.DataBind();
            up2.Update();
            initTableColumnHeader2();
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

    private void clearGrid3()
    {
        try
        {
            gve3.DataSource = getNullDataTable3();
            gve3.DataBind();
            up3.Update();
            initTableColumnHeader3();
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
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
}
