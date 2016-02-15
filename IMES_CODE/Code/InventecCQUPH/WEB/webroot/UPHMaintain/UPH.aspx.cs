﻿using System;
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
using UPH.Interface;
using com.inventec.iMESWEB;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
public partial class webroot_aspx_UPH : System.Web.UI.Page
{

    IUPH_Family uph = ServiceAgent.getInstance().GetObjectByName<IUPH_Family>(WebConstant.IUPH_Family);
    protected void Page_Load(object sender, EventArgs e) //获取全部的Family
    {
        if (!this.IsPostBack)
        {
            DataTable fy = new DataTable();
            fy = uph.GetUPHFamily();
            var q = from r in fy.AsEnumerable()
                    select new ListItem(r[0].ToString());

            string FY = fy.ToString();
            this.Select3.Items.AddRange(q.ToArray());
            bindbullTable(null,20);
        }
    }

    protected void Button2_Click(object sender, EventArgs e)//添加维护数据
    {   
        UPHInfo uphinfo = new UPHInfo()
        {
            Process = Select1.SelectedValue,
            Family = Select3.SelectedValue,
            Attend_normal = int.Parse(TextBox1.Text),
            ST = TextBox6.Text,
            NormalUPH = int.Parse(TextBox2.Text),
            Cycle = TextBox3.Text,
            Remark = TextBox4.Text,
            Special = Select2.SelectedValue,
            //Editor = TextBox4.Text,
            Editor = Master.userInfo.UserId,
            Cdt = DateTime.Now,
            Udt = DateTime.Now
        };
       
        IList<UPHInfo> list = uph.GetProductUPHInfoList(uphinfo);
        if (list != null && list.Count > 0)
        {
            Response.Write("<script>alert('数据已维护不能重复操作，请确认!')</script>");
                Response.Flush();
        }
        else
        {
            uph.AddProductUPHInfo(uphinfo);
                Response.Write("<script>alert('添加完成!')</script>");
                 Response.Flush();
        }
        string process = Select1.SelectedValue;
        //GridView1 = new GridView();
        this.GridView1.DataSourceID = null;
        DataTable dt = new DataTable();
        dt = uph.GetUPHA(process);
        //this.GridView1.DataSource = dt;
        //this.GridView1.DataBind();
        bindbullTable(dt, 5);
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
        TextBox6.Text = "";

    }


