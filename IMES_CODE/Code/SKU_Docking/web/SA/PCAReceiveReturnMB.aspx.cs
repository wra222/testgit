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

public partial class SA_PCAReceiveReturnMB : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public string line = "";

    //IPCARepairInput iPCARepairInput = ServiceAgent.getInstance().GetObjectByName<IPCARepairInput>(WebConstant.PCARepairInputObject);
    IPCAReceiveReturnMB iPCAReceiveReturnMB = ServiceAgent.getInstance().GetObjectByName<IPCAReceiveReturnMB>(WebConstant.PCAReceiveReturnMBObject);
    IMB iMB = ServiceAgent.getInstance().GetObjectByName<IMB>(WebConstant.CommonObject);
    IRepair iRepair = ServiceAgent.getInstance().GetObjectByName<IRepair>(WebConstant.CommonObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
          
        {
            //cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            btnProcess.ServerClick += new EventHandler(btnProcess_ServerClick);
            btnSave.ServerClick += new EventHandler(btnSave_ServerClick);
            btnExit.ServerClick += new EventHandler(btnExit_ServerClick);
            //cmbPdLine.InnerDropDownList.AutoPostBack = true;
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {
        
                InitLabel();
                clearGrid();

                //this.cmbPdLine.Station = Request["Station"];
                //this.cmbPdLine.Customer = Master.userInfo.Customer;
                //this.cmbPdLine.Stage = "SA";

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.hidStation.Value = Request["Station"];
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

    ///// <summary>
    ///// Update UI while PdLine changed.
    ///// </summary>
    ///// <param name="er"></param>
    //private void cmbPdLine_Selected(object sender, System.EventArgs e)
    //{
    //    setMBInfo("", "", "");
    //    hidInput.Value = "";
    //    showInfo("");
    //    clearGrid();
    //    callNextInput();
    //}

    private void btnExit_ServerClick(object sender, System.EventArgs e)
    {
        iPCAReceiveReturnMB.Cancel(hidInput.Value);
    }

    private void btnSave_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string ret = iPCAReceiveReturnMB.Save(this.hidInput.Value, this.hidIsBU.Value,this.hidRework.Value,this.hidLookLike.Value);
            //showInfo(ret);
            if (this.hidFlagSound.Value == "Y")
            {
                playSound();
            }
            else
            {
                noPlaySound();
            }
            reSet();
        }
        catch (FisException ee)
        {
            reSet();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            reSet();
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
            //setMBInfo("", "", "");
            clearGrid();
            callNextInput();
        }
    }

    private void btnProcess_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            //setMBInfo("", "", "");
            clearGrid();

            IList<PCAReceiveReturnMBInfo> retList = iPCAReceiveReturnMB.InputMBSno(hidInput.Value, line, UserId, hidStation.Value, Customer);
            
            //MBInfo info = iMB.GetMBInfo(hidInput.Value);
            //setMBInfo(retList[0].MBSN, "", "");
            
            DataTable dt = initTable();
            DataRow newRow;
            int cnt = 0;
            //IList<RepairInfo> repList = iRepair.GetMBRepairList(hidInput.Value);
            //if (repList == null || repList.Count == 0)
            //{
            //    iPCAReceiveReturnMB.Cancel(hidInput.Value);
            //    showInfo(this.GetLocalResourceObject(Pre + "_msgNoLog").ToString());
            //    return;
            //}

            foreach (PCAReceiveReturnMBInfo ele in retList)
            {
                newRow = dt.NewRow();
                newRow["MBSN"] += ele.MBSN;
                newRow["Defcet"] += ele.Defcet + " " + ele.DefcetDescr;
                newRow["Line"] += ele.Line;
                newRow["Remark"] += ele.Remark;
                newRow["Cdt"] += ele.Cdt.ToShortDateString();
                dt.Rows.Add(newRow);
                cnt++;
            }

            for (; cnt < initRowsCount; cnt++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            this.GridViewExt1.DataSource = dt;
            this.GridViewExt1.DataBind();
            initTableColumnHeader();
            gridViewUP.Update();

            //iPCAReceiveReturnMB.Save(hidInput.Value);
            //playSound();
        }
        catch (FisException ee)
        {
            reSet();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            reSet();
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();

            /*
             * Answer to: ITC-1360-0201
             * Description: Refresh page after save.
             */
            //setMBInfo("", "", "");
            //clearGrid();
            callNextInput();
        }
    }

    private void InitLabel()
    {        
        this.lblRework.Text = this.GetLocalResourceObject(Pre + "_lblRework").ToString();
        this.lblMBLookLike.Text = this.GetLocalResourceObject(Pre + "_lblMBLookLike").ToString();
        this.chkMusic.Text = this.GetLocalResourceObject(Pre + "_lblMusicOn").ToString();
        this.lblMBSno.Text = this.GetLocalResourceObject(Pre + "_lblMBSno").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblTableTitle.Text = this.GetLocalResourceObject(Pre + "_lblTableTitle").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
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
        retTable.Columns.Add("MBSN", Type.GetType("System.String"));//0
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Defcet", Type.GetType("System.String"));
        retTable.Columns.Add("Cause", Type.GetType("System.String"));
        retTable.Columns.Add("Cover", Type.GetType("System.String"));
        retTable.Columns.Add("ICT", Type.GetType("System.String"));
        retTable.Columns.Add("ICTOpt", Type.GetType("System.String"));
        retTable.Columns.Add("Fun", Type.GetType("System.String"));
        retTable.Columns.Add("FunOpt", Type.GetType("System.String"));
        retTable.Columns.Add("Remark", Type.GetType("System.String"));//9
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));//10
        return retTable;
    }

    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = "MBSN";
        this.GridViewExt1.HeaderRow.Cells[1].Text = "Line";
        this.GridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titleDefect").ToString();
        this.GridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titleCause").ToString();
        this.GridViewExt1.HeaderRow.Cells[4].Text = "Cover";
        this.GridViewExt1.HeaderRow.Cells[5].Text = "ICT";
        this.GridViewExt1.HeaderRow.Cells[6].Text = "ICTOpt";
        this.GridViewExt1.HeaderRow.Cells[7].Text = "Fun";
        this.GridViewExt1.HeaderRow.Cells[8].Text = "FunOpt";
        this.GridViewExt1.HeaderRow.Cells[9].Text = "Remark";
        this.GridViewExt1.HeaderRow.Cells[10].Text = "Cdt";
        //this.GridViewExt1.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titlePdLine").ToString();
        //this.GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleTestStn").ToString();
        //this.GridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titleDefect").ToString();
        //this.GridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titleCause").ToString();
        //this.GridViewExt1.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_titleCDate").ToString();
        //this.GridViewExt1.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_titleUDate").ToString();

        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(65);
        this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(65);
        this.GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(65);
        this.GridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(65);
        this.GridViewExt1.HeaderRow.Cells[6].Width = Unit.Pixel(65);
        this.GridViewExt1.HeaderRow.Cells[7].Width = Unit.Pixel(65);
        this.GridViewExt1.HeaderRow.Cells[8].Width = Unit.Pixel(65);
        this.GridViewExt1.HeaderRow.Cells[9].Width = Unit.Pixel(65);
        this.GridViewExt1.HeaderRow.Cells[10].Width = Unit.Pixel(65);
    }

    //private void setMBInfo(string id, string family, string model)
    //{
    //    try
    //    {
    //        this.txtMBSno.Text = id;
    //        this.UpdatePanel1.Update();
    //        //this.txtFamily.Text = family;
    //        //this.UpdatePanel2.Update();
    //        this.txtModel.Text = model;
    //        this.UpdatePanel3.Update();
    //    }

    //    catch (FisException ee)
    //    {
    //        writeToAlertMessage(ee.mErrmsg);
    //    }
    //    catch (Exception ex)
    //    {
    //        writeToAlertMessage(ex.Message);
    //    }
    //}

    private void clearGrid()
    {
        try
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            initTableColumnHeader();
            gridViewUP.Update();
            //reSet();
        }
        catch (FisException ee)
        {
            reSet();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            reSet();
            writeToAlertMessage(ex.Message);
        }
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        /*
         * Answer to: ITC-1360-0199
         * Description: Play or donot play sound according to user's choise.
         */
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\", flagSound);");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }



    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "jsAlert", script, false);
    }

    private void playSound()
    {
        String script = "<script language='javascript'> playSound(); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "playSound", script, false);
    }

    private void noPlaySound()
    {
        String script = "<script language='javascript'> noplaySound(); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "noplaySound", script, false);
    }

    private void reSet()
    {
        String script = "<script language='javascript'> Reset(); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "Reset", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }

    private void callNextInput()
    {
        String script = "<script language='javascript'>callNextInput();</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callNextInput", script, false);
    }
}
