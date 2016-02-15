/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for Const Value Maintain Dlg Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1              
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-8-6     Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System.Web.UI;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.IO;
using System.Web.Configuration;
using com.inventec.system.util;
using System.Web;
using System.Reflection;
using System.Runtime.InteropServices;
using System;

public partial class DataMaintain_ConstValueMaintainDlg : System.Web.UI.Page
{

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); 

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String userName;
    public string pmtMessage1;
    public string pmtMessage2;
    private IConstValueMaintain iConstValueMaintain;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iConstValueMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueMaintain>(WebConstant.ConstValueMaintainObject);

            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();

            if (!this.IsPostBack)
            {              
                this.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
                
                userName = Request.QueryString["userName"];
                userName = StringUtil.decode_URL(userName);
                this.HiddenUserName.Value = userName;

                initLabel();
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

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            //20130505 Vic Mantis 1767
            //if (this.dType.Text == "")
            //{
            //    showErrorMessage(pmtMessage1);
            //    this.dType.Focus();
            //    return;
            //}

            //bool typeExist = false;
            //IList<string> typeLst = iConstValueMaintain.GetConstValueTypeList();
            //foreach (string tempType in typeLst)
            //{
            //    if (this.dType.Text.Trim() == tempType)
            //    {
            //        typeExist = true;
            //    }
            //}

            //if (typeExist == true)
            //{
            //    showErrorMessage(pmtMessage2);
            //    this.dType.Text = "";
            //    this.dType.Focus();
            //    return;
            //}

            //ConstValueInfo newTypeInfo = new ConstValueInfo();
            //newTypeInfo.name = "";
            //newTypeInfo.type = this.dType.Text.Trim();
            //newTypeInfo.value = "";
            //newTypeInfo.description = "Type";
            //newTypeInfo.editor = this.HiddenUserName.Value;
            //newTypeInfo.udt = DateTime.Now;
            //newTypeInfo.cdt = DateTime.Now;
            //iConstValueMaintain.AddConstValue(newTypeInfo);

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
        finally
        {
        }

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "OKComplete", "OKComplete();DealHideWait();", true);
    }


    private void initLabel()
    {
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnCancel.Value = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("\r\n", "<br>");
        sourceData = sourceData.Replace("\n", "<br>");
        sourceData = sourceData.Replace(@"\", @"\\");
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }


    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg+ "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }    
    
}
