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
using UPH.Interface;
using com.inventec.iMESWEB;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
public partial class webroot_UPHMaintain_AlarmMailAddress : System.Web.UI.Page
{
    IAlarmMailAddress uph = ServiceAgent.getInstance().GetObjectByName<IAlarmMailAddress>(WebConstant.IAlarmMailAddress);

    protected void Page_Load(object sender, EventArgs e)
    {
        bindbullTable(null, 20);
        if (!IsPostBack)
        {
            
                DataTable fy = new DataTable();
                fy = uph.GetLine1();
                var q = from r in fy.AsEnumerable()
                        select new ListItem(r[0].ToString());

                string FY = fy.ToString();
                this.DL_PdLine.Items.AddRange(q.ToArray());
                bindbullTable(null, 20);
        }
        DataTable sn = new DataTable();
        sn = uph.GetAlarmALL();
        bindbullTable(sn, 5);

    }
    private void bindbullTable(DataTable dt, int defaultRow)
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

        dt.Columns.Add("Dept");
        dt.Columns.Add("Process");
        dt.Columns.Add("PdLine");
        dt.Columns.Add("MailAddress");
        dt.Columns.Add("Remark");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        return dt;

    }
    private void setColumnWidth()
    {
        GridView1.HeaderRow.Cells[0].Width = Unit.Parse("50");
        GridView1.HeaderRow.Cells[1].Width = Unit.Parse("50");
        GridView1.HeaderRow.Cells[2].Width = Unit.Parse("50");
        GridView1.HeaderRow.Cells[3].Width = Unit.Parse("200");
        GridView1.HeaderRow.Cells[4].Width = Unit.Parse("90");
        GridView1.HeaderRow.Cells[5].Width = Unit.Parse("50");
        GridView1.HeaderRow.Cells[6].Width = Unit.Parse("130");
        GridView1.HeaderRow.Cells[7].Width = Unit.Parse("130");



    }
    private void Showmessge(string msg)
    {
        Response.Write("<script>alert('" + msg + "')</script>");
        Response.Flush();
    }
    
    protected void DL_PdLine_SelectedIndexChanged(object sender, EventArgs e)
    {
        string PdLine1 = DL_PdLine.SelectedValue;
        string Process1 = DL_Process.SelectedValue;
        DataTable d = new DataTable();
        d = uph.GetLineMail(PdLine1);
        if (d.Rows.Count >0)
        {

           
            string PdLine = DL_PdLine.SelectedValue;
            this.GridView1.DataSourceID = null;
            DataTable dt = new DataTable();
            dt = uph.GetLineMail(PdLine);
            this.GridView1.DataSource = dt;
            this.GridView1.DataBind();
            TB_MailAddress.Text = "";
            TB_Remark.Text = "";
            string dept=this.GridView1.Rows[0].Cells[0].Text;
            string PS = this.GridView1.Rows[0].Cells[1].Text;
            this.DL_Dept.SelectedValue = dept;
            this.DL_Process.SelectedValue = PS;
        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('你查询的数据不存在，请维护!')", true);

        }
    }
    protected void Button_Save_Click(object sender, EventArgs e)
    {
        string PdLine = DL_PdLine.SelectedValue;
        string Process = DL_Process.SelectedValue;
        string MailAddress=TB_MailAddress.Text;
        string Editor=((MasterPageMaintain)Master).userInfo.UserId.ToString();
        string Remark=TB_Remark.Text;
        DataTable dt = new DataTable();
        dt = uph.GetMail2(PdLine, Process);
        if (dt.Rows.Count>0)
        {

            uph.UpdateMail(Editor, Remark, PdLine, MailAddress);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('数据维护成功!')", true);
            string PdLine1 = DL_PdLine.SelectedValue;
            string Process1 = DL_Process.SelectedValue;
            DataTable d = new DataTable();
            d = uph.GetMail2(PdLine, Process1);
            this.GridView1.DataSource = d;
            this.GridView1.DataBind();
            TB_MailAddress.Text="";
            TB_Remark.Text = "";


        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('请选择PdLine和Process!')", true);
        }
        

    }
    protected void Button_Insert_Click(object sender, EventArgs e)
    {
        string PdLine = DL_PdLine.SelectedValue;
        string Process = DL_Process.SelectedValue;
        string Dept = DL_Dept.SelectedValue;
        string MailAddress = TB_MailAddress.Text;
        string Editor = ((MasterPageMaintain)Master).userInfo.UserId.ToString();
        string Remark = TB_Remark.Text;
        DataTable d = new DataTable();
        d = uph.GetMail2(PdLine, Process);
        if(d.Rows.Count==0)
        {
        uph.InsertMail(Editor, Remark, PdLine, MailAddress, Process, Dept);
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('Success insert data！')", true);
        string PdLine1 = DL_PdLine.SelectedValue;
        string Process1 = DL_Process.SelectedValue;
        DataTable b = new DataTable();
        b = uph.GetMail2(PdLine, Process);
        this.GridView1.DataSource =b;
        this.GridView1.DataBind();
        TB_MailAddress.Text = "";
        TB_Remark.Text = "";

        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('数据已维护，请确认后添加！')", true);

        }
    }
    
    protected void Button_Del_Click(object sender, EventArgs e)
    {
        string PdLine = DL_PdLine.SelectedValue;
        string Process = DL_Process.SelectedValue;
        string Editor = ((MasterPageMaintain)Master).userInfo.UserId.ToString();
        DataTable d = new DataTable();
        d = uph.GetMail2(PdLine, Process);
        if (d.Rows.Count > 0)
        {
            uph.DELETEMail(Editor, PdLine, Process);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('数据已删除！')", true);
            string PdLine1 = DL_PdLine.SelectedValue;
            DataTable b = new DataTable();
            b = uph.GetLineMail(PdLine);
            this.GridView1.DataSource = b;
            this.GridView1.DataBind();
            TB_MailAddress.Text = "";
            TB_Remark.Text = "";


        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", "alert('数据不存在，请确认后！')", true);

        }
    }
}
