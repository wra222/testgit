/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Unit Weight
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx –2011/11/25
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx –2011/11/25         
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-25   Chen Xu               Create
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
using IMES.Station.Interface.BSamIntf;
//using IMES.FisObject;
//using IMES.FisObject.PAK.Carton;
using System.IO;

public partial class BSam_CartonWeight : System.Web.UI.Page
{
    
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String Station;
    protected string Pcode;
    protected int isTestWeight = WebCommonMethod.isTestWeight();
     public string UnitWeightFilePath = ConfigurationManager.AppSettings["UnitWeightPath"].ToString();
    public string productTableHeader = "Product ID,Custromer SN,Model,Location";
    public int initProductTableRowsCount = 6;
    ICartonWeight iCartonWeight = ServiceAgent.getInstance().GetObjectByName<ICartonWeight>(WebConstant.ICartonWeight);
    public string pcode;
    public String AccountId;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Station = Request["Station"] ?? "";
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            hidIsBsam.Value = Request["IsBSam"] ?? "N";
            pcode = Request["PCode"];
            AccountId = Master.userInfo.AccountId.ToString();
            CheckPrintItem();
            if (hidIsBsam.Value == "N")
            {
                this.txtDataEntry.IsKeepWhitespace = true;
                
            }
            if (!this.IsPostBack)
            {
                initLabel();
                InitProductTable();
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
    private void CheckPrintItem()
    {
        IPrintTemplate iPrintTemplate = ServiceAgent.getInstance().GetObjectByName<IPrintTemplate>(WebConstant.CommonObject);
        IList<string> labelTypeList = iPrintTemplate.GetPrintLabelTypeList(pcode);
        if (labelTypeList != null && labelTypeList.Count != 0)
        {
            btnPrintSet.Visible = true;
            btnReprint.Visible = true;
        }
        else
        {
            btnPrintSet.Visible = false;
            btnReprint.Visible = false;
        }
    }
    private void InitProductTable()
    {

        string[] header = productTableHeader.Split(',');
        DataTable retTable = new DataTable();
        int k = 0;
        foreach (string s in header)
        {
            retTable.Columns.Add(s, Type.GetType("System.String"));
            grvProduct.Columns[k].HeaderText = s;
            k++;
        }
        DataRow newRow;
        for (int i = 0; i < initProductTableRowsCount; i++)
        {
            newRow = retTable.NewRow();
            foreach (string s in header)
            { newRow[s] = String.Empty; }
            retTable.Rows.Add(newRow);
        }
        grvProduct.DataSource = retTable;
        grvProduct.DataBind();
        //  IniGrvWidth();
        //      this.grvDN.Columns[0].HeaderText

    }
    protected void btnInputFirstSN_Click(object sender, EventArgs e)
    {
      ArrayList ret;
      try
      {
          ret = iCartonWeight.InputCustSn(hidCustsn.Value, "", UserId, Station, Customer, hidActWeight.Value);
          hidCartonSN.Value=ret[0].ToString();
          hidBoxID.Value=ret[1].ToString().Trim();
          hidModel.Value=ret[2].ToString();
          if (ret[3] == null)
          {
              hidStdWeight.Value = "0";
          }
          else
          {
              hidStdWeight.Value = ret[3].ToString();
          }
       
          List<S_BSam_Product> sProduct = (List<S_BSam_Product>)ret[4];
          BindPrdGrv(sProduct);
          CallClientFun("SetInfo");
      }
  
      catch (FisException ex)
        {
           writeToAlertMessageAndEndWait(ex.mErrmsg);
           InitProductTable();
        }
        catch (Exception ex)
        {
    
           writeToAlertMessageAndEndWait(ex.Message);
           InitProductTable();
        }
    
    
    }
    private void BindPrdGrv(List<S_BSam_Product> lstPrd)
    {
        string[] header = productTableHeader.Split(',');
        DataTable dt = new DataTable();
        int k = 0;
        foreach (string s in header)
        {
            dt.Columns.Add(s, Type.GetType("System.String"));
            grvProduct.Columns[k].HeaderText = s;
            k++;
        }
        DataRow newRow;
        foreach (S_BSam_Product sPrd in lstPrd) //Product ID,Custromer SN,Model,Location
        {
            newRow = dt.NewRow();
            newRow["Product ID"] = sPrd.ProductID;
            newRow["Custromer SN"] = sPrd.CustomerSN;
            newRow["Model"] = sPrd.Model;
            newRow["Location"] = sPrd.Location;
            dt.Rows.Add(newRow);
        }

        if (lstPrd.Count < initProductTableRowsCount)
        {

            for (int i = 0; i < initProductTableRowsCount - lstPrd.Count; i++)
            {
                newRow = dt.NewRow();
                newRow["Product ID"] = "";
                newRow["Custromer SN"] = "";
                newRow["Model"] =  "";
                newRow["Location"] = "";
                dt.Rows.Add(newRow);

            }

        }
        grvProduct.DataSource = dt;
        grvProduct.DataBind();
        //   IniGrvWidth();


    }
    private void CallClientFun(string funcName)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine(funcName + "();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "funcName", scriptBuilder.ToString(), false);
    }
    private void writeToAlertMessageAndEndWait(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("endWaitingCoverDiv();ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");callNextInput();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    /// <returns></returns>
     private void initLabel()
    {
        this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrtSet").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();

   
        this.lblUnitWeight.Text = this.GetLocalResourceObject(Pre + "_lblUnitWeight").ToString();
        this.lblStdWeight.Text = this.GetLocalResourceObject(Pre + "_lblStdWeight").ToString();

        this.lblCartonNo.Text = this.GetLocalResourceObject(Pre + "_lblCartonNo").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
    
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();

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