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
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using System.IO;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.system.util;

public partial class DataMaintain_COAReceivingUploadFile : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private ICOAReceiving ICoaReceiving;
    public char[] SPLITSTR = new char[] { ':' };
    public const string CUSTOMERNAME = "Customer Name";
    public const string INVENTECPO = "Inventec P/O";
    public const string CUSTPN = "Customer P/N";
    public const string IECPN = "IEC P/N";
    public const string DESC = "Description";
    public const string SHIPPINGDATE = "Shipping Date";
    public const string QTY = "Quantity";
    public const string BEGNO = "Start COA Number";
    public const string ENDNO = "End COA Number";
    public String userName;

    public string msgInvalidFileType;
    public string msgEmptyFile;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        userName = Request.QueryString["userName"]; 
        userName = StringUtil.decode_URL(userName);
        
        ICoaReceiving = (ICOAReceiving)ServiceAgent.getInstance().GetMaintainObjectByName<ICOAReceiving>(WebConstant.COARECEIVING);
        try
        {
            msgInvalidFileType = this.GetLocalResourceObject(Pre + "_MsgInvalidFileType").ToString();
            msgEmptyFile = this.GetLocalResourceObject(Pre + "_msgEmptyFile").ToString();
            this.btnFileUpload.Value = this.GetLocalResourceObject(Pre + "_btnFileUpload").ToString();
            this.cancel.Value = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

     protected void Upload_ServerClick(Object sender, EventArgs e)
    {
        string path = string.Empty;
        try 
        {
            if (txtBrowse.PostedFile.ContentLength > 0)
            {
                if (Directory.Exists(Server.MapPath("~/File")) == false)
                {
                    Directory.CreateDirectory(Server.MapPath("~/File"));
                }
                else
                {
                    if (IsAllowableFileType())
                    {
                        string uploadfilePath = Server.MapPath("~/File/");
                        string fullname = txtBrowse.PostedFile.FileName;
                        string newName = DateTime.Now.Ticks.ToString() + fullname.Substring(fullname.LastIndexOf("."));
                        txtBrowse.SaveAs(uploadfilePath + newName);
                        path = uploadfilePath + newName;
                        string ip = GetLocationIP();

                        IList<COAReceivingDef> defLst = ICoaReceiving.ReadTXTFile(uploadfilePath + newName,userName,GetLocationIP());
                        ICoaReceiving.SaveTXTIntoTmpTable(defLst);
                       
                    }
                }
            }
        }
         catch(FisException fe)
        {
            showErrorMessage(fe.mErrmsg);
            return;
        }
         catch(Exception ee)
        {
            
            showErrorMessage(ee.Message);
            return;
         }
         finally
        {
            //删除文件
            if(!String.IsNullOrEmpty(path))
            {
                deleteFiles(path);
            }
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "closeDialog", "closeDialog();", true); 
        }
       
        
    }
    
     public static void deleteFiles(string strDir)
     {
         try
         {
             if (File.Exists(strDir))
             {
                 File.Delete(strDir);
                 Console.WriteLine("文件删除成功！");
             }
         }
         catch (Exception ex)
         {
             Console.WriteLine(ex.Message);
         }
     }

     private string GetLocationIP()
     {
         string ip = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
         return ip;
     }

     protected bool IsAllowableFileType()
     {
         string strFileTypeLimit = ConfigurationManager.AppSettings["FileType"].ToString();
         if (strFileTypeLimit.IndexOf(Path.GetExtension(txtBrowse.FileName).ToLower()) != -1)
         {
             return true;
         }
         return false;
     }
    /// <summary>
    /// 判断是否为空
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="errorMsg"></param>
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");

        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");

        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;

    }
}
