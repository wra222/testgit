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
using System.Data.SqlClient;
using com.inventec.system;

public partial class webroot_commonaspx_getdata : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(webroot_commonaspx_getdata));
    }

    [AjaxPro.AjaxMethod()]

    public DataTable getTable()
    {
        DataTable dt = new DataTable();
        DataColumn dcid = new DataColumn("id");
        //dcid.DataType = typeof(char); 
        DataColumn dcname = new DataColumn("name");
        DataColumn dctype = new DataColumn("type");
        DataColumn dchaschild = new DataColumn("haschild", System.Type.GetType("System.Int32"));
        //dchaschild.DataType = System.Type.GetType("System.Integer");  
        
        dt.Columns.Add(dcid);
        dt.Columns.Add(dcname);
        dt.Columns.Add(dctype);
        dt.Columns.Add(dchaschild);
        DataRow dHeader = dt.NewRow();
        dHeader[0] = "aa";
        dHeader[1] = "bb";
        dHeader[2] = "cc";
        dHeader[3] = 1;

        dt.Rows.Add(dHeader);
        DataRow dr = dt.NewRow();
        dr[0] = "111111";
        dr[1] = "Data Sources Setting";
        dr[2] = "222";
        dr[3] = 12;

        dt.Rows.Add(dr);
        //Response.Write("aaa=="+dt.Columns.Count);
        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    //Array arrTableLine = new Array();
        //    for(int j = 0; j < dt.Columns.Count; j++)
        //    {                
        //        //arrTableLine[arrTableLine.length] = dt.Rows[i][j];
        //        record += dt.Rows[i][j] + Constants.COL_DELIM;
        //    }
        //    //record += arrTableLine.join(Constants.COL_DELIM);
        //    record += Constants.ROW_DELIM;
        //}
        return dt;
    }

    [AjaxPro.AjaxMethod()]
    public DataTable getTableSecond()
    {
        DataTable dt = new DataTable();
        DataColumn dcid = new DataColumn("id");
        DataColumn dcname = new DataColumn("name");
        DataColumn dctype = new DataColumn("type");
        DataColumn dchaschild = new DataColumn("haschild");

        dt.Columns.Add(dcid);
        dt.Columns.Add(dcname);
        dt.Columns.Add(dctype);
        dt.Columns.Add(dchaschild);
        DataRow dHeader = dt.NewRow();
        dHeader[0] = "aa";
        dHeader[1] = "bb";
        dHeader[2] = "cc";
        dHeader[3] = "1";

        dt.Rows.Add(dHeader);
        DataRow dr = dt.NewRow();
        dr[0] = "XXXX";
        dr[1] = "YYY";
        dr[2] = "ZZZ";
        dr[3] = "1";

        dt.Rows.Add(dr);
        //Response.Write("aaa=="+dt.Columns.Count);
        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    //Array arrTableLine = new Array();
        //    for(int j = 0; j < dt.Columns.Count; j++)
        //    {                
        //        //arrTableLine[arrTableLine.length] = dt.Rows[i][j];
        //        record += dt.Rows[i][j] + Constants.COL_DELIM;
        //    }
        //    //record += arrTableLine.join(Constants.COL_DELIM);
        //    record += Constants.ROW_DELIM;
        //}
        return dt;
    }

}
