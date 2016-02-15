/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PACosmetic
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-20   zhu lei            Create 
 * 
 * Known issues:Any restrictions about this file 
 */
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
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.DataModel;

using System.Web.Services;
using IMES.Station.Interface.CommonIntf;

public partial class DefectComponentPrint : System.Web.UI.Page
{
    public string[] GvQueryColumnName = { "No","ReturnType","Family","IECPn","PartType",
                                            "Vendor","NPQty","Defect"};
    public int[] GvQueryColumnNameWidth = { 30,70,70,70,70,
                                             70, 70,150};
    public string[] Gv2QueryColumnName = { "Vendor","Family","PartNo","PartType","IECPn",
                                             "PartSN","Defect"};
    public int[] Gv2QueryColumnNameWidth = { 70,70,70,70,70,
                                               70,150};

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    private const int DEFAULT_ROWS = 6;
    private const int COL_NUM = 8;
    private const int COL_NUM2 = 7;
    public int count = 0;

    //IDismantlePilotRun iDismantlePilotRun = ServiceAgent.getInstance().GetObjectByName<IDismantlePilotRun>(WebConstant.DismantlePilotRun);
    IDefectComponentPrint iDefectComponentPrint = ServiceAgent.getInstance().GetObjectByName<IDefectComponentPrint>(WebConstant.DefectComponentPrintObject);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
                initLabel();
                //SetStage();
                initcmbVendor();
                initcmbReturnLine();
                bindTable(null);
                bindTable2(null);
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

    private void initLabel()
    {
      //  this.lblPdline.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
 
    }

    private void initcmbVendor()
    {
        ListItem item = null;
        IList<string> vendorList = iDefectComponentPrint.GetVendorList();
        this.cmbVendor.Items.Clear();
        foreach (string temp in vendorList)
        {
            if (temp != "")
            {
                item = new ListItem(temp, temp);
                this.cmbVendor.Items.Add(item);
            }
        }
    }

    private void initcmbReturnLine()
    {
        ListItem item = null;
        IList<string> returnList = iDefectComponentPrint.GetReturnLineList();
        this.cmbReturnLine.Items.Clear();
        this.cmbReturnLine.Items.Add(string.Empty);
        foreach (string temp in returnList)
        {
            item = new ListItem(temp, temp);
            this.cmbReturnLine.Items.Add(item);
        }
    }

