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
using System.Collections.Generic;
using UPH.Interface;
using System.Data.OleDb;
using System.IO;
using System.Text;

public partial class webroot_UPHMaintain_AlarmMaintain : System.Web.UI.Page
{
    IAlarmMaintain IAM = ServiceAgent.getInstance().GetObjectByName<IAlarmMaintain>(WebConstant.UPHAlarm);
    #region 设置列宽
    public int[] GvQueryColumnNameWidth = { 50, 60, 50, 60,80,80,50,150,100,155,155 };
    private void InitGridView()
    {
        for (int i = 0; i < GvQueryColumnNameWidth.Length; i++)
        {
            GridView1.HeaderRow.Cells[i].Height = Unit.Pixel(20);
            GridView1.HeaderRow.Cells[i].Style.Add("text-align", "center");
            GridView1.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<string> process = IAM.GetAlarmProcess();
            Process.Items.Clear();
            Process.Items.Insert(0, new ListItem("全部制程", "ALL"));
            foreach (string temp in process)
            {
                ListItem item = new ListItem(temp, temp);
                Process.Items.Add(item);
            }
            List<string> line = IAM.GetAlarmline(Process.SelectedValue);
            PdLine.Items.Clear();
            foreach (string temp in line)
            {
                ListItem item = new ListItem(temp, temp);
                PdLine.Items.Add(item);
            }
            if (Process.SelectedValue == "ALL")
            {
                PdLine.Items.Insert(0, new ListItem("全部线别", "ALL"));
            }
            Button_Query_Click(this, null);
        }
        //BeginTime.Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
        //EndTime.Value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
    }
    protected void Button_Maintain_Click(object sender, EventArgs e)
    {
        if (Process.SelectedValue == "ALL")
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请选择具体制程！')", true);
        }
        else if (Shift.SelectedValue == "ALL")
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请选择具体班次！')", true);
        }
        else if (PdLine.SelectedValue == "ALL")
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请选择具体线别！')", true);
        }
        //else if (get_id.Value != "" || get_id.Value != null)
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('表中选中的数据请使用Update更新！')", true);
        //    get_id.Value = null;
        //}
        else
        {
            AlarmInfo alarminfo = new AlarmInfo()
            {
                process = Process.SelectedValue,
                Class = Shift.SelectedValue,
                PdLine = PdLine.SelectedValue,
                BeginTime = BeginTime.Value,
                EndTime = EndTime.Value,
                Status = Status.SelectedValue,
                Remark = Request.Form["Remark"],
                Editor = ((MasterPageMaintain)Master).userInfo.UserId.ToString(),
                Cdt = DateTime.Now,
                Udt = DateTime.Now
            };
            AlarmInfoLog alarminfolog = new AlarmInfoLog()
            {
                process = Process.SelectedValue,
                Class = Shift.SelectedValue,
                PdLine = PdLine.SelectedValue,
                BeginTime = BeginTime.Value,
                EndTime = EndTime.Value,
                Status = Status.SelectedValue,
                Remark = "Maintain！ " + Request.Form["Remark"],
                Editor = ((MasterPageMaintain)Master).userInfo.UserId.ToString(),
                Cdt = DateTime.Now,
            };
            if (BeginTime.Value.Trim() != "" && EndTime.Value.Trim() != "")
            {
                IAM.AddAlarmInfo(alarminfo);
                IAM.AddAlarmlog(alarminfolog);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('添加Alarm已完成!')", true);
                PdLine_SelectedIndexChanged1(this, null);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请设定需要Alarm的时间段!')", true);
            }
        }
    }
    protected void Process_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        List<string> line = IAM.GetAlarmline(Process.SelectedValue);
        PdLine.Items.Clear();
        PdLine.Items.Insert(0, new ListItem("全部线别", "ALL"));
        foreach (string temp in line)
        {
            ListItem item = new ListItem(temp, temp);
            PdLine.Items.Add(item);
        }
        if (Process.SelectedValue == "ALL")
        {
            Shift.SelectedValue = "ALL";
        }
        //string process = Process.SelectedValue;
        ////GridView1 = new GridView();
        //this.GridView1.DataSourceID = null;
        //DataTable dt = new DataTable();
        //dt = IAM.GetAlarm(process);
        //this.GridView1.DataSource = dt;
        //this.GridView1.DataBind();
        //InitGridView();
        //if (dt.Rows.Count == 0)
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('制程" + process + "尚未维护Alarm!')", true);
        //}
    }
    protected void Button_Query_Click(object sender, EventArgs e)
    {
        //string process = Process.SelectedValue;
        //GridView1 = new GridView();
        string process = Process.SelectedValue;
        string Class = Shift.SelectedValue;
        string line=PdLine.SelectedValue;
        this.GridView1.DataSourceID = null;
        DataTable dt = new DataTable();
        if (process == "ALL")
        {
            if (Class != "ALL")
            {
                if (line != "ALL")
                {
                    dt = IAM.GetAlarmPd(line);
                }
                else
                {
                    dt = IAM.GetAlarmCALL(Class);
                }
            }
            else
            {
                if (line != "ALL")
                {
                    dt = IAM.GetAlarmPd(line);
                }
                else
                {
                    dt = IAM.GetAlarmALL();
                }
            }
        }
        else if (process != "ALL" && Class == "ALL" && line == "ALL")
        {
            dt = IAM.GetAlarm(process);
        }
        else if (process != "ALL" && Class != "ALL" && line == "ALL")
        {
            if (get_line.Value != "" && get_line.Value != null)
            { 
                line = get_line.Value;
                dt = IAM.GetAlarmPd(line);
                PdLine.SelectedValue = get_line.Value;
                get_line.Value = null;
            }
            else
            {
                dt = IAM.GetAlarmC(process, Class);
            }
        }
        else if (process != "ALL" && Class == "ALL" && line != "ALL")
        {
            dt = IAM.GetAlarmPd(line);
        }
        else if (process != "ALL" && Class != "ALL" && line != "ALL")
        {
            dt = IAM.GetAlarmPd(line);
        }
        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();
        InitGridView();
        get_line.Value = null;
        //if (dt.Rows.Count == 0)
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('该制程尚未维护Alarm!')", true);
        //}
        
    }
    protected void Button_Delete_Click(object sender, EventArgs e)
    {
        //DELETE();
        //bool bl;
        int id; 
        //listId.Add(id);
        //bl = true;
        if (get_id.Value == ""||get_id.Value==null)
        {
            //Response.Write("<script>alert('请勾选需要删除的数据!')</script>");
            //Response.Flush();
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请在下表中点选需要删除的数据!')", true);
        }
        else
        {
            id = Convert.ToInt32(get_id.Value);
            IAM.DelAlarmLog(id);
            IAM.DelAlarm(id);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('删除Alarm已完成!')", true);
            PdLine_SelectedIndexChanged1(this, null);
            get_id.Value = null;
        }
    }
    protected void Shift_SelectedIndexChanged(object sender, EventArgs e)
    {
        string process = Process.SelectedValue;
        string Class = Shift.SelectedValue;
        //GridView1 = new GridView();
        this.GridView1.DataSourceID = null;
        DataTable dt = new DataTable();
        dt = IAM.GetAlarmC(process, Class);
        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();
        InitGridView();
        //if (dt.Rows.Count == 0)
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('制程" + process + "(" + Shift.SelectedItem + ")尚未维护Alarm!')", true);
        //}
    }
    protected void PdLine_SelectedIndexChanged1(object sender, EventArgs e)
    {
        string pdLine;
        if (get_id.Value != "" && get_id.Value != null)
        {
            pdLine = get_line.Value;
        }
        else
        {
            pdLine = PdLine.SelectedValue;
        }
        //GridView1 = new GridView();
        this.GridView1.DataSourceID = null;
        DataTable dt = new DataTable();
        dt = IAM.GetAlarmPd(pdLine);
        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();
        if (dt != null && dt.Rows.Count != 0)
        {
            InitGridView();
        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('线别" + pdLine + "尚未维护Alarm!')", true);

        }
        //if (dt.Rows.Count == 0)
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('线别" + pdLine + "尚未维护Alarm!')", true);
        //}
    }
    protected void Button_Import_Click(object sender, EventArgs e)
    {
        //获取文件路径
        string filePath = this.FileUpload1.PostedFile.FileName;
        //filePath = Path.GetExtension(filePath);//获取文件后缀名
        filePath = Path.GetFileName(filePath);
        string SavePath = HttpRuntime.AppDomainAppPath + "App_Data\\" + filePath;

        if (filePath != ""&&filePath!=null)
        {
            if (filePath.Contains("xls")||filePath.Contains("xlsx"))//判断文件是否存在
            {
                FileUpload1.SaveAs(SavePath);
                InputExcel(SavePath);
                File.Delete(SavePath);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请确认您选择的文件是否为Excel文件！')", true);
            }
        }

        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请先选择导入文件后，再执行导入！')", true);

        }
    }
