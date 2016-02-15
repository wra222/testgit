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
using System.Xml.Linq;
using com.inventec.iMESWEB;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;

using System.Diagnostics;
using System.IO;

using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;
using IMES.Docking.Interface.DockingIntf;

public partial class Docking_OfflineLabelPrint : IMESBasePage 
{
    static int[] g_boxSelected = new int[]{0,0,0,0,0,0,0,0};
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IOfflineLabelPrintForDocking iOfflineLabelPrint = ServiceAgent.getInstance().GetObjectByName<IOfflineLabelPrintForDocking>(WebConstant.OfflineLabelPrintForDockingObject);
        
    public Boolean floorEnabled = false;
    public string userId;
    public string customer;

    public static IList<OfflineLableSettingDef> got_all_list = null;
    int show_index = 0;

    /*
    private void cmbCode_Selected(object sender, System.EventArgs e)
    {
        string label_name = this.cmbLabelName.InnerDropDownList.Text;
        if (label_name == "")
        {
            this.txtLabelSpec.Text = "";
            this.txtFileName.Text = "";
            for (int i = 0; i < 8; i++)
            {
                get_Label(i).Visible = false;
                get_textBox(i).Visible = false;
            }
            //this.txtProductID.Focus();
            this.cmbLabelName.InnerDropDownList.Focus();
        }
        else
        {
            if (got_all_list == null)
            {
                got_all_list = iOfflineLabelPrint.Get_offline_lable_list();
            }

            foreach (OfflineLableSettingDef info in got_all_list)
            {
                if (string.Compare(info.description,label_name) == 0)
                {
                    this.txtLabelSpec.Text = info.labelSpec;
                    this.txtFileName.Text = info.fileName;
                    // to compute COUNT must be shown.
                    ArrayList bufferList = new ArrayList();

                    for (int i = 0; i < 8; i++)
                    {
                        get_Label(i).Text = "";
                        get_textBox(i).Text = "";
                        get_textBox(i).Visible = false;
                    }

                    show_index = 0;
                    for (int i = 0; i < 8; i++) 
                    {
                        show_index_(info, i, bufferList);
                    }

                    bufferList.RemoveRange(0, bufferList.Count);
                    break;
                }
            }
            //this.txtProductID.Focus();
            this.cmbLabelName.InnerDropDownList.Focus();
        }
    }
     */


