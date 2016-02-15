/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for UploadShipmentData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Upload Shipment Data to SAP.docx –2011/10/26 
 * UC:CI-MES12-SPEC-PAK-UC Upload Shipment Data to SAP.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/
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
using IMES.DataModel;
using System.Text;
using System.IO;
using System.Linq;

[Serializable]
public partial class PAK_UploadShipmentData : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    IUploadShipmentData iUploadShipmentData = ServiceAgent.getInstance().GetObjectByName<IUploadShipmentData>(WebConstant.UploadShipmentDataObject);
    
    public int initRowsCount =10;
    public int defaultRow =10;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmbStatus.Type = "DeliveryStatusForQuery";
            cmbStatus.DefaultValue="88";
            UserId = Master.userInfo.UserId;
            if (!Page.IsPostBack)
            {
                InitLabel();
                clearGrid();
           //     hidGUID.Value = Guid.NewGuid().ToString().Replace("-", "");
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
    public void GridView1_RowCreate(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //this.gvDN.HeaderRow.Cells[9].Width = Unit.Pixel(1);
          //  this.gvDN.HeaderRow.Cells[10].Width = Unit.Pixel(1);
         //   e.Row.Cells[9].Style.Add("display", "none");
         //   e.Row.Cells[10].Style.Add("display", "none");
      //      e.Row.Cells[9].Width = Unit.Pixel(0);
       //     e.Row.Cells[10].Width = Unit.Pixel(0);
        }
    
    }
    public void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
       // this.gvDN.HeaderRow.Cells[9].Width = Unit.Pixel(0);
    
  //      e.Row.Cells[8].Attributes.Add("Colspan", "3");
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Cells[9].Style.Add("display", "none");
        //    e.Row.Cells[10].Style.Add("display", "none");
        //    CheckBox chkSelect = e.Row.FindControl("chk") as CheckBox;
        //    DataRowView drv = e.Row.DataItem as DataRowView;
        //    string id = drv["DN"].ToString();
        //    chkSelect.Attributes.Add("onclick", string.Format("setSelectVal(this,'{0}');", id));
        //}
    } 
        
 
     [Serializable]
    public class UploadShipmentData
     {
         public string m_date;
         public string m_dn;
         public string m_pn;
         public string m_model;
         public string m_qty;
         public string m_status;
         public string m_pack;
         public string m_paqc;
         public string m_bAllowUpload;
         public string m_udt;
     
     }
    [System.Web.Services.WebMethod]
 
    public static  List<UploadShipmentData>  Query(DateTime fromDate,DateTime toDate, string dnNo, string status)
    {
        IUploadShipmentData mainObj = ServiceAgent.getInstance().GetObjectByName<IUploadShipmentData>(WebConstant.UploadShipmentDataObject);
        IList<S_RowData_UploadShipmentData> lst= null;
        if (string.IsNullOrEmpty(dnNo.Trim()))
        {
            lst = mainObj.GetTableData(fromDate, toDate, status);
        }
        else
        {
            lst = mainObj.GetTableDataByDnList(dnNo.Trim(),status);
        
        }
  
        List<UploadShipmentData> lstR=new List<UploadShipmentData>();

        foreach (S_RowData_UploadShipmentData ele in lst)
         {
             lstR.Add(new UploadShipmentData
                       {
                           m_date = ele.m_date.ToString("yyyy-MM-dd"),
                           m_dn = ele.m_dn,
                           m_pn = ele.m_pn,
                           m_model = ele.m_model,
                           m_qty = ele.m_qty.ToString(),
                           m_status = ele.m_status,
                           m_pack = ele.m_pack.ToString(),
                           m_paqc = ele.m_paqc,
                           m_bAllowUpload = ele.m_bAllowUpload.ToString(),
                           m_udt = ele.m_udt.ToString("yyyy-MM-dd HH:mm:ss.fff")
                       }
              );
         }

      
      
        return lstR;
    }
    [System.Web.Services.WebMethod]
    public static ArrayList Upload(object mpUser, List<object> dnList, List<object> lstUdt)
    {
        IUploadShipmentData mainObj = ServiceAgent.getInstance().GetObjectByName<IUploadShipmentData>(WebConstant.UploadShipmentDataObject);
        MpUserInfo mu = UTI.ConvertMpUserToObj(mpUser);
        IList<UploadDNInfo> lstUploadDNInfo = new List<UploadDNInfo>();
        int idx = 0;
        foreach (string d in dnList)
        {
  
            lstUploadDNInfo.Add(new UploadDNInfo { DeliveryNo = d, Udt = DateTime.Parse(lstUdt[idx].ToString()) });
            idx++;
          
        }
        ArrayList arr = mainObj.Upload(mu, lstUploadDNInfo);
          return arr;
    }
    
    private void InitLabel()
    {
        this.lblFrom.Text = this.GetLocalResourceObject(Pre + "_lblFrom").ToString();
        this.lblTo.Text = this.GetLocalResourceObject(Pre + "_lblTo").ToString();
     
    }

    private DataTable getNullDataTable()
    {
      
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
//        retTable.Columns.Add("Chk");
        retTable.Columns.Add("Date", Type.GetType("System.String"));
        retTable.Columns.Add("DN", Type.GetType("System.String"));
        retTable.Columns.Add("PN", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("Pack", Type.GetType("System.Int32"));
        retTable.Columns.Add("PAQC", Type.GetType("System.String"));
        retTable.Columns.Add("Flag");
        retTable.Columns.Add("Udt");

        return retTable;
    }

    private void initTableColumnHeader()
    {
        //this.GridViewExt1.HeaderRow.Cells[0].Text = "";
        this.gvDN.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleDate").ToString();
        this.gvDN.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titleDN").ToString();
        this.gvDN.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titlePN").ToString();
        this.gvDN.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_titleModel").ToString();
        this.gvDN.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_titleQty").ToString();
        this.gvDN.HeaderRow.Cells[6].Text = this.GetLocalResourceObject(Pre + "_titleStatus").ToString();
        this.gvDN.HeaderRow.Cells[7].Text = this.GetLocalResourceObject(Pre + "_titlePack").ToString();
        this.gvDN.HeaderRow.Cells[8].Text = this.GetLocalResourceObject(Pre + "_titlePAQC").ToString();
        this.gvDN.HeaderRow.Cells[9].Text = "";
        this.gvDN.HeaderRow.Cells[10].Text = "";

        this.gvDN.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        this.gvDN.HeaderRow.Cells[1].Width = Unit.Pixel(70);
        this.gvDN.HeaderRow.Cells[2].Width = Unit.Pixel(100);
        this.gvDN.HeaderRow.Cells[3].Width = Unit.Pixel(100);
        this.gvDN.HeaderRow.Cells[4].Width = Unit.Pixel(90);
        this.gvDN.HeaderRow.Cells[5].Width = Unit.Pixel(30);
        this.gvDN.HeaderRow.Cells[6].Width = Unit.Pixel(30);
        this.gvDN.HeaderRow.Cells[7].Width = Unit.Pixel(30);
        this.gvDN.HeaderRow.Cells[8].Width = Unit.Pixel(30);
   this.gvDN.HeaderRow.Cells[9].Width = Unit.Pixel(0);
     this.gvDN.HeaderRow.Cells[10].Width = Unit.Pixel(0);
     //this.gvDN.HeaderRow.Cells[9].Visible = false;
     //this.gvDN.HeaderRow.Cells[10].Visible = false;
  //   this.gvDN.HeaderRow.Cells[9].Style.Add("display", "none");
  //   this.gvDN.HeaderRow.Cells[10].Style.Add("display", "none");
    }

    private void clearGrid()
    {
        try
        {
          

            gvDN.DataSource = getNullDataTable();
            gvDN.DataBind();
            initTableColumnHeader();
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

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void transFile()
    {
        String script = "<script language='javascript'> downloadFile(); </script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "transFile", script, false);
    }

    private void clickDateFrom()
    {
        String script = "<script language='javascript'>document.getElementById('btnFrom').click();</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "clickDateFrom", script, false);
    }

    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "jsAlert", script, false);
    }

   

}