    protected void Button1_Click(object sender, EventArgs e)//查询符合条件的数据
    {
        UPHInfo uphinfo = new UPHInfo()
        {
            Process = Select1.SelectedValue,
            Family = Select3.SelectedValue,
            Special = Select2.SelectedValue,
           
        };

       IList<UPHInfo> list= uph.GetProductUPHInfoList(uphinfo);
       if (list != null && list.Count > 0)
       {
           TextBox1.Text = list[0].Attend_normal.ToString();
           TextBox2.Text = list[0].NormalUPH.ToString();
           TextBox3.Text = list[0].Cycle;
           TextBox4.Text = list[0].Remark;
           TextBox6.Text = list[0].ST;
       }
       else
       {
           Response.Write("<script>alert('你查询数据为空!')</script>");
           Response.Flush();

       }
        
    }
    protected void Button4_Click(object sender, EventArgs e)//删除符合条件的数据
    { 
        UPHInfo uphinfo = new UPHInfo()
   
        {
            Process = Select1.SelectedValue,
            Family =  Select3.SelectedValue,
            Attend_normal = int.Parse(TextBox1.Text),
            ST = TextBox6.Text,
            NormalUPH = int.Parse(TextBox2.Text),
            Cycle = TextBox3.Text,
            Special = Select2.SelectedValue,
            Remark = TextBox4.Text,
            //Editor = TextBox4.Text,
            Editor = Master.userInfo.UserId,
            Cdt = DateTime.Now,

        };
        IList<UPHInfo> list = uph.GetProductUPHInfoList(uphinfo);
        if (list != null && list.Count > 0)
        {
            uph.AddUPHLog(uphinfo);
            uph.DelProductUPHInfo(uphinfo);
            Response.Write("<script>alert('删除数据成功!')</script>");
            Response.Flush();
            string process = Select1.SelectedValue;
            //GridView1 = new GridView();
            this.GridView1.DataSourceID = null;
            DataTable dt = new DataTable();
            dt = uph.GetUPHA(process);
          //  this.GridView1.DataSource = dt;
          //  this.GridView1.DataBind();
            bindbullTable(dt, 5);
        }
        else
        {
            Response.Write("<script>alert('维护的数据不存在!')</script>");
            Response.Flush();

        }
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
        TextBox6.Text = "";
    }
    protected void Button5_Click(object sender, EventArgs e)//修改数据
    {

        UPHInfo uphinfo = new UPHInfo()
        
        {
            Process = Select1.SelectedValue,
            Family = Select3.SelectedValue,
            Special = Select2.SelectedValue,
            Attend_normal = int.Parse(TextBox1.Text),
            ST = TextBox6.Text,
            NormalUPH = int.Parse(TextBox2.Text),
            Cycle = TextBox3.Text,
            Remark = TextBox4.Text,
            //Editor = TextBox3.Text,
            Editor = Master.userInfo.UserId,
            Cdt = DateTime.Now,
            Udt = DateTime.Now
        };
        IList<UPHInfo> list = uph.GetProductUPHInfoList(uphinfo);

        if (list != null && list.Count > 0)
        {
            string process = Select1.SelectedValue;
            string Family = Select3.SelectedValue;
            string Special = Select2.SelectedValue;
            string Editor = Master.userInfo.UserId;
            //string Editor = TextBox3.Text;

            uph.InsertUPH(process, Family, Special, Editor);
            uph.UpdateProductUPHInfo(uphinfo);

            Response.Write("<script>alert('修改完成!')</script>");
            Response.Flush();
            //GridView1 = new GridView();
            this.GridView1.DataSourceID = null;
            DataTable dt = new DataTable();
            dt = uph.GetUPHZ(process, Family, Special);
           // this.GridView1.DataSource = dt;
           // this.GridView1.DataBind();
            bindbullTable(dt, 5);
        }
        else
        {
            Response.Write("<script>alert('你维护的数据不存在!')</script>");
            Response.Flush();

        }
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
        TextBox6.Text = "";
        
        
     
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        
        DataTable sn = new DataTable();
        sn = uph.GetAlarmALL();
        if (sn != null)
        {
            bindbullTable(sn, 5);
        }
        else
        {
            Response.Write("<script>alert('你查询的数据为空!')</script>");
            Response.Flush();
        }

    }//查询全部数据

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count > 0)
        {
            ToolUtility tu = new ToolUtility();

            tu.ExportExcel(GridView1, "Sheet1", this.Page);
        }
        else
        {
            Response.Write("<script>alert('请查询后再进行导出!')</script>");
            Response.Flush();
        }
        //ToolUtility tu = new ToolUtility();

        //  tu.ExportExcel(GridView1, "Sheet1", this.Page);
                
    }//将数据导出Excel
    protected void paging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
    }
    private void InputExcel(string pPath)
    {
        string conn = "Provider = Microsoft.ACE.OLEDB.12.0 ; Data Source =" + pPath + ";Extended Properties='Excel 7.0;HDR=Yes;IMEX=1'";
        OleDbConnection oleCon = new OleDbConnection(conn);
        oleCon.Open();
        string Sql = "select DISTINCT * from [Sheet1$] WHERE Process<>''AND Family<>''AND Attend_normal<>''AND ST<>''AND Cycle<>''AND NormalUPH<>''AND Special<>''";
        OleDbDataAdapter mycommand = new OleDbDataAdapter(Sql, oleCon);
        DataSet ds = new DataSet();
        mycommand.Fill(ds, "[Sheet1$]");
        oleCon.Close();
        int count = ds.Tables["[Sheet1$]"].Rows.Count;
        UPHInfo alarminfo = new UPHInfo();
        for (int i = 0; i < count; i++)
        {
            if (ds.Tables["[Sheet1$]"].Rows[i]["Process"].ToString().Trim() != "" && ds.Tables["[Sheet1$]"].Rows[i]["Family"].ToString().Trim() !="")
            {
                alarminfo.Process = ds.Tables["[Sheet1$]"].Rows[i]["Process"].ToString().Trim();
                alarminfo.Family = ds.Tables["[Sheet1$]"].Rows[i]["Family"].ToString().Trim();
                alarminfo.Attend_normal = int.Parse(ds.Tables["[Sheet1$]"].Rows[i]["Attend_normal"].ToString().Trim());
                alarminfo.ST = ds.Tables["[Sheet1$]"].Rows[i]["ST"].ToString().Trim();
                alarminfo.NormalUPH = int.Parse(ds.Tables["[Sheet1$]"].Rows[i]["NormalUPH"].ToString().Trim());
                alarminfo.Cycle = ds.Tables["[Sheet1$]"].Rows[i]["Cycle"].ToString().Trim();
                alarminfo.Remark = ds.Tables["[Sheet1$]"].Rows[i]["Remark"].ToString().Trim();
                alarminfo.Special = ds.Tables["[Sheet1$]"].Rows[i]["Special"].ToString().Trim();
                alarminfo.Editor = ((MasterPageMaintain)Master).userInfo.UserName.ToString().Trim();
                alarminfo.Cdt = DateTime.Now;
                alarminfo.Udt = DateTime.Now;
                uph.AddProductUPHInfo(alarminfo);
            }
            else
            {
                break;
            }
        }
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('导入成功！'+count)", true);
    }

    private void Inutexcel2(Stream ExcelFileStream)
    {
        ToolUtility tu = new ToolUtility();
        DataTable dt= tu.getExcelSheetData( ExcelFileStream,false);
        bindbullTable(dt,0);
        foreach (DataRow dr in dt.Rows)
        {
            string process= dr["Process"].ToString().Trim();
            if (string.IsNullOrEmpty(process))
            {
                break;
            }
            UPHInfo alarminfo = new UPHInfo();
            alarminfo.Process = process;
           
            alarminfo.Family = dr["Family"].ToString().Trim();
            alarminfo.Attend_normal = int.Parse(dr["Attend_normal"].ToString().Trim());
            alarminfo.ST = dr["ST"].ToString().Trim();
            alarminfo.NormalUPH = int.Parse(dr["NormalUPH"].ToString().Trim());
            alarminfo.Cycle = dr["Cycle"].ToString().Trim();
            alarminfo.Remark = dr["Remark"].ToString().Trim();
            alarminfo.Special = dr["Special"].ToString().Trim();
            // alarminfo.Editor = ((MasterPageMaintain)Master).userInfo.UserName.ToString().Trim();
            alarminfo.Editor = Master.userInfo.UserId;
            alarminfo.Cdt = DateTime.Now;
            alarminfo.Udt = DateTime.Now;
            uph.AddProductUPHInfo(alarminfo);


        }

    }

   
    protected void Button8_Click(object sender, EventArgs e)//将Excel数据插入数据库中
    {
        
        string filePath = this.FileUpload1.PostedFile.FileName;
        string SavePath = HttpRuntime.AppDomainAppPath + "Temp\\" + filePath;

        if (filePath != "")
        {
            if (filePath.Contains("xls"))//判?文件是否存在
            {
               Inutexcel2(this.FileUpload1.FileContent);
                //InsertDBFromExcel(SavePath);
               Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('导入成功！')", true);
               uph.Del();
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请确认是否为Excel文件！')", true);
            }
        }

        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请选择文件，再进行导入！')", true);

        }

    }
    protected void Select1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string process = Select1.SelectedValue;
        //GridView1 = new GridView();
        this.GridView1.DataSourceID = null;
        DataTable dt = new DataTable();
        dt = uph.GetUPHA(process);
        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();
        Select2.SelectedValue = "";
    }
    protected void Select3_SelectedIndexChanged(object sender, EventArgs e)
    {
        string process = Select1.SelectedValue;
        string Family = Select3.SelectedValue;
        //GridView1 = new GridView();
        this.GridView1.DataSourceID = null;
        DataTable dt = new DataTable();
        dt = uph.GetUPHH(process, Family);
        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();

       
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow GridR = GridView1.SelectedRow;
        TextBox2.Text = GridR.Cells[2].Text.ToString(); 
        //string S = GridR.Cells[2].Text.ToString();
        Response.Redirect("UPH.aspx?EmployeeID=" + TextBox2.Text);
    }
    private void bindbullTable(  DataTable dt, int defaultRow)
    {
        if (dt != null)
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
           
        }
        else
        {
             dt = GetNullTable();
            DataRow dr = null;
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
           
        }
        GridView1.DataSource = dt;
        GridView1.DataBind();
        setColumnWidth();

       
    }
    public DataTable GetNullTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("ID");
        dt.Columns.Add("Process");
        dt.Columns.Add("Family");
        dt.Columns.Add("Attend_normal");
        dt.Columns.Add("ST");
        dt.Columns.Add("Cycle");
        dt.Columns.Add("NormalUPH");
        dt.Columns.Add("Special");
        dt.Columns.Add("Remark");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        return dt;

    }
    private void setColumnWidth()
    {
        GridView1.HeaderRow.Cells[0].Width = Unit.Parse("20");
        GridView1.HeaderRow.Cells[1].Width = Unit.Parse("30");
        GridView1.HeaderRow.Cells[2].Width = Unit.Parse("70");
        GridView1.HeaderRow.Cells[3].Width = Unit.Parse("70");
        GridView1.HeaderRow.Cells[4].Width = Unit.Parse("50");
        GridView1.HeaderRow.Cells[5].Width = Unit.Parse("50");
        GridView1.HeaderRow.Cells[6].Width = Unit.Parse("50");
        GridView1.HeaderRow.Cells[7].Width = Unit.Parse("50");
        GridView1.HeaderRow.Cells[8].Width = Unit.Parse("90");
        GridView1.HeaderRow.Cells[9].Width = Unit.Parse("50");
        GridView1.HeaderRow.Cells[10].Width = Unit.Parse("130");
        GridView1.HeaderRow.Cells[11].Width = Unit.Parse("130");
       

    }
    private void Showmessge(string msg)
    {
        Response.Write("<script>alert('" + msg + "')</script>");
        Response.Flush();
    }
}
