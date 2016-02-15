/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for PoData (for docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PoData to IMES for Docking.docx
 * UC:CI-MES12-SPEC-PAK-UC PoData to IMES for Docking.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-06  itc202017             (Reference Ebook SourceCode) Create
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
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;
using System.IO;

public partial class PAK_PoData : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    IPoData iPoData = ServiceAgent.getInstance().GetObjectByName<IPoData>(WebConstant.PoDataForDockingObject);

    public int initRowsCount = 6;

    [WebMethod]
    public static ArrayList print(string customer, IList<PrintItem> printItems)
    {
        ArrayList ret = new ArrayList();

        try
        {
            IList<PrintItem> retLst = ServiceAgent.getInstance().GetObjectByName<IPoData>(WebConstant.PoDataForDockingObject).GetPrintTemplate(customer, printItems);
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst);
            return ret;
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnUploadData.ServerClick += new EventHandler(btnUploadData_ServerClick);
            btnQueryData.ServerClick += new EventHandler(btnQueryData_ServerClick);
            btnShowDetail.ServerClick += new EventHandler(btnShowDetail_ServerClick);

            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;

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

    private void btnUploadData_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string dtsFtpPath = ConfigurationManager.AppSettings["DTSFtpPath"].ToString();
            if (!dtsFtpPath.EndsWith("\\"))
            {
                dtsFtpPath = dtsFtpPath + "\\";
            }
            string fn1 = dtsFtpPath + hidGUID.Value + "COMN.TXT";
            string fn2 = dtsFtpPath + hidGUID.Value + "PALTNO.TXT";
            //string fn1 = "E:\\EFIS-Workdir\\itc202017-Zhuangqinping\\files\\COMN.TXT";
            //string fn2 = "E:\\EFIS-Workdir\\itc202017-Zhuangqinping\\files\\PALTNO.TXT";
            uploadData(fn1, fn2);
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

    private void btnQueryData_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string dn = txtDN.Text.Trim();
            string pn = txtPO.Text.Trim();
            string model = txtModel.Text.Trim();
            string from = hidDateFrom.Value;
            string to = hidDateTo.Value;
            string dninfo = txtDNInfo.Text.Trim();
            if ((dninfo == null || dninfo == "")
                && (dn == null || dn == "")
                && (pn == null || pn == "")
                && (model == null || model == "")
                && (from == null || from == "")
                && (to == null || to == ""))
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgNoInput").ToString());
                SetFocus(txtDN.ClientID);
                return;
            }

            if ((from == null || from == "") && (to != null && to != ""))
            {
                from = to;
            }

            if ((from != null && from != "") && (to == null || to == ""))
            {
                to = from;
            }

            DNQueryCondition cond = new DNQueryCondition();
            cond.DNInfoValue = dninfo;
            cond.DeliveryNo = dn.ToUpper();
            cond.PONo = pn.ToUpper();
            cond.Model = model.ToUpper();
            if ((from != null && from != "") && (to != null && to != ""))
            {
                DateTime t1 = Convert.ToDateTime(from);// new DateTime(int.Parse(from.Substring(0, 4)), int.Parse(from.Substring(5, 2)), int.Parse(from.Substring(8, 2)));
                DateTime t2 = Convert.ToDateTime(to);// new DateTime(int.Parse(to.Substring(0, 4)), int.Parse(to.Substring(5, 2)), int.Parse(to.Substring(8, 2)));
                if (t1 > t2)
                {
                    jsAlert(GetLocalResourceObject(Pre + "_msgBadDate").ToString());
                    clickDateFrom();
                    return;
                }
                cond.ShipDateFrom = t1;
                cond.ShipDateTo = t2;
            }

            clearGrid();

            int recordCount = 0;
            long sum = 0;
            IList<DNForUI> dnList = iPoData.QueryData(cond, out recordCount, out sum);

            if (dnList.Count == 0)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgNoRecord").ToString());
                return;
            }

            if (recordCount > 1000)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgTooManyRecords").ToString());
            }

            showTableData(dnList, sum);
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

    private void showTableData(IList<DNForUI> data, long sum)
    {
        DataTable dt = initTable1();
        DataRow newRow;
        int cnt = 0;
        if (data != null)
        {
            foreach (DNForUI ele in data)
            {
                newRow = dt.NewRow();
                newRow["DN"] += ele.DeliveryNo;
                newRow["ShipNo"] += ele.ShipmentID;
                newRow["PoNo"] += ele.PoNo;
                newRow["Model"] += ele.ModelName;
                newRow["Qty"] += ele.Qty.ToString();
                newRow["ShipDate"] += ele.ShipDate.ToString("yyyy-MM-dd");
                newRow["Status"] += ele.Status;
                newRow["CDate"] += ele.Cdt.ToString("yyyy-MM-dd");
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
        initTableColumnHeader1();
        up1.Update();
        this.txtTotal.Text = sum.ToString();
        UpdatePanel1.Update();
        return;
    }

    private void btnShowDetail_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            clearGrid2();
            clearGrid3();
            string dn = hidSelectedDN.Value;
            if (dn == "")
            {
                return;
            }

            IList<DNInfoForUI> infoList = iPoData.GetDNInfoList(dn);

            DataTable dt = initTable2();
            DataRow newRow;
            int cnt = 0;
            if (infoList != null)
            {
                foreach (DNInfoForUI ele in infoList)
                {
                    if (ele.InfoValue != "")
                    {
                        newRow = dt.NewRow();
                        newRow["AttrName"] += ele.InfoType;
                        newRow["AttrValue"] += ele.InfoValue;
                        dt.Rows.Add(newRow);
                        cnt++;
                    }
                }
            }

            for (; cnt < initRowsCount; cnt++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            this.gve2.DataSource = dt;
            this.gve2.DataBind();
            initTableColumnHeader2();
            up2.Update();

            IList<DNPalletQty> palletList = iPoData.GetDNPalletList(dn);

            dt = initTable3();
            cnt = 0;
            if (palletList != null)
            {
                foreach (DNPalletQty ele in palletList)
                {
                    newRow = dt.NewRow();
                    newRow["PNo"] += ele.PalletNo;
                    newRow["UCC"] += ele.UCC;
                    newRow["Qty"] += ele.DeliveryQty.ToString();
                    dt.Rows.Add(newRow);
                    cnt++;
                }
            }

            for (; cnt < initRowsCount; cnt++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            this.gve3.DataSource = dt;
            this.gve3.DataBind();
            initTableColumnHeader3();
            up3.Update();
        }
        catch (FisException ee)
        {
            clearGrid2();
            clearGrid3();
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

	class Encoding_Result
	{
		public string Encoding;
		public int TextLength;
	}
	static string[] Encoding_SupportedEncodings = new string[] {
		"GB18030",
		"big5",
		"gb2312",
		"utf-16",
		"utf-7",
		"utf-8"
	};
	static string Encoding_GuessEncoding(string path)
	{
		using (Stream fs = File.Open(path, FileMode.Open))
		{
			if (fs.Length <= 0)
			{
				throw new Exception (
					string.Format("File({0}) is empty.", path));
			}

			byte[] rawData = new byte[fs.Length];
			fs.Read(rawData, 0, rawData.Length);
            fs.Close();

			List<Encoding_Result> al = new List<Encoding_Result>();
			foreach (string enc in Encoding_SupportedEncodings)
			{
				try
				{
					Encoding encoding = Encoding.GetEncoding(enc,
						new EncoderExceptionFallback(), new DecoderExceptionFallback());

					string text = encoding.GetString(rawData);
					char[] char2 = text.ToCharArray();

					for (int j = 0; j < char2.Length; ++j)
					{
						System.Globalization.UnicodeCategory uc = Char.GetUnicodeCategory(char2[j]);
                        if (uc == System.Globalization.UnicodeCategory.PrivateUse)
                            throw new Exception(string.Format("Fail to identify file({0}).", path));
					}

					al.Add(new Encoding_Result() { Encoding = enc, TextLength = text.Length });
				}
				catch (Exception ex)
				{
				}
			}

			if (al.Count > 0)
			{
				return al[0].Encoding;
			}
		}

		throw new Exception(string.Format("Fail to determine encoding of file({0}).", path));
	}
    private void uploadData(string dnFn, string pnFn)
    {
        try
        {
            clearGrid();
            //StreamReader m_streamReaderDN = new StreamReader(dnFn, Encoding.GetEncoding("GB18030"), true);//设定读写的编码
            //StreamReader m_streamReaderPN = new StreamReader(pnFn, Encoding.GetEncoding("GB18030"), true);//设定读写的编码
            StreamReader m_streamReaderDN = new StreamReader(dnFn, Encoding.GetEncoding(Encoding_GuessEncoding(dnFn)), true);//设定读写的编码
            StreamReader m_streamReaderPN = new StreamReader(pnFn, Encoding.GetEncoding(Encoding_GuessEncoding(pnFn)), true);//设定读写的编码

            if (m_streamReaderDN == null || m_streamReaderPN == null)
            {
                writeToAlertMessage(GetLocalResourceObject(Pre + "_msgCheckFTP").ToString());
                return;
            }

            //使用StreamReader类来读取文件
            m_streamReaderDN.BaseStream.Seek(0, SeekOrigin.Begin);
            m_streamReaderPN.BaseStream.Seek(0, SeekOrigin.Begin);

            IList<string> dnLines = new List<string>();
            //从数据流中读取每一行，直到文件的最后一行
            string strLineDN = m_streamReaderDN.ReadLine();
            while (strLineDN != null)
            {
                if (strLineDN.Trim() != "") dnLines.Add(strLineDN);
                strLineDN = m_streamReaderDN.ReadLine();
            }

            IList<string> pnLines = new List<string>();
            //从数据流中读取每一行，直到文件的最后一行
            string strLinePN = m_streamReaderPN.ReadLine();
            while (strLinePN != null)
            {
                if (strLinePN.Trim() != "") pnLines.Add(strLinePN);
                strLinePN = m_streamReaderPN.ReadLine();
            }
            //关闭此StreamReader对象
            m_streamReaderDN.Close();
            m_streamReaderPN.Close();

            long sum = 0;
            IList<DNForUI> dnList = iPoData.UploadData(dnLines, pnLines, UserId, out sum);

            if (dnList.Count == 0)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgNoRecordUpload").ToString());
                return;
            }

            showTableData(dnList, sum);
            jsAlert(GetLocalResourceObject(Pre + "_msgUploadSuccess").ToString());

            return;
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            string errmsg = ex.Message.Replace("\"", "'").Replace("\\", "\\\\");
            writeToAlertMessage(errmsg);
        }
        finally
        {
            File.Delete(dnFn);
            File.Delete(pnFn);
        }
    }

    private void InitLabel()
    {
        lgUpload.InnerText = GetLocalResourceObject(Pre + "_fsUpload").ToString();
        lblFile.Text = GetLocalResourceObject(Pre + "_lblFile").ToString();

        lgQuery.InnerText = GetLocalResourceObject(Pre + "_fsQuery").ToString();
        lblDN.Text = GetLocalResourceObject(Pre + "_lblDN").ToString();
        lblPO.Text = GetLocalResourceObject(Pre + "_lblPO").ToString();
        lblFrom.Text = GetLocalResourceObject(Pre + "_lblFrom").ToString();
        lblTo.Text = GetLocalResourceObject(Pre + "_lblTo").ToString();
        lblModel.Text = GetLocalResourceObject(Pre + "_lblModel").ToString();
        lblDNInfo.Text = GetLocalResourceObject(Pre + "_lblDNInfo").ToString();

        lblDNList.Text = GetLocalResourceObject(Pre + "_lblDNList").ToString();
        lblTotal.Text = GetLocalResourceObject(Pre + "_lblTotal").ToString();
        lblDNInfoList.Text = GetLocalResourceObject(Pre + "_lblDNInfoList").ToString();
        lblPalletList.Text = GetLocalResourceObject(Pre + "_lblPalletList").ToString();
        lblPallet.Text = GetLocalResourceObject(Pre + "_lblPallet").ToString();
        lblPQty.Text = GetLocalResourceObject(Pre + "_lblPQty").ToString();
    }

    private DataTable getNullDataTable1()
    {
        DataTable dt = initTable1();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable2()
    {
        DataTable dt = initTable2();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable3()
    {
        DataTable dt = initTable3();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable1()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("DN", Type.GetType("System.String"));
        retTable.Columns.Add("ShipNo", Type.GetType("System.String"));
        retTable.Columns.Add("PoNo", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("ShipDate", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("CDate", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable2()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("AttrName", Type.GetType("System.String"));
        retTable.Columns.Add("AttrValue", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable3()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("PNo", Type.GetType("System.String"));
        retTable.Columns.Add("UCC", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        return retTable;
    }

    private void initTableColumnHeader1()
    {
        gve1.HeaderRow.Cells[0].Text = GetLocalResourceObject(Pre + "_titleDN").ToString();
        gve1.HeaderRow.Cells[1].Text = GetLocalResourceObject(Pre + "_titleShipNo").ToString();
        gve1.HeaderRow.Cells[2].Text = GetLocalResourceObject(Pre + "_titlePoNo").ToString();
        gve1.HeaderRow.Cells[3].Text = GetLocalResourceObject(Pre + "_titleModel").ToString();
        gve1.HeaderRow.Cells[4].Text = GetLocalResourceObject(Pre + "_titleSDate").ToString();
        gve1.HeaderRow.Cells[5].Text = GetLocalResourceObject(Pre + "_titleQty").ToString();
        gve1.HeaderRow.Cells[6].Text = GetLocalResourceObject(Pre + "_titleStatus").ToString();
        gve1.HeaderRow.Cells[7].Text = GetLocalResourceObject(Pre + "_titleCDate").ToString();
        gve1.HeaderRow.Cells[0].Width = Unit.Pixel(80);
        gve1.HeaderRow.Cells[1].Width = Unit.Pixel(80);
        gve1.HeaderRow.Cells[2].Width = Unit.Pixel(80);
        gve1.HeaderRow.Cells[3].Width = Unit.Pixel(80);
        gve1.HeaderRow.Cells[4].Width = Unit.Pixel(50);
        gve1.HeaderRow.Cells[5].Width = Unit.Pixel(40);
        gve1.HeaderRow.Cells[6].Width = Unit.Pixel(40);
        gve1.HeaderRow.Cells[7].Width = Unit.Pixel(50);
    }

    private void initTableColumnHeader2()
    {
        gve2.HeaderRow.Cells[0].Text = GetLocalResourceObject(Pre + "_titleAttrName").ToString();
        gve2.HeaderRow.Cells[1].Text = GetLocalResourceObject(Pre + "_titleAttrValue").ToString();
        gve2.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        gve2.HeaderRow.Cells[1].Width = Unit.Pixel(100);
    }

    private void initTableColumnHeader3()
    {
        gve3.HeaderRow.Cells[0].Text = GetLocalResourceObject(Pre + "_titlePNo").ToString();
        gve3.HeaderRow.Cells[1].Text = GetLocalResourceObject(Pre + "_titleUCC").ToString();
        gve3.HeaderRow.Cells[2].Text = GetLocalResourceObject(Pre + "_titleQty").ToString();
        gve3.HeaderRow.Cells[0].Width = Unit.Pixel(80);
        gve3.HeaderRow.Cells[1].Width = Unit.Pixel(80);
        gve3.HeaderRow.Cells[2].Width = Unit.Pixel(40);
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

    private void clearGrid()
    {
        try
        {
            clearGrid1();
            clearGrid2();
            clearGrid3();
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

    private void clearGrid1()
    {
        try
        {
            gve1.DataSource = getNullDataTable1();
            gve1.DataBind();
            up1.Update();
            initTableColumnHeader1();
            txtTotal.Text = "0";
            UpdatePanel1.Update();
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

    private void clearGrid2()
    {
        try
        {
            gve2.DataSource = getNullDataTable2();
            gve2.DataBind();
            up2.Update();
            initTableColumnHeader2();
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

    private void clearGrid3()
    {
        try
        {
            gve3.DataSource = getNullDataTable3();
            gve3.DataBind();
            up3.Update();
            initTableColumnHeader3();
            txtPallet.Text = "";
            UpdatePanel2.Update();
            txtPQty.Text = "0";
            UpdatePanel3.Update();
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

        ScriptManager.RegisterStartupScript(updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void clickDateFrom()
    {
        String script = "<script language='javascript'>document.getElementById('btnFrom').click();</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "clickDateFrom", script, false);
    }
    
    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "jsAlert", script, false);
    }

    private void blockPage()
    {
        String script = "<script language='javascript'> document.getElementById('btnUpload').disabled = true;document.getElementById('btnQuery').disabled = true; </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "blockPage", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
}
