/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for Image Download Page
 * UI:CI-MES12-SPEC-FA-UI Image Download.docx –2011/10/28 
 * UC:CI-MES12-SPEC-FA-UC Image Download.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-4   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0801, Jessica Liu, 2012-2-28
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

public partial class FA_ImageDownload : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public int initRowsCount = 10;

    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {
           //2012-4-16
           UserId = Master.userInfo.UserId;
           Customer = Master.userInfo.Customer;

            if (!this.IsPostBack)
            {
                initLabel();

                this.gridview.DataSource = getNullDataTable();
                this.gridview.DataBind();
                InitGridView();

                //ITC-1360-0801, Jessica Liu, 2012-2-28
                this.hiddenStation.Value = "66"; //Request["Station"];
                this.pCode.Value = Request["PCode"];
                /*
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                 */
                this.txtDataEntry.Focus();
            }
       }
       catch (FisException ex)
       {
           writeToAlertMessage(ex.mErrmsg);
       }
       catch (Exception ex)
       {
           writeToAlertMessage(ex.Message);
       }
    }

    private void initLabel()
    {
        this.lblCPQSNO.Text = this.GetLocalResourceObject(Pre + "_lblCPQSNO").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblBIOS.Text = this.GetLocalResourceObject(Pre + "_lblBIOS").ToString();
        this.lblImage.Text = this.GetLocalResourceObject(Pre + "_lblImage").ToString();
        this.lblDataLog.Text = this.GetLocalResourceObject(Pre + "_lblDataLog").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString(); 
    }


    private DataTable getNullDataTable()
    {

        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
			
            newRow["ProdID"] = "";
            newRow["Model"] = "";
            newRow["BIOS"] = "";
            newRow["Image"] = "";
            
            dt.Rows.Add(newRow);
        }

        return dt;
    }
    

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();

        retTable.Columns.Add("ProdID", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("BIOS", Type.GetType("System.String"));
        retTable.Columns.Add("Image", Type.GetType("System.String"));
        
        return retTable;
    }
    

    private void InitGridView()
    {
        this.gridview.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_lblProdIDItem").ToString(); 
        this.gridview.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_lblModelItem").ToString(); 
        this.gridview.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_lblBIOSItem").ToString(); 
        this.gridview.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_lblImageItem").ToString();

        this.gridview.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        this.gridview.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        this.gridview.HeaderRow.Cells[2].Width = Unit.Pixel(200);
        this.gridview.HeaderRow.Cells[3].Width = Unit.Pixel(300);
    }
    

    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            this.gridview.DataSource = getNullDataTable();
            this.gridview.DataBind();
            InitGridView();
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

    public void btnHidden_Click(object sender, System.EventArgs e)
    {
        try
        {
            //ÖØÖÃÎÄ±¾¿ò
            this.lblCPQSNO.Text = "";
            this.lblModel.Text = "";
            this.lblBIOS.Text = "";
            this.lblImage.Text = "";
            this.lblDataLog.Text = "";
            this.btnSave.Value = "";

            this.txtDataEntry.Focus();
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
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
}


