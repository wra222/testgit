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


public partial class PAK_PackingListForPL : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string filename = "";
    IPackingList iPackingList = ServiceAgent.getInstance().GetObjectByName<IPackingList>(WebConstant.PackingListObject);
    private int fullRowCount = 14;
    string UserId;
    string Customer;
    string Station;
    IShipToCartonLabel iShipToCartonLabel =
                    ServiceAgent.getInstance().GetObjectByName<IShipToCartonLabel>(WebConstant.ShipToCartonLabelObject);

    protected void Page_Load(object sender, EventArgs e)
    {
		if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
        }
        if (!Page.IsPostBack)
        {
            Station = Request["Station"];
            //pcode = Request["PCode"];


            this.gridview.DataSource = getNullDataTable(fullRowCount);
            this.gridview.DataBind();
            initTableColumnHeader();
            InitLabel();
			this.cmbDocType.ServiceWithout = "Summary";
            this.cmbDocType.Service = "PackingListForPL";
            this.cmbDocType.setSelected(1);

            //UserId = Master.userInfo.UserId;
            //Customer = Master.userInfo.Customer;
            this.hidStation.Value = Station;
            this.hidEditor.Value = UserId;
            this.hidCustomer.Value = Customer;
        }

        IList<string> imagepath = iShipToCartonLabel.GetEditAddr("OAEditsImage");
        if (imagepath != null && imagepath.Count > 0)
        {
            this.addr.Value = imagepath[0].ToString();
            //this.addr.Value.ToString().Replace('\', );
        }
        else
        {
            this.addr.Value = "";
        }

        //XML
        IList<string> xmlpath = iShipToCartonLabel.GetEditAddr("OAEditsXML");
        if (xmlpath != null && xmlpath.Count > 0)
        {
            this.editsxml.Value = xmlpath[0].ToString();
        }
        else
        {
            this.editsxml.Value = "";
        }

        //Template
        IList<string> temppath = iShipToCartonLabel.GetEditAddr("OAEditsTemplate");
        if (temppath != null && temppath.Count > 0)
        {
            this.editstemp.Value = temppath[0].ToString();
        }
        else
        {
            this.editstemp.Value = "";
        }

        //pdf
        IList<string> pdfpath = iShipToCartonLabel.GetEditAddr("OAEditsPDF");
        if (pdfpath != null && pdfpath.Count > 0)
        {
            this.editspdf.Value = pdfpath[0].ToString();
        }
        else
        {
            this.editspdf.Value = "";
        }
        //editspath
        IList<string> path3 = iShipToCartonLabel.GetEditAddr("OAEditsURL");
        if (path3 != null && path3.Count > 0)
        {
            this.editspath.Value = path3[0].ToString();
        }
        else
        {
            this.editspath.Value = "";
        }
        //fop path
        IList<string> path1 = iShipToCartonLabel.GetEditAddr("FOPFullFileName");
        if (path1 != null && path1.Count > 0)
        {
            this.foppath.Value = path1[0].ToString();
        }
        else
        {
            this.foppath.Value = "";
        }
        //exe path
        IList<string> path2 = iShipToCartonLabel.GetEditAddr("PDFPrintPath");
        if (path2 != null && path2.Count > 0)
        {
            this.exepath.Value = path2[0].ToString();
        }
        else
        {
            this.exepath.Value = "";
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
    
    public void serclear(object sender, System.EventArgs e)
    {
        
        updatePanel1.Update();
        endWaitingCoverDiv();
    }

    
    private DataTable getDataTable(int rowCount, IList<string> list)
    {
        DataTable dt = initTable();
        DataRow newRow;
        if (list != null && list.Count != 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                newRow = dt.NewRow();
                newRow["Item"] = list[i].ToString();
                dt.Rows.Add(newRow);
            }

            if (list.Count < fullRowCount)
            {
                for (int i = list.Count; i < fullRowCount; i++)
                {
                    newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                }
            }
        }
        else
        {
            for (int i = 0; i < fullRowCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
        }

        return dt;
    }



    private DataTable getNullDataTable(int rowCount)
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < rowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["Item"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Item", Type.GetType("System.String"));
        return retTable;
    }

    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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

    private void initTableColumnHeader()
    {

        this.gridview.HeaderRow.Cells[0].Text = "Item";
        //this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        //this.gridview.Columns[0].ItemStyle.Width = Unit.Pixel(80);
        //this.gridview.Columns[0].ItemStyle.Width = Unit.Pixel(40);
    }

    private void InitLabel()
    {
        this.lbDocType.Text = this.GetLocalResourceObject(Pre + "_lblDocType").ToString();
        this.lbCustomer.Text = this.GetLocalResourceObject(Pre + "_lblCustomer").ToString();
        this.lbDNList.Text = this.GetLocalResourceObject(Pre + "_listCustomerSN").ToString();
        this.btnClear.Value = this.GetLocalResourceObject(Pre + "_btnClear").ToString();
        
    }
}