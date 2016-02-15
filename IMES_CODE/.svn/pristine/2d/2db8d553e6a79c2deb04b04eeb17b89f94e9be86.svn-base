using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using UPH.Interface;
using com.inventec.iMESWEB;
using System.Collections.Generic;

public partial class webroot_DinnerTimeMaintian_DinnerTimeMaintian : System.Web.UI.Page
{
    private IDinner DinnerTime = ServiceAgent.getInstance().GetObjectByName<IDinner>(WebConstant.DINNER);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtprocess.Items.Clear();
            List<string> process = DinnerTime.GetAllProcess();
            foreach (string PP in process)
            {
                ListItem pp = new ListItem(PP, PP);
                txtprocess.Items.Add(pp);
            }
        }
    }

    protected void txtquery_Click(object sender, EventArgs e)
    {
        DataTable dinnertime = DinnerTime.GetAllDinnerTime();
        BindTable(dinnertime);
    }

    protected void txtsave_Click(object sender, EventArgs e)
    {
        string process = txtprocess.Text;
        string Class = txtclass.Text;
        string pdline = txtpdline.Text;
        DinnerTimeInfo dinnerinfo = new DinnerTimeInfo()
        {
            Process = txtprocess.SelectedValue,
            Type = txttype.SelectedValue,
            Class = txtclass.SelectedValue,
            PdLine = txtpdline.SelectedValue,
            BeginTime = txtfromdate.Value,
            EndTime = txttodate.Value,
            Remark = "Insert! " + txtremark.Text,
            Editor = Master.userInfo.UserId,
            Cdt = DateTime.Now,
            Udt = DateTime.Now
        };

        DinnerLogInfo dinnerLoginfo = new DinnerLogInfo()
        {
            Process = txtprocess.SelectedValue,
            Type = txttype.SelectedValue,
            Class = txtclass.SelectedValue,
            PdLine = txtpdline.SelectedValue,
            BeginTime = txtfromdate.Value,
            EndTime = txttodate.Value,
            Remark = "Insert! "+txtremark.Text,
            Editor = Master.userInfo.UserId,
            Cdt = DateTime.Now,
        };

        if (txtfromdate.Value.Trim() == "" || txttodate.Value.Trim() == "")
        {
            Response.Write("<script>alert('請選擇時間段!')</script>");
        }
        else
        {
            DinnerTime.AddDinnerLogInfo(dinnerLoginfo);
            DinnerTime.AddDinnerTimeInfo(dinnerinfo);
            txtremark.Text = "";
            
        }
       DataTable dt = DinnerTime.GetAllDinnerProcessClass(process, Class, pdline);
       BindTable(dt);

    }

    protected void txtupdate_Click(object sender, EventArgs e)
    {
        string process = txtprocess.Text;
        string Class = txtclass.Text;
        string pdline = txtpdline.Text;
        foreach (GridViewRow r in dinnertime.Rows)
        {
            int id;
            if (((CheckBox)r.FindControl("CheckBox1")).Checked == true)
            {
                id = Convert.ToInt32(r.Cells[1].Text);
                string begintime = txtfromdate.Value;
                string endtime = txttodate.Value;
                string remark = "Update! "+txtremark.Text;
                string editor = Master.userInfo.UserId;

                DinnerTime.ADDDinnerLog(id,remark,editor);
                DinnerTime.UpdateDinnerTime(id,begintime,endtime,remark,editor);
                txtremark.Text = "";
            }
        }
        DataTable dt = DinnerTime.GetAllDinnerProcessClass(process, Class, pdline);
        BindTable(dt);
    }

    protected void txtdelete_Click(object sender, EventArgs e)
    {
        string process = txtprocess.Text;
        string Class = txtclass.Text;
        string pdline = txtpdline.Text;
        foreach (GridViewRow r in dinnertime.Rows)
        {
            int id;
            if (((CheckBox)r.FindControl("CheckBox1")).Checked == true)
            {
                id = Convert.ToInt32(r.Cells[1].Text);
                string remark = "Update! " + txtremark.Text;
                string editor = Master.userInfo.UserId;

                DinnerTime.ADDDinnerLog(id, remark, editor);
                DinnerTime.DelDinnerTime(id);
            }
        }
        DataTable dt = DinnerTime.GetAllDinnerProcessClass(process, Class, pdline);
        BindTable(dt);
    }

    private void BindTable(DataTable dt)
    {
        dinnertime.DataSource = dt;
        dinnertime.DataBind();
        if (dt.Rows.Count > 0)
        {
            InitGridView();
        }
        else
        {
            Response.Write("<script>alert('沒有數據!')</script>");
            Response.Flush();
        }
    }

    private void InitGridView()
    {
        dinnertime.HeaderRow.Cells[0].Width = Unit.Percentage(1);
        dinnertime.HeaderRow.Cells[1].Width = Unit.Percentage(5);
        dinnertime.HeaderRow.Cells[2].Width = Unit.Percentage(5);
        dinnertime.HeaderRow.Cells[3].Width = Unit.Percentage(5);
        dinnertime.HeaderRow.Cells[4].Width = Unit.Percentage(5);
        dinnertime.HeaderRow.Cells[5].Width = Unit.Percentage(5);
        dinnertime.HeaderRow.Cells[6].Width = Unit.Percentage(5);
        dinnertime.HeaderRow.Cells[7].Width = Unit.Percentage(5);
        dinnertime.HeaderRow.Cells[8].Width = Unit.Percentage(20);
        dinnertime.HeaderRow.Cells[9].Width = Unit.Percentage(5);
        dinnertime.HeaderRow.Cells[10].Width = Unit.Percentage(20);
        dinnertime.HeaderRow.Cells[11].Width = Unit.Percentage(20);
    }

    protected void txtprocess_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtpdline.Items.Clear();
        string process = txtprocess.SelectedValue;
        List<string> line = DinnerTime.GetAllLine(process);
        foreach (string LL in line)
        {
            ListItem ll = new ListItem(LL, LL);
            txtpdline.Items.Add(ll);
        }
        DataTable dinnertime = DinnerTime.GetAllDinnerProcess(process);
        BindTable(dinnertime);
    }

    protected void txtclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        string process = txtprocess.SelectedValue;
        string Class = txtclass.SelectedValue;
        string line = txtpdline.SelectedValue;
        DataTable dinnertime = DinnerTime.GetAllDinnerProcessClass(process, Class, line);
        BindTable(dinnertime);
    }

    protected void txtpdline_SelectedIndexChanged(object sender, EventArgs e)
    {
        string process = txtprocess.SelectedValue;
        string line = txtpdline.SelectedValue;
        DataTable dinnertime = DinnerTime.GetAllDinnerLine(process, line);
        BindTable(dinnertime);
    }

    protected void txtImport_Click(object sender, EventArgs e)
    {
        //获取文件路径   
        string filePath = this.FileUpload1.PostedFile.FileName;
        if (filePath != "")
        {
            if (filePath.Contains("xls"))//判断文件是否存在   
            {
                InputExcel(filePath);
            }
            else
            {
                Response.Write("<script>alert('請選擇Excel文件')</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('請選擇文件路徑!')</script>");
        }
    }

    private void InputExcel(string pPath)
    {
        int NUM = 0;
        ToolUtility TU = new ToolUtility();
        DataTable DT = TU.getExcelSheetData(pPath, false);
        DataTable dinnertime = DinnerTime.GetAllDinnerTime();
        foreach (DataRow DR in DT.Rows)
        {
            DinnerTimeInfo dinnerinfo = new DinnerTimeInfo()
            {
                Process = DR["Process"].ToString().Trim(),
                Type = DR["Type"].ToString().Trim(),
                Class = DR["Class"].ToString().Trim(),
                PdLine = DR["PdLine"].ToString().Trim(),
                BeginTime = DR["BeginTime"].ToString().Trim(),
                EndTime = DR["EndTime"].ToString().Trim(),
                Remark = DR["Remark"].ToString().Trim(),
                Editor = Master.userInfo.UserId,
                Cdt = DateTime.Now,
                Udt = DateTime.Now
            };

            DinnerLogInfo dinnerLoginfo = new DinnerLogInfo()
            {
                Process = DR["Process"].ToString().Trim(),
                Type = DR["Type"].ToString().Trim(),
                Class = DR["Class"].ToString().Trim(),
                PdLine = DR["PdLine"].ToString().Trim(),
                BeginTime = DR["BeginTime"].ToString().Trim(),
                EndTime = DR["EndTime"].ToString().Trim(),
                Remark = DR["Remark"].ToString().Trim(),
                Editor = Master.userInfo.UserId,
                Cdt = DateTime.Now
            };

            if (dinnerinfo.Process != "" && dinnerinfo.Type != "" && dinnerinfo.Class != "" && dinnerinfo.PdLine != "" && dinnerinfo.BeginTime != "" && dinnerinfo.EndTime != "")
            {
                DinnerTime.AddDinnerLogInfo(dinnerLoginfo);
                DinnerTime.AddDinnerTimeInfo(dinnerinfo);
            }
            else
            {
                NUM += 1;
                continue;
            }
            foreach (DataRow allDR in dinnertime.Rows)
            {
                if (allDR["Class"].ToString().Trim() == DR["Class"].ToString().Trim() && allDR["PdLine"].ToString().Trim() == DR["PdLine"].ToString().Trim())
                {
                    string id  = allDR["ID"].ToString().Trim();
                    DinnerTime.DelDinnerTime(Int32.Parse(id));
                    id = null;
                }
            }
        }
        txtremark.Text = "";
        txtquery_Click(null, null);
    }

    protected void txtExport_Click(object sender, EventArgs e)
    {
        ExcelOut(this.dinnertime);
    }

    public void ExcelOut(GridView gv)
    {
        if (gv.Rows.Count > 0)
        {
            ToolUtility tu = new ToolUtility();
            tu.ExportExcel(dinnertime, "Sheet1", this.Page);
        }
        else
        {
            Response.Write("<script>alert('沒有數據!')</script>");
        }
    }
}