    protected void Query_ServerClick(Object sender, EventArgs e)
    {
        string vendor = this.cmbVendor.SelectedValue.Trim();
        try
        {
            IDefectComponentPrint iDefectComponentPrint = ServiceAgent.getInstance().GetObjectByName<IDefectComponentPrint>(WebConstant.DefectComponentPrintObject);
            ArrayList arr = iDefectComponentPrint.GetDefectComponentInfo(vendor, Customer, "", UserId);
            this.hidguid.Value = arr[0].ToString();
            IList<DefectComponentPrintGV1> list = (IList<DefectComponentPrintGV1>)arr[1];
            bindTable(list);
            this.txtTotalQty.Text = arr[2].ToString();
            ShowInfo();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null);
        }
    }
    protected void btnQuerygd2_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            IDefectComponentPrint iDefectComponentPrint = ServiceAgent.getInstance().GetObjectByName<IDefectComponentPrint>(WebConstant.DefectComponentPrintObject);
            string defectCode = this.hiddefect.Value.Split('-')[0].Trim();
            ArrayList arr = iDefectComponentPrint.GetDefectComponentDetailInfo(this.hidguid.Value, this.hidvendor.Value, this.hidfamily.Value, this.hidiecpn.Value, defectCode);
            IList<DefectComponentPrintGV2> list = (IList<DefectComponentPrintGV2>)arr[0];
            bindTable2(list);

        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null);
        }
    }

    private void bindTable(IList<DefectComponentPrintGV1> list)
    {
        DataTable dt = initTable();
        DataRow dr = null;
        if (list != null && list.Count != 0)
        {
            foreach (DefectComponentPrintGV1 temp in list)
            {
                dr = dt.NewRow();
                dr[1] = temp.ReturnType;
                dr[2] = temp.Family;
                dr[3] = temp.IECPn;
                dr[4] = temp.PartType;
                dr[5] = temp.Vendor;
                dr[6] = temp.NPQty;
                dr[7] = temp.DefectCode + "-" + temp.DefectDesc;
                
                dt.Rows.Add(dr);
            }

            for (int i = dt.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }
        else
        {
            dt = getNullDataTable(DEFAULT_ROWS);
        }
        gd.DataSource = dt;
        gd.DataBind();
        //InitGridView();
        this.UpdatePanel1.Update();
    }
    private void bindTable2(IList<DefectComponentPrintGV2> list)
    {
        DataTable dt = initTable2();
        DataRow dr = null;
        if (list != null && list.Count != 0)
        {
            foreach (DefectComponentPrintGV2 temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.Vendor;
                dr[1] = temp.Family;
                dr[2] = temp.PartNo;
                dr[3] = temp.PartType;
                dr[4] = temp.IECPn;
                dr[5] = temp.PartSN;
                dr[6] = temp.Defect;
                dt.Rows.Add(dr);
            }

            for (int i = dt.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }
        else
        {
            dt = getNullDataTable2(DEFAULT_ROWS);
        }
        gd2.DataSource = dt;
        gd2.DataBind();
        //InitGridView();
        this.UpdatePanel2.Update();
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            for (int k = 0; k < COL_NUM; k++)
            {
                newRow[k] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable getNullDataTable2(int j)
    {
        DataTable dt = initTable2();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            for (int k = 0; k < COL_NUM2; k++)
            {
                newRow[k] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        for (int i = 0; i < GvQueryColumnName.Count(); i++)
        {
            retTable.Columns.Add(GvQueryColumnName[i], System.Type.GetType("System.String"));
        }
        return retTable;
    }
    private DataTable initTable2()
    {
        DataTable retTable = new DataTable();
        for (int i = 0; i < Gv2QueryColumnName.Count(); i++)
        {
            retTable.Columns.Add(Gv2QueryColumnName[i], System.Type.GetType("System.String"));
        }
        return retTable;
    }

    private void showErrorMessage(string errorMsg)
    {
     
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private void ShowInfo()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text.Trim() != "&nbsp;")
            {
                count += 1;
                e.Row.Cells[0].Text = count.ToString();
            }
        }
    }
    protected void gd2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM2; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }

    [System.Web.Services.WebMethod]
    public static ArrayList GetDefectComponentInfo(string sn, string customer, string station, string user)
    {
        try
        {
            IDefectComponentPrint iDefectComponentPrint = ServiceAgent.getInstance().GetObjectByName<IDefectComponentPrint>(WebConstant.DefectComponentPrintObject);
            ArrayList arr = iDefectComponentPrint.GetDefectComponentInfo(sn, customer, station, user);
            return arr;
        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    [System.Web.Services.WebMethod]
    public static ArrayList Save(string guid, string returnLine, int totalQty, List<PrintItem> printItems)
    {
        try
        {
            IDefectComponentPrint iDefectComponentPrint = ServiceAgent.getInstance().GetObjectByName<IDefectComponentPrint>(WebConstant.DefectComponentPrintObject);
            ArrayList arr = iDefectComponentPrint.Save(guid, returnLine, totalQty, printItems);
            return arr;
        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    [System.Web.Services.WebMethod]
    public static void Cancel(string guid)
    {
        try
        {
            IDefectComponentPrint iDefectComponentPrint = ServiceAgent.getInstance().GetObjectByName<IDefectComponentPrint>(WebConstant.DefectComponentPrintObject);
            iDefectComponentPrint.Cancel(guid);
        }
        catch (FisException e)
        {
            throw new Exception(e.mErrmsg);
        }
            catch (Exception e)
        {
            throw e;
        }
    }
}
