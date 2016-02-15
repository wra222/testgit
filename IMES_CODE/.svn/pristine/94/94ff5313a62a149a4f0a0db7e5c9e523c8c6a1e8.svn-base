<%@ Page Language="C#" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Import Namespace="IMES.Query.Interface.QueryIntf" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Script" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>


<%        
    try
    {


        //string MVS_SUM = com.inventec.iMESWEB.IMESQueryBasePage.MVS_SUM;        
        //string PAKCosmetic_SUM = com.inventec.iMESWEB.IMESQueryBasePage.PAKCosmetic_SUM;
        //string COA_SUM = com.inventec.iMESWEB.IMESQueryBasePage.COA_SUM;
        
        IFA_MPInputEx MPInputEx = ServiceAgent.getInstance().GetObjectByName<IFA_MPInputEx>(WebConstant.MPInputEx);
        int FixedColCount = 4;
        string plusColumnName = "MVS,ITCND,COA";
        bool isGetDetail = Request["isGetDetail"] == null ? false :bool.Parse(Request["isGetDetail"].ToString());
        string[] GvQueryColumnName = { "Line", "Family", "Model", "Qty","CombineDN","MVS_SUM", "PAKCosmetic_SUM" };
        int[] GvQueryColumnNameWidth = { 50, 100, 100};

        string[] GvQueryDetailColumnName = { "No", "ProductID", "CUSTSN", "PCBID", "Family", "Model", "PalletNo", "ShipDate", "Station", "Status", "Descr", "StartLine","Line", "Cdt", "Udt", "Editor" };
        int[] GvQueryDetailColumnNameWidth = { 50, 90, 100, 100, 100, 100, 80, 80, 50, 50, 200, 150, 150, 200, 200, 100 };
                                              
        
        string DBType = Request["dbtype"].ToString();
        string DBName = Request["dbname"].ToString();

        string txtShipDate = Request["txtShipDate"] == null ? "" : Request["txtShipDate"].ToString();
    //    string txtToDate = Request["txtToDate"] == null ? "" : Request["txtToDate"].ToString();
        string family = Request["family"] == null ? "" : Request["family"].ToString();
        string pdline = Request["pdline"] == null ? "" : Request["pdline"].ToString();
        string line = Request["line"] == null ? "" : Request["line"].ToString();
        string lineshift = Request["lineshift"] == null ? "" : Request["lineshift"].ToString();
        string model = Request["model"] == null ? "" : Request["model"].ToString();
        string rbProcess = Request["rbProcess"] == null ? "" : Request["rbProcess"].ToString();
        string inputstation = Request["inputstation"] == null ? "" : Request["inputstation"].ToString();
        string station = Request["station"] == null ? "" : Request["station"].ToString();
        string grpmodel = Request["grpmodel"] == null ? "false" : Request["grpmodel"].ToString();
        string modelCategory = Request["modelCategory"] == null ? "" : Request["modelCategory"].ToString();
        
        string DBConnection = ToolUtility.GetDBConnection(DBType, DBName);

        List<string> lstPdLine = new List<string>(pdline.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        List<string> lstModel = new List<string>(model.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

        string StationList = Request["stationlist"] == null ? "false" : Request["stationlist"].ToString();
        string MVS_SUM = Request["mvs_sum"] == null ? "false" : Request["mvs_sum"].ToString();
        string PAKCosmetic_SUM = Request["pakcosmetic_sum"] == null ? "false" : Request["pakcosmetic_sum"].ToString();
        string COA_SUM = Request["coa_sum"] == null ? "false" : Request["coa_sum"].ToString();
        //if (rbProcess == "rbProcess_All")
        //{
        //    StationList = plusColumnName + "," + IMESQueryBasePage.FAOnlineStation + "," + IMESQueryBasePage.PAKOnlineStation;
        //}
        //else if (rbProcess == "rbProcess_FA")
        //{
        //    StationList = IMESQueryBasePage.FAOnlineStation;
        //}
        //else if (rbProcess == "rbProcess_PAK")
        //{
        //    StationList = IMESQueryBasePage.PAKOnlineStation;
        //}

        
        switch (Request["action"].ToString().ToUpper())
        {        
            case "GET_SUMMARY":
           isGetDetail = false;

                DataTable dt = MPInputEx.GetQueryResult(DBConnection, DateTime.Parse(txtShipDate),
                        lstPdLine, family, lstModel, StationList, bool.Parse(lineshift), inputstation, bool.Parse(grpmodel), modelCategory);

                StringBuilder sb = new StringBuilder();

               sb.AppendLine("<div id='divData' style='height: 400px;width: 98%; overflow: auto'>");
               
                sb.AppendLine("<table id='gvResult' class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;width:98%;table-layout:fixed;'>");
            
                sb.AppendLine("<thead><tr class='iMes_grid_HeaderRowGvExt' >");

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i < GvQueryColumnNameWidth.Length)
                    {
                        sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width: {1}px'>{0}</th>", dt.Columns[i].ColumnName, GvQueryColumnNameWidth[i].ToString());
                    }
                    else if (i == 4 || i == 5 || i == 6)
                    {
                        if (dt.Columns[i].ColumnName == "PAKCosmetic_SUM")
                        { sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width: 140px'>{0}</th>", "PAKCosmetic"); }
                        else
                        { sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width: 140px'>{0}</th>", dt.Columns[i].ColumnName); }
                     
                    
                    }
                    else
                    {
                        sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width: 70px'>{0}</th>", dt.Columns[i].ColumnName);
                    }

                }
                sb.AppendLine("</thead></tr><tbody>");

                int[] sum = new int[dt.Columns.Count];
               
                for (int j = 0; j < dt.Rows.Count; j++)
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

                    sb.AppendLine(string.Format("<tr class='{0}' onclick='CR(this)'>", classname));

                    int MVS = 0;
                    int ITCND = 0;
                    int COA = 0;
                    string[] item = new string[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string r_Station = "";
                        r_Station = dt.Columns[i].ColumnName;
                        item[i] = dt.Rows[j][i].ToString();

                        if (item[i] != "0" && i >= FixedColCount-1)
                        {
                            if (Array.IndexOf(MVS_SUM.Split(','), r_Station) > -1)
                            {
                                MVS += int.Parse(item[i] != "" ? item[i] : "0");
                            }
                            if (Array.IndexOf(PAKCosmetic_SUM.Split(','), r_Station) > -1)
                            {
                                ITCND += int.Parse(item[i] != "" ? item[i] : "0");
                            }
                            if (Array.IndexOf(COA_SUM.Split(','), r_Station) > -1)
                            {
                                COA += int.Parse(item[i] != "" ? item[i] : "0");
                            }
                            sum[i] += int.Parse(item[i] != "" ? item[i] : "0");
                        }
                    }
                    item[5] = (-1 * MVS).ToString();
                    sum[5] += (-1 * MVS);
                    item[6] = (-1 * ITCND).ToString();
                    sum[6] += (-1 * ITCND);
                    //item[6] = (-1 * COA).ToString();
                    //sum[6] += (-1 * COA);

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                      
                        
                        
                        if (item[i] != "" && item[i] != "0" && i >= FixedColCount )
                        {
                            sb.AppendFormat("<td class='querycell' onclick='SelectDetail_Ajax();'>{0}</td>", item[i]);
                        }
                        else if (item[i] != "" && item[i] != "0" && i >= FixedColCount)
                        {
                            sb.AppendFormat("<td class='querycell nopointer'>{0}</td>", item[i]);
                        }
                        else
                        {

                            sb.AppendFormat("<td>{0}</td>", item[i]);
                        }
                    }
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</tbody>");
            
                if (dt.Rows.Count > 0)
                {
               
                   
                    sb.AppendLine("<tr class='footer'>");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                       
                            
                        
                        if (i >= FixedColCount-1)
                        {
                            if (sum[i].ToString() != "" && sum[i].ToString() != "0" && i!=3)
                            { sb.AppendFormat("<td class='querycell' onclick='SelectDetail_Ajax();'>{0}</td>", sum[i].ToString()); }
                            else
                            { sb.AppendLine(string.Format("<td>{0}</td>", sum[i].ToString())); }
                            
                        }
                        else
                        {
                            sb.AppendLine(string.Format("<td></td>", sum[i].ToString()));
                        }
                    }
                    sb.AppendLine("</tr>");
                }
             sb.AppendLine("</table></div>");
             //sb.AppendLine("</table>");
                if (Session["dt"] == null)
                {
                    Session.Add("dt", dt);
                }
                else
                {
                    Session["dt"] = dt;
                }

                
                
                
                Response.Write(sb.ToString());                  
                break;
            case "GET_DETAIL":
           isGetDetail = true;
                DataTable dt2 = MPInputEx.GetSelectDetail(DBConnection, DateTime.Parse(txtShipDate),  lstPdLine, family,
                    lstModel, line, station, bool.Parse(lineshift));
                
                StringBuilder sb2 = new StringBuilder();
                
                sb2.AppendLine("<br/><div style='height: 300px;width: 98%; overflow: auto'>");
                sb2.AppendLine("<table id='gvDetail' class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;width:98%;table-layout:fixed;'>");
                sb2.AppendLine("<thead><tr class='iMes_grid_HeaderRowGvExt'>");
                for (int i = 0; i < dt2.Columns.Count; i++)
                {
                    if (i < GvQueryDetailColumnNameWidth.Length)
                    {
                        sb2.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width: {1}px'>{0}</th>", dt2.Columns[i].ColumnName, GvQueryDetailColumnNameWidth[i].ToString());
                    }
                    else
                    {
                        sb2.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style=''>{0}</th>", dt2.Columns[i].ColumnName);
                    }
                }
                sb2.AppendLine("<thead></tr><tbody>");
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
                        sb2.AppendFormat("<td>{0}</td>", dt2.Rows[j][i].ToString().Trim());
                    }
                    sb2.AppendLine("</tr>");
                }
                sb2.AppendLine("</tbody></table></div>");
                
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
            case "GET_EXCEL":
                DataTable outdt1 = new DataTable();
                DataTable outdt2 = new DataTable();
                if (Session["dt"] == null)
                {
                    outdt1 = MPInputEx.GetQueryResult(DBConnection, DateTime.Parse(txtShipDate),  lstPdLine, family, lstModel, StationList, bool.Parse(lineshift), inputstation, bool.Parse(grpmodel), modelCategory);
                }
                else
                {
                    outdt1 = (DataTable)(Session["dt"]);
                }

                for (int j = 0; j < outdt1.Rows.Count; j++)
                {
                    int MVS = 0;
                    int ITCND = 0;
                    int COA = 0;
                    string[] item = new string[outdt1.Columns.Count];
                    for (int i = 0; i < outdt1.Columns.Count; i++)
                    {
                        string r_Station = "";
                        r_Station = outdt1.Columns[i].ColumnName;
                        item[i] = outdt1.Rows[j][i].ToString();

                        if (item[i] != "0" && i >= FixedColCount)
                        {
                            if (Array.IndexOf(MVS_SUM.Split(','), r_Station) > -1)
                            {
                                MVS += int.Parse(item[i] != "" ? item[i] : "0");
                            }
                            if (Array.IndexOf(PAKCosmetic_SUM.Split(','), r_Station) > -1)
                            {
                                ITCND += int.Parse(item[i] != "" ? item[i] : "0");
                            }
                            if (Array.IndexOf(COA_SUM.Split(','), r_Station) > -1)
                            {
                                COA += int.Parse(item[i] != "" ? item[i] : "0");
                            }
                        }
                    }
                    outdt1.Rows[j][5] = (-1 * MVS).ToString();
                    outdt1.Rows[j][6] = (-1 * ITCND).ToString();
                  //  outdt1.Rows[j][6] = (-1 * COA).ToString();
                }


                if (Session["dt2"] == null)
                {
                    outdt2 = MPInputEx.GetSelectDetail(DBConnection, DateTime.Parse(txtShipDate), lstPdLine, family, lstModel, line, station, bool.Parse(lineshift));
                }
                else
                {
                    outdt2 = (DataTable)(Session["dt2"]);
                }

                MemoryStream ms;
                DataTable[] dts ;
                if (isGetDetail)
                {
                    dts = new DataTable[] { outdt1, outdt2 };
                     ms = ExcelTool.DataTableToExcel(dts, new string[] { "Summary", "Detail" },new int[]{8});
                }
                else
                {
                    dts = new DataTable[] { outdt1  };
                     ms = ExcelTool.DataTableToExcel(dts, new string[] { "Summary" });
                }
             
               
                
                Response.ContentType = "application/download";
                Response.AddHeader("Content-Disposition", "attachment; filename=mpinput.xls");
                Response.Clear();
                Response.BinaryWrite(ms.GetBuffer());
                ms.Close();
                ms.Dispose();
                
                break;
            default:
            break;
        }
        

    
        
    }
    catch (Exception ex)
    {
        Response.StatusCode = 500;
        Response.Write(ex.ToString());
        System.Diagnostics.Debug.WriteLine(ex.ToString());
    }         
    
%>
<head id="Head1" runat="server" />