#region excel作为数据源导入数据
    //private void InputExcel(string pPath)
    //{
    //    //FileStream file = null;
    //    //string conn = "Provider = Microsoft.ACE.OLEDB.12.0 ; Data Source =" + pPath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";
    //    string connstr2003 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pPath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
    //    string connstr2007 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pPath + ";Extended Properties=\"Excel 12.0;HDR=YES\"";
    //    OleDbConnection oleCon;
    //    if (System.IO.Path.GetExtension(pPath) == ".xls")
    //    {
    //        oleCon = new OleDbConnection(connstr2003);
    //    }
    //    else
    //    {
    //        oleCon = new OleDbConnection(connstr2007);
    //    }
    //    oleCon.Open();
    //    //file = new FileStream(pPath, FileMode.Open);
    //    string Sql = "select * from [Sheet1$] where process<>'' and Class<>'' and PdLine<>'' and BeginTime<>'' and EndTime<>'' and Status<>''";
    //    OleDbDataAdapter mycommand = new OleDbDataAdapter(Sql, oleCon);
    //    DataSet ds = new DataSet();
    //    mycommand.Fill(ds, "[Sheet1$]");
    //    oleCon.Close();
    //    //file.Close();
    //    int count = ds.Tables["[Sheet1$]"].Rows.Count;
    //    AlarmInfo alarminfo = new AlarmInfo();
    //    AlarmInfoLog alarminfolog = new AlarmInfoLog();
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (ds.Tables["[Sheet1$]"].Rows[i]["process"].ToString().Trim() != "")
    //        {
    //            alarminfo.process = ds.Tables["[Sheet1$]"].Rows[i]["process"].ToString().ToUpper().Trim();
    //            alarminfo.Class = ds.Tables["[Sheet1$]"].Rows[i]["Class"].ToString().ToUpper().Trim();
    //            alarminfo.PdLine = ds.Tables["[Sheet1$]"].Rows[i]["PdLine"].ToString().ToUpper().Trim();
    //            alarminfo.BeginTime = Convert.ToDateTime(ds.Tables["[Sheet1$]"].Rows[i]["BeginTime"]).ToString("HH:mm").Trim();
    //            alarminfo.EndTime = Convert.ToDateTime(ds.Tables["[Sheet1$]"].Rows[i]["EndTime"]).ToString("HH:mm").Trim();
    //            alarminfo.Status = ds.Tables["[Sheet1$]"].Rows[i]["Status"].ToString().Trim();
    //            alarminfo.Remark = ds.Tables["[Sheet1$]"].Rows[i]["Remark"].ToString().Trim();
    //            alarminfo.Editor = ((MasterPageMaintain)Master).userInfo.UserName.ToString().Trim();
    //            alarminfo.Cdt = DateTime.Now;
    //            alarminfo.Udt = DateTime.Now;
    //            IAM.AddAlarmInfo(alarminfo);
    //            //塞入Log
    //            alarminfolog.process = ds.Tables["[Sheet1$]"].Rows[i]["process"].ToString().ToUpper().Trim();
    //            alarminfolog.Class = ds.Tables["[Sheet1$]"].Rows[i]["Class"].ToString().ToUpper().Trim();
    //            alarminfolog.PdLine = ds.Tables["[Sheet1$]"].Rows[i]["PdLine"].ToString().ToUpper().Trim();
    //            alarminfolog.BeginTime = Convert.ToDateTime(ds.Tables["[Sheet1$]"].Rows[i]["BeginTime"]).ToString("HH:mm").Trim();
    //            alarminfolog.EndTime = Convert.ToDateTime(ds.Tables["[Sheet1$]"].Rows[i]["EndTime"]).ToString("HH:mm").Trim();
    //            alarminfolog.Status = ds.Tables["[Sheet1$]"].Rows[i]["Status"].ToString().Trim();
    //            alarminfolog.Remark = "InputExcel！ " + ds.Tables["[Sheet1$]"].Rows[i]["Remark"].ToString().Trim();
    //            alarminfolog.Editor = ((MasterPageMaintain)Master).userInfo.UserName.ToString().Trim();
    //            alarminfolog.Cdt = DateTime.Now;
    //            IAM.AddAlarmlog(alarminfolog);
    //        }
    //        else
    //        {
    //            break;
    //        }
    //    }
    //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('导入成功！')", true);
    //    Button_Query_Click(this, null);
    //}
