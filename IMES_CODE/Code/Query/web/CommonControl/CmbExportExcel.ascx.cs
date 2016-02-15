using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using System.IO;

public partial class CommonControl_CmbExportExcel : System.Web.UI.UserControl
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //public string gvResult = "iMESContent$gvResult";        
    private string gvc = "";
       
    public string GetGV
    {
        get
        {
            return gvc;
        }
        set
        {
            gvc = value;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          //  this.btnEpExcel.InnerText = this.GetLocalResourceObject(Pre + "_btnEpExcel").ToString();
        }
    }
    protected void btnEpExcel_Click(object sender, EventArgs e)
    {        
        GridView gv = (GridView)Page.Form.FindControl(gvc.Replace('_', '$')); //gvResult       
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=Export.xls");
        Response.Charset = ""; Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gv.AllowPaging = false;
        //gv.DataBind();
        //Change the Header Row back to white color     
        gv.HeaderRow.Style.Add("background-color", "green");
        //Apply style to Individual Cells     
        /*gv.HeaderRow.Cells[0].Style.Add("background-color", "green");
        gv.HeaderRow.Cells[1].Style.Add("background-color", "green");
        gv.HeaderRow.Cells[2].Style.Add("background-color", "green");
        gv.HeaderRow.Cells[3].Style.Add("background-color", "green");
         */

        /*for (int i = 0; i <= gv.Rows.Count-1; i++)
        {
            GridViewRow row = gv.Rows[i];
            //Change Color back to white         
            row.BackColor = System.Drawing.Color.White;
            //Apply text style to each Row         
            row.Attributes.Add("class", "textmode");
            //Apply style to Individual Cells of Alternating Row         
            if (i % 2 != 0)
            {
                row.Cells[0].Style.Add("background-color", "#C2D69B");
                row.Cells[1].Style.Add("background-color", "#C2D69B");
                row.Cells[2].Style.Add("background-color", "#C2D69B");
                row.Cells[3].Style.Add("background-color", "#C2D69B");
            }
        }*/
        gv.RenderControl(hw);
        //style to format numbers to string     
        //string style = @"&lt;style&gt; .textmode { mso-number-format:\@; } &lt;/style&gt;";
        //Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }

    public void ExportExcel(GridView gv)
    {
        
    }

    public void CheckVisible()
    {
        /*GridView gv = (GridView)Page.Form.FindControl(gvResult);
        if (gv.Rows.Count == 0)
        {
            btnEpExcel.Visible = false;
        }
        else
        {
            btnEpExcel.Visible = true;
        }*/
    }
}
