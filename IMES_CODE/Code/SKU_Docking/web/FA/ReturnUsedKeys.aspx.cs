/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Codebehind for Final Scan page          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * Known issues:
 * TODO:
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
using System.IO;

public partial class PAK_ReturnUsedKeys : IMESBasePage
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public const int DEFAULT_ROWS = 8;//12;//9
    private Object commServiceObj;
    public String UserId;
    public String Customer;
    public String Station;
    ReturnUsedKeys iReturnUsedKeys = ServiceAgent.getInstance().GetObjectByName<ReturnUsedKeys>(WebConstant.ReturnUsedKeysObject);

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();
                
                bindEmptyTable(DEFAULT_ROWS);
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                Station = Request["Station"];
                setColumnWidth();
                setFocus();
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

     private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("CustSN", Type.GetType("System.String"));
        retTable.Columns.Add("P/N", Type.GetType("System.String"));
        retTable.Columns.Add(" ", Type.GetType("System.String"));
        return retTable;
    }

     private DataTable getDataTable(IList<string> SNList, IList<string> PNList)
     {
         bindEmptyTable(DEFAULT_ROWS);
         DataTable dt = initTable();
         DataRow newRow;
         
         if (SNList != null && SNList.Count != 0)
         {
             
           
             for (int i = 0; i < SNList.Count; i++)
             {
                 newRow = dt.NewRow();
                 newRow["CustSN"] = SNList[i].ToString();
                 newRow["P/N"] = PNList[i].ToString();
                 //newRow["TEMP"] = "";

                 dt.Rows.Add(newRow);
             }

             if (SNList.Count < DEFAULT_ROWS)
             {
                 for (int i = SNList.Count; i < DEFAULT_ROWS; i++)
                 {
                     newRow = dt.NewRow();
                     dt.Rows.Add(newRow);
                 }
             }
             ClearIndex(SNList.Count);
         }
         else
         {
             for (int i = 0; i < DEFAULT_ROWS; i++)
             {
                 newRow = dt.NewRow();
                 dt.Rows.Add(newRow);
             }
             ClearIndex(0);
         }

         return dt;
     }

    public void uploadOver(object sender, System.EventArgs e)
    {
        string errorMsg = "";
        DataTable dt = null;
        FileStream fileStream = null;
        StreamReader streamReader = null;  

        try
        {
            string uploadFile = this.hiddenCostCenter.Value;
            string fullName = Server.MapPath("../") + uploadFile;

            fileStream = new FileStream(fullName, FileMode.Open, FileAccess.Read);
            streamReader = new StreamReader(fileStream, Encoding.Default);
            fileStream.Seek(0, SeekOrigin.Begin);
            string content = streamReader.ReadLine();  

            string CustSN = "";
            bool bEmptyFile = true;
            string allErrors = string.Empty;
            IList<string> SNList = new List<string>();
            IList<string> PNList = new List<string>();


            while (content != null)
            {
                CustSN = content.Trim();
               
                if (CustSN == "")
                {
                    content = streamReader.ReadLine();
                    continue;
                }
                else
                {
                    ArrayList serviceResult = new ArrayList();
                    bEmptyFile = false;
                    //if(!(CustSN.Length == 10 && CustSN.Substring(0, 3) == "CNU"))
					if (!CheckCustomerSN(CustSN))
                    {
                        allErrors = allErrors + CustSN + ":Wrong Code!";
                        content = streamReader.ReadLine();
                        continue;
                    }
                   
                    bool bExist = false;
                    try
                    {
                        serviceResult = iReturnUsedKeys.Check(CustSN, UserId, Station, Customer);
                    }
                    catch (FisException ee)
                    {
                        allErrors = allErrors + ee.mErrmsg;
                        content = streamReader.ReadLine();
                        continue;
                    }
                    catch (Exception ex)
                    {
                        allErrors = allErrors + ex.Message;
                        content = streamReader.ReadLine();
                        continue;
                    }
                    
                    for (int j = 0; j < SNList.Count; j++)
                    {
                        if (CustSN == SNList[j])
                        {
                            bExist = true;
                        }
                    }
                    if (bExist == false)
                    {
                        SNList.Add(CustSN);
                        PNList.Add((string)serviceResult[0]);
                    }
                    content = streamReader.ReadLine();
                }
            }

            if (bEmptyFile)
            {
                errorMsg = this.GetLocalResourceObject(Pre + "_mesEmptyFile").ToString();
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }  
                endWaitingCoverDiv();
                writeToAlertMessage(errorMsg);
               
            }
            else
            {

                if (fileStream != null)
                {
                    fileStream.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }  
                string tmp = fullName;
                File.Delete(fullName);

                this.gd.DataSource = getDataTable(SNList, PNList);

                this.gd.DataBind();
                //initTableColumnHeader();
                //updatePanel1.Update();

                FileProcess();
                if (allErrors != "")
                {
                    writeToAlertMessage(allErrors);


                   // endWaitingCoverDiv();
                   // FileProcess();
                }
                else
                {
                    FileProcess2();
                  //  FileProcess();
                }

            }
           
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

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
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
    private void ClearIndex(int n)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ClearIndex(\"" + n + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ClearIndex", scriptBuilder.ToString(), false);
    }
    private void ClearTable()
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ClearTable();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ClearTable", scriptBuilder.ToString(), false);
    }
    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void showErrorMessage(string errorMsg)
    {
        bindEmptyTable(DEFAULT_ROWS); //default show 12Lines
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        //Show error message
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    
    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void initLabel()
    {
        //Get resource form local resource
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.lblCustList.Text = this.GetLocalResourceObject(Pre + "_lbCustList").ToString();
        this.lbInserFile.Text = this.GetLocalResourceObject(Pre + "_lbInserFile").ToString();
        this.refresh.Value = this.GetLocalResourceObject(Pre + "_lbRefresh").ToString();
        this.returnKey.Value = this.GetLocalResourceObject(Pre + "_lbRetrunKey").ToString();      
    }
    /// <summary>
    /// Create table and bind GridViewExt
    /// </summary>
      private void bindEmptyTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCustSN").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPN").ToString());
        dt.Columns.Add(" ");

        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
        }
        //iMES:GridViewExt ID="gd" data bind
        gd.DataSource = dt;
        gd.DataBind();
    }

    private void setColumnWidth()
    {
        //gd.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        //Set column width 
    }
    //-----------------------------------
    //Set input Object Focus 
    //-----------------------------------
    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }

    private void setInfoAndEndWaiting()
    {
        String script = "<script language='javascript'>  setInfo();inputFlag = true;endWaitingCoverDiv(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setInfoAndEndWaiting", script, false);
    }
}
