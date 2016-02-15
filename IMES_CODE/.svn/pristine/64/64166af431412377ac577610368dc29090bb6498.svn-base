<%@ Page Language="C#" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Import Namespace="IMES.Query.Interface.QueryIntf" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Script" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<head id="Head1" runat="server" />

<%        
    try
    {
        string[] GvQueryColumnName = { "LESSON", "TimeRange", "D_N" ,"Station", "[A]", "[B]", "[C]", "[D]", "[E]", "[F]", "[G]", "[H]", "[J]", "[K]", "[L]", "[M]", "[R]","[TT]" };
        int[] GvQueryColumnNameWidth = { 50, 200, 50 ,150, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50,50 };
    
        string[] GvQueryDetailColumnName = { "Line", "LESSON", "Model"};
        int[] GvQueryDetailColumnNameWidth = { 50, 50, 150 };

        string[] GvPeriodQueryDetailColumnName = {  "Model" ,"Line",};
        int[] GvPeriodQueryDetailColumnNameWidth = { 150, 50 };
        
        IFA_ProductEfficiency ProductEfficiency = ServiceAgent.getInstance().GetObjectByName<IFA_ProductEfficiency>(WebConstant.IProductEfficiency);
        
        //string DBConnection = "Data Source=10.99.183.65;Initial Catalog=HPIMES;Persist Security Info=True;User ID=imes;Password=imes;Max Pool Size=100;Min Pool Size=50";
        //string DBConnection = "Data Source=10.99.169.204;Initial Catalog=HPIMES_RPT;Persist Security Info=True;User ID=imes;Password=imes;Max Pool Size=100;Min Pool Size=5;App=AP01_Query;Enlist=false";

        string DBType = Request["dbtype"].ToString();
        string DBName = Request["dbname"].ToString();
        string DBConnection = ToolUtility.GetDBConnection(DBType, DBName);

        string txtFromDate = Request["txtFromDate"] == null ? "" : Request["txtFromDate"].ToString();
        string txtPeriodFromDate = Request["txtPeriodFromDate"] == null ? "" : Request["txtPeriodFromDate"].ToString();
        string txtPeriodToDate = Request["txtPeriodToDate"] == null ? "" : Request["txtPeriodToDate"].ToString();

        string txtModel = Request["Model"] == null ? "" : Request["Model"].ToString();
        string ddlstation = Request["station"] == null ? "" : Request["station"].ToString();
        string pdline = Request["pdline"] == null ? "" : Request["pdline"].ToString();
        string family = Request["family"] == null ? "" : Request["family"].ToString();        

        IList<string> Model = new List<string>(txtModel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        List<string> Station = new List<string>(ddlstation.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        List<string> Line = new List<string>(pdline.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
              
       
        switch (Request["action"].ToString().ToUpper())
        {
            case "GET_DAY_QUERY":
                DataTable dt1 = ProductEfficiency.GetQueryResult(DBConnection, DateTime.Parse(txtFromDate), DateTime.Parse(txtFromDate).AddDays(1),
                                      family, Model, Station,Line);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<table id='gvResult' class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;width:98%;table-layout:fixed;'><tr class='iMes_grid_HeaderRowGvExt'>");
                for (int i = 0; i < dt1.Columns.Count; i++)
                {
                    sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width: {1}px'>{0}</th>", dt1.Columns[i].ColumnName, GvQueryColumnNameWidth[i].ToString());
                }
                sb.AppendLine("</tr>");
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    string classname = "";
                    #region aa

                    switch (dt1.Rows[j][0].ToString())
                    {
                        case "1":
                            classname = "lesson1";
                            break;
                        case "4":
                            classname = "lesson4";
                            break;
                        case "2":
                            classname = "lesson2";
                            break;
                        case "5":
                            classname = "lesson5";
                            break;
                        case "3":
                            classname = "lesson3";
                            break;
                        case "6":
                            classname = "lesson6";
                            break;
                        case "All":
                            classname = "lesson_sum";
                            break;
                        default:
                            break;
                    }
                    #endregion


                    sb.AppendLine(string.Format("<tr class='{0}'>", classname));
                    for (int i = 0; i < dt1.Columns.Count; i++)
                    {
                        sb.AppendFormat("<td>{0}</td>", dt1.Rows[j][i].ToString());
                    }
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</table><br />");

                DataTable dt2 = ProductEfficiency.GetQueryDetail(DBConnection, DateTime.Parse(txtFromDate), DateTime.Parse(txtFromDate).AddDays(1),
                                 family, Model, Station,Line);

                sb.AppendLine("<div style='height: 300px; overflow: auto'><table id='gvDetail' class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;width:98%;table-layout:fixed;'><tr class='iMes_grid_HeaderRowGvExt'>");
                for (int i = 0; i < dt2.Columns.Count; i++)
                {
                    if (i < GvQueryDetailColumnNameWidth.Length)
                    {
                        sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width: {1}px'>{0}</th>", dt2.Columns[i].ColumnName, GvQueryDetailColumnNameWidth[i].ToString());
                    }
                    else
                    {
                        sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style=''>{0}</th>", dt2.Columns[i].ColumnName);
                    }
                }
                sb.AppendLine("</tr>");
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    string classname = "";
                    classname = "lesson" + dt2.Rows[j][1].ToString();
                    sb.AppendLine(string.Format("<tr class='{0}'>", classname));
                    for (int i = 0; i < dt2.Columns.Count; i++)
                    {
                        sb.AppendFormat("<td>{0}</td>", dt2.Rows[j][i].ToString());
                    }
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</table></div>");

                if (Session["dt1"] == null)
                {
                    Session.Add("dt1", dt1);
                }
                else
                {
                    Session["dt1"] = dt1;
                }

                if (Session["dt2"] == null)
                {
                    Session.Add("dt2", dt2);
                }
                else
                {
                    Session["dt2"] = dt2;
                }
                Response.Write(sb.ToString());
                break;
            case "GET_PERIOD_QUERY":
                dt2 = ProductEfficiency.GetQueryPeriodDetail(DBConnection, DateTime.Parse(txtPeriodFromDate), DateTime.Parse(txtPeriodToDate),
                     family, Model, Station, Line);
                StringBuilder sb2 = new StringBuilder();

                sb2.AppendLine("<div style='height: 300px; overflow: auto'><table id='gvDetail_Period' class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;width:98%;table-layout:fixed;'><tr class='iMes_grid_HeaderRowGvExt'>");
                for (int i = 0; i < dt2.Columns.Count; i++)
                {
                    if (i < GvPeriodQueryDetailColumnNameWidth.Length)
                    {
                        sb2.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width: {1}px'>{0}</th>", dt2.Columns[i].ColumnName, GvPeriodQueryDetailColumnNameWidth[i].ToString());
                    }
                    else
                    {
                        sb2.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style=''>{0}</th>", dt2.Columns[i].ColumnName);
                    }
                }
                sb2.AppendLine("</tr>");
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    string classname = "";
                    if ((j + 1) % 2 == 0)
                    {
                        classname = "iMes_grid_RowGvExt";
                    }
                    else
                    {
                        classname = "iMes_grid_AlternatingRowGvExt";
                    }
                    
                    sb2.AppendLine(string.Format("<tr class='{0}'>", classname));
                    for (int i = 0; i < dt2.Columns.Count; i++)
                    {
                        sb2.AppendFormat("<td>{0}</td>", dt2.Rows[j][i].ToString());
                    }
                    sb2.AppendLine("</tr>");
                }
                sb2.AppendLine("</table></div>");
                
                if (Session["dt2"] == null)
                {
                    Session.Add("dt2", dt2);
                }
                else
                {
                    Session["dt2"] = dt2;
                }

                Response.Write(sb2.ToString());
                break;
            case "GET_DAY_EXCEL":
                DataTable outdt1 = new DataTable();
                DataTable outdt2 = new DataTable();
                if (Session["dt1"] != null)
                {
                    outdt1 = (DataTable)Session["dt1"];
                }
                else {
                    outdt1 = ProductEfficiency.GetQueryResult(DBConnection, DateTime.Parse(txtFromDate), DateTime.Parse(txtFromDate).AddDays(1),
                      family, Model, Station,Line);

                }
                if (Session["dt2"] != null)
                {
                    outdt2 = (DataTable)Session["dt2"];
                }
                else {
                    outdt2 = ProductEfficiency.GetQueryDetail(DBConnection, DateTime.Parse(txtFromDate), DateTime.Parse(txtFromDate).AddDays(1),
                     family, Model, Station, Line);
                }

                DataTable[] dts = new DataTable[] { outdt1, outdt2 };
                MemoryStream ms = ExcelTool.DataTableToExcel(dts, new string[] { "效率", "FIS" });
                Response.ContentType = "application/download";
                Response.AddHeader("Content-Disposition", "attachment; filename=ProductEfficency.xls");
                Response.Clear();
                Response.BinaryWrite(ms.GetBuffer());
                ms.Close();
                ms.Dispose();
                                              
                break;
            case "GET_PERIOD_EXCEL":
                DataTable outperioddt2 = new DataTable();                
                if (Session["dt2"] != null)
                {
                    outperioddt2 = (DataTable)Session["dt2"];
                }
                else
                {
                    outperioddt2 = ProductEfficiency.GetQueryPeriodDetail(DBConnection, DateTime.Parse(txtPeriodFromDate), DateTime.Parse(txtPeriodToDate),
                         family, Model, Station, Line);
                }

                DataTable[] dts2 = new DataTable[] { outperioddt2 };
                MemoryStream ms2 = ExcelTool.DataTableToExcel(dts2, new string[] { "FIS_Period" });
                Response.ContentType = "application/download";
                Response.AddHeader("Content-Disposition", "attachment; filename=ProductEfficency_Period.xls");
                Response.Clear();
                Response.BinaryWrite(ms2.GetBuffer());
                ms2.Close();
                ms2.Dispose();

                break;
            default:
                break;
        }
        

    }        

    catch (Exception ex)
    {
        Response.Write(ex.ToString());
        System.Diagnostics.Debug.WriteLine(ex.ToString());        
    }         
    
          
  

%>