#endregion
    private void InputExcel(string pPath)
    {
        int NUM = 0;
        ToolUtility TU = new ToolUtility();
        DataTable DT = TU.getExcelSheetData(pPath, false);
        DataTable allDT = IAM.GetAlarmALL();
        foreach (DataRow DR in DT.Rows)
        {
            AlarmInfo alarminfo = new AlarmInfo()
            {
                process = DR["Process"].ToString().Trim(),
                Class = DR["Class"].ToString().Trim(),
                PdLine = DR["PdLine"].ToString().Trim(),
                BeginTime = DR["BeginTime"].ToString().Trim(),
                EndTime = DR["EndTime"].ToString().Trim(),
                Status = DR["Status"].ToString().Trim(),
                Remark = DR["Remark"].ToString().Trim(),
                Editor = ((MasterPageMaintain)Master).userInfo.UserId.ToString(),
                Cdt = DateTime.Now,
                Udt = DateTime.Now
            };
            AlarmInfoLog alarminfolog = new AlarmInfoLog()
            {
                process = DR["Process"].ToString().Trim(),
                Class = DR["Class"].ToString().Trim(),
                PdLine = DR["PdLine"].ToString().Trim(),
                BeginTime = DR["BeginTime"].ToString().Trim(),
                EndTime = DR["EndTime"].ToString().Trim(),
                Status = DR["Status"].ToString().Trim(),
                Remark = "Excel Input！ " + DR["Remark"].ToString().Trim(),
                Editor = ((MasterPageMaintain)Master).userInfo.UserId.ToString(),
                Cdt = DateTime.Now,
            };
            if (alarminfo.process != "" && alarminfo.Class != "" && alarminfo.PdLine != "" && alarminfo.BeginTime != "" && alarminfo.EndTime != "")
            {
                IAM.AddAlarmInfo(alarminfo);
                IAM.AddAlarmlog(alarminfolog);
            }
            else
            {
                NUM += 1;
                continue;
            }
            foreach (DataRow allDR in allDT.Rows)
            {
                if (allDR["Class"].ToString().Trim() == DR["Class"].ToString().Trim() && allDR["PdLine"].ToString().Trim() == DR["PdLine"].ToString().Trim())
                {
                    get_id.Value = allDR["ID"].ToString().Trim();
                    IAM.DelAlarm(Int32.Parse(get_id.Value));
                    get_id.Value = null;
                }
            }
        }
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('导入Excel已完成！" + NUM + "行数据不完整，未进行导入！')", true);
        Button_Query_Click(this, null);
    }
    protected void Button_Export_Click(object sender, EventArgs e)
    {
        //Response.ClearContent();
        //Response.AddHeader("content-disposition", "attachment; filename=Sheet1.xls");
        //Response.ContentType = "application/excel";
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter htw = new HtmlTextWriter(sw);
        //GridView1.RenderControl(htw);
        //Response.Write(sw.ToString());
        //Response.End();
        //string filename = String.Format("Sheet1.xls", DateTime.Now.Month.ToString());
        //ConvertToExcel(GridView1, "UTF-8", filename);
        ExportExcel(GridView1);
    }
    protected void ExportExcel(GridView gv)
    {
        if (GridView1.Rows.Count > 0)
        {
            ToolUtility tu = new ToolUtility();
            tu.ExportExcel(GridView1, "Sheet1", Page);
        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请查出需要导出的数据！')", true);
        }
    }
    #region 删除方法
    //protected void DELETE()
    //{
    //    bool i = false;
    //    foreach (GridViewRow rw in GridView1.Rows)
    //    {
    //        int id;
    //        if (((CheckBox)rw.FindControl("CheckBox1")).Checked == true)
    //        {
    //            id = Convert.ToInt32(rw.Cells[1].Text);
    //            //listId.Add(id);
    //            IAM.DelAlarmLog(id);
    //            IAM.DelAlarm(id);
    //            i = true;
    //        }
    //    }
    //    if (i == false)
    //    {
    //        //Response.Write("<script>alert('请勾选需要删除的数据!')</script>");
    //        //Response.Flush();
    //        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请勾选需要删除的数据!')", true);
    //    }
    //    else
    //    {
    //        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('删除Alarm已完成!')", true);
    //        Process_SelectedIndexChanged(this, null);
    //    }
    //}
