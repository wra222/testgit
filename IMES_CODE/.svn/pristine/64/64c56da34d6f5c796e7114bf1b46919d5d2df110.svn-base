using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Data.SqlClient;
using System.Collections;
/// <summary>
/// DAO 的摘要描述
/// </summary>
public class DAO
{
	public DAO()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}
    public static bool InjectionCheck(string str)
    {
        bool check = false;
        char[] injection = { '\'', ';' };

        if (str.IndexOfAny(injection) != -1)
        {
            check = true;
        }

        return check;
    }

    public static string ReplaceInjection(string str)
    {
        return str.Replace('\'', '"');
    }

    public static void selDropDownList(System.Web.UI.WebControls.DropDownList obj, string config, string sql)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "dropdownlist");

        obj.DataSource = ds.Tables["dropdownlist"].DefaultView;
        obj.DataValueField = ds.Tables["dropdownlist"].Columns[1].ToString();
        obj.DataTextField = ds.Tables["dropdownlist"].Columns[0].ToString();
        obj.DataBind();
    }


    public static void selDropDownList(System.Web.UI.WebControls.DropDownList obj, string config, string sql, bool blCOne)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "dropdownlist");

        obj.DataSource = ds.Tables["dropdownlist"].DefaultView;
        obj.DataValueField = ds.Tables["dropdownlist"].Columns[0].ToString();
        obj.DataTextField = ds.Tables["dropdownlist"].Columns[1].ToString();
        obj.DataBind();
    }

    public static void selDropDownList(System.Web.UI.WebControls.DropDownList obj, string config, string sql, string firstString)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "dropdownlist");

        obj.DataSource = ds.Tables["dropdownlist"].DefaultView;
        obj.DataValueField = ds.Tables["dropdownlist"].Columns[0].ToString();
        obj.DataTextField = ds.Tables["dropdownlist"].Columns[1].ToString();
        obj.DataBind();

        if (obj.Items.Count == 0)
        {
            obj.Items.Insert(0, new ListItem("-- no data found --", Constant.DefaultSelect));
        }
        else
        {
            obj.Items.Insert(0, firstString);
        }
    }


    public static void selDropDownList(System.Web.UI.WebControls.DropDownList obj, string config, string sql, string firstString, string addnew)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "dropdownlist");

        obj.DataSource = ds.Tables["dropdownlist"].DefaultView;
        obj.DataValueField = ds.Tables["dropdownlist"].Columns[0].ToString();
        obj.DataTextField = ds.Tables["dropdownlist"].Columns[1].ToString();
        obj.DataBind();

        if (obj.Items.Count == 0)
        {
            obj.Items.Insert(0, new ListItem("-- no data found --", Constant.DefaultSelect));
        }
        else
        {
            obj.Items.Insert(0, addnew);
            obj.Items.Insert(0, firstString);
        }
    }

    public static void fillDataGrid(System.Web.UI.WebControls.DataGrid dg, string config, string sql)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "datasetresult");

        dg.DataSource = ds.Tables["datasetresult"].DefaultView;
        dg.DataBind();
    }

    public static DataSet sqlCmdDataSet(string config, string sql)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);

        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "datasetresult");

        da.FillSchema(ds, SchemaType.Source, "datasetresult");

        return ds;
    }

    public static DataSet sqlCmdDataSetSP(string config, string sql)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);

        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "datasetresult");

        return ds;
    }

    public static void sqlCmd(string config, string sql)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.CommandTimeout = 1800;

        cn.Open();
        cmd.ExecuteNonQuery();
        cn.Close();
    }

    public static ArrayList SelCmdArr(string config, string sql)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);

        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "datasetresult");

        ArrayList alist = new ArrayList();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            alist.Add(dr[0].ToString());
        }
        return alist;
    }

    public static String[] sqlCmdArr(string config, string sql)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlCommand cmd = new SqlCommand(sql, cn);

        cn.Open();

        SqlDataReader dr = cmd.ExecuteReader();

        String[] arrSQL = new string[40];

        if (dr.Read())
        {
            dr.GetValues(arrSQL);
        }
        else
        {
            for (int i = 0; i < 40; i++)
            {
                arrSQL[i] = "";
            }
        }

        cn.Close();
        return arrSQL;
    }

    public static String[] sqlCmdArrSingleCol(string config, string sql)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlCommand cmd = new SqlCommand(sql, cn);

        cn.Open();

        SqlDataReader dr = cmd.ExecuteReader();

        String[] arrSQL = new string[100];

        int x = 0;

        if (dr.HasRows)
        {
            while (dr.Read())
            {
                arrSQL[x] = dr.GetString(0);
                x++;
            }
        }

        cn.Close();
        return arrSQL;
    }

    public static void addTableRowCell(System.Web.UI.WebControls.TableRow obj, string addStrings)
    {
        TableCell tc = new TableCell();

        tc.HorizontalAlign = HorizontalAlign.Center;
        tc.Text = addStrings;
        obj.Cells.Add(tc);
    }

    public void addTableRowCell(System.Web.UI.WebControls.TableRow obj, string butStrings, int addrow)
    {
        TableCell tc = new TableCell();

        tc.HorizontalAlign = HorizontalAlign.Center;
        tc.Text = "<input align='center' runat='server' type='button' id='butAction' style='font-family:arial; font-size:9px' value='" + butStrings + "' onchange='butAction(" + addrow + ")' />";

        obj.Cells.Add(tc);
    }

    public static DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
    {
        DataTable dt = new DataTable(TableName);
        dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

        string accumulatedValue = "";
        foreach (DataRow dr in SourceTable.Select("", FieldName))
        {
            if (accumulatedValue.IndexOf(dr[FieldName].ToString()) == -1)
            {
                accumulatedValue += dr[FieldName].ToString();
                dt.Rows.Add(new object[] { dr[FieldName].ToString() });
            }
        }
        return dt;
    }
}
