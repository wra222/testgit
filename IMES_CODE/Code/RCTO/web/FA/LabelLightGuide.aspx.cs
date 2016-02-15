/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for LabelLightGuide Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Label Light Guide.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Label Light Guide.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-19  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Data;
using System.Web.Services;
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
using System.Globalization;

public partial class FA_LabelLightGuide : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    ILabelLightGuide iLabelLightGuide = ServiceAgent.getInstance().GetObjectByName<ILabelLightGuide>(WebConstant.LabelLightGuideObject);

    public int initRowsCount = 6;

    /*
     * Answer to: ITC-1360-0578
     * Description: Make com setting configurable.
    */
    [WebMethod]
    public static IList<string> getCommSetting(string hName, string editor)
    {
        IList<string> ret = new List<string>();
        try
        {
            IList<COMSettingInfo> wsiList = ServiceAgent.getInstance().GetObjectByName<ILabelLightGuide>(WebConstant.LabelLightGuideObject).getCommSetting(hName, editor);

            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(wsiList[0].commPort.ToString());
            ret.Add(wsiList[0].baudRate);
            ret.Add(wsiList[0].rthreshold.ToString());
            ret.Add(wsiList[0].sthreshold.ToString());
            ret.Add(wsiList[0].handshaking.ToString());
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            cmbLC.InnerDropDownList.AutoPostBack = true;
            chkQuery.AutoPostBack = true;
            btnGridFresh.ServerClick += new EventHandler(btnGridFresh_ServerClick);
            btnExit.ServerClick += new EventHandler(btnExit_ServerClick);
            cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_SelectedIndexChanged);
            cmbLC.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_SelectedIndexChanged);
            chkQuery.CheckedChanged += new EventHandler(chkQuery_CheckedChanged);
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {
                InitLabel();
                clearGrid();

                this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                this.cmbPdLine.Stage = "FA";
                this.hidStation.Value = Request["Station"];
                this.upHidden.Update();
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
    
    private void chkQuery_CheckedChanged(object sender, System.EventArgs e)
    {
        callNextInput();
    }

    private void cmbPdLine_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        this.upHidden.Update();
        showInfo("");
        clearGrid();
        callNextInput();
    }

    private void btnExit_ServerClick(object sender, System.EventArgs e)
    {
        iLabelLightGuide.Cancel(hidProdId.Value);
    }

    private void btnGridFresh_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            clearGrid();
            ProductInfo pInfo = iLabelLightGuide.GetProductInfo(hidProdId.Value, cmbPdLine.InnerDropDownList.SelectedValue, UserId, hidStation.Value, Customer, chkQuery.Checked);
            setProdInfo(pInfo.id, pInfo.modelId, pInfo.customSN);

            string code = cmbLC.InnerDropDownList.SelectedValue;

            IList<WipBuffer> wbList = iLabelLightGuide.getBomData(pInfo.modelId, code);
            DataTable dt = initTable();
            DataRow newRow;

            IList<string> lnList = new List<string>();
            int cnt = 0;
            foreach (WipBuffer ele in wbList)
            {
                lnList.Add(ele.LightNo);
                newRow = dt.NewRow();
                newRow["PartNo"] += ele.PartNo;
                newRow["Type"] += ele.Tp;
                newRow["LightNo"] += ele.LightNo;
                newRow["Qty"] += ele.Qty.ToString();
                dt.Rows.Add(newRow);
                cnt++;
            }
            setLight(lnList);

            for (; cnt < initRowsCount; cnt++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            this.GridViewExt1.DataSource = dt;
            this.GridViewExt1.DataBind();
            initTableColumnHeader();
            gridViewUP.Update();
            transLight();
            iLabelLightGuide.Save(hidProdId.Value);
        }
        catch (FisException ee)
        {
            clearGrid();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            this.upHidden.Update();
            endWaitingCoverDiv();
            this.chkQuery.Checked = false;
            upChkQuery.Update();
            callNextInput();
        }
    }

    private void InitLabel()
    {
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblProId.Text = this.GetLocalResourceObject(Pre + "_lblProId").ToString();
        this.lblLC.Text = this.GetLocalResourceObject(Pre + "_lblLC").ToString();
        this.lblCPQS.Text = this.GetLocalResourceObject(Pre + "_lblCPQS").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblTableTitle.Text = this.GetLocalResourceObject(Pre + "_lblTableTitle").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.chkQuery.Text = this.GetLocalResourceObject(Pre + "_chkQuery").ToString();

        setPdLineCombFocus();
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
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        retTable.Columns.Add("Type", Type.GetType("System.String"));
        retTable.Columns.Add("LightNo", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        return retTable;
    }

    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titlePartNo").ToString();
        this.GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleType").ToString();
        this.GridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titleLightNo").ToString();
        this.GridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titleQty").ToString();
        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(200);
        this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        this.GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(150);
        this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(50);
    }

    private void clearGrid()
    {
        try
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            this.gridViewUP.Update();
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

    private void setProdInfo(string id, string model, string cpqs)
    {
        try
        {
            this.txtProId.Text = id;
            this.upProId.Update();
            this.txtModel.Text = model;
            this.upModel.Update();
            this.txtCPQS.Text = cpqs;
            this.upCPQS.Update();
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

    void setLight(IList<string> lnList)
    {

        IList<int> lightList = new List<int>();
        for (int i = 0; i < 192; i++)
        {
            lightList.Add(0);
        }

        for (int j = 0; j < lnList.Count; j++)
        {
            int temp;
            if (int.TryParse(lnList[j], out temp))
            {
                lightList[temp-1] = 1;
            }
        }


        string result = "";
        for (int k = 0; k < 24; k++)
        {
            int tempLight8 = 0;
            for (int m = 0; m < 8; m++)
            {
                tempLight8 = tempLight8 + (int)(lightList[k * 8 + m] * Math.Pow(2d, (double)m));
            }
            result = result + "," + tempLight8;
        }
        result = result.TrimStart(new char[] { ',' });

        hidData2Send.Value = result;
        return;
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private void callNextInput()
    {

        String script = "<script language='javascript'>callNextInput(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callNextInput", script, false);
    }

    private void showInfo(string info)
    {

        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void setPdLineCombFocus()
    {

        String script = "<script language='javascript'>  setPdLineCmbFocus(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }

    private void transLight()
    {
        String script = "<script language='javascript'>transLight();</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "transLight", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
}
