
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
using log4net;
using System.Text;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using System.IO;
public partial class FA_CombineMaterialLot : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICollectionMaterialLot iCollectionMaterialLot = ServiceAgent.getInstance().GetMaintainObjectByName<ICollectionMaterialLot>(WebConstant.CollectionMaterialLotObject);
    public String userId;
    public String customer;
    public String station;
    public int initProductTableRowsCount = 10;

    protected void Page_Load(object sender, EventArgs e)
    {
        drpCollection.Width = Unit.Percentage(Convert.ToDouble(70));
        station = Request["Station"] ?? "";
        hidMaterialTypeID.Value = cmbMaterialType.InnerDropDownList.ClientID;
        hidCollectStageID.Value = cmbCollectStage.InnerDropDownList.ClientID;
        customer = Master.userInfo.Customer;
        userId = Master.userInfo.UserId;
   
        if (!this.IsPostBack)
        {
            InitProductTable();
            InitLabel();
        }
    }
    private void InitProductTable()
    {


        DataTable retTable = new DataTable();
        retTable.Columns.Add("Material CT", Type.GetType("System.String"));
        grvProduct.Columns[0].HeaderText = "Material CT";

        DataRow newRow;
        for (int i = 0; i < initProductTableRowsCount; i++)
        {
            newRow = retTable.NewRow();
            newRow[0] = "";
            retTable.Rows.Add(newRow);
        }
        grvProduct.DataSource = retTable;
        grvProduct.DataBind();
    
    }
    private void InitLabel()
    {
   
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbDataEntry").ToString();

    }


    private void ShowInfo2AndSound(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
     //   scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo2(\"" + errorMsg.Replace("\r\n", "\\n") + "\");PlaySound();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo2(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void writeToAlertMessageAndEndWait(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("endWaitingCoverDiv();ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo2(\"" + errorMsg.Replace("\r\n", "\\n") + "\");getCommonInputObject().focus();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void writeToInfoMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo2(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToInfoMessage", scriptBuilder.ToString(), false);
    }

    private void EndWaitingCoverDiv()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("endWaitingCoverDiv();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "endWaitingCoverDiv", scriptBuilder.ToString(), false);
    }




    private void ResetAll()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getResetAll();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ResetAll", scriptBuilder.ToString(), false);
    }


    private void CallClientFun(string funcName)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine(funcName + "();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "funcName", scriptBuilder.ToString(), false);
    }
    protected void btnCheckCTandLot_Click(object sender, EventArgs e)
    {
        try
        {
            iCollectionMaterialLot.CheckExistCtAndLotNo(hidInputCT.Value,hidLotNo.Value.Trim(),station);
            CallClientFun("AddTable");
        }
        catch (FisException ex)
        {

            writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            //EndWaitingCoverDiv();
        }
    }
    protected void btnCheckCT_Click(object sender, EventArgs e)
    {
        try 
        {
            iCollectionMaterialLot.CheckExistCT(hidInputCT.Value);
            CallClientFun("AddTable");
        }
        catch (FisException ex)
        {
            ShowInfo2AndSound(ex.mErrmsg);
          //  writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            ShowInfo2AndSound(ex.Message);

        //    writeToAlertMessage(ex.Message);
        }
        finally
        {
            //EndWaitingCoverDiv();
        }
    
    }
    protected void btnInputFirstSN_Click(object sender, EventArgs e)
    {
        try
        {
            List<string> rst;
            if (hidCollection.Value == "Input")
            {
                rst = iCollectionMaterialLot.GetMaterialByLot(hidLotNo.Value.Trim(), hidMaterialTypeValue.Value.Trim());
                hidSpecNo.Value = rst[0].ToString();
                hidQty.Value = rst[1].ToString();
                hidComQty.Value = rst[2].ToString();
                CallClientFun("SetInfo");
            }
            else
            {
                 rst = iCollectionMaterialLot.GetLotInfoByCT
                    (hidInputCT.Value.Trim(), hidMaterialTypeValue.Value.Trim(), station, cmbCollectStage.InnerDropDownList.SelectedValue);
                hidSpecNo.Value = rst[0].ToString();
                hidQty.Value = rst[1].ToString();
                hidLotNo.Value = rst[2].ToString();

                hidComQty.Value = rst[3].ToString();
                CallClientFun("SetInfo");
            }
        
        }
        catch (FisException ex)
        {
            if(ex.mErrcode.Trim().Equals("CHK1006"))
           {
               ShowInfo2AndSound(ex.mErrmsg);
            }
            else
            { writeToAlertMessage(ex.mErrmsg); }
    //        writeToAlertMessage(ex.mErrmsg + "----" + ex.mErrcode);
         //   writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            EndWaitingCoverDiv();
        }


    }
    protected void btnSaveByOut_Click(object sender, EventArgs e)
    {
        try
        {
          
            string action = "Collect";
            if (hidCollection.Value == "Output") action = "Return";
            IList<string> lst = new List<string>();
            string[] split = hidCtList.Value.Split(',');
            //foreach (string s in split)
            //{
            //    if (s.Trim().Length == 13)
            //    { lst.Add(s); }
            //}
            iCollectionMaterialLot.UpdateMaterialByCtList(lst, hidCollectStageValue.Value, userId,
                  station, action, "");
            InitProductTable();
            CallClientFun("EndSave");
        }
        catch (FisException ex)
        {

            writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            EndWaitingCoverDiv();
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
         
            IList<string> lst = new List<string>();
            string[] split=hidCtList.Value.Split(',');
            foreach (string s in split)
            {
                if (s.Trim().Length == 13 || s.Trim().Length == 14)
                { lst.Add(s); }
            }
            iCollectionMaterialLot.AddMaterialCtList(lst, hidMaterialTypeValue.Value, hidLotNo.Value, "Collect", "", 
                hidCollectStageValue.Value, "", userId);
            InitProductTable();
            CallClientFun("EndSave");
        }
        catch (FisException ex)
        {

            writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            EndWaitingCoverDiv();
        }


    }
}
