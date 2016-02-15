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

public partial class FA_UploadFile : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IDismantleFA DismantleFAManager = (IDismantleFA)ServiceAgent.getInstance().GetObjectByName<IDismantleFA>(WebConstant.DismantleFA);
    public string userId;
    public string customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //<bug>
            //    BUG NO:ITC-1122-0314 
            //    REASON:每次都获取，防止提交后置空
            //</bug> 
            //userId = Master.userInfo.UserId;
            //customer = Master.userInfo.Customer;

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
            "window.parent.V_endWaitingCoverDiv();" + "\r\n" +
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
    /// MakeDirIfNotExist
    /// </summary>
    /// <param name="path"></param>
    private static void MakeDirIfNotExist(string path)
    {
        if (!Directory.Exists(path))
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch
            {
                //忽略
            }
        }
    }


    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void uploadClick(object sender, System.EventArgs e)
    {
        string errorMsg = "";
        string fullFileName = "";
        bool fileOK = false;
        IList<string> productidlist = new List<string>();
        try {
            //beginWaitingCoverDiv();
            if (this.FileUpload.HasFile)
            {
                
                string strfile = this.FileUpload.FileName;
                
                String fileExtension =
                        System.IO.Path.GetExtension(strfile).ToLower();
                String[] allowedExtensions = 
                        { ".txt"};

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
                else if (this.FileUpload.PostedFile.ContentLength > 250000)
                {
                    errorMsg = this.GetLocalResourceObject(Pre + "_meslargefile").ToString();
                    endWaitingCoverDiv();
                    writeToAlertMessage(errorMsg);
                }
                else
                {

                    //上传文件

                    string filePath = HttpContext.Current.Server.MapPath("~");
                    string path = filePath + "\\tmp";
                    //根下tmp目录存上传临时文件
                    MakeDirIfNotExist(path);  

                    string extName = strfile.Substring(strfile.LastIndexOf("."));
                    Guid guid = System.Guid.NewGuid();
                    fullFileName = path + "\\" + guid.ToString() + extName;



                    string fullName = fullFileName;//Server.MapPath("../")  + strfile;
                    this.FileUpload.PostedFile.SaveAs(fullName);
                    //分析文件，
                    productidlist = GetProductIDorSNListbyText(fullName);//GetProductIDListbyText

                    if (productidlist.Count==0)
                    {
                        errorMsg = this.GetLocalResourceObject(Pre + "_mesEmptyFile").ToString();
                        endWaitingCoverDiv();
                        writeToAlertMessage(errorMsg);
                        
                    }
                    else
                    {

                        //iSmallPartsUpload.Save(ret);
                        //File.Delete(fullName);
                        //上传完毕，Check Dismantle
                        //errorMsg = this.GetLocalResourceObject(Pre + "_mesUploadOver").ToString();
                        
                        string rstr = DismantleFAManager.CheckPrdIDorSNList(productidlist, this.lDismantletype.Value, this.lKeyparts.Value, this.lReturnStation.Value, "", this.pCode.Value, this.Editor.Value, this.Station.Value, this.Customer.Value);
                        if (rstr != "OK")
                        {
                            throw new Exception("Please check upload file: "+ rstr); 
                        }
                        DismantleFAManager.DismantleBatch(productidlist, this.lDismantletype.Value, this.lKeyparts.Value, this.lReturnStation.Value, "", this.pCode.Value, this.Editor.Value, this.Station.Value, this.Customer.Value);

                        endWaitingCoverDiv();
                        //writeToAlertMessage("Batch dismantle success!");
                        writeToSuccessMessage();
                        
                        
                    }
                }
                

            } 
            else if (this.FileUpload.FileName == "") 
            {
               errorMsg = this.GetLocalResourceObject(Pre + "_mesNoInputFile").ToString();
               endWaitingCoverDiv();
               writeToAlertMessage(errorMsg);
            }  
            else if (this.FileUpload.FileContent.Length == 0 ) 
            {
               errorMsg = this.GetLocalResourceObject(Pre + "_mesNoExistedFile").ToString();
               endWaitingCoverDiv();
               writeToAlertMessage(errorMsg);
            }
            
        }
        catch (FisException em)
        {
            endWaitingCoverDiv();
            writeToAlertMessage(em.Message);
        }
        catch (Exception ex)
        {
            endWaitingCoverDiv();
            writeToAlertMessage(ex.Message);
            
        }

    }


    private IList<string> GetProductIDListbyText(string strFilePath)
    {
        IList<string> ret = new List<string>();
        StreamReader sr = null;
        string str = string.Empty;
        try
        {
            sr = new StreamReader(strFilePath);
            sr.BaseStream.Seek(0, SeekOrigin.End);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            while ((str = sr.ReadLine()) != null)
            {
                if (str.Trim().Length == 10)
                {
                    //str.TrimStart().Substring(0, 1).Equals("#")
                    //str.Substring(0, str.IndexOf('=')).Equals(strKey)
                    ret.Add(str.Trim());
                }
            }
            return ret;
        }
        catch (Exception ex)
        {
            endWaitingCoverDiv();
            writeToAlertMessage(ex.Message);
            return null;
        }
    }
    private IList<string> GetProductIDorSNListbyText(string strFilePath)
    {
        int IDCount = 0;
        int start = 0, end = 0;
        string entries = "";
        string entry;
        var bFindID = false;
        IList<string> entryList = new List<string>();
        StreamReader sr = null;
        try
        {
            sr = new StreamReader(strFilePath);
            entries = sr.ReadToEnd();
            sr.Close();
            sr = null;
            //删除文件
            File.Delete(strFilePath);
            while (end < entries.Length)
            {
                end = entries.IndexOf('\r', start);
                if (end == -1)
                {
                    end = entries.Length;
                    //continue;
                }
                IDCount = IDCount + 1;
                entry = entries.Substring(start, (end - start)).Trim();
                bFindID = false;
                if ((entry.Length == 9) || (entry.Length == 10) || (entry.Length == 0))
                {
                    if (entry.Length != 0)
                    {
                        if ((entry.Length == 10) && (entry.Substring(0, 3) != "CNU"))
                            entry = entry.Substring(0, 9);
                        for (int i = 0; i < entryList.Count; i++)
                        {
                            if (entryList[i] == entry)
                            {
                                bFindID = true;
                                break;
                            }
                        }
                        if (bFindID == false)
                            entryList.Add(entry);
                    }
                }
                else
                {
                    string errorMsg = "";
                    errorMsg = this.GetLocalResourceObject(Pre + "_mesErrorFile").ToString() + " (Please Check line:" + IDCount+")";
                    throw new Exception(errorMsg);


                }
                start = end + 2;
            }

            return entryList;
        }
        catch (Exception ex)
        {
            endWaitingCoverDiv();
            writeToAlertMessage(ex.Message);
            return null;
        }
    }


    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lblFile.Text = this.GetLocalResourceObject(Pre + "_lblFile").ToString();
        //this.btnUpload.Value = this.GetLocalResourceObject(Pre + "_btnUpload").ToString();


    }

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("window.parent.ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("window.parent.ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("window.parent.bindNullTable();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToSuccessMessage()
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("window.parent.ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        //scriptBuilder.AppendLine("window.parent.ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("window.parent.query();");
        scriptBuilder.AppendLine("window.parent.ShowSuccessfulInfo(true);");
        
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }
}
