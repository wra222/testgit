/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:COA Status Change
 * CI-MES12-SPEC-PAK-COA Status Change.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
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
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using log4net;
using System.Text;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
using System.IO;
public partial class PAK_COAStatusChangeForBuyer : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICOAStatusChange iCOAStatusChange = ServiceAgent.getInstance().GetObjectByName<ICOAStatusChange>(WebConstant.COAStatusChangeObject);
    public String userId;
    public String customer;
    public String station;
    public int initRowsCount = 6;
    public String cmdValue = "";
    public String placeValue = "";
    public string msgWrongCode = "";
    public string msgStatusError = "";
    public string msgPnoError = "";
    public string msgCountError = "";
    public string msgCountFirst = "";


    public string msgInvalidFileType;
    public string msgEmptyFile;
    public string msgFileEmpty;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        btnGetCOAList.ServerClick += new EventHandler(btnGetCOAList_ServerClick);
        btnGetCOAListNoEnter.ServerClick += new EventHandler(btnGetCOAListNoEnter_ServerClick);
        btnUpdateCOAList.ServerClick += new EventHandler(btnUpdateCOAList_ServerClick);
        btnReceiveCOAList.ServerClick += new EventHandler(btnReceiveCOAList_ServerClick);
        btnQueryEarly.ServerClick += new EventHandler(btnQueryEarly_ServerClick);
        msgStatusError = this.GetLocalResourceObject(Pre + "_msgStatusError").ToString();
        msgPnoError = this.GetLocalResourceObject(Pre + "_msgPnoError").ToString();
        msgCountError = this.GetLocalResourceObject(Pre + "_msgCountError").ToString();
        cmdValue = this.GetLocalResourceObject(Pre + "_cmdValue").ToString();
        msgWrongCode = this.GetLocalResourceObject(Pre + "_msgWrongCode").ToString();
        msgCountFirst = this.GetLocalResourceObject(Pre + "_msgCountFirst").ToString();
        placeValue = "In W/H,A0;In PdLine,P1;In P/L Coa Center,P0;In P/L,D1;Consumed,A1;Return,16;Removal,A2;Removal,A3;Return to W/H,RE;Damaged,01;Lost,02;Obsolete,05;Correction,11;Rerurn,16";
        station = Request["Station"];
        userId = Master.userInfo.UserId;
        customer = Master.userInfo.Customer;
        msgInvalidFileType = this.GetLocalResourceObject(Pre + "_MsgInvalidFileType").ToString();
        msgEmptyFile = this.GetLocalResourceObject(Pre + "_msgEmptyFile").ToString();
        msgFileEmpty = this.GetLocalResourceObject(Pre + "_msgFileEmpty").ToString();
        if (!this.IsPostBack)
        {
            this.TextBox1.Attributes.Add("onkeydown", "onTextBox1KeyDown()");
            this.TextBox2.Attributes.Add("onkeydown", "onTextBox2KeyDown()");
            InitLabel();
            initCNCardChange();
            initTableColumnHeader();
            //绑定空表格
            this.gridview.DataSource = getNullDataTable();
            this.gridview.DataBind();
            this.drpCOACardChange.Attributes.Add("onchange", "drpOnChange()");
        }
    }
    private void TableClear()
    {
        initTableColumnHeader();
        //绑定空表格
        this.gridview.DataSource = getNullDataTable();
        this.gridview.DataBind();
        this.updatePanelGrid.Update();
    }
    private void InitLabel()
    {
        this.lbCardNo.Text = this.GetLocalResourceObject(Pre + "_lbCardNo").ToString();
        this.lbRange.Text = this.GetLocalResourceObject(Pre + "_lbRange").ToString();
        this.lbPlace.Text = this.GetLocalResourceObject(Pre + "_lbPlace").ToString();
        this.lbCount.Text = this.GetLocalResourceObject(Pre + "_lbCount").ToString();
        this.lbPN.Text = this.GetLocalResourceObject(Pre + "_lbPN").ToString();
        this.lbTarget.Text = this.GetLocalResourceObject(Pre + "_lbTarget").ToString();
        this.btnQuery.Value =  this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnReceive.Value = this.GetLocalResourceObject(Pre + "_btnReceive").ToString();
    }
    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.gridview.Columns[0].HeaderText = "HP P/N";
        this.gridview.Columns[1].HeaderText = "Begin No";
        this.gridview.Columns[2].HeaderText = "End No";
        this.gridview.Columns[3].HeaderText = "Qty";
    }
    /// <summary>
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("HP P/N", Type.GetType("System.String"));
        retTable.Columns.Add("Begin No", Type.GetType("System.String"));
        retTable.Columns.Add("End No", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));
        return retTable;
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

    private DataTable getNullDataTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            newRow["HP P/N"] = String.Empty;
            newRow["Begin No"] = String.Empty;
            newRow["End No"] = String.Empty;
            newRow["Qty"] = String.Empty;
            dt.Rows.Add(newRow);
        }

        return dt;
    }
    private void initControl(IList<SelectInfoDef> lstCOACardChange)
    {
        ListItem item = null;

        this.drpCOACardChange.Items.Clear();
        this.drpCOACardChange.Items.Add(string.Empty);

        if (lstCOACardChange != null)
        {
            foreach (SelectInfoDef temp in lstCOACardChange)
            {
                item = new ListItem(temp.Value, temp.Text);
                this.drpCOACardChange.Items.Add(item);
            }

        }
    }
    public void initCNCardChange()
    {

        IList<SelectInfoDef> lstCOACardChange = new List<SelectInfoDef>();
        int position = cmdValue.IndexOf("'");
        while (position > -1)
        {
            cmdValue = cmdValue.Replace("'", "");
            position = cmdValue.IndexOf("'");
        }
        string[] sArray = cmdValue.Split(new char[2] { ',', ';' });
        int count = 0;
        SelectInfoDef temp = null;
        foreach (string i in sArray)
        {
            if (count > 1)
            {
                count = 0;
                if (null != temp)
                {
                    lstCOACardChange.Add(temp);
                }
            }
            if (0 == count)
            {
                temp = new SelectInfoDef();
                temp.Value = i.ToString();
                count++;
                continue;
            }
            if (1 == count)
            {
                string tmp = temp.Value;
                temp.Value = i.ToString();
                temp.Text = tmp + ";" + i.ToString();
                temp.Value = temp.Text;
                temp.Text = i.ToString();
                count++;
                continue;
            }
        }
        if (lstCOACardChange != null && lstCOACardChange.Count != 0)
        {
            initControl(lstCOACardChange);
        }
        else
        {
            initControl(null);
        }
    }
    private void btnGetCOAList_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string begNo = begNO.Value;
            string endNo = endNO.Value;
            int rangeCount = int.Parse(range.Value);
            string tempStatus = "";
            string tempPno = "";
            bool first = true;
            IList<S_RowData_COAStatus> infoBeg = iCOAStatusChange.GetCOAList(begNo, begNo);
            if ((null == infoBeg)
                || (null != infoBeg && infoBeg.Count == 0))
            {
                ClearPage();
                string tmpStr = msgWrongCode + " No:";
                tmpStr = tmpStr + begNo;
                writeToInfoMessage(tmpStr);
                return;
            }
            IList<S_RowData_COAStatus> infoEnd = iCOAStatusChange.GetCOAList(endNo, endNo);
            if ((null == infoEnd)
                || (null != infoEnd && infoEnd.Count == 0))
            {
                ClearPage();
                string tmpStr = msgWrongCode + " No:";
                tmpStr = tmpStr + endNo;
                writeToInfoMessage(tmpStr);
                return;
            }
            IList<S_RowData_COAStatus> info = iCOAStatusChange.GetCOAList(begNo, endNo);
            int count = info.Count;
            if(null == info
                || 0 == count)
            {
                ClearPage();
                writeToInfoMessage(msgCountFirst);
                return;
            }
            if (rangeCount != count)
            {
                ClearPage();
                writeToInfoMessage(msgCountError);
                return;
            }
            foreach (S_RowData_COAStatus ele in info)
            {
                if (first)
                {
                    tempStatus = ele.Status;
                    tempPno = ele.Pno;
                    first = false;
                    continue;
                }
                if (tempStatus != ele.Status)
                {
                    ClearPage();
                    writeToInfoMessage(msgStatusError);
                    return;
                }
                if (tempPno != ele.Pno)
                {
                    ClearPage();
                    writeToInfoMessage(msgPnoError);
                    return;
                }
            }
            string place = GetCurrentPlace(tempStatus);
            if (tempStatus == "P0" || tempStatus == "D1")
            {
                string ret = iCOAStatusChange.QueryEarly(begNo);
                ReQueryEarly(ret);
            }
            place = tempStatus + "," + place;
            ReQueryPno(tempPno);
            ReQueryPlace(place);
            ReQueryStatus(tempStatus);
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
    private void btnGetCOAListNoEnter_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string begNo = begNO.Value;
            string endNo = endNO.Value;
            int rangeCount = int.Parse(range.Value);
            string tempStatus = "";
            string tempPno = "";
            bool first = true;
            IList<S_RowData_COAStatus> infoBeg = iCOAStatusChange.GetCOAList(begNo, begNo);
            if ((null == infoBeg)
                || (null != infoBeg && infoBeg.Count == 0))
            {
                ClearPage();
                string tmpStr = msgWrongCode + " No:";
                tmpStr = tmpStr + begNo;
                writeToInfoMessage(tmpStr);
                return;
            }
            IList<S_RowData_COAStatus> infoEnd = iCOAStatusChange.GetCOAList(endNo, endNo);
            if ((null == infoEnd)
                || (null != infoEnd && infoEnd.Count == 0))
            {
                ClearPage();
                string tmpStr = msgWrongCode + " No:";
                tmpStr = tmpStr + endNo;
                writeToInfoMessage(tmpStr);
                return;
            }
            IList<S_RowData_COAStatus> info = iCOAStatusChange.GetCOAList(begNo, endNo);
            int count = info.Count;
            if(null == info
                || 0 == count)
            {
                ClearPage();
                writeToInfoMessage(msgCountFirst);
                return;
            }
            if (rangeCount != count)
            {
                ClearPage();
                writeToInfoMessage(msgCountError);
                return;
            }
            foreach (S_RowData_COAStatus ele in info)
            {
                if (first)
                {
                    tempStatus = ele.Status;
                    tempPno = ele.Pno;
                    first = false;
                    continue;
                }
                if (tempStatus != ele.Status)
                {
                    ClearPage();
                    writeToInfoMessage(msgStatusError);
                    return;
                }
                if (tempPno != ele.Pno)
                {
                    ClearPage();
                    writeToInfoMessage(msgPnoError);
                    return;
                }
            }
            string place = GetCurrentPlace(tempStatus);
            if (tempStatus == "P0" || tempStatus == "D1")
            {
                string ret = iCOAStatusChange.QueryEarly(begNo);
                ReQueryEarly(ret);
            }
            place = tempStatus + "," + place;
            ReQueryPno(tempPno);
            ReQueryPlace(place);
            ReQueryStatusNoEnter(tempStatus);
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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void writeToInfoMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void ClearPage()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ResetPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ClearPage", scriptBuilder.ToString(), false);
    }
    private void ReQueryPno(string pno)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getQueryPno(\"" + pno + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReQueryPno", scriptBuilder.ToString(), false);
    }
    private void ReQueryPlace(string place)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getQueryPlace(\"" + place + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReQueryPlace", scriptBuilder.ToString(), false);
    }
    private void ReQueryStatus(string status)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getQueryStatus(\"" + status + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReQueryStatus", scriptBuilder.ToString(), false);
    }
    private void ReQueryStatusNoEnter(string status)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getQueryStatusNoEnter(\"" + status + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReQueryStatus", scriptBuilder.ToString(), false);
    }

   
   
    
 
    public string GetCurrentPlace(string currentStatus)
    {
        int position = placeValue.IndexOf("'");
        while (position > -1)
        {
            cmdValue = placeValue.Replace("'", "");
            position = placeValue.IndexOf("'");
        }
        string[] sArray = placeValue.Split(new char[2] { ',', ';' });
        string temp = "";
        foreach (string i in sArray)
        {
            if (i.ToString().IndexOf(currentStatus) != -1)
            {
                return temp;
            }
            temp = i.ToString();
        }
        return "";
    }
    private void btnUpdateCOAList_ServerClick(object sender, System.EventArgs e)
    {
        string begNo = begNO.Value;
        string endNo = endNO.Value;
        string statusTarget = status.Value;
        string changeStation = "";
        changeStation = station;
        try
        {
            
            if (null == changeStation)
            {
                changeStation = "";
            }
            iCOAStatusChange.UpdateCOA(begNo, endNo, userId, statusTarget, "", changeStation);
            ReSave();
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
    private void btnReceiveCOAList_ServerClick(object sender, System.EventArgs e)
    {
        string begNo = begNO.Value;
        string endNo = endNO.Value;
        try
        {
            IList<S_RowData_COAStatus> infoBeg = iCOAStatusChange.GetCOAList(begNo, begNo);
            if ((null == infoBeg)
                || (null != infoBeg && infoBeg.Count == 0))
            {
                ClearPage();
                writeToInfoMessage(msgWrongCode);
                return;
            }
            IList<S_RowData_COAStatus> infoEnd = iCOAStatusChange.GetCOAList(endNo, endNo);
            if ((null == infoEnd)
                || (null != infoEnd && infoEnd.Count == 0))
            {
                ClearPage();
                writeToInfoMessage(msgWrongCode);
                return;
            }
            iCOAStatusChange.ReceiveCOA(begNo, endNo, userId);
            ReReceive();
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
    private void btnQueryEarly_ServerClick(object sender, System.EventArgs e)
    {
        string begNo = begNO.Value;
        string ret = "";
        try
        {
            IList<S_RowData_COAStatus> infoBeg = iCOAStatusChange.GetCOAList(begNo, begNo);
            if ((null == infoBeg)
                || (null != infoBeg && infoBeg.Count == 0))
            {
                ClearPage();
                writeToInfoMessage(msgWrongCode);
                return;
            }
            ret = iCOAStatusChange.QueryEarly(begNo);
            ReQueryEarly(ret);
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
    private void ReQueryEarly(string number)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getQueryEarly(\"" + number + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReQueryEarly", scriptBuilder.ToString(), false);
    }
    private void ReReceive()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getReceive();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReReceive", scriptBuilder.ToString(), false);
    }

    private void ReSave()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getSave();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReSave", scriptBuilder.ToString(), false);
    }
    public void btnTableClear_ServerClick(object sender, System.EventArgs e)
    {
        TableClear();
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
                throw new Exception(
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
    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnCheckUpload_ServerClick(object sender, System.EventArgs e)
    {
        string path = string.Empty;
        string errorMsg = "";
        bool fileOK = false;
        string strfile = "";
        string strfileTemp = "";
        string fullName = "";
        try
        {
            if (this.txtBrowse.FileName!= "")
            {

                strfile = this.txtBrowse.FileName;

                String fileExtension =
                        System.IO.Path.GetExtension(strfile).ToLower();
                String[] allowedExtensions = { ".txt" };

                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension.ToLower() == allowedExtensions[i].ToLower())
                    {
                        fileOK = true;
                        break;
                    }
                }

                if (!fileOK)
                {
                    errorMsg = msgInvalidFileType;
                    writeToAlertMessage(errorMsg);
                }
                else
                {
                    strfileTemp = DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                    fullName = Server.MapPath("../") + strfileTemp + strfile;
                    this.txtBrowse.PostedFile.SaveAs(fullName);
                    StreamReader m_streamReaderCoa = new StreamReader(fullName, Encoding.GetEncoding(Encoding_GuessEncoding(fullName)), true);//设定读写的编码
                   

                    if (m_streamReaderCoa == null )
                    {
                        //writeToAlertMessage(GetLocalResourceObject(Pre + "_msgCheckFTP").ToString());
                        return;
                    }

                    //使用StreamReader类来读取文件
                    m_streamReaderCoa.BaseStream.Seek(0, SeekOrigin.Begin);
     

                    IList<string> coaLines = new List<string>();
                    //从数据流中读取每一行，直到文件的最后一行
                    string strLineCoa = m_streamReaderCoa.ReadLine();
                    while (strLineCoa != null)
                    {
                        if (strLineCoa.Trim() != "")
                        {
                            coaLines.Add(strLineCoa);
                        }
                        strLineCoa = m_streamReaderCoa.ReadLine();
                    }
                  
                    //关闭此StreamReader对象
                    m_streamReaderCoa.Close();

                }
            }
            else
            {
                writeToAlertMessage(msgEmptyFile);
            }
        }
        catch (FisException fe)
        {
            writeToAlertMessage(fe.mErrmsg);
            return;
        }
        catch (Exception ee)
        {
            writeToAlertMessage(ee.Message);
            return;
        }
    }
    private void CheckCoaListToUI(IList<string> coaLines)
    {
        foreach (string tmp in coaLines)
        { 

        }
    }
}
