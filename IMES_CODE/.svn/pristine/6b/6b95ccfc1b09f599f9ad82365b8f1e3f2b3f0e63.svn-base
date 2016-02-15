/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: CombineCOAandDNReprint
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-2-7     207003           Create 
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
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using System.IO;
using IMES.DataModel;

public partial class PAK_COAReturn : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICOAReturn iCOAReturn = ServiceAgent.getInstance().GetObjectByName<ICOAReturn>(WebConstant.COAReturnObject);
    public String UserId;
    public String Customer;
    public String station;
    public String code;
    public string msgInvalidFileType;
    public string msgEmptyFile;
    public string msgFileEmpty;
    private int fullRowCount = 14;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnCancel.ServerClick += new EventHandler(btnCancel_ServerClick);
            msgInvalidFileType = this.GetLocalResourceObject(Pre + "_MsgInvalidFileType").ToString();
            msgEmptyFile = this.GetLocalResourceObject(Pre + "_msgEmptyFile").ToString();
            msgFileEmpty = this.GetLocalResourceObject(Pre + "_msgFileEmpty").ToString();
            if (null == station || "" == station)
            {
                station = Request["Station"];
            }
            if (!this.IsPostBack)
            {
                initLabel();
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                TableClear();
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
    private void initTableColumnHeaderV()
    {
        this.gridviewV.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titleSN").ToString();
        this.gridviewV.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleCOA").ToString();
    }
    private void initTableColumnHeaderE()
    {
        this.gridviewE.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titleSN").ToString();
        this.gridviewE.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleERROR").ToString();
    }
     private void TableClear()
    {
        this.gridviewV.DataSource = getNullDataTableV(fullRowCount);
        this.gridviewV.DataBind();
        this.gridviewE.DataSource = getNullDataTableE(fullRowCount);
        this.gridviewE.DataBind();
        initTableColumnHeaderV();
        initTableColumnHeaderE();
        this.updatePanelV.Update();
        this.updatePanelE.Update();
    }
    private DataTable getNullDataTableV(int rowCount)
    {
        DataTable dt = initTableV();
        DataRow newRow;
        for (int i = 0; i < rowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["SN"] = String.Empty;
            newRow["COA"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTableV()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("SN", Type.GetType("System.String"));
        retTable.Columns.Add("COA", Type.GetType("System.String"));

        return retTable;
    }
    private DataTable getNullDataTableE(int rowCount)
    {
        DataTable dt = initTableE();
        DataRow newRow;
        for (int i = 0; i < rowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["SN"] = String.Empty;
            newRow["ERROR"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTableE()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("SN", Type.GetType("System.String"));
        retTable.Columns.Add("ERROR", Type.GetType("System.String"));

        return retTable;
    }
    private void showErrorMessage(string errorMsg)
    {
        iCOAReturn.Cancel(hidFileName.Value);
        hidFileName.Value = "";
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelALL, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelALL, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void initLabel()
    {
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.lbVList.Text = this.GetLocalResourceObject(Pre + "_lbV").ToString();
        this.lbEList.Text = this.GetLocalResourceObject(Pre + "_lbE").ToString();
        this.lbCOA.Text = this.GetLocalResourceObject(Pre + "_lbCOA").ToString();
    }
    public void btnGetProductTable_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            TableClear();
            iCOAReturn.Cancel(hidFileName.Value);
            S_COAReturn retTable = new S_COAReturn();
            DataTable dt = ExcelManager.getExcelSheetData(hidFileName.Value);
            int i = 0;
            string SN = "";
            List<string> ret = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                if (i == 1)
                {
                    continue;
                }
                else
                {
                    SN = dr[0].ToString();
                    if (SN.Trim() == "")
                    {
                        continue;
                    }
                    else
                    {
                        ret.Add(SN);
                        if (ret.Count == 100)
                        {
                            if (i == dt.Rows.Count)
                            {
                                retTable = iCOAReturn.GetProductTable("", UserId, station, Customer, hidFileName.Value, ret, true);
                            }
                            else
                            {
                                iCOAReturn.GetProductTable("", UserId, station, Customer, hidFileName.Value, ret, false);
                            }
                            ret = new List<string>();
                        }
                    }
                }
            }
            if (ret.Count > 0)
            {
                retTable = iCOAReturn.GetProductTable("", UserId, station, Customer, hidFileName.Value, ret, true);
            }
            if (retTable.reValue == "true")
            {
                if (retTable.validProduct.Count > 0)
                {
                    DataTable dtV = initTableV();
                    DataRow newRow;

                    int cnt = 0;

                    foreach (S_RowData_COAReturn ele in retTable.validProduct)
                    {
                        newRow = dtV.NewRow();
                        newRow["SN"] += ele.SN;
                        newRow["COA"] += ele.COAorError;
                        dtV.Rows.Add(newRow);
                        cnt++;
                    }

                    for (; cnt < fullRowCount; cnt++)
                    {
                        newRow = dtV.NewRow();
                        dtV.Rows.Add(newRow);
                    }

                    this.gridviewV.DataSource = dtV;
                    this.gridviewV.DataBind();
                    updatePanelV.Update();
                    initTableColumnHeaderV();
                }
                if (retTable.inValidProduct.Count > 0)
                {
                    DataTable dtE = initTableE();
                    DataRow newRow;
                    int cnt = 0;
                    foreach (S_RowData_COAReturn ele in retTable.inValidProduct)
                    {
                        newRow = dtE.NewRow();
                        newRow["SN"] += ele.SN;
                        newRow["ERROR"] += ele.COAorError;
                        dtE.Rows.Add(newRow);
                        cnt++;
                    }

                    for (; cnt < fullRowCount; cnt++)
                    {
                        newRow = dtE.NewRow();
                        dtE.Rows.Add(newRow);
                    }

                    this.gridviewE.DataSource = dtE;
                    this.gridviewE.DataBind();
                    updatePanelE.Update();
                    initTableColumnHeaderE();
                }
            }
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            if (ex.Message.IndexOf("Cannot find table 0") >= 0)
            {
                showErrorMessage(msgFileEmpty);
            }
            else
            {
                showErrorMessage(ex.Message);
            }
            return;
        }
    }

    private void btnCancel_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            iCOAReturn.Cancel(hidFileName.Value);
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            return;
        }
    }
    public void btnSave_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            iCOAReturn.MakeSureSave(hidFileName.Value);
            Success();
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
    }
    private void Success()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript' type='text/javascript'>");
        scriptBuilder.AppendLine("Success();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelALL, typeof(System.Object), "Success", scriptBuilder.ToString(), false);
    }
    public void btnTableClear_ServerClick(object sender, System.EventArgs e)
    {
        TableClear();
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void uploadClick(object sender, System.EventArgs e)
     {
         string path = string.Empty;
         string errorMsg = "";
         bool fileOK = false;
         string strfile = "";
         string strfileTemp = "";
         string fullName = "";
         try
         {
             if (this.txtBrowse.HasFile)
             {

                 strfile = this.txtBrowse.FileName;

                 String fileExtension =
                         System.IO.Path.GetExtension(strfile).ToLower();
                 String[] allowedExtensions = { ".xls" };

                 for (int i = 0; i < allowedExtensions.Length; i++)
                 {
                     if (fileExtension.ToLower() == allowedExtensions[i].ToLower())
                     {
                         fileOK = true;
                         break;
                     }
                 }

                 if (!fileOK)
                 {
                     errorMsg = msgInvalidFileType;
                     writeToAlertMessage(errorMsg);
                 }
                 else
                 {
                     strfileTemp = DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() ;
                     fullName = Server.MapPath("../") + strfileTemp + strfile;
                     this.txtBrowse.PostedFile.SaveAs(fullName);
                 }
             }
             else
             {
                 showErrorMessage(msgEmptyFile);
             }
         }
         catch (FisException fe)
         {
             showErrorMessage(fe.mErrmsg);
             return;
         }
         catch (Exception ee)
         {
             showErrorMessage(ee.Message);
             return;
         }
         try
         {
             if (hidFileName.Value != "")
             {
                 iCOAReturn.Cancel(hidFileName.Value);
             }
             hidFileName.Value = strfileTemp;
             hidName.Value = strfile;
             TableClear();
             bool empty = true;
             S_COAReturn retTable = new S_COAReturn();
             DataTable dt = ExcelManager.getExcelSheetData(fullName);
             int i = 0;
             string SN = "";
             List<string> ret = new List<string>();
             foreach (DataRow dr in dt.Rows)
             {
                 i++;
                 if (i == 1)
                 {
                     continue;
                 }
                 else
                 {
                     SN = dr[0].ToString();
                     if (SN.Trim() == "")
                     {
                         break;
                     }
                     else
                     {
                         ret.Add(SN);
                     }
                 }
             }
             if (ret.Count > 0)
             {
                 retTable = iCOAReturn.GetProductTable("", UserId, station, Customer, hidFileName.Value, ret, true);
                 empty = false;
             }
             if (empty == true)
             {
                 showErrorMessage(msgFileEmpty);
                 return;
             }
             if (retTable.reValue == "true")
             {
                 if (retTable.validProduct.Count > 0)
                 {
                     DataTable dtV = initTableV();
                     DataRow newRow;

                     int cnt = 0;

                     foreach (S_RowData_COAReturn ele in retTable.validProduct)
                     {
                         newRow = dtV.NewRow();
                         newRow["SN"] += ele.SN;
                         newRow["COA"] += ele.COAorError;
                         dtV.Rows.Add(newRow);
                         cnt++;
                     }

                     for (; cnt < fullRowCount; cnt++)
                     {
                         newRow = dtV.NewRow();
                         dtV.Rows.Add(newRow);
                     }

                     this.gridviewV.DataSource = dtV;
                     this.gridviewV.DataBind();
                     updatePanelV.Update();
                     initTableColumnHeaderV();
                 }
                 if (retTable.inValidProduct.Count > 0)
                 {
                     DataTable dtE = initTableE();
                     DataRow newRow;
                     int cnt = 0;
                     foreach (S_RowData_COAReturn ele in retTable.inValidProduct)
                     {
                         newRow = dtE.NewRow();
                         newRow["SN"] += ele.SN;
                         newRow["ERROR"] += ele.COAorError;
                         dtE.Rows.Add(newRow);
                         cnt++;
                     }

                     for (; cnt < fullRowCount; cnt++)
                     {
                         newRow = dtE.NewRow();
                         dtE.Rows.Add(newRow);
                     }

                     this.gridviewE.DataSource = dtE;
                     this.gridviewE.DataBind();
                     updatePanelE.Update();
                     initTableColumnHeaderE();
                 }
             }

         }
         catch (FisException ee)
         {
             showErrorMessage(ee.mErrmsg);
             return;
         }
         catch (Exception ex)
         {
             if (ex.Message.IndexOf("Cannot find table 0") >= 0)
             {
                 showErrorMessage(msgFileEmpty);
             }
             else
             {
                 showErrorMessage(ex.Message);
             }
             return;
         }
         finally
         {
             deleteFiles(fullName);
         }
         writeToSuccessMessage(strfile);
     }
    
     private void writeToSuccessMessage(string fullname)
     {
         StringBuilder scriptBuilder = new StringBuilder();
         ArrayList test = new ArrayList();
         test.Add(fullname);
         string temp = "Upload " + fullname + " success!";
         string temp2 = "green";
         scriptBuilder.AppendLine("<script language='javascript'>");
         scriptBuilder.AppendLine("ShowInfo(\"" + temp + "\", \"" + temp2 + "\");");
         scriptBuilder.AppendLine("</script>");
         ScriptManager.RegisterStartupScript(this.updatePanelALL, typeof(System.Object), "writeToSuccessMessage", scriptBuilder.ToString(), false);
     }
     public  void deleteFiles(string strDir)
     {
         try
         {
             if (File.Exists(strDir))
             {
                 File.Delete(strDir);
             }
         }
         catch (Exception ex)
         {
             showErrorMessage(ex.Message);
         }
     }
}
