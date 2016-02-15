/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PoData(Delete for PL user) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PO Data.docx –2011/11/08 
 * UC:CI-MES12-SPEC-PAK-UC PO Data.docx –2011/11/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-09  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
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
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;



public partial class PAK_PoData_DeleteOB : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;

    IPoData iPoData = ServiceAgent.getInstance().GetObjectByName<IPoData>(WebConstant.PoDataObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnQueryData.ServerClick += new EventHandler(btnQueryData_ServerClick);
            btnDeleteDN.ServerClick += new EventHandler(btnDeleteDN_Click);
            btnDeleteBatchDN.ServerClick += new EventHandler(btnDeleteBatchDN_Click);

            UserId = Master.userInfo.UserId;
            if (!Page.IsPostBack)
            {        
                InitLabel();
                clearGrid();
                hidGUID.Value = Guid.NewGuid().ToString().Replace("-", "");
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

    private void btnDeleteDN_Click(object sender, System.EventArgs e)
    {
        try
        {
            iPoData.DeleteOBDN(hidSelectedDN.Value, UserId);
            jsAlert(GetLocalResourceObject(Pre + "_msgDeleteSuccess").ToString());
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
            reQuery();
        }
    }

    private void btnDeleteBatchDN_Click(object sender, System.EventArgs e)
    {
        try
        {
            string dtsFtpPath = ConfigurationManager.AppSettings["DTSFtpPath"].ToString();
            if (!dtsFtpPath.EndsWith("\\"))
            {
                dtsFtpPath = dtsFtpPath + "\\";
            }
            string fn = dtsFtpPath + hidGUID.Value + ".TXT";
            //string fn = "C:\\EFIS-Workdir\\itc202017-Zhuangqinping\\files\\xxx1.TXT";
            StreamReader m_streamReader = new StreamReader(fn, System.Text.Encoding.GetEncoding("GB2312"));//设定读写的编码
            if (m_streamReader == null)
            {
                writeToAlertMessage(GetLocalResourceObject(Pre + "_msgCheckFTP").ToString());
                return;
            }
            //使用StreamReader类来读取文件
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            IList<string> dnLines = new List<string>();
            //从数据流中读取每一行，直到文件的最后一行
            string strLineDN = m_streamReader.ReadLine();
            while (strLineDN != null)
            {
                if (strLineDN.Trim() != "") dnLines.Add(strLineDN.Trim());
                strLineDN = m_streamReader.ReadLine();
            }
            //关闭此StreamReader对象
            m_streamReader.Close();
            File.Delete(fn);

            if (dnLines.Count == 0)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgNoData").ToString());
                return;
            }

            IList<string> fList;
            iPoData.BatchDeleteOBDN(dnLines, UserId, out fList);
            if (fList.Count == 0)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgDeleteSuccess").ToString());
            }
            else if (dnLines.Count != fList.Count)
            {
                string errmsg = GetLocalResourceObject(Pre + "_msgFailedDN").ToString();
                foreach (string str in fList)
                {
                    errmsg += "[" + str + "],";
                }
                jsAlert(errmsg.Substring(0, errmsg.Length - 1));
                showInfo(errmsg.Substring(0, errmsg.Length - 1));
            }
            else
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgAllFailed").ToString());
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
        finally
        {
            endWaitingCoverDiv();
        }
    }

    private void btnQueryData_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            clearGrid();

            string input = txtInput.Text.Trim();
            if (input == null || input == "") {
                jsAlert(GetLocalResourceObject(Pre + "_msgNoInput").ToString());
                SetFocus(txtInput.ClientID);
                return;
            }

            int recordCount = 0;

            IList<VPakComnInfo> dnList = iPoData.QueryOBData(input.ToUpper(), out recordCount);

            if (dnList.Count == 0)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgNoRecord").ToString());
                return;
            }

            if (recordCount > 1000)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgTooManyRecords").ToString());
            }

            DataTable dt = initTable();
            DataRow newRow;
            int cnt = 0;
            if (dnList != null)
            {
                foreach (VPakComnInfo ele in dnList)
                {
                    newRow = dt.NewRow();
                    newRow["DN"] += ele.internalID;
                    newRow["ShipNo"] += ele.consol_invoice;
                    newRow["WaybillNo"] += ele.waybill_number;
                    newRow["Carrier"] += ele.intl_carrier;
                    dt.Rows.Add(newRow);
                    cnt++;
                }
            }

            for (; cnt < initRowsCount; cnt++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            this.gve1.DataSource = dt;
            this.gve1.DataBind();
            initTableColumnHeader();
            up1.Update();
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
            endWaitingCoverDiv();
        }
    }

    private void InitLabel()
    {
        lblInput.Text = GetLocalResourceObject(Pre + "_lblInput").ToString();
        lblInputTip.Text = GetLocalResourceObject(Pre + "_tipInput").ToString();
        lblTableTitle.Text = GetLocalResourceObject(Pre + "_lblTableTitle").ToString();
        lblDNFile.Text = GetLocalResourceObject(Pre + "_lblDNFile").ToString();
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

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
        retTable.Columns.Add("DN", Type.GetType("System.String"));
        retTable.Columns.Add("ShipNo", Type.GetType("System.String"));
        retTable.Columns.Add("WaybillNo", Type.GetType("System.String"));
        retTable.Columns.Add("Carrier", Type.GetType("System.String"));
        return retTable;
    }

    private void initTableColumnHeader()
    {
        gve1.HeaderRow.Cells[0].Text = GetLocalResourceObject(Pre + "_titleDN").ToString();
        gve1.HeaderRow.Cells[1].Text = GetLocalResourceObject(Pre + "_titleShipNo").ToString();
        gve1.HeaderRow.Cells[2].Text = GetLocalResourceObject(Pre + "_titleWaybillNo").ToString();
        gve1.HeaderRow.Cells[3].Text = GetLocalResourceObject(Pre + "_titleCarrier").ToString();
        gve1.HeaderRow.Cells[0].Width = Unit.Pixel(125);
        gve1.HeaderRow.Cells[1].Width = Unit.Pixel(125);
        gve1.HeaderRow.Cells[2].Width = Unit.Pixel(125);
        gve1.HeaderRow.Cells[3].Width = Unit.Pixel(125);
    }

    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            clearGrid();
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

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "jsAlert", script, false);
    }

    private void reQuery()
    {
        String script = "<script language='javascript'>\n"
                        + "document.getElementById('" + hidSelectedDN.ClientID + "').value = \"\";\n"
                        + "document.getElementById('" + btnQueryData.ClientID + "').click();\n"
                        + "</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "reQuery", script, false);
    }
   
    private void clearGrid()
    {
        try
        {
            gve1.DataSource = getNullDataTable();
            gve1.DataBind();
            up1.Update();
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

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
}