    private void cmbCode_Selected(object sender, System.EventArgs e)
    {
        int addFlag = 0;

        this.TextBox1.Text = ""; this.TextBox2.Text = ""; this.TextBox3.Text = ""; this.TextBox4.Text = "";
        this.TextBox5.Text = ""; this.TextBox6.Text = ""; this.TextBox7.Text = ""; this.TextBox8.Text = "";

        string __all_para = "";
        int index = this.cmbLabelName.InnerDropDownList.SelectedIndex;
        if (index == 0)
        {
            __refresh__(",,,,,,,");
        }
        else
        {
            //if (got_all_list == null)
            {   //modify ITC-1414-0177 BUG
                got_all_list = iOfflineLabelPrint.Get_offline_lable_list();
            }

            OfflineLableSettingDef node = got_all_list[index - 1];
            int nMustShowCount =
                (node.param1 == "" ? 0 : 1) + (node.param2 == "" ? 0 : 1) + (node.param3 == "" ? 0 : 1) + (node.param4 == "" ? 0 : 1) +
                (node.param5 == "" ? 0 : 1) + (node.param6 == "" ? 0 : 1) + (node.param7 == "" ? 0 : 1) + (node.param8 == "" ? 0 : 1);
            if (nMustShowCount == 0)
            {
                if (node.labelSpec != "") { __all_para += node.labelSpec; }  __all_para += ","; 
                if (node.fileName != "") { __all_para += node.fileName; } __all_para += ","; 
                __all_para += ",,,,,,,,,,,";
                __refresh__(__all_para);
            }
            else
            {
                if (node.labelSpec != "") { __all_para += node.labelSpec; } __all_para += ",";
                if (node.fileName != "") { __all_para += node.fileName; } __all_para += ",";
                if (node.param1 != "") { if (addFlag != 0) { __all_para += ","; } __all_para += node.param1; addFlag = 1; }
                if (node.param2 != "") { if (addFlag != 0) { __all_para += ","; } __all_para += node.param2; addFlag = 1; }
                if (node.param3 != "") { if (addFlag != 0) { __all_para += ","; } __all_para += node.param3; addFlag = 1; }
                if (node.param4 != "") { if (addFlag != 0) { __all_para += ","; } __all_para += node.param4; addFlag = 1; }
                if (node.param5 != "") { if (addFlag != 0) { __all_para += ","; } __all_para += node.param5; addFlag = 1; }
                if (node.param6 != "") { if (addFlag != 0) { __all_para += ","; } __all_para += node.param6; addFlag = 1; }
                if (node.param7 != "") { if (addFlag != 0) { __all_para += ","; } __all_para += node.param7; addFlag = 1; }
                if (node.param8 != "") { if (addFlag != 0) { __all_para += ","; } __all_para += node.param8; addFlag = 1; }

                int xyz = 0;
                while (xyz < 8 - nMustShowCount)
                {
                    __all_para += ",";
                    xyz++;
                }

                __refresh__(__all_para);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try {
            
            this.cmbLabelName.InnerDropDownList.AutoPostBack = true;
            this.cmbLabelName.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbCode_Selected);
            if (!Page.IsPostBack)
            {
                //this.cmbLabelName.AutoPostBack = true;
                
                InitLabel();
                /*
                ArrayList this_list = liu_get_list();
                foreach (object item in this_list)
                {
                    this.cmbLabelName.Items.Add(new ListItem(item.ToString()));
                }
                //this.cmbLabelName.DataSource = GetList(); 
                this.cmbLabelName.SelectedIndex = 0; 
                //this.cmbLabelName.AutoPostBack = true;
                //this.cmbLabelName.SelectedIndexChanged += new EventHandler(cmbCode_Selected); 
                if (this_list.Count == 2)
                {
                    this.cmbLabelName.SelectedIndex = 1;
                    cmbCode_Selected(sender, e);
                }
                */
                this.SetFocus(this.txtProductID);

                //this.station.Value = Request["Station"]; 
                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;
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

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lbFilePath.Text = this.GetLocalResourceObject(Pre + "_lblFilePath").ToString();
        this.lbFileName.Text = this.GetLocalResourceObject(Pre + "_lblFileName").ToString();
        this.lbLabelSpec.Text = this.GetLocalResourceObject(Pre + "_lblLabelSpec").ToString();
        this.lbLabelName.Text = this.GetLocalResourceObject(Pre + "_lblLabelName").ToString();
        //this.lbParameter1.Text = this.GetLocalResourceObject(Pre + "_lblParameter1").ToString();
        this.lbParameter1.Text = "";
        this.lbParameter2.Text = "";
        this.lbParameter3.Text = "";
        this.lbParameter4.Text = "";
        this.lbParameter5.Text = "";
        this.lbParameter6.Text = "";
        this.lbParameter7.Text = "";
        this.lbParameter8.Text = "";
        //for (int i = 2; i < 8; i++)
        //{
        //    get_textBox(i).Visible = false;
        //}
        this.btnPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
    }

    public void btnHidden2_Click(object sender, System.EventArgs e)
    {
        ArrayList this_list = liu_get_list();
        
        bool b1Visuable = this.TextBox1.Visible;
        bool b2Visuable = this.TextBox2.Visible;
        bool b3Visuable = this.TextBox3.Visible;
        bool b4Visuable = this.TextBox4.Visible;
        bool b5Visuable = this.TextBox5.Visible;
        bool b6Visuable = this.TextBox6.Visible;
        bool b7Visuable = this.TextBox7.Visible;
        bool b8Visuable = this.TextBox8.Visible;

        if (b1Visuable)
        {
            this.TextBox1.Visible = false;
        }

        if (b2Visuable)
        {
            this.TextBox2.Visible = false;
        }

        //if (this_list.Count > 2)
        //{
        //    this.cmbLabelName.SelectedIndex = 1;
        //    cmbCode_Selected(sender, e);
        //}

        this.SetFocus(this.txtProductID);
    }

    public void btnHid_en_drpdownList_Click(object sender, System.EventArgs e)
    {
        try
        {
            this.cmbLabelName.setSelected(0);
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
            int count = 0;

            if (this.txtProductID.Text == "")
            {
                this.txtProductID.Focus();
                return;
            }

            IList<string> strTextboxList = new List<string>();

            foreach (int a in g_boxSelected)
            {
                count += a;
            }

            if (count <= 0) return;
            
            for (int x = 0; x < count; x++)
            {
                strTextboxList.Add(get_textBox(x).Text);
            }

            //string strWriteFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "test1.bat";
            //if (File.Exists(strWriteFile))
            //{
            //    File.Delete(strWriteFile);
            //}
            //FileStream fsWriteStream = File.OpenWrite(strWriteFile);

            int index_Textbox_ShownInPage = 0;
            string _all_content = "";
            for (int i = 0; i < 8; i++)
            {
                if (g_boxSelected[i] != 0)
                {
                    string _content = strTextboxList[index_Textbox_ShownInPage]; 
                    string _lbl_text = get_Label(index_Textbox_ShownInPage).Text.ToString().Trim();
                    //string _lbl_text = get_Label(i).Text.ToString().Trim();
                    index_Textbox_ShownInPage++;
                    _all_content += "set " + _lbl_text + "=" + _content + "\r\n";
                }
            }

            if (_all_content != "")
            {
                _all_content += "call ";
                string tmp = this.txtProductID.Text.ToString().Trim();
                if ((tmp.Length > 1) && (tmp[tmp.Length - 1] != '\\'))
                {
                    tmp += "\\";
                }
                tmp += this.txtFileName.Text.ToString().Trim();
                if (tmp.Contains(" ") || tmp.Contains("\t"))
                {
                    string x = "\"" + tmp + "\"";
                    tmp = x;
                }

                //if (!File.Exists(tmp))
                //{
                //    run_client_show_error_msg();
                //    return;
                //}

                _all_content += tmp;

                byte[] byteArray = System.Text.Encoding.Default.GetBytes(_all_content);
                //fsWriteStream.Write(byteArray, 0, _all_content.Length);
            }

            //fsWriteStream.Close();
            //call_dos_cmd(strWriteFile);

            run_client_bat(_all_content, true);//false);
            this.txtProductID.Focus();
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

    // ------------------------------------------------------------------------
    //      The following is basic function to use in this file.
    // ------------------------------------------------------------------------

    public TextBox get_textBox(int index)
    {
        switch (index)
        {
            case 0:
                return this.TextBox1;
            case 1:
                return this.TextBox2;
            case 2:
                return this.TextBox3;
            case 3:
                return this.TextBox4;
            case 4:
                return this.TextBox5;
            case 5:
                return this.TextBox6;
            case 6:
                return this.TextBox7;
            case 7:
                return this.TextBox8;
        }

        return this.TextBox1;
    }

    public Label get_Label(int index)
    {
        switch (index)
        {
            case 0:
                return this.lbParameter1;
            case 1:
                return this.lbParameter2;
            case 2:
                return this.lbParameter3;
            case 3:
                return this.lbParameter4;
            case 4:
                return this.lbParameter5;
            case 5:
                return this.lbParameter6;
            case 6:
                return this.lbParameter7;
            case 7:
                return this.lbParameter8;
        }

        return this.lbParameter1;
    }

    public string get_parameterN(OfflineLableSettingDef info, int index)
    {
        switch (index)
        {
            case 0:
                return info.param1;
            case 1:
                return info.param2;
            case 2:
                return info.param3;
            case 3:
                return info.param4;
            case 4:
                return info.param5;
            case 5:
                return info.param6;
            case 6:
                return info.param7;
            case 7:
                return info.param8;
        }

        return info.param1;
    }

    public bool isInArrayList(string str, ArrayList _list)
    {
        bool _in = false;
        for (int i = 0; i < _list.Count; i++)
        {
            if (string.Compare(_list.ToString(), str) == 0)
            {
                _in = true;
                break;
            }
        }

        return _in;
    }

    public void call_dos_cmd(string batFilename)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = batFilename;
        process.Start();
    }

    public ArrayList liu_get_list()
    {
        bool _has_empty_record = false;
        got_all_list = iOfflineLabelPrint.Get_offline_lable_list();

        ArrayList retLst = new ArrayList();
        foreach (OfflineLableSettingDef info in got_all_list)
        {
            if ((!_has_empty_record) && (info.description == ""))
            {
                _has_empty_record = true;
                break;
            }
        }

        if (!_has_empty_record)
        {
            retLst.Add("");
            _has_empty_record = true;
        }

        foreach (OfflineLableSettingDef info in got_all_list)
        {
            //if (info.description == "27")
            //{
                retLst.Add(info.description);
            //}
        }

        if (retLst.Count == 2)
        {
            this.btnHidden.InnerText = "1";
        }
        else
        {
            this.btnHidden.InnerText = "0";
        }
        return retLst;
    }

    private void show_inner(string _str, int index)
    {
        get_Label(index).Visible = true;
        get_Label(index).Text = _str;
        get_textBox(index).Visible = true;
    }

    private void show_index_(OfflineLableSettingDef info, int index_num, ArrayList bufferList)
    {
        string paramN = get_parameterN(info, index_num);
        if ((paramN != "") && !isInArrayList(paramN, bufferList))
        {
            bufferList.Add(paramN); show_inner(paramN, show_index); show_index++;
            g_boxSelected[index_num] = 1;
        }
        else
        {
            g_boxSelected[index_num] = 0;
        }
    }

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {
       
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void __refresh__(string editMessage)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("Refresh(\"" + editMessage.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void run_client_bat(string batContent, bool isSynchronized)
    {
        string NeedSyn = "false";
        if (isSynchronized) NeedSyn = "true";

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        batContent = batContent.Replace("\r\n", "##");
        batContent = batContent.Replace("\\", "\\\\");
        batContent = batContent.Replace("\"", "\\\"");
        scriptBuilder.AppendLine("__runBat(\"" + batContent + "\"," + NeedSyn + ");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "run_client_batAgent", scriptBuilder.ToString(), false);

    }
    private void Refreshpage()
    {
        //String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        //ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
        //DEBUG ITC-1360-0729
        String script = "<script language='javascript'>" + "\r\n" +
                        "window.setTimeout (callNextInput,100);" + "\r\n" +
                        "</script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "callNextInput", script, false);
    }
}