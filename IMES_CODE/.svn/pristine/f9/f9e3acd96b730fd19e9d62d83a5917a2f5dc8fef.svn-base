/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Input
* UI:CI-MES12-SPEC-PAK-UC PAQC Input.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Input.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   liuqingbiao           Create   
* Known issues:
* TODO：
* 
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


using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.DataModel;
using System.IO;

public partial class PAK_COARemoval : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICOARemoval iCOARemoval = ServiceAgent.getInstance().GetObjectByName<ICOARemoval>(WebConstant.COARemovalObject);
   
    private const int DEFAULT_ROWS = 12;
    //private IDefect iDefect;
    public String UserId;
    public String Customer;
    public String Station;

    protected void Page_Load(object sender, EventArgs e)
    {
            if (!this.IsPostBack)
            {
                initLabel();
                this.gridview.DataSource = getNullDataTable(DEFAULT_ROWS);
                this.gridview.DataBind();
                initTableColumnHeader();
         
                //bindTable(DEFAULT_ROWS);
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                
                setColumnWidth();
                setFocus();
            }

    }
    private DataTable getNullDataTable(int rowCount)
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < rowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["Action"] = String.Empty;
            newRow["COANo"] = String.Empty;
            newRow["Cause"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
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

    public void uploadOver(object sender, System.EventArgs e)
    {
        string errorMsg = "";
        DataTable dt = null;
        string Succ_info = "Upload Success!";
        try
        {
            
            /*if (this.radScrap.Checked)
            {
                string cause = this.cmbCauseItem.InnerDropDownList.SelectedValue;

                if (cause == "")
                {
                    errorMsg = this.GetLocalResourceObject(Pre + "_msgNoSelectCause").ToString();
                    writeToAlertMessage(errorMsg);
                }
            }*/

            string uploadFile = this.hiddenCostCenter.Value;
            string fullName = Server.MapPath("../") + uploadFile;

            dt = ExcelManager.getExcelSheetData(fullName);
            int i = 0;
            string DNShipment = "";
            bool bEmptyFile = true;
            string allErrors = string.Empty;
            IList<string> returnList = new List<string>();
            IList<string> tempList = new List<string>();

            ////////////////////////////////////////////////////////////
            bool bFlag = false;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                if (i == 1)
                {
                    continue;
                }
                else
                {
                    DNShipment = dr[0].ToString();
                        for (int j = 0; j < tempList.Count; j++)
                        {
                            if (DNShipment == tempList[j])
                            {
                                bFlag = true;
                            }
                        }
                        if (bFlag == false)
                        {
                            returnList.Add(DNShipment);
                        }
                        bFlag = false;
                    tempList.Add(DNShipment);
                    if (DNShipment.Trim() == "")
                    {
                        break;
                    }
                }
            }
            ///////////////////////////////////////////////////////////

            ArrayList serviceResult = new ArrayList();
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                if (i == 1)
                {
                    continue;
                }
                else
                {
                    DNShipment = dr[0].ToString();
                    tempList.Add(DNShipment);
                    if (DNShipment.Trim() == "")
                    {
                        continue;
                    }
                    else
                    {
                        bEmptyFile = false;
                    }
                }
            }
           /* foreach (DataRow dr in dt.Rows)
            {
                i++;
                if (i == 1)
                {
                    continue;
                }
                else
                {
                    DNShipment = dr[0].ToString();
                    tempList.Add(DNShipment);
                    if (DNShipment.Trim() == "")
                    {
                        continue;
                    }
                    else*/
                    if (bEmptyFile == false)
                    {
                        //bEmptyFile = false;
                        //bool bExist = false;
                        string pdLine = "", prodId = "", editor = "", stationId = "", customerId = "";
                        try
                        {
                            string action = "";
                            if (this.radScrap.Checked)
                            {
                                action = "scrap";
                            }

                            serviceResult = iCOARemoval.InputCOANumberList(returnList, pdLine, prodId, editor, stationId, customerId, action);
                        }
                       catch (FisException ee)
                        {
                            allErrors = allErrors + ee.mErrmsg;
                            //continue;
                        }
                        catch (Exception ex)
                        {
                            allErrors = allErrors + ex.Message;
                            //continue;
                        }
                        /*
                        DNShipment = (string)serviceResult[1];
                        for (int j = 0; j < returnList.Count; j++)
                        {
                            if (DNShipment == returnList[j])
                            {
                                bExist = true;
                            }
                        }
                        if (bExist == false)
                        {
                            returnList.Add(DNShipment);
                       }
                        */
                    }
                //}
           // }
            if (bEmptyFile)
            {
                errorMsg = this.GetLocalResourceObject(Pre + "_mesEmptyFile").ToString();

                endWaitingCoverDiv();
                writeToAlertMessage(errorMsg);

            }
            else
            {
                if (allErrors != "")
                {
                    writeToAlertMessage(allErrors);
                    endWaitingCoverDiv();
                    //FileProcess();
                }
                /*else
                {
                    writeToAlertMessage(Succ_info);
                }*/
                
                string tmp = fullName;
                File.Delete(fullName);

                //this.gridview.DataSource = getDataTable(ret.Count, ret);
                IList<string> resultList = new List<string>();
                for (int ii = 0; ii < serviceResult.Count - 1; ii++)
                {
                    resultList.Add((string)serviceResult[ii]);
                }
                string errortmp = "";
                errortmp = (string)serviceResult[serviceResult.Count - 1];
                //errortmp = errortmp.Substring(0, errortmp.Length - 3);
                if (errortmp != "")
                writeToAlertMessage(errortmp);
                endWaitingCoverDiv();

                this.gridview.DataSource = getDataTable(resultList.Count, resultList);

                this.gridview.DataBind();
                initTableColumnHeader();

                updatePanel1.Update();
                //FileProcess();
                //FileProcess2();

            }

            ////////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////////
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.Message);
            endWaitingCoverDiv();
        }
        catch (Exception ex)
        {
            if (dt == null)
            {
                errorMsg = this.GetLocalResourceObject(Pre + "_mesEmptyFile").ToString();
                writeToAlertMessage(errorMsg);
            }
            else
            {
                writeToAlertMessage(ex.Message);
            }
            endWaitingCoverDiv();
        }

    }

    private void initTableColumnHeader()
    {

        this.gridview.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_colAction").ToString();
        this.gridview.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colCOANo").ToString();
        this.gridview.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_colCause").ToString();

    }

    private void FileProcess()
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("fileProcess();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "FileProcess", scriptBuilder.ToString(), false);
    }

    private void FileProcess2()
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("fileProcess2();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "FileProcess2", scriptBuilder.ToString(), false);
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private DataTable initTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colAction").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCOANo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCause").ToString());

        return dt;
    }

    private DataTable getDataTable(int rowCount, IList<string> returnList)
    {
        DataTable dt = initTable();
        DataRow newRow;

        string errorMsg = "";

        string cause = this.cmbCauseItem.InnerDropDownList.SelectedValue;
        
        string action = "";
        if (this.radScrap.Checked)
        {
            action = "Scrap";
            if (cause == "")
            {
                errorMsg = this.GetLocalResourceObject(Pre + "_msgNoSelectCause").ToString();
                writeToAlertMessage(errorMsg);
                for (int i = 0; i < DEFAULT_ROWS; i++)
                {
                    newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
            else
            {
                if (returnList != null && returnList.Count != 0)
                {
                    for (int i = 0; i < returnList.Count; i++)
                    {
                        newRow = dt.NewRow();
                        newRow["Action"] = action;
                        newRow["COANo"] = returnList[i];
                        newRow["Cause"] = cause;

                        dt.Rows.Add(newRow);
                    }

                    if (returnList.Count < DEFAULT_ROWS)
                    {
                        for (int i = returnList.Count; i < DEFAULT_ROWS; i++)
                        {
                            newRow = dt.NewRow();
                            dt.Rows.Add(newRow);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < DEFAULT_ROWS; i++)
                    {
                        newRow = dt.NewRow();
                        dt.Rows.Add(newRow);
                    }
                }

                return dt;
            }
        }
        else
        {
            action = "Remove";
            if (returnList != null && returnList.Count != 0)
            {
                for (int i = 0; i < returnList.Count; i++)
                {
                    newRow = dt.NewRow();
                    newRow["Action"] = action;
                    newRow["COANo"] = returnList[i];
                    newRow["Cause"] = cause;

                    dt.Rows.Add(newRow);
                }

                if (returnList.Count < DEFAULT_ROWS)
                {
                    for (int i = returnList.Count; i < DEFAULT_ROWS; i++)
                    {
                        newRow = dt.NewRow();
                        dt.Rows.Add(newRow);
                    }
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS; i++)
                {
                    newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                }
            }

            return dt;
        }


    }

    private void initLabel()
    {
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblCOANumber").ToString();
        this.lblRepairList.Text = this.GetLocalResourceObject(Pre + "_lblCOAListForRemove").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

        this.lblChooseAction.Text = this.GetLocalResourceObject(Pre + "_lblChooseAction").ToString();
        this.lblScrap.Text = this.GetLocalResourceObject(Pre + "_lblScrap").ToString();
        this.lblRemove.Text = this.GetLocalResourceObject(Pre + "_lblRemove").ToString();
        this.lblCause.Text = this.GetLocalResourceObject(Pre + "_lblCause").ToString();
  
    }

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colAction").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCOANo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCause").ToString());

        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }

        gridview.DataSource = dt;
        gridview.DataBind();
        //gd.Row.Cells[0].Visible = false;
        //.Row.Cells[3].Visible = false;      
    }

    private void setColumnWidth()
    {

        //gd.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        //Set column width 
        //================================= Add Code ======================================


        //================================= Add Code End ==================================
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }
}