#endregion
    #region 导出excel方法
    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    //override  VerifyRenderingInServerForm
    //}
    //public static void ConvertToExcel(System.Web.UI.Control control, string encoding, string filename)
    //{
    //    //设置文件名格式，防止中文文件名乱码
    //    string FileName = System.Web.HttpUtility.UrlEncode(Encoding.UTF8.GetBytes(filename));
    //    System.Web.HttpContext.Current.Response.Clear();
    //    System.Web.HttpContext.Current.Response.Buffer = true;
    //    System.Web.HttpContext.Current.Response.Charset = "" + encoding + "";
    //    //下面这行很重要， attachment 参数表示作为附件下载，您可以改成 online在线打开
    //    //filename=FileFlow.xls 指定输出文件的名称，注意其扩展名和指定文件类型相符，可以为：.doc 　　 .xls 　　 .txt 　　.htm
    //    System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
    //    System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("" + encoding + "");
    //    //Response.ContentType指定文件类型 可以为application/ms-excel、application/ms-word、application/ms-txt、application/ms-html 或其他浏览器可直接支持文档
    //    System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
    //    control.EnableViewState = false;
    //    //　定义一个输入流
    //    System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
    //    System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

    //    control.RenderControl(oHtmlTextWriter);
    //    //this 表示输出本页，你也可以绑定datagrid,或其他支持obj.RenderControl()属性的控件
    //    System.Web.HttpContext.Current.Response.Write(oStringWriter.ToString());
    //    System.Web.HttpContext.Current.Response.End();
    //}
    #endregion
    protected void Button_Update_Click(object sender, EventArgs e)
    {
        int id;
        //listId.Add(id);
        //bl = true;
        string begin = BeginTime.Value;
        string end = EndTime.Value;
        string status=Status.SelectedValue;
        string remark = Request.Form["Remark"];
        string editor = ((MasterPageMaintain)Master).userInfo.UserId.ToString();
        DateTime Cdt = DateTime.Now;
        DateTime Udt = DateTime.Now;
        if (get_id.Value == "" || get_id.Value == null)
        {
            //Response.Write("<script>alert('请勾选需要删除的数据!')</script>");
            //Response.Flush();
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请在下表中点选需要更新的数据!')", true);
        }
        else
        {
            id = Convert.ToInt32(get_id.Value);
            IAM.UpdateAlarm_ID(id, begin, end,status, remark, editor, Udt);
            //IAM.UpdateAlarm_ID(id, begin, end);
            IAM.UpdateAlarmLog_ID(id);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('更新Alarm已完成!')", true);
            PdLine_SelectedIndexChanged1(this, null);
            get_id.Value = null;
            get_line.Value = null;
        }
    }
    //private const int COL_NUM = 11;
    //protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    //for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
    //    //{
    //    //    e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
    //    //}

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        for (int i = 0; i < COL_NUM; i++)
    //        {
    //            if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
    //            {
    //                e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
    //            }
    //        }
    //    }
    //}
}
