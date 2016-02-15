/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-03   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;
using System.Data.OleDb;

public partial class PalletVerifyDataForDocking : System.Web.UI.Page
{
    
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public int initRowsCount = 6;
    public string PDFClinetPath = ConfigurationManager.AppSettings["CreatePDFfileClientPath"].ToString();
    IPalletVerify iPalletVerify = ServiceAgent.getInstance().GetObjectByName<IPalletVerify>(WebConstant.PalletVerifyObject);
    public string PdfFilename = string.Empty;
    public string XmlFilename = string.Empty;
    public string Templatename = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();

                initTableColumnHeader();
                //绑定空表格
                this.GridViewExt1.DataSource = getNullDataTable();
                this.GridViewExt1.DataBind();

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;

                this.txtDataEntry.Focus();
               
            }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
            errorPlaySound();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            errorPlaySound();
        }
    }

    private DataTable getNullDataTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();

            newRow["ProdID"] = String.Empty;
            newRow["Carton"] = String.Empty;
            newRow["CustSN"] = String.Empty;
         //   newRow["PAQC"] = String.Empty;
          //  newRow["POD"] = String.Empty;
          //  newRow["WC"] = String.Empty;
          //  newRow["CollectionData"] = String.Empty;
           
            dt.Rows.Add(newRow);
        }

        return dt;
    }

    /// <summary>
    /// 初始化列表格
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("ProdID", Type.GetType("System.String"));
        retTable.Columns.Add("Carton", Type.GetType("System.String"));
        retTable.Columns.Add("CustSN", Type.GetType("System.String"));
        return retTable;
    }

    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.GridViewExt1.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_gridProd").ToString();
        this.GridViewExt1.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_gridCarton").ToString();
        this.GridViewExt1.Columns[2].HeaderText = this.GetLocalResourceObject(Pre + "_gridCustSn").ToString();
    }


    /// <summary>
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    /// <returns></returns>
     private void initLabel()
    {
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblPalletInfo.InnerText = this.GetLocalResourceObject(Pre + "_lblPalletInfo").ToString();
        this.lblPalletNo.Text = this.GetLocalResourceObject(Pre + "_lblPalletNo").ToString();
        this.lblPalletQty.Text = this.GetLocalResourceObject(Pre + "_lblPalletQty").ToString();
        this.lblProductLst.InnerText = this.GetLocalResourceObject(Pre + "_lblProductLst").ToString();
        this.lblScanQty.Text = this.GetLocalResourceObject(Pre + "_lblScanQty").ToString();
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.gdCheckBox.Text = this.GetLocalResourceObject(Pre + "_gdCheckBox").ToString();

     //   this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
    }


     /// <summary>
     /// Generate PDF: 调call_Op_TemplateCheckLaNew 获得模版名称
     /// Revision: 9241: 1.	增加 “Generate PDF”步骤; 2.	增加“Generate PDF”业务规则描述
     /// </summary>
     /// <returns></returns>
    protected void GetOPGrid(object sender, System.EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
           string plt = this.HD_PLT.Value;
           IList<string> arrDelivery = new List<string>();
           arrDelivery = iPalletVerify.getDeliveryPalletList(plt);
           string edi_Delivery = string.Empty;

           if (arrDelivery != null && arrDelivery.Count > 0)
           {
               edi_Delivery = arrDelivery[0];
               dt = iPalletVerify.call_Op_TemplateCheckLaNew(edi_Delivery, "Pallet Ship Label- Pack ID Single");
               
           }
           if (dt != null && dt.Rows.Count>0)
           {
               
               int a = 0;
               int rowsCount = dt.Rows.Count;
               int colsCount = dt.Columns.Count;
               string[,] arrTmp = new string[rowsCount, colsCount];
               foreach (System.Data.DataRow row in dt.Rows)
               {
                   int b = 0;
                   foreach (System.Data.DataColumn column in dt.Columns)
                   {
                       arrTmp[a, b] = row[column.ColumnName].ToString(); 
                       b = b + 1;
                   }
                   a = a + 1;
               }

               if (rowsCount > 1)
               {
                   for (int TpCount = 0; TpCount < rowsCount; TpCount++)
                   {
                       var Template = arrTmp[TpCount, 0].Trim();
                       var DOC_SET = arrTmp[TpCount, 2].Trim();
                       var Sntp = "";
                       var Attp = "";
                       var plttp = "";

                       if (DOC_SET.Contains("LA") || DOC_SET.Contains("NA-00010") || DOC_SET.Contains("EM"))
                       {
                           if (Template.Contains("Serial"))
                           {
                               Sntp = "SN";
                           }
                           else if (Template.Contains("General"))
                           {
                               Sntp = "Ship";
                           }
                           else if (Template.Contains("BOX_ID"))
                           {
                               Sntp = "EMEA";
                           }
                           else Sntp = "";
                       }

                       int TCount = 0;

                       if (Template.Contains("_ATT"))
                       {
                           Attp = "ATT";
                       }
                       else if (Template.Contains("Verizon2D"))
                       {
                           Attp = "2D";
                       }
                       else Attp = "";

                       if (Template.Contains("TypeB"))
                       {
                           plttp = "B";
                           CmdGeneratePdf(edi_Delivery, plt, "PLT", Template, Attp, TCount, Sntp);
                       }
                       else
                       {
                           plttp = "A";

                           int arrDNlength = arrDelivery.Count;
                           for (int DNCount = 0; DNCount < arrDNlength; DNCount++)
                           {
                               CmdGeneratePdf(arrDelivery[DNCount], plt, "PLT", Template, Attp, TpCount, Sntp);
                           }
                       }

                   }
               }
               else
               {
                   var Template = arrTmp[0,0].Trim();
                   var Sntp = "";
                   var Attp = "";
                   int TpCount= 0;
                   var plttp = "";

                   if (Template.Contains("TypeB"))
                   {
                       plttp = "B";
                       CmdGeneratePdf(edi_Delivery, plt, "PLT", Template, Attp, TpCount, Sntp);
                   }
                   else
                   {
                       plttp = "A";

                       int arrDNlength = arrDelivery.Count;
                       for (int DNCount = 0; DNCount < arrDNlength; DNCount++)
                       {
                           CmdGeneratePdf(arrDelivery[DNCount], plt, "PLT", Template, Attp, TpCount, Sntp);
                       }
                   }
               }
           }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
            errorPlaySound();
            initPage();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            errorPlaySound();
            initPage();
        }
    }

    /// <summary>
    /// Generate PDF
    /// Revision: 9241: 1.	增加 “Generate PDF”步骤; 2.	增加“Generate PDF”业务规则描述
    /// </summary>
    /// <returns></returns>
    private void CmdGeneratePdf(string DeliveryNo,string Pallet,string plttp,string Template,string Attp,int TCount,string Sntp)
    {
        try
        {
            Boolean CmdGeneratePdf = false;
            
            Templatename = Template;
            
            if (Template.Contains("TypeB"))
            {
                plttp = "B";
            }
            else plttp = "A";
            
            
            XmlFilename = DeliveryNo + "-" + Pallet + "-[PalletShipLabel]" + plttp.Trim() + Sntp.Trim() + ".xml";

            

            if (Attp == "ATT")
            {
                PdfFilename = DeliveryNo + "-" + Pallet + "-[ATTPalletShipLabel]" + Sntp.Trim() + ".pdf";
            }
            else if (Attp == "2D")
            {
                PdfFilename = DeliveryNo + "-" + Pallet + "-[2DPalletShipLabel]" + Sntp.Trim() + ".pdf";
            }
            else 
            {
                PdfFilename = DeliveryNo + "-" + Pallet + "-[PalletShipLabel]" + Sntp.Trim() + ".pdf";
            }

            para_transfer();

            if (TCount == 0)
            {

                generateCaseSGTIN96(DeliveryNo, Pallet, plttp);

            }

            //generatePDF();
            CmdGeneratePdf = true;
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
            errorPlaySound();
            initPage();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            errorPlaySound();
            initPage();
        }

    }
    /// <summary>
    /// para
    /// <returns></returns>
    private void para_transfer()
    {
           StringBuilder scriptBuilder = new StringBuilder();

          scriptBuilder.AppendLine("<script language='javascript'>");
          scriptBuilder.AppendLine("para_transfer(\"" + PdfFilename + "\",\"" + XmlFilename + "\",\"" + Templatename +  "\");");
          scriptBuilder.AppendLine("</script>");

          ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "para_transfer", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// GeneratePDF
    /// <returns></returns>
    private void generatePDF()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (GeneratePDF,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "GeneratePDF", script, false);
    }

    /// <summary>
    /// generateCaseSGTIN96
    /// <returns></returns>
    private void generateCaseSGTIN96(string DeliveryNo, string Pallet, string plttp)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("GenerateCaseSGTIN96(\"" + DeliveryNo +"\",\"" + Pallet  + "\",\"" + plttp + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "GenerateCaseSGTIN96", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// 报错信息
    /// </summary>
    /// <returns></returns>
    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");

        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// 报错提示音
    /// <returns></returns>
    private void errorPlaySound()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (PlaySound,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "PlaySound", script, false);
    }

    /// <summary>
    /// 清空页面
    /// <returns></returns>
    private void initPage()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (ResetPage,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "ResetPage", script, false);
    }
}