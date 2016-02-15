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
using System.Text;
using System.IO;
using IMES.DataModel;

public partial class PAK_CT13KeyPartsRequirementQuery_Upload : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string userId;
    public string customer;
    //    IPackingList iPackingList = ServiceAgent.getInstance().GetObjectByName<IPackingList>(WebConstant.PackingListObject);


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //userId = UserInfo.UserId;
            //customer = UserInfo.Customer;
            if (!Page.IsPostBack)
            {
                InitLabel();
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
            "window.parent.endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.parent.beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void uploadClick(object sender, System.EventArgs e)
    {
        string errorMsg = "";
        bool fileOK = false;

        try
        {
            //beginWaitingCoverDiv();
            if (this.FileUpload.HasFile)
            {
                string strfile = this.FileUpload.FileName;
                String fileExtension =
                            System.IO.Path.GetExtension(strfile).ToLower();
                String[] allowedExtensions = { ".xls", ".xlsx" };

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
                    errorMsg = this.GetLocalResourceObject(Pre + "_mesNoExcelFile").ToString();
                    endWaitingCoverDiv();
                    writeToAlertMessage(errorMsg);
                }
                else
                {
                    string fullName = Server.MapPath("../") + strfile;
                    this.FileUpload.PostedFile.SaveAs(fullName);
                    //·ÖÎöÎÄ¼þ£¬updateÊý¾Ý¿â
                    /*DataTable dt = ExcelManager.getExcelSheetData(fullName);
                    bool bEmptyFile = true;
                    foreach (DataRow dr in dt.Rows)
                    {
                    
                    }*/
                    //if (bEmptyFile)
                    //{
                    //    errorMsg = this.GetLocalResourceObject(Pre + "_mesEmptyFile").ToString();
                    //    endWaitingCoverDiv();
                    //    writeToAlertMessage(errorMsg);
                    //}
                    //else
                    {
                        string tmp = fullName;
                        //writeToAlertMessage(fullName);
                        //File.Delete(fullName);
                        //ÉÏ´«Íê±Ï£¬µô¸¸Ç×Ò³Ãæ£¬Ö´ÐÐquery²Ù×÷
                        //errorMsg = this.GetLocalResourceObject(Pre + "_mesUploadOver").ToString();
                        ////endWaitingCoverDiv();
                        writeToSuccessMessage(strfile);
                    }
                }
            }
            else if (this.FileUpload.FileName == "")
            {
                errorMsg = this.GetLocalResourceObject(Pre + "_mesNoInputFile").ToString();
                endWaitingCoverDiv();
                writeToAlertMessage(errorMsg);
            }
            else if (this.FileUpload.FileContent.Length == 0)
            {
                errorMsg = this.GetLocalResourceObject(Pre + "_mesNoExistedFile").ToString();
                endWaitingCoverDiv();
                writeToAlertMessage(errorMsg);
            }

        }
        catch (Exception ex)
        {
            endWaitingCoverDiv();
            writeToAlertMessage(ex.Message);
        }
    }


    /// <summary>
    /// </summary>
    private void InitLabel()
    {
        this.btnUpload.Value = this.GetLocalResourceObject(Pre + "_btnUpload").ToString();
    }

    private void ToAlert(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("window.parent.alert(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ToAlert", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("window.parent.ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("window.parent.ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// Êä³ö´íÎóÐÅÏ¢
    /// </summary>
    /// <param name="er"></param>
    private void writeToSuccessMessage(string fullname)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("window.parent.finish(\"" + fullname + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
}
