/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Codebehind for Final Scan page
 * UI:CI-MES12-SPEC-PAK-UI Final Scan.docx --2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC Final Scan.docx --2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-10  Zhang.Kai-sheng       (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
 * UI/UC Update (2010/10/20),ADD "Chep Pallet" and Data
 * UC --"5. Check Pallet"/"7. Check Chep Pallet"/"6. Save Data" -add "Chep Pallet Check" process (P7-P10)
 * Need to add/change Activity, modify WorkFlow, and UI Process
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

public partial class PAK_FinalScan : IMESBasePage
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 8;//12;//9
    private Object commServiceObj;
    public String UserId;
    public String Customer;
    private IFinalScan iFinalScan;
    //private string FinalScanBllName;

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
    /*
   protected void hidBtn_ServerClick(object sender, EventArgs e)
   {
       
       try
       {
           string pickID = string.Empty;
           string editor = UserInfo.UserId;
           string station = Request["Station"];
           string customer = UserInfo.Customer;
           IList ret = null;

           iFinalScan = ServiceAgent.getInstance().GetObjectByName<IFinalScan>(FinalScanBllName);
           ret = iFinalScan.InputPickID(string.Empty, pickID, editor, station, customer);
           setInfoAndEndWaiting();
           setFocus();
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
   */
    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void showErrorMessage(string errorMsg)
    {
        bindEmptyTable(DEFAULT_ROWS); //default show 12Lines
        //combine error message HTML string
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        //Show error message
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    
    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void initLabel()
    {
        //Get resource form local resource
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.lblPickID.Text = this.GetLocalResourceObject(Pre + "_lblPickID").ToString();
        this.lblForwarder.Text = this.GetLocalResourceObject(Pre + "_lblForwarder").ToString();
        this.lblDriver.Text = this.GetLocalResourceObject(Pre + "_lblDriver").ToString();
        this.lblTruckID.Text = this.GetLocalResourceObject(Pre + "_lblTruckID").ToString();
        this.lblRemainQty.Text = this.GetLocalResourceObject(Pre + "_lblRemainQty").ToString();
        this.lblChepPallet.Text = this.GetLocalResourceObject(Pre + "_lblChepPallet").ToString();
        this.lblChepPalletQty.Text = this.GetLocalResourceObject(Pre + "_lblChepPalletQty").ToString();
    }
    /// <summary>
    /// Create table and bind GridViewExt
    /// </summary>
      private void bindEmptyTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPickID").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPalletNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colQty").ToString());

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
