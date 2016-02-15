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
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using com.inventec.iMESWEB;
using System.IO;
using System.Text.RegularExpressions;
using IMES.Maintain.Interface.MaintainIntf;

public partial class DataMaintain_COAReceiving : System.Web.UI.Page
{
    public String userName;
    public  char[] SPLITSTRWITHENTER = new char[]{'\n'};
    public  char[] SPLITSTR = new char[]{':'};

    public const string CUSTOMERNAME="Customer Name";
    public const string INVENTECPO = "Inventec P/O";
    public const string CUSTPN = "Customer P/N";
    public const string IECPN = "IEC P/N";
    public const string DESC = "Description";
    public const string SHIPPINGDATE = "Shipping Date";
    public const string QTY = "Quantity";
    public const string BEGNO = "Start COA Number";
    public const string ENDNO = "End COA Number";


    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private ICOAReceiving ICoaReceiving;
    private const int COL_NUM = 3;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        //PostBackTrigger trigger = new PostBackTrigger();
        //trigger.ControlID = btnUpLoad.ID;
        //updatePanel.Triggers.Add(trigger);
        ICoaReceiving = (ICOAReceiving)ServiceAgent.getInstance().GetMaintainObjectByName<ICOAReceiving>(WebConstant.COARECEIVING);
        if (!this.IsPostBack)
        {
             pmtMessage1 = this.GetLocalResourceObject(Pre + "_successMessage").ToString();
            //pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            //pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            //pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            //pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            //pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            //pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();

            
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            initLabel();

            try
            {
                showListByACAdaptorList();
            }
            catch (FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
                return;
            }
            catch (Exception ee)
            {
                showErrorMessage(ee.Message);
                return;
            }
            

        }
        //IList<COAReceivingDef> datalst1 = ICoaReceiving.GetTmpTableItem("");
        //bindTable(datalst1, DEFAULT_ROWS);
   //     bindTable(null, DEFAULT_ROWS);
    }
   
   
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }

    }
    private void initLabel()
    {
        this.lblPO.Text = this.GetLocalResourceObject(Pre + "_lblPO").ToString();
        this.lblShippingDate.Text = this.GetLocalResourceObject(Pre + "_lblShippingDate").ToString();
        this.lblCUSTPN.Text = this.GetLocalResourceObject(Pre + "_lblCustPN").ToString();
        this.lblIECPN.Text = this.GetLocalResourceObject(Pre + "_lblIECPN").ToString();
        this.lblDescription.Text = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();
        this.lblCOARangeLst.Text = this.GetLocalResourceObject(Pre + "_lblCOARangeList").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnClear.Value = this.GetLocalResourceObject(Pre + "_btnClear").ToString();
 //       this.lblInsertFile.Text = this.GetLocalResourceObject(Pre + "_lblInsertFile").ToString();
        this.btnUpLoad.Value = this.GetLocalResourceObject(Pre + "_btnUpload").ToString();
        
    }
    private void bindTable(IList<COAReceivingDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        //init top Label...
        if(list!=null&&list.Count>0)
        {
            COAReceivingDef def = list[0];
            this.lblPOValue.Text = def.po.Trim();
            this.lblShippingDateVal.Text = def.shippingDate.Trim();
            this.lblCustPNVal.Text = def.custPN.Trim();
            this.lblIecPNVal.Text = def.iecPN.Trim();
            this.lblDescriptionVal.Text = def.description.Trim();
            this.updatePanel1.Update();
        }
        

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colBegNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEndNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colQty").ToString());
        dt.Columns.Add("ID");
        if (list != null && list.Count != 0)
        {
            foreach (COAReceivingDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.begNO;
                dr[1] = temp.endNO;
                dr[2] = temp.qty;
                dr[3] = temp.id;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            this.hidRecordCount.Value = "";
        }
        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();

        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10); 
    }
    protected void Show_Click(Object sender,EventArgs e) 
    {
        showListByACAdaptorList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true);
    }

    protected void Save_Click(Object sender,EventArgs e)
    {
        bool insertFlag = false;
        try 
        {
            string pc = GetLocationIP();
            insertFlag=ICoaReceiving.saveItemIntoCOAReceiveTable(pc);
            showListByACAdaptorList();
            this.updatePanel2.Update();
            ClearLabel();
        }
        catch(FisException fe)
        {
            showErrorMessage(fe.mErrmsg);
        }
        catch(Exception ee)
        {
            showErrorMessage(ee.Message);
        }
        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true); 
        if (insertFlag)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "shwoSuccess", "showSuccess();", true); 
        }
        
    }

    protected void Clear_Click(Object sender,EventArgs e) 
    {
        try 
        {
            string pc=GetLocationIP();
            ICoaReceiving.RemoveTmpTableItem(pc);
            //clear label
            ClearLabel();


            
            showListByACAdaptorList();
            this.updatePanel2.Update();
        }
        catch(FisException fe)
        {
            showErrorMessage(fe.mErrmsg);
        }
        catch(Exception ee)
        {
            showErrorMessage(ee.Message);
            
        }
        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true);
    }

    private void ClearLabel()
    {
        lblPOValue.Text = String.Empty;

        lblShippingDateVal.Text = String.Empty;

        lblCustPNVal.Text = String.Empty;

        lblIecPNVal.Text = String.Empty;
        lblDescriptionVal.Text = String.Empty;
        this.updatePanel1.Update();
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
       
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
       
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
       
        return errorMsg;
    }
    private Boolean showListByACAdaptorList()
    {
        
        IList<COAReceivingDef> dataLst = null;
        try
        {
            string pc = GetLocationIP();
            dataLst = ICoaReceiving.GetTmpTableItem(pc);

            if (dataLst == null || dataLst.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataLst, DEFAULT_ROWS);
            }
        }
        catch (FisException fex)
        {

            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(fex.mErrmsg);
            return false;
        }
        catch (System.Exception ex)
        {

            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }
    private string GetLocationIP()
    {
        string ip = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
        return ip;
    }
}
